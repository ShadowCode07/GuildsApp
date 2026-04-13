using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.Models
{
    [Table("QuestEnrollment")]
    public class QuestEnrollment
    {
        public int QuestId { get; set; }
        public int UserId { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "enrolled";
        public DateTime? SubmittedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
