using System.Numerics;

namespace DutchCoronaCheckUtils.Models
{
    public class TopLevelStructure
    {
        public BigInteger DisclosureTimeSeconds { get; set; }
        public BigInteger C { get; set; }
        public BigInteger A { get; set; }
        public BigInteger EResponse { get; set; }
        public BigInteger VResponse { get; set; }
        public BigInteger AResponse { get; set; }
        public SecurityAspect? ADisclosed { get; set; }
    }
}