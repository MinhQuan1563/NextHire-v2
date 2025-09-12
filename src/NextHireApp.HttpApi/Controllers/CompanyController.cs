using Microsoft.AspNetCore.Mvc;
using NextHireApp.Dtos;
using NextHireApp.Services;
using System.Threading.Tasks;

namespace NextHireApp.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : NextHireAppController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchCompanyByTaxCode([FromQuery] string taxCode)
        {
            return Ok(await _companyService.GetCompanyByTaxCode(taxCode));
        }

        [HttpGet("companies")]
        public async Task<IActionResult> GetListCompany([FromQuery] int pageSize = 10, 
            [FromQuery] int pageStart = 1)
        {
            return Ok(await _companyService.GetListCompany(pageSize, pageStart-1));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDTO input)
        {
            return Ok(await _companyService.CreateCompany(input));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteByTaxCode([FromQuery] string taxCode)
        {
            return Ok(await _companyService.DeleteByTaxCode(taxCode));
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDTO companyDTO)
        {
            return Ok(await _companyService.UpdateCompany(companyDTO));
        }

        [HttpPost("verify-status")]
        public async Task<IActionResult> VerifyStatus([FromQuery] string taxCode, [FromQuery] int status)
        {
            return Ok(await _companyService.VerifyStatus(taxCode, status));
        }
    }
}
