<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="videolist.aspx.cs" Inherits="Web.videolist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>e家亲</title>
    <link href="Css/style.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.css" />
    <style type="text/css">
        .lzb { position: absolute; width: 40px; height: 100%; }
        .lzbdiv { margin-left: 5px;}
        #VideoList .ui-link-inherit{padding-top:1em;padding-bottom:1em;}
        a { text-decoration: none; /*去除下划线*/ color: white; font-size:13px; }
        #LoadVideo { display:none; width: 60%; margin: 0 20%;}
        #TypeTitle { width: 60%;  margin: 0 20%; text-align: center; background-color:#303030; line-height: 50px; border: 1px solid #868687; border-radius: 10px;}
        .tuisongspan{background-color: #303030; border-radius: 3px; position: relative; top: 18%; right: 50%; width: 150%; line-height: 25px; text-align: center; border: 1px solid #868687;}
        #VideoList { width: 100%; background-color: #5c5c5c; border-radius: 10px; border: 2px solid #868687;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div data-role="page" data-theme="a" id="pageone">
            <div data-role="header" data-position="fixed">
                <a href="#" id="Home" data-icon="home" data-role="button">首页</a>
                <h1>e家亲</h1>
            </div>
            <div data-role="content">
                <h2 id="TypeTitle"></h2>
                <ul id="VideoList" data-role="listview" data-inset="true">
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
            if (q == null||q==0) {
                window.location.href = "/";
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
                window.location.href = "/";
            });
            $("#VideoList").delegate(".tuisong", "tap", function () {
                var q = $(this).attr("val");
                var p = $(this).attr("position");
                $.ajax({
                    url: '/API/WebService.asmx/WeChatPushMsg',
                    type: 'post',
                    dataType: 'text',
                    data: { "msg": q ,"post":p},
                    success: function (data) {
                        layer.open({
                            content: data,
                            style: 'background-color:#282a40; color:#fff; border:1px solid #3a3d5c;text-align:center;',
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
                        //alert(e.responseText);
                    }
                });
            } else {
                $("#TypeTitle").text("最近更新");
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
                        if (Math.ceil(count / size) > 1 &&index<Math.ceil(count / size)) {
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
                var html = '<li class=" ui-btn-icon-right ui-li colre " style="width:100%;height:44px" data-theme="a" data-icon="false" data-corners="false" data-shadow="false" data-iconshadow="true" data-wrapperels="div" data-iconpos="right">';
                html += '<div class="ui-btn-inner ui-li ui-li-has-alt">';
                html += '<div class="ui-btn-text" style="text-align:right" >';

                html += '<div style="text-align:right" >';
                html += '<a title="推送"  style="margin-left:-50px;margin-top:1px;"   val="' + data[i].Id + '" position="' + position + '" class="tuisong lzb" href="#" data-theme="a" data-icon="false" data-corners="false" data-shadow="false" data-iconshadow="true" data-wrapperels="span" data-iconpos="notext">';
                html += '<p class="tuisongspan">推送</p>';
                html += '</a>';
                html += '</div>';
                html += '<div style="text-align:left;" >';
                html += '<a class="ui-link-inherit lzbdiv"   href="#">' + data[i].Title + '</a></div>';
                html += '</div>';

                html += '</div><br/>';
                html += '</li>';
                position++
                $("#VideoList").append(html);
                /*i++;

                html = '<li class=" ui-btn-icon-right ui-li colre " style="width:105%;background-color:#3A3433;height:44px" data-theme="a" data-icon="false" data-corners="false" data-shadow="false" data-iconshadow="true" data-wrapperels="div" data-iconpos="right">';
                html += '<div class="ui-btn-inner ui-li ui-li-has-alt">';
                html += '<div class="ui-btn-text" style="text-align:right" >';
                html += '<div style="text-align:right" >';
                html += '<a title="推送"  style="margin-left:-50px;margin-top:1px;"   val="' + data[i].Id + '" position="' + position + '" class="tuisong lzb" href="#" data-theme="a" data-icon="false" data-corners="false" data-shadow="false" data-iconshadow="true" data-wrapperels="span" data-iconpos="notext">';

                html += '<span class="ui-btn-text"></span>';
                html += '<span title="" class="ui-btn ui-btn-up-b ui-shadow ui-btn-corner-all ui-btn-icon-notext" data-theme="b" data-icon="arrow-r" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperels="span" data-iconpos="notext">';
                html += '<span class="ui-btn-inner"><span ></span>';
                html += '<span class="ui-icon ui-icon-arrow-r ui-icon-shadow">';
                html += '&nbsp;';
                html += '</span>';
                html += '</span>';
                html += '</span>';
                html += '</span>';
                html += '</a>';
                html += '</div>';
                html += '<div style="text-align:left;" >';
                html += '<a class="ui-link-inherit lzbdiv" href="#">' + data[i].Title + '</a></div>';
                html += '</div>';
                html += '</div><br/>';
                html += '</li>';
                position++
                $("#VideoList").append(html);*/

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
