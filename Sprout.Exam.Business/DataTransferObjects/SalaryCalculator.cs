using Sprout.Exam.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class SalaryCalculator : SalaryDto
    {        
        public int EmployeeTypeId { get; set; }        

        public decimal Calculate()
        {
            decimal salary = 0;

            var type = (EmployeeType)this.EmployeeTypeId;            
            switch(type)
            {
                case EmployeeType.Regular:
                    int monthlyRate = 20000;

                    decimal deductions = decimal.Divide(monthlyRate , 22m) * this.AbsentDays;
                    decimal deductedSalary = monthlyRate - deductions;

                    decimal taxDeductions = monthlyRate * 0.12m;
                    salary = deductedSalary - taxDeductions;
                    break;

                case EmployeeType.Contractual:
                    salary = 50 * this.WorkedDays;
                    break;

                default:
                    throw new InvalidOperationException("Employee type is invalid.");
            }

            return Math.Round(salary, 2);
        }
    }
}
