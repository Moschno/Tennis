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


namespace MexicanTennisSimulator
{
    public partial class MainWindow : DXWindow
    {
        private Ball _ball = new Ball();
        private Player _playerOne = new Player();
        private Player _playerTwo = new Player();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)
        {
            Draw.DrawCourt(rCourt);

            rCourt.Children.Add(_ball);
            rCourt.Children.Add(_playerOne);
            rCourt.Children.Add(_playerTwo);
        }

        private void canvasSimulation_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _playerOne.MoveTo(new Point(100, 100));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }
    }
}
