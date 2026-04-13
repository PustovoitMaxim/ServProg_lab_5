// Models/User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        
        [Required]
        public string username { get; set; }
        
        [Required]
        public string display_name { get; set; }
        
        public string avatar_url { get; set; }
        public string bio { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public bool is_active { get; set; } = true;
        
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Interaction> Interactions { get; set; }
        public virtual Auth Auth { get; set; }
    }
}