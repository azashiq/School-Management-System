using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Entities
{
    public class Teacher : BaseEntity
    {
        [Required]
        [Display(Name = "Employee Id")]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name ")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name ")]

        public string LastName { get; set; } = string.Empty;
        [Required]

        public string Qualification { get; set; } = string.Empty;
        [Required]

        public string Email { get; set; } = string.Empty;
        [Required]
        [Phone]

        public string Phone { get; set; } = string.Empty;

        public string? ImagePath { get; set; }
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public string FullName => $"{FirstName} {LastName}";
    }
}
