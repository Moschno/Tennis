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

        public override void MoveTo(int vTargetPosX, int vTargetPosY, double time)
        {
            if (time < 0 || time > 10)
                throw new ArgumentOutOfRangeException("speed", "Der Wert von 'speed' muss zwischen 0 und 10 liegen. 0 steht für sofortigen Standortwechsel");

            _speed = time;
            vTargetPos = new Point(vTargetPosX, vTargetPosY);
            if (time > 0)
            {
                _sumAnimations = new Animation[3];
                SetMoveAnimation();
                SetSizeChangeAnimation(1.5);
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

        public void GotBated(int vTargetPosX, int vTargetPosY, int strength)
        {
            vTargetPos = new Point(vTargetPosX, vTargetPosY);
            double time = this.CalcAnimationTime(vTargetPosX, vTargetPosY, strength);
            MoveTo(vTargetPosX, vTargetPosY, 0.4);
        }

        public double CalcAnimationTime(int vTargetPosX, int vTargetPosY, int strength)
        {

            return strength;
        }

        public double GetKmH(double time, int distance)
        {
            return -1;
        }

        public void SetSizeChangeAnimation(double changeFactor, bool autoreverseOverTime = true)
        {
            double time = 5;
            double totalTime;
            var actAnimation = new Storyboard();

            if (autoreverseOverTime)
                totalTime = time / 2;
            else
                totalTime = time;

            var sizeChangeAnimation = new DoubleAnimation(
                this.StrokeThickness, this.StrokeThickness * changeFactor, new Duration(
                    TimeSpan.FromSeconds(time)));
            sizeChangeAnimation.AutoReverse = autoreverseOverTime;
            actAnimation.Children.Add(sizeChangeAnimation);
            Storyboard.SetTarget(sizeChangeAnimation, this);
            Storyboard.SetTargetProperty(sizeChangeAnimation, new PropertyPath(Ball.StrokeThicknessProperty));
            _sumAnimations[2] = new Animation(actAnimation.Begin);
        }
    }
}
