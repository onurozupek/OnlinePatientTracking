using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagementMicroservice.DAL;
using ManagementMicroservice.Entities;
using ManagementMicroservice.Repositories;
using ManagementMicroservice.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ManagementMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly GenericRepository<Doctor> _genericRepository;
        private readonly IMessageProducer _messageProducer;
        private readonly IMessageConsumer _messageConsumer;

        public DoctorController(AppDbContext context, GenericRepository<Doctor> genericRepository, IMessageProducer messageProducer, IMessageConsumer messageConsumer)
        {
            _context = context;
            _genericRepository = genericRepository;
            _messageProducer = messageProducer;
            _messageConsumer = messageConsumer;
        }

        [HttpGet]
        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            var message = await _messageConsumer.ConsumeMessage("doctorAdded");
            return await _genericRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctorById(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(int id, Doctor Doctor)
        {
            if (id != Doctor.Id)
            {
                return BadRequest();
            }

            var entity = _genericRepository.Update(Doctor);
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult> AddDoctor(Doctor doctor)
        {
            await _genericRepository.AddAsync(doctor);
            _messageProducer.SendingMessage<Doctor>(doctor, "doctorAdded");
            return Ok("Doctor added successfully!");
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var Doctor = await _genericRepository.GetByIdAsync(id);
            if (Doctor == null)
            {
                return NotFound();
            }

            _genericRepository.Remove(Doctor);
            return Ok();
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
