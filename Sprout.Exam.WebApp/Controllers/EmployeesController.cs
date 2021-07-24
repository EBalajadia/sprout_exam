using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Helpers;
using Sprout.Exam.WebApp.Services;
using Microsoft.EntityFrameworkCore;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {        
        private EmployeeRepository _employeeRepo;

        public EmployeesController(EmployeeRepository employeeRepo) //Data.ApplicationDbContext context)
        {            
            _employeeRepo = employeeRepo;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {            
            var result = await _employeeRepo.GetEmployees();
            return Ok(result.Select(x => x.ToDto()).ToList());
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {            
            var result = await _employeeRepo.GetEmployee(id);
            return Ok(result.ToDto());
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var item = await _employeeRepo.UpdateEmployee(input);
                if (!(await _employeeRepo.Save()))
                {
                    throw new Exception("Save failed.");
                }
                return Ok(item);
            }
            catch (ArgumentOutOfRangeException)
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var nuEmployee = await _employeeRepo.AddEmployee(input);
            if(!( await _employeeRepo.Save()))
            {
                throw new Exception("Save failed.");
            }
            return Created($"/api/employees/{nuEmployee.Id}", nuEmployee.Id);            
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
                await _employeeRepo.DeleteEmployee(id);
                await _employeeRepo.Save();
                return Ok(id);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
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
            var result = await _employeeRepo.GetEmployee(input.Id);
            if (result == null) return NotFound();
            if (!Enum.IsDefined(typeof(EmployeeType), result.EmployeeTypeId)) return NotFound("Employee Type not found");             
            return Ok(SalaryCalculator.Calculate(result.EmployeeTypeId, input));            
        }

    }
}
