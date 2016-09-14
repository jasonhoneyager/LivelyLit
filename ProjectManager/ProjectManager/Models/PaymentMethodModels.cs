using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class PaymentMethodModels
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Payment Method:")]
        public string projectPaymentMethod { get; set; }
    }
}