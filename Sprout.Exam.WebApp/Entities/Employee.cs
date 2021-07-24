using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Tin { get; set; }

        [Required]
        [EnumDataType(typeof(EmployeeType))]
        public int EmployeeTypeId { get; set; }

        public EmployeeDto ToDto() => new EmployeeDto
        {
            Id = this.Id,
            FullName = this.FullName,
            Birthdate = this.BirthDate.ToString("yyyy-MM-dd"),
            Tin = this.Tin,
            TypeId = this.EmployeeTypeId
        };        
    }
}
