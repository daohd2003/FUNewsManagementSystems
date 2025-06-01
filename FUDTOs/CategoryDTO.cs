using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUDTOs
{
    public class CategoryDTO
    {
        public short CategoryId { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;

        [Required]
        [Display(Name = "Description")]
        public string CategoryDesciption { get; set; } = null!;

        [Display(Name = "Parent Category")]
        public short? ParentCategoryId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation property for parent category
        [JsonIgnore]
        public CategoryDTO? ParentCategory { get; set; }

        /*[JsonIgnore]*/
        public string ParentCategoryName { get; set; } = "None";
    }
}