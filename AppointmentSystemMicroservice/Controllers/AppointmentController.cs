using AppointmentSystemMicroservice.DAL;
using AppointmentSystemMicroservice.Entities;
using AppointmentSystemMicroservice.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQProcessor;

namespace AppointmentSystemMicroservice.Controllers;

[Route("[controller]")] 
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly GenericRepository<Appointment> _genericRepository;
    private readonly IGenericMessageProducer _messageProducer;
    public AppointmentController(GenericRepository<Appointment> genericRepository, IGenericMessageProducer messageProducer)
    {
        _genericRepository = genericRepository;
        _messageProducer = messageProducer;
    }

    [HttpPost("CreateAppointment")]
    public async Task<ActionResult> CreateAppointment(Appointment appointment)
    {
        await _genericRepository.AddAsync(appointment);
        try
        {
            _messageProducer.SendingMessage<Appointment>(appointment, "appointmentCreated");
            return Ok("New appointment made successfully!");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("RemoveAppointment")]
    public async Task<ActionResult> RemoveAppointment(int id)
    {
        var entity = await _genericRepository.GetByIdAsync(id);
        _genericRepository.Remove(entity);
        try
        {
            _messageProducer.SendingMessage<Appointment>(entity, "appointmentRemoved");
            return Ok("Appointment removed successfully!");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(ex);
        }
    }
}