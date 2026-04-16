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
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        private int GetUserId() =>
            int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value
                ?? throw new UnauthorizedAccessException("User ID claim is missing."));

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPost(id);
            return View(post);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Guild(int id)
        {
            var posts = await _postService.GetPostsByGuild(id);
            return View(posts);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> User(int id)
        {
            var posts = await _postService.GetPostsByUser(id);
            return View(posts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var dto = new CreatePostDto();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                var post = await _postService.Write(GetUserId(), dto);
                return RedirectToAction(nameof(Details), new { id = post.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAjax([FromBody] CreatePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new
                {
                    success = false,
                    message = "Invalid post data.",
                    errors
                });
            }

            try
            {
                var post = await _postService.Write(GetUserId(), dto);

                return Ok(new
                {
                    success = true,
                    id = post.Id,
                    message = "Post created successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postService.GetPost(id);

            var dto = new UpdatePostDto
            {
                Title = post.Title,
                Body = post.Body
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _postService.Edit(id, GetUserId(), dto);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.Delete(id, GetUserId());
            return RedirectToAction("Index", "Feed");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pin(int id)
        {
            await _postService.Pin(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unpin(int id)
        {
            await _postService.Unpin(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpVote(int id)
        {
            await _postService.UpVote(id, GetUserId());
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownVote(int id)
        {
            await _postService.DownVote(id, GetUserId());
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}