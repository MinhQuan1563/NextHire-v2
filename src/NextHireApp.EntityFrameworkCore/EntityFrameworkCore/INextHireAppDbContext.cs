using Microsoft.EntityFrameworkCore;
using NextHireApp.Model;
using Volo.Abp.EntityFrameworkCore;

namespace NextHireApp.EntityFrameworkCore
{
    public interface INextHireAppDbContext : IEfCoreDbContext
    {
        DbSet<AppUser> AppUsers { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<Job> Jobs { get; set; }
        DbSet<JobApplication> JobApplications { get; set; }
        DbSet<CVTemplate> CVTemplates { get; set; }
        DbSet<UserCV> UserCVs { get; set; }
        DbSet<CvView> CvViews { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<PostLike> PostLikes { get; set; }
        DbSet<PostComment> PostComments { get; set; }
        DbSet<Friendship> Friendships { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Game> Games { get; set; }
        DbSet<GameScore> GameScores { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<ErrorLog> ErrorLogs { get; set; }
    }
}
