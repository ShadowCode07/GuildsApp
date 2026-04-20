using AutoMapper;
using GuildsApp.Application.DTOs.PostDTOs;
using GuildsApp.Application.Interfaces;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Core.Models;

namespace GuildsApp.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostVoteRepository _postVoteRepository;
        private readonly ICommunityMemberRepository _communityMemberRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IPostVoteRepository postVoteRepository, ICommunityMemberRepository communityMemberRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _postVoteRepository = postVoteRepository;
            _communityMemberRepository = communityMemberRepository;
            _mapper = mapper;
        }

        private async Task EnsureMemberAsync(int communityId, int userId)
        {
            var isMember = await _communityMemberRepository.IsMemberAsync(userId, communityId);
            if (!isMember)
                throw new UnauthorizedAccessException("User is not a member of this guild.");
        }

        private async Task<Post> GetActivePostAsync(int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId)
                ?? throw new KeyNotFoundException($"Post {postId} not found.");

            if (post.IsDeleted)
                throw new InvalidOperationException("Post has been deleted.");

            return post;
        }

        public async Task<PostDto> Write(int userId, CreatePostDto dto)
        {
            await EnsureMemberAsync(dto.CommunityId, userId);

            var post = _mapper.Map<Post>(dto);
            post.AuthorUserId = userId;

            post.Id = await _postRepository.CreateAsync(post);
            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> Edit(int postId, int userId, UpdatePostDto dto)
        {
            var post = await GetActivePostAsync(postId);

            await EnsureMemberAsync(post.CommunityId, userId);

            if (post.AuthorUserId != userId)
                throw new UnauthorizedAccessException("Only the author can edit this post.");

            _mapper.Map(dto, post);
            post.UpdatedAt = DateTime.UtcNow;

            var updated = await _postRepository.UpdateAsync(post);
            if (!updated)
                throw new InvalidOperationException("Failed to update post.");

            return _mapper.Map<PostDto>(post);
        }

        public async Task Delete(int postId, int userId)
        {
            var post = await GetActivePostAsync(postId);

            await EnsureMemberAsync(post.CommunityId, userId);

            if (post.AuthorUserId != userId)
                throw new UnauthorizedAccessException("Only the author can delete this post.");

            post.IsDeleted = true;
            await _postRepository.UpdateAsync(post);
        }

        public async Task<PostDto> GetPost(int id)
        {
            var post = await GetActivePostAsync(id);
            return _mapper.Map<PostDto>(post);
        }

        public async Task<IEnumerable<PostDto>> GetPostsByGuild(int guildId)
        {
            var posts = await _postRepository.GetByGuildAsync(guildId) ?? Array.Empty<Post>();
            return _mapper.Map<IEnumerable<PostDto>>(posts.Where(p => !p.IsDeleted));
        }

        public async Task<IEnumerable<PostDto>> GetPostsByUser(int userId)
        {
            var posts = await _postRepository.GetByUserAsync(userId) ?? Array.Empty<Post>();
            return _mapper.Map<IEnumerable<PostDto>>(posts.Where(p => !p.IsDeleted));
        }

        public async Task Pin(int postId)
        {
            var post = await GetActivePostAsync(postId);
            post.IsPinned = true;
            await _postRepository.UpdateAsync(post);
        }

        public async Task Unpin(int postId)
        {
            var post = await GetActivePostAsync(postId);
            post.IsPinned = false;
            await _postRepository.UpdateAsync(post);
        }

        public async Task UpVote(int postId, int userId)
        {
            var post = await GetActivePostAsync(postId);
            await EnsureMemberAsync(post.CommunityId, userId);

            var existingVote = await _postVoteRepository.GetAsync(postId, userId);
            if (existingVote?.Value == 1)
                return;

            await _postVoteRepository.UpsertAsync(new PostVote
            {
                PostId = postId,
                UserId = userId,
                Value = 1
            });
        }

        public async Task DownVote(int postId, int userId)
        {
            var post = await GetActivePostAsync(postId);
            await EnsureMemberAsync(post.CommunityId, userId);

            var existingVote = await _postVoteRepository.GetAsync(postId, userId);
            if (existingVote?.Value == -1)
                return;

            await _postVoteRepository.UpsertAsync(new PostVote
            {
                PostId = postId,
                UserId = userId,
                Value = -1
            });
        }
    }
}
