using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUDTOs
{
    public class TagDTO
    {
        public int TagId { get; set; }

        [Required]
        [Display(Name = "Tag Name")]
        public string? TagName { get; set; }

        [Display(Name = "Note")]
        public string? Note { get; set; }
    }
}
