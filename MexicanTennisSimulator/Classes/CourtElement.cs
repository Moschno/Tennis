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

        public static readonly DependencyProperty vActPosProperty =
            DependencyProperty.Register("vActPos", typeof(Point), typeof(CourtElement));
        public static readonly DependencyProperty vTargetPosProperty =
            DependencyProperty.Register("vTargetPos", typeof(Point), typeof(CourtElement));
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
        protected AnimationStart[] _sumAnimationsStart;
        protected AnimationPause[] _sumAnimationsStop;
        protected double _speed = 5;

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

        protected delegate void AnimationStart(FrameworkElement element, bool controllable);
        protected delegate void AnimationPause(FrameworkElement element);
        protected abstract void SetColor(Color color);
        public abstract void MoveTo(double targetPosX, double targetPosY, double time);

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
            _sumAnimationsStart = null;
            this.vActPos = vTargetPos;
        }

        protected void StartAnimation()
        {
            AnimationStart startAnimation = (AnimationStart)Delegate.Combine(_sumAnimationsStart);
            startAnimation(_rCourt, true);
        }

        public void PauseAnimation()
        {
            AnimationPause pauseAnimation = (AnimationPause)Delegate.Combine(_sumAnimationsStop);
            pauseAnimation(_rCourt);
        }

        protected void SetMoveAnimation()
        {
            var duration = new Duration(TimeSpan.FromSeconds(_speed));
            var rActPos = Get_rCourtPos(vActPos);
            var rTargetPos = Get_rCourtPos(vTargetPos);
            var actAnimation = new Storyboard();

            var moveRightAnimation = new DoubleAnimation(rActPos.X, rTargetPos.X, duration);
            actAnimation.Children.Add(moveRightAnimation);
            Storyboard.SetTarget(moveRightAnimation, this);
            Storyboard.SetTargetProperty(moveRightAnimation, new PropertyPath(Canvas.LeftProperty));
            _sumAnimationsStart[0] = new AnimationStart(actAnimation.Begin);
            _sumAnimationsStop[0] = new AnimationPause(actAnimation.Pause);

            var moveDownAnimation = new DoubleAnimation(rActPos.Y, rTargetPos.Y, duration);
            actAnimation.Children.Add(moveDownAnimation);
            Storyboard.SetTarget(moveDownAnimation, this);
            Storyboard.SetTargetProperty(moveDownAnimation, new PropertyPath(Canvas.TopProperty));
            _sumAnimationsStart[1] = new AnimationStart(actAnimation.Begin);
            _sumAnimationsStop[1] = new AnimationPause(actAnimation.Pause);
        }

        protected Point Get_rCourtPos(Point vTargetPos)
        {
            var rPos = new Point();
            rPos.X = vTargetPos.X * _rCourt.ActualWidth / _vCourt.Width + _rCourt.ActualWidth / 2;
            rPos.Y = (-1) * vTargetPos.Y * _rCourt.ActualHeight / _vCourt.Height + _rCourt.ActualHeight / 2;

            return rPos;
        }
    }
}
