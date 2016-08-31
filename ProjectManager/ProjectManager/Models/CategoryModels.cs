using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class CategoryModels
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Category:")]
        public string categoryName { get; set; }
    }
}