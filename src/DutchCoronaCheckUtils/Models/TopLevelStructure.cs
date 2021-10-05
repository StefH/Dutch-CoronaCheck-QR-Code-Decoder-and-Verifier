using System.Numerics;

namespace DutchCoronaCheckUtils.Models
{
    /// <summary>
    /// DHC DEFINITIONS ::= BEGIN
    ///     ProofSerializationV2 ::= SEQUENCE {
    ///         disclosureTimeSeconds  INTEGER,
    ///         c                      INTEGER,
    ///         a                      INTEGER,
    ///         eResponse              INTEGER,
    ///         vResponse              INTEGER,
    ///         aResponse              INTEGER,
    ///         aDisclosed             SEQUENCE OF INTEGER
    ///     }
    /// 
    ///     CredentialMetadataSerialization ::= SEQUENCE {
    ///         -- CredentialVersion identifies the credential version, and is always a single byte
    ///         credentialVersion OCTET STRING,
    /// 
    ///         -- IssuerPkId identifies the public key to use for verification
    ///         issuerPkId PrintableString
    ///     }
    /// END
    /// </summary>
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