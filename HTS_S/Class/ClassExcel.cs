using System;
using System.Collections.Generic;
using System.Text; 
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Windows.Forms;

namespace HTS_S
{
    public class ExcelClass : IDisposable
    {
        private Excel._Application m_ExcelApplication = null;
        private Excel._Workbook m_Workbook = null;
        public Excel._Worksheet m_Worksheet = null;
        private object missing = System.Reflection.Missing.Value;

        public ExcelClass()
        {
            if (m_ExcelApplication == null)
            {
                m_ExcelApplication = new Excel.ApplicationClass();
            }
        }

        ~ExcelClass()
        {
            try
            {
                if (m_ExcelApplication != null)
                    m_ExcelApplication.Quit();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
        /// <summary>
        /// ��ȡ�����õ�ǰ������
        /// </summary>
        public int CurrentWorksheetIndex
        {
            set
            {
                if (value <= 0 || value > m_Workbook.Worksheets.Count)
                    throw new Exception("����������Χ");
                else
                {
                    object index = value;
                    m_Worksheet = m_Workbook.Worksheets[index] as Excel._Worksheet;
                }
            }
        }
        /// <summary>
        /// ��һ��Excel������
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenWorkbook(string fileName)
        {
            m_Workbook = m_ExcelApplication.Workbooks.Open(fileName, missing, missing, missing, missing, missing,
                missing, missing, missing, missing, missing, missing, missing, missing, missing);

            if (m_Workbook.Worksheets.Count > 0)
            {
                object index = 1;
                m_Worksheet = m_Workbook.Worksheets[index] as Excel._Worksheet;

            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public void Save()
        {
            if (m_Workbook != null)
            {
                m_Workbook.Save();
            }
        }
        /// <summary>
        /// �ر��ĵ�
        /// </summary>
        /// <param name="isSave"></param>
        public void Close(bool isSave)
        {
            this.ClearClipboard();

            object obj_Save = isSave;
            if (m_Workbook != null)
                m_Workbook.Close(obj_Save, missing, missing);
        }
        /// <summary>
        /// ���õ�ǰ��������ĳ��Ԫ���ֵ
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <param name="value"></param>
        public void SetCellValue(string cellIndex, object value)
        {
            if (m_Worksheet != null)
            {
                object cell1 = cellIndex;
                Excel.Range range = m_Worksheet.get_Range(cell1, missing);
                if (range != null)
                {
                    range.Value2 = value;
                }
            }
        }
        /// <summary>
        /// �ϲ���Ԫ��
        /// </summary>
        /// <param name="cellIndex1"></param>
        /// <param name="cellIndex2"></param>
        public void Merge(string cellIndex1, string cellIndex2)
        {
            if (m_Worksheet != null)
            {
                object cell1 = cellIndex1;
                object cell2 = cellIndex2;
                Excel.Range range = m_Worksheet.get_Range(cell1, cell2);
                range.Merge(true);
            }
        }
        /// <summary>
        /// ����ǰ�������еı�����ݸ��Ƶ����а�
        /// </summary>
        public void Copy()
        {
            if (m_Worksheet != null)
            {
                try
                {
                    m_Worksheet.UsedRange.Select();
                }
                catch { }
                m_Worksheet.UsedRange.Copy(missing);
            }
        }
        /// <summary>
        /// ��ռ��а�
        /// </summary>
        public void ClearClipboard()
        {
            Clipboard.Clear();
        }

        #region IDisposable ��Ա

        public void Dispose()
        {
            try
            {
                if (m_ExcelApplication != null)
                {
                    this.Close(false);
                    m_ExcelApplication.Quit();
                    m_ExcelApplication = null;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        #endregion
    }
}
