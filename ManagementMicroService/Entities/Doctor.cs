﻿namespace ManagementMicroservice.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Phone { get; set; }
    public int Appointments { get; set; }
    public string Question { get; set; }
}
