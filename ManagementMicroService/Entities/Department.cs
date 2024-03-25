using System.Text.Json.Serialization;

namespace ManagementMicroservice.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
}