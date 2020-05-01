using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    [Flags]
    public enum BugStatus
    {
        Open = 1,
        Assigned = 2,
        Resolved = 4,
        Closed = 8,
        Reopened = 16
    }
}