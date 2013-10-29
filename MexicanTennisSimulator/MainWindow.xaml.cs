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

            _ball = new Ball(ref _rCourt, Colors.Yellow);
            _playerOne = new Player(ref _rCourt, Colors.Blue);
            _playerTwo = new Player(ref _rCourt, Colors.Red);

            _playerOne.MoveTo(new Point(100, 200));
            _playerTwo.MoveTo(new Point(200, 100));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbZufallszahl.Text = "false";
            int count = 1;
            while (!Probability.GetTrueOrFalse(0.0001))
            {
                count++;
            }
            tbZufallszahl.Text = "true";
            tbZufallszahl2.Text = count.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tbZufallszahl2.Text = Probability.GetTrueOrFalse(10.00).ToString();
        }
    }
}
