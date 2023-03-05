using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace New_Ston_Request.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Oreder")]
        [Required]
        [Range(1,1000, ErrorMessage ="Display Order must be greater than 0!")]
        public int DisplayOrder { get; set; }
        

    }
}
