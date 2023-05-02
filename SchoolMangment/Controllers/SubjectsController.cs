using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMangment.dbContext;
using SchoolMangment.Dtos;
using SchoolMangment.Models;

namespace SchoolMangment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        ApplicationDbcontext _context;


        public SubjectsDto subject;

        public SubjectsController(ApplicationDbcontext context)
        {
            _context = context;
        }



        [HttpGet]
      
        public async Task<IActionResult> AllSubject()
        {
           
            var subject = await _context.Subjects.ToListAsync();
            
            if (!subject.Any())
            {
                return NotFound();
            }
            return Ok(subject);
        }




        [HttpPost]
        public async Task<IActionResult> AddSubject(SubjectsDto dto )
        {
            var departmentExists = await _context.Departments.AnyAsync(d => d.Id == dto.DeptId);

            if (!departmentExists)
            {
                return NotFound();
            }

            var subject = new Subjects
            {
                Name = dto.Name,
                MinimumDegree = dto.MinimumDegree,
                Term = dto.Term,
                Year = dto.Year,
                
                DepartmentsId = dto.DeptId
            };

            try
            {
                await _context.Subjects.AddAsync(subject);
                await _context.SaveChangesAsync();

                return Ok(subject);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("id")]
            public async Task<IActionResult> UpdateSubjecy(SubjectsDto dto, int id)
            {

                var subject = await _context.Subjects.FindAsync(id);
               
                if (subject == null)
                {
                    return NotFound("the Student is not found ");
                }
                if (!_context.Departments.Any(dept => dept.Id == dto.DeptId))
                {
                    return NotFound(" the Departments is not Found");
                }

            subject.Name = dto.Name;
            subject.MinimumDegree = dto.MinimumDegree;
            subject.Term = dto.Term;
            subject.DepartmentsId = dto.DeptId;

                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                    return Ok(subject);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

                
                [HttpDelete("id")]
                public async Task<IActionResult> DeleteSubject(int id)
                {
                    var subjects = await _context.Subjects.FindAsync(id);
                         if (subjects is null)
                         {
                         return NotFound();
                         }
                         try
                         {
                          _context.Remove(subjects);
                          await _context.SaveChangesAsync();
                         return Ok(subjects);
                         } catch (Exception ex)
                         {
                          return BadRequest(ex.Message);
                         }
                }
        }
}
