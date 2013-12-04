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
using System.Runtime.InteropServices;

namespace MexicanTennisSimulator.Classes
{
    sealed class Ball : CourtElement
    {
        public const int BallDiameter = 10;
        private Point _firstLandingPos;
        private Player _lastBatPlayer;

        public Point FirstLandingPos
        {
            get { return _firstLandingPos; }
        }

        public Player LastBatPlayer
        {
            get { return _lastBatPlayer; }
        }

        public Ball()
            : base()
        {
        }

        public void GotBatted(Player ballBatter, double speed_ms)
        {
            //MoveToTargetPos(speed_ms);
        }

        private int CompareDistances(Point startPos, Point targetPos1, Point targetPos2)
        {
            double distance1X = Math.Abs(startPos.X - targetPos1.X);
            double distance1Y = Math.Abs(startPos.Y - targetPos1.Y);
            double distance2X = Math.Abs(startPos.X - targetPos2.X);
            double distance2Y = Math.Abs(startPos.Y - targetPos2.Y);

            double distance1 = Math.Sqrt(distance1X * distance1X + distance1Y * distance1Y);
            double distance2 = Math.Sqrt(distance2X * distance2X + distance2Y * distance2Y);

            if (distance1 > distance2)
                return 1;
            else if (distance2 > distance1)
                return 2;
            else
                return -1;
        }
    }
}
