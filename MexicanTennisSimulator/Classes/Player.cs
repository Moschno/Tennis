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
        public static readonly double MinGlobalBatSpeed_KmH = 120;
        public static readonly double MaxGlobalBatSpeed_KmH = 170;
        public static readonly double MinGlobalServiceSpeed_KmH = 160;
        public static readonly double MaxGlobalServiceSpeed_KmH = 220;
        public static readonly double MinGlobalMovementSpeed_KmH = 16;
        public static readonly double MaxGlobalMovementSpeed_KmH = 21;
        public static readonly Point ServiceBatPos = new Point(-50, 400);
        public static readonly Point ServiceTakePos = new Point(100, -400);
        public int Strength;
        public int Velocity;
        public int Precision;
        public int Service;
        public int Retörn;
        private sBatProps _tempBatProps;
        public Player MatchOpponent;
        public Ball Gameball;

        public sBatProps BatProbs
        {
            get { return _tempBatProps; }
        }

        public Player(int strength, int velocity, int precision, int service, int retörn) 
            : base()
        {
            Strength = strength;
            Velocity = velocity;
            Precision = precision;
            Service = service;
            Retörn = retörn;
        }

        public sBatProps DoBat(sBatProps batProps)
        {
            _tempBatProps = batProps;
            CalcBatType();
            CalcBatSpeed_KmH();
            CalcBallTargetPos();

            return _tempBatProps;
        }

        private void CalcBatType()
        {
            if (_tempBatProps.BatType == eBatType.FirstService ||
                _tempBatProps.BatType == eBatType.SecondService)
            {
                _tempBatProps.BatPlayerBat = eBats.Service;
            }
            else if (_tempBatProps.BatType == eBatType.Return)
            {
                _tempBatProps.BatPlayerBat = eBats.Return;
            }
            else if (_tempBatProps.BatType == eBatType.Bat)
            {
                _tempBatProps.BatPlayerBat = eBats.Bat;
            }
        }

        private void CalcBallTargetPos()
        {
            Point opponentPos = MatchOpponent.VActPos;
            int negateFactor;
            if (opponentPos.X < 0)
                negateFactor = 1;
            else
                negateFactor = -1;

            if (_tempBatProps.BatType == eBatType.FirstService)
            {
                int safety = 5;
                int disturbFactor = CalcBatTargetDisturb();
                _tempBatProps.vBallTargetPosFromBattingPlayer = new Point((Match.BallOutRightX - safety + disturbFactor)
                                                                         , Match.BallServiceOutY + safety - disturbFactor);
            }
            else if (_tempBatProps.BatType == eBatType.SecondService)
            {
                int safety = 20;
                int disturbFactor = CalcBatTargetDisturb();
                _tempBatProps.vBallTargetPosFromBattingPlayer = new Point((Match.BallOutRightX - safety + disturbFactor)
                                                                         , Match.BallServiceOutY + safety - disturbFactor);
            }
            else if (_tempBatProps.BatType == eBatType.Return)
            {
                int safety = 15;
                int disturbFactor = CalcBatTargetDisturb();
                _tempBatProps.vBallTargetPosFromBattingPlayer = new Point((Match.BallOutRightX - safety + disturbFactor)
                                                                         , Match.BallOutY + safety - disturbFactor);
            }
            else if (_tempBatProps.BatType == eBatType.Bat)
            {
                int safety = 20;
                int disturbFactor = CalcBatTargetDisturb();
                _tempBatProps.vBallTargetPosFromBattingPlayer = new Point((Match.BallOutRightX - safety + disturbFactor)
                                                                         , Match.BallOutY + safety - disturbFactor);
            }
        }

        private int CalcBatTargetDisturb()
        {
            int disturbFactorSpeed = new int();
            int disturbFactor = new int();
            if (_tempBatProps.BatPlayerBat == eBats.Service)
            {
                disturbFactorSpeed = (int)(_tempBatProps.BallSpeedBat / 10);
                disturbFactor = (int)(disturbFactorSpeed * 1.2 - Probability.GetBetterRandomNumber(Service * 3)); 
            }
            else if (_tempBatProps.BatPlayerBat == eBats.Return)
            {
                disturbFactorSpeed = (int)(_tempBatProps.BallSpeedBat / 10);
                disturbFactor = (int)(disturbFactorSpeed * 1.5 - Probability.GetBetterRandomNumber(Retörn * 3));
            }
            else if (_tempBatProps.BatPlayerBat == eBats.Bat)
            {
                disturbFactorSpeed = (int)(_tempBatProps.BallSpeedBat / 10);
                disturbFactor = (int)(disturbFactorSpeed * 2 - Probability.GetBetterRandomNumber(Precision * 3));
            }
            return disturbFactor;
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

        private void CalcBatSpeed_KmH() 
        {
            double interval = 0;
            double batSpeed = 0;
            if (_tempBatProps.BatType == eBatType.FirstService)
            {
                int firstService = Service;
                interval = (MaxGlobalServiceSpeed_KmH - MinGlobalServiceSpeed_KmH) / 10;
                batSpeed = MinGlobalServiceSpeed_KmH + firstService * interval;   
            }
            else if (_tempBatProps.BatType == eBatType.SecondService)
            {
                int factor = Probability.GetBetterRandomNumber(Service);
                int secondService = (int)(Service / (Service - factor));
                interval = (MaxGlobalServiceSpeed_KmH - MinGlobalServiceSpeed_KmH) / 10;
                batSpeed = MinGlobalServiceSpeed_KmH + secondService * interval;   
            }
            else if (_tempBatProps.BatType == eBatType.Return)
            {
                int factor = Probability.GetBetterRandomNumber(Retörn);
                int retörn = (int)(Retörn / (Retörn - factor));
                interval = (MaxGlobalBatSpeed_KmH - MinGlobalBatSpeed_KmH) / 10;
                batSpeed = MinGlobalBatSpeed_KmH + retörn * interval;
            }
            else if (_tempBatProps.BatType == eBatType.Bat)
            {
                int factor = Probability.GetBetterRandomNumber(Strength);
                int strength = (int)(Strength / (Strength - factor));
                interval = (MaxGlobalBatSpeed_KmH - MinGlobalBatSpeed_KmH) / 10;
                batSpeed = MinGlobalBatSpeed_KmH + strength * interval;
            }

            double numerator = (double)Probability.GetBetterRandomNumber(-100, 101);
            double denominator = (double)Probability.GetBetterRandomNumber(10, 51);

            batSpeed += numerator / denominator;
            _tempBatProps.BallSpeedBat = batSpeed;
        }

        public double GetAveragePlayerSpeed_KmH()
        {
            double interval = (MaxGlobalMovementSpeed_KmH - MinGlobalMovementSpeed_KmH) / 10;
            double playerSpeed = MinGlobalMovementSpeed_KmH + Velocity * interval;

            return playerSpeed;
        }

        public double MoveToTargetPos()
        {
            double elapsedTime;
            elapsedTime = Bat.CalcTimeTillTarget(_vActPos, _vTargetPos, GetAveragePlayerSpeed_KmH());
            _vActPos = _vTargetPos;
            return elapsedTime;
        }
    }
}
