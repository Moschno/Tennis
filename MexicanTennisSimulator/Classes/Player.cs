﻿using System;
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
        public static readonly double MinGlobalBatSpeed_KmH = 70;
        public static readonly double MaxGlobalBatSpeed_KmH = 130;
        private int _strength;
        private int _velocity;
        private int _precision;
        private sBatProps _batProbs;
        private Player _matchOpponent;
        private Ball _gameball;

        public sBatProps BatProbs
        {
            get { return _batProbs; }
        }

        public Player(int strength, int velocity, int precision) 
            : base()
        {
            _strength = strength;
            _velocity = velocity;
            _precision = precision;
        }

        void Player_OtherPlayerBattedBall()
        {
            TryToReachBallBatPoint();
        }

        public sBatProps DoFirstService()
        {
            int strength = _strength;
            return BatBall(135, -210, strength);
        }

        public void DoSecondService()
        {
            int strength;
            if (_strength > 3)
                strength = 3;
            else
                strength = _strength;

            BatBall(100, -180, strength);
        }

        public void ReturnBall()
        {
            BatBall(100, -180, _strength);
        }

        private void TryToReachBallBatPoint()
        {
            VTargetPos = CalcBallBatPoint();
            //CalcTimeTillTarget(MaxPlayerSpeed_KmH / 3.6);
            //MoveToTargetPos(MaxPlayerSpeed_KmH / 3.6);
        }

        private sBatProps BatBall(double vTargetPosX, double vTargetPosY, int strength)
        {
            double batSpeed_ms = ConvertStrengthToSpeed_ms(strength);
            var vTargetPosBall = new Point(vTargetPosX, vTargetPosY);

            sBatProps batProbs = new sBatProps();
            batProbs.Speed = batSpeed_ms;
            batProbs.vTargetPos = vTargetPosBall;

            return batProbs;
        }

        private Point CalcBallBatPoint()
        {
            double distanceTillFirstLandingX = _gameball.VTargetPos.X - _gameball.VActPos.X;
            double distanceTillFirstLandingY = _gameball.VTargetPos.Y - _gameball.VActPos.Y;

            double distanceFromFirstBatPosX = distanceTillFirstLandingX + distanceTillFirstLandingX / Match.BallSlowDownFactor / 2;
            double distanceFromFirstBatPosY = distanceTillFirstLandingY + distanceTillFirstLandingY / Match.BallSlowDownFactor / 2;

            var ballBatPoint = new Point();
            ballBatPoint.X = _gameball.VActPos.X + distanceFromFirstBatPosX;
            ballBatPoint.Y = _gameball.VActPos.Y + distanceFromFirstBatPosY;

            return ballBatPoint;
        }

        private double ConvertStrengthToSpeed_ms(int strength) //todo: Funktion muss noch angepasst werden.
        {
            double interval = (MaxGlobalBatSpeed_KmH - MinGlobalBatSpeed_KmH) / 10;
            double batSpeed = MinGlobalBatSpeed_KmH / 3.6 + strength * interval;
            return batSpeed;
        }
    }
}
