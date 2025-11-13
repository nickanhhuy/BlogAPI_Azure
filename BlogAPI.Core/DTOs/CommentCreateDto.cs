using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTOs
{
    public class CommentCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;
    }
}
