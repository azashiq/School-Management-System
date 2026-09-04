using System;
using SchoolManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Entities
{
    public class Grade : BaseEntity
    {
        [Required]
        [Display(Name ="Student Id")]
        public int StudentId { get; set; }

        public Student? Student { get; set; }
        [Required]
        [Display(Name = "Subject Id")]

        public int SubjectId { get; set; }

        public Subject? Subject { get; set; }
        [Required]
        

        public decimal Score { get; set; }

        [Required]
        [Display(Name = "Exam Type")]
        public ExamType ExamType { get; set; }

        [Required]
        [Display(Name = "Exam Date")]

        public DateTime ExamDate { get; set; }
       
    }
}
