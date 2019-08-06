using System;
using System.Collections.Generic;
using System.Text;

namespace IFrameWork.Data  
{
    public class DataValidException : EJQException
    {
        public DataValidException(string userVisibleMessage)
            : base(userVisibleMessage)
        {
            
        }

        public DataValidException(string userVisibleMessage, System.Exception innerException)
            :base(userVisibleMessage, innerException)
        {
            
        }
    }
}
