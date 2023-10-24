using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Study_Mate.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Icon { get; set; } = "";

        [Display(Name = "Status")]
        [Column(TypeName = "nvarchar(50)")]
        public string Type { get; set; } = "Studied";

        [NotMapped]
        public string? TitleAndIcon
        {
            get
            {
                return this.Icon + " " + this.Title;
            }
        }
    }
}
