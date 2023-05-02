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
    public class SubjectLecturesController : ControllerBase
    {


        ApplicationDbcontext _context;
        public SubjectLecturesController(ApplicationDbcontext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> getAllSubjectLectures()
        {
           
            var subjectlectures = await _context.SubjectLectures.ToListAsync();
           
            if (!subjectlectures.Any())
            {
                return NotFound();
            }
            return Ok(subjectlectures);
        }

       
        [HttpPost]
        public async Task<IActionResult> AddSubjectLectures(SubjectLecturesDto dt)
        {
            


            if (!ModelState.IsValid)
            {
                return BadRequest(dt);
            }

            if (!_context.Subjects.Any(dept => dept.Id == dt.SubjId))
            {
                return NotFound(" Subjects not Found");
            }

            var subjectLectures = new SubjectLectures
            {

                Content = dt.Content,
                Title = dt.Title,
                SubjectsId = dt.SubjId,

            };

           
            try
            {
                
                await _context.SubjectLectures.AddAsync(subjectLectures);
               
                await _context.SaveChangesAsync();
                return Ok(subjectLectures);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("id")]
        public async Task<IActionResult> UpdatetLectures(SubjectLecturesDto dt, int id)
        {

            var SubjectLectures = await _context.SubjectLectures.FindAsync(id);
           
            if (SubjectLectures == null)
            {
                return NotFound("the Subject is not found ");
            }
            if (!_context.Subjects.Any(dept => dept.Id == dt.SubjId))
            {
                return NotFound("dept not Found");
            }

            SubjectLectures.Content = dt.Content;
            SubjectLectures.Title = dt.Title;
            SubjectLectures.SubjectsId = dt.SubjId;

            try
            {
                _context.Update(SubjectLectures);
                await _context.SaveChangesAsync();
                return Ok(SubjectLectures);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteSubjectLectures(int id)
        {
            var SubjectLectures = await _context.SubjectLectures.FindAsync(id);
            if (SubjectLectures is null)
            {
                return NotFound();
            }
            try
            {
                _context.Remove(SubjectLectures);
                await _context.SaveChangesAsync();
                return Ok(SubjectLectures);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
