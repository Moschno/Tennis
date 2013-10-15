using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    interface IEntity
    {
        void Move(double durationInSeconds, double[] targetPos);
    }
}
