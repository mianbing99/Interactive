<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberDelete.aspx.cs" Inherits="Web.Interactive.MemberDelete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../layui/css/layui.css" rel="stylesheet" />
    <script src="../Js/jquery-1.11.3.js"></script>
    <script src="../layui/layui.js"></script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>删除成员</title>
    <style>
        html{height: 100%;}
        body{
            margin: 0px;
            font-size: 2rem;
            background: url('../Img/50.jpg');
            background-repeat: repeat-y;
            background-size: 100% 100%;
            font-family:"黑体";
            height: 100%;
        }
        .tab{
            width: 100%;
        }
        td{
            text-align: center;
            padding: 20px 0px;
        }
        td input {
            zoom:150%;
            width: 120px;
            background-color: #0bb7ef;
            border: 1px solid #73dbfd;
            border-radius: 5px;
            height: 50px;
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
        .content { background-image: url(../Img/成员管理框@2x.png); background-repeat: repeat-y; background-size: 100% 100%; padding: 5% 0;}
        .panle { height: 20%; background-color: #a1e4f7; font-size: 2.8rem; border-radius: 15px;}
        #tstitle { text-align: center; line-height: 80px; }
        #tscontent { text-align: center; line-height: 220px; }
        .delbut { background-image: url(../Img/删除.png); background-size: 100% 100%; border: none; }
        #sqr { width: 220px; height: 5rem; background-image: url(../Img/确定.png); background-size: 100% 100%; border: none; border-radius: 5px; position: relative; left: 33%; }
        #scqd { width: 220px; height: 5rem; background-image: url(../Img/确定.png); background-size: 100% 100%; border: none; border-radius: 5px; }
        #qr { width: 220px; height: 5rem; background-image: url(../Img/确定.png); background-size: 100% 100%; border: none; border-radius: 5px; position: relative; left: 33%; }
        #qx { width: 220px; height: 5rem; background-image: url(../Img/取消.png); background-size: 100% 100%; border: none; border-radius: 5px; position: relative; left: 10%; }
    </style>
    <script>
        var layer;
        layui.use('layer', function () {
            layer = layui.layer;
        });
        $.ajax({
            url: '/API/InteractiveService.asmx/GetMemberList',
            type: 'get',
            success: function (data) {
                var i = 0;
                $.each(JSON.parse(data).user_info_list, function (i, e) {
                    $('.tab tbody').append('<tr>' +
                                                '<td style="width: 30%"><img src="' + e.headimgurl + '" width="50%"/></td>' +
                                                '<td style="width: 40%;text-align: left;"><p>' + e.nickname + '</p></td>' +
                                                '<td style="width: 30%"><input type="button" name="aa" class="delbut"  /><input type="hidden" value="' + e.openid + '"/></td>' +
                                            '</tr>' +
                                            '<tr><td colspan="3"><hr style="border-top: 1px solid #0bb7ef; margin:0 3%;"/></td></tr>');
                });
                $("[name=aa]").click(function () {
                    var id = $(this).parent().find("[type=hidden]").val();
                    layer.open({
                        type: 1,
                        title: false,
                        closeBtn: 0,
                        shadeClose: true,
                        skin: 'panle',
                        content: '<div class="panle">' +
                                    '<p id="tstitle">温馨提示</p>' +
                                    '<hr style="border-top: 1px solid #18afda;"/>' +
                                    '<p id="tscontent">确认删除选中成员吗？</p>' +
                                    '<div style="margin-bottom: 20px;"><input type="button" value="" id="qx" /><input type="button" value="" id="qr" /></div>' +
                                '</div>'
                    });
                    $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                    $("#qr").click(function () {
                        layer.closeAll();
                        $.ajax({
                            url: '/API/InteractiveService.asmx/DeleteMember',
                            type: 'get',
                            data: { deleteOpenId: id },
                            success: function (data) {
                                if (data == 'true') {
                                    layer.open({
                                        type: 1,
                                        title: false,
                                        closeBtn: 0,
                                        shadeClose: true,
                                        skin: 'panle',
                                        time:1000,
                                        content: '<div class="panle">' +
                                                    '<p id="tstitle">温馨提示</p>' +
                                                    '<hr style="border-top: 1px solid #18afda;"/>' +
                                                    '<p id="tscontent">删除成功！</p>' +
                                                '</div>'
                                    });
                                    $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                                    window.location.href = "MemberDelete.aspx";
                                } else {
                                    layer.open({
                                        type: 1,
                                        title: false,
                                        closeBtn: 0,
                                        shadeClose: true,
                                        skin: 'panle',
                                        content: '<div class="panle">' +
                                                    '<p id="tstitle">温馨提示</p>' +
                                                    '<hr style="border-top: 1px solid #18afda;"/>' +
                                                    '<p id="tscontent">管理员不能删除自己！</p>' + '</div>'

                                    });
                                    $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                                }
                            },
                            error: function (e) {
                                alert(e.responseText);
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
                    $("#qx").click(function () {
                        layer.closeAll();
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
                                '<p id="tscontent">发生错误！错误信息：' + e.responseText + '</p>' +
                            '</div>'
                });
                $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
            }
        });
        $.ajax({
            url: '/API/InteractiveService.asmx/GetDeviceName',
            type: 'get',
            success: function (data) {
                if (data.length > 0) {
                    var data = JSON.parse(data);
                        $("#title h3").remove();
                        $("#title").append("<h3>当前设备名称:" + data.deviceName + "</h3>");
                        return false;
                    } else {
                        $("#title h3").remove();
                        $("#title").append('<h3>当前你还未绑定设备</h3>');
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="title"></div>
    <div class="content">
        <table class="tab">
            <tbody>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
