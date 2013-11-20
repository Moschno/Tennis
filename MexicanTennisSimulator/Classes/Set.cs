using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class Set
    {
		private Player _playerWithServiceInFirstGame;
		private Player _playerWithoutServiceInFirstGame;
        private Game _game;
        private List<Game> _games;
        private eCourtElements _winner;
        private bool _setRunning;
        private bool _setFinished;
        private int _games4Win;
        private int _gamesPlayerWithServiceInFirstGame = 0;
        private int _gamesPlayerWithoutServiceInFirstGame = 0;

		public Player PlayerWithServiceInFirstGame
		{
			get
			{
				return _playerWithServiceInFirstGame;
			}
		}

		public Player PlayerWithoutServiceInFirstGame
		{
			get
			{
				return _playerWithoutServiceInFirstGame;
			}
		}

        public List<Game> Games
        {
            get { return _games; }
        }

        public eCourtElements Winner
        {
            get { return _winner; }
            private set 
            { 
                _winner = value;
                _setFinished = true;
            }
        }

        public int Games4Win
        {
            get { return _games4Win; }
            set 
            {
                if (_setRunning || _setFinished)
                {
                    throw new Exception();
                }
                else
                {
                    _games4Win = value; 
                }
            }
        }

        public Set(ref Player playerWithServiceInFirstGame, ref Player playerWithoutServiceInFirstGame)
        {
            _playerWithServiceInFirstGame = playerWithServiceInFirstGame;
            _playerWithoutServiceInFirstGame = playerWithoutServiceInFirstGame;
            _games4Win = 6;
        }

        public void StartSet()
        {
            if (!_setRunning && !_setFinished)
            {
                _setRunning = true;
                _games = new List<Game>();

                do
                {
                    if (_games.Count % 2 == 0)
	                {
                        _game = new Game(ref _playerWithServiceInFirstGame, ref _playerWithoutServiceInFirstGame);
	                }
                    else
                    {
                        _game = new Game(ref _playerWithoutServiceInFirstGame, ref _playerWithServiceInFirstGame);
                    }
                    StartAndSaveGame();
                    CalcSetWinner();
                } while (!_setFinished);

                _setRunning = false;
            }
        }

        private void StartAndSaveGame()
        {
            _game.StartGame();
            _games.Add(_game);
        }

        private void CalcSetWinner()
        {
            if (_game.Winner == eCourtElements.PlayerWithService)
            {
                if (_games.Count % 2 == 0)
                {
                    _gamesPlayerWithServiceInFirstGame += 1;
                }
                else
                {
                    _gamesPlayerWithoutServiceInFirstGame += 1;
                }
            }
            else
            {
                if (_games.Count % 2 == 0)
                {
                    _gamesPlayerWithoutServiceInFirstGame += 1;
                }
                else
                {
                    _gamesPlayerWithServiceInFirstGame += 1;
                }
            }

            if (_gamesPlayerWithServiceInFirstGame >= _games4Win ||
                _gamesPlayerWithoutServiceInFirstGame >= _games4Win)
            {
                int difference = Math.Abs(_gamesPlayerWithServiceInFirstGame - _gamesPlayerWithoutServiceInFirstGame);

                if (difference >= 2)
                {
                    if (_gamesPlayerWithServiceInFirstGame > _gamesPlayerWithoutServiceInFirstGame)
                    {
                        Winner = eCourtElements.PlayerWithServiceInFirstGame;
                    }
                    else
                    {
                        Winner = eCourtElements.PlayerWithoutServiceInFirstGame;
                    }
                }
            }
        }
    }
}
