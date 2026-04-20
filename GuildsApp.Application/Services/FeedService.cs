using AutoMapper;
using GuildsApp.Application.DTOs.FeedDTOs;
using GuildsApp.Application.Interfaces;
using GuildsApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.Services
{
    public class FeedService : IFeedService
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IMapper _mapper;

        public FeedService(IFeedRepository feedRepository, IMapper mapper)
        {
            _feedRepository = feedRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FeedPostDto>> GetMixedFeedAsync(int? userId)
        {
            var posts = await _feedRepository.GetMixedFeedAsync(userId);
            return _mapper.Map<IEnumerable<FeedPostDto>>(posts);
        }

        public async Task<IEnumerable<FeedPostDto>> GetGuildFeedAsync(string slug, int? userId)
        {
            var posts = await _feedRepository.GetGuildFeedAsync(slug, userId);
            return _mapper.Map<IEnumerable<FeedPostDto>>(posts);
        }

        public async Task<IEnumerable<FeedGuildDto>> GetSidebarGuildsAsync(int? userId)
        {
            var guilds = await _feedRepository.GetSidebarGuildsAsync(userId);
            return _mapper.Map<IEnumerable<FeedGuildDto>>(guilds);
        }

        public async Task<IEnumerable<GuildSearchDto>> SearchGuildsAsync(string query, int? userId)
        {
            var guilds = await _feedRepository.SearchGuildsAsync(query, userId);
            return _mapper.Map<IEnumerable<GuildSearchDto>>(guilds);
        }

        public async Task<FeedGuildDto?> GetGuildBySlugAsync(string slug, int? userId)
        {
            var guild = await _feedRepository.GetGuildBySlugAsync(slug, userId);
            if (guild == null)
                return null;

            return _mapper.Map<FeedGuildDto>(guild);
        }
    }
}
