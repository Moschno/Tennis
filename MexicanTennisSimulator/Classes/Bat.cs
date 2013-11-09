using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class Bat
    {
        public static event BatFinishedEventHandler BatFinished;
        public static event BatFinishedEventHandler FirstServiceFinished;
        public static event BatFinishedEventHandler SecondServiceFinished;

        private Player _playerWithBat;
        private Player _playerWithoutBat;
        private Ball _gameBall;
        private eBatBeginning _bat;
        private bool _batFinished;
        private eBatEnding _whatHappend;

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
                _whatHappend = value;
                switch (_bat)
                {
                    case eBatBeginning.Default:
                        BatFinished(this, new BatFinishedEventArgs(value));
                        break;
                    case eBatBeginning.FirstService:
                        FirstServiceFinished(this, new BatFinishedEventArgs(value));
                        break;
                    case eBatBeginning.SecondService:
                        SecondServiceFinished(this, new BatFinishedEventArgs(value));
                        break;
                    case eBatBeginning.Return:
                        BatFinished(this, new BatFinishedEventArgs(value));
                        break;
                }
            }
        }

        public Bat(ref Player playerWithBat, ref Player playerWithoutBat, ref Ball gameBall, eBatBeginning bat = eBatBeginning.Default)
        {
            _playerWithBat = playerWithBat;
            _playerWithoutBat = playerWithoutBat;
            _gameBall = gameBall;
            _bat = bat;
        }

        public void StartBat()
        {
            sBatProps tempBatProps, finalBatProps;

            if (_bat == eBatBeginning.FirstService)
            {
                tempBatProps = _playerWithBat.DoFirstService();
                finalBatProps = DisturbBat(tempBatProps);
                bool ballOut = CheckIfBallOut(finalBatProps);
                if (ballOut)
                {
                    whatHappend = eBatEnding.BallIsOut;
                }
                else
                {
                    if (Probability.GetTrueOrFalse(50))
                    {
                        whatHappend = eBatEnding.Ace;
                    }
                    else
                    {
                        whatHappend = eBatEnding.BallIsNotReturned; 
                    }
                } 
            }

            return;
        }

        private sBatProps DisturbBat(sBatProps batProps) //todo: Schlag stören
        {
            return batProps;
        }

        private bool CheckIfBallOut(sBatProps batProbs)
        {
            var firstLandingPos = batProbs.vTargetPos;
            firstLandingPos.X = Math.Abs(firstLandingPos.X);
            firstLandingPos.Y = Math.Abs(firstLandingPos.Y);
            if (firstLandingPos.X >= Match.BallServiceOutRightX
                || firstLandingPos.X <= Match.BallServiceOutLeftX
                || firstLandingPos.Y >= Match.BallServiceOutY)
            {
                return true;
            }
            else
                return false;
        }
    }
}
