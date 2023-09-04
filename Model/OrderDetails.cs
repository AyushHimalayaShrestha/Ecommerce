using Ecommerce9am.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [ValidateNever]
        public Product Product { get; set; }
        public int OrderHeaderID { get; set; }
        [ForeignKey("OrderHeaderID")]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set;}
        [Required]
        public int Quantity { get; set; }
       
        [Required]
        public int Price { get; set; }  
    }
}
