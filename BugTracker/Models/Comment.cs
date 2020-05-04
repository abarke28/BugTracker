using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Text { get; set; }
        public string SubmittedBy { get; set; }
        public int BugId { get; set; }
    }
}
