using InvesAuto.Infra.BaseTest;
using System.Security.Cryptography;
using System.Text;

namespace InvestAuto.Infra.BaseTest
{
    public class BaseTest : Base
    {

        #region Generate uniq key 
        internal static readonly char[] chars =
         "1234567890".ToCharArray();
        protected static string GenerateUniqueKey(int size)
        {
            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }
        #endregion

        #region Generate user id
        public static string GenerateUserId()
        {
            Random random = new Random();
            int[] idDigits = new int[9];
            idDigits[0] = random.Next(1, 10);
            for (int i = 1; i < 8; i++)
            {
                idDigits[i] = random.Next(10);
            }
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                int digit = idDigits[i];
                if (i % 2 == 0)
                {
                    sum += digit;
                }
                else
                {
                    sum += ((digit * 2) % 10) + ((digit * 2) / 10);
                }
            }
            int lastDigit = (10 - (sum % 10)) % 10;
            idDigits[8] = lastDigit;
            string idNumber = string.Join("", idDigits);
            return idNumber.ToString();
        }
        #endregion
        #region Write to log file
        public void WriteToFile(string fileNameRand, string i_textLog)
        {
            string randomNume = GenerateUniqueKey(3);
            string timestamp = DateTime.Now.ToString("yyyyMMdd");
            string fileTime = timestamp;
            int maxAttempts = 3;
            int currentAttempt = 0;
            bool success = false;

            string correntPath = Directory.GetCurrentDirectory();
            string logeName = correntPath + $"\\LogFiles\\logFile_{fileNameRand}_{fileTime}.txt";

            while (!success && currentAttempt < maxAttempts)
            {

                try
                {
                    using (StreamWriter writer = new StreamWriter(logeName, true))
                    {
                        writer.WriteLine(i_textLog);
                    }
                    success = true;
                }


                catch (IOException e)
                {
                    Thread.Sleep(500);
                    currentAttempt++;
                }
            }

        }
        #endregion



    }
}
