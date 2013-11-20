using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class Game
    {
		private Player _playerWithService;
		private Player _playerWithoutService;
        private Rally _rally;
        private List<Rally> _rallys;
        private eCourtElements _winner;
        private bool _gameRunning;
        private bool _gameFinished;
        private int _pointsPlayerWithService = 0;
        private int _pointsPlayerWithoutService = 0;

		public Player PlayerWithService
		{
			get
			{
				return _playerWithService;
			}
		}

		public Player PlayerWithoutService
		{
			get
			{
				return _playerWithoutService;
			}
		}

        public List<Rally> Rallys
        {
            get { return _rallys; }
        }

        public eCourtElements Winner
        {
            get { return _winner; }
            private set
            {
                _winner = value;
                _gameFinished = true;
            }
        }

        public int PointsPlayerWithService
        {
            get { return _pointsPlayerWithService; }
        }

        public int PointsPlayerWithoutService
        {
            get { return _pointsPlayerWithoutService; }
        }

        public Game(ref Player playerWithService, ref Player playerWithoutService)
        {
            _playerWithService = playerWithService;
            _playerWithoutService = playerWithoutService;
        }

        public void StartGame()
        {
            if (!_gameRunning && !_gameFinished)
            {
                _gameRunning = true;
                _rallys = new List<Rally>();

                do
                {
                    _rally = new Rally(ref _playerWithService, ref _playerWithoutService);
                    StartAndSaveRally();
                    CalcGameWinner();
                } while (!_gameFinished);

                _gameRunning = false;
            }
        }

        private void StartAndSaveRally()
        {
            _rally.StartRally();
            _rallys.Add(_rally);
        }

        private void CalcGameWinner()
        {
            if (_rally.Winner == eCourtElements.PlayerWithService)
            {
                _pointsPlayerWithService += 1;
            }
            else
            {
                _pointsPlayerWithoutService += 1;
            }

            if (_pointsPlayerWithService >= 4 ||
                _pointsPlayerWithoutService >= 4)
            {
                int difference = Math.Abs(_pointsPlayerWithService - _pointsPlayerWithoutService);

                if (difference >= 2)
                {
                    if (_pointsPlayerWithService > _pointsPlayerWithoutService)
                    {
                        Winner = eCourtElements.PlayerWithService;
                    }
                    else
                    {
                        Winner = eCourtElements.PlayerWithoutService;
                    }
                }
            }
        }
    }
}
