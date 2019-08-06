<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestVideoPlay.aspx.cs" Inherits="Web.Admin.TestVideoPlay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
   <link href="js/layer/btnCss/layui.css" rel="stylesheet"/>
    <style>

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <input id="start" type="button" value="检测" 
                    class="layui-btn layui-btn-primary layui-btn-mini" style="color:green"/>
       <div style="height:500px" id="list">

       </div>
    </form>
</body>
</html>
<script src="js/jquery-1.11.3.js"></script>
<script src="js/layer/layer.js"></script>
<script>
    var c=0;
    $("#start").click(function () {
        $.post(
            "ashx/checkYK.ashx",
            { api: "yi" },
            function (data) {
                var obj = eval("(" + data + ")");
                
                if (obj.msg == "ok") {
                    for (var i = 0; i < obj.data.length ;i++) {
                        $("#list").append(obj.data[i].Id + "__" + obj.data[i].Vid + "__" + obj.data[i].Path + "__" + obj.data[i].jg+"<br />");
                        c++;
                    }
                    alert("优酷视频100内共有"+c+"个无法解析");
                } else {
                    $("#list").append("error:"+obj.msg);
                }
                
            }
            );
    });
    

</script>