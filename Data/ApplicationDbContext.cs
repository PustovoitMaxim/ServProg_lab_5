using Microsoft.EntityFrameworkCore;
using SocialApp.Models;

namespace SocialApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Auth> Auths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Post>().ToTable("posts");
            modelBuilder.Entity<Tag>().ToTable("tags");
            modelBuilder.Entity<PostTag>().ToTable("post_tags");
            modelBuilder.Entity<Interaction>().ToTable("interactions");
            modelBuilder.Entity<Event>().ToTable("events");
            modelBuilder.Entity<Auth>().ToTable("auth");

            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.post_id, pt.tag_id });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.user_id).HasColumnName("user_id");
                entity.Property(e => e.username).HasColumnName("username");
                entity.Property(e => e.display_name).HasColumnName("display_name");
                entity.Property(e => e.avatar_url).HasColumnName("avatar_url");
                entity.Property(e => e.bio).HasColumnName("bio");
                entity.Property(e => e.created_at).HasColumnName("created_at");
                entity.Property(e => e.is_active).HasColumnName("is_active");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.post_id).HasColumnName("post_id");
                entity.Property(e => e.user_id).HasColumnName("user_id");
                entity.Property(e => e.content).HasColumnName("content");
                entity.Property(e => e.post_type).HasColumnName("post_type");
                entity.Property(e => e.media_url).HasColumnName("media_url");
                entity.Property(e => e.media_type).HasColumnName("media_type");
                entity.Property(e => e.alt_text).HasColumnName("alt_text");
                entity.Property(e => e.thumbnail_url).HasColumnName("thumbnail_url");
                entity.Property(e => e.created_at).HasColumnName("created_at");
                entity.Property(e => e.parent_post_id).HasColumnName("parent_post_id");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.tag_id).HasColumnName("tag_id");
                entity.Property(e => e.tag_name).HasColumnName("tag_name");
                entity.Property(e => e.created_at).HasColumnName("created_at");
                entity.Property(e => e.usage_count).HasColumnName("usage_count");
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.Property(e => e.post_id).HasColumnName("post_id");
                entity.Property(e => e.tag_id).HasColumnName("tag_id");
            });

            modelBuilder.Entity<Interaction>(entity =>
            {
                entity.Property(e => e.interaction_id).HasColumnName("interaction_id");
                entity.Property(e => e.user_id).HasColumnName("user_id");
                entity.Property(e => e.post_id).HasColumnName("post_id");
                entity.Property(e => e.interaction_type).HasColumnName("interaction_type");
                entity.Property(e => e.created_at).HasColumnName("created_at");
                entity.Property(e => e.content).HasColumnName("content");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.event_id).HasColumnName("event_id");
                entity.Property(e => e.post_id).HasColumnName("post_id");
                entity.Property(e => e.event_time).HasColumnName("event_time");
                entity.Property(e => e.location).HasColumnName("location");
                entity.Property(e => e.host_org).HasColumnName("host_org");
                entity.Property(e => e.rsvp_count).HasColumnName("rsvp_count");
                entity.Property(e => e.max_capacity).HasColumnName("max_capacity");
            });

            modelBuilder.Entity<Auth>(entity =>
            {
                entity.Property(e => e.user_id).HasColumnName("user_id");
                entity.Property(e => e.password_hash).HasColumnName("password_hash");
                entity.Property(e => e.last_login).HasColumnName("last_login");
                entity.Property(e => e.reset_token).HasColumnName("reset_token");
                entity.Property(e => e.token_expiry).HasColumnName("token_expiry");
            });

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.ParentPost)
                .WithMany()
                .HasForeignKey(p => p.parent_post_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Post)
                .WithOne(p => p.Event)
                .HasForeignKey<Event>(e => e.post_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Auth>()
                .HasOne(a => a.User)
                .WithOne(u => u.Auth)
                .HasForeignKey<Auth>(a => a.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.post_id);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.tag_id);

            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.User)
                .WithMany(u => u.Interactions)
                .HasForeignKey(i => i.user_id);

            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.Post)
                .WithMany(p => p.Interactions)
                .HasForeignKey(i => i.post_id);
        }
    }
}