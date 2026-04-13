// Models/Tag.cs
using System.ComponentModel.DataAnnotations;

namespace SocialApp.Models
{
    public class Tag
    {
        [Key]
        public int tag_id { get; set; }
        
        [Required]
        public string tag_name { get; set; }
        
        public DateTime created_at { get; set; } = DateTime.Now;
        public int usage_count { get; set; } = 0;
        
        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}