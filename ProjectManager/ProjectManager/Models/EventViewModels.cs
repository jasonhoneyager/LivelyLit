using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class CalendarEventViewModels
    {
        public ProjectModels Event { get; set; }
    }

    public class EventViewModel
    {
        public string title { get; set; }
        public string start { get; set; }
        public string color { get; set; }
    }

}