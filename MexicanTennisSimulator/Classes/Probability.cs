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
        public static bool RollByFactor(string trueProbability, bool recursion = false)
        {
            double getTrueProbabilityDouble = Convert.ToDouble(trueProbability);
            int getTrueProbabilityInt = (int)getTrueProbabilityDouble;

            if (!recursion)
            {
                int innerTrueProbability = 0;
                if (getTrueProbabilityDouble < 0 || getTrueProbabilityDouble > 100)
                    throw new ArgumentOutOfRangeException("trueProbabiliy", "Der Wert von 'trueProbabiliy' muss zwischen 0 und 100 liegen.");

                int indexPoint = trueProbability.IndexOf(",");
                if (indexPoint == -1)
                    indexPoint = trueProbability.IndexOf(".");
                if (indexPoint != -1)
                {
                    string stringDecimalPlaces = trueProbability.Substring(indexPoint + 1);
                    int countDecimalPlaces = stringDecimalPlaces.Length;
                    for (int i = countDecimalPlaces - 1; i >= 0; i--)
                    {
                        innerTrueProbability += Convert.ToInt32(stringDecimalPlaces.Substring(i, 1));
                        innerTrueProbability += innerTrueProbability * 9;
                        if (RollByFactor(innerTrueProbability.ToString(), true))
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

        public static int GetBetterRandomNumber(int exclMax, bool inclZero = true)
        {
            int result = 0;
            if (inclZero == true)
            {
                RNGCryptoServiceProvider c = new RNGCryptoServiceProvider();
                byte[] randomNumber = new byte[4];
                c.GetBytes(randomNumber);
                result = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
            }
            else
            {
                do
                {
                    RNGCryptoServiceProvider c = new RNGCryptoServiceProvider();
                    byte[] randomNumber = new byte[4];
                    c.GetBytes(randomNumber);
                    result = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
                } while (result % exclMax == 0); 
            }

            return result % exclMax;
        }

        public static int GetBetterRandomNumber(int inclMin, int exclMax, bool inclZero = true)
        {
            int result = 0;
            if (inclZero == true)
            {
                RNGCryptoServiceProvider c = new RNGCryptoServiceProvider();
                byte[] randomNumber = new byte[4];
                c.GetBytes(randomNumber);
                result = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
            }
            else
            {
                do
                {
                    RNGCryptoServiceProvider c = new RNGCryptoServiceProvider();
                    byte[] randomNumber = new byte[4];
                    c.GetBytes(randomNumber);
                    result = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
                } while (result % exclMax + inclMin == 0);
            }

            return result % exclMax + inclMin;
        }
    }
}
