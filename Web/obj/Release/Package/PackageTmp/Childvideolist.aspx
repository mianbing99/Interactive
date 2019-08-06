<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Childvideolist.aspx.cs" Inherits="Web.Childvideolist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>幼儿伴侣</title>
    <link href="Css/style.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.css" />
    <style type="text/css">
        .lzb { position: absolute; width: 40px; height: 100%; }
    .lzbdiv { margin-left: 50px; }
        #VideoList .ui-link-inherit{padding-top:1em;padding-bottom:1em;}
        #LoadVideo { display:none;}
        /*ul { color:#fff}*/
        body { height: 100%; width: 100%; background-image: url(/Img/5.jpg); background-size: cover; background-repeat: no-repeat; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div data-role="page" data-theme="a" id="pageone">
            <div data-role="header" data-position="fixed">
                <div data-role="navbar" id="OneType">
                    <ul>
                        <li><a href="IndexPage.aspx" val="1" target="_top">视频列表</a></li>
                        <li><a href="#" val="2" class="ui-state-persist">最新资源</a></li>
                        <li><a href="ResourceFeedback.aspx" val="3" target="_top">资源反馈</a></li>
                    </ul>
                </div>
            </div>
            <div data-role="content">
                <h2 id="TypeTitle"></h2>
                <ul id="VideoList" data-role="listview" data-inset="true" style="width:100px">
                </ul>

            </div>
            <a href="#" id="LoadVideo" data-role="button">加载更多</a>
        </div>
    </form>
    <script src="http://code.jquery.com/jquery-1.8.3.min.js"></script>
    <script src="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.js"></script>
    <script src="/Js/layer.m/layer.m.js"></script>
    <script type="text/javascript">
        var index = 1;
        var size = 50;
        var count = 0;
        var q = 0;
        var position = 0;
        $(document).ready(function () {
            position = 0;
            q = GetQueryString("q");
            if (q == null || q == 0) {
                window.location.href = "IndexPage.aspx";
            }
            $("#outpage").click(function () {
                history.go(-1);
            });
            InitType();
            $("#VideoList li").remove();
            InitListVideo();
            $("#LoadVideo").on("tap", function () {
                index++;
                if (index >= Math.ceil(count / size)) {
                    $("#LoadVideo").css('display', 'none')
                }
                InitListVideo();
            });
            $("#Home").on("tap", function () {
                window.location.href = "IndexPage.aspx";
            });
            $("#VideoList").delegate(".tuisong", "tap", function () {
                var q = $(this).attr("val");
                var p = $(this).attr("position");
                $.ajax({
                    url: '/API/WebService.asmx/WeChildPushMsg',
                    type: 'post',
                    dataType: 'text',
                    data: { "msg": q, "post": p },
                    success: function (data) {
                        layer.open({
                            content: data,
                            style: 'background-color:#f2c149; color:#fff; border:1px solid #3a3d5c; text-align:center',
                            time: 2
                        });
                    },
                    error: function (e) {
                        alert(e.responseText);
                    }
                });
            });
        });
        function InitType() {
            if (q > 0) {
                $.ajax({
                    url: '/API/WebService.asmx/GetTypeTitle',
                    type: 'post',
                    dataType: 'json',
                    data: { "typeid": q },
                    success: function (data) {
                        if (data.length > 0) {
                            $("#TypeTitle").text(data[0].Title);
                        }
                    },
                    error: function (e) {
                        alert(e.responseText);
                    }
                });
            } else {
                $("#TypeTitle").text("最新资源");
            }
        }
        //GetVideoPage(int IndexPage, int PageSize, int TypeId)
        function InitListVideo() {
            $.ajax({
                url: '/API/WebService.asmx/GetVideoPage',
                type: 'post',
                dataType: 'json',
                data: { "IndexPage": index, "PageSize": size, "TypeId": q },
                success: function (data) {
                    if (data.Count > 0) {
                        count = data.Count;
                        if (Math.ceil(count / size) > 1 && index < Math.ceil(count / size)) {
                            $("#LoadVideo").css('display', 'block')
                        }
                        BindListVideo(data.Data);
                    }
                },
                error: function (e) {
                    alert(e.responseText);
                }
            });
        }
        function BindListVideo(data) {
            for (var i = 0; i < data.length; i++) {
                var html = '<li class="ui-btn ui-btn-icon-right ui-li ui-li-has-alt ui-btn-up-a" data-theme="a" data-icon="false" data-corners="false" data-shadow="false" data-iconshadow="true" data-wrapperels="div" data-iconpos="right">';
                html += '<div class="ui-btn-inner ui-li ui-li-has-alt">';
                html += '<div class="ui-btn-text">';
                html += '<a title="推送"  val="' + data[i].Id + '" position="' + position + '" class="tuisong  lzb " href="#" data-theme="a" data-icon="false" data-corners="false" data-shadow="false" data-iconshadow="true" data-wrapperels="span" data-iconpos="notext">';
                html += '<span class="ui-btn-inner">';
                html += '<span class="ui-btn-text"></span>';
                html += '<span title="" class="ui-btn ui-btn-up-b ui-shadow ui-btn-corner-all ui-btn-icon-notext" data-theme="b" data-icon="arrow-r" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperels="span" data-iconpos="notext">';
                html += '<span class="ui-btn-inner"><span class="ui-btn-text"></span>';
                html += '<span class="ui-icon ui-icon-arrow-r ui-icon-shadow">';
                html += '&nbsp;';
                html += '</span>';
                html += '</span>';
                html += '</span>';
                html += '</span>';
                html += '</a>';
                html += '<a class="ui-link-inherit lzbdiv" href="#">' + data[i].Title + '</a></div>';
                html += '</div>';
                html += '</li>';
                position++
                $("#VideoList").append(html);
            }
        }
        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return (r[2]); return null;
        }
    </script>
</body>
</html>
