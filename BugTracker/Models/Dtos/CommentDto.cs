using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.Dtos
{
    public class CommentDto
    {
        public int BugId { get; set; }
        public string Text { get; set; }
    }
}
