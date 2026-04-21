namespace GuildsApp.Application.DTOs.PostDTOs
{
    public class PostVoteResultDto
    {
        public int PostId { get; set; }
        public int Score { get; set; }
        public sbyte CurrentUserVote { get; set; }
    }
}
