namespace DutchCoronaCheckUtils.Models
{
    public class SecurityAspect
    {
        public CredentialMetadataSerialization? Metadata { get; set; } = null!;

        /// <summary>
        /// Indicates whether we're dealing with a demo or sample ("1"), or with a live one ("0").
        /// </summary>
        public string? IsSpecimen { get; set; } = null!;

        /// <summary>
        /// Indicates whether we're dealing with a QR that's been generated by the app, or one that's been printed out.
        /// The difference here is that QR codes generated by the app are only considered "fresh" for a very brief amount of time
        /// (a few minutes, from what I've been able to find), and won't be accepted once expired. Paper QRs are exempt from this check.
        /// </summary>
        public string? IsPaperProof { get; set; } = null!;

        /// <summary>
        /// Defines the start time of the QR's validity. Defined as Epoch.
        /// There is apparently some randomness involved with this value so as to avoid disclosing exactly when someone has been vaccinated or tested,
        /// even though this really only indicates when the QR has been created.
        /// </summary>
        public string? ValidFrom { get; set; } = null!;

        /// <summary>
        /// Defines how long the QR is valid for.
        /// This value is added to the "validFrom" time when determining if a QR is valid at a particular moment.
        /// For QRs generated by the app this value will always be 24 hours.
        /// Paper certificates are valid for 40 hours (in case of a negative test) or 28 days (in case of a vaccination or recovery).
        /// Digital certificates are currently valid for 8760 hours, which is 1 year.
        /// </summary>
        public string? ValidForHours { get; set; } = null!;

        /// <summary>
        /// The first initial of the person's first name.
        /// </summary>
        public string? FirstNameInitial { get; set; } = null!;

        /// <summary>
        /// The first initial of the person's last name
        /// </summary>
        public string? LastNameInitial { get; set; } = null!;

        /// <summary>
        /// Defines the day-of-month of the person's birthday.
        /// </summary>
        public string? BirthDay { get; set; } = null!;

        /// <summary>
        /// Defines the month of the person's birthday.
        /// </summary>
        public string? BirthMonth { get; set; } = null!;
    }
}