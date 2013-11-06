using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class TennisMatch
    {
        private Player _playerOne;
        private Player _playerTwo;
        private Ball _ball;
        private vCourt _vCourt;
        private Rally _rallyProbs;
        private bool _matchRunning;

        public int StrengthPlayerOne
        {
            get { return _playerOne.MaxBatStrength; }
            set
            {
                if (!_matchRunning)
                {
                    _playerOne.MaxBatStrength = value; 
                }
            }
        }

        public int StrengthPlayerTwo
        {
            get { return _playerTwo.MaxBatStrength; }
            set
            {
                if (!_matchRunning)
                {
                    _playerTwo.MaxBatStrength = value; 
                }
            }
        }

        public double SpeedPlayerOne
        {
            get { return _playerOne.MaxPlayerSpeed_KmH; }
            set
            {
                if (!_matchRunning)
                {
                    _playerOne.MaxPlayerSpeed_KmH = value; 
                }
            }
        }

        public double SpeedPlayerTwo
        {
            get { return _playerTwo.MaxPlayerSpeed_KmH; }
            set
            {
                if (!_matchRunning)
                {
                    _playerTwo.MaxPlayerSpeed_KmH = value; 
                }
            }
        }

        public TennisMatch()
        {
            _playerOne = new Player();
            _playerTwo = new Player();
            _ball = new Ball();
            _vCourt = new vCourt(_playerOne, _playerTwo, _ball);
        }

        private void Prepare4Rally()
        {
            SetRallyProbs();
            _playerOne.Prepare4Rally(_rallyProbs);
            _playerTwo.Prepare4Rally(_rallyProbs);
        }

        private void SetRallyProbs()
        {
            _rallyProbs = new Rally();
            _rallyProbs.Service = Players.One;
            _rallyProbs.UpperSide = Players.One;
        }
    }
}
