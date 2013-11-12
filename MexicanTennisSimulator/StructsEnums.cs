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
                            ; public Point vTakePos
                            ; public double SpeedTillFirstLanding_KmH
                            ; public double SpeedFromFirstTillSecondLanding_KmH
                            ; public double TimeTillFirstTarget
                            ; public double TimeFromFirstTillSecondTarget
                            ; public bool BallIsTaken 
                            ; public eTaking TakingDifficulty
                            ; public bool BallIsBroken
                            ; public bool BallWillLandOut
                            ; public eBallSeeableOut BallSeeableOut;
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
    public enum eBats { NotSet, Volley, Smash, Lob, Cross, Longline, Service, Return};
    public enum eBatResult { NotSet, BallIsTaken, BallIsOut, BallIsReturned, BallIsBroken, BallIsNotTaken, Ace , Let, };
    public enum eBatType { NotSet, Bat, FirstService, SecondService, Return };
    public enum eTaking { NotSet, Easy, Medium, Hard };
    public enum eBallSeeableOut { NotSet, ClearlyNotOut, ClearlyVisibleOut, MaybeOut, CantSeeIfOut };
}
