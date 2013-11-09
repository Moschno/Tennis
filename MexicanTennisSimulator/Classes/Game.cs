using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class Game
    {
        public event FinishedEventHandler GameFinished;

        private Player _playerWithService;
        private Player _playerWithoutService;
        private eCourtElements _winner;
        private bool _gameRunning;

        public eCourtElements Winner
        {
            get { return _winner; }
            set
            {
                _winner = value;
                GameFinished(this, new FinishedEventArgs(value));
            }
        }

        public Game(ref Player playerWithService, ref Player playerWithoutService)
        {
            _playerWithService = playerWithService;
            _playerWithoutService = playerWithoutService;
        }

        public void StartGame()
        {
            if (!_gameRunning && Winner == eCourtElements.Default)
            {
                _gameRunning = true;

            }
        }
    }
}
