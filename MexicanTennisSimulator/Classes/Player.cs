using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MexicanTennisSimulator.Classes
{
    class Player : CourtElement
    {
        Player _otherPlayer;
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

        public override void MoveTo(Point targetPos)
        {
            TargetPos = targetPos;
            if (_speed > 0)
            {
                _sumAnimations = new Animation[2];
                SetMoveAnimation();
            }
            Go();
            RefreshValues();
        }

        public override void StartGame()
        {
            if (_ownCourtIndex == 11)
                _otherPlayer = (Player)_rCourt.Children[12];
            else if (_ownCourtIndex == 12)
                _otherPlayer = (Player)_rCourt.Children[11];
            else
                throw new Exception("Es wurden die Kinds-Objekte des rCourt falsch zugeordnet."
                                    + "\n_rCourt.Children.Add[10] -> immer der Spielball"
                                    + "\n_rCourt.Children.Add[11] -> immer Spieler 1"
                                    + "\n_rCourt.Children.Add[12] -> immer Spieler 2"
                                    + "\nJeder weitere hinzugefügte Ball/Spieler wirft diese Exception");
        }
    }
}
