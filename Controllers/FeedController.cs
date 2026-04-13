using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Models;
using System.Text.RegularExpressions; 

namespace SocialApp.Controllers
{
    public class FeedController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Event)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .OrderByDescending(p => p.created_at)
                .Take(50)
                .ToListAsync();

            var postViewModels = new List<PostViewModel>();
            
            foreach (var post in posts)
            {
                var interactions = await _context.Interactions
                    .Where(i => i.post_id == post.post_id)
                    .ToListAsync();

                postViewModels.Add(new PostViewModel
                {
                    Post = post,
                    User = post.User,
                    LikeCount = interactions.Count(i => i.interaction_type == "like"),
                    CommentCount = interactions.Count(i => i.interaction_type == "comment"),
                    ReribbCount = interactions.Count(i => i.interaction_type == "reribb"),
                    RsvpCount = interactions.Count(i => i.interaction_type == "rsvp"),
                    Event = post.Event,
                    Hashtags = ExtractHashtags(post.content)
                });
            }

            var trendingTags = await GetTrendingTags();

            var viewModel = new FeedViewModel
            {
                Posts = postViewModels,
                TrendingTags = trendingTags
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Post(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Event)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.post_id == id);

            if (post == null)
            {
                return NotFound();
            }

            var replies = await _context.Posts
                .Include(p => p.User)
                .Where(p => p.parent_post_id == id)
                .OrderBy(p => p.created_at)
                .ToListAsync();

            var interactions = await _context.Interactions
                .Where(i => i.post_id == id)
                .ToListAsync();

            var comments = await _context.Interactions
                .Include(i => i.User)
                .Where(i => i.post_id == id && i.interaction_type == "comment")
                .OrderBy(i => i.created_at)
                .ToListAsync();

            var postViewModel = new PostViewModel
            {
                Post = post,
                User = post.User,
                LikeCount = interactions.Count(i => i.interaction_type == "like"),
                CommentCount = interactions.Count(i => i.interaction_type == "comment"),
                ReribbCount = interactions.Count(i => i.interaction_type == "reribb"),
                RsvpCount = interactions.Count(i => i.interaction_type == "rsvp"),
                Event = post.Event,
                Hashtags = ExtractHashtags(post.content)
            };

            var trendingTags = await GetTrendingTags();

            ViewBag.Replies = replies;
            ViewBag.Comments = comments;
            ViewBag.TrendingTags = trendingTags;

            return View(postViewModel);
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