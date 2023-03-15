using DentalClinicManagementApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicManagementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProfessionalRole> ProfessionalRoles { get; set; } = default!;
        public DbSet<Speciality> Specialities { get; set; } = default!;
        public DbSet<PostalCode> PostalCodes { get; set; } = default!;
        public DbSet<Professional> Professionals { get; set; } = default!;
        public DbSet<Client> Clients { get; set; } = default!;
        public DbSet<MedicalAppointment> MedicalAppointments { get; set; } = default!;
        public DbSet<Invoice> Invoices { get; set; } = default!;

    }
}