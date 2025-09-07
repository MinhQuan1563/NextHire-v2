using NextHireApp.AppUsers;
using NextHireApp.UserCvs;
using System;
using Volo.Abp.Domain.Entities;


namespace NextHireApp.CvViews
{
    public class CvView : Entity<Guid>
    {
        public string ViewerUserCode { get; set; }
        public Guid ViewedCvId { get; set; }
        public DateTime ViewedAt { get; set; }
        public virtual AppUser Viewer { get; set; }
        public virtual UserCv Cv { get; set; }
    }
}
