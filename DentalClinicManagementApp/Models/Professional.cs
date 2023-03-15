using System.ComponentModel.DataAnnotations;

namespace DentalClinicManagementApp.Models
{
    public class Professional
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "NIF")]
        public int NIF { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; } = "";

        /*FK*/
        [Display(Name = "Zip Code")]
        public string PostalCodeID { get; set; } = "";
        public PostalCode? PostalCode { get; set; }

        [Display(Name = "Role")]
        public int ProfessionalRoleID { get; set; }
        public ProfessionalRole? ProfessionalRole { get;set; }

        [Display(Name="Speciality")]
        public int? SpecialityID { get; set; } 
        public Speciality? Speciality { get; set; }


    }
}
