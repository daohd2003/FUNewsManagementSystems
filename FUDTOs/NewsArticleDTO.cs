using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUDTOs
{
    public class NewsArticleDTO
    {
        [Required]
        public string NewsArticleId { get; set; } = null!;

        [Display(Name = "Title")]
        public string? NewsTitle { get; set; }

        [Required]
        [Display(Name = "Headline")]
        public string Headline { get; set; } = null!;

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Content")]
        public string? NewsContent { get; set; }

        [Display(Name = "Source")]
        public string? NewsSource { get; set; }

        [Display(Name = "Category")]
        public short? CategoryId { get; set; }

        [Display(Name = "Status")]
        public bool NewsStatus { get; set; } = false;

        [Display(Name = "Created By")]
        public short? CreatedById { get; set; }

        [Display(Name = "Updated By")]
        public short? UpdatedById { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // Optional display data
        public string? CategoryName { get; set; }
        public string? CreatedByName { get; set; }

        public CategoryDTO? Category { get; set; }
        public SystemAccountDTO? CreatedBy { get; set; }
        public SystemAccountDTO? UpdatedBy { get; set; }

        public List<TagDTO>? Tags { get; set; }
    }
}
