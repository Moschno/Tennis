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
    class Ball : Entity
    {
        public Ball()
            : base()
        {
            Color = Brushes.Yellow;
            _entity.Fill = Color;
            _entity.Width = 8;
            _entity.Height = 8;
        }

        public override void Move(double durationInSeconds = 0, double[] targetPos = null)
        {
            SetTarget(durationInSeconds, targetPos);
            if (_durationTillTarget > 0)
            {
                _sumAnimations = new Animation[4];
                SetMoveAnimation();
                SetSizeChangeAnimation(1.5);
            }
            Go();
            RefreshValues();
        }

        private void SetSizeChangeAnimation(double multiplier, bool autoreverseOverDuration = true)
        {
            double duration;
            var actAnimation = new Storyboard();

            if (autoreverseOverDuration)
                duration = _durationTillTarget / 2;
            else
                duration = _durationTillTarget;

            var heightAnimation = new DoubleAnimation(
                _entity.Height, _entity.Height * multiplier, new Duration(
                    TimeSpan.FromSeconds(duration)));
            heightAnimation.AutoReverse = autoreverseOverDuration;
            actAnimation.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, _entity);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Ellipse.HeightProperty));
            _sumAnimations[2] = new Animation(actAnimation.Begin);

            var widthAnimation = new DoubleAnimation(
                _entity.Width, _entity.Width * multiplier, new Duration(
                    TimeSpan.FromSeconds(duration)));
            widthAnimation.AutoReverse = autoreverseOverDuration;
            actAnimation.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, _entity);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Ellipse.WidthProperty));
            _sumAnimations[3] = new Animation(actAnimation.Begin);
        }
    }
}
