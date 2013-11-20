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


namespace MexicanTennisSimulator
{
    /// <summary>
    /// Interaction logic for WinDebug.xaml
    /// </summary>
    public partial class WinDebug : DXWindow
    {
        private Player _playerOne;
        private Player _playerTwo;

        internal WinDebug(ref Player playerOne, ref Player playerTwo)
        {
            InitializeComponent();
            _playerOne = playerOne;
            _playerTwo = playerTwo;
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

        private void btnMatch_Click(object sender, RoutedEventArgs e)
        {
            var match = new Match(ref _playerOne, ref _playerTwo);
            match.StartMatch();

            var nL = Environment.NewLine;
            string txtPlayer = "Sets P1" + nL + "0" + nL, txtEnding = "Sets P2" + nL + "0" + nL, txtBeginning = "", txtBat = "";
            int pointsP1 = 0;
            int pointsP2 = 0;
            foreach (var item in match.Sets)
            {
                if (item.Winner == eCourtElements.PlayerWithServiceInFirstGame)
                {
                    if (item.PlayerWithServiceInFirstGame.Equals(_playerOne))
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
                else
                {
                    if (item.PlayerWithServiceInFirstGame.Equals(_playerOne))
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
            }

            tbPlayer.Text = txtPlayer;
            tbBeginning.Text = txtBeginning;
            tbEnding.Text = txtEnding;
            tbBat.Text = txtBat;
            tbWinner.Text = match.Winner.ToString();
        }
    }
}
