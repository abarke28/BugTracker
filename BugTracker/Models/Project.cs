using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Repo { get; set; }

        public ICollection<Bug> Bugs { get; set; }
    }
}
