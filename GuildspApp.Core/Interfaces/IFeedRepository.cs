using GuildsApp.Core.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.Interfaces
{
    public interface IFeedRepository
    {
        Task<IEnumerable<FeedPostQueryModel>> GetMixedFeedAsync(int? userId);
        Task<IEnumerable<FeedPostQueryModel>> GetGuildFeedAsync(string slug, int? userId);
        Task<IEnumerable<FeedGuildQueryModel>> GetSidebarGuildsAsync(int? userId);
        Task<IEnumerable<GuildSearchQueryModel>> SearchGuildsAsync(string query, int? userId);
        Task<FeedGuildQueryModel?> GetGuildBySlugAsync(string slug, int? userId);
    }
}
