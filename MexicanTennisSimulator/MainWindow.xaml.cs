﻿using System;
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


namespace MexicanTennisSimulator
{
    public partial class MainWindow : DXWindow
    {
        private Player _playerOne;
        private Player _playerTwo;
        private Draw _drawOnGridCenter;
        private DataTable _tblMatchStats;
        private Match _match;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void winMain_Loaded(object sender, RoutedEventArgs e)
        {
            _playerOne = new Player(5, 5, 5, 5, 5);
            _playerTwo = new Player(5, 5, 5, 5, 5);

            //_drawOnGridCenter = new Draw(ref gridTennis, 1);
            //_drawOnGridCenter.DrawCourt();

            InitGridMatchStats();
        }

        private void InitGridMatchStats()
        {
            DataColumn[] cols = new DataColumn[3];
            cols[0] = new DataColumn("Spieler 1");
            cols[1] = new DataColumn(" ");
            cols[2] = new DataColumn("Spieler 2");

            _tblMatchStats = new DataTable();
            _tblMatchStats.Columns.AddRange(cols);

            gridMatchStats.ItemsSource = _tblMatchStats.DefaultView;
            gridMatchStats.DataContext = _tblMatchStats.DefaultView;

			gridMatchStats.Columns[1].Width = 300;
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
            if (gridTennis.IsLoaded)
            {
                _drawOnGridCenter.ResizeCourt(); 
            }
        }

        private void btnStartMatch_Click(object sender, RoutedEventArgs e)
        {
            _tblMatchStats.Clear();
            _match = new Match(ref _playerOne, ref _playerTwo);
            _match.StartMatch();
            CalcMatchStats();
        }

        private void CalcMatchStats()
        {
            int[] firstServeNotOut = new int[2];
            int[] firstServe = new int[2];
            int[] aces = new int[2];
            int[] serviceWinner = new int[2];
            int[] doubleFaults = new int[2];
            int[] fastestServeSpeed = new int[2];
			int[] winningOn1stServe = new int[2];
			int[] winningOn2ndServe = new int[2];
			List<double[]> firstServeSpeeds = new List<double[]>();
			List<double[]> secondServeSpeeds = new List<double[]>(); 
            

            foreach (var set in _match.Sets)
            {
                foreach (var game in set.Games)
                {
                    foreach (var rally in game.Rallys)
                    {
						if (rally.Winner == eCourtElements.PlayerWithService)
						{
							var lastServiceBatOfRally = (from p in rally.Bats
														 where p.FinalBatProps.BatPlayerBat == eBats.Service
														 select p).Last();
							if (lastServiceBatOfRally.FinalBatProps.BatType == eBatType.FirstService)
							{
								if (rally.PlayerWithService.Equals(_playerOne))
								{
									winningOn1stServe[0] += 1;
								}
								else
									winningOn1stServe[1] += 1;
							}
							else if (lastServiceBatOfRally.FinalBatProps.BatType == eBatType.FirstService)
							{
								if (rally.PlayerWithService.Equals(_playerOne))
								{
									winningOn2ndServe[0] += 1;
								}
								else
									winningOn2ndServe[1] += 1;
							}
						}

                        foreach (var bat in rally.Bats)
                        {
                            if (bat.FinalBatProps.BatPlayerBat == eBats.Service)
                            {
                                if (bat.FinalBatProps.BatType == eBatType.FirstService)
                                {
                                    if (bat.PlayerWithBat.Equals(_playerOne))
                                    {
                                        firstServeSpeeds.Add(new double[2] { bat.FinalBatProps.BallSpeedTillFirstLanding_KmH, 0 });
                                        firstServe[0] += 1;
                                        if (bat.WhatHappend != eBatResult.BallIsOut)
                                        {
                                            firstServeNotOut[0] += 1; 
                                        }
                                    }
                                    else
                                    {
                                        firstServeSpeeds.Add(new double[2] { 0, bat.FinalBatProps.BallSpeedTillFirstLanding_KmH });
                                        firstServe[1] += 1;
                                        if (bat.WhatHappend != eBatResult.BallIsOut)
                                        {
                                            firstServeNotOut[1] += 1;
                                        }
                                    }
                                }

                                if (bat.FinalBatProps.BatType == eBatType.SecondService)
                                {
                                    if (bat.WhatHappend == eBatResult.BallIsOut)
                                    {
                                        if (rally.PlayerWithService.Equals(_playerOne))
                                        {
                                            secondServeSpeeds.Add(new double[2] { bat.FinalBatProps.BallSpeedTillFirstLanding_KmH, 0 });
                                            doubleFaults[0] += 1;
                                        }
                                        else
                                        {
                                            secondServeSpeeds.Add(new double[2] { 0, bat.FinalBatProps.BallSpeedTillFirstLanding_KmH });
                                            doubleFaults[1] += 1;
                                        }
                                    }
                                }

                                if (bat.WhatHappend == eBatResult.Ace)
                                {
                                    if (rally.PlayerWithService.Equals(_playerOne))
                                    {
                                        aces[0] += 1;
                                    }
                                    else
                                        aces[1] += 1;
                                }

                                if (bat.WhatHappend == eBatResult.BallIsNotTaken)
                                {
                                    if (rally.PlayerWithService.Equals(_playerOne))
                                    {
                                        serviceWinner[0] += 1;
                                    }
                                    else
                                        serviceWinner[1] += 1;
                                }
                            }
                        }
                    }
                }
            }

            DataRow rowFirstServices = _tblMatchStats.NewRow();

            rowFirstServices[0] = firstServeNotOut[0] + " / " + firstServe[0];
            rowFirstServices[1] = "Erste Aufschläge";
            rowFirstServices[2] = firstServeNotOut[1] + " / " + firstServe[1];
            _tblMatchStats.Rows.Add(rowFirstServices);

            DataRow rowAces = _tblMatchStats.NewRow();
            rowAces[0] = aces[0];
            rowAces[1] = "Asse";
            rowAces[2] = aces[1];
            _tblMatchStats.Rows.Add(rowAces);

            DataRow rowServiceWinner = _tblMatchStats.NewRow();
            rowServiceWinner[0] = serviceWinner[0];
            rowServiceWinner[1] = "Aufschlag-Winner";
            rowServiceWinner[2] = serviceWinner[1];
            _tblMatchStats.Rows.Add(rowServiceWinner);

            DataRow rowDoubleFaults = _tblMatchStats.NewRow();
            rowDoubleFaults[0] = doubleFaults[0];
            rowDoubleFaults[1] = "Doppelfehler";
            rowDoubleFaults[2] = doubleFaults[1];
            _tblMatchStats.Rows.Add(rowDoubleFaults);

            DataRow rowFastestServeSpeed = _tblMatchStats.NewRow();
            rowFastestServeSpeed[0] = (from p in firstServeSpeeds where p[0] > 0 select p[0]).Max().Round();
            rowFastestServeSpeed[1] = "Schnellster Aufschlag";
            rowFastestServeSpeed[2] = (from p in firstServeSpeeds where p[1] > 0 select p[1]).Max().Round();
            _tblMatchStats.Rows.Add(rowFastestServeSpeed);

            DataRow rowAverageFirstServeSpeed = _tblMatchStats.NewRow();
            rowAverageFirstServeSpeed[0] = (from p in firstServeSpeeds where p[0] > 0 select p[0]).Average().Round();
            rowAverageFirstServeSpeed[1] = "Durchschnittliche Aufschlagsgeschw. 1st";
            rowAverageFirstServeSpeed[2] = (from p in firstServeSpeeds where p[1] > 0 select p[1]).Average().Round();
            _tblMatchStats.Rows.Add(rowAverageFirstServeSpeed);

			var averagesPlayerOne = (from p in secondServeSpeeds where p[0] > 0 select p[0]);
			var averagesPlayerTwo = (from p in secondServeSpeeds where p[1] > 0 select p[1]);

            DataRow rowAverageSecondServeSpeed = _tblMatchStats.NewRow();
			if (averagesPlayerOne.Count() > 0)
			{
				rowAverageSecondServeSpeed[0] = (from p in secondServeSpeeds where p[0] > 0 select p[0]).Average().Round(); 
			}
            rowAverageSecondServeSpeed[1] = "Durchschnittliche Aufschlagsgeschw. 2nd";
			if (averagesPlayerTwo.Count() > 0)
			{
				rowAverageSecondServeSpeed[2] = (from p in secondServeSpeeds where p[1] > 0 select p[1]).Average().Round(); 
			}
            _tblMatchStats.Rows.Add(rowAverageSecondServeSpeed);

			DataRow rowWinningOn1stServe = _tblMatchStats.NewRow();
			rowWinningOn1stServe[0] = winningOn1stServe[0];
			rowWinningOn1stServe[1] = "Winning on 1st Serve";
			rowWinningOn1stServe[2] = winningOn1stServe[1];
			_tblMatchStats.Rows.Add(rowWinningOn1stServe);

			DataRow rowWinningOn2ndServe = _tblMatchStats.NewRow();
			rowWinningOn2ndServe[0] = winningOn2ndServe[0];
			rowWinningOn2ndServe[1] = "Winning on 2nd Serve";
			rowWinningOn2ndServe[2] = winningOn2ndServe[1];
			_tblMatchStats.Rows.Add(rowWinningOn2ndServe);
        }

        private void btnRealtime_Click(object sender, RoutedEventArgs e)
        {
            WinDebug winDebug = new WinDebug(ref _playerOne, ref _playerTwo);
            winDebug.Show();
        }
    }
}
