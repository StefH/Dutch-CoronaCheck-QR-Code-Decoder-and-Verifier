using System.Numerics;

namespace DutchCoronaCheckUtils.Extensions
{
    internal static class BigIntegerExtensions
    {
        /// <summary>
        /// Write the bytes in a big-endian byte order.
        /// </summary>
        public static byte[] ToBigEndianByteArray(this BigInteger value)
        {
            return value.ToByteArray(false, true);
        }

        /// <summary>
        /// Convert the bytes to a big-endian byte order BigInteger.
        /// </summary>
        public static BigInteger ToBigEndianInteger(this byte[] value)
{
            return new BigInteger(value, false, true);
        }
    }
}