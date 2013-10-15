using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MexicanTennisSimulator.Classes
{
    class VirtualCourt
    {
        private List<Entity> _element;
        private int _actIndex;

        public int this[int index]
        {
            get
            {
                return _actIndex;
            }
            set
            {
                _actIndex = value;
            }
        }

        public List<Entity> Element
        {
            get { return _element; }
        }

        /// <summary>
        /// Whole-Size: b = 720px; h = 1560px
        /// Outer-Lines of the court: b = 360px; h = 780px; Postion = Centre of whole size
        /// </summary>
        public VirtualCourt()
        {
        }

        public void Add(Entity element)
        {
            _element.Add(element);
        }

        public void ChangePosition(double durationInSeconds = 0, double[] targetPos = null)
        {
            _element[_actIndex].Move(durationInSeconds, targetPos);
        }
    }
}
