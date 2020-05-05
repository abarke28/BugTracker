using BugTracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ViewModels
{
    public class NewBugVm
    {
        public BugDto Bug { get; set; }
        public string ProjectName { get; set; }
    }
}
