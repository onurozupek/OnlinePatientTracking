using AppointmentSystemMicroservice.Entities;
using AppointmentSystemMicroservice.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQProcessor;
using System.Numerics;

namespace AppointmentSystemMicroservice.Controllers;

//[Route("api/[controller]")]
[Route("[controller]")] 
[ApiController]
public class PatientGeneralController : ControllerBase
{
    private readonly GenericRepository<Patient> _genericRepository;
    private readonly GenericRepository<PatientVisit> _genericVisitRepository;
    private readonly IGenericMessageProducer _messageProducer;
    public PatientGeneralController(GenericRepository<Patient> genericRepository, GenericRepository<PatientVisit> genericVisitRepository, IGenericMessageProducer messageProducer)
    {
        _genericRepository = genericRepository;
        _genericVisitRepository = genericVisitRepository;
        _messageProducer = messageProducer;
    }

    [HttpGet("Patients")]
    public async Task<IEnumerable<Patient>> GetPatientInfo()
    {
        return await _genericRepository.GetAllAsync();
    }

    [HttpGet("Patient Visit History")]
    public async Task<IEnumerable<PatientVisit>> GetAllDoctors()
    {
        return await _genericVisitRepository.GetAllAsync();
    }
}
