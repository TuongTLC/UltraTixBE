using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using UltraTix2022.API.UltraTix2022.Business.Services.SystemAdminService;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/system-admin")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class SystemAdminController : ControllerBase
    {
        private readonly ISystemAdminService _systemAdminService;

        public SystemAdminController(ISystemAdminService systemAdminService)
        {
            _systemAdminService = systemAdminService;
        }

        [HttpGet("get-system-admin-accounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminAccounts()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _systemAdminService.GetAdminAccounts(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("insert-account")]
        [SwaggerOperation(Summary = "Insert admin account for admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertAdmin([FromBody] UserAccountInsertModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                bool result = await _systemAdminService.InsertAdmin(token, request);
                return (result) ? Ok(request) : StatusCode(500, "Internal Server Exception");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-accounts")]
        [SwaggerOperation(Summary = "Get accounts for admin management")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAccouts()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _systemAdminService.GetAccounts(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-account-status")]
        [SwaggerOperation(Summary = "Update account status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccountStatus(string status, Guid accountID)
        {
            if (!status.Equals("enable") && !status.Equals("disable"))
            {
                return StatusCode(500, "Status must be enable or disable!!!");
            }
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _systemAdminService.UpdateAccoutnStatus(token, status, accountID);
                return (result != false) ? Ok("Status updated!") : StatusCode(500, "Internal Server Exception");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
