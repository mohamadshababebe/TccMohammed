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
    public class DepartmentsController : ControllerBase
    {
        private  ApplicationDbcontext _context;
        public DepartmentsController(ApplicationDbcontext context)
        {
            _context= context;
        }
        

        [HttpGet]
       
        public async Task<IActionResult> AllDepartmentst()
        {
           
            var depts =await _context.Departments.ToListAsync();
          
            if (!depts.Any())
            {
                return NotFound();
            }
            return Ok(depts);
        }
      
        [HttpPost]
        public async Task<IActionResult>AddDept(DeptDto dto)
        {
         

         
            if (!ModelState.IsValid)
            {
                return BadRequest(dto);
            }
            var dept = new Departments
            {
                Name = dto.Name,
            };
           
            try
            {
                
                await _context.Departments.AddAsync(dept);
               
                await  _context.SaveChangesAsync();
                return Ok(dept);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }
        [HttpPut("id")]
        public async Task<IActionResult>UpdateDept(DeptDto dto,int id )
        {
           
            var dept = await _context.Departments.FindAsync(id);

            
            _context.Departments.Where(dept => dept.Id == id);
            {
                dept.Name = dto.Name;
            }
            try
            {
                _context.Update(dept);
                await _context.SaveChangesAsync();
                return Ok(dept);
            }   
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("id")]
        public async Task<IActionResult>DeleteDept(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if(dept is null)
            {
                return NotFound();
            }
            try
            {
                _context.Remove(dept);
                await _context.SaveChangesAsync();
                return Ok(dept);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
