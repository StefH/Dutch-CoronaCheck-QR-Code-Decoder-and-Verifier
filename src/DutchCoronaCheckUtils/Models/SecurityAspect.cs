namespace DutchCoronaCheckUtils.Models
{
    public class SecurityAspect
    {
        public Metadata? Metadata { get; set; } = null!;
        public string? IsSpecimen { get; set; } = null!;
        public string? IsPaperProof { get; set; } = null!;
        public string? ValidFrom { get; set; } = null!;
        public string? ValidForHours { get; set; } = null!;
        public string? FirstNameInitial { get; set; } = null!;
        public string? LastNameInitial { get; set; } = null!;
        public string? BirthDay { get; set; } = null!;
        public string? BirthMonth { get; set; } = null!;
    }
}