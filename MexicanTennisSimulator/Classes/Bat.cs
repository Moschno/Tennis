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

            tempBatProps = _playerWithBat.DoFirstService();
            finalBatProps = DisturbBat(tempBatProps);

            if (Probability.GetTrueOrFalse("10"))
            {
                whatHappend = eBatEnding.BallIsBroken;
                return;
            }

            bool ballOut = CheckIfBallOut(finalBatProps);
            if (ballOut)
            {
                whatHappend = eBatEnding.BallIsOut;
                return;
            }

            if (_bat == eBatBeginning.FirstService)
            {
                if (Probability.GetTrueOrFalse("10"))
                {
                    whatHappend = eBatEnding.Ace;
                    return;
                }
                else
                {
                    whatHappend = eBatEnding.BallIsReturned;
                    return;
                }
            }
            else if (_bat == eBatBeginning.SecondService)
            {
                if (Probability.GetTrueOrFalse("10"))
                {
                    whatHappend = eBatEnding.Ace;
                    return;
                }
                else
                {
                    whatHappend = eBatEnding.BallIsReturned;
                    return;
                }
            }
            else
            {
                if (Probability.GetTrueOrFalse("10"))
                {
                    whatHappend = eBatEnding.BallIsNotReturned;
                    return;
                }
                else if (Probability.GetTrueOrFalse("50"))
                {
                    whatHappend = eBatEnding.BallIsReturned;
                    return;
                }
                else
                {
                    whatHappend = eBatEnding.BallIsOut;
                    return;
                }
            }
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
