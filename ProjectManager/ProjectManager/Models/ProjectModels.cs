using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class ProjectModels
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Project Title:")]
        public string projectName { get; set; }
        [Display(Name = "Category:")]
        public IEnumerable<CategoryModels> CategoryID { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Project Description:")]
        public string projectDescription { get; set; }
        [Display(Name = "Requested Due Date:")]
        public DateTime projectRequestedDueDate { get; set; }
        [Display(Name = "Payment Type:")]
        public string projectOfferedPaymentType { get; set; } //per word, per assignment
        [Display(Name = "Payment Amount:")]
        public string projectOfferedPaymentAmount { get; set; }
        [Display(Name = "Payment Method:")]
        public string projectPaymentMethod { get; set; } // paypal, check, escrow (paypal adaptive), etc...
        [Display(Name = "Project Status:")]
        public IEnumerable<ProjectStatusModels> ProjectStatusID { get; set; }

    }
}