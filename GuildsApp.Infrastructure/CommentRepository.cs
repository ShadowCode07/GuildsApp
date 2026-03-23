using GuildsApp.Core.Models;
using Microsoft.Extensions.Configuration;

namespace GuildsApp.Infrastructure
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config)
        {
        }
    }
}
