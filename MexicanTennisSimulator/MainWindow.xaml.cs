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
        private Ball _ball;
        private Player _playerOne;
        private Player _playerTwo;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)
        {
            Draw.DrawCourt(ref _rCourt);

            Rally rallyProps;
            rallyProps.Service = Players.One;
            rallyProps.UpperSide = Players.One;

            _playerOne.Prepare4Rally(rallyProps);
            _playerTwo.Prepare4Rally(rallyProps);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _playerOne.StartRally();
            _playerTwo.StartRally();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void winMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
