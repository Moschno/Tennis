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
        private Point _vStartingPosLastBat;
        private Point _vFirstTarget;

        public Point vStartingPosLastBat
        {
            get { return _vStartingPosLastBat; }
        }

        public Point vFirstTarget
        {
            get { return _vFirstTarget; }
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

        public override void MoveTo(double vTargetPosX, double vTargetPosY, double time)
        {
            vTargetPos = new Point(vTargetPosX, vTargetPosY);
            double distanceX = vTargetPosX - vActPos.X;
            double distanceY = vTargetPosY - vActPos.Y;
            _speed = time;
            if (_speed > 0)
            {
                _sumAnimationsStart = new AnimationStart[3];
                _sumAnimationsStop = new AnimationPause[3];
                SetMoveAnimation();
                SetSizeChangeAnimation(1.0);
                var sb = (Storyboard)_sumAnimationsStart[2].Target;
                sb.Completed += ((s, e) => MoveTo(vActPos.X + distanceX / 3, vActPos.Y + distanceY / 3, time));
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

        public void GotBated(double vTargetPosX, double vTargetPosY, double batedSpeed_ms)
        {
            vTargetPos = new Point(vTargetPosX, vTargetPosY);
            _vStartingPosLastBat = vActPos;
            _vFirstTarget = vTargetPos;
            double time = this.CalcAnimationTime(vTargetPosX, vTargetPosY, batedSpeed_ms);
            MoveTo(vTargetPosX, vTargetPosY, time);
        }

        public double CalcAnimationTime(double vTargetPosX, double vTargetPosY, double batedSpeed_ms)
        {
            double distanceX = Math.Abs(vTargetPosX - vActPos.X);
            double distanceY = Math.Abs(vTargetPosY - vActPos.Y);
            double distancePixel = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
            double distanceMeter = distancePixel * 10.9728 / 360;
            double time = distanceMeter / batedSpeed_ms;
            return time;
        }

        public double GetKmH(double time, int distance)
        {
            return -1;
        }

        public void SetSizeChangeAnimation(double changeFactor, bool autoreverseOverTime = true) //todo: Nur noch als Pseudo vorhanden
        {
            changeFactor = 1;
            double totalTime;
            var actAnimation = new Storyboard();

            if (autoreverseOverTime)
                totalTime = _speed / 2;
            else
                totalTime = _speed;

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
