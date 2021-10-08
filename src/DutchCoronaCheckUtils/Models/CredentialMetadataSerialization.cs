namespace DutchCoronaCheckUtils.Models
{
public class CredentialMetadataSerialization
{
    /// <summary>
    /// CredentialVersion identifies the credential version, and is always a single byte.
    /// </summary>
    public byte CredentialVersion { get; set; }

    /// <summary>
    /// A string that identifies the public key of the issuer that should be used for verification.
    /// </summary>
    public string IssuerPkId { get; set; } = null!;
}
}