<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="feedback.aspx.cs" Inherits="Web.feedback" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>e家亲</title>
    <link href="Css/style.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.css" />
    <style type="text/css">
        .ui-block-a,
        .ui-block-b { border: 1px solid black; text-align: center; padding: 10px; }
        .ui-block-a { width: 100px; }
        #ftext { height: auto; min-height: 200px; line-height: 30px; overflow: hidden; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div data-role="page" data-theme="a" id="pageone">
            <div data-role="header" data-position="fixed">
                <a href="#" id="Home" data-icon="home" data-role="button">首页</a>
                <h1>资源反馈</h1>
            </div>
            <div data-role="content" id="content">
                <div id="type">
                    <label for="day">选择分类</label>
                    <%--   <select name="day" id="day">
                    <option value="0">请选择</option>
                    <option value="1">老人</option>
                    <option value="2">幼儿</option>
                </select>--%>
                </div>
                <textarea id="ftext"></textarea>
                <input type="button" id="submit" value="提交" />
            </div>
        </div>
    </form>
    <script src="http://code.jquery.com/jquery-1.8.3.min.js"></script>
    <script src="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.js"></script>
    <script type="text/javascript">
        var index = 1;
        $(document).ready(function () {
            InitListType(0);
            $("#type").delegate("select[id^=list_]", "change", function () {
                var id = $(this).find("option:selected").val();
                if (id > 0) {
                    var inx = parseInt($(this).attr("id").split("_")[1]);
                    inx = inx + 1;
                    var coun = $("#type div[id^=type_]").length;
                    for (var i = inx; i <= coun; i++) {
                        $("#type div[id=type_" + i + "]").remove();
                    }
                    index = inx;
                    InitListType(id);
                }
            });
            $("#Home").on("tap", function () {
                window.location.href = "/";
            });
            $("#submit").click(function () {
                $("#submit").attr("disabled", true);
                var str = "";
                var coun = $("#type div[id^=type_]").length;
                for (var i = 1; i <= coun; i++) {
                    str += $("#type select[id=list_" + i + "] option:selected").text();
                    if (i < coun) {
                        str += "-";
                    }
                }
                str += " : " + $("#ftext").val();
                $.ajax({
                    url: '/API/WebService.asmx/WriteLog',
                    type: 'post',
                    dataType: 'json',
                    data: { "text": str },
                    success: function (data) {
                        if (data == "1") {
                            window.location.href = "/";
                            $("#submit").attr("disabled", false);
                        }
                    },
                    error: function (e) {
                        $("#submit").attr("disabled", false);
                    }
                });
            });
        });
        function InitListType(id) {
            $.ajax({
                url: '/API/WebService.asmx/GetVideoType',
                type: 'post',
                dataType: 'json',
                data: { "typeid": id },
                success: function (data) {
                    if (data.length > 0) {
                        BindListType(data);
                    }
                }
            });
        }
        function BindListType(data) {
            var html = '<div id="type_' + index + '">';
            html += '<select id="list_' + index + '">';
            html += '<option value="0">请选择</option>';
            for (var i = 0; i < data.length; i++) {
                html += '<option value="' + data[i].Id + '">' + data[i].Title + '</option>';
            }
            html += '</select>';
            html += '</div>';
            $("#type").append(html);
            $("#type").trigger("create");
        }
    </script>
</body>
</html>
