using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Application.DTOs.PostDTOs
{
    public class UpdatePostDto
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
