using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextHireApp.Dtos;
using NextHireApp.JobApplications;
using NextHireApp.Model;
using NextHireApp.Service;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;

namespace NextHireApp.Controllers
{
    [Route("api/[controller]")]
    public class JobApplicationController : NextHireAppController 
    {
        private readonly IJobApplicationService _service;
        public JobApplicationController(IJobApplicationService service)
        {
            _service = service;
            
        }
        /// <summary>
        /// Lây  chi tiết ưng tuyển
        /// </summary>
        [HttpGet("{applicationCode}")]
        public async Task<ActionResult<JobApplicationDetailDto>> GetJobApplicationDetailByCodeAsync([FromRoute] string applicationCode)
        {
            var result = await _service.GetJobApplicationDetailByCodeAsync(applicationCode);
            return Ok(result);
        }
        /// <summary>
        /// Người dùng đăng ký ứng tuyển
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<JobApplicationDto>> CreateJobApplicationAsync(CreateJobApplicationDto input)
        {
            var result = await _service.CreateJobApplicationAsync(input);
            return Ok(result);

        }
        /// <summary>
        /// API lấy danh sách hồ sơ ứng tuyển của một người dùng
        /// </summary>
        [HttpGet("by-user/{userCode}")]
        public async Task<ActionResult<List<JobApplicationDto>>> GetJobApplicationsByUserCodeAsync(string userCode)
        {
            var result = await _service.GetJobApplicationsByUserCodeAsync(userCode);
            return Ok(result);
        }

        /// <summary>
        /// API lấy danh sách ứng viên đã ứng tuyển vào một công việc
        /// </summary>
        [HttpGet("by-job/{jobCode}")]
        public async Task<ActionResult<List<JobApplicationDto>>> GetJobApplicationsByJobCodeAsync(string jobCode)
        {
            var result = await _service.GetJobApplicationsByJobCodeAsync(jobCode);
            return Ok(result);
        }

        /// <summary>
        /// API cập nhật trạng thái hồ sơ ứng tuyển
        /// </summary>
        [HttpPut("status")]
        public async Task<ActionResult<JobApplicationDto>> UpdateApplicationStatusAsync([FromBody] UpdateJobApplicationStatusDto input)
        {
            var result = await _service.UpdateApplicationStatusAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// API kiểm tra người dùng đã ứng tuyển vào công việc này chưa
        /// </summary>
        [HttpGet("check-applied")]
        public async Task<ActionResult<bool>> HasUserAppliedToJobAsync([FromQuery] string userCode, [FromQuery] string jobCode)
        {
            var result = await _service.HasUserAppliedToJobAsync(userCode, jobCode);
            return Ok(result);
        }

        /// <summary>
        /// API cho phép ứng viên hủy đơn ứng tuyển
        /// </summary>
        [HttpPost("cancel")]
        public async Task<ActionResult<bool>> CancelJobApplicationAsync([FromBody] CancelJobApplicationDto input)
        {
            var result = await _service.CancelJobApplicationAsync(input.ApplicationCode, input.UserCode);
            return Ok(result);
        }
        [HttpGet("download-test")]
        public async   Task<IActionResult> DownloadPdf()
        {
            var application = await _service.GetJobApplicationDetailByCodeAsync("APP329KR2J1");
            byte[] bytes = Convert.FromBase64String(application.CvFile!);
            return File(bytes, "application/pdf","Xincao.pdf");
        }
    }
}
