using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Entities;
using Sprout.Exam.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Tests.Data
{
    public class MockEmployees : IEmployeeRepository
    {
        private ObservableCollection<Employee> _data;        

        public MockEmployees()
        {
            _data = new ObservableCollection<Employee>();            
        }

        public ObservableCollection<Employee> Data { get => _data; }

        public Task<Employee> AddEmployee(CreateEmployeeDto input)
        {
            var nuEntry = new Employee
            {
                Id = _data.Any() ? _data.Max(x => x.Id) + 1 : 1,
                FullName = input.FullName,
                BirthDate = input.Birthdate,
                Tin = input.Tin,
                EmployeeTypeId = input.TypeId
            };
            _data.Add(nuEntry);
            return Task.FromResult(nuEntry);
        }

        public Task DeleteEmployee(int id)
        {
            var item = _data.FirstOrDefault(x => x.Id == id);
            if (item == null) throw new ArgumentOutOfRangeException();
            return Task.FromResult(_data.Remove(item)); 
        }

        public Task<Employee> GetEmployee(int id)
        {            
            return Task.FromResult(_data.FirstOrDefault(x => x.Id == id));
        }

        public Task<List<Employee>> GetEmployees()
        {
            return Task.FromResult(_data.ToList());
        }

        public Task<bool> Save()
        {
            return Task.FromResult(true);
        }

        public Task<Employee> UpdateEmployee(EditEmployeeDto input)
        {
            var item = _data.FirstOrDefault(x => x.Id == input.Id);
            if (item == null) throw new ArgumentOutOfRangeException();
            item.FullName = input.FullName;
            item.BirthDate = input.Birthdate;
            item.Tin = input.Tin;
            item.EmployeeTypeId = input.TypeId;
            return Task.FromResult(item);
        }
    }
}
