using AppointmentSystemMicroservice.DAL;
using AppointmentSystemMicroservice.Entities;
using AppointmentSystemMicroservice.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQConsumer;

namespace AppointmentSystemMicroservice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly GenericRepository<Appointment> _genericRepository;
    private readonly IGenericMessageProducer _messageProducer;
    public AppointmentController(AppDbContext dbContext, GenericRepository<Appointment> genericRepository, IGenericMessageProducer messageProducer)
    {
        _context = dbContext;
        _genericRepository = genericRepository;
        _messageProducer = messageProducer;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAppointment(Appointment appointment)
    {
        await _genericRepository.AddAsync(appointment);
        try
        {
            await _context.SaveChangesAsync();
            _messageProducer.SendingMessage<Appointment>(appointment, "appointmentCreated");
            return Ok("New appointment made successfully!");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(ex);
        }
    }
}