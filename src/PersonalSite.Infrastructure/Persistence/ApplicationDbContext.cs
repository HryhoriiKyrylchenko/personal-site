using PersonalSite.Domain.Entities.Pages;

namespace PersonalSite.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<AnalyticsEvent> AnalyticsEvents { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<BlogPostTag> BlogPostTags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<LogEntry> Logs { get; set; }
    public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<LearningSkill> LearningSkills { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<ProjectSkill> ProjectSkills { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<SkillCategory> SkillCategories { get; set; }
    public DbSet<UserSkill> UserSkills { get; set; }
    public DbSet<BlogPostTranslation> BlogPostTranslations { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<PageTranslation> PageTranslations { get; set; }
    public DbSet<ProjectTranslation> ProjectTranslations { get; set; }
    public DbSet<SkillCategoryTranslation> SkillCategoryTranslations { get; set; }
    public DbSet<SkillTranslation> SkillTranslations { get; set; }
    public DbSet<Translation> Translations { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<AnalyticsEvent>(entity =>
        {
            entity.HasIndex(e => e.PageSlug);
            entity.HasIndex(e => e.EventType);
        });
        
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.HasIndex(e => e.IsPublished);
            entity.HasIndex(e => e.PublishedAt);
        });
        
        modelBuilder.Entity<BlogPostTag>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
        });
        
        modelBuilder.Entity<PostTag>(entity =>
        {
            entity.HasIndex(e => new { e.BlogPostId, e.BlogPostTagId }).IsUnique();
        });
        
        modelBuilder.Entity<LogEntry>(entity =>
        {
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => e.Level);
            entity.HasIndex(e => e.Source);
        });

        modelBuilder.Entity<SocialMediaLink>(entity =>
        {
            entity.HasIndex(e => e.Platform).IsUnique();
        });

        modelBuilder.Entity<Resume>(entity =>
        {
            entity.HasIndex(e => e.UploadedAt);
        });
        
        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.IsRead);
        });
        
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.UpdatedAt);
        });
        
        modelBuilder.Entity<LearningSkill>(entity =>
        {
            entity.HasIndex(e => e.SkillId); 
            entity.HasIndex(e => e.DisplayOrder);
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasIndex(p => p.Key).IsUnique();
        });

        modelBuilder.Entity<ProjectSkill>(entity =>
        {
            entity.HasIndex(e => e.ProjectId);
            entity.HasIndex(e => e.SkillId);
            entity.HasIndex(e => new { e.ProjectId, e.SkillId }).IsUnique();
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.Key).IsUnique();
        });

        modelBuilder.Entity<SkillCategory>(entity =>
        {
            entity.HasIndex(e => e.Key).IsUnique();
            entity.HasIndex(e => e.DisplayOrder);
        });
        
        modelBuilder.Entity<UserSkill>(entity =>
        {
            entity.HasIndex(e => e.SkillId);
        });
        
        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasIndex(e => e.Code).IsUnique();
        });
        
        modelBuilder.Entity<BlogPostTranslation>(entity =>
        {
            entity.HasIndex(e => e.BlogPostId);
        });
        
        modelBuilder.Entity<SkillTranslation>(entity =>
        {
            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<Translation>(entity =>
        {
            entity.HasOne(t => t.Language)
                .WithMany()
                .HasForeignKey(t => t.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}