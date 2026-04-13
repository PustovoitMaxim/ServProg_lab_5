// Models/Interaction.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Interaction
    {
        [Key]
        public int interaction_id { get; set; }
        
        [Required]
        public int user_id { get; set; }
        
        [Required]
        public int post_id { get; set; }
        
        [Required]
        public string interaction_type { get; set; } // comment, reribb, like, rsvp
        
        public DateTime created_at { get; set; } = DateTime.Now;
        public string content { get; set; }
        
        [ForeignKey("user_id")]
        public virtual User User { get; set; }
        
        [ForeignKey("post_id")]
        public virtual Post Post { get; set; }
    }
}