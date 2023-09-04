using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce9am.Models
{
    public class ProductViewModel
    {
        public Product product { get;set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem>categoryList { get; set; }
    }
}
