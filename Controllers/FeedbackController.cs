using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UltraTix2022.API.UltraTix2022.Business.Services.FeedbackServices;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Feedback;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/feedback")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpGet("get-feedbacks")]
        [SwaggerOperation(Summary = "Get all feedback")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                List<FeedbackViewModel> result = await _feedbackService.GetFeedbacks();
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }
        }
        [HttpPut("update-feedback-status")]
        [SwaggerOperation(Summary = "Update feedback Status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFeedbackStatus(Guid FeedbackID, string status)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _feedbackService.UpdateFeedbackStatus(token, FeedbackID, status);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }
        }
        [HttpPost("insert-feedback")]
        [SwaggerOperation(Summary = "Insert a feedback")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertFeedback(FeedbackInsertModel feedback)
        {
            try
            {

                var result = await _feedbackService.InsertFeedback(feedback);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }
        }
        [HttpGet("get-feedback-types")]
        [SwaggerOperation(Summary = "get feedback types")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeedbackTypes()
        {
            try
            {
                var result = await _feedbackService.GetFeedbackType();
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }
        }
    }
}

