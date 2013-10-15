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
    abstract class Entity : IEntity
    {
        protected Canvas _panel;
        protected Animation[] _sumAnimations;
        protected Ellipse _entity;
        private Brush _color;
        protected double _actualPosX;
        protected double _actualPosY;
        protected double _targetPosX;
        protected double _targetPosY;
        protected double _durationTillTarget;

        public Brush Color
        {
            get { return _color; }
            set { _color = value; _entity.Fill = value; }
        }

        protected delegate void Animation();
        public abstract void Move(double durationInSeconds = 0, double? targetPosX = null, double? targetPosY = null);

        protected Entity(ref Canvas canvas, double actualPosX, double actualPosY)
        {
            _entity = new Ellipse();
            _panel = canvas;
            _panel.Children.Add(_entity);

            _actualPosX = actualPosX;
            _actualPosY = actualPosY;
            _entity.SetLeft(actualPosX);
            _entity.SetTop(actualPosY);
        }

        protected void RefreshValues()
        {
            _durationTillTarget = 0;
            _sumAnimations = null;
            _actualPosX = _targetPosX;
            _actualPosY = _targetPosY;
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
                _entity.SetLeft(_targetPosX);
                _entity.SetTop(_targetPosY);
            }  
        }

        protected void SetTarget(double durationInSeconds = 0, double? targetPosX = null, double? targetPosY = null)
        {
            _durationTillTarget = durationInSeconds;

            if (targetPosX != null)
                _targetPosX = (double)targetPosX;

            if (targetPosY != null)
                _targetPosY = (double)targetPosY;
        }

        protected void SetMoveAnimation()
        {
            var duration = new Duration(TimeSpan.FromSeconds(_durationTillTarget));
            var actAnimation = new Storyboard();

            var moveDownAnimation = new DoubleAnimation(_actualPosY, _targetPosY, duration);
            actAnimation.Children.Add(moveDownAnimation);
            Storyboard.SetTarget(moveDownAnimation, _entity);
            Storyboard.SetTargetProperty(moveDownAnimation, new PropertyPath(Canvas.TopProperty));
            _sumAnimations[0] = new Animation(actAnimation.Begin);

            var moveRightAnimation = new DoubleAnimation(_actualPosX, _targetPosX, duration);
            actAnimation.Children.Add(moveRightAnimation);
            Storyboard.SetTarget(moveRightAnimation, _entity);
            Storyboard.SetTargetProperty(moveRightAnimation, new PropertyPath(Canvas.LeftProperty));
            _sumAnimations[1] = new Animation(actAnimation.Begin);
        }
    }
}
