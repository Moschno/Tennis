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
        }

        public void StartRally()
        {
            _bats = new List<Bat>();
            if (!_rallyRunning && !_rallyFinished)
            {
                _rallyRunning = true;
                Bat.BatFinished += bat_BatFinished;
                Bat.FirstServiceFinished += Bat_FirstServiceFinished;
                Bat.SecondServiceFinished += Bat_SecondServiceFinished;

                var bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.FirstService);
                bat.StartBat();
            }
        }

        void Bat_FirstServiceFinished(object sender, BatFinishedEventArgs e)
        {
            _bats.Add((Bat)sender);
            Bat bat;
            switch (e.WhatHappend)
            {
                case eBatEnding.BallIsOut:
                    _gameBall = new Ball();
                    bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.SecondService);
                    bat.StartBat();
                    break;
                case eBatEnding.BallIsReturned:
                    bat = new Bat(ref _playerWithoutService, ref _playerWithService, ref _gameBall, eBatBeginning.Return);
                    bat.StartBat();
                    break;
                case eBatEnding.BallIsBroken:
                    _gameBall = new Ball();
                    bat = new Bat(ref _playerWithService, ref _playerWithoutService, ref _gameBall, eBatBeginning.FirstService);
                    bat.StartBat();
                    break;
                case eBatEnding.BallIsNotReturned:
                    winner = eCourtElements.PlayerWithService;
                    break;
                case eBatEnding.Ace:
                    winner = eCourtElements.PlayerWithService;
                    break;
            }
        }

        void Bat_SecondServiceFinished(object sender, BatFinishedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void bat_BatFinished(object sender, BatFinishedEventArgs e)
        {
            System.Windows.MessageBox.Show("BatFinished");
        }
    }
}
