using Microsoft.AspNetCore.Mvc.Rendering;
using New_Ston_Request.Models;
using System.Collections.Generic;

namespace New_Ston_Request.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public IEnumerable<SelectListItem> ApplicationTypeList { get; set; }
    }
}
