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
        public static double vCourtWidth = 720;
        public static double vCourtHeight = 1560;
        public static readonly double BallOutRightX = vCourtWidth / 2 / 36 * 135 / 10 + Ball.BallDiameter / 2;
        public static readonly double BallOutLeftX = -BallOutRightX;
        public static readonly double BallServiceOutRightX = BallOutRightX;
        public static readonly double BallServiceOutLeftX = 0 - Ball.BallDiameter / 2;
        public static readonly double BallOutY = -vCourtHeight / 2 / 2 + Ball.BallDiameter / 2;
        public static readonly double BallServiceOutY = -vCourtHeight / 2 / 39 / 2 * 21 + Ball.BallDiameter / 2;

		private Player _playerOne;
		private Player _playerTwo;
		private Set _set;
		private List<Set> _sets;
		private eCourtElements _winner;
		private bool _matchRunning;
		private bool _matchFinished;
		private int _numberSets;
		private int _sets4Win;
		private int _setsPlayerOne = 0;
		private int _setsPlayerTwo = 0;

		public List<Set> Sets
		{
			get
			{
				return _sets;
			}
		}

		public eCourtElements Winner
		{
			get
			{
				return _winner;
			}
			private set
			{
				_winner = value;
				_matchFinished = true;
			}
		}

		public int NumberSets
		{
			get
			{
				return _numberSets;
			}
			set
			{
				if (!_matchRunning || 
					!_matchFinished ||
					value % 2 != 0 ||
					value <= 0)
				{
                    _numberSets = value;
                    _sets4Win = (int)(value / 2D + 1D / 2D);
				}
			}
		}

        public Match(ref Player playerWithServiceInFirstGame, ref Player playerWithoutServiceInFirstGame)
        {
            _playerOne = playerWithServiceInFirstGame;
            _playerTwo = playerWithoutServiceInFirstGame;
            _playerOne.MatchOpponent = _playerTwo;
            _playerTwo.MatchOpponent = _playerOne;
			NumberSets = 5;
        }

        public void StartMatch()
        {
            if (!_matchRunning && !_matchFinished)
            {
                _matchRunning = true;
                _sets = new List<Set>();
				do
				{
					if (_sets.Count == 0)
					{
						_set = new Set(ref _playerOne, ref _playerTwo); 
					}
					else
					{
						Player servicePlayerLastGame = _sets[_sets.Count - 1].Games[_sets[_sets.Count - 1].Games.Count - 1].PlayerWithService;
						if (servicePlayerLastGame.Equals(_playerOne))
						{
                            _set = new Set(ref _playerTwo, ref _playerOne);
						}
						else
						{
                            _set = new Set(ref _playerOne, ref _playerTwo); 
						}
					}
					StartAndSaveSet();
					CalcMatchWinner();
				} while (!_matchFinished);

				_matchRunning = false;
            }
        }

		private void CalcMatchWinner()
		{
			if (_set.Winner == eCourtElements.PlayerWithServiceInFirstGame)
			{
                if (_set.PlayerWithServiceInFirstGame.Equals(_playerOne))
                {
                    _setsPlayerOne += 1; 
                }
                else
                {
                    _setsPlayerTwo += 1;
                }
			}
			else
			{
                if (_set.PlayerWithServiceInFirstGame.Equals(_playerOne))
                {
                    _setsPlayerTwo += 1;
                }
                else
                {
                    _setsPlayerOne += 1;
                }
			}

			if (_setsPlayerOne == _sets4Win)
			{
                Winner = eCourtElements.PlayerOne;
			}
			else if (_setsPlayerTwo == _sets4Win)
			{
				Winner = eCourtElements.PlayerTwo;
			}
		}

		private void StartAndSaveSet()
		{
			_set.StartSet();
			_sets.Add(_set);
		}
    }
}
