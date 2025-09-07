using System.Net.NetworkInformation;

namespace NextHireApp;

public static class NextHireAppDomainErrorCodes
{
    /* You can add your business exception error codes here, as constants */
    public static readonly string CreateCompanyFailed = "NextHire:N001";
    public static readonly string APINotResponse = "NextHire:N002";
    public static readonly string TaxCodeInvalid = "NextHire:N003";
    public static readonly string TaxCodeNotfound = "NextHire:N004";
    public static readonly string TaxCodeAuthenFailed = "NextHire:N005";
    public static readonly string IndustryInvalid = "NextHire:N006";
    public static readonly string UserNotFound = "NextHire:N007";
    public static readonly string UpdateCompanyFailed = "NextHire:N008";
    public static readonly string CompanyNotFound = "NextHire:N009";
    public static readonly string StatusInvalid = "NextHire:N010";
}
