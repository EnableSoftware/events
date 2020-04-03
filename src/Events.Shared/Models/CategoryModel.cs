using System;
using System.ComponentModel.DataAnnotations;

namespace Events.Shared.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required]
        [ListingColumn(DisplayName = "Name")]
        [StringLength(32, ErrorMessage = "Name too long (32 character limit).")]
        public string Name { get; set; }
        public int UpcomingEvents { get; set; }
        public DateTimeOffset? NextEvent { get; set; }
    }
}
