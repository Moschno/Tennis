using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MexicanTennisSimulator.Classes
{
    sealed class Referee : FrameworkElement
    {
        private delegate void StartTogether();

        private Match _gameCourt;
        private sRally _rallyProbs;

        private void SetRallyProbs()
        {
            _rallyProbs = new sRally();
            _rallyProbs.FirstService = eCourtElements.PlayerOne;
            _rallyProbs.UpperSide = eCourtElements.PlayerOne;
        }

        private void CheckBallTargetPos()
        {
            
        }

        private void StartRally()
        {
            //StartTogether[] st = new StartTogether[2];
            //st[0] = new StartTogether(_gameCourt.PlayerOne.StartRally);
            //st[1] = new StartTogether(_gameCourt.PlayerTwo.StartRally);
            //StartTogether start = (StartTogether)Delegate.Combine(st);
            //start();
        }

        //private void GameBall_Batted()
        //{
        //    var firstLandingPos = e.VTargetPosBall;
        //    firstLandingPos.X = Math.Abs(firstLandingPos.X);
        //    firstLandingPos.Y = Math.Abs(firstLandingPos.Y);
        //    if (_gameCourt.PlayerOne.Service || _gameCourt.PlayerTwo.Service)
        //    {
        //        if (firstLandingPos.X >= Match.BallServiceOutX || firstLandingPos.Y >= Match.BallServiceOutY)
        //        {
        //            //todo: Ball im aus. Zweiter Aufschlag oder Punkt für Gegner
        //        }
        //    }
        //    else
        //    {
        //        if (firstLandingPos.X >= Match.BallOutX || firstLandingPos.Y >= Match.BallOutY)
        //        {
        //            //todo: Ball im aus. Punkt für Gegner
        //        }
        //    }
        //}
    }
}
