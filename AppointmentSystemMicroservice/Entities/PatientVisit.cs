using System.Text.Json.Serialization;

namespace AppointmentSystemMicroservice.Entities;

public class PatientVisit
{
    public int Id { get; set; }

    [JsonIgnore]
    public Patient Patient { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    public DateTime VisitDate { get; set; }
}
