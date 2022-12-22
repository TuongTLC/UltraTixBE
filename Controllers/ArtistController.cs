using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using UltraTix2022.API.UltraTix2022.Business.Services.ArtistService;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Artist;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/artist")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet("get-artist-accounts")]
        [Authorize(Roles = "Admin,Organizer,Staff")]
        public async Task<IActionResult> GetArtistAccounts()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _artistService.GetArtistAccounts(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("follow-an-artist")]
        [SwaggerOperation(Summary = "Follow or unfollow an artist")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer,Customer")]
        public async Task<IActionResult> FollowArtist([FromBody] FollowArtistRequestModel artist)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _artistService.FollowArtist(token, artist.ArtistId);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

        [HttpPost("follow-artist")]
        [SwaggerOperation(Summary = "Follow or unfollow an artist")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer,Customer")]
        public async Task<IActionResult> FollowArtist(Guid artistId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _artistService.FollowArtist(token, artistId);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

        [HttpGet("get-artists-followed")]
        [SwaggerOperation(Summary = "Get list artists followed for user")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer,Customer")]
        public async Task<IActionResult> GetArtistsFollowed()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _artistService.GetArtistsFollowed(token);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

        [HttpGet("get-artist-list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArtistList()
        {
            try
            {
                string token = string.Empty;
                var isLogin = (Request.Headers)["Authorization"].ToString();
                if (!string.IsNullOrEmpty(isLogin))
                {
                    token = isLogin.Split(" ")[1];
                }
                var result = await _artistService.GetArtistList(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("insert-account")]
        [SwaggerOperation(Summary = "Insert artist account for admin role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertOrganizer([FromBody] ArtistInsertModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                bool result = await _artistService.InsertArtist(token, request);
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

    }
}
