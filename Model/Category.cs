using System.ComponentModel.DataAnnotations;

namespace Ecommerce9am.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage ="Category Name is required")]
        [MinLength(3, ErrorMessage ="Category Name must be atleast 3 Characters")]
        public string CategoryName { get; set; }= string.Empty;
    }
}
