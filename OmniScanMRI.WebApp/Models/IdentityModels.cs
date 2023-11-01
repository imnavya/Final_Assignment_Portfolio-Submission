using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OmniScanMRI.WebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class OmniScanContext : IdentityDbContext<ApplicationUser>
    {
        public OmniScanContext()
            : base("OmniScanMRIDb", throwIfV1Schema: false)
        {
        }

        public static OmniScanContext Create()
        {
            return new OmniScanContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointments>()
                .HasRequired(a => a.Patient)
                .WithMany() 
                .HasForeignKey(a => a.PatientID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Appointments>()
                .HasRequired(a => a.Doctor)
                .WithMany() 
                .HasForeignKey(a => a.DoctorID)
                .WillCascadeOnDelete(false); 

            modelBuilder.Entity<Appointments>()
                .HasOptional(a => a.Administrator)
                .WithMany() 
                .HasForeignKey(a => a.AdminID)
                .WillCascadeOnDelete(false); 
        }

        // Existing DbSets
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Administrators> Administrators { get; set; }

        public DbSet<ScanDetails> ScanImages { get; set; }
        public DbSet<TrackEmail> EmailLogs { get; set; }

        public DbSet<Appointments> Appointments { get; set; }

        public DbSet<Ratings> Ratings { get; set; }

        public DbSet<AppointmentsAvailability> ApptSlots { get; set;}
    }

}