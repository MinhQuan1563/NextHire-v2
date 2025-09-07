using Microsoft.EntityFrameworkCore;
using NextHireApp.Model;
using System;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace NextHireApp.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class NextHireAppDbContext :
    AbpDbContext<NextHireAppDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext,
    INextHireAppDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<CVTemplate> CVTemplates { get; set; }
    public DbSet<UserCV> UserCVs { get; set; }
    public DbSet<CvView> CvViews { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameScore> GameScores { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ErrorLog> ErrorLogs { get; set; }

    public NextHireAppDbContext(DbContextOptions<NextHireAppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */
        builder.Entity<AppUser>(b =>
        {
            b.ToTable("AppUser");
            b.HasKey(u => u.Id);
            b.Property(u => u.UserCode).HasMaxLength(12).IsRequired();
            b.HasIndex(u => u.UserCode).IsUnique();
        });

        builder.Entity<Company>(b =>
        {
            b.ToTable("Companies");
            b.Property(c => c.UserCode)
             .HasMaxLength(12)
             .IsRequired();
            b.HasIndex(c => c.UserCode);
        });

        // Job
        builder.Entity<Job>(b =>
        {
            b.ToTable("Jobs");
            b.HasKey(x => x.JobId);
            b.HasIndex(x => x.JobCode).IsUnique();
            b.Property(x => x.JobCode).HasMaxLength(12);
            b.Property(x => x.Title).HasMaxLength(50);
        });


        // JobApplication
        builder.Entity<JobApplication>(b =>
        {
            b.ToTable("JobApplications");
            b.HasKey(x => x.ApplicationId);
            b.HasIndex(x => x.ApplicationCode).IsUnique();
            b.Property(x => x.ApplicationCode).HasMaxLength(12);
        });


        // CVTemplate
        builder.Entity<CVTemplate>(b =>
        {
            b.ToTable("CVTemplates");
            b.HasKey(x => x.TemplateId);
            b.HasIndex(x => x.TemplateCode).IsUnique();
            b.Property(x => x.TemplateCode).HasMaxLength(12);
        });


        // UserCV
        builder.Entity<UserCV>(b =>
        {
            b.ToTable("UserCVs");
            b.HasKey(x => x.CvId);
        });


        // CvView
        builder.Entity<CvView>(b =>
        {
            b.ToTable("CvViews");
            b.HasKey(x => x.ViewId);
        });

        // Post
        builder.Entity<Post>(b =>
        {
            b.ToTable("Posts");
            b.HasKey(x => x.PostId);
            b.HasIndex(x => x.PostCode).IsUnique();
            b.Property(x => x.PostCode).HasMaxLength(12);
        });


        // PostLike
        builder.Entity<PostLike>(b =>
        {
            b.ToTable("PostLikes");
            b.HasKey(x => x.LikeId);
        });


        // PostComment
        builder.Entity<PostComment>(b =>
        {
            b.ToTable("PostComments");
            b.HasKey(x => x.CommentId);
        });


        // Friendship
        builder.Entity<Friendship>(b =>
        {
            b.ToTable("Friendships");
            b.HasKey(x => new { x.UserCode1, x.UserCode2 });
        });


        // Message
        builder.Entity<Message>(b =>
        {
            b.ToTable("Messages");
            b.HasKey(x => x.MessageId);
        });


        // Game
        builder.Entity<Game>(b =>
        {
            b.ToTable("Games");
            b.HasKey(x => x.GameId);
            b.HasIndex(x => x.GameCode).IsUnique();
        });


        // GameScore
        builder.Entity<GameScore>(b =>
        {
            b.ToTable("GameScores");
            b.HasKey(x => x.ScoreId);
        });


        // Notification
        builder.Entity<Notification>(b =>
        {
            b.ToTable("Notifications");
            b.HasKey(x => x.NotificationId);
        });


        // ErrorLog
        builder.Entity<ErrorLog>(b =>
        {
            b.ToTable("ErrorLogs");
            b.HasKey(x => x.LogId);
        });
    }
}
