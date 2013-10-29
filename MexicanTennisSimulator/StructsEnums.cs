using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator
{
    public struct Rally { public RallyProp Service; public RallyProp Side; };
    public enum RallyProp { ServicePlayerOne, ServicePlayerTwo, UpperFieldPlayerOne, UpperFieldPlayerTwo};
}
