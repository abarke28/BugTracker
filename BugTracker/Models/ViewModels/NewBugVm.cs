using BugTracker.Models.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IEnumerable<SelectListItem> Severities { get; set; }

        public NewBugVm()
        {
            Severities = new List<SelectListItem>
            (
                Enumerable.Range(0, 11).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() })
            );
        }
    }
}
