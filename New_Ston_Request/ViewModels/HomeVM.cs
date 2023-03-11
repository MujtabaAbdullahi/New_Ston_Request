using Microsoft.AspNetCore.Mvc.Rendering;
using New_Ston_Request.Models;
using System.Collections.Generic;

namespace New_Ston_Request.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}
