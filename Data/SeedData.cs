using Microsoft.EntityFrameworkCore;
using SocialApp.Models;

namespace SocialApp.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            if (await context.Users.AnyAsync())
            {
                Console.WriteLine("📊 База данных уже содержит данные. Пропускаем инициализацию.");
                return;
            }

            
            var users = new List<User>
            {
                new User { username = "john_doe", display_name = "John Doe", bio = "Software developer", avatar_url = "/images/avatars/avatar1.svg", is_active = true, created_at = DateTime.Now.AddDays(-30) },
                new User { username = "jane_smith", display_name = "Jane Smith", bio = "Tech enthusiast", avatar_url = "/images/avatars/avatar2.svg", is_active = true, created_at = DateTime.Now.AddDays(-25) },
                new User { username = "alex_wilson", display_name = "Alex Wilson", bio = "Travel blogger", avatar_url = "/images/avatars/avatar3.svg", is_active = true, created_at = DateTime.Now.AddDays(-20) },
                new User { username = "emma_davis", display_name = "Emma Davis", bio = "Food lover", avatar_url = "/images/avatars/avatar4.svg", is_active = true, created_at = DateTime.Now.AddDays(-15) },
                new User { username = "mike_brown", display_name = "Mike Brown", bio = "Gamer", avatar_url = "/images/avatars/avatar5.svg", is_active = true, created_at = DateTime.Now.AddDays(-10) }
            };
            
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
            
            var tags = new List<Tag>
            {
                new Tag { tag_name = "coding", usage_count = 0, created_at = DateTime.Now },
                new Tag { tag_name = "dotnet", usage_count = 0, created_at = DateTime.Now },
                new Tag { tag_name = "photography", usage_count = 0, created_at = DateTime.Now },
                new Tag { tag_name = "nature", usage_count = 0, created_at = DateTime.Now },
                new Tag { tag_name = "travel", usage_count = 0, created_at = DateTime.Now },
                new Tag { tag_name = "food", usage_count = 0, created_at = DateTime.Now },
                new Tag { tag_name = "gaming", usage_count = 0, created_at = DateTime.Now },
                new Tag { tag_name = "tech", usage_count = 0, created_at = DateTime.Now }
            };
            
            await context.Tags.AddRangeAsync(tags);
            await context.SaveChangesAsync();
            
            var post1 = new Post { user_id = 1, content = "Just started learning ASP.NET Core! It's amazing! #coding #dotnet", post_type = "text", created_at = DateTime.Now.AddHours(-2) };
            var post2 = new Post { user_id = 2, content = "Beautiful sunset at the beach! #photography #nature", post_type = "image", media_url = "/images/posts/post1.svg", media_type = "image", alt_text = "Nature", created_at = DateTime.Now.AddHours(-5) };
            var post3 = new Post { user_id = 3, content = "Exploring the mountains! #travel #nature", post_type = "image", media_url = "/images/posts/post2.svg", media_type = "image", alt_text = "Mountains", created_at = DateTime.Now.AddDays(-1) };
            var post4 = new Post { user_id = 1, content = "Check out my new GitHub project! #coding #tech", post_type = "text", created_at = DateTime.Now.AddDays(-1).AddHours(-3) };
            var post5 = new Post { user_id = 4, content = "Homemade pasta! So delicious! #food", post_type = "image", media_url = "/images/posts/post3.svg", media_type = "image", alt_text = "Food", created_at = DateTime.Now.AddDays(-2) };
            var post6 = new Post { user_id = 5, content = "Live stream starting soon! Join the fun! #gaming", post_type = "text", created_at = DateTime.Now.AddDays(-2).AddHours(-4) };
            
            var posts = new List<Post> { post1, post2, post3, post4, post5, post6 };
            
            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Добавлено {posts.Count} постов");

            var postTags = new List<PostTag>();
            
            var codingTag = tags.First(t => t.tag_name == "coding");
            var dotnetTag = tags.First(t => t.tag_name == "dotnet");
            var photographyTag = tags.First(t => t.tag_name == "photography");
            var natureTag = tags.First(t => t.tag_name == "nature");
            var travelTag = tags.First(t => t.tag_name == "travel");
            var techTag = tags.First(t => t.tag_name == "tech");
            var foodTag = tags.First(t => t.tag_name == "food");
            var gamingTag = tags.First(t => t.tag_name == "gaming");
            
            postTags.Add(new PostTag { post_id = post1.post_id, tag_id = codingTag.tag_id });
            postTags.Add(new PostTag { post_id = post1.post_id, tag_id = dotnetTag.tag_id });
            
            postTags.Add(new PostTag { post_id = post2.post_id, tag_id = photographyTag.tag_id });
            postTags.Add(new PostTag { post_id = post2.post_id, tag_id = natureTag.tag_id });
            
            postTags.Add(new PostTag { post_id = post3.post_id, tag_id = travelTag.tag_id });
            postTags.Add(new PostTag { post_id = post3.post_id, tag_id = natureTag.tag_id });
            
            postTags.Add(new PostTag { post_id = post4.post_id, tag_id = codingTag.tag_id });
            postTags.Add(new PostTag { post_id = post4.post_id, tag_id = techTag.tag_id });
            
            
            postTags.Add(new PostTag { post_id = post5.post_id, tag_id = foodTag.tag_id });
            
            postTags.Add(new PostTag { post_id = post6.post_id, tag_id = gamingTag.tag_id });
            
            await context.PostTags.AddRangeAsync(postTags);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Добавлено {postTags.Count} связей пост-тег в таблицу post_tags");

            foreach (var tag in tags)
            {
                var usageCount = await context.PostTags.CountAsync(pt => pt.tag_id == tag.tag_id);
                tag.usage_count = usageCount;
                Console.WriteLine($"   Тег #{tag.tag_name}: {usageCount} использований");
            }
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Обновлены счетчики тегов");

            var interactions = new List<Interaction>
            {
                new Interaction { user_id = 2, post_id = post1.post_id, interaction_type = "like", created_at = DateTime.Now.AddHours(-1) },
                new Interaction { user_id = 3, post_id = post1.post_id, interaction_type = "like", created_at = DateTime.Now.AddHours(-1) },
                new Interaction { user_id = 1, post_id = post2.post_id, interaction_type = "like", created_at = DateTime.Now.AddHours(-3) },
                new Interaction { user_id = 3, post_id = post2.post_id, interaction_type = "like", created_at = DateTime.Now.AddHours(-2) },
                new Interaction { user_id = 2, post_id = post3.post_id, interaction_type = "like", created_at = DateTime.Now.AddDays(-1).AddHours(-5) },
                new Interaction { user_id = 1, post_id = post3.post_id, interaction_type = "comment", content = "Amazing view!", created_at = DateTime.Now.AddDays(-1).AddHours(-3) },
                new Interaction { user_id = 2, post_id = post4.post_id, interaction_type = "like", created_at = DateTime.Now.AddDays(-1).AddHours(-2) },
                new Interaction { user_id = 5, post_id = post4.post_id, interaction_type = "reribb", created_at = DateTime.Now.AddDays(-1).AddHours(-1) },
                new Interaction { user_id = 1, post_id = post5.post_id, interaction_type = "like", created_at = DateTime.Now.AddDays(-2).AddHours(-3) },
                new Interaction { user_id = 3, post_id = post5.post_id, interaction_type = "like", created_at = DateTime.Now.AddDays(-2).AddHours(-2) },
                new Interaction { user_id = 1, post_id = post6.post_id, interaction_type = "like", created_at = DateTime.Now.AddDays(-2).AddHours(-3) },
                new Interaction { user_id = 4, post_id = post6.post_id, interaction_type = "like", created_at = DateTime.Now.AddDays(-2).AddHours(-2) }
            };
            
            await context.Interactions.AddRangeAsync(interactions);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Добавлено {interactions.Count} интеракций");

            var eventPost = new Post {
                user_id = 2,
                content = "🎉 Annual Tech Conference 2024! Join us for amazing talks! #tech #coding",
                post_type = "text",
                created_at = DateTime.Now.AddDays(-7)
            };
            
            await context.Posts.AddAsync(eventPost);
            await context.SaveChangesAsync();
            
            // Добавляем связи для события
            await context.PostTags.AddRangeAsync(new[]
            {
                new PostTag { post_id = eventPost.post_id, tag_id = codingTag.tag_id },
                new PostTag { post_id = eventPost.post_id, tag_id = techTag.tag_id }
            });
            
            var eventEntity = new Event {
                post_id = eventPost.post_id,
                event_time = DateTime.Now.AddDays(14),
                location = "Convention Center, San Francisco",
                host_org = "Tech Community Association",
                rsvp_count = 89,
                max_capacity = 300
            };
            
            await context.Events.AddAsync(eventEntity);
            await context.SaveChangesAsync();

        }
    }
}