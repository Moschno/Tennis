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
        public Player()
            : base()
        {
            var radBrush = new RadialGradientBrush();
            // Create a radial gradient brush with five stops 
            RadialGradientBrush fiveColorRGB = new RadialGradientBrush();
            fiveColorRGB.GradientOrigin = new Point(0.5, 0.5);
            fiveColorRGB.Center = new Point(0.5, 0.5);

            // Create and add Gradient stops
            GradientStop blueGS = new GradientStop();
            blueGS.Color = Colors.Blue;
            blueGS.Offset = 0.0;
            fiveColorRGB.GradientStops.Add(blueGS);

            GradientStop orangeGS = new GradientStop();
            orangeGS.Color = Colors.Orange;
            orangeGS.Offset = 0.25;
            fiveColorRGB.GradientStops.Add(orangeGS);

            GradientStop yellowGS = new GradientStop();
            yellowGS.Color = Colors.Yellow;
            yellowGS.Offset = 0.50;
            fiveColorRGB.GradientStops.Add(yellowGS);

            GradientStop greenGS = new GradientStop();
            greenGS.Color = Colors.Green;
            greenGS.Offset = 0.75;
            fiveColorRGB.GradientStops.Add(greenGS);

            GradientStop redGS = new GradientStop();
            redGS.Color = Colors.Red;
            redGS.Offset = 1.0;
            fiveColorRGB.GradientStops.Add(redGS);

            // Set Fill property of rectangle
            _entity.Fill = fiveColorRGB;
            _entity.Width = 50;
            _entity.Height = 50;
            _entity.StrokeThickness = 10;

            this.Width = 50;
            this.Height = 50;
            this.StrokeThickness = 100;
            this.Stroke = fiveColorRGB;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return (Geometry)new EllipseGeometry();
            }
        }

        public override void MoveTo(Point targetPos)
        {
            TargetPos = targetPos;
            if (_durationTillTarget > 0)
            {
                _sumAnimations = new Animation[2];
                SetMoveAnimation();
            }
            Go();
            RefreshValues();
        }
    }
}
