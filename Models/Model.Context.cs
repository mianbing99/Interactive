﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class VideoEntities : DbContext
    {
        public VideoEntities()
            : base("name=VideoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DataLog> DataLog { get; set; }
        public virtual DbSet<TempBind> TempBind { get; set; }
        public virtual DbSet<Video> Video { get; set; }
        public virtual DbSet<VideoType> VideoType { get; set; }
        public virtual DbSet<WeChatUser> WeChatUser { get; set; }
        public virtual DbSet<VideoUrl> VideoUrl { get; set; }
        public virtual DbSet<TencentUser> TencentUser { get; set; }
        public virtual DbSet<ChildChatUser> ChildChatUser { get; set; }
        public virtual DbSet<ChildBind> ChildBind { get; set; }
        public virtual DbSet<StudentAlarmClock> StudentAlarmClock { get; set; }
        public virtual DbSet<StudentDeviceBind> StudentDeviceBind { get; set; }
        public virtual DbSet<StudentDeviceQRcode> StudentDeviceQRcode { get; set; }
        public virtual DbSet<StudentLearnSt> StudentLearnSt { get; set; }
        public virtual DbSet<LearnSituation> LearnSituation { get; set; }
    }
}
