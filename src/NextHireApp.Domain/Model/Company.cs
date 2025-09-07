using System;

namespace NextHireApp.Model
{
    public class Company : BaseModel
    {
        public Guid CompanyId { get; set; }
        public string TaxCode { get; set; } = default!;
        public string UserCode { get; set; } = default!; // Owner
        public string CompanyName { get; set; } = default!;
        public string? Description { get; set; }
        public string Address { get; set; } = default!;
        public int Status { get; set; } // 1: Inactive, 2: Active
        public IndustryEnum Industry { get; set; } // Genre of company
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public int CompanyVersion { get; set; } // So lan Update
    }

    public enum IndustryEnum
    {
        Technology = 1,
        Finance = 2,
        Healthcare = 3,
        Education = 4,
        Retail = 5,
        Manufacturing = 6,
        Entertainment = 7,
        Hospitality = 8,
        Transportation = 9,
        RealEstate = 10
    }
}
