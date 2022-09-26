using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public bool Banned { get; set; }

        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
    }
}
