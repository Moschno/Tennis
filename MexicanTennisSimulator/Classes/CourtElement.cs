﻿using System;
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
        public static readonly DependencyProperty ActPosProperty =
            DependencyProperty.Register("ActPos", typeof(Point), typeof(CourtElement));     
        public static readonly DependencyProperty TargetPosProperty =
            DependencyProperty.Register("TargetPos", typeof(Point), typeof(CourtElement));


        protected Ellipse _entity;
        protected Animation[] _sumAnimations;
        private Brush _color;
        private double maxSpeed = 5;
        protected double _durationTillTarget = 5;

        public Brush Color
        {
            get { return _color; }
            set { _color = value; _entity.Fill = value; }
        }

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

        protected delegate void Animation();
        public abstract void MoveTo(Point targetPos);

        protected CourtElement()
        {
            _entity = new Ellipse();
        }

        protected void RefreshValues()
        {
            _sumAnimations = null;
            this.ActPos = TargetPos;
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
                this.SetLeft(TargetPos.X);
                this.SetTop(TargetPos.Y);
                this.ActPos = TargetPos;
            }  
        }

        protected void SetMoveAnimation()
        {
            var duration = new Duration(TimeSpan.FromSeconds(_durationTillTarget));
            var actAnimation = new Storyboard();

            var moveRightAnimation = new DoubleAnimation(this.ActPos.X, this.TargetPos.X, duration);
            actAnimation.Children.Add(moveRightAnimation);
            Storyboard.SetTarget(moveRightAnimation, this);
            Storyboard.SetTargetProperty(moveRightAnimation, new PropertyPath(Canvas.LeftProperty));
            _sumAnimations[0] = new Animation(actAnimation.Begin);

            var moveDownAnimation = new DoubleAnimation(this.ActPos.Y, this.TargetPos.Y, duration);
            actAnimation.Children.Add(moveDownAnimation);
            Storyboard.SetTarget(moveDownAnimation, this);
            Storyboard.SetTargetProperty(moveDownAnimation, new PropertyPath(Canvas.TopProperty));
            _sumAnimations[1] = new Animation(actAnimation.Begin);
        }
    }
}
