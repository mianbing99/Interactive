<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VidelListType.aspx.cs" Inherits="Web.Child.ChildVidelList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>资源列表</title>
    <link href="../Css/style.css" rel="stylesheet" />
    <link href="../Css/ChildThree.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.css" />
    <style type="text/css">
    form{min-height: 667px;}
        .lzb { position: absolute; width: 40px; height: 100%; }
        .lzbdiv { margin-left: 0px; }
        #VideoList .ui-link-inherit { padding-top: 1em; padding-bottom: 1em; }
        #LoadVideo { display: none; width: 60%; margin-left: 20%;}
        a { text-decoration: none; /*去除下划线*/ color: white; font-size:13px; }
        form{background-image: url(/Img/50.jpg); background-size: cover; background-repeat: repeat-y;}
        ul li { color: white; }
        body { height: 100%; width: 100%; font-family:"黑体"; }
        #title { height: 40px; width: 100%; line-height: 40px;  color:black}
        /*#titlevidel { height: 41px; width: 95%; background-color:white; }*/
        #list { height: 40px; width: 100%; float: left; font-size: 20px; text-align: center;}
        #videl { height: 40px; width: 45%; float: right; font-size: 20px; text-align: right; } 
        .right { margin-right: 10px; }
        .colre{background-color:#F8F8F8}
        .ui-content{background-image: url(/Img/50.jpg); background-size: cover; background-repeat: repeat-y; padding-bottom: 0px;}
        .tuisongspan{background-color: #0bb7ef; border-radius: 5px; position: relative; top: 10%; right: 50%; width: 150%; line-height: 30px; text-align: center}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="box">
            <%--<div data-role="page" data-theme="a" id="pageone" style="background-color:red;">--%>
            <div id="nav" style="position: fixed">
                <!-- 导航栏 -->
                <ul style="color: white;">
                    <li><a href="ChildIndex.aspx" target="_blank">0—3岁</a></li>
                    <li><a href="ChildrenPage.aspx" target="_blank">4—6岁</a></li>
                    <li class="navclass"><a href="VidelListType.aspx?q=-1" target="_blank">最新资源</a></li>
                </ul>
            </div>
        </div>
        <div data-role="content">
            <div id="title" style="width: 60%; background-color: white; line-height: 50px; height: 50px; border: 1px solid #A8BFDB; margin-left: 20%; border-radius: 10px;">
                <div id="list" >
                    <h3 id="TypeTitle">资源列表</h3>
                </div>
            </div>
            <ul id="VideoList" data-role="listview" data-inset="true" style="width: 96%; background-color: white; background-image: url('../Img/设备%20框@2x.png'); padding: 0 2%; background-repeat: no-repeat; background-size: 100% 100%;">
            </ul>
        </div>
        <a href="#" id="LoadVideo" data-role="button">加载更多</a>
    </form>
</body>
<script src="../Js/jquery-1.11.0.min.js"></script>
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
            window.location.href = "ChildIndex.aspx";
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
                        style: 'background-color:#f2c149; color:#fff; border:1px solid #3a3d5c;  text-align:center',
                        time: 2
                    });
                },
                error: function (e) {
                    layer.open({
                        content: "网络繁忙！",
                        style: 'background-color:#f2c149; color:#fff; border:1px solid #3a3d5c;  text-align:center',
                        time: 2
                    });
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
            $("#TypeTitle").text("资源列表");
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
                //alert(e.responseText);
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
            html += '<hr style="border-top: 1px solid #0bb7ef;"/>';
            position++
            $("#VideoList").append(html);
            /*i++

            html = '<li class=" ui-btn-icon-right ui-li colre " style="width:105%;height:44px" data-theme="a" data-icon="false" data-corners="false" data-shadow="false" data-iconshadow="true" data-wrapperels="div" data-iconpos="right">';
            html += '<div class="ui-btn-inner ui-li ui-li-has-alt">';
            html += '<div class="ui-btn-text" style="text-align:right" >';
            html += '<div style="text-align:right" >';
            html += '<a title="推送"  style="margin-left:-50px;margin-top:1px;"   val="' + data[i].Id + '" position="' + position + '" class="tuisong lzb" href="#" data-theme="a" data-icon="false" data-corners="false" data-shadow="false" data-iconshadow="true" data-wrapperels="span" data-iconpos="notext">';
            html += '<p class="tuisongspan">推送</p>';
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
</html>
