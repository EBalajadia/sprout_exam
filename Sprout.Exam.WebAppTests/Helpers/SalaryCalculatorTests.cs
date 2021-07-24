using Sprout.Exam.Common.Enums;
using Sprout.Exam.Business.DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprout.Exam.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Tests
{
    [TestClass]
    public class SalaryCalculatorTests
    {
        [TestMethod]
        public void CalculateRegularTest()
        {            
            Assert.AreEqual(SalaryCalculator.Calculate((int)EmployeeType.Regular, new SalaryDto { AbsentDays = 1 }), 16690.91m);
            Assert.AreEqual(SalaryCalculator.Calculate((int)EmployeeType.Regular, new SalaryDto { AbsentDays = 0 }), 17600);
            Assert.AreEqual(SalaryCalculator.Calculate((int)EmployeeType.Regular, new SalaryDto { WorkedDays = 0 }), 17600);
            Assert.AreEqual(SalaryCalculator.Calculate((int)EmployeeType.Regular, new SalaryDto { WorkedDays = 10 }), 17600);
        }

        [TestMethod]
        public void CalculateContractualTest()
        {
            Assert.AreEqual(SalaryCalculator.Calculate((int)EmployeeType.Contractual, new SalaryDto { WorkedDays = 15.5m }), 7750);
            Assert.AreEqual(SalaryCalculator.Calculate((int)EmployeeType.Contractual, new SalaryDto { WorkedDays = 0 }), 0);
            Assert.AreEqual(SalaryCalculator.Calculate((int)EmployeeType.Contractual, new SalaryDto { AbsentDays = 0 }), 0);
            Assert.AreEqual(SalaryCalculator.Calculate((int)EmployeeType.Contractual, new SalaryDto { AbsentDays = 10 }), 0);
        }

        [TestMethod]
        public void CalculateExceptionsTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SalaryCalculator.Calculate((int)EmployeeType.Regular, null));
            Assert.ThrowsException<ArgumentNullException>(() => SalaryCalculator.Calculate((int)EmployeeType.Contractual, null));
            Assert.ThrowsException<InvalidOperationException>(() => SalaryCalculator.Calculate(999, new SalaryDto { AbsentDays = 10, WorkedDays = 5 }));
            Assert.ThrowsException<InvalidOperationException>(() => SalaryCalculator.Calculate(-100, new SalaryDto { AbsentDays = 0, WorkedDays = 5 }));
        }
    }
}