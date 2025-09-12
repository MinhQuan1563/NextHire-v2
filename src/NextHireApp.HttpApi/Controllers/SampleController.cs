using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NextHireApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleController : NextHireAppController
    {
        [HttpGet]
        public Task<IActionResult> Get()
        {
            return Task.FromResult<IActionResult>(Ok("Welcome to NextHire!!!"));
        }
    }
}
