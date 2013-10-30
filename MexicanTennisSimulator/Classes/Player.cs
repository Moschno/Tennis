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
        private bool _service;
        private bool _upperSide;

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

        public override void MoveTo(int targetPosX, int targetPosY, double speed)
        {
            vTargetPos = new Point(targetPosX, targetPosY);
            if (speed > 0)
            {
                _sumAnimations = new Animation[2];
                SetMoveAnimation();
                Go();
                _sumAnimations = null;
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
                    this._ball.MoveTo((int)this.vActPos.X, (int)this.vActPos.Y, 0);
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
                    this._ball.MoveTo((int)this.vActPos.X, (int)this.vActPos.Y, 0);
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
                this.DoService();
            }
        }

        private void DoService()
        {
            if (_upperSide)
            {
                BatBall(-135, -390, 1);
            }
            else
            {
                BatBall(-135, 390, 1);
            }
        }

        private void BatBall(int vTargetPosX, int vTargetPosY, int strength)
        {
            _ball.GotBated(vTargetPosX, vTargetPosY, strength);
        }
    }
}
