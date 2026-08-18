namespace TanfeethIntegration.DTOs
{
    public class DefendantTypeLookupDto
    {
        public int Id { get; set; }
        public string nameAr { get; set; }
        public string nameEn { get; set; }
        public bool? Disabled { get; set; } // Nullable to omit if not required
        public bool? Selected { get; set; } // Nullable to omit if not required
    }
}
