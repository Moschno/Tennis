﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MexicanTennisSimulator.Classes
{
    class Bat
    {
        public static event BatFinishedEventHandler BatFinished;

        private Player _playerWithBat;
        private Player _playerWithoutBat;
        private Ball _gameBall;
        private eBatBeginning _batBeginning;
        private sBatProps _tempBatProps;
        private sBatProps _finalBatProps;
        private bool _batRunning;
        private bool _batFinished;
        private eBatEnding _whatHappend;

        public Player PlayerWithBat
        {
            get { return _playerWithBat; }
        }

        public eBatBeginning BatBeginning
        {
            get { return _batBeginning; }
        }

        public sBatProps FinalBatProps
        {
            get { return _finalBatProps; }
        }

        public eBatEnding WhatHappend
        {
            get { return _whatHappend; }
        }

        private eBatEnding whatHappend
        {
            get { return _whatHappend; }
            set
            {
                _batFinished = true;
                _batRunning = false;
                _whatHappend = value;
            }
        }

        public Bat(ref Player playerWithBat, ref Player playerWithoutBat, ref Ball gameBall, eBatBeginning batBeginning = eBatBeginning.Bat)
        {
            _playerWithBat = playerWithBat;
            _playerWithoutBat = playerWithoutBat;
            _gameBall = gameBall;
            _batBeginning = batBeginning;
        }

        public void StartBat()
        {
            if (!_batRunning && !_batFinished)
            {
                _batRunning = true;

                switch (_batBeginning)
                {
                    case eBatBeginning.Bat:
                        _tempBatProps = _playerWithBat.DoBat();
                        break;
                    case eBatBeginning.FirstService:
                        SetPlayersToServicePositions();
                        _tempBatProps = _playerWithBat.DoFirstService();
                        break;
                    case eBatBeginning.SecondService:
                        SetPlayersToServicePositions();
                        _tempBatProps = _playerWithBat.DoSecondService();
                        break;
                    case eBatBeginning.Return:
                        _tempBatProps = _playerWithBat.DoReturn();
                        break;
                }

                DisturbBat();
                _tempBatProps.TimeTillFirstTarget = CalcTimeTillTarget(_tempBatProps.vBatPos
                                                                     , _tempBatProps.vFirstLandingPos
                                                                     , _tempBatProps.SpeedTillFirstLanding_KmH
                                                                     );
                CalcSecondLanding();
                _tempBatProps.TimeTillSecondTarget = CalcTimeTillTarget(_tempBatProps.vFirstLandingPos
                                                                     , _tempBatProps.vSecondLandingPos
                                                                     , _tempBatProps.SpeedTillSecondLanding_KmH
                                                                     );
                _finalBatProps = _tempBatProps;
                if (CheckIfBallIsBroken())
                {
                    whatHappend = eBatEnding.BallIsBroken;
                    return;
                }

                if (CheckIfTryToTakeBall())
                {
                    _playerWithoutBat.RunToBatPosition();
                    if (Probability.GetTrueOrFalse("80"))
                    {
                        _tempBatProps.BallIsTaken = true; 
                    }
                    if (_tempBatProps.BallIsTaken)
                    {
                        if (_batBeginning == eBatBeginning.FirstService ||
                                        _batBeginning == eBatBeginning.SecondService)
                        {
                            whatHappend = eBatEnding.BallIsReturned;
                        }
                        else
                            whatHappend = eBatEnding.BallIsTaken;
                    }
                    else
                        whatHappend = eBatEnding.BallIsNotTaken;
                }
                if (CheckIfBallIsOut())
                {
                    whatHappend = eBatEnding.BallIsOut;
                    return;
                }
            }
        }

        private void SetPlayersToServicePositions()
        {
            _playerWithBat.VTargetPos = new Point(-50, 400);
            _playerWithBat.MoveToTargetPos(0);
            _playerWithoutBat.VTargetPos = new Point(50, -390);
            _playerWithoutBat.MoveToTargetPos(0);
        }

        private bool CheckIfTryToTakeBall()
        {
            return true;
        }

        private void DisturbBat() //todo: Schlag stören
        {
        }

        public static double CalcTimeTillTarget(Point startPos, Point targetPos, double speed_KmH)
        {
            double distanceX = Math.Abs(targetPos.X - startPos.X);
            double distanceY = Math.Abs(targetPos.Y - startPos.Y);
            double distancePixel = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
            double distanceMeter = distancePixel * 10.9728 / 360;
            double time = distanceMeter / (speed_KmH / 3.6);
            return time;
        }

        private void CalcSecondLanding()
        {
            if (_tempBatProps.Bat == eBats.Service || _tempBatProps.Bat == eBats.Smash)
            {
                _tempBatProps.vSecondLandingPos = new Point(9999, -9999);
                _tempBatProps.SpeedTillSecondLanding_KmH = _tempBatProps.SpeedTillFirstLanding_KmH;
            }
            else
            {
                double distanceX = Math.Abs(_tempBatProps.vFirstLandingPos.X - _tempBatProps.vBatPos.X);
                double distanceY = Math.Abs(_tempBatProps.vFirstLandingPos.Y - _tempBatProps.vBatPos.Y);

                distanceX /= 3;
                distanceY /= 3;

                double SecondLandingX = _tempBatProps.vFirstLandingPos.X + distanceX;
                double SecondLandingY = _tempBatProps.vFirstLandingPos.Y + distanceY;

                _tempBatProps.vSecondLandingPos = new Point(SecondLandingX, SecondLandingY);
                _tempBatProps.SpeedTillSecondLanding_KmH = _tempBatProps.SpeedTillSecondLanding_KmH / 2;
            }
        }

        private bool CheckIfBallIsBroken()
        {
            double probability = _tempBatProps.SpeedTillFirstLanding_KmH / 80;
            return Probability.GetTrueOrFalse(probability.ToString());
        }

        private bool CheckIfBallIsOut()
        {
            var firstLandingPos = _tempBatProps.vFirstLandingPos;
            firstLandingPos.X = Math.Abs(firstLandingPos.X);
            firstLandingPos.Y = Math.Abs(firstLandingPos.Y);
            if (firstLandingPos.X <= Match.BallServiceOutLeftX || 
                firstLandingPos.X >= Match.BallServiceOutRightX ||
                firstLandingPos.Y >= Match.BallServiceOutY)
            {
                return true;
            }
            else
                return false;
        }
    }
}