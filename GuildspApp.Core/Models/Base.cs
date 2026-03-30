using System.ComponentModel.DataAnnotations;

namespace GuildsApp.Core.Models
{
    public abstract class Base
    {
        [Key]
        public int Id { get; set; }
    }
}
