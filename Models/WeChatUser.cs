//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class WeChatUser
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public string Token { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string DeviceId { get; set; }
        public Nullable<bool> State { get; set; }
        public string DeviceName { get; set; }
        public string HeadImg { get; set; }
        public string UserPhone { get; set; }
    }
}