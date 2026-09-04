using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        [Required]
        [Display(Name ="Student Id")]
        public int StudentId { get; set; }

        public Student? Student { get; set; }
        

        public DateTime Date { get; set; }

        public bool IsPresent { get; set; }
        

        public string Remarks { get; set; } = string.Empty;
    }
}
