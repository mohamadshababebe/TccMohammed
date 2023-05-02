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
    public class ExamsController : ControllerBase
    {
        ApplicationDbcontext _context;
        public ExamsController(ApplicationDbcontext context)
        {
            _context = context;
        }

        [HttpGet]
       
        public async Task<IActionResult> getAllExams()
        {
           
            var exams = await _context.Exams.Select(e => new
            {
                e.Id,
                e.Term,
                e.SubjectsId,
                e.DateOnly 
            }).ToListAsync();

          
            if (!exams.Any())
            {
                return NotFound();
            }
            return Ok(exams);
        }


        [HttpPost]
        public async Task<IActionResult> AddExamsDto(ExamsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(dto);
            }

            if (!_context.Subjects.Any(dept => dept.Id == dto.SubjID))
            {
                return NotFound("Subjects not Found");
            }

            var Exams = new Exams
            {
                Date = dto.Date,
                Term = dto.Term,
                SubjectsId = dto.SubjID,
            };

             
            try
            {
                await _context.Exams.AddAsync(Exams);
                
                await _context.SaveChangesAsync();
                return Ok(Exams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateExams(ExamsDto dto, int id)
        {

            var Exams = await _context.Exams.FindAsync(id);
         
            if (Exams == null)
            {
                return NotFound("the Exams is not found ");
            }
            if (!_context.Subjects.Any(dept => dept.Id == dto.SubjID))
            {
                return NotFound("Subjects not Found");
            }

            Exams.Date = dto.Date;
            Exams.Term = dto.Term;

            Exams.SubjectsId = dto.SubjID;

            try
            {
                _context.Update(Exams);
                await _context.SaveChangesAsync();
                return Ok(Exams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteExams(int id)
        {
            var Exams = await _context.Exams.FindAsync(id);
            if (Exams is null)
            {
                return NotFound();
            }
            try
            {
                _context.Remove(Exams);
                await _context.SaveChangesAsync();
                return Ok(Exams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
