using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MexicanTennisSimulator.Classes
{
    static class Probability
    {
        public static bool GetTrueOrFalse(string getTrueProbability, bool recursion = false)
        {
            double getTrueProbabilityDouble = Convert.ToDouble(getTrueProbability);
            int getTrueProbabilityInt = (int)getTrueProbabilityDouble;

            if (!recursion)
            {
                int innerTrueProbability = 0;
                if (getTrueProbabilityDouble < 0 || getTrueProbabilityDouble > 100)
                    throw new ArgumentOutOfRangeException("trueProbabiliy", "Der Wert von 'trueProbabiliy' muss zwischen 0 und 100 liegen.");

                int indexPoint = getTrueProbability.IndexOf(",");
                if (indexPoint == -1)
                    indexPoint = getTrueProbability.IndexOf(".");
                if (indexPoint != -1)
                {
                    string stringDecimalPlaces = getTrueProbability.Substring(indexPoint + 1);
                    int countDecimalPlaces = stringDecimalPlaces.Length;
                    for (int i = countDecimalPlaces - 1; i >= 0; i--)
                    {
                        innerTrueProbability += Convert.ToInt32(stringDecimalPlaces.Substring(i, 1));
                        innerTrueProbability += innerTrueProbability * 9;
                        if (GetTrueOrFalse(innerTrueProbability.ToString(), true))
                            innerTrueProbability = 1;
                        else
                            innerTrueProbability = 0;
                    }
                }
                getTrueProbabilityInt += innerTrueProbability;
            }

            int randomNumber = GetBetterRandomNumber(100);
            if (randomNumber < getTrueProbabilityInt)
                return true;
            else
                return false;
        }

        public static int GetBetterRandomNumber(int exclMax)
        {
            RNGCryptoServiceProvider c = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[4];
            c.GetBytes(randomNumber);
            int result = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
            return result % exclMax;
        }

        public static int GetBetterRandomNumber(int inclMin, int exclMax)
        {
            RNGCryptoServiceProvider c = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[4];
            c.GetBytes(randomNumber);
            int result = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
            return result % exclMax + inclMin;
        }
    }
}
