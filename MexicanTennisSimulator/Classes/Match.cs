using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class Match
    {
        public static double BallSlowDownFactor = 3;
        public static int vCourtWidth = 720;
        public static int vCourtHeight = 1560;
        public static readonly int BallOutRightX = vCourtWidth / 2 / 36 * 135 / 10 + Ball.BallDiameter / 2;
        public static readonly int BallOutLeftX = -BallOutRightX;
        public static readonly int BallServiceOutRightX = BallOutRightX;
        public static readonly int BallServiceOutLeftX = 0 - Ball.BallDiameter / 2;
        public static readonly int BallOutY = -vCourtHeight / 2 / 2 + Ball.BallDiameter / 2;
        public static readonly int BallServiceOutY = -vCourtHeight / 2 / 39 / 2 * 21 + Ball.BallDiameter / 2;

        private Player _playerOne;
        private Player _playerTwo;
        private int _numberSets;
        private bool _matchRunning;
        private List<Set> _sets;

        public Player PlayerOne
        {
            get { return _playerOne; }
            set
            {
                if (!_matchRunning)
                {
                    _playerOne = value; 
                }
            }
        }

        public Player PlayerTwo
        {
            get { return _playerTwo; }
            set
            {
                if (!_matchRunning)
                {
                    _playerTwo = value; 
                }
            }
        }

        public int NumberSets
        {
            get { return _numberSets; }
            set
            {
                if (!_matchRunning)
                {
                    _numberSets = value; 
                }
            }
        }

        public bool MatchRunning
        {
            get { return _matchRunning; }
        }

        public Match(ref Player playerOne, ref Player playerTwo)
        {
            _playerOne = playerOne;
            _playerTwo = playerTwo;
        }

        public void StartMatch()
        {
            if (!_matchRunning)
            {
                _matchRunning = true;
                _sets = new List<Set>();
                for (int set = 1; set <= _numberSets; set++)
			    {
                    Set setTennis = new Set(ref _playerOne, ref _playerTwo);
			    }
            }
        }

        public void CreateTennisMatch()
        {
        }
    }
}
