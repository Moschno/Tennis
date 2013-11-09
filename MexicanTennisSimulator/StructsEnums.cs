using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MexicanTennisSimulator
{
    public struct sRally { public eCourtElements FirstService; public eCourtElements SecondService; public eCourtElements UpperSide; };
    public struct sElement { public eCourtElements Element; };
    public struct sPlayer { public eCourtElements Player; };
    public struct sBatProps { public eBats Bat; public Point vStartingPos; public Point vTargetPos; public double Speed; };
    public enum eCourtElements { Default, PlayerOne, PlayerTwo, PlayerWithService, PlayerWithoutService, PlayerWithBat, PlayerWithoutBat, GameBall };
    public enum eBats { Default, Service, Forehand, Backhand };
    public enum eBatEnding { Default, BallIsOut, BallIsReturned, BallIsBroken, BallIsNotReturned, Ace };
    public enum eBatBeginning { Default, FirstService, SecondService, Return };
}
