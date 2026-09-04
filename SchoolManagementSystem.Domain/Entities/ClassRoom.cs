using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Entities
{
    public class ClassRoom : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]

        public string Section { get; set; } = string.Empty;
        [Required]

        public int Capacity { get; set; }
        [Required]

        public ICollection<Student> Students { get; set; } = new List<Student>();

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public string DisplayName => $"{Name} - {Section}";
    }
}
