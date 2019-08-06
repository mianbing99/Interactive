using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Common {
    public class AlarmClocks {
        private int id;
        /// <summary>
        /// 编号
        /// </summary>
        public int Id {
            get { return id; }
            set { id = value; }
        }
        private string clockTime;
        /// <summary>
        /// 时间
        /// </summary>
        public string ClockTime {
            get { return clockTime; }
            set { clockTime = value; }
        }
        private string deviceId;
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceId {
            get { return deviceId; }
            set { deviceId = value; }
        }

        private string content;
        /// <summary>
        /// 描述
        /// </summary>
        public string Content {
            get { return content; }
            set { content = value; }
        }
        private string repeat;
        /// <summary>
        /// 是否循环
        /// </summary>
        public string Repeat {
            get { return repeat; }
            set { repeat = value; }
        }
        private string repeatDate;
        /// <summary>
        /// 响应日期
        /// </summary>
        public string RepeatDate {
            get { return repeatDate; }
            set { repeatDate = value; }
        }

        private int frequency;
        /// <summary>
        /// 循环次数
        /// </summary>
        public int Frequency {
            get { return frequency; }
            set { frequency = value; }
        }
        private int interval;
        /// <summary>
        /// 响应时间数
        /// </summary>
        public int Interval {
            get { return interval; }
            set { interval = value; }
        }
        private string  alarmType;

        public string AlarmType {
            get { return alarmType; }
            set { alarmType = value; }
        }
        private int state;

        public int State {
            get { return state; }
            set { state = value; }
        }
    }
}