using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MexicanTennisSimulator;
using System.Windows.Media.Animation;

namespace MexicanTennisSimulator.Classes
{
    sealed class Player : CourtElement
    {
        private Player _otherPlayer;
        private Ball _ball;
        private bool _gameRunning;
        private bool _service;
        private bool _upperSide;
        public int MaxBatStrength = 1;
        private double _minGlobalBatSpeed_KmH = 70;
        private double _maxGlobalBatSpeed_KmH = 130;
        public double MaxPlayerSpeed_KmH = 20;

        public void MoveToTargetPos(double speed_ms)
        {
           
        }

        public void Prepare4Rally(Rally rallyProps)
        {
            if (_playerOne)
            {
                if (rallyProps.UpperSide == Players.One)
                {
                    _upperSide = true;
                    vTargetPos = new Point(-100, 400);
                    this.MoveToTargetPos(0);
                }
                else
                    vTargetPos = new Point(100, -400);
                    this.MoveToTargetPos(0);

                if (rallyProps.Service == Players.One)
                {
                    _service = true;
                    this._ball.vTargetPos = vActPos;
                    this._ball.MoveToTargetPos(0);
                }
            }
            else if (_playerTwo)
            {
                if (rallyProps.UpperSide == Players.Two)
                {
                    _upperSide = true;
                    vTargetPos = new Point(-100, 400);
                    this.MoveToTargetPos(0);
                }
                else
                {
                    vTargetPos = new Point(100, -400);
                    this.MoveToTargetPos(0);
                }

                if (rallyProps.Service == Players.Two)
                {
                    _service = true;
                    this._ball.vTargetPos = vActPos;
                    this._ball.MoveToTargetPos(0);
                }
            }
        }

        public void StartRally()
        {
            if (_service)
            {
                DoService();
            }
        }

        private void DoService()
        {
            if (_upperSide)
            {
                BatBall(100, -210, MaxBatStrength);
            }
            else
            {
                BatBall(-100, 210, MaxBatStrength);
            }

        }

        public void ReturnBall()
        {
            if (_upperSide)
            {
                BatBall(100, -310, MaxBatStrength);
                vTargetPos = new Point(0, 400);
                MoveToTargetPos(MaxPlayerSpeed_KmH / 3.6);
            }
            else
            {
                BatBall(-115, 330, MaxBatStrength);
                vTargetPos = new Point(0, -400);
                MoveToTargetPos(MaxPlayerSpeed_KmH / 3.6);
            }
        }

        public void OtherPlayerBatBall()
        {
            TryToReachBallBatPoint();
        }

        private void TryToReachBallBatPoint()
        {
            vTargetPos = CalcBallBatPoint();
            CalcTimeTillTarget(MaxPlayerSpeed_KmH / 3.6);
            MoveToTargetPos(MaxPlayerSpeed_KmH / 3.6);
        }

        private void BatBall(double vTargetPosX, double vTargetPosY, int strength)
        {
            double batSpeed_ms = ConvertStrengthToSpeed_ms(strength);
            _ball.GotBated(vTargetPosX, vTargetPosY, batSpeed_ms, this);
            _otherPlayer.OtherPlayerBatBall();
        }

        private Point CalcBallBatPoint()
        {
            double distanceTillFirstLandingX = _ball.vFirstTarget.X - _ball.vGotBatedPos.X;
            double distanceTillFirstLandingY = _ball.vFirstTarget.Y - _ball.vGotBatedPos.Y;

            double distanceFromFirstBatPosX = distanceTillFirstLandingX + distanceTillFirstLandingX / vCourt.BallSlowDownFactor / 2;
            double distanceFromFirstBatPosY = distanceTillFirstLandingY + distanceTillFirstLandingY / vCourt.BallSlowDownFactor / 2;

            var ballBatPoint = new Point();
            ballBatPoint.X = _ball.vGotBatedPos.X + distanceFromFirstBatPosX;
            ballBatPoint.Y = _ball.vGotBatedPos.Y + distanceFromFirstBatPosY;

            return ballBatPoint;
        }

        private double ConvertStrengthToSpeed_ms(int strength) //todo: Funktion muss noch angepasst werden.
        {
            double interval = (_maxGlobalBatSpeed_KmH - _minGlobalBatSpeed_KmH) / 10;
            double batSpeed = _minGlobalBatSpeed_KmH / 3.6 + strength * interval;
            return batSpeed;
        }
    }
}
