using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace New_Ston_Request.Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("Application Type Name")]
        public string Name { get; set; }
    }
}
