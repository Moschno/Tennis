using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class Rally
    {
        private Player _playerOne;
        private Player _playerTwo;
        private Ball _gameBall;
        private eCourtElements _service;
        private eCourtElements _winner;
        private bool _rallyRunning;

        public eCourtElements Winner
        {
            get { return _winner; }
        }

        public Rally(ref Player playerOne, ref Player playerTwo, eCourtElements service)
        {
            _playerOne = playerOne;
            _playerTwo = playerTwo;
            _service = service;
            _gameBall = new Ball();
        }

        public void StartRally()
        {
            if (!_rallyRunning)
            {
                _rallyRunning = true;
                sBatProps tempBatProps, finalBatProps;
                Player playerWithService;
                Player playerWithoutService;
                if (_service == eCourtElements.PlayerOne)
                {
                    playerWithService = _playerOne;
                    playerWithoutService = _playerTwo;
                }
                else if (_service == eCourtElements.PlayerTwo)
                {
                    playerWithService = _playerTwo;
                    playerWithoutService = _playerOne;
                }
                else
                    throw new Exception();

                tempBatProps = playerWithService.DoFirstService();
                finalBatProps = DisturbBat(tempBatProps);
                bool ballOut = CheckIfBallOut(finalBatProps);
                if (ballOut)
                {
                    _rallyRunning = false;
                    if (_service == eCourtElements.PlayerOne)
                    {
                        _winner = eCourtElements.PlayerTwo;
                        return;
                    }
                    else
                    {
                        _winner = eCourtElements.PlayerOne;
                        return;
                    }
                }
                else
                {
                    _rallyRunning = false;
                    _winner = _service;
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
