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
        private VirtualCourt _vCourt;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)

        {
            _vCourt = new VirtualCourt();
            //_draw = new Draw(ref rCourt, ref _vCourt);
            Draw.DrawCourt(rCourt);

            _playerOne = new Player();
            _vCourt.Add(_playerOne);
            _playerOne.Move(5, new double[] { 50, 50 });
        }

        private void canvasSimulation_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw.DrawCourt(rCourt);
        }
    }
}
