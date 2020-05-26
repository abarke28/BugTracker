using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ViewModels
{
    public class HomeIndexVm
    {
        public IList<Bug> Bugs { get; set; }
        public IList<Project> Projects { get; set; }
        public int CardCount { get; set; }
    }
}
