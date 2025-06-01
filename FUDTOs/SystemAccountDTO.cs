using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUDTOs
{
    public class SystemAccountDTO
    {
        public short AccountId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string? AccountName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? AccountEmail { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int? AccountRole { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? AccountPassword { get; set; }
    }
}
