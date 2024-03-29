using System.Text.Json.Serialization;

namespace AppointmentSystemMicroservice.Entities;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }

    [JsonIgnore]
    public List<PatientVisit> PatientVisit { get; set; }
}
