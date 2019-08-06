<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceManagement.aspx.cs" Inherits="Web.Interactive.DeviceManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../layui/css/layui.css" rel="stylesheet" />
    <script src="../layui/layui.js"></script>
    <script src="../Js/jquery-1.11.3.js"></script>
    <title>设备管理</title>
    <style>
        html,body{
            height: 100%;
        }
        body{
            margin: 0px;
            font-size: 2rem;
            background: url('../Img/50.jpg');
            background-repeat: repeat-y;
            background-size: 100% 100%;
            font-family:"黑体";
        }
        #devices{
            width: 100%;
            text-align:center;
        }
        #title{
            text-align:center;
        }
        .itemleft{  
            float: left;
            width: 46%;
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
            width: 60%;
            margin: 1% 0 2% 2%;
            overflow-x: hidden;
	        white-space:nowrap;
            text-overflow:ellipsis;
            float: left;
        }
        .btn{
            width: 30%;
            height:2.5rem;
            background-image:url(../Img/切换设备.png);
            background-size:100% 100%;
            font-size: 24px;
            border:none;
            background-color: #0bb7ef; 
            border-radius: 10px;
	        padding: 0px;
        }
        .btndel{
            width: 30%;
            height:2.5rem;
            background-image:url(../Img/删除设备.png);
            background-size:100% 100%;
            font-size: 24px;
            border:none;
            background-color: #0bb7ef; 
            border-radius: 10px;
	        padding: 0px;
        }
        .left{
            float: left;
        }
        .panle { height: 20%; background-color: #a1e4f7; font-size: 2.8rem; border-radius: 15px;}
        #tstitle { text-align: center; line-height: 80px; }
        #tscontent { text-align: center; line-height: 220px; }
         #qr { width: 220px; height: 5rem; background-image: url(../Img/确定.png); background-size: 100% 100%; border: none; border-radius: 5px; position: relative; left: 33%; }
        #qx { width: 220px; height: 5rem; background-image: url(../Img/取消.png); background-size: 100% 100%; border: none; border-radius: 5px; position: relative; left: 10%; }
      
    </style>
    <script>
        var layer;
        layui.use('layer', function () {
            layer = layui.layer;
        });
        function getPicture(imgs) {
            $(imgs).parent().find('[name=photo]').click();
        }

        function setImage(docObj) {
            var deviceId = $(docObj).parent().find('[type=hidden]').val();
            var f = $(docObj).val();
            f = f.toLowerCase();
            var strRegex = ".bmp|jpg|jpeg$";
            var re = new RegExp(strRegex);
            if (re.test(f.toLowerCase())) {
                layer.msg('<span style="font-size: 3rem; line-height: 80px;">图片上传中！</span>', { time: 2000 });
            }
            else {
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
                                '<p id="tscontent">只支持JPG格式的图片！</p>' +
                            '</div>'
                });
                $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                file = $("#photo");
                file.after(file.clone());
                file.remove();
                return false;
            }
            alert("11");
            if (docObj.files && docObj.files[0]) {
                layer.msg('<span style="font-size: 3rem; line-height: 80px;">图片上传成功！</span>', { time: 2000 });
                var image = '';
                var reader = new FileReader();
                reader.onload = function (evt) {
                    $(docObj).parent().find('img').attr("src", evt.target.result);
                    //imgObjPreview.src = evt.target.result;
                    image = evt.target.result;
                    $.ajax({
                        type: 'POST',
                        url: '/API/InteractiveService.asmx/updateDeviceAvatar',
                        data: { image: image, deviceId: deviceId },
                        async: false,
                        dataType: 'json',
                        success: function (data) {
                            if (data == "true") {
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
                                                '<p id="tscontent">图片上传成功！</p>' +
                                            '</div>'
                                });
                                $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                            } else {
                                //alert('上传失败');
                            }
                        },
                        error: function (err) {
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
                                            '<p id="tscontent">网络故障！</p>' +
                                        '</div>'
                            });
                            $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                        }
                    });
                }
                reader.readAsDataURL(docObj.files[0]);

            }
            return true;
        }
        $(function () {
            $.ajax({
                url: '/API/InteractiveService.asmx/GetDeviceList',
                type: 'post',
                dataType: 'text',
                data: {},
                success: function (data) {
                    if (data == "null") {
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
                                        '<p id="tscontent">当前没有绑定设备！</p>' +
                                    '</div>'
                        });
                        $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                    } else {
                        var i = 1;
                        $.each(JSON.parse(data), function (i, e) {
                            var state = "";
                            if (e.state == true) {
                                state = "当前管控推送设备";
                            } else {
                                state = "空闲非管控设备";
                            }
                            if (i % 2 != 0) {
                                $("#devices").append('<div class="itemright">' +
                                                        '<img src="' + e.deviceAvatar + '" width="85%" onclick="getPicture(this)" height="383"/>' +
                                                        '<input type="file" name="photo" onchange="setImage(this);" style="display: none" />' +
                                                        '<p><label class="left">名称:</label><label class="lab">' + e.deviceName + '<input type="hidden" value="' + e.deviceId + '"/></label></p>' +
                                                        '<p><label class="left">状态:</label><label class="lab" name="state">' + state + '</label></p>' +
                                                        '<p><label class="left">权限:</label>&nbsp;<input type="button" name="qh" value="" class="btn"/>&nbsp;<input type="button" name="del" value="" class="btndel"/></p>' +
                                                    '</div>');
                            } else {
                                $("#devices").append('<div class="itemleft">' +
                                                        '<img src="' + e.deviceAvatar + '" width="85%" onclick="getPicture(this)" height="383"/>' +
                                                        '<input type="file" name="photo" onchange="setImage(this);" style="display: none" />' +
                                                        '<p><label class="left">名称:</label><label class="lab">' + e.deviceName + '<input type="hidden" value="' + e.deviceId + '"/></label></p>' +
                                                        '<p><label class="left">状态:</label><label class="lab" name="state">' + state + '</label></p>' +
                                                        '<p><label class="left">权限:</label>&nbsp;<input type="button" name="qh" value="" class="btn"/>&nbsp;<input type="button" name="del" value="" class="btndel"/></p>' +
                                                    '</div>');
                            }
                            i++;
                        });
                        $("[name=qh]").click(function () {
                            var btn = $(this);
                            var deviceId = $(this).parent().parent().find("[type=hidden]").val();
                            $.ajax({
                                url: '/API/InteractiveService.asmx/updateDeviceState',
                                type: 'get',
                                data: { deviceId: deviceId },
                                success: function (data) {
                                    if (data == "true") {
                                        $(btn).parent().parent().parent().find("[name=state]").text("空闲非管控设备");
                                        $(btn).parent().parent().find("[name=state]").text("当前管控推送设备");
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
                                                        '<p id="tscontent">切换设备成功！</p>' +
                                                    '</div>'
                                        });
                                        $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
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
                                                        '<p id="tscontent">发生错误！错误信息：' + data + '</p>' +
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
                        $("[name=del]").click(function () {
                            var btn2 = $(this);
                            var deviceId = $(this).parent().parent().find("[type=hidden]").val();
                            layer.open({
                                type: 1,
                                title: false,
                                closeBtn: 0,
                                shadeClose: true,
                                skin: 'panle',
                                content: '<div class="panle">' +
                                            '<p id="tstitle">温馨提示</p>' +
                                            '<hr style="border-top: 1px solid #18afda;"/>' +
                                            '<p id="tscontent">确认删除设备吗？</p>' +
                                            '<div style="margin-bottom: 20px;"><input type="button" value="" id="qx" /><input type="button" value="" id="qr" /></div>' +
                                        '</div>'
                            });
                            $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                            $("#qr").click(function () {
                                $.ajax({
                                    url: '/API/InteractiveService.asmx/deleteDeviceInfo?deviceId=' + deviceId,
                                    type: 'get',
                                    success: function (data) {
                                        if (data == "true") {
                                            $(btn2).parent().parent().remove();
                                            layer.closeAll();
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
                                                            '<p id="tscontent">设备删除成功！</p>' +
                                                        '</div>'
                                            });
                                            $(".layui-layer-page").css({ "width": "80%", "left": "10%", "top": "30%" });
                                            window.location.href = "DeviceManagement.aspx";
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
                                                            '<p id="tscontent">发生错误！错误信息：' + data + '</p>' +
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
                            $("#qx").click(function () {
                                layer.closeAll();
                            });
                        });
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="devices">
        </div>
    </div>
    </form>
</body>
</html>
