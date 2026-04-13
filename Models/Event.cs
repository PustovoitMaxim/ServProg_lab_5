// Models/Event.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Event
    {
        [Key]
        public int event_id { get; set; }
        
        [Required]
        public int post_id { get; set; }
        
        [Required]
        public DateTime event_time { get; set; }
        
        [Required]
        public string location { get; set; }
        
        public string host_org { get; set; }
        public int rsvp_count { get; set; } = 0;
        public int? max_capacity { get; set; }
        
        [ForeignKey("post_id")]
        public virtual Post Post { get; set; }
    }
}