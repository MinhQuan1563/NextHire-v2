using Microsoft.EntityFrameworkCore;
using NextHireApp.EntityFrameworkCore;
using NextHireApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NextHireApp.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly INextHireAppDbContext _context;
        private readonly INextHireAppReadDbContext _readContext;

        public CompanyRepository(INextHireAppDbContext context, INextHireAppReadDbContext readContext)
        {
            _context = context;
            _readContext = readContext;
        }

        public async Task<Company> CreateCompany(Company company)
        {
            try
            {
                var res = _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                return company;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByTaxCode(string taxCode)
        {
            try
            {
                var company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.TaxCode == taxCode);
                if (company == null) return false;
                company.Status = 1;
                return await _context.SaveChangesAsync() > 0;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Company?> GetCompanyByTaxCode(string taxCode)
        {
            return await _readContext.Companies
                .FirstOrDefaultAsync(c => c.TaxCode == taxCode);
        }

        public async Task<List<Company>> GetListCompany(int limit, int offset)
        {
            return await _readContext.Companies
                    .OrderBy(c => c.CompanyName)
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();
        }

        public async Task<Company> UpdateCompany(Company company, CancellationToken ct = default)
        {
            try
            {
                var exists = await _context.Companies
                                   .AsNoTracking()
                                   .AnyAsync(x => x.CompanyId == company.CompanyId, ct);
                if (!exists)
                    throw new KeyNotFoundException($"Company {company.TaxCode} not found.");

                _context.Attach(company);
                _context.Entry(company).State = EntityState.Modified;

                await _context.SaveChangesAsync(ct);
                return company;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> VerifyStatus(string taxCode, int status)
        {
            try
            {
                var existingCompany = await _context.Companies
                    .FirstOrDefaultAsync(c => c.TaxCode == taxCode);
                if (existingCompany == null)
                {
                    throw new Exception("Company not found or status does not match.");
                }
                existingCompany.Status = status;
                return await _context.SaveChangesAsync() > 0;

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
