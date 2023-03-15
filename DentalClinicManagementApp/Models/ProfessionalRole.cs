using System.ComponentModel.DataAnnotations;

namespace DentalClinicManagementApp.Models
{
    public class ProfessionalRole
    {
        public int ID { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; } = "";
    }
}
