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
        private Match _match;
        private Player _playerOne;
        private Player _playerTwo;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)
        {
            _playerOne = new Player(8, 8, 8);
            _playerTwo = new Player(4, 4, 4);
            _match = new Match(ref _playerOne, ref _playerTwo);
            _match.CreateTennisMatch();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rally = new Rally(ref _playerOne, ref _playerTwo);
            rally.RallyFinished += rally_RallyFinished;
            rally.StartRally();
        }

        void rally_RallyFinished(object sender, FinishedEventArgs e)
        {
            tbZufallszahl.Text = e.Winner.ToString();
            var rally = (Rally)sender;
            tbZufallszahl2.Text = rally.Bats[rally.Bats.Count - 1].WhatHappend.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }
    }
}
