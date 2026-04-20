using GuildsApp.Application.DTOs.CommunityDTOs;
using GuildsApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuildsApp.Web.Controllers
{
    public class CommunityController : Controller
    {
        private readonly ICommunityService _communityService;

        public CommunityController(ICommunityService communityService)
        {
            _communityService = communityService;
        }

        private int GetUserId()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
                throw new UnauthorizedAccessException("User ID claim is missing.");

            return int.Parse(userIdClaim);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var communities = await _communityService.GetAllAsync();
            return View(communities);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string slug)
        {
            var community = await _communityService.GetBySlugAsync(slug);
            if (community == null)
                return NotFound();

            return RedirectToAction("Guild", "Feed", new { slug = community.Slug });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View(new CreateCommunityDto());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCommunityDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var community = await _communityService.CreateAsync(GetUserId(), dto);
                return RedirectToAction("Guild", "Feed", new { slug = community.Slug });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var community = await _communityService.GetByIdAsync(id);
            if (community == null)
                return NotFound();

            var dto = new UpdateCommunityDto
            {
                Name = community.Name,
                Description = community.Description,
                Rules = community.Rules,
                IsPrivate = community.IsPrivate,
                IsArchived = community.IsArchived
            };

            return View(dto);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCommunityDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _communityService.UpdateAsync(id, GetUserId(), dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            try
            {
                await _communityService.JoinAsync(id, GetUserId());
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave(int id)
        {
            try
            {
                await _communityService.LeaveAsync(id, GetUserId());
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(int id)
        {
            try
            {
                await _communityService.ArchiveAsync(id, GetUserId());
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}   
