using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTOs
{
    public class PostUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
    }
}
