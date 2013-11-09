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
                _bat.StartBat();
                _bats.Add(_bat);

                if (_bat.WhatHappend == eBatEnding.Let)
                {
                    do
                    {
                        _bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.FirstService);
                        _bat.StartBat();
                        _bats.Add(_bat); 
                    } while (_bat.WhatHappend == eBatEnding.Let);
                }
                else if (_bat.WhatHappend == eBatEnding.BallIsOut)
                {
                    _bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.SecondService);
                    _bat.StartBat();
                    _bats.Add(_bat);

                    if (_bat.WhatHappend == eBatEnding.Let)
                    {
                        do
                        {
                            _bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.SecondService);
                            _bat.StartBat();
                            _bats.Add(_bat);
                        } while (_bat.WhatHappend == eBatEnding.Let);
                    }
                }

                if (_bat.WhatHappend == eBatEnding.Ace ||
                    _bat.WhatHappend == eBatEnding.BallIsNotReturned)
                {
                    winner = eCourtElements.PlayerWithService;
                    return;
                }
                else
                {
                    bool servicePlayerBatBall = false;
                    if (_bat.WhatHappend != eBatEnding.BallIsBroken)
                    {
                        do
                        {
                            if (servicePlayerBatBall)
                            {
                                _bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.Return);
                                servicePlayerBatBall = false;
                            }
                            else
                            {
                                _bat = new Bat(ref _playerWithoutService, ref _playerWithService, ref _gameBall, eBatBeginning.Return);
                                servicePlayerBatBall = true;
                            }

                            _bat.StartBat();
                            _bats.Add(_bat);
                        } while (_bat.WhatHappend == eBatEnding.BallIsReturned); 
                    }

                    if (_bat.WhatHappend == eBatEnding.BallIsBroken)
                    {
                        _rallyRunning = false;
                        StartRally();
                        return;
                    }

                    if (servicePlayerBatBall)
                    {
                        winner = eCourtElements.PlayerWithService
                    }
                    else
                    {
                        winner = eCourtElements.PlayerWithoutService;
                    } 
                }
            }
        }
    }
}
