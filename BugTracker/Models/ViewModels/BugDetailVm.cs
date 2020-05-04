using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ViewModels
{
    public class BugDetailVm
    {
        public Bug Bug { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
