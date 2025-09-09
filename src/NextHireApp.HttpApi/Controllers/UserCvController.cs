using Microsoft.AspNetCore.Mvc;
using NextHireApp.Dtos;
using NextHireApp.Service;
using NextHireApp.UserCVs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextHireApp.Controllers
{
    [Route("api/[controller]")]
    public class UserCvController : NextHireAppController
    {
        private readonly IUserCvService _service;

        public UserCvController(IUserCvService service)
        {
            _service = service;
        }

        /// <summary>
        /// Upload a new CV
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserCvDto>> CreateUserCvAsync([FromForm] CreateUserCvDto input)
        {
            var result = await _service.CreateUserCvAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// Get CV details by ID
        /// </summary>
        [HttpGet("{cvId}")]
        public async Task<ActionResult<UserCvDetailDto>> GetUserCvAsync(Guid cvId)
        {
            var result = await _service.GetUserCvAsync(cvId);
            return Ok(result);
        }

        /// <summary>
        /// Get all CVs for a user
        /// </summary>
        [HttpGet("by-user/{userCode}")]
        public async Task<ActionResult<List<UserCvDto>>> GetUserCvsByUserCodeAsync(string userCode)
        {
            var result = await _service.GetUserCvsByUserCodeAsync(userCode);
            return Ok(result);
        }

        /// <summary>
        /// Get the default CV for a user
        /// </summary>
        [HttpGet("default/{userCode}")]
        public async Task<ActionResult<UserCvDetailDto>> GetDefaultUserCvAsync(string userCode)
        {
            var result = await _service.GetDefaultUserCvAsync(userCode);
            return Ok(result);
        }

        /// <summary>
        /// Set a CV as the default
        /// </summary>
        [HttpPost("set-default")]
        public async Task<ActionResult<UserCvDto>> SetDefaultUserCvAsync([FromBody] SetDefaultUserCvDto input)
        {
            var result = await _service.SetDefaultUserCvAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// Delete a CV
        /// </summary>
        [HttpDelete("{cvId}")]
        public async Task<ActionResult<bool>> DeleteUserCvAsync(Guid cvId, [FromQuery] string userCode)
        {
            var result = await _service.DeleteUserCvAsync(cvId, userCode);
            return Ok(result);
        }

        /// <summary>
        /// Update a CV
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<UserCvDto>> UpdateUserCvAsync([FromForm] UpdateUserCvDto input)
        {
            var result = await _service.UpdateUserCvAsync(input);
            return Ok(result);
        }

        /// <summary>
        /// Download a CV as PDF
        /// </summary>
        [HttpGet("download/{cvId}")]
        public async Task<IActionResult> DownloadCvAsync(Guid cvId)
        {
            var cv = await _service.GetUserCvAsync(cvId);
            if (cv == null)
            {
                return NotFound();
            }

            byte[] bytes = Convert.FromBase64String(cv.FileCv);
            return File(bytes, "application/pdf", $"{cv.CvName}.pdf");
        }
    }
}