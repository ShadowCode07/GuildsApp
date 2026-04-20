using GuildsApp.Application.DTOs.FeedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.Interfaces
{
    public interface IFeedService
    {
        Task<IEnumerable<FeedPostDto>> GetMixedFeedAsync(int? userId);
        Task<IEnumerable<FeedPostDto>> GetGuildFeedAsync(string slug, int? userId);
        Task<IEnumerable<FeedGuildDto>> GetSidebarGuildsAsync(int? userId);
        Task<IEnumerable<GuildSearchDto>> SearchGuildsAsync(string query, int? userId);
        Task<FeedGuildDto?> GetGuildBySlugAsync(string slug, int? userId);
    }
}
