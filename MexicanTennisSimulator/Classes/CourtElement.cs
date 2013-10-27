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

namespace MexicanTennisSimulator.Classes
{
    abstract class CourtElement : Shape
    {
        protected override Geometry DefiningGeometry
        {
            get
            {
                return (Geometry)new EllipseGeometry();
            }
        }

        public static readonly DependencyProperty ActPosProperty =
            DependencyProperty.Register("ActPos", typeof(Point), typeof(CourtElement));
        public static readonly DependencyProperty TargetPosProperty =
            DependencyProperty.Register("TargetPos", typeof(Point), typeof(CourtElement));
        public static readonly DependencyProperty ElementColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(Player)
                , new PropertyMetadata(), ValidateValueCallback(SetColor

        protected Canvas _rCourt;
        protected Canvas _vCourt;
        protected Animation[] _sumAnimations;
        protected double _speed = 5;

        public Point ActPos
        {
            get { return (Point)GetValue(ActPosProperty); }
            set { SetValue(ActPosProperty, value); }
        }

        public Point TargetPos
        {
            get { return (Point)GetValue(TargetPosProperty); }
            set { SetValue(TargetPosProperty, value); }
        }

        public Color Color
        {
            get { return (Color)GetValue(ElementColorProperty); }
            set { SetValue(ElementColorProperty, value); }
        }

        protected delegate void Animation();
        protected abstract void SetColor(Color color);
        public abstract void MoveTo(Point targetPos);

        protected CourtElement(ref Canvas rCourt, Color color)
        {

            this.Color = color;
            //SetColor(this.Color);

            _rCourt = rCourt;
            _rCourt.Children.Add(this);

            _vCourt = new Canvas();
            _vCourt.Width = 720;
            _vCourt.Height = 1560;
        }

        protected void RefreshValues()
        {
            _sumAnimations = null;
            this.ActPos = TargetPos;
        }

        protected void Go()
        {
            if (_speed > 0)
            {
                Animation executeAnimation = (Animation)Delegate.Combine(_sumAnimations);
                executeAnimation();
            }
            else
            {
                this.SetLeft(TargetPos.X);
                this.SetTop(TargetPos.Y);
                this.ActPos = TargetPos;
            }  
        }

        protected void SetMoveAnimation()
        {
            var duration = new Duration(TimeSpan.FromSeconds(_speed));
            var rCourtTargetPos = new Point();
            rCourtTargetPos.X = TargetPos.X * _rCourt.ActualWidth / _vCourt.Width;
            rCourtTargetPos.Y = TargetPos.Y * _rCourt.ActualHeight / _vCourt.Height;
            var actAnimation = new Storyboard();

            var moveRightAnimation = new DoubleAnimation(this.ActPos.X, rCourtTargetPos.X, duration);
            actAnimation.Children.Add(moveRightAnimation);
            Storyboard.SetTarget(moveRightAnimation, this);
            Storyboard.SetTargetProperty(moveRightAnimation, new PropertyPath(Canvas.LeftProperty));
            _sumAnimations[0] = new Animation(actAnimation.Begin);

            var moveDownAnimation = new DoubleAnimation(this.ActPos.Y, rCourtTargetPos.Y, duration);
            actAnimation.Children.Add(moveDownAnimation);
            Storyboard.SetTarget(moveDownAnimation, this);
            Storyboard.SetTargetProperty(moveDownAnimation, new PropertyPath(Canvas.TopProperty));
            _sumAnimations[1] = new Animation(actAnimation.Begin);
        }
    }
}
