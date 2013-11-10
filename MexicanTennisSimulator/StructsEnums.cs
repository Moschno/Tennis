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
    public struct sBatProps { public eBats Bat
                            ; public Point vBatPos
                            ; public Point vFirstLandingPos
                            ; public Point vSecondLandingPos
                            ; public double SpeedTillFirstLanding_KmH
                            ; public double SpeedTillSecondLanding_KmH
                            ; public double TimeTillFirstTarget
                            ; public double TimeTillSecondTarget
                            ; public bool BallIsTaken; 
                            };
    public enum eCourtElements { Default
                               , PlayerOne
                               , PlayerTwo
                               , PlayerWithService
                               , PlayerWithoutService
                               , PlayerWithBat
                               , PlayerWithoutBat
                               , GameBall 
                               };
    public enum eBats { Default, Volley, Smash, Lob, Cross, Longline, Service};
    public enum eBatSide { Default, Forehand, Backhand };
    public enum eBatEnding { Default, BallIsOut, BallIsReturned, BallIsBroken, BallIsNotReturned, Ace , Let, DoubleFault};
    public enum eBatBeginning { Bat, FirstService, SecondService, Return };
}
