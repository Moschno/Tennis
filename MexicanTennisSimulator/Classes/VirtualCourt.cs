using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class VirtualCourt
    {
        private int[] _actPosPlayerOne;
        private int[] _actPosPlayerTwo;
        private int[] _actPosBall;
        /// <summary>
        /// Whole-Size: b = 720px; h = 1560px
        /// Outer-Lines of the court: b = 360px; h = 780px; Postion = Centre of whole size
        /// </summary>
        public VirtualCourt()
        { 
            _actPosPlayerOne = new int[2];
            _actPosPlayerTwo = new int[2];
            _actPosBall = new int[2];
        }
    }
}
