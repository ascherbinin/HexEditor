using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Collections;

namespace HexEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArrayList _myHexaGrids = new ArrayList();
        private HexGrid _currentHexGrid = new HexGrid();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMenuOpen_Click(object sender, RoutedEventArgs e)
        {
            LoadFile();
        }

        private void LoadFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                hexGrid.Setup(ofd.FileName, this, rtbTranslate);
                hexGrid.Width = 490;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            hexGrid.UpdateBytes();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                if (sfd.FileName != "")
                {
                    File.WriteAllBytes(sfd.FileName, hexGrid.CurrentBytes);
                }
            }
        }
    }

    
}
