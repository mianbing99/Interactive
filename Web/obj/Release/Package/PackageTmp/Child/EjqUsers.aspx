<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EjqUsers.aspx.cs" Inherits="Web.Child.EjqUsers" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../layui/css/layui.css" rel="stylesheet" />
    <script src="../layui/layui.js"></script>
    <title>成员管理</title>
    <style>
        html{
            height: 100%;
        }
        body{
            margin: 0px;
            font-size: 2rem;
            background-color: #262727;
            font-family:"黑体";
        }
        #users,#caozuo{
            width: 100%;
            text-align:center;
        }
        #title{
            text-align: center;
        }
        #title h3{
            margin: 3% 20% 0 20%;
            background-color: white;
            
            width: 60%;
            border-radius: 15px;
            border: 1px solid black;
            overflow-x: hidden;
	        white-space:nowrap;
            text-overflow:ellipsis;
            line-height: 70px;
        }
        .itemleft{  
            float: left;
            width: 46%;
            height: 448px;
            font-size: 2rem;
            background: url('../Img/设备%20框@2x.png');
            background-repeat: no-repeat;
            background-size: 100% 100%;
            margin: 3% 1% 0 3%;
            padding-top:3%;
            padding-bottom: 3%;
        }
        .itemright{
            float: right;
            width: 46%;
            height: 448px;
            font-size: 2rem;
            background: url('../Img/设备%20框@2x.png');
            background-repeat: no-repeat;
            background-size: 100% 100%;
            margin: 3% 3% 0 1%;
            padding-top:3%;
            padding-bottom: 3%;
        }
        .itemleft img, .itemright img{
            border-radius: 15px;
        }
        .itemleft p, .itemright p{
            text-align: left;
            margin: 0px;
            margin-left: 7%;
            line-height: 40px;
            width: 100%;
            float: left;
        }
        .lab{
            background-color: #9ed3fb;
            display: block;
            border-radius: 10px;
            border: 1px solid black;
            width: 82%;
            margin: 3% 0 2% 2%;
            overflow-x: hidden;
	        white-space:nowrap;
            text-overflow:ellipsis;
            float: left;
            text-align: center;
        }
        .panle { height: 20%; background-color: #262727; font-size: 2.8rem; border-radius: 15px; color: white;}
        #tstitle { text-align: center; line-height: 80px; }
        #tscontent { text-align: center; line-height: 220px; }
        #qr { width: 220px; background-color: #303030; border: 3px solid #868687; border-radius: 15px; position: relative; left: 33%; color: white;}
        #qx { width: 220px; background-color: #303030; border: 3px solid #868687; border-radius: 15px; position: relative; left: 10%; color: white; }
    </style>
    <script src="../Js/jquery-1.11.3.js"></script>
    <script>
        var layer;
        layui.use('layer', function () {
            layer = layui.layer;
        });
        $(function () {
            $.ajax({
                url: '/API/WebService.asmx/EGetUsers',
                type: 'get',
                success: function (data) {
                    var i = 0;
                    $.each(JSON.parse(data).user_info_list, function (i, e) {
                        if (i % 2 != 0) {
                            $("#users").append('<div class="itemright">' +
                                                    '<img src="' + e.headimgurl + '" width="85%" />' +
                                                    '<p><label class="lab">' + e.nickname + '</label></p>' +
                                                '</div>');
                        } else {
                            if (i == 0) {
                                $("#users").append('<div class="itemleft">' +
                                                        '<img src="' + e.headimgurl + '" width="85%" />' +
                                                        '<p><label class="lab">' + e.nickname + '</label></p>' +
                                                        '<img src="../Img/管理员@2x.png" style="float: left; position: absolute; left: 4%; top: 140px;"/>' +
                                                    '</div>');
                            } else {
                                $("#users").append('<div class="itemleft">' +
                                                        '<img src="' + e.headimgurl + '" width="85%" />' +
                                                        '<p><label class="lab">' + e.nickname + '</label></p>' +
                                                    '</div>');
                            }
                        }
                        i++;
                    });
                    $("#caozuo").append('<div class="itemleft">' +
                                            '<a href="../EWM2.html"><img src="../Img/加@2x.png" width="85%" /></a>' +
                                            '<p><label class="lab" style="">邀请成员</label></p>' +
                                        '</div>' +
                                        '<div class="itemright" name="jian">' +
                                            '<img src="../Img/减@2x.png" width="85%" />' +
                                            '<p><label class="lab" style="">删除成员</label></p>' +
                                        '</div>');
                    $("[name=jian]").click(function () {
                        $.ajax({
                            url: '/API/WebService.asmx/ECheckUser',
                            type: 'get',
                            success: function (data) {
                                if (data == 'true') {
                                    location.href = 'EDelUser.aspx';
                                } else {
                                    layer.open({
                                        type: 1,
                                        title: false,
                                        closeBtn: 0,
                                        shadeClose: true,
                                        skin: 'panle',
                                        time: 1000,
                                        content: '<div class="panle">' +
                                                    '<p id="tstitle">温馨提示</p>' +
                                                    '<hr style="border-top: 1px solid #18afda;"/>' +
                                                    '<p id="tscontent">' + data + '</p>' +
                                                '</div>'
                                    });
                                    $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                                }
                            },
                            error: function (e) {
                                layer.open({
                                    type: 1,
                                    title: false,
                                    closeBtn: 0,
                                    shadeClose: true,
                                    skin: 'panle',
                                    time: 1000,
                                    content: '<div class="panle">' +
                                                '<p id="tstitle">温馨提示</p>' +
                                                '<hr style="border-top: 1px solid #18afda;"/>' +
                                                '<p id="tscontent">发生错误！错误信息：' + e.responseText + '</p>' +
                                            '</div>'
                                });
                                $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                            }
                        });
                    });
                },
                error: function (e) {
                    layer.open({
                        type: 1,
                        title: false,
                        closeBtn: 0,
                        shadeClose: true,
                        skin: 'panle',
                        time: 1000,
                        content: '<div class="panle">' +
                                    '<p id="tstitle">温馨提示</p>' +
                                    '<hr style="border-top: 1px solid #18afda;"/>' +
                                    '<p id="tscontent">你还未绑定设备或者设备未激活!</p>' +
                                '</div>'
                    });
                    $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                }
            });
            $.ajax({
                url: '/API/WebService.asmx/EGetDevices',
                type: 'get',
                success: function (data) {
                    $.each(JSON.parse(data), function (i, e) {
                        if (e.State == true) {
                            $("#title h3").remove();
                            $("#title").append("<h3>当前设备:" + e.DeviceName + "</h3>");
                            return false;
                        } else {
                            $("#title h3").remove();
                            $("#title").append('<h3>当前你还未绑定设备</h3>');
                        }
                    });
                },
                error: function (e) {
                    layer.open({
                        type: 1,
                        title: false,
                        closeBtn: 0,
                        shadeClose: true,
                        skin: 'panle',
                        time: 1000,
                        content: '<div class="panle">' +
                                    '<p id="tstitle">温馨提示</p>' +
                                    '<hr style="border-top: 1px solid #18afda;"/>' +
                                    '<p id="tscontent">发生错误！错误信息：' + e.responseText + '</p>' +
                                '</div>'
                    });
                    $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="title"></div>
        <div id="users">
        </div>
        <div style="clear: both;"></div>
        <div id="caozuo">
        </div>
    </div>
    </form>
</body>
</html>