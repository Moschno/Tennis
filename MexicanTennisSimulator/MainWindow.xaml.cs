using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using MexicanTennisSimulator.Classes;
using System.Windows.Media.Animation;
using System.Security.Cryptography;


namespace MexicanTennisSimulator
{
    public partial class MainWindow : DXWindow
    {
        private Player _playerOne;
        private Player _playerTwo;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)
        {
            _playerOne = new Player(5, 5, 5, 1, 5);
            _playerTwo = new Player(4, 4, 4, 4, 4);
        }

        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            WinDebug winDebug = new WinDebug(ref _playerOne, ref _playerTwo);
            winDebug.Show();
        }        
    }
}
