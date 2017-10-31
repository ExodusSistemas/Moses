using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Moses
{
    public static class RandomUtils
    {
        private static Random randomSeed = new Random();

        public static string RandomText()
        {
            return RandomText(null);
        }

        public static string RandomText(int? maxSize)
        {
            if (!maxSize.HasValue)
                maxSize = Convert.ToInt32( 255 * randomSeed.NextDouble() );

            StringBuilder RandStr = new StringBuilder(maxSize.Value);

            for (int i = 0; i < maxSize; i++)
            {
                RandStr.Append(RandomString(Convert.ToInt32(20 * randomSeed.NextDouble()), null));
                RandStr.Append(" ");
            }

            return RandStr.ToString(0,RandStr.Length-1);
        }

        public static string RandomString()
        {
            return RandomString(null,null);
        }

        public static string RandomString(int size)
        {
            return RandomString(size, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerCase">Se for null, são ambos</param>
        /// <returns></returns>
        public static string RandomString( int? size, bool? lowerCase)
        {
            /* StringBuilder is faster than using strings (+=)*/
            if (!size.HasValue)
                size = RandomNumber() % 20;
            StringBuilder RandStr = new StringBuilder(size.Value);

            /* Ascii start position (65 = A / 97 = a)*/
            int start = (lowerCase.GetValueOrDefault()) ? 97 : 65;

            int charsets = lowerCase.HasValue ?  26 : 58;

            /* Add random chars*/
            for (int i = 0; i < size; i++)
                RandStr.Append((char)(charsets * randomSeed.NextDouble() + start));

            return RandStr.ToString();
        }

        public static string RandomPassword(int PasswordLength)
        {
            String _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789";
            Byte[] randomBytes = new Byte[PasswordLength];
            char[] chars = new char[PasswordLength];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
                
                int allowedCharCount = _allowedChars.Length;

                for (int i = 0; i < PasswordLength; i++)
                {
                    chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
                }
            }

            return new string(chars);
        }

        public static int RandomNumber()
        {
            return  randomSeed.Next() ;
        }

        public static int RandomNumber(int minimal, int maximal)
        {
            return randomSeed.Next(minimal, maximal);
        }

        public static decimal RandomDecimal()
        {
            byte scale = (byte)randomSeed.Next(29);
            bool sign = randomSeed.Next(2) == 1;
            return new decimal(randomSeed.Next(),
                                randomSeed.Next(),
                                randomSeed.Next(),
                                sign,
                                scale);

        }

        public static string RandomPhone()
        {
            return string.Format("({0}){1}-{2}", 
                                randomSeed.Next(10,99) ,
                                randomSeed.Next(3000,9999),
                                randomSeed.Next(1000, 9999));

        }

        public static bool RandomBool()
        {
            return (randomSeed.NextDouble() > 0.5);
        }

        public static DateTime RandomDateTime(DateTime min, DateTime max)
        {
            if (max <= min)
            {
                string message = "Max must be greater than min.";
                throw new ArgumentException(message);
            }
            long minTicks = min.Ticks;
            long maxTicks = max.Ticks;
            double rn = (Convert.ToDouble(maxTicks)
               - Convert.ToDouble(minTicks)) * randomSeed.NextDouble()
               + Convert.ToDouble(minTicks);
            return new DateTime(Convert.ToInt64(rn));
        }
    }
}
