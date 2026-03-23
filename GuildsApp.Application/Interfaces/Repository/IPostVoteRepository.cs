using GuildsApp.Core.Models;

namespace GuildsApp.Application.Interfaces.Repository
{
    public interface IPostVoteRepository
    {
        Task<IReadOnlyList<PostVote>?> GetByPostAsync(int postId);
        Task<PostVote?> GetAsync(int postId, int userId);
        Task<bool> UpsertAsync(PostVote vote);
        Task<bool> RemoveAsync(int postId, int userId);
    }
}
