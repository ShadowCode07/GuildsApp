using GuildsApp.Core.Models;

namespace GuildsApp.Application.Interfaces.Repository
{
    public interface ICommunityMemberRepository
    {
        Task<IReadOnlyList<CommunityMember>?> GetByCommunityAsync(int communityId);
        Task<IReadOnlyList<CommunityMember>?> GetByUserAsync(int userId);
        Task<CommunityMember?> GetAsync(int userId, int communityId);
        Task<bool> AddAsync(CommunityMember member);
        Task<bool> UpdateAsync(CommunityMember member);
        Task<bool> RemoveAsync(int userId, int communityId);
    }
}
