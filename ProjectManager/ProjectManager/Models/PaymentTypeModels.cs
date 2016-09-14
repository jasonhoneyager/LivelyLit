using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class PaymentTypeModels
    {
        //Per word, per assignment
        [Key]
        public int ID { get; set; }
        [Display(Name = "Payment Type:")]
        public string projectPaymentType { get; set; }
    }
}