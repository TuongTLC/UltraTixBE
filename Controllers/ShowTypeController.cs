using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowTypeService;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowType;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/show-type")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ShowTypeController : ControllerBase
    {
        private readonly IShowTypeService _showTypeService;

        public ShowTypeController(IShowTypeService showTypeService)
        {
            _showTypeService = showTypeService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetShowTypes()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTypeService.GetShowType(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("insert-show-type")]
        [SwaggerOperation(Summary = "Insert show type account for admin role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertShowStaff([FromBody] ShowTypeRequestInsertModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                bool result = await _showTypeService.InsertShowType(token, request);
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
