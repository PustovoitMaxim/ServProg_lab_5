// Models/Auth.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Auth
    {
        [Key]
        public int user_id { get; set; }
        
        [Required]
        public string password_hash { get; set; }
        
        public DateTime? last_login { get; set; }
        public string reset_token { get; set; }
        public DateTime? token_expiry { get; set; }
        
        [ForeignKey("user_id")]
        public virtual User User { get; set; }
    }
}