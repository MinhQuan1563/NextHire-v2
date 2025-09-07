using Microsoft.EntityFrameworkCore;
using NextHireApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace NextHireApp.EntityFrameworkCore
{
    public interface INextHireAppReadDbContext : IEfCoreDbContext
    {
        DbSet<Company> Companies { get; }
        DbSet<Job> Jobs { get; }
        DbSet<JobApplication> JobApplications { get; }
        DbSet<CVTemplate> CVTemplates { get; }
        DbSet<UserCV> UserCVs { get;  }
        DbSet<CvView> CvViews { get; }
        DbSet<Post> Posts { get; }
        DbSet<PostLike> PostLikes { get; }
        DbSet<PostComment> PostComments { get; }
        DbSet<Friendship> Friendships { get; }
        DbSet<Message> Messages { get;  }
        DbSet<Game> Games { get; }
        DbSet<GameScore> GameScores { get; }
        DbSet<Notification> Notifications { get; }
        DbSet<ErrorLog> ErrorLogs { get; }
    }
}
