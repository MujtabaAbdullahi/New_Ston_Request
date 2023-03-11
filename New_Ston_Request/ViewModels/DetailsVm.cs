using Microsoft.CodeAnalysis;
using New_Ston_Request.Models;

namespace New_Ston_Request.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Product = new Product();
        }

        public Product Product { get; set; }
        public bool ExistsInCart { get; set; }
    }
}    
