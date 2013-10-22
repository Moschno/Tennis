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
    abstract class Entity : Shape, IEntity
    {
        protected VirtualCourt _vcourt;
        protected Animation[] _sumAnimations;
        protected Ellipse _entity;
        private Brush _color;
        protected double[] _actualPos;
        protected double[] _targetPos;
        protected double _durationTillTarget;

        public Brush Color
        {
            get { return _color; }
            set { _color = value; _entity.Fill = value; }
        }

        protected delegate void Animation();
        public abstract void Move(double durationInSeconds = 0, double[] targetPos = null);

        protected Entity()
        {
            _entity = new Ellipse();
        }

        protected void RefreshValues()
        {
            _durationTillTarget = 0;
            _sumAnimations = null;
            _actualPos = _targetPos;
            _actualPos = _targetPos;
        }

        protected void Go()
        {
            if (_durationTillTarget > 0)
            {
                Animation executeAnimation = (Animation)Delegate.Combine(_sumAnimations);
                executeAnimation();
            }
            else
            {
                _entity.SetLeft(_targetPos[0]);
                _entity.SetTop(_targetPos[1]);
            }  
        }

        protected void SetTarget(double durationInSeconds = 0, double[] targetPos = null)
        {
            _durationTillTarget = durationInSeconds;

            if (targetPos != null)
                _targetPos = targetPos;
        }

        protected void SetMoveAnimation()
        {
            var duration = new Duration(TimeSpan.FromSeconds(_durationTillTarget));
            var actAnimation = new Storyboard();

            var moveRightAnimation = new DoubleAnimation(_actualPos[0], _targetPos[0], duration);
            actAnimation.Children.Add(moveRightAnimation);
            Storyboard.SetTarget(moveRightAnimation, _entity);
            Storyboard.SetTargetProperty(moveRightAnimation, new PropertyPath(Canvas.LeftProperty));
            _sumAnimations[1] = new Animation(actAnimation.Begin);

            var moveDownAnimation = new DoubleAnimation(_actualPos[1], _targetPos[1], duration);
            actAnimation.Children.Add(moveDownAnimation);
            Storyboard.SetTarget(moveDownAnimation, _entity);
            Storyboard.SetTargetProperty(moveDownAnimation, new PropertyPath(Canvas.TopProperty));
            _sumAnimations[0] = new Animation(actAnimation.Begin);
        }
    }
}
