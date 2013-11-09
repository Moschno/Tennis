using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    internal delegate void FinishedEventHandler(Object sender, FinishedEventArgs e);
    internal delegate void BatFinishedEventHandler(Object sender, BatFinishedEventArgs e);

    class FinishedEventArgs : EventArgs
    {
        public readonly eCourtElements Winner;

        public FinishedEventArgs(eCourtElements winner)
        {
            Winner = winner;
        }
    }

    class BatFinishedEventArgs : EventArgs
    {
        public readonly eBatEnding WhatHappend;
        public readonly eBatBeginning Bat;
        public readonly Player PlayerWithBat;
        public readonly Player PlayerWithoutBat;

        public BatFinishedEventArgs(eBatEnding whatHappend)
        {
            WhatHappend = whatHappend;
        }
    }
}
