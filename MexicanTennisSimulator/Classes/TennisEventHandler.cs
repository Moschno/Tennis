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
        public readonly eBatResult WhatHappend;
        public readonly Player PlayerWhoBattedBall;

        public BatFinishedEventArgs(eBatResult whatHappend, Player playerWithBat)
        {
            WhatHappend = whatHappend;
            PlayerWhoBattedBall = playerWithBat;
        }
    }
}
