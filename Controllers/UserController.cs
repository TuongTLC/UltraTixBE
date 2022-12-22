using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using UltraTix2022.API.UltraTix2022.Business.Services.EmailService;
using UltraTix2022.API.UltraTix2022.Business.Services.UserService;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userSerivce;
        private readonly IEmailService _emailService;

        public UserController(IUserService userSerivce, IEmailService emailService)
        {
            _userSerivce = userSerivce;
            _emailService = emailService;

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccount([FromBody] UserLoginModel model)
        {
            try
            {
                UserTokenModel user = new();

                if (model == null) throw new ArgumentException("User Null");

                if (model.EmailOrUsername.Contains('@'))
                {
                    user = await _userSerivce.LoginWithEmail(model);
                }
                else if (!model.EmailOrUsername.Contains('@'))
                {
                    user = await _userSerivce.LoginWithUsername(model);
                }
                return Ok(user.Token);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login-mobile")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccountMobile([FromBody] UserLoginModel model)
        {
            try
            {
                UserTokenModel user = new();

                if (model == null) throw new ArgumentException("User Null");

                if (model.EmailOrUsername.Contains('@'))
                {
                    user = await _userSerivce.LoginWithEmail(model);
                }
                else if (!model.EmailOrUsername.Contains('@'))
                {
                    user = await _userSerivce.LoginWithUsername(model);
                }

                if(user.RoleId != Commons.CUSTOMER)
                {
                    return StatusCode(401, "Only Customer Can Login Mobile App");
                }

                return Ok(user.Token);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login-mobile-scanner-staff")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccountMobileScannerStaff([FromBody] UserLoginModel model)
        {
            try
            {
                UserTokenModel user = new();

                if (model == null) throw new ArgumentException("User Null");

                if (model.EmailOrUsername.Contains('@'))
                {
                    user = await _userSerivce.LoginWithEmail(model);
                }
                else if (!model.EmailOrUsername.Contains('@'))
                {
                    user = await _userSerivce.LoginWithUsername(model);
                }

                if (!user.RoleId.Equals(Commons.STAFF) && !user.RoleId.Equals(Commons.TICKETINSPECTOR))
                {
                    return StatusCode(401, "Only Staff Can Login Mobile App");
                }

                return Ok(user.Token);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("send-email-verification-code")]
        [AllowAnonymous]
        public async Task<IActionResult> SendEmailVerificationCode(string email)
        {
            try
            {
                var OTP = GenerateOTP();

                await _emailService.SendEmailVerificationOTP(email, OTP);

                return Ok(OTP);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(RegisterAccountModel account)
        {
            try
            {
                var isSignUpSuccess = await _userSerivce.SignUp(account);

                return Ok(isSignUpSuccess);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("is-email-used")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailUsed(string email)
        {
            try
            {
                var isUsed = await _userSerivce.IsEmailUsed(email);

                return Ok(isUsed);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("profile")]
        [Authorize(Roles = "Admin,Artist,Customer,Staff,Organizer")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                if (token == null) throw new ArgumentException("Token Null");

                var profile = await _userSerivce.GetUserProfileByToken(token);

                return Ok(profile);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("get-artists")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArtists()
        {
            try
            {
                var profile = await _userSerivce.GetArtists();

                return Ok(profile);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update profile for authenticated user")]
        [Authorize(Roles = "Staff,Admin,Organizer,Artist,Customer")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateRequestModel profile)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _userSerivce.UpdateUserProfile(token, profile);
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
        [Authorize(Roles = "Admin,Organizer,Artist,Customer")]
        public async Task<IActionResult> RemoveStaffAccount(Guid userId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _userSerivce.RemoveAccount(token, userId);
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

        private string GenerateOTP()
        {
            Random rnd = new();

            return (rnd.Next(100000, 999999)).ToString();
        }
    }
}
