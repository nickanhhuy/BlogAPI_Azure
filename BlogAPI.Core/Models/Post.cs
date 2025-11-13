using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.Models
{
    public class Post
    {
        [Key]
        public int Id;

        [Required]
        [MaxLength(200)]
        public String? Title { get; set; }

        [Required]
        public String? Content { get; set; }
        [Required]
        public String? Author { get; set; } = "admin";
        public DateTime CreatedDate { get; set; }
        
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<Comment>? Comments { get; set; }
    }
}