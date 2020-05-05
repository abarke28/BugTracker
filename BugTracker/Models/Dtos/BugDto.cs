using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.Dtos
{
    public class BugDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
