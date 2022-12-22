using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltraTix2022.API.UltraTix2022.Business.Services.ArtistRequestServices;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ArtistRequest;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/request")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class RequestController : ControllerBase
	{
        private readonly IArtistRequestService _artistRequestService;
		public RequestController(IArtistRequestService artistRequestService)
		{
            _artistRequestService = artistRequestService;
		}
        [HttpGet("get-artist-request")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _artistRequestService.GetRequest(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("update-to-artist")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateToArtist(Guid requestID, Guid userID, string status)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _artistRequestService.UpdateArtistAccount(requestID, userID, status, token);
                return (result != false) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("insert-artist-request")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> InsertArtistRequest(ArtistRequestInsertModel artistRequestInsertModel)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _artistRequestService.CreateArtistRequest(token, artistRequestInsertModel);
                return (result != false) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    
}

