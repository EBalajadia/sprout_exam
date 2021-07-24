using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class SalaryDto
    {
        public int Id { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal WorkedDays { get; set; }
    }
}
