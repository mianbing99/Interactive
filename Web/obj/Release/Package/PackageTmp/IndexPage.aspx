<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexPage.aspx.cs" Inherits="Web.IndexPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>幼儿伴侣</title>
    <link href="Css/style.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.css" />
    <style type="text/css">       
        #videotype .ui-btn-inner { padding: 10px; }
        #videotype .ui-btn-inner img { display: block; width: 100%; height: 100%; border: none; }
        html { height: 100%; width: 100%; }
        body { height: 100%; width: 100%; background-image: url(/Img/5.jpg); background-size: cover; background-repeat: no-repeat; }
        #OneType ul li{ 
        float:left ;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div data-role="page" data-theme="a" id="pageone" style="background-color: #d6dbe9">
            <div data-role="header" data-position="fixed">
                <div data-role="navbar" id="OneType">
                    <ul>
                        <li><a href="#" val="1" class="ui-state-persist">视频列表</a></li>
                        <li><a href="#" val="2">最新资源</a></li>
                        <li><a href="#" val="3">资源反馈</a></li>
                    </ul>
                </div>
            </div>
            <div data-role="content">
                <div class="ui-grid-b" id="videotype">
                </div>
            </div>
        </div>
    </form>
</body>
<script src="http://code.jquery.com/jquery-1.8.3.min.js"></script>
<script src="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        var a = GetQueryString("a");
        if (a == -1) {
            window.location.href = "Childvideolist.aspx?q=" + a;
        }
        var a = GetQueryString("t");
        if (a > 0) {
            InitListType(a);
        } else {
            InitListType(2);
        }
        $("#OneType a").click(function () {
            var id = $(this).attr("val");
            if (id == 1) {
                window.location.href = "IndexPage.aspx?t=2";
            }
            if (id == 2) {
                window.location.href = "Childvideolist.aspx?q=-1";
                
            } else if (id == 3) {
                window.location.href = "ResourceFeedback.aspx";
            }
            else {
                window.location.href = "IndexPage.aspx?t=2";
                //InitListType($(this).attr("val"));
            }
        });
        $("#videotype").delegate("a", "click", function () {
            window.location.href = "IndexPage.aspx?t=" + $(this).attr("val");
        });
    });
    function InitListType(id) {
        $.ajax({
            url: '/API/WebService.asmx/GetChildVideo',
            type: 'post',
            dataType: 'json',
            data: { "typeid": id },
            success: function (data) {
                if (data.length > 0) {
                    BindListType(data);
                } else {
                    window.location.href = "Childvideolist.aspx?q=" + id;
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
</html>
