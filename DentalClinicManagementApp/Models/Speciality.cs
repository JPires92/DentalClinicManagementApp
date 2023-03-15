using System.ComponentModel.DataAnnotations;

namespace DentalClinicManagementApp.Models
{
    public class Speciality
    {
        public int ID { get; set; }

        [Display(Name = "Speciality")]
        public string SpecialityName { get; set; } = "";
    }
}
