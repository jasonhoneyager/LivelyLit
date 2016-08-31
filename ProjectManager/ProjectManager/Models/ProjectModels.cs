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
        public string projectName { get; set; }
        public string projectCategory { get; set; }
        [DataType(DataType.MultilineText)]
        public string projectDescription { get; set; }
        public DateTime projectRequestedDueDate { get; set; }
        public string projectOfferedPaymentType { get; set; } //per word, per assignment
        public string projectOfferedPaymentAmount { get; set; }
        public string projectPaymentMethod { get; set; } // paypal, check, escrow (paypal adaptive), etc...

    }
}