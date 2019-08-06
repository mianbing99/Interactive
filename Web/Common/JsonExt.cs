using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Web.Common
{
    public static class JsonExt
    {
        /// <summary>
        /// obj转JSON
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToJsonStr(this object target) {
            if (target == null) return "";
            return JsonConvert.SerializeObject(target);
        }
        /// <summary>
        /// JSON转T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T ToObj<T>(this string target) {
            if (string.IsNullOrEmpty(target)) return default(T);
            return JsonConvert.DeserializeObject<T>(target);
        }
    }
}