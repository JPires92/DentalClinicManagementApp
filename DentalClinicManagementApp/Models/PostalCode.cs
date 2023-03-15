using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DentalClinicManagementApp.Models
{
    public class PostalCode
    {
        [Key]
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; } = "";

        [Display(Name = "Location")]
        public string Location { get; set; } = "";
    }
}
