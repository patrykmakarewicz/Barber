namespace Barber.Models
{
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }

    public class Appointment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
    }

}
