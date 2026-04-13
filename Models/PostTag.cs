// Models/PostTag.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class PostTag
    {
        [Key]
        [Column(Order = 0)]
        public int post_id { get; set; }
        
        [Key]
        [Column(Order = 1)]
        public int tag_id { get; set; }
        
        [ForeignKey("post_id")]
        public virtual Post Post { get; set; }
        
        [ForeignKey("tag_id")]
        public virtual Tag Tag { get; set; }
    }
}