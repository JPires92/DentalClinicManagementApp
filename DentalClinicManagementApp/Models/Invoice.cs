using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinicManagementApp.Models
{
    public class Invoice
    {
        public int ID { get; set; }

        [Display(Name = "Invoice number")]
        public string InvoiceNumber { get; set; } = "";

        [Display(Name = "Description")]
        public string Description { get; set; } = "";

        [Display(Name = "Final Value")]
        [Column(TypeName = "money")]
        public decimal FinalValue { get; set; }

        [Display(Name = "Paid?")]
        public bool State { get; set; }

        [Display(Name = "Payment date")]
        public DateTime? PaymentDate { get; set; }

        /*FK*/
        [Display(Name = "Client")]
        public int ClientID { get; set; }
        public Client? Client { get; set; }

        [Display(Name = "Medical Appointment")]
        public int MedicalAppointmentID { get; set; }
        public MedicalAppointment? MedicalAppointment { get; set; }
    }
}
