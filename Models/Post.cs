// Models/Post.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Post
    {
        [Key]
        public int post_id { get; set; }
        
        [Required]
        public int user_id { get; set; }
        
        [Required]
        public string content { get; set; }
        
        [Required]
        public string post_type { get; set; } // text, image, video
        
        public string media_url { get; set; }
        public string media_type { get; set; }
        public string alt_text { get; set; }
        public string thumbnail_url { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public int? parent_post_id { get; set; }
        
        [ForeignKey("user_id")]
        public virtual User User { get; set; }
        
        [ForeignKey("parent_post_id")]
        public virtual Post ParentPost { get; set; }
        
        public virtual ICollection<PostTag> PostTags { get; set; }
        public virtual ICollection<Interaction> Interactions { get; set; }
        public virtual Event Event { get; set; }
    }
}