using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ViewModels
{
    public class EditBugVm
    {
        public Bug Bug { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
    }
}
