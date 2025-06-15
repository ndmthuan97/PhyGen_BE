using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class User : EntityBase<Guid>
    {
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string photoURL { get; set; } = string.Empty;

        public string? Role { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool isConfirm { get; set; }
        public bool IsActive { get; set; }

        public int Coin { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Matrix> Matrices { get; set; } = new List<Matrix>();
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
