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
        private Ball _ball;
        private Player _playerOne;
        private Player _playerTwo;
        private Draw _draw;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)
        {
            //_ball = new Ball(ref stackpanelSimulation, 10, 10);
            //_playerOne = new Player(ref stackpanelSimulation, 300, 300);
            _draw = new Draw(ref canvasSimulation);
        }

        private void canvasSimulation_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _draw = new Draw(ref canvasSimulation);
        }
    }
}
