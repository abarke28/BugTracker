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
        public IEnumerable<SelectListItem> CoreStatuses { get; set; }
        public IEnumerable<SelectListItem> Severities { get; set; }
        public bool Assigned { get; set; }
        public bool Reopened { get; set; }

        public EditBugVm()
        {
            CoreStatuses = new List<SelectListItem>
            {
                new SelectListItem {Text = BugStatus.Open.ToString(), Value = BugStatus.Open.ToString()},
                new SelectListItem {Text = BugStatus.Resolved.ToString(), Value = BugStatus.Resolved.ToString()},
                new SelectListItem {Text = BugStatus.Closed.ToString(), Value = BugStatus.Closed.ToString()}
            };

            Severities = new List<SelectListItem>
            (
                Enumerable.Range(0,11).Select(i=>new SelectListItem {Text=i.ToString(), Value = i.ToString() })
            );
        }
    }
}