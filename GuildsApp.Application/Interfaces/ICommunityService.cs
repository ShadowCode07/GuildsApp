using GuildsApp.Application.DTOs.CommunityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.Interfaces
{
    public interface ICommunityService
    {
        Task<CommunityDto> CreateAsync(int userId, CreateCommunityDto dto);
        Task<CommunityDto?> GetByIdAsync(int id);
        Task<CommunityDto?> GetBySlugAsync(string slug);
        Task<IEnumerable<CommunityListItemDto>> GetAllAsync();
        Task<IEnumerable<CommunityListItemDto>> GetByUserAsync(int userId);
        Task UpdateAsync(int communityId, int userId, UpdateCommunityDto dto);
        Task ArchiveAsync(int communityId, int userId);
        Task JoinAsync(int communityId, int userId);
        Task LeaveAsync(int communityId, int userId);
    }
}
