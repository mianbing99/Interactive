<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Web.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>e家亲</title>
    <link href="Css/style.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.css" />
    <style type="text/css">
        #videotype .ui-btn-inner { padding: 10px; }
        #videotype .ui-btn-inner img { display: block; width: 100%; height: 100%; border: none; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div data-role="page" data-theme="a" id="pageone" >
            <div data-role="header" data-position="fixed">
                <div data-role="navbar" id="OneType" style="width:100%">
                    <ul style="display:block; width:100%">
                        <li style="width:25%"><a href="index.aspx?t=1" target="_blank" val="1">老人平台</a></li>
                        <li style="width:25%"><a href="index.aspx?t=2" target="_blank" val="2">幼儿平台</a></li>
                        <li style="width:25%"><a href="videolist.aspx?q=-1" target="_blank" val="3">最新资源</a></li>
                        <li style="width:25%"><a href="feedback.aspx" target="_blank" val="4">资源反馈</a></li>
                    </ul>
                </div>
            </div>
            <div data-role="content">
                <div class="ui-grid-b" id="videotype">
                    <%--<div class="ui-block-a">
                        <a href="#" data-role="button">
                            <img src="http://v.icoxedu.cn/Img/laoren/xiqu/2.png" />
                        </a>
                    </div>--%>
                </div>
            </div>
            <%--<div data-role="footer" data-position="fixed">
                <h1>金龙锋科技有限公司</h1>
            </div>--%>
        </div>
    </form>
    <script src="http://code.jquery.com/jquery-1.8.3.min.js"></script>
    <script src="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
           var  a = GetQueryString("a");
            if (a == -1) {
                window.location.href = "videolist.aspx?q=" + a; //链接最新资源
            }

            var a = GetQueryString("t");
            if (a > 0) {
                InitListType(a);
            } else {
                InitListType(1);
            }
            //$("#OneType a").click(function () {
            //    var id = $(this).attr("val");
            //    switch(id)
            //    {
            //        case "1": window.location.href = "index.aspx?t=" + $(this).attr("val"); document.getElementById("lr_a").style.background="#aaa"; break;
            //        case "2": window.location.href = "index.aspx?t=" + $(this).attr("val"); document.getElementById("ye_a").style.background = "#eee"; break;
            //        case "3": window.location.href = "videolist.aspx?q=-1";break;
            //        case "4": window.location.href = "feedback.aspx";break;
            //    }
            //});
            $("#videotype").delegate("a", "click", function () {
                window.location.href = "index.aspx?t=" + $(this).attr("val");
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
                    } else {
                        window.location.href = "videolist.aspx?q=" + id;
                    }
                }
            });
        }
        function BindListType(data) {
            $("#videotype div").remove();
            for (var i = 0; i < data.length; i++) {
                var ca = "";
                switch (i % 3) {
                    case 0:
                        ca = "ui-block-a";
                        break;
                    case 1:
                        ca = "ui-block-b";
                        break;
                    case 2:
                        ca = "ui-block-c";
                        break;
                }
                var html = '<div class="' + ca + '">';
                html += '<a val="' + data[i].Id + '" class="ui-btn ui-shadow ui-btn-corner-all ui-btn-up-a" href="#" data-role="button" data-theme="a" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperEls="span">';
                html += '<span class="ui-btn-inner">';
                html += '<span class="ui-btn-text">';
                html += '<img src="' + data[i].Cover + '"/>';
                html += '</span>';
                html += '</span>';
                html += '</a>';
                html += '</div>';
                $("#videotype").append(html);
            }
        }
        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return (r[2]); return null;
        }
    </script>
    <p>
&nbsp;</p>
</body>
</html>
