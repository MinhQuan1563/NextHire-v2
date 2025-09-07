using NextHireApp.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NextHireApp.Repository
{
    public interface ICompanyRepository : ITransientDependency
    {
        Task<Company?> GetCompanyByTaxCode(string taxCode);
        Task<List<Company>> GetListCompany(int limit, int offset);

        Task<Company> CreateCompany(Company company);

        Task<bool> DeleteByTaxCode(string taxCode);

        Task<Company> UpdateCompany(Company company, CancellationToken ct = default);

        Task<bool> VerifyStatus (string taxCode, int status);
    }
}
