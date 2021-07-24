using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace Sprout.Exam.WebApp.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddEmployee(CreateEmployeeDto input)
        {
            var nuEmployee = new Employee
            {
                BirthDate = input.Birthdate,
                FullName = input.FullName,
                Tin = input.Tin,
                EmployeeTypeId = input.TypeId
            };
            await _context.AddAsync(nuEmployee);
            return nuEmployee;
        }

        public async Task DeleteEmployee(int id)
        {
            var result = await _context.Employee.FirstOrDefaultAsync(m => m.Id == id);
            if (result == null) throw new ArgumentOutOfRangeException("Employee not found.");
            _context.Employee.Remove(result);
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await _context.Employee.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await _context.Employee.ToListAsync();
        }

        public async Task<Employee> UpdateEmployee(EditEmployeeDto input)
        {
            var item = await _context.Employee.FirstOrDefaultAsync(m => m.Id == input.Id);
            if (item == null) throw new ArgumentOutOfRangeException("Employee not found");

            item.FullName = input.FullName;
            item.Tin = input.Tin;
            item.BirthDate = Convert.ToDateTime(input.Birthdate);
            item.EmployeeTypeId = input.TypeId;

            return item;
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
