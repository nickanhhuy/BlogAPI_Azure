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
        public String? Author { get; set; } = "admin";
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedDate { get; set; } 
        public ICollection<Comment>? Comments { get; set; }
    }
}