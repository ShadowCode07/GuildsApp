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
        private readonly ICommunityRepository _communityRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IPostVoteRepository postVoteRepository, ICommunityMemberRepository communityMemberRepository, ICommunityRepository communityRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _postVoteRepository = postVoteRepository;
            _communityMemberRepository = communityMemberRepository;
            _communityRepository = communityRepository;
            _mapper = mapper;
        }

        private async Task EnsureMemberAsync(int communityId, int userId)
        {
            var isMember = await _communityMemberRepository.IsMemberAsync(userId, communityId);
            if (!isMember)
                throw new UnauthorizedAccessException("User is not a member of this guild.");
        }

        private async Task EnsureCanVoteAsync(int communityId, int userId)
        {
            var community = await _communityRepository.GetByIdAsync(communityId)
                ?? throw new KeyNotFoundException($"Guild {communityId} not found.");

            if (community.IsArchived)
                throw new InvalidOperationException("This guild is archived.");

            if (!community.IsPrivate)
                return;

            await EnsureMemberAsync(communityId, userId);
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
            await Vote(postId, userId, 1);
        }

        public async Task DownVote(int postId, int userId)
        {
            await Vote(postId, userId, -1);
        }

        public async Task<PostVoteResultDto> Vote(int postId, int userId, sbyte value)
        {
            if (value is not (sbyte)1 and not (sbyte)-1)
                throw new ArgumentOutOfRangeException(nameof(value), "Vote value must be 1 or -1.");

            var post = await GetActivePostAsync(postId);
            await EnsureCanVoteAsync(post.CommunityId, userId);

            var existingVote = await _postVoteRepository.GetAsync(postId, userId);
            sbyte currentUserVote;

            if (existingVote != null && existingVote.Value == value)
            {
                var removed = await _postVoteRepository.RemoveAsync(postId, userId);
                if (!removed)
                    throw new InvalidOperationException("Failed to remove vote.");

                currentUserVote = 0;
            }
            else
            {
                var saved = await _postVoteRepository.UpsertAsync(new PostVote
                {
                    PostId = postId,
                    UserId = userId,
                    Value = value
                });

                if (!saved)
                    throw new InvalidOperationException("Failed to save vote.");

                currentUserVote = value;
            }

            var refreshedPost = await GetActivePostAsync(postId);

            return new PostVoteResultDto
            {
                PostId = postId,
                Score = refreshedPost.Score,
                CurrentUserVote = currentUserVote
            };
        }
    }
}
