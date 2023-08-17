using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PMS_Data_Extraction_from_Machinery_Manual.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter EmployeeId")]
        [Display(Name = "Please Enter EmployeeId")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Please Enter Username")]
        [Display(Name = "Please Enter Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Please Enter Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Access")]
        [Display(Name = "Please Enter Access")]
        public string Access { get; set; }
    }
}
