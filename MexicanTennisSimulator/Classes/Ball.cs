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
    class Ball : CourtElement
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

        public override void MoveTo(Point targetPos)
        {
            TargetPos = targetPos;
            if (_speed > 0)
            {
                _sumAnimations = new Animation[4];
                SetMoveAnimation();
                SetSizeChangeAnimation(1.5);
            }
            Go();
            RefreshValues();
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

            var heightAnimation = new DoubleAnimation(
                this.Height, this.Height * changeFactor, new Duration(
                    TimeSpan.FromSeconds(duration)));
            heightAnimation.AutoReverse = autoreverseOverDuration;
            actAnimation.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, this);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Ellipse.HeightProperty));
            _sumAnimations[2] = new Animation(actAnimation.Begin);

            var widthAnimation = new DoubleAnimation(
                this.Width, this.Width * changeFactor, new Duration(
                    TimeSpan.FromSeconds(duration)));
            widthAnimation.AutoReverse = autoreverseOverDuration;
            actAnimation.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, this);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Ellipse.WidthProperty));
            _sumAnimations[3] = new Animation(actAnimation.Begin);
        }
    }
}
