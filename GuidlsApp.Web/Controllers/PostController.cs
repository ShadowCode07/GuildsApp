using GuildsApp.Application.DTOs.PostDTOs;
using GuildsApp.Application.Interfaces;
using GuildsApp.Application.Services;
using GuildsApp.Core.Models;
using GuildsApp.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GuildsApp.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User ID claim is missing."));

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            return Ok(post);
        }

        [HttpGet("guild/{guildId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsByGuild(int guildId)
        {
            var posts = await _postService.GetPostsByGuild(guildId);
            return Ok(posts);
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsByUser(int userId)
        {
            var posts = await _postService.GetPostsByUser(userId);
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
        {
            var post = await _postService.Write(GetUserId(), dto);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDto dto)
        {
            var post = await _postService.Edit(id, GetUserId(), dto);
            return Ok(post);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.Delete(id, GetUserId());
            return NoContent();
        }

        [HttpPost("{id}/pin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PinPost(int id)
        {
            await _postService.Pin(id);
            return NoContent();
        }

        [HttpPost("{id}/unpin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnpinPost(int id)
        {
            await _postService.Unpin(id);
            return NoContent();
        }

        [HttpPost("{id}/upvote")]
        public async Task<IActionResult> UpVote(int id)
        {
            await _postService.UpVote(id, GetUserId());
            return NoContent();
        }

        [HttpPost("{id}/downvote")]
        public async Task<IActionResult> DownVote(int id)
        {
            await _postService.DownVote(id, GetUserId());
            return NoContent();
        }
    }
}