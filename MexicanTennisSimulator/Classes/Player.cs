using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MexicanTennisSimulator;

namespace MexicanTennisSimulator.Classes
{
    sealed class Player : CourtElement
    {
        private Player _otherPlayer;
        private Ball _ball;
        private bool _gameRunning;
        private bool _service;
        private bool _upperSide;
        private bool _otherPlayerBatBall;
        private int _maxBatStrength = 1;
        private double _minGlobalStdBatSpeed_KmH = 70;
        private double _maxGlobalStdBatSpeed_KmH = 130;

        public int PlayerStrength
        {
            get { return _maxBatStrength; }
            set 
            {
                if (!_gameRunning)
                    _maxBatStrength = value;  
                else
                    throw new InvalidOperationException("Der Wert kann während der Simulation nicht geändert werden.");
            }
        }

        public bool OtherPlayerBatBall
        {
            get { return _otherPlayerBatBall; }
            set 
            { 
                _otherPlayerBatBall = value;
                if (_otherPlayerBatBall)
                {
                    CalcBallBatPoint();
                }
            }
        }

        public Player(ref Canvas rCourt, Color color)
            : base(ref rCourt, color)
        {
            this.StrokeThickness = 40;
            this.SetValue(Panel.ZIndexProperty, 3);
        }

        protected override void SetColor(Color color)
        {
            var radBrush = new RadialGradientBrush();
            // Create a radial gradient brush with five stops 
            RadialGradientBrush fourColorRGB = new RadialGradientBrush();
            fourColorRGB.GradientOrigin = new Point(0.5, 0.5);
            fourColorRGB.Center = new Point(0.5, 0.5);

            // Create and add Gradient stops
            GradientStop customGS = new GradientStop();
            customGS.Color = color;
            customGS.Offset = 0.2;
            fourColorRGB.GradientStops.Add(customGS);

            GradientStop yellowGS = new GradientStop();
            yellowGS.Color = Colors.Yellow;
            yellowGS.Offset = 0.3;
            fourColorRGB.GradientStops.Add(yellowGS);

            GradientStop yellow2GS = new GradientStop();
            yellow2GS.Color = Colors.Yellow;
            yellow2GS.Offset = 0.7;
            fourColorRGB.GradientStops.Add(yellow2GS);

            GradientStop blackGS = new GradientStop();
            blackGS.Color = Colors.Black;
            blackGS.Offset = 1.1;
            fourColorRGB.GradientStops.Add(blackGS);

            this.Stroke = fourColorRGB;
        }

        public override void MoveTo(double targetPosX, double targetPosY, double speed)
        {
            vTargetPos = new Point(targetPosX, targetPosY);
            if (speed > 0)
            {
                _sumAnimationsStart = new AnimationStart[2];
                _sumAnimationsStop = new AnimationPause[2];
                SetMoveAnimation();
                StartAnimation();
            }
            else
            {
                var rTargetPos = Get_rCourtPos(vTargetPos);
                this.SetValue(Canvas.LeftProperty, rTargetPos.X);
                this.SetValue(Canvas.TopProperty, rTargetPos.Y);
            }
            this.vActPos = vTargetPos;
        }

        public void Prepare4Rally(Rally rallyProps)
        {
            if (_playerOne)
            {
                _otherPlayer = (Player)_rCourt.Children[12];
                _ball = (Ball)_rCourt.Children[10];
                if (rallyProps.UpperSide == Players.One)
                {
                    _upperSide = true;
                    this.MoveTo(-100, 400, 0);
                }
                else
                    this.MoveTo(100, -400, 0);

                if (rallyProps.Service == Players.One)
                {
                    _service = true;
                    this._ball.MoveTo(this.vActPos.X + 100, this.vActPos.Y, 0);
                }
            }
            else if (_playerTwo)
            {
                _otherPlayer = (Player)_rCourt.Children[11];
                _ball = (Ball)_rCourt.Children[10];
                if (rallyProps.UpperSide == Players.Two)
                {
                    _upperSide = true;
                    this.MoveTo(-100, 400, 0);
                }
                else
                    this.MoveTo(100, -400, 0);

                if (rallyProps.Service == Players.Two)
                {
                    _service = true;
                    this._ball.MoveTo(this.vActPos.X, this.vActPos.Y, 0);
                }
            }
            else
                throw new Exception("Dies ist kein Spieler auf dem rCourt bzw. es wurden die Kinds-Objekte des rCourt falsch zugeordnet."
                                    + "\n_rCourt.Children.Add[10] -> immer der Spielball"
                                    + "\n_rCourt.Children.Add[11] -> immer Spieler 1"
                                    + "\n_rCourt.Children.Add[12] -> immer Spieler 2"
                                    + "\nJeder weitere hinzugefügte Ball/Spieler wirft diese Exception");
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
                BatBall(100, -210, _maxBatStrength);
            }
            else
            {
                BatBall(-100, 210, _maxBatStrength);
            }
        }

        private void BatBall(double vTargetPosX, double vTargetPosY, int strength)
        {
            double batSpeed_ms = CalcBatSpeed_ms(strength);
            _ball.GotBated(vTargetPosX, vTargetPosY, batSpeed_ms);
            _otherPlayer._otherPlayerBatBall = true;
        }



        private Point CalcBallBatPoint()
        {
            double distanceTillFirstLandingX = _ball.vFirstTarget.X - _ball.vStartingPosLastBat.X;
            double distanceTillFirstLandingY = _ball.vFirstTarget.Y - _ball.vStartingPosLastBat.Y;

            var ballBatPoint = new Point();
            ballBatPoint.X = distanceTillFirstLandingX + distanceTillFirstLandingX / 6;
            ballBatPoint.Y = distanceTillFirstLandingY + distanceTillFirstLandingY / 6;

            return ballBatPoint;
        }

        private double CalcBatSpeed_ms(int strength) //todo: Funktion muss noch angepasst werden.
        {
            double interval = (_maxGlobalStdBatSpeed_KmH - _minGlobalStdBatSpeed_KmH) / 10;
            double batSpeed = _minGlobalStdBatSpeed_KmH / 3.6 + strength * interval;
            return batSpeed;
        }
    }
}
