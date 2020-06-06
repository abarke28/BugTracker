using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Assignee { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime DateTargeted { get; set; }
        public DateTime DateResolved { get; set; }
        public BugStatus Status { get; set; }
        public string ResolutionDescription { get; set; }
        public string ResolutionCommit { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public int Severity { get; set; }
        public int ProjectId { get; set; }
    }
}
