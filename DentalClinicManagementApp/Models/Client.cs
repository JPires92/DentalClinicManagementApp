using System.ComponentModel.DataAnnotations;

namespace DentalClinicManagementApp.Models
{
    public class Client
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        [Display(Name = "NIF")]
        public int NIF { get; set; }

        [Display(Name = "Health Insurance Company ")]
        public string? HealthInsuranceCompany { get; set; } = "";

        [Display(Name = "Address")]
        public string Address { get; set; } = "";

        /*FK*/
        [Display(Name = "Zip Code")]
        public string PostalCodeID { get; set; } = "";
        public PostalCode? PostalCode { get; set; }
    }
}
