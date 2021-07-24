using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Services
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddEmployee(CreateEmployeeDto input);
        Task DeleteEmployee(int id);
        Task<Employee> GetEmployee(int id);
        Task<List<Employee>> GetEmployees();
        Task<Employee> UpdateEmployee(EditEmployeeDto input);
        Task<bool> Save();
    }
}
