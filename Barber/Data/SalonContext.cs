using Barber.Models;
using Microsoft.EntityFrameworkCore;
using Barber.Models;

namespace Barber.Data
{
    public class SalonContext : DbContext
    {
        public SalonContext(DbContextOptions<SalonContext> options) : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja relacji

            // Appointment -> Customer (wiele wizyt na jednego klienta)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment -> Service (jedna usługa na wizytę)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment -> Employee (opcjonalnie przypisany pracownik)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indeksy lub ograniczenia (opcjonalne)
            // Na przykład unikalny indeks na email klienta (jeżeli chcesz)
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Można również wstępnie zseedować dane (opcjonalne)
            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Strzyżenie damskie", Description = "Strzyżenie + modelowanie", DurationInMinutes = 60, Price = 100 },
                new Service { Id = 2, Name = "Strzyżenie męskie", Description = "Strzyżenie klasyczne", DurationInMinutes = 30, Price = 50 }
            );
        }
    }
}
