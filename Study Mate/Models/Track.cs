using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Study_Mate.Models
{
    public class Track
    {
        [Key]
        public int TrackId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a Subject.")]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        [Display(Name = "Session Time")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public string? SubjectTitleAndIcon
        {
            get
            {
                return Subject == null ? "" : Subject.Icon + " " + Subject.Title;
            }
        }

        [NotMapped]
        public string? FormattedSession
        {
            get
            {
                return ((Subject == null || Subject.Type == "Studied") ? "- " : "+ ") + Amount.ToString();
            }
        }
    }
}
