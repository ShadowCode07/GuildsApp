using GuidlsMVC.Entities;
using GuidlsMVC.Interfaces.Repositories;

namespace GuidlsMVC.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(IConfiguration config) : base(config)
        {
        }
    }
}
