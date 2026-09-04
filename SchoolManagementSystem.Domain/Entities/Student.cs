using System;
using System.Collections.Generic;
using SchoolManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Entities
{
    public class Student : BaseEntity
    {
        [Required]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; } = string.Empty;
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name")]

        public string LastName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Date Of Birth")]

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }
        [Required]

        public string Email { get; set; } = string.Empty;
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; } = string.Empty;
        [Required]

        public string Address { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Class Room Id")]

        public int ClassRoomId { get; set; }
        [Required]
        [Display(Name = "Class Room")]

        public ClassRoom? ClassRoom { get; set; }
       
        public string? ImagePath { get; set; }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();

        public string FullName => $"{FirstName} {LastName}";
    }
}
