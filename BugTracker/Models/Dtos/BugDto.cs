using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.Dtos
{
    public class BugDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public int ProjectId { get; set; }

        [Range(0,10,ErrorMessage = "Severity must be in range 0-10")]
        public int Severity { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
