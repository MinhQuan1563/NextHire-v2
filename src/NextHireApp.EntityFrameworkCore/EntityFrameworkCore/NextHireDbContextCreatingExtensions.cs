

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NextHireApp.AppUsers;
using NextHireApp.CvViews;
using NextHireApp.JobApplications;
using NextHireApp.Jobs;
using NextHireApp.UserCvs;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Users.EntityFrameworkCore;

namespace NextHireApp.EntityFrameworkCore
{
    public static class NextHireDbContextCreatingExtensions
    {
        public static void ConfigureCvAndJobApplication(this ModelBuilder builder)
        {
            builder.Entity<UserCv>(b =>
              {
                  b.ToTable(NextHireAppConsts.DbTablePrefix + "UserCvs", NextHireAppConsts.DbSchema);
                  b.ConfigureByConvention();
                  b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserCode).HasPrincipalKey(x => x.UserCode).OnDelete(DeleteBehavior.Cascade);
                  b.Property(x => x.UserCode).IsRequired().HasMaxLength(12);
                  b.Property(x => x.CvName).IsRequired().HasMaxLength(256);
                  b.Property(x => x.Base64Source).IsRequired();
                  b.HasIndex(x => new { x.UserCode, x.IsDefault }).HasDatabaseName("IX_UserCvs_UserCode_IsDefault");
              });

            builder.Entity<CvView>(b =>
            {
                b.ToTable(NextHireAppConsts.DbTablePrefix + "CvViews", NextHireAppConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne(x => x.Cv).WithMany().HasForeignKey(x => x.ViewedCvId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Viewer).WithMany().HasForeignKey(x => x.ViewerUserCode).HasPrincipalKey(x => x.UserCode).OnDelete(DeleteBehavior.Cascade);
                b.Property(x => x.ViewedAt).IsRequired();
            });

            builder.Entity<JobApplication>(b =>
            {
                b.ToTable(NextHireAppConsts.DbTablePrefix + "JobApplications", NextHireAppConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserCode).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Job).WithMany().HasForeignKey(x => x.JobCode).HasPrincipalKey(x => x.JobCode).OnDelete(DeleteBehavior.Restrict);
                b.Property(x => x.ApplicationCode).IsRequired().HasMaxLength(12);
                b.Property(x => x.UserCode).IsRequired().HasMaxLength(12);
                b.Property(x => x.JobCode).IsRequired().HasMaxLength(12);
                b.Property(x => x.CvFileUrl).IsRequired().HasMaxLength(2048);
                b.Property(x => x.CoverLetterUrl).HasMaxLength(2048);
                b.Property(x => x.AttachmentFileUrl).HasMaxLength(2048);
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.ApplicationSource).HasMaxLength(256);
                b.HasIndex(x => new { x.ApplicationCode }).IsUnique().HasDatabaseName("IX_JobApplications_ApplicationCode");
                b.HasIndex(x => new { x.UserCode, x.JobCode }).IsUnique().HasDatabaseName("IX_JobApplications_User_Job");
            });
        }

        public static void ConfigureAppUsers(this ModelBuilder builder)
        {

            builder.Entity<AppUser>(b =>
            {
                b.ToTable("AbpUsers");

                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                b.Property(x => x.UserCode).IsRequired().HasMaxLength(12);
                b.Property(x => x.FullName).HasMaxLength(256);
                b.Property(x => x.AvatarUrl).HasMaxLength(2048);
                b.HasIndex(x => new { x.UserCode }).IsUnique().HasDatabaseName("IX_AppUsers_UserCode");
                b.HasOne<IdentityUser>().WithOne().HasForeignKey<AppUser>(u => u.Id);
            });
        }

        public static void ConfigureJobs(this ModelBuilder builder)
        {
            builder.Entity<Job>(b =>
                {
                    b.ToTable(NextHireAppConsts.DbTablePrefix + "Jobs", NextHireAppConsts.DbSchema);
                    b.ConfigureByConvention();
                    b.Property(x => x.JobCode).IsRequired().HasMaxLength(12);
                    b.Property(x => x.Title).IsRequired().HasMaxLength(256);
                    b.Property(x => x.Description).IsRequired();
                    b.Property(x => x.Location).HasMaxLength(256);
                    b.Property(x => x.ExperienceLevel).HasMaxLength(100);
                    b.HasAlternateKey(x => x.JobCode);
                });
        }
    }
}