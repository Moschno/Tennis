using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using System.ComponentModel;

namespace MexicanTennisSimulator.Classes
{
    abstract class CourtElement : FrameworkElement
    {

        public static readonly DependencyProperty vActPosProperty =
            DependencyProperty.Register("vActPos", typeof(Point), typeof(CourtElement));
        public static readonly DependencyProperty vTargetPosProperty =
            DependencyProperty.Register("vTargetPos", typeof(Point), typeof(CourtElement));

        public Point vActPos
        {
            get { return (Point)GetValue(vActPosProperty); }
            set { SetValue(vActPosProperty, value); }
        }

        public Point vTargetPos
        {
            get { return (Point)GetValue(vTargetPosProperty); }
            set { SetValue(vTargetPosProperty, value); }
        }

        private abstract void SetColor(Color color);

        protected CourtElement()
        {
            
        }

        protected double CalcTimeTillTarget(double speed_ms)
        {
            double distanceX = Math.Abs(vTargetPos.X - vActPos.X);
            double distanceY = Math.Abs(vTargetPos.Y - vActPos.Y);
            double distancePixel = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
            double distanceMeter = distancePixel * 10.9728 / 360;
            double time = distanceMeter / speed_ms;
            return time;
        }
    }
}
