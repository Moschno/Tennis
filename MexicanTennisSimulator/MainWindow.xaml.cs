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
            _playerOne = new Player(5, 5, 5, 1, 5);
            _playerTwo = new Player(4, 4, 4, 4, 4);
            _match = new Match(ref _playerOne, ref _playerTwo);
            _match.CreateTennisMatch();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rally = new Rally(ref _playerOne, ref _playerTwo);
            rally.StartRally();

            var nL = Environment.NewLine;
            string txtPlayer = "", txtEnding = "", txtBeginning = "", txtBat = "";
            foreach (var item in rally.Bats)
            {
                txtEnding += item.WhatHappend.ToString() + nL;
                txtBeginning += item.FinalBatProps.BatType.ToString() + nL;
                txtBat += item.FinalBatProps.BatPlayerBat.ToString() + nL;
                if (item.PlayerWithBat.Equals(_playerOne))
                    txtPlayer += eCourtElements.PlayerOne.ToString() + nL;
                else
                    txtPlayer += eCourtElements.PlayerTwo.ToString() + nL;
            }

            tbPlayer.Text = txtPlayer;
            tbBeginning.Text = txtBeginning;
            tbEnding.Text = txtEnding;
            tbBat.Text = txtBat;
            if (rally.Winner == eCourtElements.PlayerWithService)
            {
                tbWinner.Text = eCourtElements.PlayerOne.ToString();
            }
            else
                tbWinner.Text = eCourtElements.PlayerTwo.ToString();
        }

        private void btnGame_Click(object sender, RoutedEventArgs e)
        {
            var game = new Game(ref _playerOne, ref _playerTwo);
            game.StartGame();

            var nL = Environment.NewLine;
            string txtPlayer = "Points P1" + nL + "0" + nL, txtEnding = "Points P2" + nL + "0" + nL, txtBeginning = "", txtBat = "";
            int pointsP1 = 0;
            int pointsP2 = 0;
            foreach (var item in game.Rallys)
            {
                if (item.Winner == eCourtElements.PlayerWithService)
                {
                    pointsP1 += 1;
                    txtPlayer += pointsP1 + nL;
                }
                else
                {
                    pointsP2 += 1;
                    txtEnding += pointsP2 + nL;
                }
            }

            tbPlayer.Text = txtPlayer;
            tbBeginning.Text = txtBeginning;
            tbEnding.Text = txtEnding;
            tbBat.Text = txtBat;

            if (game.Winner == eCourtElements.PlayerWithService)
            {
                tbWinner.Text = eCourtElements.PlayerOne.ToString();
            }
            else
                tbWinner.Text = eCourtElements.PlayerTwo.ToString();
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            var set = new Set(ref _playerOne, ref _playerTwo);
            set.StartSet();

            var nL = Environment.NewLine;
            string txtPlayer = "Games P1" + nL + "0" + nL, txtEnding = "Games P2" + nL + "0" + nL, txtBeginning = "", txtBat = "";
            int pointsP1 = 0;
            int pointsP2 = 0;
            bool evenGamecount = true;
            foreach (var item in set.Games)
            {
                if (item.Winner == eCourtElements.PlayerWithService)
                {
                    if (evenGamecount)
                    {
                        pointsP2 += 1;
                        txtEnding += pointsP2 + nL;
                    }
                    else
                    {
                        pointsP1 += 1;
                        txtPlayer += pointsP1 + nL; 
                    }
                }
                else
                {
                    if (evenGamecount)
                    {
                        pointsP1 += 1;
                        txtPlayer += pointsP1 + nL;
                    }
                    else
                    {
                        pointsP2 += 1;
                        txtEnding += pointsP2 + nL;
                    }
                }

                evenGamecount = !evenGamecount;
            }

            tbPlayer.Text = txtPlayer;
            tbBeginning.Text = txtBeginning;
            tbEnding.Text = txtEnding;
            tbBat.Text = txtBat;

            if (set.Winner == eCourtElements.PlayerWithServiceInFirstGame)
            {
                tbWinner.Text = eCourtElements.PlayerOne.ToString();
            }
            else
                tbWinner.Text = eCourtElements.PlayerTwo.ToString();
        }        
    }
}
