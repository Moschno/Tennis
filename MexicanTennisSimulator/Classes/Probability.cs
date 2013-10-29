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
        public static bool GetTrueOrFalse(double getTrueProbability)
        {
            if (getTrueProbability < 0 || getTrueProbability > 100)
                throw new ArgumentOutOfRangeException("trueProbabiliy", "Der Wert von 'trueProbabiliy' muss zwischen 0 und 100 liegen.");

            string stringNumber = getTrueProbability.ToString();
            if (stringNumber.IndexOf("E") != -1)
                throw new ArgumentOutOfRangeException("trueProbabiliy", "Zu viele hintereinander liegende Nullen in den Nachkommastellen.");
            
            int indexPoint = stringNumber.IndexOf(",");
            if (indexPoint != -1)
	        {
                string stringDecimalPlaces = stringNumber.Substring(indexPoint + 1);
                int countDecimalPlaces = stringDecimalPlaces.Length;
                int innerTrueProbability = 0;
                for (int i = countDecimalPlaces - 1; i >= 0; i--)
                {
                    innerTrueProbability += Convert.ToInt32(stringDecimalPlaces.Substring(i, 1));
                    innerTrueProbability += innerTrueProbability * 9;
                    if (GetTrueOrFalse(innerTrueProbability))
                        innerTrueProbability = 1;
                    else
                        innerTrueProbability = 0;
                }

                getTrueProbability = (int)getTrueProbability + innerTrueProbability;
	        }
            
            int randomNumber = GetBetterRandomNumber(100);
            if (randomNumber < getTrueProbability)
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
