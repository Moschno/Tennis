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

        private Player _playerBat;
        private Player _playerTake;
        private Ball _gameBall;
        private eBatType _batBeginning;
        private sBatProps _tempBatProps;
        private sBatProps _finalBatProps;
        private bool _batRunning;
        private bool _batFinished;
        private eBatResult _whatHappend;

        public Player PlayerWithBat
        {
            get { return _playerBat; }
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
                NegateCourtElementsPos();
                _batFinished = true;
                _batRunning = false;
                _whatHappend = value;
            }
        }

        public Bat(ref Player playerWithBat, ref Player playerWithoutBat, ref Ball gameBall, eBatType batBeginning = eBatType.Bat)
        {
            _playerBat = playerWithBat;
            _playerTake = playerWithoutBat;
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
                        _tempBatProps = _playerBat.DoBat();
                        break;
                    case eBatType.FirstService:
                        SetPlayersToServicePositions();
                        _tempBatProps = _playerBat.DoFirstService();
                        break;
                    case eBatType.SecondService:
                        SetPlayersToServicePositions();
                        _tempBatProps = _playerBat.DoSecondService();
                        break;
                    case eBatType.Return:
                        _tempBatProps = _playerBat.DoReturn();
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
            CalcBatPlayerRepositioning();
            _finalBatProps = _tempBatProps;
        }

        private void CalcBatPlayerRepositioning()
        {
            if (_tempBatProps.BallIsTaken)
            {
                double ratioCourt = Match.BallOutRightX / ((Match.vCourtWidth / 2 / 36 * 27) / 3);
                double repoPosY = (Match.vCourtHeight / 4) - 10;
                double repoPosX = 0;
                if (Math.Abs(_tempBatProps.vTakePlayerTakePos.X) <= Match.BallOutRightX)
                {
                    repoPosX = -_tempBatProps.vTakePlayerTakePos.X * ratioCourt;
                }
                else
                    repoPosX = -((Match.vCourtWidth / 2 / 36 * 27) / 3);

                var vOptimalRepositioningPos = new Point(repoPosX, repoPosY);

                double disposalTimeForRepositioning = _tempBatProps.BallTimeTillTakePos;
                double requiredTimeForRepositioning = CalcTimeTillTarget(_tempBatProps.vBatPlayerBatPos, vOptimalRepositioningPos, Player.MaxGlobalMovementSpeed_KmH);

                Point vTargetPos;
                if (requiredTimeForRepositioning > disposalTimeForRepositioning)
                {
                    double ratioTime = disposalTimeForRepositioning / requiredTimeForRepositioning;
                    double vtargetPosX = vOptimalRepositioningPos.X * ratioTime;
                    double vtargetPosY = vOptimalRepositioningPos.Y * ratioTime;
                    vTargetPos = new Point(vtargetPosX, vtargetPosY);
                }
                else
                    vTargetPos = vOptimalRepositioningPos;

                _playerBat.VTargetPos = vTargetPos;
                _playerBat.MoveToTargetPos(Player.MaxGlobalMovementSpeed_KmH);
            }
        }

        private void SetPlayersToServicePositions()
        {
            _playerBat.VTargetPos = Player.ServiceWithBatPos;
            _playerBat.MoveToTargetPos(0);
            _playerTake.VTargetPos = Player.ServiceWithoutBatPos;
            _playerTake.MoveToTargetPos(0);
        }

        private void NegateCourtElementsPos()
        {
            var elements = new CourtElement[3];
            elements[0] = _gameBall;
            elements[1] = _playerBat;
            elements[2] = _playerTake;

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
            double timeTillTakePos_PlayerWithoutBat = 0;
            double ratio = (_tempBatProps.vBatPlayerBatPos.X * _tempBatProps.vBallFirstLandingPos.Y) / (_tempBatProps.vBatPlayerBatPos.Y * _tempBatProps.vBallFirstLandingPos.X);
            if (_tempBatProps.BatPlayerBat == eBats.Service)
            {
                double ballTakePosY = Player.ServiceWithoutBatPos.Y;
                double ballTakePosX = _tempBatProps.vBatPlayerBatPos.X * ballTakePosY / (_tempBatProps.vBatPlayerBatPos.Y * ratio);
                ballTakePos = new Point(ballTakePosX, ballTakePosY);
            }
            else
            {
                double ballTakePosY = _tempBatProps.vBallFirstLandingPos.Y + ((_tempBatProps.vBallSecondLandingPos.Y - _tempBatProps.vBallFirstLandingPos.Y) / 2);
                double ballTakePosX = _tempBatProps.vBallFirstLandingPos.X + ((_tempBatProps.vBallSecondLandingPos.X - _tempBatProps.vBallFirstLandingPos.X) / 2);
                ballTakePos = new Point(ballTakePosX, ballTakePosY);
            }

            timeTillTakePos_PlayerWithoutBat = CalcTimeTillTarget(_playerTake.VActPos, ballTakePos, Player.MaxGlobalMovementSpeed_KmH);
            if (_tempBatProps.BatPlayerBat == eBats.Smash || _tempBatProps.BatPlayerBat == eBats.Volley)
            {
                _tempBatProps.BallTimeTillTakePos = _tempBatProps.BallTimeTillFirstTarget + CalcTimeTillTarget(_tempBatProps.vBallFirstLandingPos, ballTakePos, _tempBatProps.BallSpeedFromFirstTillSecondLanding_KmH);
            }
            else
	            _tempBatProps.BallTimeTillTakePos = CalcTimeTillTarget(_tempBatProps.vBatPlayerBatPos, ballTakePos, _tempBatProps.BallSpeedTillFirstLanding_KmH); 

            double timeDifference = _tempBatProps.BallTimeTillTakePos - timeTillTakePos_PlayerWithoutBat;
            if (timeDifference >= 0)
            {
                if (timeDifference <= 0.1)
                {
                    _tempBatProps.BallTakingDifficulty = eTaking.Hard;
                }
                else if (timeDifference <= 0.5)
                {
                    _tempBatProps.BallTakingDifficulty = eTaking.Medium;
                }
                else
                    _tempBatProps.BallTakingDifficulty = eTaking.Easy;

                if (_tempBatProps.BallSeeableOut == eBallSeeableOut.MaybeOut)
                {
                    if (_tempBatProps.BallTakingDifficulty == eTaking.Hard)
                    {
                        _tempBatProps.BallIsTaken = Probability.RollByFactor("30"); //todo: Wahrscheinlichkeit abhängig vom Spieler
                    }
                    else if (_tempBatProps.BallTakingDifficulty == eTaking.Medium)
                    {
                        _tempBatProps.BallIsTaken = Probability.RollByFactor("70");
                    }
                    else if (_tempBatProps.BallTakingDifficulty == eTaking.Easy)
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
                    _tempBatProps.vTakePlayerTakePos = ballTakePos;
                }
            }
            else
                _tempBatProps.BallIsTaken = false;

            if (_tempBatProps.BallIsTaken)
            {
                _playerTake.MoveToTargetPos(Player.MaxGlobalMovementSpeed_KmH);
            }
        }

        private void CalcBatDisturb() //todo: Schlag stören
        {
        }

        public static double CalcTimeTillTarget(Point startPos, Point targetPos, double speed_KmH)
        {
            double distanceX = Math.Abs(targetPos.X - startPos.X);
            double distanceY = Math.Abs(targetPos.Y - startPos.Y);
            double distancePixel = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
            double distanceMeter = distancePixel * 10.9728 / (Match.vCourtWidth / 2);
            double time = distanceMeter / (speed_KmH / 3.6);
            return time;
        }

        private void CalcFirstLanding()
        {
            _tempBatProps.BallTimeTillFirstTarget = CalcTimeTillTarget(_tempBatProps.vBatPlayerBatPos
                                                                 , _tempBatProps.vBallFirstLandingPos
                                                                 , _tempBatProps.BallSpeedTillFirstLanding_KmH
                                                                 );
        }

        private void CalcSecondLanding()
        {
            if (_tempBatProps.BatPlayerBat == eBats.Service || _tempBatProps.BatPlayerBat == eBats.Smash)
            {
                _tempBatProps.vBallSecondLandingPos = new Point(9999, -9999);
                _tempBatProps.BallSpeedFromFirstTillSecondLanding_KmH = _tempBatProps.BallSpeedTillFirstLanding_KmH;
            }
            else
            {
                double distanceX = _tempBatProps.vBallFirstLandingPos.X - _tempBatProps.vBatPlayerBatPos.X;
                double distanceY = _tempBatProps.vBallFirstLandingPos.Y - _tempBatProps.vBatPlayerBatPos.Y;

                distanceX /= 3;
                distanceY /= 3;

                double SecondLandingX = _tempBatProps.vBallFirstLandingPos.X + distanceX;
                double SecondLandingY = _tempBatProps.vBallFirstLandingPos.Y + distanceY;

                _tempBatProps.vBallSecondLandingPos = new Point(SecondLandingX, SecondLandingY);
                _tempBatProps.BallSpeedFromFirstTillSecondLanding_KmH = _tempBatProps.BallSpeedTillFirstLanding_KmH / 2;
            }

            _tempBatProps.BallTimeFromFirstTillSecondTarget = CalcTimeTillTarget(_tempBatProps.vBallFirstLandingPos
                                                                     , _tempBatProps.vBallSecondLandingPos
                                                                     , _tempBatProps.BallSpeedFromFirstTillSecondLanding_KmH
                                                                     );
        }

        private void CalcIfBallIsBroken()
        {
            double probability = _tempBatProps.BallSpeedTillFirstLanding_KmH / 80;
            _tempBatProps.BallIsBroken = Probability.RollByFactor(probability.ToString());
        }

        private void CalcIfBallIsOut()
        {
            var firstLandingPos = _tempBatProps.vBallFirstLandingPos;
            firstLandingPos.X = Math.Abs(firstLandingPos.X);
            firstLandingPos.Y = Math.Abs(firstLandingPos.Y);
            if (_tempBatProps.BatPlayerBat == eBats.Service)
            {
                if (_tempBatProps.vBallFirstLandingPos.X <= Match.BallServiceOutLeftX ||
                    _tempBatProps.vBallFirstLandingPos.X >= Match.BallServiceOutRightX ||
                    _tempBatProps.vBallFirstLandingPos.Y <= Match.BallServiceOutY)
                {
                    _tempBatProps.BallWillLandOut = true;
                }
                else
                    _tempBatProps.BallWillLandOut = false; 
            }
            else
            {
                double nearestDistance;
                double tempNearestDistance1 = Math.Abs(_tempBatProps.vBallFirstLandingPos.Y - Match.BallOutY);
                double tempNearestDistance2 = Math.Abs(_tempBatProps.vBallFirstLandingPos.X - Match.BallOutLeftX);
                double tempNearestDistance3 = Math.Abs(_tempBatProps.vBallFirstLandingPos.X - Match.BallOutRightX);

                if (_tempBatProps.vBallFirstLandingPos.X <= Match.BallOutLeftX ||
                    _tempBatProps.vBallFirstLandingPos.X >= Match.BallOutRightX ||
                    _tempBatProps.vBallFirstLandingPos.Y <= Match.BallOutY)
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
