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
            this.StrokeThickness = 8;
            this.SetZIndex(2);
        }

        protected override void SetColor(Color color)
        {
            this.Stroke = new SolidColorBrush(color);
        }

        public override void MoveTo(int vTargetPosX, int vTargetPosY, bool instant = false)
        {
            vTargetPos = new Point(vTargetPosX, vTargetPosY);
            if (!instant)
            {
                _sumAnimations = new Animation[3];
                SetMoveAnimation();
                SetSizeChangeAnimation(1.5);
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

        public void SetSizeChangeAnimation(double changeFactor, bool autoreverseOverDuration = true)
        {
            double duration = 5;
            double totalDuration;
            var actAnimation = new Storyboard();

            if (autoreverseOverDuration)
                totalDuration = duration / 2;
            else
                totalDuration = duration;

            var sizeChangeAnimation = new DoubleAnimation(
                this.StrokeThickness, this.StrokeThickness * changeFactor, new Duration(
                    TimeSpan.FromSeconds(duration)));
            sizeChangeAnimation.AutoReverse = autoreverseOverDuration;
            actAnimation.Children.Add(sizeChangeAnimation);
            Storyboard.SetTarget(sizeChangeAnimation, this);
            Storyboard.SetTargetProperty(sizeChangeAnimation, new PropertyPath(Ball.StrokeThicknessProperty));
            _sumAnimations[2] = new Animation(actAnimation.Begin);
        }
    }
}
