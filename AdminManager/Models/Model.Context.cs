﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminManager.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbEntities : DbContext
    {
        public dbEntities()
            : base("name=dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DataLog> DataLog { get; set; }
        public virtual DbSet<TempBind> TempBind { get; set; }
        public virtual DbSet<TencentUser> TencentUser { get; set; }
        public virtual DbSet<Video> Video { get; set; }
        public virtual DbSet<VideoType> VideoType { get; set; }
        public virtual DbSet<VideoUrl> VideoUrl { get; set; }
        public virtual DbSet<WeChatUser> WeChatUser { get; set; }
        public virtual DbSet<VideoSort> VideoSort { get; set; }
    }
}
