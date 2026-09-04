using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Entities
{
    public class Subject : BaseEntity
    {
        [Required]
        [Display(Name = "Subject Code")]
        public string SubjectCode { get; set; } = string.Empty;
        [Required]

        public string Name { get; set; } = string.Empty;
        [Required]

        public int TeacherId { get; set; }
        [Required]
        [Display(Name = "Teacher Id")]

        public Teacher? Teacher { get; set; }
        [Required]
        [Display(Name = "Class Room Id")]

        public int ClassRoomId { get; set; }
        [Required]
        [Display(Name = "Class Room")]

        public ClassRoom? ClassRoom { get; set; }

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
