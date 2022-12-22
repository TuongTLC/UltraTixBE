using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using UltraTix2022.API.UltraTix2022.Business.Services.PostService;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/post")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet("get-all-posts")]
        [SwaggerOperation(Summary = "Get all posts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                List<PostViewModel> result = await _postService.GetAllPosts();
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }
        [HttpGet("get-posts")]
        [SwaggerOperation(Summary = "Get posts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                string token = string.Empty;
                var isLogin = (Request.Headers)["Authorization"].ToString();
                if(!string.IsNullOrEmpty(isLogin))
                {
                    token = isLogin.Split(" ")[1];
                }
                
                List<PostViewModel> result = await _postService.GetPosts(token);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }
        [HttpGet("get-artist-posts")]
        [SwaggerOperation(Summary = "Get artist posts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArtistPosts()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<PostViewModel> result = await _postService.GetPostsByArtistToken(token);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }
        [HttpPost("add-post-Like")]
        [SwaggerOperation(Summary = "Add a alike to a post")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer,Customer")]
        public async Task<IActionResult> AddPostLike([FromBody] PostLikeModel postID)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _postService.AddPostLike(token, postID.PostID);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

        [HttpPost("add-post-Like-mobile")]
        [SwaggerOperation(Summary = "Add a alike to a post")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer,Customer")]
        public async Task<IActionResult> AddPostLikeMobile(Guid postID)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _postService.AddPostLike(token, postID);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

        [HttpPost("remove-post-like")]
        [SwaggerOperation(Summary = "Remove a alike from a post")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer,Customer")]
        public async Task<IActionResult> RemovePostLIke([FromBody] PostLikeModel postID)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _postService.RemovePostLike(token, postID.PostID);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

        [HttpPost("remove-post-like-mobile")]
        [SwaggerOperation(Summary = "Remove a alike from a post")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer,Customer")]
        public async Task<IActionResult> RemovePostLIkeMobile(Guid postID)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _postService.RemovePostLike(token, postID);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

        [HttpPost("add-post-comment")]
        [SwaggerOperation(Summary = "Add a comment to a post")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer,Customer")]
        public async Task<IActionResult> AddComment(PostCommentInputModelMobile postComment)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _postService.AddCommentMobile(token, postComment);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

        [HttpPost("add-post-comment-mobile")]
        [SwaggerOperation(Summary = "Add a comment to a post in mobile")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer,Customer")]
        public async Task<IActionResult> AddCommentMobile(PostCommentInputModelMobile postComment)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _postService.AddCommentMobile(token, postComment);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

        [HttpGet("get-posts-comment")]
        [SwaggerOperation(Summary = "Get post comment")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostComment(Guid postID)
        {
            try
            {
                List<PostCommentViewModel> result = await _postService.GetPostComment(postID);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }
        [HttpPost("insert-post")]
        [SwaggerOperation(Summary = "Insert a post for admin, artist")]
        [Authorize(Roles = "Admin, Artist")]
        public async Task<IActionResult> InsertPost(PostInsertModel postModel)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _postService.InsertPost(token, postModel);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }
        }
        [HttpPut("update-post")]
        [SwaggerOperation(Summary = "Update a post for admin, artist")]
        [Authorize(Roles = "Admin, Artist")]
        public async Task<IActionResult> UpdatePost(PostUpdateModel postModel)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _postService.UpdatePost(token, postModel);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }
        }
        [HttpGet("get-posts-by-artist")]
        [SwaggerOperation(Summary = "Get posts by artist ID")]
        [AllowAnonymous]
        public async Task<IActionResult> getPostsByArtist(Guid artistID)
        {
            try
            {
                var result = await _postService.GetPostsByArtist(artistID);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }
        }
        [HttpPut("update-post-status")]
        [SwaggerOperation(Summary = "Update a post for admin, artist")]
        [Authorize(Roles = "Admin, Artist")]
        public async Task<IActionResult> UpdatePostStatus(Guid postID, string status)
        {
            if (!status.Equals("Hidden") && !status.Equals("Public"))
            {
                return BadRequest("Status must be Hidden or Public!");
            }
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _postService.UpdatePostStatus(token, postID, status);
                return Ok("Post status updated");

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }
        }

        [HttpGet("get-posts-liked-by-customer")]
        [SwaggerOperation(Summary = "Get posts user liked")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetPostsLiked()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<Guid> result = await _postService.GetPostsLiked(token);
                return Ok(result);

            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

        }

    }
}
