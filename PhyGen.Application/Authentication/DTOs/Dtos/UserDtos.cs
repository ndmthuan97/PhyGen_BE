using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Authentication.DTOs.Dtos
{
    public class UserDtos
    {
        public Guid Id { get; set; }  // kế thừa từ EntityBase<Guid>
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhotoURL { get; set; } = string.Empty;
        public string? Role { get; set; }
        public string? Phone { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int Coin { get; set; }
    }
}
