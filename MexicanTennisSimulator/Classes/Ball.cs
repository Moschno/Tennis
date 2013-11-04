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
        private Point _vGotBatedPos;
        private Point _vFirstTargetPos;
        private Point _batPoint;
        private bool _willBeBated;

        public Point vGotBatedPos
        {
            get { return _vGotBatedPos; }
        }

        public Point vFirstTarget
        {
            get { return _vFirstTargetPos; }
        }

        public Point BatPoint
        {
            get { return _batPoint; }
            set 
            { 
                _batPoint = value;
                if (_batPoint != null)
                {
                    _willBeBated = true; 
                }
            }
        }

        public Ball(ref Canvas rCourt, Color color)
            : base(ref rCourt, color)
        {
            this.StrokeThickness = 10;
            this.SetZIndex(2);
        }

        protected override void SetColor(Color color)
        {
            this.Stroke = new SolidColorBrush(color);
        }

        public void MoveTo(double speed_ms, Iteration it = Iteration.First)
        {
            _posChanged = false;
            double distanceX = vTargetPos.X - vActPos.X;
            double distanceY = vTargetPos.Y - vActPos.Y;
            if (speed_ms > 0)
            {
                if (it == Iteration.First)
                {
                    _vFirstTargetPos = vTargetPos;
                    _vGotBatedPos = vActPos;
                }
                _calcedAnimationTime = CalcAnimationTime(speed_ms);
                _sumAnimationsStart = new AnimationStart[3];
                _sumAnimationsStop = new AnimationPause[3];
                SetMoveAnimation();
                SetSizeChangeAnimation(1.0);
                var sb = (Storyboard)_sumAnimationsStart[2].Target;
                sb.Completed += ((s, e) => this.vActPos = vTargetPos);
                if (speed_ms > 0.1)
                {
                    if (_willBeBated)
                    {
                        int checkValue = CompareDistances(vActPos, vTargetPos, _batPoint);
                        if (checkValue == 1)
                        {
                            
                        }
                    }
                    else
                    {
                        sb.Completed += ((s, e) => distanceX = vActPos.X + distanceX / vCourt.BallSlowDownFactor);
                        sb.Completed += ((s, e) => distanceY = vActPos.Y + distanceY / vCourt.BallSlowDownFactor);
                        sb.Completed += ((s, e) => speed_ms /= vCourt.BallSlowDownFactor);
                        sb.Completed += ((s, e) => vTargetPos = new Point(distanceX, distanceY));
                        sb.Completed += ((s, e) => MoveTo(speed_ms, Iteration.Recursion)); 
                    }
                }
                StartAnimation();
            }
            else
            {
                this.vActPos = vTargetPos;
            }
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

        public void GotBated(double vTargetPosX, double vTargetPosY, double batedSpeed_ms)
        {
            vTargetPos = new Point(vTargetPosX, vTargetPosY);
            _vGotBatedPos = vActPos;
            _vFirstTargetPos = vTargetPos;
            MoveTo(batedSpeed_ms);
        }

        public double GetKmH(double time, int distance)
        {
            return -1;
        }

        public void SetSizeChangeAnimation(double changeFactor, bool autoreverseOverTime = true) //todo: Nur noch als Pseudo vorhanden
        {
            changeFactor = 1;
            double totalTime;

            if (autoreverseOverTime)
                totalTime = _calcedAnimationTime / 2;
            else
                totalTime = _calcedAnimationTime;

            var actAnimation = new Storyboard();
            var sizeChangeAnimation = new DoubleAnimation(
                this.StrokeThickness, this.StrokeThickness * changeFactor, new Duration(
                    TimeSpan.FromSeconds(totalTime)));
            sizeChangeAnimation.AutoReverse = autoreverseOverTime;
            actAnimation.Children.Add(sizeChangeAnimation);
            Storyboard.SetTarget(sizeChangeAnimation, this);
            Storyboard.SetTargetProperty(sizeChangeAnimation, new PropertyPath(Ball.StrokeThicknessProperty));
            _sumAnimationsStart[2] = new AnimationStart(actAnimation.Begin);
            _sumAnimationsStop[2] = new AnimationPause(actAnimation.Pause);
        }
    }
}
