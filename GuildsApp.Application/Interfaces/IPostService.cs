using GuildsApp.Application.DTOs.PostDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.Interfaces
{
    public interface IPostService
    {
        Task<PostDto> Write(int userId, CreatePostDto dto);
        Task<PostDto> Edit(int postId, int userId, UpdatePostDto dto);
        Task Delete(int postId, int userId);
        Task<PostDto> GetPost(int id);
        Task<IEnumerable<PostDto>> GetPostsByGuild(int guild);
        Task<IEnumerable<PostDto>> GetPostsByUser(int userId);
        Task Pin(int postId);
        Task Unpin(int postId);
        Task UpVote(int postId, int userId);
        Task DownVote(int postId, int userId);
        Task<PostVoteResultDto> Vote(int postId, int userId, sbyte value);
    }
}
