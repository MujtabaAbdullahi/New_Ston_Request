using New_Ston_Request.Models;
using System.Collections.Generic;

namespace New_Ston_Request.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            productList = new List<Product>();
        }
        public ApplicationUser applicationUser { get; set; }
        public IEnumerable<Product> productList { get; set; }
    }
}
