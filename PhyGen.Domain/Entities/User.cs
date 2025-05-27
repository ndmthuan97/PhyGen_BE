using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PhyGen.Domain.Entities
{
    public class User : EntityBase<Guid>
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? Role { get; set; }

        public string? Address { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Curriculum> Curriculums { get; set; } = new List<Curriculum>();
        public virtual ICollection<Matrix> Matrices { get; set; } = new List<Matrix>();
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
