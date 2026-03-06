using GuildsApp.Application.Interfaces.Repository;
using GuildsAPP.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Infrastructure
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(IConfiguration config) : base(config)
        {
        }
    }
}
