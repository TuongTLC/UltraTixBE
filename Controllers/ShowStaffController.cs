using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowStaffService;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/showstaff")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ShowStaffController : ControllerBase
    {
        private readonly IShowStaffService _showStaffService;

        public ShowStaffController(IShowStaffService organizerService)
        {
            _showStaffService = organizerService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetShowStaffAccounts()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showStaffService.GetShowStaffAccounts(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-staff-detail-by-id")]
        [Authorize(Roles = "Admin,Organizer,Staff")]
        public async Task<IActionResult> GetShowStaffDetailById(Guid staffId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showStaffService.GetShowStaffDetailById(token, staffId);

                return (result != null) ? Ok(result) : StatusCode(404, "Staff Account Not Found with ID: " + staffId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("insert-showstaff-account")]
        [SwaggerOperation(Summary = "Insert showstaff account for organizer role")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> InsertShowStaff([FromBody] ShowStaffAccountInsertModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                bool result = await _showStaffService.InsertShowStaff(token, request);
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

        [HttpPut("update-profile")]
        [SwaggerOperation(Summary = "Update staff profile for organizer")]
        [Authorize(Roles = "Staff,Admin,Organizer")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateRequestModel profile)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showStaffService.UpdateUserProfile(token, profile);
                return result ? Ok(profile) : BadRequest("UPDATE PROFILE FAILED");
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

        [HttpDelete("remove-account")]
        [SwaggerOperation(Summary = "Remove staff account for admin, organizer")]
        [Authorize(Roles = "Staff,Admin,Organizer")]
        public async Task<IActionResult> RemoveStaffAccount(Guid staffId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showStaffService.RemoveStaffAccount(token, staffId);
                return result ? Ok(result) : BadRequest("Remove PROFILE FAILED");
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
