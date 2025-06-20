using System.Security.Cryptography;

namespace Api.Helpers
{
    public class RandomGenerator
    {
        public int GenerateRandomNumber(int maxNumber)
        {
            if (maxNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxNumber));

            byte[] bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);

            uint value = BitConverter.ToUInt32(bytes, 0);

            return (int)(value % (uint)maxNumber) + 1;
        }
    }
}
