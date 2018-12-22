using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;
using System.Diagnostics;

using System.Globalization;
using System.Text.RegularExpressions;

//using System.threading;

namespace HTS_S
{
    public partial class HTS_S : Form
    {

        #region 变量

        public const Byte COM1 = 0;
        public const Byte COM2 = 1;

        public const Byte TIMES = 3;
        public const Byte DELAY_1S = 10;

        public const int TI = 40;
        Int64[,] TestItem = new Int64[TI, 8];

        struct Str
        {
            public bool InputScheme;
            public Int16 CommState;
            public int ItemNum;
            public UInt16 Step;
            public bool StepOK;
            public bool ItemRun;
            public UInt16 Time_100ms;
            public UInt16 TestErr;
            public UInt16 Start;
        }
        Str RunState = new Str();

        struct TestItemState
        {
            public UInt16[] Timeout;
            public UInt16 Time_500ms;
            public UInt16 ItemOK;
            public bool ItemResult;
        }
        TestItemState[] ItemState = new TestItemState[TI];

        private const int BUF_MAX_LENGTH = 120;               //最大数据长度
        struct SP
        {
            public bool Closing ;//正在关闭
            public bool SciLock ;
            public bool RcvEnable;
            public int Waitsleep;
            public byte[] buff ;      //接收缓冲器
            public int buffLen ;
            public byte[] rBuff ;
            public byte[] wBuff ;
            public UInt16 RcvOK;
            public Byte pHead;
            public Byte pHead2;
            public Byte pEnd;
            public Byte stat;//功能码
            public Byte res;//返回状态
        }
        SP[] SP_State = new SP[2];


        public const byte READ = 0x81;             //读
        public const byte WRITE = 0x82;            //写
        public const byte WRITE_ADDRESS = 0x83;    //写地址
        public const byte WRITE_BAUDRATE = 0x84;   //写波特率
        public const byte TRANSPARENT = 0x85;      //透传

        public const UInt16 COM1_ERR = 0x01;          //错误
        public const UInt16 COM1_ERR_T = 0x02;          //错误
        public const UInt16 COM1_ERR_T_D = 0x04;          //数据错误
        public const UInt16 COM1_Rcv = 0x0010;           //Rcv
        public const UInt16 COM1_RcvOK = 0x0020;           //OK
        public const UInt16 COM1_RcvOK_T = 0x0040;           //OK
        public const UInt16 COM2_ERR = 0x0100;          //错误
        public const UInt16 COM2_Rcv = 0x1000;           //Rcv
        public const UInt16 COM2_RcvOK = 0x2000;           //OK

        public const UInt16 DI_S = 0x20;           //数字输入状态
        public const UInt16 AI_S = 0x10;           //模拟输入状态
        public const UInt16 CURRENT_H = 0x400;           //加湿电流

        string STestOK = "通过";
        string STestERR = "不通过";
        string STestERR_D = "数据错误";

        #endregion

        #region 软件初始化
        public HTS_S()
        {
            InitializeComponent();
        }

        private void HTS_S_Load(object sender, EventArgs e)
        {
            PortDefaultPara();
            Refresh_Display(0, 0, 10);

            string str = @"Data\M1.xls";
            EcxelToDataGridView(str, 1);
        }
        #endregion

        #region 串口部分

        #region 串口初始化
        public void PortDefaultPara()
        {
            //串口1默认参数
            for (int i = 0; i < 20; i++)//最大支持到串口40，可根据自己需求增加
            {
                Port1Name.Items.Add("COM" + (i + 1).ToString());
            }
            Port1Name.SelectedIndex = 0;
            //列出常用的波特率 
            Port1Baudrate.Items.Add("2400");
            Port1Baudrate.Items.Add("4800");
            Port1Baudrate.Items.Add("9600");
            Port1Baudrate.Items.Add("19200");
            Port1Baudrate.SelectedIndex = 3;

            //串口1默认参数
            for (int i = 0; i < 20; i++)//最大支持到串口40，可根据自己需求增加
            {
                Port2Name.Items.Add("COM" + (i + 1).ToString());
            }
            Port2Name.SelectedIndex = 1;
            //列出常用的波特率 
            Port2Baudrate.Items.Add("2400");
            Port2Baudrate.Items.Add("4800");
            Port2Baudrate.Items.Add("9600");
            Port2Baudrate.Items.Add("19200");
            Port2Baudrate.SelectedIndex = 2;

            TestStart.Text = "开始测试";
        }

        public void UnlinkCtrl()
        {
            SP_State[COM1].Closing= true;
            SP_State[COM2].Closing = true;

            timer1.Enabled = false;//关闭定时器1
            timer2.Enabled = false;//关闭定时器2

            serialPort1.Close();
            RunState.CommState &= ~0x01;

            serialPort2.Close();
            RunState.CommState &= ~0x02; 

            groupBoxParam.Enabled = true;
            TestStart.Text = "开始测试";
        }

        //串口初始化
        private void Init_Comm1()
        {
            Byte Comtype = COM1;
            serialPort1.PortName = Port1Name.Text.Trim();//设置串口名
            serialPort1.BaudRate = Convert.ToInt32(Port1Baudrate.Text.Trim());//设置串口的波特率

            SP_State[Comtype].Closing = false;
            SP_State[Comtype].SciLock = false;
            SP_State[Comtype].Waitsleep = 19200 / serialPort1.BaudRate * 40;
            SP_State[Comtype].buff = new byte[BUF_MAX_LENGTH];      //接收缓冲器
            SP_State[Comtype].rBuff = null;
            SP_State[Comtype].wBuff = null;
            SP_State[Comtype].buffLen = 0;
            SP_State[Comtype].pHead = 0x68;
            SP_State[Comtype].pEnd = 0x16;
            SP_State[Comtype].stat = 0x00;
            SP_State[Comtype].res = 0x00;
        }

        private void Init_Comm2()
        {
            Byte Comtype = COM2;
            serialPort2.PortName = Port2Name.Text.Trim();//设置串口名
            serialPort2.BaudRate = Convert.ToInt32(Port2Baudrate.Text.Trim());//设置串口的波特率

            SP_State[Comtype].Closing = false;
            SP_State[Comtype].SciLock = false;
            SP_State[Comtype].Waitsleep = 9600 / serialPort2.BaudRate * 40;
            SP_State[Comtype].buff = new byte[BUF_MAX_LENGTH];      //接收缓冲器
            SP_State[Comtype].rBuff = null;
            SP_State[Comtype].wBuff = null;
            SP_State[Comtype].buffLen = 0;
            SP_State[Comtype].pHead = 0x20;
            SP_State[Comtype].pHead2 = 0x2D;
            SP_State[Comtype].pEnd = 0x0A;
            SP_State[Comtype].stat = 0x00;
            SP_State[Comtype].res = 0x00;
        }


        void LinkCtrl()
        {

            Init_Comm1();
            serialPort1.Open();
            SP_State[COM1].RcvEnable = false;
            RunState.CommState |= 0x01;//串口1打开

            Init_Comm2();
            serialPort2.Open();
            SP_State[COM2].RcvEnable = false;
            RunState.CommState |= 0x02;//串口2打开
            //            Modbus.MBConfig(serialPort1, 100, serialPort1.BaudRate);
            groupBoxParam.Enabled = false;
            TestStart.Text = "停止测试";
        }

        private void Port1Open_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    UnlinkCtrl();
                }
                else
                {
                    LinkCtrl();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                UnlinkCtrl();
            }
        }
        #endregion

        #region 串口锁定
        /// <summary>
        /// 发送指令调度锁定
        /// </summary>
        private void SciSchedulingLock(byte ComType)
        {
            SP_State[ComType].SciLock = true;
        }
        /// <summary>
        /// 发送指令调度解锁
        /// </summary>
        private void SciSchedulingUnlock(Byte ComType)
        {
            SP_State[ComType].SciLock = false;
        }
        private bool SciScheduling(Byte ComType)
        {
            return SP_State[ComType].SciLock;
        }
        #endregion

        #region 校验
        //计算校验和
        private int CheckSum(Byte[] pucFrame, int usLen)
        {
            Byte Sum =0;

            for (int i = 0; i < usLen-1; i++)
            {
                Sum+=pucFrame[i];
            }
            return (Sum ^ pucFrame[usLen-1]);
        }

        private static readonly byte[] aucCRCHi = {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40
        };
        private static readonly byte[] aucCRCLo = {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
            0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
            0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
            0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
            0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
            0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
            0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 
            0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
            0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
            0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
            0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 
            0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
            0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
            0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
            0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
            0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
            0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
            0x41, 0x81, 0x80, 0x40
        };
        /// <summary>
        /// CRC效验
        /// </summary>
        /// <param name="pucFrame">效验数据</param>
        /// <param name="usLen">数据长度</param>
        /// <returns>效验结果</returns>
        public static int Crc16(byte[] pucFrame, int usLen)
        {
            int i = 0;
            byte ucCRCHi = 0xFF;
            byte ucCRCLo = 0xFF;
            UInt16 iIndex = 0x0000;

            while (usLen-- > 0)
            {
                iIndex = (UInt16)(ucCRCLo ^ pucFrame[i++]);
                ucCRCLo = (byte)(ucCRCHi ^ aucCRCHi[iIndex]);
                ucCRCHi = aucCRCLo[iIndex];
            }
            return (ucCRCHi << 8 | ucCRCLo);
        }
        #endregion

        #region 串口发送数据
        public bool CommSend(byte ComType, byte[] Buffer, byte Offset, int Length)
        {
            if (ComType == COM1)
            {
                SP_State[ComType].RcvOK = 0x00;
                SP_State[ComType].wBuff = new byte[Length];
                Array.Copy(Buffer, SP_State[ComType].wBuff, Length);

                serialPort1.Write(Buffer, Offset, Length);
                //PrintFrame(0, Type, Buffer, Length);
                //SendNum1++;//发送
                return true;
            }
            else if (ComType == COM2)
            {
                serialPort2.Write(Buffer, Offset, Length);
                serialPort2.DiscardInBuffer();//清接收缓存
                //PrintFrame(0, Type, Buffer, Length);
                //SendNum1++;//发送
                return true;
            }
            return false;
        }
        #endregion

        #region 串口1接收数据


        public bool Compare_Result(UInt16 i16Step, long Tmp_Mdata)
        {
            //误差
            long Dif = System.Math.Abs(TestItem[i16Step, 1]);
            long Deviation = Dif * TestItem[i16Step, 2] / 100;
            if (System.Math.Abs(Tmp_Mdata - TestItem[i16Step, 1]) <= Deviation)
            {
                return true;
            }
            return false;
        }

        public bool ReadMultimeter(UInt16 i16Step, byte[] data)
        {
            bool res = false;
            int ASC = 0x30;
            long Mdata = new int();
            long Tmp_Mdata = new int();
            int Ldata = new Int16();
            int Hdata = 3;
            int PointAddr = 0;//小数点位置

            string data_str = System.Text.Encoding.ASCII.GetString(data).Substring(2);

              data_str = Regex.Replace(data_str, @"[^\d.\d]", "");


          //  Tmp_Mdata = Convert.to(data_str);
          



            //switch (data[1])
            //{
            //    case 0x30:
            //        PointAddr = 3;
            //        Ldata = PointAddr+2;
            //        break;
            //    case 0x31:
            //        PointAddr = 4;
            //        Ldata = PointAddr + 2;
            //        break;
            //    case 0x32:
            //        PointAddr = 5;
            //        Ldata = PointAddr + 2;
            //        break;
            //    case 0x33:
            //        PointAddr = 6;
            //        Ldata = PointAddr + 2;
            //        break;
            //    case 0x34:
            //        PointAddr = 7;
            //        Ldata = PointAddr + 1;
            //        break;
            //    default:
            //        Ldata = 8;
            //        break;
            //}
             
            //for (int i = Hdata; i <= Ldata; i++)//取有效数据3-8位，扩大100倍
            //{
            //    if (data[i] == 0x2E)//小数点
            //    {
            //        //Mdata *= 10;
            //    }
            //    else
            //    {
            //        Mdata += data[i] - ASC;
            //        if (i != Ldata)
            //        {
            //            Mdata *= 10;
            //        }
            //    }
            //}
            //if (data[2] == 0x2D)
            //{
            //    Tmp_Mdata = -Mdata;
            //}
            //else
            //{
            //    Tmp_Mdata = Mdata; 
            //}
            TestItem[i16Step, 3] = Tmp_Mdata;
            if (Mdata >= 100)//扩大100倍
            {
                res = Compare_Result(i16Step, Tmp_Mdata);
                ////误差
                //long Dif = System.Math.Abs(TestItem[i16Step, 1]);
                //long Deviation = Dif * TestItem[i16Step, 2] / 100;
                //if (System.Math.Abs(Tmp_Mdata - TestItem[i16Step, 1]) <= Deviation)
                //{
                //    res = true;
                //}
            }

            return res;
        }

        private bool ReceiveTtansparent(UInt16 i16Step, byte[] buff)//解析透传命令
        {
            bool res = true;

            if (buff[7] > 0x80)//Modbus返回错误
            {
                res = false;
            }
            else
            {
                if (buff[7] == 0x03)//读命令
                {
                    if (TestItem[i16Step, 5] == DI_S)//数字输入
                    {
                        int pDI = buff[9] << 8;
                        pDI |= buff[10];
                        if (pDI != 0x2C0)
                        {
                            res = false;
                        }
                    }
                    else if (TestItem[i16Step, 5] == AI_S)//模拟输入
                    {
                        int[] TmpBuffer = new int[10] { 0x50, 0x50, 0x50, 0x50, 0x50, 0x50, 0x50, 0x50, 0x50, 0x50 };
                        int TmpDIF = 0x10;
                        for (int i = 0; i < 10; i++)
                        {
                            int pAI = buff[9 + i] << 8;
                            pAI |= buff[10 + i];
                            //if (System.Math.Abs(pAI - TmpBuffer[i]) < TmpDIF)
                            //{
                            //    res = false;
                            //    break;
                            //}
                        }
                    }
                    else if (TestItem[i16Step, 5] == CURRENT_H)//加湿电流
                    {

                        int pCurrent = buff[9] << 8;
                        pCurrent |= buff[10];
                        pCurrent *= 100;
                        TestItem[i16Step, 3] = pCurrent;
                        res = Compare_Result(i16Step, pCurrent);
                        if (res == false)
                        {
                            ItemState[i16Step].ItemOK |= COM1_ERR_T_D;
                        }
                    }

                } 
            }

            if (res == true)
            {
                ItemState[i16Step].ItemOK |= COM1_RcvOK_T; 
            }
            return res;
        }

        private bool ReceiveDataProcess(byte ComType,UInt16 i16Step, byte[] buff)
        {
            if (buff == null)
                return false;
            if (buff.Length < 5)    //回传的数据 地址+功能码+长度+2效验 = 5字节
                return false;
            bool res = true;
            if (ComType == COM1)
            {
                switch (buff[2])//功能码
                {
                    case READ: ; 
                        break;//读数据
                    case WRITE: ; 
                        break;
                    case WRITE_ADDRESS: ; 
                        break;
                    case WRITE_BAUDRATE: ; 
                        break;
                    case TRANSPARENT: 
                        res =ReceiveTtansparent(i16Step,buff); 
                        break;
                    default: 
                        res = false; 
                        break;
                }
            }
            else if (ComType == COM2)
            {
                switch (buff[0])//功能码
                {
                    case 0x30:  //直流
                    case 0x31:  //交流
                        res = ReadMultimeter(i16Step,buff);
                        break;//交流
                    default: res = false; break;
                }
            }
            else
            {
                res = false;
            }
            return res;
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //Modbus.MBDataReceive();
            Byte Comtype = COM1;

            if (SP_State[Comtype].Closing)
            {
                return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环
            }
            try
            {
                if (RunState.Step <= 0)
                {
                    return;
                }
                SciSchedulingLock(Comtype);
                System.Threading.Thread.Sleep(SP_State[Comtype].Waitsleep);      //等待缓冲器满                this.Invoke((EventHandler)(delegate
                SP_State[Comtype].buffLen = serialPort1.BytesToRead;          //获取缓冲区字节长度
                if (SP_State[Comtype].buffLen > BUF_MAX_LENGTH)            //如果长度超出范围 直接退出
                {
                    SciSchedulingUnlock(Comtype);
                    return;
                }
                serialPort1.Read(SP_State[Comtype].buff, 0, SP_State[Comtype].buffLen);            //读取数据
                if ((SP_State[Comtype].buff[0] == SP_State[Comtype].pHead) && (SP_State[Comtype].buff[SP_State[Comtype].buffLen - 1] == SP_State[Comtype].pEnd))
                {
                    if (CheckSum(SP_State[Comtype].buff, SP_State[Comtype].buffLen-1) == 0)//接收数据正确
                    {
                        SP_State[Comtype].RcvOK |= COM1_Rcv;
                        //SP_State[Comtype].rBuff = new byte[BUF_MAX_LENGTH];
                        //Array.Copy(SP_State[Comtype].buff, SP_State[Comtype].rBuff, SP_State[Comtype].buffLen);
                        UInt16 TmpStep = (UInt16)(RunState.Step - 1);
                        if (ReceiveDataProcess(Comtype, TmpStep, SP_State[Comtype].buff) == true)
                        {
                            SP_State[Comtype].RcvOK |= COM1_RcvOK;  //标记 所接收到的数据正确
                            ItemState[TmpStep].ItemOK |= COM1_RcvOK;
                        }
                    } 
                }
            }
            finally
            {
                SciSchedulingUnlock(Comtype);//我用完了，ui可以关闭串口了。
            }
            return;

        }
        #endregion

        #region 串口2接收数据
        //串口2接收数据
        private void serialPort2_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Byte Comtype = COM2;
            Byte Length = 21;

            if (SP_State[Comtype].Closing)
            {
                return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环
            }
            try
            {
                if (SP_State[Comtype].RcvEnable==false)
                {
                    serialPort2.DiscardInBuffer();
                    return;                   
                }
                SciSchedulingLock(Comtype);
                System.Threading.Thread.Sleep(SP_State[Comtype].Waitsleep);      //等待缓冲器满                this.Invoke((EventHandler)(delegate
                SP_State[Comtype].buffLen = serialPort2.BytesToRead;          //获取缓冲区字节长度
                //if (SP_State[Comtype].buffLen > BUF_MAX_LENGTH)            //如果长度超出范围 直接退出
                //{
                 
                //    SciSchedulingUnlock(Comtype);
                //    return;
                //}
                if (SP_State[Comtype].buffLen != Length)            //如果长度超出范围 直接退出
                {
                    SciSchedulingUnlock(Comtype);
                    serialPort2.DiscardInBuffer();
                    return;
                }
                else
                {
                    SP_State[Comtype].RcvEnable = false;//禁止接收
                }
                serialPort2.Read(SP_State[Comtype].buff, 0, SP_State[Comtype].buffLen);            //读取数据
                if (((SP_State[Comtype].buff[2] == SP_State[Comtype].pHead) || (SP_State[Comtype].buff[2] == SP_State[Comtype].pHead2)) && (SP_State[Comtype].buff[SP_State[Comtype].buffLen - 1] == SP_State[Comtype].pEnd))
                {
                    if (SP_State[Comtype].buff[SP_State[Comtype].buffLen - 2] == 0x0D)//接收数据正确
                    {
                        SP_State[Comtype].RcvOK |= COM2_Rcv;
                        //SP_State[Comtype].rBuff = new byte[BUF_MAX_LENGTH];
                        //Array.Copy(SP_State[Comtype].buff, SP_State[Comtype].rBuff, SP_State[Comtype].buffLen);
                        UInt16 TmpStep = (UInt16)(RunState.Step - 1);
                        if (ReceiveDataProcess(Comtype,TmpStep, SP_State[Comtype].buff) == true)
                        {
                            SP_State[Comtype].RcvOK |= COM2_RcvOK;  //标记 所接收到的数据正确
                            ItemState[TmpStep].ItemOK |= COM2_RcvOK;
                        }
                    }
                }
            }
            finally
            {
                SciSchedulingUnlock(Comtype);//我用完了，ui可以关闭串口了。
            }
            return;
        }
        #endregion

        #endregion

        #region 定时器

        bool Test_ItemRcv()
        {
            UInt16 TmpStep = 0x00;
            if (RunState.Step > 0)
            {
                TmpStep = (UInt16)(RunState.Step - 1);
            }
            switch (TestItem[TmpStep, 5])
            {
                case 0x01://交流电源测试
                case 0x02:
                case 0x04:
                case 0x08:
                case 0x1000:
                    if ((((ItemState[TmpStep].ItemOK & COM1_RcvOK) == COM1_RcvOK)//COM1接收正确
                        && ((ItemState[TmpStep].ItemOK & COM2_RcvOK) == COM2_RcvOK))//COM2接收正确
                        || ((ItemState[TmpStep].ItemOK & COM2_ERR) == COM2_ERR))//COM2接收错误
                    {
                        return true;
                    }
                    break;
                //case 0x10:
                //    break;
                //case 0x20:
                //    break;
                case 0x40:
                    break;
                case 0x80:
                    break;
                case 0x10:
                case 0x20:
                case 0x100://HMI通信
                case 0x200://监控通信
                case CURRENT_H://加湿板通信
                    if ((((ItemState[TmpStep].ItemOK & COM1_RcvOK) == COM1_RcvOK)//COM1接收正确
                        && ((ItemState[TmpStep].ItemOK & COM1_RcvOK_T) == COM1_RcvOK_T))//COM1接收正确 
                        || ((ItemState[TmpStep].ItemOK & COM1_ERR_T) == COM1_ERR_T))//COM1接收错误
                    {
                        return true;
                    }
                    break;
                case 0x800:
                    break;
                default:
                    break;
            }
            return false;
        }
        //定时器2  100ms
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;//关闭定时器

            RunState.Time_100ms++;
            if (RunState.Time_100ms >= 2)
            {
                RunState.Time_100ms = 0;
                if (RunState.ItemRun)
                {
                    UInt16 TmpStep = 0x00;
                    if (RunState.Step > 0)
                    {
                        TmpStep = (UInt16)(RunState.Step - 1);
                    }
                    if (Test_ItemRcv())//接收正确
                    {
                        float TestData = (float)TestItem[TmpStep, 3] / 100;
                        string SResult;
                        if (((ItemState[TmpStep].ItemOK & COM2_ERR) == COM2_ERR)//COM2接收错误
                            ||((ItemState[TmpStep].ItemOK & COM1_ERR_T) == COM1_ERR_T))
                        {
                            if ((ItemState[TmpStep].ItemOK & COM1_ERR_T_D) == COM1_ERR_T_D)
                            {
                                SResult = STestERR_D;
                            }
                            else
                            {
                                SResult = STestERR;
                            }
                            ItemState[TmpStep].ItemResult = false;
                        }
                        else
                        {
                            SResult = STestOK;
                            ItemState[TmpStep].ItemResult = true;
                        }
                        dataGridView1.Rows[TmpStep].Cells[4].Value = TestData.ToString();
                        dataGridView1.Rows[TmpStep].Cells[5].Value = SResult;
                        RunState.StepOK = true;
                    }
                }
            }
            timer2.Enabled = true;//打开定时器
            return;

        }

        //public static void AppendTextColorful(this RichTextBox rtBox, string text, Color color, bool addNewLine = true)
        //{
        //    if (addNewLine)
        //    {
        //        text += Environment.NewLine;
        //    }
        //    rtBox.SelectionStart = rtBox.TextLength;
        //    rtBox.SelectionLength = 0;
        //    rtBox.SelectionColor = color;
        //    rtBox.AppendText(text);
        //    rtBox.SelectionColor = rtBox.ForeColor;
        //}

        public void RTB_Delete(RichTextBox rtb, UInt16 Delete_Line)
        {
            //string[] sLines = rtb.Lines;
            //string[] sNewLines = new string[sLines.Length - 1];
            //Array.Copy(sLines, 1, sNewLines, 0, sNewLines.Length);
            //rtb.Lines = sNewLines;
            int Start = rtb.GetFirstCharIndexFromLine(Delete_Line - 1);
            int End = rtb.GetFirstCharIndexFromLine(Delete_Line);
            rtb.Select(Start, End);
            rtb.SelectedText = "";
        }
        public void RTB_Disply(RichTextBox rtb, string strInput, Color fontColor,Byte FontSize,Byte AddrType)
        {
            int p1 = rtb.TextLength;  //取出未添加时的字符串长度。  
            if (AddrType == 2)
            {
                rtb.AppendText(strInput);  //保留每行的所有颜色。 //  rtb.Text += strInput + "/n";  //添加时，仅当前行有颜色。   
                int p2 = strInput.Length;  //取出要添加的文本的长度   
                rtb.Select(p1, p2);        //选中要添加的文本   
                rtb.SelectionColor = fontColor;  //设置要添加的文本的字体色   
                rtb.SelectionFont = new Font("Arial", FontSize);  //设置要添加的文本的字大小  
            }
            else
            {
                rtb.AppendText(strInput + Environment.NewLine);  //保留每行的所有颜色。 //  rtb.Text += strInput + "/n";  //添加时，仅当前行有颜色。   
                int p2 = strInput.Length;  //取出要添加的文本的长度   
                rtb.Select(p1, p2);        //选中要添加的文本   
                rtb.SelectionColor = fontColor;  //设置要添加的文本的字体色   
                rtb.SelectionFont = new Font("楷体", FontSize);  //设置要添加的文本的字大小              
            } 
            if (AddrType==1)
            {
                rtb.SelectionAlignment = HorizontalAlignment.Center;
            }
            //设置滚动条到底部
            //rtb.Focus();
            //rtb.Select(rtb.TextLength,0);
            //rtb.ScrollToCaret(); 
            rtb.SelectionStart = rtb.TextLength;
            rtb.Focus();
        }

        //刷新提示信息及滚动条
        private bool Refresh_Display(UInt16 i16Step, int TotalNum, Byte TestProcess)
        {
            //刷新提示信息
            if (TestProcess == 1)
            {
                //先清空相关显示信息
                Capion.Clear();
                for (int i = 0; i < RunState.ItemNum; i++)
                {
                    ItemState[i].Timeout = new UInt16[3];
                    ItemState[i].Time_500ms = 0x00;
                    ItemState[i].ItemOK = 0x00;
                    ItemState[i].ItemResult = false;

                    dataGridView1.Rows[i].Cells[4].Value = null;
                    dataGridView1.Rows[i].Cells[5].Value = null;
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }

                String Pstr = "测试信息";
                RTB_Disply(Capion, Pstr, Color.Green, 25, 1);

                return true;
            }
            else if (TestProcess == 2)
            {

                UInt16 TmpStep = 0x00;
                if (i16Step > 0)
                {
                    TmpStep = (UInt16)(i16Step - 1);
                }
                if (TmpStep > 0)
                {
                    if (ItemState[TmpStep-1].ItemResult)
                    {
                        //RTB_Delete(Capion, TmpStep);  
                        string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        String Pstr1 = " 第";
                        String Pstr2 = TmpStep.ToString();
                        String Pstr3 = "项测试通过";
                        RTB_Disply(Capion, sTime, Color.Black, 8, 2);
                        RTB_Disply(Capion, Pstr1 + Pstr2 + Pstr3, Color.Green, 14, 0);

                        dataGridView1.Rows[TmpStep-1].DefaultCellStyle.BackColor = Color.GreenYellow;
                    }
                    else
                    {
                        RunState.TestErr++;//不通过项累加
                        //RTB_Delete(Capion, TmpStep);  
                        string sTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        String Pstr1 = " 第";
                        String Pstr2 = TmpStep.ToString();
                        String Pstr3 = "项测试不通过";
                        RTB_Disply(Capion, sTime, Color.Black, 8, 2);
                        RTB_Disply(Capion, Pstr1 + Pstr2 + Pstr3, Color.Red, 14, 0);

                        dataGridView1.Rows[TmpStep-1].DefaultCellStyle.BackColor = Color.Red; 
                    }
                }

                if (i16Step > TotalNum)//总测试项，测试完成
                {
                    //停止测试
                    Test_Stop();

                    String Pstr1 = "";
                    RTB_Disply(Capion, Pstr1, Color.Green, 14, 0);

                    Pstr1 = "测试结果:";
                    String Pstr2 = RunState.TestErr.ToString();
                    String Pstr3 = "项测试不通过";
                    RTB_Disply(Capion, Pstr1 + Pstr2 + Pstr3, Color.Red, 14, 0);


                    Title.Clear();
                    if (RunState.TestErr > 0)
                    {
                        String Pstr11 = "测试完成：共";
                        String Pstr21 = RunState.TestErr.ToString();
                        String Pstr31 = "项测试不通过，需要维修！";
                        RTB_Disply(Title, Pstr11 + Pstr21 + Pstr31, Color.Red, 18, 0);
                    }
                    else
                    {
                        String Pstr11 = "测试通过！";
                        RTB_Disply(Title, Pstr11, Color.Green, 20, 0);
                    }

                    return false;
                }
                string sTime1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                String str1 = " 测试第";
                String str2 = i16Step.ToString();
                String str3 = "项";
                RTB_Disply(Capion, sTime1, Color.Black, 8, 2);
                RTB_Disply(Capion, str1 + str2 + str3, Color.LightSeaGreen, 14, 0);
                //dataGridView1当前测试项为浅蓝色
                dataGridView1.Rows[i16Step - 1].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else if (TestProcess == 10)
            {
                String Pstr1 = "请加载测试方案，配置串口后，开始测试！";
                RTB_Disply(Title, Pstr1, Color.Green, 18, 0);
                return true;
            }
            else if (TestProcess == 11)
            {
                Title.Clear();
                String Pstr1 = "测试中......";
                RTB_Disply(Title, Pstr1, Color.Green, 18, 0);
                return true;
            }
            //Capion.AppendTextColorful("提示信息", Color.Green);

            //dataGridView1滚动条刷新
            int index = 0x00;
            if (i16Step < 2)
            {
                index = i16Step - 1;
            }
            else
            {
                index = i16Step - 2;
            }
            dataGridView1.FirstDisplayedScrollingRowIndex = index;

            return true;
        }
        //定时器1  500ms
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (RunState.ItemRun)
            {
                if (RunState.StepOK)//该项测试完成
                {
                    RunState.StepOK = false;
                    if (RunState.Step <= RunState.ItemNum)
                    {
                        RunState.Step++;
                    }
                    else
                    {
                        RunState.ItemRun = false;
                        return;
                    }
                    if (Refresh_Display(RunState.Step,RunState.ItemNum, 2)==false)
                    {
                        timer1.Enabled = false;
                        return;
                    }
                }
                UInt16 TmpStep = (UInt16)(RunState.Step - 1);
                TestRun_Control(TmpStep);//测试控制处理
            }
            return;
        }
        #endregion

 
        //延时毫秒
        public static void Delay(int ms)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < ms)
            {
                Application.DoEvents();
            }
        }

        //切换继电器
        private bool Command_Write(byte ComType, Byte[] pDI, Byte[] pBuff, Byte pLength)//写数据
        {
 

            Byte[] Buffer = new Byte[20];
            Byte CheckSum = 0;

            Byte pHead = 0x68;
            Byte pEnd = 0x16;
            Byte pAddress = 0x01;
            Byte pCommand = pDI[1];

            SciSchedulingLock(ComType);

            Buffer[0] = pHead;
            Buffer[1] = pAddress;
            Buffer[2] = pCommand;
            Buffer[3] = pLength;
            Buffer[4] = pDI[1];
            Buffer[5] = pDI[0];
            Array.Copy(pBuff, 0, Buffer, 6, pLength-2);//复制数据
            //Array.Reverse(Buffer, 6, 6);//数据反向
            for (int i = 0; i < (Buffer[3] + 4); i++)
            {
                CheckSum += Buffer[i];
            }
            Buffer[Buffer[3] + 4] = CheckSum;
            Buffer[Buffer[3] + 5] = pEnd;

            //发送数据
            if (CommSend(ComType, Buffer, 0, Buffer[3] + 6))
            {
                SciSchedulingUnlock(ComType);
                return true;
            }

            return false;
        }

        //继电器切换
        private bool Command_Switch(UInt16 i16Step)//
        {
             //切换继电器
            Byte[] pBuff1 = BitConverter.GetBytes(TestItem[i16Step, 6]);

            if (((ItemState[i16Step].ItemOK & COM1_RcvOK) == 0) && ((ItemState[i16Step].ItemOK & COM1_ERR) == 0))//未测试通过
            {
                if (SciScheduling(COM1))//是否锁住
                {
                    return false;
                }
                ItemState[i16Step].Timeout = new UInt16[3];
                if (ItemState[i16Step].Timeout[0] <= TIMES)//重试次数
                {
                    if (ItemState[i16Step].Timeout[0] != 0)
                    {
                        ItemState[i16Step].Time_500ms++;
                        if (ItemState[i16Step].Time_500ms <= (TestItem[i16Step, 7] / 500))//延时等待
                        {
                            //ItemState[i16Step].Time_500ms = 0;
                            return false;
                        }
                    }
                    Byte[] pDI= new Byte[2]{0x00,0x02};//数据标识
                    if (Command_Write(COM1,pDI, pBuff1,2+6))
                    {
                        ItemState[i16Step].Time_500ms = 0x00;
                        ItemState[i16Step].Timeout[0]++;
                    }
                    return false;
                }
                else//通信异常
                {
                    ItemState[i16Step].Time_500ms = 0;
                    ItemState[i16Step].ItemOK |= COM1_ERR;
                    return true;
                }
            }
            return false;
        }

        #region 电压测试
        //读取电压
        private bool Command_ReadVoltage(byte ComType, Byte[] pBuff)//读电压
        {
            Byte TmpType = pBuff[0];
            Byte TmpCMD = new Byte();
            Byte[] Buffer = new Byte[5];

            //SciSchedulingLock(ComType);
            switch (TmpType)
            {
                case 0x01:
                case 0x08:
                    TmpCMD = 0x41;//A,直流
                    break;
                case 0x02:
                case 0x04:
                    TmpCMD = 0x42;//B,交流
                    break;
                default:
                    break;
            }

            Buffer[0] = TmpCMD;
            //发送数据2编
            if (CommSend(ComType, Buffer, 0, 1))
            {
                //延时
                Delay(50);
                if (CommSend(ComType, Buffer, 0, 1))
                {
                    //SciSchedulingUnlock(ComType);
                    return true;
                }
            }

            return false;
        }

        private bool Command_Voltage(UInt16 i16Step)//
        {
            //读电压
            Byte[] pBuff2 = BitConverter.GetBytes(TestItem[i16Step, 5]);

            if (((ItemState[i16Step].ItemOK & COM2_RcvOK) == 0) && ((ItemState[i16Step].ItemOK & COM2_ERR) == 0))//未测试通过
            {
                if (SciScheduling(COM2))//是否锁住
                {
                    return false;
                }
                if (ItemState[i16Step].Timeout[1] <= TIMES)//重试次数
                {
                    if (ItemState[i16Step].Timeout[1] != 0)
                    {
                        ItemState[i16Step].Time_500ms++;
                        if (ItemState[i16Step].Time_500ms <= (TestItem[i16Step, 7] / 250))//延时等待
                        {
                            //ItemState[i16Step].Time_500ms = 0;
                            return false;
                        }
                    }

                    if (i16Step > 0x00)
                    {
                        if (TestItem[i16Step, 5] == TestItem[i16Step - 1, 5])//连续两次读电压命令相同
                        {
                            ItemState[i16Step].Timeout[1]++;
                            serialPort2.DiscardInBuffer();//清接收缓存
                            SP_State[COM2].RcvEnable = true;//允许接收
                            return false;
                        }
                    }
                    if (Command_ReadVoltage(COM2, pBuff2))
                    {
                        ItemState[i16Step].Time_500ms = 0x00;
                        ItemState[i16Step].Timeout[1]++;
                        serialPort2.DiscardInBuffer();//清接收缓存
                        SP_State[COM2].RcvEnable = true;//允许接收

                        Delay(100);//延时

                    }
                    return false;
                }
                else//通信异常
                {
                    ItemState[i16Step].Time_500ms = 0;
                    ItemState[i16Step].ItemOK |= COM2_ERR;
                    return true;
                }
            }

            return false;
        }

        //电压测试
        void Test_VAC_VDC(UInt16 i16Step)
        {

            Command_Switch(i16Step);//切换继电器

            Command_Voltage(i16Step);//读电压
            return;
 
        }
        #endregion

        bool Command_Transport_T(UInt16 i16Step, Int64 T_Type)//透传命令
        {
            Byte[] pBuff1 = new Byte[20];
            Byte[] pDI = new Byte[2] { 0x00, 0x05 };//数据标识
            Byte pAddress = 0x01;
            Byte pCommand = 0x03;
            int pOffset = 0x100;
            int pAddr = 0x00;
            int pData = 0x00;
            int pCrcValue = 0x00;

            switch (T_Type)
            {
                case 0x20://DI测试
                    pCommand = 0x03;
                    pOffset = 0x300;
                    pAddr = 117 + pOffset;
                    pData = 0x01;
                    break;
                case 0x10://AI测试
                    pCommand = 0x03;
                    pOffset = 0x300;
                    pAddr = 101 + pOffset;
                    pData = 0x0A;
                    break;
                case 0x100://HMI通信
                    pCommand = 0x06;
                    pOffset = 0x100;
                    pAddr = 0x06 + pOffset;
                    pData = 0x01;
                    break;
                case 0x200://监控通信
                    pCommand = 0x03;
                    pOffset = 0x00;
                    pAddr = 0x04 + pOffset;//回风温度
                    pData = 0x01;
                    break;
                case CURRENT_H://加湿板通信
                    pAddress = 0x09;
                    pCommand = 0x03;
                    pOffset = 0x00;
                    pAddr = 0x16 + pOffset;//电流、水位
                    pData = 0x02;
                    break;
                default:
                    break;
            }
            //Modbus协议
            pBuff1[0] = pAddress;
            pBuff1[1] = pCommand;
            pBuff1[2] = (Byte)(pAddr / 256);
            pBuff1[3] = (Byte)(pAddr % 256);
            pBuff1[4] = (Byte)(pData / 256);
            pBuff1[5] = (Byte)(pData % 256);
            //CRC校验
            pCrcValue = Crc16(pBuff1, 6);

            pBuff1[6] = (Byte)(pCrcValue & 0xFF);
            pBuff1[7] = (Byte)(pCrcValue >> 8);

            if (Command_Write(COM1, pDI, pBuff1, 2 + 8))
            {
            }
            return true;

        }

        bool Command_Transport(UInt16 i16Step, Int64 T_Type)//透传命令
        {
            if (((ItemState[i16Step].ItemOK & COM1_RcvOK_T) == 0) && ((ItemState[i16Step].ItemOK & COM1_ERR_T) == 0))//未测试通过
            {
                if (SciScheduling(COM1))//是否锁住
                {
                    return false;
                }
                if (ItemState[i16Step].Timeout[1] <= TIMES)//重试次数
                {
                    if (ItemState[i16Step].Timeout[1] != 0)
                    {
                        ItemState[i16Step].Time_500ms++;
                        if (ItemState[i16Step].Time_500ms <= (TestItem[i16Step, 7] / 100))//延时等待
                        {
                            //ItemState[i16Step].Time_500ms = 0;
                            return false;
                        }
                    }
                    if (Command_Transport_T(i16Step,T_Type))
                    {
                        ItemState[i16Step].Time_500ms = 0;
                        ItemState[i16Step].Timeout[1]++;
                    }
                    return false;
                }
                else//通信异常
                {
                    ItemState[i16Step].Time_500ms = 0;
                    ItemState[i16Step].ItemOK |= COM1_ERR_T;
                    return true;
                }
            }
            return false;
        }

        #region 通信测试
        //HMI通信测试
        void Test_Communiction(UInt16 i16Step, Int64 T_Type)
        {
            Command_Switch(i16Step);//切换继电器

            Command_Transport(i16Step, T_Type);//透传命令

        }
        #endregion

        #region DI测试
        //DI测试
        void Test_DI_AI(UInt16 i16Step, Int64 T_Type)
        {

            //Command_Switch(i16Step);//切换继电器

            Command_Transport(i16Step, T_Type);//透传命令
            return;

        }
        #endregion 
        void TestRun_Control(UInt16 i16Step)
        {
            switch (TestItem[i16Step, 5])
            {
                case 0x01://交流电源测试
                case 0x02:
                case 0x04:
                case 0x08:
                case 0x1000:
                    Test_VAC_VDC(i16Step);
                    break;
                case 0x10:
                case 0x20:
                    Test_DI_AI(i16Step, TestItem[i16Step, 5]);
                    break;
                case 0x40:
                    break;
                case 0x80:
                    break;
                case 0x100://HMI通信
                case 0x200:
                case CURRENT_H://加湿板通信
                    Test_Communiction(i16Step,TestItem[i16Step, 5]);
                    break;
                case 0x800:
                    break;
                default:
                    break;
            }
            return;
        }

        public bool TestStart_Check()
        {

            bool Result = false;
            if (RunState.InputScheme != true)//未导入方案
            {
                MessageBox.Show("请导入测试方案！", "提示");
            }
            else
            {
                try//打开串口
                {
                    if ((serialPort1.IsOpen) || (serialPort2.IsOpen))
                    {
                        UnlinkCtrl();
                    }
                    else
                    {
                        LinkCtrl();
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    UnlinkCtrl();
                }

                if (RunState.CommState == 0x03)
                {
                    Result = true;
                }
            }

            return Result;

        }


        public void Test_Stop()
        {
            RunState.ItemRun = false;

            Byte[] pDI = new Byte[2] { 0x00, 0x02 };//数据标识
            Byte[] pBuff1 = new Byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//数据标识 
            Command_Write(COM1, pDI, pBuff1, 2 + 6);

            UnlinkCtrl();//关串口

            timer1.Enabled = false;//关闭定时器1
            timer2.Enabled = false;//关闭定时器2 
        }

        public void Test_State()
        {
            if(RunState.CommState == 0x03)
            {
                RunState.CommState = 0x00;

                Test_Stop();
            }
            else
            {
                if (TestStart_Check() == true)//启动正确
                {
                    //ItemState.Timeout = new UInt16[3] ;
                    for (int i = 0; i < TI; i++)
                    {
                        ItemState[i].Timeout = new UInt16[3];
                    }
                    RunState.ItemRun = true;
                    RunState.Step = 0;
                    RunState.StepOK = true;
                    RunState.TestErr = 0;

                    timer1.Enabled = true;//启动定时器1
                    timer2.Enabled = true;//启动定时器2

                    Refresh_Display(0, 0, 11);
                    Refresh_Display(RunState.Step, RunState.ItemNum, 1);
                }
            }
            //if (TestStart_Check() == true)//启动正确
            //{
            //    //ItemState.Timeout = new UInt16[3] ;
            //    for (int i = 0; i < TI; i++)
            //    {
            //        ItemState[i].Timeout = new UInt16[3];
            //    }
            //    RunState.ItemRun = true;
            //    RunState.Step = 0;
            //    RunState.StepOK = true;
            //    RunState.TestErr = 0;

            //    timer1.Enabled = true;//启动定时器1
            //    timer2.Enabled = true;//启动定时器2

            //    Refresh_Display(0, 0, 11);
            //    Refresh_Display(RunState.Step, RunState.ItemNum, 1);
            //}
            //else
            //{
            //    RunState.ItemRun = false;

            //    Byte[] pDI = new Byte[2] { 0x00, 0x02 };//数据标识
            //    Byte[] pBuff1 = new Byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//数据标识 
            //    Command_Write(COM1, pDI, pBuff1, 2 + 6);

            //    UnlinkCtrl();//关串口

            //    timer1.Enabled = false;//关闭定时器1
            //    timer2.Enabled = false;//关闭定时器2
            //}

            return;
        }

        private void TestStart_Click(object sender, EventArgs e)
        {
            Test_State();
            return;

        }

        private void 开始测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Test_State();
            return;
        }


        #region 配置方案

        /// <summary>
        /// Excel数据导入方法
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dgv"></param> 
        private OleDbConnection conn;//创建个连接对象
        private OleDbCommand comm;//创建个命令对象,方便于执行SQL
        private OleDbDataAdapter da;//创建个适配器，是连接conn和ds的桥梁
        private DataSet ds;//创建个内存表
        public void EcxelToDataGridView(string filePath,Byte SechemeType )
        {
            conn = new OleDbConnection();
            //conn.ConnectionString = "provider=microsoft.jet.oledb.12.0;data source=" + @filePath + ";extended properties='excel 12.0;HDR=Yes;IMEX=1'";
            conn.ConnectionString = "provider=microsoft.jet.oledb.4.0;data source=" + @filePath + ";extended properties='excel 8.0;IMEX=1'";

            comm = new OleDbCommand();
            comm.Connection = conn;

            string Cstr = "Sheet1";
            if (SechemeType == 1)
            {
                comm.CommandText = "select * from [Sheet1$]";
                Cstr = "Sheet1";
            }
            else if (SechemeType == 2)
            {
                comm.CommandText = "select * from [Sheet2$]";
                Cstr = "Sheet2";
            }
            else if (SechemeType == 3)
            {
                comm.CommandText = "select * from [Sheet3$]";
                Cstr = "Sheet3";
            }
            da = new OleDbDataAdapter();
            da.SelectCommand = comm;
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            ds = new DataSet();
            da.Fill(ds, Cstr);

            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 180;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[5].Width = 100;
            for (Int16 i = 6; i < 11; i++)
            {
                dataGridView1.Columns[i].Visible = false;
            }

            //导入方案数据
            DataTable ExcelTable = ds.Tables[Cstr];
            int iColums = ExcelTable.Columns.Count;//列数             
            int iRows = ExcelTable.Rows.Count;//行数 
            RunState.ItemNum = iRows;

            String StrBuf = null;
            for (int i = 0; i < iRows;i++ )
            {
                TestItem[i, 0] = Convert.ToInt64(ExcelTable.Rows[i][0].ToString());
                //TestItem[i, 1] = Convert.ToInt64(ExcelTable.Rows[i][2].ToString());
                //StrBuf = ExcelTable.Rows[i][2].ToString();
                StrBuf = ExcelTable.Rows[i][2].ToString();
                //if (StrBuf == "")
                //{
                //    TestItem[i, 1] = 1;
                //}

                if (StrBuf == "OK")
                {
                    TestItem[i, 1] = 1;
                }
                else
                {
                    TestItem[i, 1] = (Int64)(float.Parse(StrBuf) * 100);//放大100倍
                }
                StrBuf = ExcelTable.Rows[i][3].ToString();
                if (StrBuf == "N.A")
                {
                    TestItem[i, 2] = 0;
                }
                else
                {
                    TestItem[i, 2] = (Int64)(float.Parse(StrBuf) * 100);//放大100倍
                }
                TestItem[i, 5] = Convert.ToInt64(ExcelTable.Rows[i][6].ToString(),16);//
                TestItem[i, 6] = Convert.ToInt64(ExcelTable.Rows[i][7].ToString(),16);
                TestItem[i, 7] = Convert.ToInt64(ExcelTable.Rows[i][8].ToString()); //延时时间
            }

            RunState.InputScheme = true;//导入方案数据成功

            conn.Close();

            return;
        }

        //private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        //{
        //    SetDataGridViewRowXh(e, dataGridView1);
        //}

        //private void SetDataGridViewRowXh(DataGridViewRowPostPaintEventArgs e, DataGridView dataGridView)
        //{
        //    SolidBrush solidBrush = new SolidBrush(dataGridView.RowHeadersDefaultCellStyle.ForeColor);
        //    int xh = e.RowIndex + 1;
        //    e.Graphics.DrawString(xh.ToString(CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, 
        //        solidBrush, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
        //}

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 主控板测试MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = @"Data\M1.xls";
            EcxelToDataGridView(str,1);
            return;
        }

        private void 电源板测试SPAC01P1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = @"Data\M1.xls";
            EcxelToDataGridView(str, 2);
            return;
        }

        private void 加湿板测试SPAC01H1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = @"Data\M1.xls";
            EcxelToDataGridView(str, 3);
            return;
        }
        #endregion

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SPAX控制器测试软件 V1.1", "关于"); return;
        }



    }
}
