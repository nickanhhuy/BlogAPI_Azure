namespace BlogAPI.Core.Models
{
    public class Post
    {
        public int Id;
        public String? Title { get; set; }
        public String? Content { get; set; }
        public String? Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}