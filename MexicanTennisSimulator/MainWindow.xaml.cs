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
using System.Data;
using System.Windows.Forms;
using System.Drawing;


namespace MexicanTennisSimulator
{
    public partial class MainWindow : DXWindow
    {
        private Player _playerOne;
        private Player _playerTwo;
        private DataTable _tblMatchStats;
        private DataTable _tblMatchHistory;
        private Match _match;

        public MainWindow()
        {
            InitializeComponent();
            _playerOne = new Player(5, 5, 5, 5, 5);
            _playerTwo = new Player(5, 5, 5, 5, 5);
        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void InitGridMatchStats()
        {
            if (_tblMatchStats != null)
                _tblMatchStats.Clear(); 

            DataColumn[] cols = new DataColumn[3];
            cols[0] = new DataColumn("Spieler 1");
            cols[1] = new DataColumn(" ");
            cols[2] = new DataColumn("Spieler 2");

            _tblMatchStats = new DataTable();
            _tblMatchStats.Columns.AddRange(cols);

            gridMatchStats.ItemsSource = _tblMatchStats.DefaultView;
            gridMatchStats.DataContext = _tblMatchStats.DefaultView;

			gridMatchStats.Columns[1].Width = 250;
        }

        private void InitGridMatchHistory()
        {
            if (_tblMatchHistory != null)
                _tblMatchHistory.Clear();
            
            DataColumn[] cols = new DataColumn[_match.Sets.Count + 1];
            for (int i = 0; i < cols.Length; i++)
                cols[i] = new DataColumn();

            _tblMatchHistory = new DataTable();
            _tblMatchHistory.Columns.AddRange(cols);

            DataRow rowMatchHistoryPlayerOne = _tblMatchHistory.NewRow();
            DataRow rowMatchHistoryPlayerTwo = _tblMatchHistory.NewRow();

            _tblMatchHistory.Rows.Add(rowMatchHistoryPlayerOne);
            _tblMatchHistory.Rows.Add(rowMatchHistoryPlayerTwo);

            _tblMatchHistory.Rows[0][0] = "Spieler 1";
            _tblMatchHistory.Rows[1][0] = "Spieler 2";

            gridMatchHistory.Columns.Clear();
            gridMatchHistory.ItemsSource = _tblMatchHistory.DefaultView;
            gridMatchHistory.DataContext = _tblMatchHistory.DefaultView;

            foreach (var col in gridMatchHistory.Columns)
            {
                if (col.VisibleIndex != 0) col.Width = 25;
            }
        }

        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            WinDebug winDebug = new WinDebug(ref _playerOne, ref _playerTwo);
            winDebug.Show();
        }

        private void tbStrength_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
        }

        private void tbStrength_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
        }

        private void trackBarEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var trackBar = (DevExpress.Xpf.Editors.TrackBarEdit)sender;
            var gbGrid = (Grid)trackBar.Parent;
            var groupBox = (DevExpress.Xpf.LayoutControl.GroupBox)gbGrid.Parent;
            var stackPanel = (StackPanel)groupBox.Parent;
            var groupBoxOuter = (DevExpress.Xpf.LayoutControl.GroupBox)stackPanel.Parent;
            var col = (int)groupBoxOuter.GetValue(Grid.ColumnProperty);

            Player player;
            if (col == 0)
                player = _playerOne;
            else
                player = _playerTwo;

            int value = Convert.ToInt32(e.NewValue);
            if (stackPanel.Children[0].Equals(groupBox))
            {
                player.Strength = value;
            }
            else if (stackPanel.Children[1].Equals(groupBox))
            {
                player.Velocity = value;
            }
            else if (stackPanel.Children[2].Equals(groupBox))
            {
                player.Precision = value;
            }
            else if (stackPanel.Children[3].Equals(groupBox))
            {
                player.Service = value;
            }
            else if (stackPanel.Children[4].Equals(groupBox))
            {
                player.Retörn = value;
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void btnStartMatch_Click(object sender, RoutedEventArgs e)
        {
            _match = new Match(ref _playerOne, ref _playerTwo);
            _match.StartMatch();
            InitGridMatchStats();
            InitGridMatchHistory();
            CalcMatchStats();
            CalcHistoryStats();
        }

        private void CalcMatchStats()
        {
            int[] firstServeNotOut = new int[2];
            int[] firstServe = new int[2];
            int[] secondServeNoOut = new int[2];
            int[] aces = new int[2];
            int[] doubleFaults = new int[2];
            int[] fastestServeSpeed = new int[2];
			int[] winningOn1stServe = new int[2];
			int[] winningOn2ndServe = new int[2];
			List<double[]> firstServeSpeeds = new List<double[]>();
			List<double[]> secondServeSpeeds = new List<double[]>();
            int[] totalPointsWon = new int[2];
            
            foreach (var set in _match.Sets)
            {
                foreach (var game in set.Games)
                {
                    foreach (var rally in game.Rallys)
                    {
						if (rally.Winner == eCourtElements.PlayerWithService)
						{
                            if (rally.PlayerWithService.Equals(_playerOne))
                            {
                                totalPointsWon[0] += 1;
                            }
                            else
                                totalPointsWon[1] += 1;

							var lastServiceBatOfRally = (from p in rally.Bats
														 where p.FinalBatProps.BatPlayerBat == eBats.Service
														 select p).Last();
                            if (lastServiceBatOfRally.WhatHappend == eBatResult.Ace ||
                                lastServiceBatOfRally.WhatHappend == eBatResult.BallIsReturned ||
                                lastServiceBatOfRally.WhatHappend == eBatResult.BallIsTaken)
                            {
                                if (lastServiceBatOfRally.FinalBatProps.BatType == eBatType.FirstService)
                                {
                                    if (rally.PlayerWithService.Equals(_playerOne))
                                    {
                                        winningOn1stServe[0] += 1;
                                    }
                                    else
                                        winningOn1stServe[1] += 1;
                                }
                                else if (lastServiceBatOfRally.FinalBatProps.BatType == eBatType.SecondService)
                                {
                                    if (rally.PlayerWithService.Equals(_playerOne))
                                    {
                                        winningOn2ndServe[0] += 1;
                                    }
                                    else
                                        winningOn2ndServe[1] += 1;
                                } 
                            }
						}
                        else
                        {
                            if (rally.PlayerWithoutService.Equals(_playerOne))
                            {
                                totalPointsWon[0] += 1;
                            }
                            else
                                totalPointsWon[1] += 1;
                        }

                        foreach (var bat in rally.Bats)
                        {
                            if (bat.FinalBatProps.BatPlayerBat == eBats.Service)
                            {
                                if (bat.WhatHappend == eBatResult.Ace)
                                {
                                    if (rally.PlayerWithService.Equals(_playerOne))
                                    {
                                        aces[0] += 1;
                                    }
                                    else
                                        aces[1] += 1;
                                }

                                if (bat.FinalBatProps.BatType == eBatType.FirstService)
                                {
                                    if (bat.PlayerWithBat.Equals(_playerOne))
                                    {
                                        firstServeSpeeds.Add(new double[2] { bat.FinalBatProps.BallSpeedTillFirstLanding_KmH, 0 });
                                        firstServe[0] += 1;
                                        if (bat.WhatHappend == eBatResult.Ace ||
                                            bat.WhatHappend == eBatResult.BallIsReturned ||
                                            bat.WhatHappend == eBatResult.BallIsTaken)
                                        {
                                            firstServeNotOut[0] += 1; 
                                        }
                                    }
                                    else
                                    {
                                        firstServeSpeeds.Add(new double[2] { 0, bat.FinalBatProps.BallSpeedTillFirstLanding_KmH });
                                        firstServe[1] += 1;
                                        if (bat.WhatHappend == eBatResult.Ace ||
                                            bat.WhatHappend == eBatResult.BallIsReturned ||
                                            bat.WhatHappend == eBatResult.BallIsTaken)
                                        {
                                            firstServeNotOut[1] += 1;
                                        }
                                    }
                                }

                                if (bat.FinalBatProps.BatType == eBatType.SecondService)
                                {
                                    if (bat.PlayerWithBat.Equals(_playerOne))
                                    {
                                        secondServeSpeeds.Add(new double[2] { bat.FinalBatProps.BallSpeedTillFirstLanding_KmH, 0 });
                                        if (bat.WhatHappend == eBatResult.Ace ||
                                            bat.WhatHappend == eBatResult.BallIsReturned ||
                                            bat.WhatHappend == eBatResult.BallIsTaken)
                                        {
                                            secondServeNoOut[0] += 1;
                                        }
                                        else
                                            doubleFaults[0] += 1;
                                    }
                                    else
                                    {
                                        secondServeSpeeds.Add(new double[2] { 0, bat.FinalBatProps.BallSpeedTillFirstLanding_KmH });
                                        if (bat.WhatHappend == eBatResult.Ace ||
                                            bat.WhatHappend == eBatResult.BallIsReturned ||
                                            bat.WhatHappend == eBatResult.BallIsTaken)
                                        {
                                            secondServeNoOut[1] += 1;
                                        }
                                        else
                                            doubleFaults[1] += 1; 
                                    }
                                }
                            }
                        }
                    }
                }
            }

            DataRow rowFirstServices = _tblMatchStats.NewRow();

            rowFirstServices[0] = firstServeNotOut[0] + " / " + firstServe[0] + " (" + (int)(firstServeNotOut[0] * 100 / firstServe[0]) + "%)";
            rowFirstServices[1] = "Erste Aufschläge";
            rowFirstServices[2] = firstServeNotOut[1] + " / " + firstServe[1] + " (" + (int)(firstServeNotOut[1] * 100 / firstServe[1]) + "%)"; ;
            _tblMatchStats.Rows.Add(rowFirstServices);

            DataRow rowAces = _tblMatchStats.NewRow();
            rowAces[0] = aces[0];
            rowAces[1] = "Asse";
            rowAces[2] = aces[1];
            _tblMatchStats.Rows.Add(rowAces);

            DataRow rowDoubleFaults = _tblMatchStats.NewRow();
            rowDoubleFaults[0] = doubleFaults[0];
            rowDoubleFaults[1] = "Doppelfehler";
            rowDoubleFaults[2] = doubleFaults[1];
            _tblMatchStats.Rows.Add(rowDoubleFaults);

            DataRow rowFastestServeSpeed = _tblMatchStats.NewRow();
            rowFastestServeSpeed[0] = (from p in firstServeSpeeds where p[0] > 0 select p[0]).Max().Round() + " Km/h";
            rowFastestServeSpeed[1] = "Schnellster Aufschlag";
            rowFastestServeSpeed[2] = (from p in firstServeSpeeds where p[1] > 0 select p[1]).Max().Round() + " Km/h";
            _tblMatchStats.Rows.Add(rowFastestServeSpeed);

            DataRow rowAverageFirstServeSpeed = _tblMatchStats.NewRow();
            rowAverageFirstServeSpeed[0] = (from p in firstServeSpeeds where p[0] > 0 select p[0]).Average().Round() + " Km/h";
            rowAverageFirstServeSpeed[1] = "Durchschnittliche Geschw. erster Aufschlag";
            rowAverageFirstServeSpeed[2] = (from p in firstServeSpeeds where p[1] > 0 select p[1]).Average().Round() + " Km/h";
            _tblMatchStats.Rows.Add(rowAverageFirstServeSpeed);

			var averagesPlayerOne = (from p in secondServeSpeeds where p[0] > 0 select p[0]);
			var averagesPlayerTwo = (from p in secondServeSpeeds where p[1] > 0 select p[1]);

            DataRow rowAverageSecondServeSpeed = _tblMatchStats.NewRow();
            if (averagesPlayerOne.Count() > 0)
                rowAverageSecondServeSpeed[0] = (from p in secondServeSpeeds where p[0] > 0 select p[0]).Average().Round() + " Km/h";
            else
                rowAverageSecondServeSpeed[0] = "n.A.";
            rowAverageSecondServeSpeed[1] = "Durchschnittliche Geschw. zweiter Aufschlag";
			if (averagesPlayerTwo.Count() > 0)
                rowAverageSecondServeSpeed[2] = (from p in secondServeSpeeds where p[1] > 0 select p[1]).Average().Round() + " Km/h"; 
            else
                rowAverageSecondServeSpeed[2] = "n.A.";
            _tblMatchStats.Rows.Add(rowAverageSecondServeSpeed);

            DataRow rowWinningOn1stServe = _tblMatchStats.NewRow();
            if (firstServeNotOut[0] != 0)
                rowWinningOn1stServe[0] = winningOn1stServe[0] + " / " + firstServeNotOut[0] + " (" + (int)(winningOn1stServe[0] * 100 / firstServeNotOut[0]) + "%)"; 
            else
                rowWinningOn1stServe[0] = winningOn1stServe[0] + " / " + firstServeNotOut[0] + " (0%)";
            rowWinningOn1stServe[1] = "Punkt nach erstem Aufschlag";
            if (firstServeNotOut[1] != 0)
                rowWinningOn1stServe[2] = winningOn1stServe[1] + " / " + firstServeNotOut[1] + " (" + (int)(winningOn1stServe[1] * 100 / firstServeNotOut[1]) + "%)";
            else
                rowWinningOn1stServe[2] = winningOn1stServe[1] + " / " + firstServeNotOut[1] + " (0%)";
            _tblMatchStats.Rows.Add(rowWinningOn1stServe);

			DataRow rowWinningOn2ndServe = _tblMatchStats.NewRow();
            if (secondServeNoOut[0] != 0)
                rowWinningOn2ndServe[0] = winningOn2ndServe[0] + " / " + secondServeNoOut[0] + " (" + (int)(winningOn2ndServe[0] * 100 / secondServeNoOut[0]) + "%)"; 
            else
                rowWinningOn2ndServe[0] = winningOn2ndServe[0] + " / " + secondServeNoOut[0] + " (0%)"; 
			rowWinningOn2ndServe[1] = "Punkt nach zweitem Aufschlag";
            if (secondServeNoOut[1] != 0)
                rowWinningOn2ndServe[2] = winningOn2ndServe[1] + " / " + secondServeNoOut[1] + " (" + (int)(winningOn2ndServe[1] * 100 / secondServeNoOut[1]) + "%)";
            else
                rowWinningOn2ndServe[2] = winningOn2ndServe[1] + " / " + secondServeNoOut[1] + " (0%)"; 
			_tblMatchStats.Rows.Add(rowWinningOn2ndServe);

            DataRow rowTotalPointsWon = _tblMatchStats.NewRow();
            rowTotalPointsWon[0] = totalPointsWon[0];
            rowTotalPointsWon[1] = "Punkte Gesamt";
            rowTotalPointsWon[2] = totalPointsWon[1];
            _tblMatchStats.Rows.Add(rowTotalPointsWon);
        }

        private void CalcHistoryStats()
        {
            int[] games;
            int actSet = new int();
            foreach (var set in _match.Sets)
            {
                actSet += 1;
                games = new int[2];

                foreach (var game in set.Games)
                {
                    if (game.Winner == eCourtElements.PlayerWithService)
                    {
                        if (game.PlayerWithService.Equals(_playerOne))
                        {
                            games[0] += 1;
                        }
                        else
                            games[1] += 1;
                    }
                    else
                    {
                        if (game.PlayerWithoutService.Equals(_playerOne))
                        {
                            games[0] += 1;
                        }
                        else
                            games[1] += 1;
                    }
                }

                _tblMatchHistory.Rows[0][actSet] = games[0];
                _tblMatchHistory.Rows[1][actSet] = games[1];
                games = null;
            }

            if (_match.Winner == eCourtElements.PlayerOne)
                _tblMatchHistory.Rows[0][0] += " (Sieger)"; 
            else
                _tblMatchHistory.Rows[1][0] += " (Sieger)";
        }

        private void btnRealtime_Click(object sender, RoutedEventArgs e)
        {
            WinDebug winDebug = new WinDebug(ref _playerOne, ref _playerTwo);
            winDebug.Show();
        }
    }
}
