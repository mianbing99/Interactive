using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Common {
    public class LearnSit {
        private int id;
        /// <summary>
        /// 编号
        /// </summary>
        public int ID {
            get { return id; }
            set { id = value; }
        }
        private string deviceName;
        /// <summary>
        /// 绑定对应的设备名
        /// </summary>
        public string DeviceName {
            get { return deviceName; }
            set { deviceName = value; }
        }

        private string studyName;
        /// <summary>
        /// 学习内容
        /// </summary>
        public string StudyName {
            get { return studyName; }
            set { studyName = value; }
        }

        private string createTime;
        /// <summary>
        /// 学习时间
        /// </summary>
        public string CreateTime {
            get { return createTime; }
            set { createTime = value; }
        }
    }
}