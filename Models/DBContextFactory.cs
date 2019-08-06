using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Models {
    public static class DBContextFactory {
        public static VideoEntities GetDbContext() {
            // 首先先线程上下文中查看是否有已存在的DBContext  
            // 如果有那么直接返回这个，如果没有就新建   
            VideoEntities DB = CallContext.GetData("VideoEntities") as VideoEntities;
            if (DB == null) {
                DB = new VideoEntities();
                CallContext.SetData("VideoEntities", DB);
            }
            return DB;
        }
    }
}
