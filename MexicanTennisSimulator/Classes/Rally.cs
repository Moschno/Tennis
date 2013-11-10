using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class Rally
    {
        public event FinishedEventHandler RallyFinished;

        private Player _playerWithService;
        private Player _playerWithoutService;
        private Ball _gameBall;
        private Bat _bat;
        private List<Bat> _bats;
        private eCourtElements _winner;
        private bool _rallyRunning;
        private bool _rallyFinished;

        public List<Bat> Bats
        {
            get { return _bats; }
        }

        private eCourtElements winner
        {
            get { return _winner; }
            set
            {
                _winner = value;
                _rallyRunning = false;
                _rallyFinished = true;
                RallyFinished(this, new FinishedEventArgs(value));
            }
        }

        public eCourtElements Winner
        {
            get { return _winner; }
        }

        public Rally(ref Player playerWithService, ref Player playerWithoutService)
        {
            _playerWithService = playerWithService;
            _playerWithoutService = playerWithoutService;
            _gameBall = new Ball();
            _bats = new List<Bat>();
        }

        public void StartRally()
        {
            if (!_rallyRunning && !_rallyFinished)
            {
                _rallyRunning = true;

                _bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.FirstService);
                StartAndSaveBat();

                if (_bat.WhatHappend == eBatEnding.Let)
                {
                    do
                    {
                        _bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.FirstService);
                        StartAndSaveBat(); 
                    } while (_bat.WhatHappend == eBatEnding.Let);
                }
                else if (_bat.WhatHappend == eBatEnding.BallIsOut)
                {
                    _bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.SecondService);
                    StartAndSaveBat();

                    if (_bat.WhatHappend == eBatEnding.Let)
                    {
                        do
                        {
                            _bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.SecondService);
                            StartAndSaveBat();
                        } while (_bat.WhatHappend == eBatEnding.Let);
                    }
                }

                if (_bat.WhatHappend == eBatEnding.BallIsReturned)
                {
                    _bat = new Bat(ref _playerWithoutService, ref _playerWithService, ref _gameBall, eBatBeginning.Return);
                    StartAndSaveBat();
                    if (_bat.WhatHappend == eBatEnding.BallIsNotReturned)
                    {
                        winner = eCourtElements.PlayerWithoutService;
                        return;
                    }
                }

                if (_bat.WhatHappend == eBatEnding.Ace ||
                    _bat.WhatHappend == eBatEnding.BallIsNotReturned ||
                    _bat.WhatHappend == eBatEnding.BallIsOut)
                {
                    winner = eCourtElements.PlayerWithService;
                    return;
                }

                bool servicePlayerHasToBatBall = true;
                if (_bat.WhatHappend != eBatEnding.BallIsBroken)
                {
                    do
                    {
                        if (servicePlayerHasToBatBall)
                        {
                            _bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.Bat);
                            servicePlayerHasToBatBall = false;
                        }
                        else
                        {
                            _bat = new Bat(ref _playerWithoutService, ref _playerWithService, ref _gameBall, eBatBeginning.Bat);
                            servicePlayerHasToBatBall = true;
                        }
                        StartAndSaveBat();
                    } while (_bat.WhatHappend == eBatEnding.BallIsReturned); 
                }

                if (_bat.WhatHappend == eBatEnding.BallIsBroken)
                {
                    _rallyRunning = false;
                    StartRally();
                    return;
                }

                if (servicePlayerHasToBatBall)
                {
                    winner = eCourtElements.PlayerWithService;
                }
                else
                {
                    winner = eCourtElements.PlayerWithoutService;
                } 
            }
        }

        private void StartAndSaveBat()
        {
            _bat.StartBat();
            _bats.Add(_bat);
        }
    }
}
