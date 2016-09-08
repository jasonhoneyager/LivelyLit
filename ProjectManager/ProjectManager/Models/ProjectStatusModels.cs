using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class ProjectStatusModels
    {
        [Key]
        public int ID { get; set; }
        public string projectStatusName { get; set; }
    }
}