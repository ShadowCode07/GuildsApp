using AutoMapper;
using GuildsApp.Application.DTOs.CommunityDTOs;
using GuildsApp.Application.Interfaces;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly ICommunityMemberRepository _communityMemberRepository;
        private readonly IMapper _mapper;

        public CommunityService(ICommunityRepository communityRepository, ICommunityMemberRepository communityMemberRepository, IMapper mapper)
        {
            _communityRepository = communityRepository;
            _communityMemberRepository = communityMemberRepository;
            _mapper = mapper;
        }

        public async Task<CommunityDto> CreateAsync(int userId, CreateCommunityDto dto)
        {
            var slug = dto.Slug.Trim().ToLower();
            var existingBySlug = await _communityRepository.GetBySlugAsync(slug);

            if (existingBySlug != null)
                throw new Exception("A guild with this slug already exists.");

            var community = _mapper.Map<Community>(dto);
            community.CreatedByUserId = userId;

            var newId = await _communityRepository.CreateAsync(community);
            community.Id = newId;

            await _communityMemberRepository.AddAsync(new CommunityMember
            {
                UserId = userId,
                CommunityId = newId,
                Role = "owner",
                JoinedAt = DateTime.UtcNow,
                IsBanned = false
            });

            return _mapper.Map<CommunityDto>(community);
        }

        public async Task<CommunityDto?> GetByIdAsync(int id)
        {
            var community = await _communityRepository.GetByIdAsync(id);
            if (community == null)
                return null;

            return _mapper.Map<CommunityDto>(community);
        }

        public async Task<CommunityDto?> GetBySlugAsync(string slug)
        {
            var community = await _communityRepository.GetBySlugAsync(slug.Trim().ToLower());
            if (community == null)
                return null;

            return _mapper.Map<CommunityDto>(community);
        }

        public async Task<IEnumerable<CommunityListItemDto>> GetAllAsync()
        {
            var communities = await _communityRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CommunityListItemDto>>(communities);
        }

        public async Task<IEnumerable<CommunityListItemDto>> GetByUserAsync(int userId)
        {
            var communities = await _communityRepository.GetByUserAsync(userId);
            return _mapper.Map<IEnumerable<CommunityListItemDto>>(communities);
        }

        public async Task UpdateAsync(int communityId, int userId, UpdateCommunityDto dto)
        {
            var community = await _communityRepository.GetByIdAsync(communityId);
            if (community == null)
                throw new Exception("Guild not found.");

            if (community.CreatedByUserId != userId)
                throw new Exception("You are not allowed to edit this guild.");

            _mapper.Map(dto, community);
            await _communityRepository.UpdateAsync(community);
        }

        public async Task ArchiveAsync(int communityId, int userId)
        {
            var community = await _communityRepository.GetByIdAsync(communityId);
            if (community == null)
                throw new Exception("Guild not found.");

            if (community.CreatedByUserId != userId)
                throw new Exception("You are not allowed to archive this guild.");

            community.IsArchived = true;
            await _communityRepository.UpdateAsync(community);
        }

        public async Task JoinAsync(int communityId, int userId)
        {
            var community = await _communityRepository.GetByIdAsync(communityId);
            if (community == null)
                throw new Exception("Guild not found.");

            var exists = await _communityMemberRepository.IsMemberAsync(userId, communityId);
            if (exists)
                throw new Exception("You are already a member of this guild.");

            await _communityMemberRepository.AddAsync(new CommunityMember
            {
                UserId = userId,
                CommunityId = communityId,
                Role = "member",
                JoinedAt = DateTime.UtcNow,
                IsBanned = false
            });
        }

        public async Task LeaveAsync(int communityId, int userId)
        {
            var community = await _communityRepository.GetByIdAsync(communityId);
            if (community == null)
                throw new Exception("Guild not found.");

            if (community.CreatedByUserId == userId)
                throw new Exception("The guild owner cannot leave the guild.");

            var existing = await _communityMemberRepository.GetAsync(userId, communityId);
            if (existing == null)
                throw new Exception("You are not a member of this guild.");

            await _communityMemberRepository.RemoveAsync(userId, communityId);
        }
    }
}