﻿using System;
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
        public ICollection<Comment> Comments { get; set; }

        // Foreign Key for Project
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
