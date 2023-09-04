using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce9am.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage="Product Name is required.")]
        [MinLength(3, ErrorMessage ="Product Name must be atleast 3 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Product Price is required.")]
        [Range(1,1000000, ErrorMessage="Price must be between 1000000")]
        public int Price { get; set; }
        
        public string Description { get; set; }

        [Required(ErrorMessage ="Stock is required")]
        [Range(1,1000, ErrorMessage ="Stock must be between 1 and 1000")]
        public int Stock { get; set; }

        [Range(1,5, ErrorMessage="Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
        
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
    }
}
