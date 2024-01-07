using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcLibrary.Models
{
    public class Adherent : IdentityUser<int>
    {
        [StringLength(50)]
        public string Semester { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string? ResetPasswordToken { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
