using System;
using System.Collections.Generic;
using System.Text;

namespace IFrameWork
{
    /// <summary>
    /// 基本异常信息
    /// </summary>
    [Serializable]
    public class EJQException: Exception
    {
        private string m_Message;
        private Exception m_InnerException;


        public override string Message
        {
            get
            {
                return m_Message;
            }
        }
        
        new public Exception InnerException
        {
            get
            {
                return m_InnerException;
            }
        }

        public EJQException(string userVisibleMessage)
        {
            Init(userVisibleMessage);
        }

        public EJQException(string userVisibleMessage, System.Exception innerException)
        {
            Init(userVisibleMessage, innerException);
        }

        protected void Init(string message)
        {
            Init(message, new Exception(message));
        }
        
        protected void Init(string message, System.Exception innerException)
        {
            m_Message = message;
            m_InnerException = innerException;
        }
    }
}

