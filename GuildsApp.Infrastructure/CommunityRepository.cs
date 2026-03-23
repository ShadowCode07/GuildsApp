using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Infrastructure
{
    public class CommunityRepository : GenericRepository<Community>, ICommunityRepository
    {
        public CommunityRepository(IConfiguration config) : base(config)
        {
        }
    }
}
