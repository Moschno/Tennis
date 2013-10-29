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
            DependencyProperty.Register("Color", typeof(Color), typeof(CourtElement), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorChanged)));

        protected static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var myCourtElement = (CourtElement)d;
            myCourtElement.SetColor((Color)e.NewValue);
        }

        protected static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var myCourtElement = (CourtElement)d;
            var target = (Point)e.NewValue;            
            //myCourtElement.MoveTo
        }

        protected bool _playerOne;
        protected bool _playerTwo;
        protected bool _gameBall;
        protected Canvas _rCourt;
        protected Canvas _vCourt;
        protected Animation[] _sumAnimations;
        protected double _speed = 5;

        public Point ActPos
        {
            get { return (Point)GetValue(ActPosProperty); }
            set { SetValue(ActPosProperty, value); }
        }

        public Point vTargetPos
        {
            get { return (Point)GetValue(TargetPosProperty); }
            set { SetValue(TargetPosProperty, value); }
        }

        public Color Color
        {
            get { return (Color)GetValue(ElementColorProperty); }
            set { SetValue(ElementColorProperty, value); }
        }

        protected bool PlayerOne
        {
            get { return _playerOne; }
        }

        protected bool PlayerTwo
        {
            get { return _playerTwo; }
        }

        protected bool GameBall
        {
            get { return _gameBall; }
        }

        protected delegate void Animation();
        protected abstract void SetColor(Color color);
        public abstract void MoveTo(int targetPosX, int targetPosY, bool instant = false);

        protected CourtElement(ref Canvas rCourt, Color color)
        {
            this.Color = color;

            _rCourt = rCourt;
            int ownCourtIndex = _rCourt.Children.Add(this);
            if (ownCourtIndex == 10)
                _gameBall = true;
            else if (ownCourtIndex == 11)
                _playerOne = true;
            else if (ownCourtIndex == 12)
                _playerTwo = true;

            _vCourt = new Canvas();
            _vCourt.Width = 720;
            _vCourt.Height = 1560;
        }

        protected void RefreshValues()
        {
            _sumAnimations = null;
            this.ActPos = vTargetPos;
        }

        protected void Go()
        {
            Animation executeAnimation = (Animation)Delegate.Combine(_sumAnimations);
            executeAnimation();
        }

        protected void SetMoveAnimation()
        {
            var duration = new Duration(TimeSpan.FromSeconds(_speed));
            var rTargetPos = Get_rCourtTargetPos(vTargetPos);
            var actAnimation = new Storyboard();

            var moveRightAnimation = new DoubleAnimation(this.ActPos.X, rTargetPos.X, duration);
            actAnimation.Children.Add(moveRightAnimation);
            Storyboard.SetTarget(moveRightAnimation, this);
            Storyboard.SetTargetProperty(moveRightAnimation, new PropertyPath(Canvas.LeftProperty));
            _sumAnimations[0] = new Animation(actAnimation.Begin);

            var moveDownAnimation = new DoubleAnimation(this.ActPos.Y, rTargetPos.Y, duration);
            actAnimation.Children.Add(moveDownAnimation);
            Storyboard.SetTarget(moveDownAnimation, this);
            Storyboard.SetTargetProperty(moveDownAnimation, new PropertyPath(Canvas.TopProperty));
            _sumAnimations[1] = new Animation(actAnimation.Begin);
        }

        protected Point Get_rCourtTargetPos(Point vTargetPos)
        {
            var rTargetPos = new Point();
            rTargetPos.X = vTargetPos.X * _rCourt.ActualWidth / _vCourt.Width + _rCourt.ActualWidth / 2;
            rTargetPos.Y = (-1) * vTargetPos.Y * _rCourt.ActualHeight / _vCourt.Height + _rCourt.ActualHeight / 2;

            return rTargetPos;
        }
    }
}
