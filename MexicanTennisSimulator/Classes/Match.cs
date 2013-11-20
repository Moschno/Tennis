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

		private Player _playerWithServiceInFirstGame;
		private Player _playerWithoutServiceInFirstGame;
		private Set _set;
		private List<Set> _sets;
		private eCourtElements _winner;
		private bool _matchRunning;
		private bool _matchFinished;
		private int _numberSets;
		private int _sets4Win;
		private int _setsPlayerWithServiceInFirstGame = 0;
		private int _setsPlayerWithoutServiceInFirstGame = 0;

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
				if (_matchRunning || 
					_matchFinished ||
					value % 2 == 0 ||
					value <= 0)
				{
					throw new Exception();
				}
				else
				{
					_numberSets = value;
					_sets4Win = (int)(value / 2D + 1D / 2D);
				}
			}
		}

        public Match(ref Player playerOne, ref Player playerTwo)
        {
            _playerWithServiceInFirstGame = playerOne;
            _playerWithoutServiceInFirstGame = playerTwo;
			NumberSets = 5;
        }

        public void StartMatch()
        {
            if (!_matchRunning)
            {
                _matchRunning = true;
                _sets = new List<Set>();
				do
				{
					if (_sets.Count == 0)
					{
						_set = new Set(ref _playerWithServiceInFirstGame, ref _playerWithoutServiceInFirstGame); 
					}
					else
					{
						Player servicePlayerLastGame = _sets[_sets.Count - 1].Games[_sets[_sets.Count - 1].Games.Count - 1].PlayerWithService;
						if (servicePlayerLastGame.Equals(_playerWithoutServiceInFirstGame))
						{
							_set = new Set(ref _playerWithServiceInFirstGame, ref _playerWithoutServiceInFirstGame); 
						}
						else
						{
							_set = new Set(ref _playerWithoutServiceInFirstGame, ref _playerWithServiceInFirstGame); 
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
				_setsPlayerWithServiceInFirstGame += 1;
			}
			else
			{
				_setsPlayerWithoutServiceInFirstGame += 1;
			}

			if (_setsPlayerWithServiceInFirstGame == _sets4Win)
			{
				Winner = eCourtElements.PlayerWithServiceInFirstGame;
			}
			else if (_setsPlayerWithoutServiceInFirstGame == _sets4Win)
			{
				Winner = eCourtElements.PlayerWithoutServiceInFirstGame;
			}
		}

		private void StartAndSaveSet()
		{
			_set.StartSet();
			_sets.Add(_set);
		}
    }
}
