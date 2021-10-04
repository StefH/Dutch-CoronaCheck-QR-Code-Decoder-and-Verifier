namespace DutchCoronaCheckUtils.Models
{
    public class Metadata
    {
        /// <summary>
        /// The credential version. Currently only version 2 is supported
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// A string that identifies the public key of the issuer that should be used for verification
        /// </summary>
        public string PublicKey { get; set; } = null!;
    }
}