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
    abstract class CourtElement : FrameworkElement
    {
        protected Point _vActPos;
        protected Point _vTargetPos;

        public Point VActPos
        {
            get { return _vActPos; }
        }

        public Point VTargetPos
        {
            get { return _vTargetPos; }
            set { _vTargetPos = value; }
        }

        protected CourtElement()
        {
        }

        public double MoveToTargetPos(double speed_KmH)
        {
            double elapsedTime;
            if (speed_KmH > 0)
                elapsedTime = Bat.CalcTimeTillTarget(_vActPos, _vTargetPos, speed_KmH);
            else
                elapsedTime = 0;

            _vActPos = _vTargetPos;
            return elapsedTime;
        }
    }
}
