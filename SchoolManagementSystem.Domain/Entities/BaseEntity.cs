using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
       

        public DateTime? DeletedAt { get; set; }
    }
}
