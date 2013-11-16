using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class Set
    {
        private Player _playerOne;
        private Player _playerTwo;
        private bool _setRunning;
        private int _numberGames;
        private bool _tieBreak;
        private List<Rally> _rallys;

        public bool TieBreak
        {
            get { return _tieBreak; }
            set
            {
                if (!_setRunning)
                {
                    _tieBreak = value;
                }
            }
        }

        public Set(ref Player playerOne, ref Player playerTwo)
        {
            _playerOne = playerOne;
            _playerTwo = playerTwo;
        }

        public void StartSet()
        {
            if (!_setRunning)
            {
                _setRunning = true;
                _rallys = new List<Rally>();
                do
                {
                    //Rally rallyTennis = new Rally(ref _playerOne, ref _playerTwo);
                    //_rallys.Add(rallyTennis);
                    //rallyTennis.StartRally();

                    if (true) //Sieger steht fest
                    {
                        _setRunning = false;
                    }
                } while (_setRunning);
            }
        }
    }
}
