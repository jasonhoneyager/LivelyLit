using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManager.Models
{
    public class DropDownViewModel
    {
        public ProjectModels Project { get; set; }
        public string selectedCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string selectedStatus { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
    }

    public class CreateProjectViewModel
    {
        public DropDownViewModel ddvm { get; set; }
    }
}