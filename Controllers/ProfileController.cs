using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Models;
using System.Text.RegularExpressions; 

namespace SocialApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.username == username);

            if (user == null)
            {
                return NotFound();
            }

            var userPosts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Event)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .Where(p => p.user_id == user.user_id)
                .OrderByDescending(p => p.created_at)
                .ToListAsync();

            var postViewModels = new List<PostViewModel>();
            
            foreach (var post in userPosts)
            {
                var interactions = await _context.Interactions
                    .Where(i => i.post_id == post.post_id)
                    .ToListAsync();

                postViewModels.Add(new PostViewModel
                {
                    Post = post,
                    User = user,
                    LikeCount = interactions.Count(i => i.interaction_type == "like"),
                    CommentCount = interactions.Count(i => i.interaction_type == "comment"),
                    ReribbCount = interactions.Count(i => i.interaction_type == "reribb"),
                    RsvpCount = interactions.Count(i => i.interaction_type == "rsvp"),
                    Event = post.Event,
                    Hashtags = ExtractHashtags(post.content)
                });
            }

            var allUserPostsIds = userPosts.Select(p => p.post_id).ToList();
            var totalLikes = await _context.Interactions
                .Where(i => allUserPostsIds.Contains(i.post_id) && i.interaction_type == "like")
                .CountAsync();

            var trendingTags = await GetTrendingTags();

            var viewModel = new ProfileViewModel
            {
                User = user,
                UserPosts = postViewModels,
                PostCount = userPosts.Count,
                LikeCount = totalLikes,
                TrendingTags = trendingTags
            };

            return View(viewModel);
        }

        private List<string> ExtractHashtags(string content)
        {
            if (string.IsNullOrEmpty(content)) return new List<string>();
            
            var regex = new Regex(@"#\w+");
            return regex.Matches(content)
                .Cast<Match>()
                .Select(m => m.Value.TrimStart('#'))
                .Distinct()
                .ToList();
        }

        private async Task<List<TrendingTag>> GetTrendingTags()
        {
            var oneDayAgo = DateTime.Now.AddDays(-1);
            
            var trending = await _context.PostTags
                .Where(pt => pt.Post.created_at > oneDayAgo)
                .GroupBy(pt => new { pt.Tag.tag_id, pt.Tag.tag_name })
                .Select(g => new TrendingTag
                {
                    tag_id = g.Key.tag_id,
                    tag_name = g.Key.tag_name,
                    recent_posts = g.Count()
                })
                .OrderByDescending(t => t.recent_posts)
                .Take(10)
                .ToListAsync();

            return trending;
        }
    }
}