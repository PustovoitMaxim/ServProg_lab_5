// Models/FeedViewModel.cs
namespace SocialApp.Models
{
    public class FeedViewModel
    {
        public List<PostViewModel> Posts { get; set; }
        public List<TrendingTag> TrendingTags { get; set; }
    }

    public class PostViewModel
    {
        public Post Post { get; set; }
        public User User { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int ReribbCount { get; set; }
        public int RsvpCount { get; set; }
        public Event Event { get; set; }
        public List<string> Hashtags { get; set; }
    }

    public class TrendingTag
    {
        public int tag_id { get; set; }
        public string tag_name { get; set; }
        public int recent_posts { get; set; }
    }

    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<PostViewModel> UserPosts { get; set; }
        public List<TrendingTag> TrendingTags { get; set; }
        public int PostCount { get; set; }
        public int LikeCount { get; set; }
    }
}