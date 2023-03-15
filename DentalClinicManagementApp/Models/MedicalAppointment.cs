using System.ComponentModel.DataAnnotations;

namespace DentalClinicManagementApp.Models
{
    public class MedicalAppointment
    {
        public int ID { get; set; }

        [Display(Name = "Date of appointment")]
        public DateTime DateOfAppointment { get; set; }

        [Display(Name = "Observations")]
        public string Observations { get; set; } = "";

        [Display(Name = "Performed")]
        public bool Performed { get; set; }

        /*FK*/
        [Display(Name = "Client")]
        public int ClientID { get; set; }
        public Client? Client { get; set; }

        [Display(Name = "Doctor")]
        public int ProfessionalID { get; set; }
        public Professional? Professional { get; set; }


    }
}
