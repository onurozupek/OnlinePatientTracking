namespace AppointmentSystemMicroservice.Entities;

public class Appointment
{
    public int Id { get; set; }
    public string DoctorId { get; set; }
    public string DoctorName { get; set; }
    public string PatientName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Department { get; set; }
}
