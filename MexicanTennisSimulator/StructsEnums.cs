﻿using System;
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
    public struct sBatProps { public eBats BatPlayerBat
                            ; public Point vBatPlayerBatPos
                            ; public Point vBat
                            ; public Point vBallFirstLandingPos
                            ; public Point vBallSecondLandingPos
                            ; public Point vTakePlayerTakePos
                            ; public double BallSpeedTillFirstLanding_KmH
                            ; public double BallSpeedFromFirstTillSecondLanding_KmH
                            ; public double BallTimeTillFirstTarget
                            ; public double BallTimeFromFirstTillSecondTarget
                            ; public double BallTimeTillTakePos
                            ; public bool BallIsTaken 
                            ; public eTaking BallTakingDifficulty
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
