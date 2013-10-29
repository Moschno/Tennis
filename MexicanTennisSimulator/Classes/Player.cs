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
        private Ball _gameBall;

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

        public override void MoveTo(int targetPosX, int targetPosY, bool instant = false)
        {
            vTargetPos = new Point(targetPosX, targetPosY);
            if (!instant)
            {
                _sumAnimations = new Animation[2];
                SetMoveAnimation();
                Go();
                _sumAnimations = null;
            }
            else
            {
                var rTargetPos = Get_rCourtTargetPos(vTargetPos);
                this.SetValue(Canvas.LeftProperty, rTargetPos.X);
                this.SetValue(Canvas.TopProperty, rTargetPos.Y);
            }
            this.ActPos = vTargetPos;
        }

        public void Prepare4Rally(Rally rallyProps)
        {
            if (_playerOne)
            {
                _otherPlayer = (Player)_rCourt.Children[12];
                _gameBall = (Ball)_rCourt.Children[10];
                if (rallyProps.Side == RallyProp.UpperFieldPlayerOne)
                    this.MoveTo(-100, 400, true);
                else
                    this.MoveTo(100, -400, true);

                if (rallyProps.Service == RallyProp.ServicePlayerOne)
                    this._gameBall.MoveTo((int)this.ActPos.X, (int)this.ActPos.Y, true);
            }
            else if (_playerTwo)
            {
                _otherPlayer = (Player)_rCourt.Children[11];
                _gameBall = (Ball)_rCourt.Children[10];
                if (rallyProps.Side == RallyProp.UpperFieldPlayerTwo)
                    this.MoveTo(-100, 400, true);
                else
                    this.MoveTo(100, -400, true);

                if (rallyProps.Service == RallyProp.ServicePlayerTwo)
                    this._gameBall.MoveTo((int)this.ActPos.X, (int)this.ActPos.Y, true);
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
            
        }
    }
}
