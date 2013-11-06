using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    class vCourt
    {
        public static double BallSlowDownFactor = 3;
        public static int Width = 720;
        public static int Height = 1560;
        public readonly CourtElement PlayerOne;
        public readonly CourtElement PlayerTwo;
        public readonly CourtElement GameBall;

        public vCourt(CourtElement playerOne, CourtElement playerTwo, CourtElement ball)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            GameBall = ball;
        }
    }
}
