using System;
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
        private eBatType _batBeginning;
        private sBatProps _tempBatProps;
        private sBatProps _finalBatProps;
        private bool _batRunning;
        private bool _batFinished;
        private eBatResult _whatHappend;

        public Player PlayerWithBat
        {
            get { return _playerWithBat; }
        }

        public eBatType BatBeginning
        {
            get { return _batBeginning; }
        }

        public sBatProps FinalBatProps
        {
            get { return _finalBatProps; }
        }

        public eBatResult WhatHappend
        {
            get { return _whatHappend; }
        }

        private eBatResult whatHappend
        {
            get { return _whatHappend; }
            set
            {
                RotateVirtualFieldForNextBat();
                _batFinished = true;
                _batRunning = false;
                _whatHappend = value;
            }
        }

        public Bat(ref Player playerWithBat, ref Player playerWithoutBat, ref Ball gameBall, eBatType batBeginning = eBatType.Bat)
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
                    case eBatType.Bat:
                        _tempBatProps = _playerWithBat.DoBat();
                        break;
                    case eBatType.FirstService:
                        SetPlayersToServicePositions();
                        _tempBatProps = _playerWithBat.DoFirstService();
                        break;
                    case eBatType.SecondService:
                        SetPlayersToServicePositions();
                        _tempBatProps = _playerWithBat.DoSecondService();
                        break;
                    case eBatType.Return:
                        _tempBatProps = _playerWithBat.DoReturn();
                        break;
                }

                CalcBatProbs();

                if (_finalBatProps.BallIsBroken)
                {
                    whatHappend = eBatResult.BallIsBroken;
                    return;
                }

                if (_finalBatProps.BallIsTaken)
                {
                    _playerWithoutBat.MoveToTargetPos(Player.MaxGlobalMovementSpeed_KmH);
                    if (_batBeginning == eBatType.FirstService ||
                        _batBeginning == eBatType.SecondService)
                    {
                        whatHappend = eBatResult.BallIsReturned;
                    }
                    else
                        whatHappend = eBatResult.BallIsTaken;
                }
                else if (_finalBatProps.BallWillLandOut)
                {
                    whatHappend = eBatResult.BallIsOut;
                }
                else
                {
                    if (_batBeginning == eBatType.FirstService ||
                        _batBeginning == eBatType.SecondService)
                    {
                        whatHappend = eBatResult.Ace;
                    }
                    else
                        whatHappend = eBatResult.BallIsNotTaken;
                }
            }
        }

        private void CalcBatProbs()
        {
            CalcBatDisturb();
            CalcFirstLanding();
            CalcSecondLanding();
            CalcIfBallIsOut();
            CalcIfBallIsBroken();
            CalcIfBallIsTaken();

            _finalBatProps = _tempBatProps;
        }

        private void CalcFirstLanding()
        {
            _tempBatProps.TimeTillFirstTarget = CalcTimeTillTarget(_tempBatProps.vBatPos
                                                                 , _tempBatProps.vFirstLandingPos
                                                                 , _tempBatProps.SpeedTillFirstLanding_KmH
                                                                 );
        }

        private void SetPlayersToServicePositions()
        {
            _playerWithBat.VTargetPos = Player.ServiceWithBatPos;
            _playerWithBat.MoveToTargetPos(0);
            _playerWithoutBat.VTargetPos = Player.ServiceWithoutBatPos;
            _playerWithoutBat.MoveToTargetPos(0);
        }

        private void RotateVirtualFieldForNextBat()
        {
            var elements = new CourtElement[3];
            elements[0] = _gameBall;
            elements[1] = _playerWithBat;
            elements[2] = _playerWithoutBat;

            foreach (var item in elements)
            {
                var target = new Point();
                target.X = -item.VActPos.X;
                target.Y = -item.VActPos.Y;
                item.VTargetPos = target;
                item.MoveToTargetPos(0);
            }
        }

        private void CalcIfBallIsTaken()
        {
            if (_tempBatProps.BallSeeableOut == eBallSeeableOut.ClearlyVisibleOut)
            {
                _tempBatProps.BallIsTaken = false;
                return;
            }

            Point ballTakePos = new Point();
            double timeTallTakePos_Ball = 0;
            double timeTillTakePos_PlayerWithoutBat = 0;
            double ratio = (_tempBatProps.vBatPos.X * _tempBatProps.vFirstLandingPos.Y) / (_tempBatProps.vBatPos.Y * _tempBatProps.vFirstLandingPos.X);
            if (_tempBatProps.Bat == eBats.Service)
            {
                double ballTakePosY = Player.ServiceWithoutBatPos.Y;
                double ballTakePosX = _tempBatProps.vBatPos.X * ballTakePosY / (_tempBatProps.vBatPos.Y * ratio);
                ballTakePos = new Point(ballTakePosX, ballTakePosY);
            }
            else
            {
                double ballTakePosY = _tempBatProps.vFirstLandingPos.Y + ((_tempBatProps.vSecondLandingPos.Y - _tempBatProps.vFirstLandingPos.Y) / 2);
                double ballTakePosX = _tempBatProps.vFirstLandingPos.X + ((_tempBatProps.vSecondLandingPos.X - _tempBatProps.vFirstLandingPos.X) / 2);
                ballTakePos = new Point(ballTakePosX, ballTakePosY);
            }

            timeTillTakePos_PlayerWithoutBat = CalcTimeTillTarget(_playerWithoutBat.VActPos, ballTakePos, Player.MaxGlobalMovementSpeed_KmH);
            if (_tempBatProps.Bat == eBats.Smash || _tempBatProps.Bat == eBats.Volley)
            {
                timeTallTakePos_Ball = _tempBatProps.TimeTillFirstTarget + CalcTimeTillTarget(_tempBatProps.vFirstLandingPos, ballTakePos, _tempBatProps.SpeedFromFirstTillSecondLanding_KmH);
            }
            else
	            timeTallTakePos_Ball = CalcTimeTillTarget(_tempBatProps.vBatPos, ballTakePos, _tempBatProps.SpeedTillFirstLanding_KmH); 

            double timeDifference = timeTallTakePos_Ball - timeTillTakePos_PlayerWithoutBat;
            if (timeDifference >= 0)
            {
                if (timeDifference <= 0.1)
                {
                    _tempBatProps.TakingDifficulty = eTaking.Hard;
                }
                else if (timeDifference <= 0.5)
                {
                    _tempBatProps.TakingDifficulty = eTaking.Medium;
                }
                else
                    _tempBatProps.TakingDifficulty = eTaking.Easy;

                if (_tempBatProps.BallSeeableOut == eBallSeeableOut.MaybeOut)
                {
                    if (_tempBatProps.TakingDifficulty == eTaking.Hard)
                    {
                        _tempBatProps.BallIsTaken = Probability.GetTrueOrFalse("30"); //todo: Wahrscheinlichkeit abhängig vom Spieler
                    }
                    else if (_tempBatProps.TakingDifficulty == eTaking.Medium)
                    {
                        _tempBatProps.BallIsTaken = Probability.GetTrueOrFalse("70");
                    }
                    else if (_tempBatProps.TakingDifficulty == eTaking.Easy)
                    {
                        if (_tempBatProps.BallWillLandOut)
                        {
                            _tempBatProps.BallIsTaken = false;
                            return;
                        }
                        else
                            _tempBatProps.BallIsTaken = true;
                    }
                }
                else
                    _tempBatProps.BallIsTaken = true;

                if (_tempBatProps.BallIsTaken)
                {
                    _tempBatProps.vTakePos = ballTakePos;
                }
            }
            else
                _tempBatProps.BallIsTaken = false;
        }

        private void CalcBatDisturb() //todo: Schlag stören
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
                _tempBatProps.SpeedFromFirstTillSecondLanding_KmH = _tempBatProps.SpeedTillFirstLanding_KmH;
            }
            else
            {
                double distanceX = _tempBatProps.vFirstLandingPos.X - _tempBatProps.vBatPos.X;
                double distanceY = _tempBatProps.vFirstLandingPos.Y - _tempBatProps.vBatPos.Y;

                distanceX /= 3;
                distanceY /= 3;

                double SecondLandingX = _tempBatProps.vFirstLandingPos.X + distanceX;
                double SecondLandingY = _tempBatProps.vFirstLandingPos.Y + distanceY;

                _tempBatProps.vSecondLandingPos = new Point(SecondLandingX, SecondLandingY);
                _tempBatProps.SpeedFromFirstTillSecondLanding_KmH = _tempBatProps.SpeedTillFirstLanding_KmH / 2;
            }

            _tempBatProps.TimeFromFirstTillSecondTarget = CalcTimeTillTarget(_tempBatProps.vFirstLandingPos
                                                                     , _tempBatProps.vSecondLandingPos
                                                                     , _tempBatProps.SpeedFromFirstTillSecondLanding_KmH
                                                                     );
        }

        private void CalcIfBallIsBroken()
        {
            double probability = _tempBatProps.SpeedTillFirstLanding_KmH / 80;
            _tempBatProps.BallIsBroken = Probability.GetTrueOrFalse(probability.ToString());
        }

        private void CalcIfBallIsOut()
        {
            var firstLandingPos = _tempBatProps.vFirstLandingPos;
            firstLandingPos.X = Math.Abs(firstLandingPos.X);
            firstLandingPos.Y = Math.Abs(firstLandingPos.Y);
            if (_tempBatProps.Bat == eBats.Service)
            {
                if (_tempBatProps.vFirstLandingPos.X <= Match.BallServiceOutLeftX ||
                    _tempBatProps.vFirstLandingPos.X >= Match.BallServiceOutRightX ||
                    _tempBatProps.vFirstLandingPos.Y <= Match.BallServiceOutY)
                {
                    _tempBatProps.BallWillLandOut = true;
                }
                else
                    _tempBatProps.BallWillLandOut = false; 
            }
            else
            {
                double nearestDistance;
                double tempNearestDistance1 = Math.Abs(_tempBatProps.vFirstLandingPos.Y - Match.BallOutY);
                double tempNearestDistance2 = Math.Abs(_tempBatProps.vFirstLandingPos.X - Match.BallOutLeftX);
                double tempNearestDistance3 = Math.Abs(_tempBatProps.vFirstLandingPos.X - Match.BallOutRightX);

                if (_tempBatProps.vFirstLandingPos.X <= Match.BallOutLeftX ||
                    _tempBatProps.vFirstLandingPos.X >= Match.BallOutRightX ||
                    _tempBatProps.vFirstLandingPos.Y <= Match.BallOutY)
                {
                    _tempBatProps.BallWillLandOut = true;
                    if (tempNearestDistance1 < tempNearestDistance2 &&
                    tempNearestDistance1 < tempNearestDistance3)
                    {
                        nearestDistance = tempNearestDistance1;
                    }
                    else if (tempNearestDistance2 < tempNearestDistance3)
                    {
                        nearestDistance = tempNearestDistance2;
                    }
                    else
                        nearestDistance = tempNearestDistance3;

                    if (nearestDistance <= 5)
                    {
                        _tempBatProps.BallSeeableOut = eBallSeeableOut.CantSeeIfOut;
                    }
                    else if (nearestDistance <= 10)
                    {
                        _tempBatProps.BallSeeableOut = eBallSeeableOut.MaybeOut;
                    }
                    else
                        _tempBatProps.BallSeeableOut = eBallSeeableOut.ClearlyVisibleOut;
                }
                else
                    _tempBatProps.BallWillLandOut = false;
            }
        }
    }
}
