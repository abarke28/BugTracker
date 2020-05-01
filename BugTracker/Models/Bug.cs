using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BugStatus Status { get; set; }
        public string ResolutionDescription { get; set; }
        public string ResolutionCommit { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}
