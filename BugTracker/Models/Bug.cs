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

        [Display(Name="Date Submitted")]
        public DateTime DateSubmitted { get; set; }

        [Display(Name = "Date Targeted")]
        public DateTime DateTargeted { get; set; }

        [Display(Name = "Date Resolved")]
        public DateTime DateResolved { get; set; }
        public BugStatus Status { get; set; }

        [Display(Name = "Resolution Description")]
        public string ResolutionDescription { get; set; }

        [Display(Name = "Resolution Commit")]
        public string ResolutionCommit { get; set; }
        public ICollection<Comment> Comments { get; set; }

        [Range(0,10,ErrorMessage = "Severity must be integer from 0-10")]
        public int Severity { get; set; }
        public int ProjectId { get; set; }
    }
}