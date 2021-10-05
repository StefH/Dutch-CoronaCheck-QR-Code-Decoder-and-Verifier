using System.Linq;
using System.Numerics;

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
    public static class DutchBase45Decoder
    {
        private const string base45DutchCharset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";

        public static byte[] Decode(string inputString)
        {
            var stringLength = inputString.Length;

            BigInteger result = 0;
            foreach (var item in inputString.Select((ch, index) => new { index, ch }))
            {
                var location = base45DutchCharset.IndexOf(item.ch);
                var value = BigInteger.Pow(45, stringLength - item.index - 1);

                result += location * value;
            }

            return result.ToByteArray(false, true);
        }
    }
}