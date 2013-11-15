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
        public static readonly double MinGlobalBatSpeed_KmH = 70;
        public static readonly double MaxGlobalBatSpeed_KmH = 130;
        public static readonly double MinGlobalServiceSpeed_KmH = 160;
        public static readonly double MaxGlobalServiceSpeed_KmH = 220;
        public static readonly double MinGlobalMovementSpeed_KmH = 18;
        public static readonly double MaxGlobalMovementSpeed_KmH = 23;
        public static readonly Point ServiceBatPos = new Point(-50, 400);
        public static readonly Point ServiceTakePos = new Point(100, -400);
        public int Strength;
        public int Velocity;
        public int Precision;
        public int Service;
        private sBatProps _tempBatProps;
        public Player MatchOpponent;
        public Ball Gameball;

        public sBatProps BatProbs
        {
            get { return _tempBatProps; }
        }

        public Player(int strength, int velocity, int precision) 
            : base()
        {
            Strength = strength;
            Velocity = velocity;
            Precision = precision;
        }

        public sBatProps DoBat(sBatProps batProps)
        {
            _tempBatProps = batProps;
            CalcBallTargetPos();
            CalcBallSpeed();

            return _tempBatProps;
        }

        private void CalcBallTargetPos()
        {
            Point opponentPos = MatchOpponent.VActPos;
            int negateFactor;
            if (opponentPos.X < 0)
                negateFactor = 1;
            else
                negateFactor = -1;

            if (_tempBatProps.BatType == eBatType.FirstService ||
                _tempBatProps.BatType == eBatType.SecondService)
            {
                _tempBatProps.BatPlayerBat = eBats.Service;
                _tempBatProps.vBallTargetPosFromBattingPlayer = new Point((Match.BallOutRightX - 10), Match.BallServiceOutY - 10);
            }
            else if (_tempBatProps.BatType == eBatType.Return)
            {
                _tempBatProps.BatPlayerBat = eBats.Return;
                _tempBatProps.vBallTargetPosFromBattingPlayer = new Point((Match.BallOutRightX - 80) * negateFactor, Match.BallOutY + 50);
            }
            else if (_tempBatProps.BatType == eBatType.Bat)
            {
                _tempBatProps.BatPlayerBat = eBats.Bat;
                _tempBatProps.vBallTargetPosFromBattingPlayer = new Point((Match.BallOutRightX - 80) * negateFactor, Match.BallOutY + 10); 
            }
        }

        private void CalcBallSpeed()
        {
            _tempBatProps.BallSpeedFromBattingPlayer = ConvertStrengthToSpeed_ms(Strength);
        }

        private Point CalcBallBatPoint()
        {
            double distanceTillFirstLandingX = Gameball.VTargetPos.X - Gameball.VActPos.X;
            double distanceTillFirstLandingY = Gameball.VTargetPos.Y - Gameball.VActPos.Y;

            double distanceFromFirstBatPosX = distanceTillFirstLandingX + distanceTillFirstLandingX / Match.BallSlowDownFactor / 2;
            double distanceFromFirstBatPosY = distanceTillFirstLandingY + distanceTillFirstLandingY / Match.BallSlowDownFactor / 2;

            var ballBatPoint = new Point();
            ballBatPoint.X = Gameball.VActPos.X + distanceFromFirstBatPosX;
            ballBatPoint.Y = Gameball.VActPos.Y + distanceFromFirstBatPosY;

            return ballBatPoint;
        }

        private double ConvertStrengthToSpeed_ms(int strength) //todo: Funktion muss noch angepasst werden.
        {
            double interval = (MaxGlobalBatSpeed_KmH - MinGlobalBatSpeed_KmH) / 10;
            double batSpeed = MinGlobalBatSpeed_KmH + strength * interval;
            return batSpeed;
        }
    }
}
