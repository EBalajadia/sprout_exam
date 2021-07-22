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
            //var result = await Task.FromResult(StaticEmployees.ResultList);

            var result = await _context.Employee.ToListAsync();                          
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));

            var result = await _context.Employee.FirstOrDefaultAsync(m => m.Id == id);
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            //var item = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == input.Id));
            var item = await _context.Employee.FirstOrDefaultAsync(m => m.Id == input.Id);
            if (item == null) return NotFound();
            item.FullName = input.FullName;
            item.Tin = input.Tin;
            item.Birthdate = input.Birthdate.ToString("yyyy-MM-dd");
            item.TypeId = input.TypeId;
            try
            {
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

            var id = await Task.FromResult(StaticEmployees.ResultList.Max(m => m.Id) + 1);

            StaticEmployees.ResultList.Add(new EmployeeDto
            {
                Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
                FullName = input.FullName,
                Id = id,
                Tin = input.Tin,
                TypeId = input.TypeId
            });

            //int id = await _context.Employee.MaxAsync(m => m.Id) + 1;
            //_context.Add(new EmployeeDto
            //{
            //    Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
            //    FullName = input.FullName,
            //    Id = id,
            //    Tin = input.Tin,
            //    TypeId = input.TypeId
            //});
            //await _context.SaveChangesAsync();

            return Created($"/api/employees/{id}", id);
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            var result = await _context.Employee.FirstOrDefaultAsync(m => m.Id == id);
            if (result == null) return NotFound();
            //StaticEmployees.ResultList.RemoveAll(m => m.Id == id);
            _context.Employee.Remove(result);
            await _context.SaveChangesAsync();

            return Ok(id);
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(int id,decimal absentDays,decimal workedDays)
        {
            var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));

            if (result == null) return NotFound();
            if (!Enum.IsDefined(typeof(EmployeeType), result.TypeId)) return NotFound("Employee Type not found");

            //var type = (EmployeeType) result.TypeId;
            //return type switch
            //{
            //    EmployeeType.Regular =>
            //        //create computation for regular.
            //        Ok(25000),
            //    EmployeeType.Contractual =>
            //        //create computation for contractual.
            //        Ok(20000),
            //    _ => NotFound("Employee Type not found")
            //};
            
            var calculator = new SalaryCalculator
            {
                EmployeeTypeId = result.TypeId,
                DaysAbsent = absentDays,
                DaysWorked = workedDays
            };
            try
            {
                return Ok(calculator.Calculate());
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }

    }
}
