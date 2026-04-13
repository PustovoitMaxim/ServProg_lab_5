using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Models;
using SocialApp.Helpers;
using System.Text.RegularExpressions;

namespace SocialApp.Controllers
{
    public class PondsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PondsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Posts(string tag)
        {
            var postViewModels = new List<PostViewModel>();
            
            if (string.IsNullOrEmpty(tag))
            {
                ViewBag.CurrentTag = "empty";
                ViewBag.PostCount = 0;
                ViewBag.ErrorMessage = "Тег не указан";
                ViewBag.TrendingTags = await GetTrendingTags();
                return View(postViewModels);
            }

            var normalizedTag = tag.TrimStart('#').ToLower();
            ViewBag.CurrentTag = tag;
            
            var tagEntity = await _context.Tags
                .FirstOrDefaultAsync(t => t.tag_name.ToLower() == normalizedTag);

            if (tagEntity == null)
            {
                ViewBag.PostCount = 0;
                ViewBag.ErrorMessage = $"Тег #{tag} не найден в базе данных";
                ViewBag.TrendingTags = await GetTrendingTags();
                return View(postViewModels);
            }

            tagEntity.usage_count++;
            await _context.SaveChangesAsync();

            var postIds = await _context.PostTags
                .Where(pt => pt.tag_id == tagEntity.tag_id)
                .Select(pt => pt.post_id)
                .ToListAsync();
            
            if (postIds.Count == 0)
            {
                ViewBag.PostCount = 0;
                ViewBag.ErrorMessage = $"Нет постов с тегом #{tag}";
                ViewBag.TrendingTags = await GetTrendingTags();
                return View(postViewModels);
            }

            var postsWithTag = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Event)
                .Where(p => postIds.Contains(p.post_id))
                .OrderByDescending(p => p.created_at)
                .ToListAsync();

            foreach (var post in postsWithTag)
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

            ViewBag.TagInfo = tagEntity;
            ViewBag.PostCount = postsWithTag.Count;
            ViewBag.TrendingTags = trendingTags;
            ViewBag.ErrorMessage = null;

            return View(postViewModels);
        }

        public async Task<IActionResult> Trending()
        {
            var trendingTags = await GetTrendingTags(7);
            ViewBag.TrendingTags = trendingTags;
            return View(trendingTags);
        }

        public async Task<IActionResult> All()
        {
            var allTags = await _context.Tags
                .OrderByDescending(t => t.usage_count)
                .ThenBy(t => t.tag_name)
                .ToListAsync();

            foreach (var tag in allTags)
            {
                var realCount = await _context.PostTags.CountAsync(pt => pt.tag_id == tag.tag_id);
                if (tag.usage_count != realCount)
                {
                    tag.usage_count = realCount;
                }
            }
            await _context.SaveChangesAsync();

            var trendingTags = await GetTrendingTags();

            ViewBag.TrendingTags = trendingTags;
            ViewBag.Title = "All Ponds";
            ViewBag.TotalTags = allTags.Count;
            
            return View(allTags);
        }

        public async Task<IActionResult> Popular()
        {
            var popularTags = await _context.Tags
                .Where(t => t.usage_count > 0)
                .OrderByDescending(t => t.usage_count)
                .Take(50)
                .ToListAsync();

            var trendingTags = await GetTrendingTags();
            ViewBag.TrendingTags = trendingTags;
            ViewBag.Title = "Popular Ponds";
            
            return View("All", popularTags);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("All");
            }

            var normalizedSearch = searchTerm.TrimStart('#').ToLower();
            
            var matchingTags = await _context.Tags
                .Where(t => t.tag_name.Contains(normalizedSearch))
                .OrderByDescending(t => t.usage_count)
                .ToListAsync();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.TrendingTags = await GetTrendingTags();
            
            return View("SearchResults", matchingTags);
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

        private async Task<List<TrendingTag>> GetTrendingTags(int days = 1)
        {
            var dateThreshold = DateTime.Now.AddDays(-days);
            
            var trending = await _context.PostTags
                .Where(pt => pt.Post.created_at > dateThreshold)
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