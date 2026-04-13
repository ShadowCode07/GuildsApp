namespace GuildsApp.Web.Models.Quests
{
    public class ActiveQuestViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string RewardText { get; set; } = null!;
        public string StatusText { get; set; } = null!;
    }
}
