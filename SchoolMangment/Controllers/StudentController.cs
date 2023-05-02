using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMangment.dbContext;
using SchoolMangment.Dtos;
using SchoolMangment.Models;
using Students = SchoolMangment.Models.Students;

namespace SchoolMangment.Controllers
{
    [Route("api/[controller]/[action]")]

    [ApiController]
    public class StudentController : ControllerBase
    {
        ApplicationDbcontext _context;
        public StudentController(ApplicationDbcontext context)
        {
            _context = context;
        }

      

            [HttpGet]
        public async Task<IActionResult> getAllStudent()
        {

            var student = await _context.Students.ToListAsync();

            if (!student.Any())
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]

        public async Task<IActionResult> ADDStudent(StudentDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(dto);
            }

            if (!_context.Departments.Any(dept => dept.Id == dto.DeptId))
            {
                return NotFound("THE Department Not found");
            }

            var student = new Students
            {
                FirsName = dto.FirsName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                DepartmentsId = dto.DeptId
            };
            try
            {

                await _context.Students.AddAsync(student);

                await _context.SaveChangesAsync();
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("id")]
        public async Task<IActionResult> UpdateStudent(StudentDto dt, int id)
        {

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            if (!_context.Departments.Any(dep => dep.Id == dt.DeptId))
            {
                return NotFound("THE Department Not found");
            }
            student.FirsName = dt.FirsName;
            student.LastName = dt.LastName;
            student.Phone = dt.Phone;
            student.Email = dt.Email;
            student.DepartmentsId = dt.DeptId;

            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
            [HttpDelete("id")]
            public async Task<IActionResult> DeleteStudent(int id)
            {
            var student = await _context.Students.FindAsync(id);
            if (student is null)
            {
                return NotFound();
            }
            try
            {
                _context.Remove(student);
                await _context.SaveChangesAsync();
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
