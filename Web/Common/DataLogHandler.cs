using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Web.Common
{
    public class DataLogHandler
    {
        VideoEntities ve = DBContextFactory.GetDbContext();
        public int AddModel(DataLog dl) {
            ve.DataLog.Add(dl);
            return ve.SaveChanges();
        }
        public int GetModels() {
            //ve.Database.ExecuteSqlCommand("", "");
            //ve.Database.
            return 0;
        }
    }
    public enum LogEnum
    {
        SYSTEMLOG = 3001,
        USERLOG = 3002,
        VIDEOLOG = 3003
    }
}