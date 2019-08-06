using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat;

namespace Web.API {
    public static class AccTokenHelp {
        /*private string GetToken() {
            WeChatToken wct = null;
            Application.Lock();
            if (Application["Token"] != null) {
                wct = Application["Token"] as WeChatToken;
                if (wct != null && (DateTime.Now.Subtract(wct.CreateTime)).TotalSeconds < wct.Expires - 200) {
                    return wct.Token;
                }
            }
            Dictionary<string, object> dic = WeChatHelper.GetToken();
            wct = new WeChatToken(dic["access_token"] + "", Convert.ToInt32(dic["expires_in"] + ""));
            Application["Token"] = wct;
            Application.UnLock();
            return wct.Token;
        }*/
    }
}