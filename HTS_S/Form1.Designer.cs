namespace HTS_S
{
    partial class HTS_S
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加载配置项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.主控板测试MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.电源板测试SPAC01P1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加湿板测试SPAC01H1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Title = new System.Windows.Forms.RichTextBox();
            this.TestStart = new System.Windows.Forms.Button();
            this.groupBoxParam = new System.Windows.Forms.GroupBox();
            this.Port2Baudrate = new System.Windows.Forms.ComboBox();
            this.Port1Baudrate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Port2Name = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Port1Name = new System.Windows.Forms.ComboBox();
            this.label42 = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Capion = new System.Windows.Forms.RichTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxParam.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.操作ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1113, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.加载配置项ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 加载配置项ToolStripMenuItem
            // 
            this.加载配置项ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.主控板测试MToolStripMenuItem,
            this.电源板测试SPAC01P1ToolStripMenuItem,
            this.加湿板测试SPAC01H1ToolStripMenuItem});
            this.加载配置项ToolStripMenuItem.Name = "加载配置项ToolStripMenuItem";
            this.加载配置项ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.加载配置项ToolStripMenuItem.Text = "加载测试方案（A）";
            // 
            // 主控板测试MToolStripMenuItem
            // 
            this.主控板测试MToolStripMenuItem.Name = "主控板测试MToolStripMenuItem";
            this.主控板测试MToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.主控板测试MToolStripMenuItem.Text = "主控板测试（TQ01M1）";
            this.主控板测试MToolStripMenuItem.Click += new System.EventHandler(this.主控板测试MToolStripMenuItem_Click);
            // 
            // 电源板测试SPAC01P1ToolStripMenuItem
            // 
            this.电源板测试SPAC01P1ToolStripMenuItem.Name = "电源板测试SPAC01P1ToolStripMenuItem";
            this.电源板测试SPAC01P1ToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.电源板测试SPAC01P1ToolStripMenuItem.Text = "电源板测试(TQ01P1)";
            this.电源板测试SPAC01P1ToolStripMenuItem.Click += new System.EventHandler(this.电源板测试SPAC01P1ToolStripMenuItem_Click);
            // 
            // 加湿板测试SPAC01H1ToolStripMenuItem
            // 
            this.加湿板测试SPAC01H1ToolStripMenuItem.Name = "加湿板测试SPAC01H1ToolStripMenuItem";
            this.加湿板测试SPAC01H1ToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.加湿板测试SPAC01H1ToolStripMenuItem.Text = "加湿板测试(TQ01H1)";
            this.加湿板测试SPAC01H1ToolStripMenuItem.Click += new System.EventHandler(this.加湿板测试SPAC01H1ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.退出ToolStripMenuItem.Text = "退出（E）";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始测试ToolStripMenuItem,
            this.停止测试ToolStripMenuItem});
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.操作ToolStripMenuItem.Text = "操作";
            // 
            // 开始测试ToolStripMenuItem
            // 
            this.开始测试ToolStripMenuItem.Name = "开始测试ToolStripMenuItem";
            this.开始测试ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.开始测试ToolStripMenuItem.Text = "开始测试";
            this.开始测试ToolStripMenuItem.Click += new System.EventHandler(this.开始测试ToolStripMenuItem_Click);
            // 
            // 停止测试ToolStripMenuItem
            // 
            this.停止测试ToolStripMenuItem.Name = "停止测试ToolStripMenuItem";
            this.停止测试ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.停止测试ToolStripMenuItem.Text = "停止测试";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem,
            this.关于ToolStripMenuItem1});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.关于ToolStripMenuItem.Text = "使用说明";
            // 
            // 关于ToolStripMenuItem1
            // 
            this.关于ToolStripMenuItem1.Name = "关于ToolStripMenuItem1";
            this.关于ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.关于ToolStripMenuItem1.Text = "关于";
            this.关于ToolStripMenuItem1.Click += new System.EventHandler(this.关于ToolStripMenuItem1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.Title);
            this.groupBox2.Controls.Add(this.TestStart);
            this.groupBox2.Controls.Add(this.groupBoxParam);
            this.groupBox2.Location = new System.Drawing.Point(0, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1097, 72);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // Title
            // 
            this.Title.Location = new System.Drawing.Point(555, 14);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(536, 58);
            this.Title.TabIndex = 100;
            this.Title.Text = "";
            // 
            // TestStart
            // 
            this.TestStart.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestStart.Location = new System.Drawing.Point(419, 14);
            this.TestStart.Name = "TestStart";
            this.TestStart.Size = new System.Drawing.Size(95, 52);
            this.TestStart.TabIndex = 99;
            this.TestStart.Text = "开始测试";
            this.TestStart.UseVisualStyleBackColor = true;
            this.TestStart.Click += new System.EventHandler(this.TestStart_Click);
            // 
            // groupBoxParam
            // 
            this.groupBoxParam.Controls.Add(this.Port2Baudrate);
            this.groupBoxParam.Controls.Add(this.Port1Baudrate);
            this.groupBoxParam.Controls.Add(this.label2);
            this.groupBoxParam.Controls.Add(this.Port2Name);
            this.groupBoxParam.Controls.Add(this.label1);
            this.groupBoxParam.Controls.Add(this.label3);
            this.groupBoxParam.Controls.Add(this.Port1Name);
            this.groupBoxParam.Controls.Add(this.label42);
            this.groupBoxParam.Location = new System.Drawing.Point(0, 0);
            this.groupBoxParam.Name = "groupBoxParam";
            this.groupBoxParam.Size = new System.Drawing.Size(413, 72);
            this.groupBoxParam.TabIndex = 2;
            this.groupBoxParam.TabStop = false;
            // 
            // Port2Baudrate
            // 
            this.Port2Baudrate.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Port2Baudrate.FormattingEnabled = true;
            this.Port2Baudrate.ItemHeight = 16;
            this.Port2Baudrate.Location = new System.Drawing.Point(326, 42);
            this.Port2Baudrate.Name = "Port2Baudrate";
            this.Port2Baudrate.Size = new System.Drawing.Size(80, 24);
            this.Port2Baudrate.TabIndex = 21;
            // 
            // Port1Baudrate
            // 
            this.Port1Baudrate.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Port1Baudrate.FormattingEnabled = true;
            this.Port1Baudrate.ItemHeight = 16;
            this.Port1Baudrate.Location = new System.Drawing.Point(326, 14);
            this.Port1Baudrate.Name = "Port1Baudrate";
            this.Port1Baudrate.Size = new System.Drawing.Size(80, 24);
            this.Port1Baudrate.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(237, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 20;
            this.label2.Text = "通信波特率:";
            // 
            // Port2Name
            // 
            this.Port2Name.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Port2Name.FormattingEnabled = true;
            this.Port2Name.Location = new System.Drawing.Point(128, 42);
            this.Port2Name.Name = "Port2Name";
            this.Port2Name.Size = new System.Drawing.Size(80, 24);
            this.Port2Name.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(237, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "通信波特率:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(6, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 16);
            this.label3.TabIndex = 19;
            this.label3.Text = "(万用表)通信口:";
            // 
            // Port1Name
            // 
            this.Port1Name.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Port1Name.FormattingEnabled = true;
            this.Port1Name.Location = new System.Drawing.Point(128, 14);
            this.Port1Name.Name = "Port1Name";
            this.Port1Name.Size = new System.Drawing.Size(80, 24);
            this.Port1Name.TabIndex = 10;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label42.Location = new System.Drawing.Point(22, 17);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(112, 16);
            this.label42.TabIndex = 12;
            this.label42.Text = "(工装)通信口:";
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // serialPort2
            // 
            this.serialPort2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort2_DataReceived);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 100);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1101, 514);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1093, 488);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Test Item";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(4, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1083, 476);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "测试内容";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Capion);
            this.groupBox3.Location = new System.Drawing.Point(718, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(359, 444);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // Capion
            // 
            this.Capion.Location = new System.Drawing.Point(1, 10);
            this.Capion.Name = "Capion";
            this.Capion.ReadOnly = true;
            this.Capion.Size = new System.Drawing.Size(352, 434);
            this.Capion.TabIndex = 0;
            this.Capion.Text = "";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 23);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(709, 436);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1093, 488);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // HTS_S
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 616);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "HTS_S";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工装测试系统";
            this.Load += new System.EventHandler(this.HTS_S_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBoxParam.ResumeLayout(false);
            this.groupBoxParam.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载配置项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始测试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止测试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBoxParam;
        private System.Windows.Forms.Button TestStart;
        private System.Windows.Forms.ComboBox Port2Baudrate;
        private System.Windows.Forms.ComboBox Port1Baudrate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Port2Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Port1Name;
        private System.Windows.Forms.Label label42;
        private System.IO.Ports.SerialPort serialPort1;
        private System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem 主控板测试MToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.RichTextBox Capion;
        private System.Windows.Forms.RichTextBox Title;
        private System.Windows.Forms.ToolStripMenuItem 电源板测试SPAC01P1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加湿板测试SPAC01H1ToolStripMenuItem;
    }
}

