using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace DutchCoronaCheckUtils
{
    /// <summary>
    /// C# implementation from this Python code:
    /// 
    /// def base45decode_nl(s: str) -> bytes:
    ///     base45_nl_charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:"
    /// 
    ///     s_len = len(s)
    ///     res = 0
    ///     for i, c in enumerate(s):
    ///         f = base45_nl_charset.index(c)
    ///         w = 45 ** (s_len - i - 1)
    ///         res += f * w
    ///     return res.to_bytes((res.bit_length() + 7) // 8, byteorder='big') 
    /// </summary>
    public static class DutchCoronaCheckBase45Utils
    {
        const int BaseSize = 45;

        static readonly char[] Base45Digits =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C',
            'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%',
            '*', '+', '-', '.', '/', ':'
        };

        public static byte[] Decode(string inputString)
        {
            var stringLength = inputString.Length;

            BigInteger result = 0;
            foreach (var item in inputString.Select((ch, index) => new { index, ch }))
            {
                var location = Array.FindIndex(Base45Digits, d => d == item.ch);
                var value = BigInteger.Pow(BaseSize, stringLength - item.index - 1);

                result += location * value;
            }

            return result.ToByteArray(false, true);
        }

        public static string Encode(byte[] srcBytes)
        {
            var source = new BigInteger(srcBytes, false, true);

            int log = (int)BigInteger.Log(source, BaseSize);
            var stringBuilder = new StringBuilder(log + 1);

            while (log >= 0)
            {
                var location = BigInteger.DivRem(source, BigInteger.Pow(BaseSize, log), out BigInteger remainder);
                stringBuilder.Append(Base45Digits[(int)location]);

                source = remainder;
                log--;
            }

            return stringBuilder.ToString();
        }
    }
}