using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly Data.ApplicationDbContext _context;

        public EmployeesController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {            
            var result = await _context.Employee.Select(m => m.ToDto()).ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {            
            var result = await _context.Employee.FirstOrDefaultAsync(m => m.Id == id);
            return Ok(result.ToDto());
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            try
            {
                var item = await _context.Employee.FirstOrDefaultAsync(m => m.Id == input.Id);
                if (item == null) return NotFound();
                item.FullName = input.FullName;
                item.Tin = input.Tin;
                item.BirthDate = input.Birthdate;
                item.EmployeeTypeId = input.TypeId;            
                await _context.SaveChangesAsync();
                return Ok(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {            
            try
            {                
                var nuEmployee = new Models.Employee
                {
                    BirthDate = input.Birthdate,
                    FullName = input.FullName,                    
                    Tin = input.Tin,
                    EmployeeTypeId = input.TypeId
                };
                _context.Add(nuEmployee);
                await _context.SaveChangesAsync();

                return Created($"/api/employees/{nuEmployee.Id}", nuEmployee.Id);
            }
            catch(Exception)
            {
                return NotFound("An unexpected error occured");
            }
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {                
                var result = await _context.Employee.FirstOrDefaultAsync(m => m.Id == id);
                if (result == null) return NotFound();
                _context.Employee.Remove(result);
                await _context.SaveChangesAsync();

                return Ok(id);
            }
            catch(Exception)
            {
                return NotFound("An unexpected error occured");
            }
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(SalaryDto input) //int id,decimal absentDays,decimal workedDays)
        {            
            var result = await _context.Employee.FirstOrDefaultAsync(m => m.Id == input.Id);

            if (result == null) return NotFound();
            if (!Enum.IsDefined(typeof(EmployeeType), result.EmployeeTypeId)) return NotFound("Employee Type not found");
            
            var calculator = new SalaryCalculator
            {
                EmployeeTypeId = result.EmployeeTypeId,
                AbsentDays = input.AbsentDays,
                WorkedDays = input.WorkedDays
            };
            try
            {
                return Ok(calculator.Calculate());
            }
            catch(Exception)
            {
                return NotFound("An unexpected error occured");
            }
        }

    }
}
