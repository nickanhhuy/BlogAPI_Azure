using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTOs
{
    public class PostCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
