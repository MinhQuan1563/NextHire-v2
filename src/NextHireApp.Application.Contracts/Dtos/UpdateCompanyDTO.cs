using NextHireApp.Model;

namespace NextHireApp.Dtos
{
    public class UpdateCompanyDTO
    {
        public string? TaxCode { get; set; }
        public string? CompanyName { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public IndustryEnum Industry { get; set; }
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
    }
}
