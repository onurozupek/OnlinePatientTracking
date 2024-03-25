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

namespace ManagementMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly GenericRepository<Department> _genericRepository;

        public DepartmentsController(AppDbContext context, GenericRepository<Department> genericRepository)
        {
            _context = context;
            _genericRepository = genericRepository;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            return await _genericRepository.GetAllAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartmentById(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            var entity = _genericRepository.Update(department);
            return Ok(entity);
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> AddDepartment(Department department)
        {
            await _genericRepository.AddAsync(department);
            return Ok(department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _genericRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _genericRepository.Remove(department);
            return Ok();
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
