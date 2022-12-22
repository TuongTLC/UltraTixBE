using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltraTix2022.API.UltraTix2022.Business.Services.ImgService;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/image")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly ImgService imgService = new ImgService();
        [HttpPost("upload-images")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadFiles(IList<IFormFile> files)
        {
            try
            {
                List<string> urls = await imgService.UploadImage(files);
                return Ok(urls);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("upload-an-image")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadAnFiles(IFormFile file)
        {
            try
            {
                string result = await imgService.UploadAnImage(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-image")]
        [Authorize(Roles = "Admin,User,Staff")]
        public IActionResult GetFile(string fileName)
        {
            try
            {
                FileData fileData = imgService.GetImage(fileName);
                return File(fileData.bytes, fileData.contenType, fileData.name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-images")]
        [Authorize(Roles = "Admin,User,Staff")]
        public async Task<IActionResult> DeleteFile(List<string> imgURLs)
        {
            try
            {
                await imgService.DeleteImages(imgURLs);
                return Ok("Images Deleted!"); ;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

