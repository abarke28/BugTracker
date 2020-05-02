using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class BugVm
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ResolutionCommit { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public int ProjectId { get; set; }
    }
}
