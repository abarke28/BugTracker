using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class DashboardStack
    {
        public BugStatus AssociatedStatus { get; set; }
        public string Title { get; set; }
        public List<Bug> Bugs { get; set; }
    }
}