<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="delete.aspx.cs" Inherits="Web.Admin.delete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #txt_column {
            width: 113px;
        }
        #Select_videotype {
            width: 127px;
        }
        #txt_columnvideourl {
            width: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    通过字段名来删除符合字段名的全部数据
        <br/>
        <div>
            视频表:
            <select id="Select_video" style="width:127px">
                <option>Id</option>
                <option>Tid(类型表的Id)</option>
                <option>标题</option>
                <option>图片路径</option>
                <option>描述</option>
                <option>创建时间</option>
                <option>优先级</option>
                <option>是否显示</option>
                <option>清晰度</option>
                <option>是否有广告</option>

            </select>
            名为<input id="txt_columnvideo" type="text" runat="server" style="width:100px"/>
            <input id="video_btn" type="button" value="查询" />
        </div>
        <br/>
        <div>
            类型表:
            <select id="Select_videotype" style="width:127px">
                <option>Id</option>
                <option>Tid(自己的Id)</option>
                <option>标题</option>
                <option>图片路径</option>
                <option>优先级</option>
                <option>状态</option>
                
                
            </select>
            名为<input id="txt_columnvideotype" type="text" runat="server" style="width:100px"/>
            <input id="videotype_btn" type="button" value="查询" />
        </div>
        <br/>
        <div>
            路径表:
           <select id="Select_videourl" style="width:127px">
               <option>Id</option>
                <option>Vid(视频表的Id)</option>
                <option>信道</option>
                <option>路径</option>
                <option>解析(加密)路径</option>
                <option>状态</option>
                <option>优先级</option>
                <option>建立时间</option>
                <option>清晰度</option>
                <option>是否有广告</option>
                
                
            </select>
            名为<input id="txt_columnvideourl" type="text" runat="server"  style="width:100px" />
            <input id="videourl_btn" type="button" value="查询" />
        </div>
        
    
    </div>
    </form>
</body>
</html>
