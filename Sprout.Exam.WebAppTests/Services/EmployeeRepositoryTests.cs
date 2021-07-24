using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprout.Exam.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Entities;
using Sprout.Exam.Tests.Data;

namespace Sprout.Exam.Tests
{
    [TestClass()]
    public class EmployeeRepositoryTests
    {        
        private CreateEmployeeDto GetContractualEmployee()
        {
            return new CreateEmployeeDto
            {
                FullName = "Juan de la Cruz",
                Tin = "6789",
                Birthdate = new DateTime(1975, 12, 1),
                TypeId = (int)EmployeeType.Contractual
            };
        }

        private CreateEmployeeDto GetRegularEmployee()
        {
            return new CreateEmployeeDto
            {
                FullName = "Juana Cruz",
                Tin = "1234",
                Birthdate = new DateTime(1980, 1, 1),
                TypeId = (int)EmployeeType.Regular
            };
        }

        [TestMethod()]
        public void AddEmployeeTest()
        {
            var context = new MockEmployees();

            context.AddEmployee(GetContractualEmployee());
            context.AddEmployee(GetRegularEmployee());

            Assert.IsTrue(context.Data.Any());
        }

        [TestMethod()]
        public void DeleteEmployeeTest()
        {            
            var context = new MockEmployees();
            var employee1 = context.AddEmployee(GetContractualEmployee()).Result;
            var employee2 = context.AddEmployee(GetRegularEmployee()).Result;

            context.DeleteEmployee(employee1.Id);

            Assert.IsFalse(context.Data.Any(x => x.Id == employee1.Id));
            Assert.IsTrue(context.Data.Any(x => x.Id == employee2.Id));
        }

        [TestMethod()]
        public void GetEmployeeTest()
        {
            var context = new MockEmployees();
            var employee1 = context.AddEmployee(GetContractualEmployee()).Result;
            var employee2 = context.AddEmployee(GetRegularEmployee()).Result;

            Assert.IsNotNull(context.Data.FirstOrDefault(x => x.Id == employee1.Id));
            Assert.IsNotNull(context.Data.FirstOrDefault(x => x.Id == employee2.Id));
        }

        [TestMethod()]
        public void GetEmployeesTest()
        {
            var context = new MockEmployees();
            context.AddEmployee(GetContractualEmployee());
            context.AddEmployee(GetRegularEmployee());

            Assert.IsTrue(context.Data.Any());
        }

        [TestMethod()]
        public void UpdateEmployeeTest()
        {
            var context = new MockEmployees();
            var employee1 = context.AddEmployee(GetContractualEmployee()).Result;
            string newName = "John Doe";
            var edited = context.UpdateEmployee(new EditEmployeeDto
            {
                Id = employee1.Id,
                FullName = newName
            }).Result;
            Assert.IsTrue(edited.FullName.Equals(newName));
            Assert.IsTrue(employee1.FullName.Equals(edited.FullName));            
        }

        [TestMethod()]
        public void SaveTest()
        {
            var context = new MockEmployees();
            Assert.IsTrue(context.Save().Result);
        }
    }
}