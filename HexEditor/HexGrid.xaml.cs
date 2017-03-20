using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HexEditor
{
    /// <summary>
    /// Interaction logic for HexGrid.xaml
    /// </summary>
    public partial class HexGrid : UserControl
    {
        private string allowValues = "0123456789ABCDEF";
        private MainWindow _myMainWindow;
        private RichTextBox _rtbTranslate;
        private byte[] myBytes;
        private List<byte> _myNewBytes = new List<byte>();
        private List<string> _newBytes = new List<string>();
        public string FileName { get; private set; }
        HexDS.ByteTableDataTable oTable = new HexDS.ByteTableDataTable();

        public byte[] CurrentBytes {
            get
            {
                return myBytes;
            }
            private set
            {
                myBytes = value;
            }
        }

        public HexGrid()
        {
            InitializeComponent();
        }

        public HexGrid(string fileName, MainWindow window)
        {
            InitializeComponent();
            _myMainWindow = window;
            FileName = fileName;
            GetBytesFromFile(fileName);
            FillGrid();

        }

        public void Setup(string fileName, MainWindow window, RichTextBox rtbTranslate)
        {
            _myMainWindow = window;
            FileName = fileName;
            _rtbTranslate = rtbTranslate;
            GetBytesFromFile(fileName);
            FillGrid();
            UpdateInfo();

        }

        public string ShortFileName
        {
            get
            {
                FileInfo MyInfo = new FileInfo(FileName);
                return MyInfo.Name;
            }
        }

        private void GetBytesFromFile(string fileName)
        {
            myBytes = File.ReadAllBytes(fileName);
        }

        private void FillGrid()
        {
            int j = 0;
            for (int i = 0; i < myBytes.Length; i += 16)
            {
                string[] myStringArr = new string[17];

                myStringArr[0] = j.ToString();
                for (int k = 0; k <= 15 & i + k < myBytes.Length; k++)
                {
                    myStringArr[k + 1] = GetDisplayForByte(myBytes[i + k]);//  myBytes[i + k].ToString();
                }
                oTable.Rows.Add(myStringArr);
                j++;
            }
            grid.ItemsSource = oTable;
        }

        protected char GetDisplayChar(char cData)
        {
            if (20 > cData)
            {
                cData = (char)0xB7;
            }

            return cData;
        }

        protected char GetDisplayChar(byte byData)
        {
            return GetDisplayChar((char)byData);
        }

        protected string GetDisplayForByte(byte byData)
        {
            return string.Format("{0:X2}", byData);
        }

        public void UpdateInfo()
        {
            _rtbTranslate.Document.Blocks.Clear();
            foreach (byte item in myBytes)
            {
                _rtbTranslate.AppendText(GetDisplayChar(item).ToString());
            }
        }
        public void UpdateBytes()
        {
            _newBytes.Clear();
            var table = oTable[0]; //get first table from Dataset
            foreach (DataRow row in table.Table.Rows)
            {
                for (int i = 1; i < row.ItemArray.Length; i++)
                {
                    if (row.ItemArray[i] != null)
                    {
                        _newBytes.Add(row.ItemArray[i].ToString());
                    }

                }

            }
            UpdateChars();
        }

        public void UpdateChars()
        {
            _myNewBytes.Clear();
            foreach (var item in _newBytes)
            {
                var bytes = ToByteArray(item);
                foreach (var bt in bytes)
                {
                    _myNewBytes.Add(bt);
                }
                
            }
            myBytes = _myNewBytes.ToArray();
            UpdateInfo();
        }

        public static byte[] ToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.Back)
            {
                return;
            }
            if (!IsValidChar((char)KeyInterop.VirtualKeyFromKey(e.Key)))
            {
                e.Handled = true;
                MessageBox.Show("Is not valid hex value, sry [0123456789ABCDEF].", "Warning");
            }
        }

        private bool IsValidChar(char ch)
        {
            foreach (char item in allowValues)
            {
                if (item == ch) return true;
            }
            return false;
        }
    }
}
