

using Microsoft.EntityFrameworkCore;
using NextHireApp.Model;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;

namespace NextHireApp.EntityFrameworkCore
{
    public static class NextHireDbContextCreatingExtensions
    {
        public static void ConfigureCvAndJobApplication(this ModelBuilder builder)
        {
            builder.Entity<UserCV>(b =>
              {
                  b.ToTable("UserCVs");
                  b.HasKey(x => x.CvId);
                  b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserCode).HasPrincipalKey(x => x.UserCode).OnDelete(DeleteBehavior.Cascade);
                  b.Property(x => x.UserCode).IsRequired().HasMaxLength(12);
                  b.Property(x => x.CvName).IsRequired().HasMaxLength(256);
                  b.Property(x => x.FileCv).IsRequired();
              });

            builder.Entity<CvView>(b =>
            {
                b.ToTable("CvViews");
                b.HasKey(x => x.ViewId);
                b.HasOne(x => x.Cv).WithMany().HasForeignKey(x => x.ViewedCvId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne(x => x.Viewer).WithMany().HasForeignKey(x => x.ViewedUserCode).HasPrincipalKey(x => x.UserCode).OnDelete(DeleteBehavior.NoAction);
                b.Property(x => x.ViewedAt).IsRequired();
            });

            builder.Entity<JobApplication>(b =>
            {
                b.ToTable(NextHireAppConsts.DbTablePrefix + "JobApplications", NextHireAppConsts.DbSchema);
                b.HasKey(x => x.ApplicationId);
                b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserCode).HasPrincipalKey(x=>x.UserCode).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Job).WithMany().HasForeignKey(x => x.JobCode).HasPrincipalKey(x => x.JobCode).OnDelete(DeleteBehavior.Restrict);
                b.Property(x => x.ApplicationCode).IsRequired().HasMaxLength(12);
                b.Property(x => x.UserCode).IsRequired().HasMaxLength(12);
                b.Property(x => x.JobCode).IsRequired().HasMaxLength(12);
                b.Property(x => x.CvFile).IsRequired();
                b.Property(x => x.CoverLetter);
                b.Property(x => x.AttachmentFile);
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.JobApplicateVersion).HasMaxLength(256);
                b.HasIndex(x => new { x.ApplicationCode }).IsUnique().HasDatabaseName("IX_JobApplications_ApplicationCode");
                b.HasIndex(x => new { x.UserCode, x.JobCode }).IsUnique().HasDatabaseName("IX_JobApplications_User_Job");
            });
            builder.Entity<CVTemplate>(b =>
            {
                b.ToTable("CVTemplates");
                b.HasKey(x => x.TemplateId);
                b.Property(x => x.TemplateCode).HasMaxLength(12);
                b.HasIndex(x => x.TemplateCode).IsUnique();
            });
        }
    }
}