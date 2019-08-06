<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddResource.aspx.cs" Inherits="Web.Admin.AddResource" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
        <link href="js/layer/btnCss/layui.css" rel="stylesheet"/>

    </head>
    <body><form runat="server">
        <div style="text-align:center;margin-bottom:10px">
            <input id="Button1" type="button" value="添加类型" class="layui-btn layui-btn-primary layui-btn-radius" />
            <input id="Button2" type="button" value="添加视频" class="layui-btn layui-btn-primary layui-btn-radius" />
            <input id="Button3" type="button" value="添加信道" class="layui-btn layui-btn-primary layui-btn-radius" />
        </div>
        <iframe id="iframe1" onload="closelayer()"  style="width:100%;height:550px"></iframe>
        </form>
        <script type="text/javascript" src="js/jquery-1.11.3.js"></script>
        <script type="text/javascript" src="js/layer/layer.js"></script>
        <script type="text/javascript">
        var lm;
        window.onload = function () {
            if(myBrowser()!="IE")
            lm = layer.load(1, {
                shade: [0.1, '#fff'] //0.1透明度的白色背景
            });
            //iframe1.src = "AddAll/addType.aspx";
            document.getElementById("iframe1").src = "AddAll/addType.aspx";
        }
        function closelayer() {
            layer.close(lm);
        }

    $(function () {
        $("#Button1").click(function () {
            if (myBrowser() != "IE")
            lm = layer.load(1, {
                shade: [0.1, '#fff'] //0.1透明度的白色背景
            });
            //iframe1.src = "AddAll/addType.aspx";
            document.getElementById("iframe1").src = "AddAll/addType.aspx";
        });
        $("#Button2").click(function () {
            if (myBrowser() != "IE")
            lm = layer.load(1, {
                shade: [0.1, '#fff'] //0.1透明度的白色背景
            });
            //iframe1.src = "AddAll/addVideo.aspx";
            document.getElementById("iframe1").src = "AddAll/addVideo.aspx";
        })
        $("#Button3").click(function () {
            if (myBrowser() != "IE")
            lm = layer.load(1, {
                shade: [0.1, '#fff'] //0.1透明度的白色背景
            });
            //iframe1.src = "AddAll/addSource.aspx";
            document.getElementById("iframe1").src = "AddAll/addSource.aspx";
        })
    });
    function myBrowser() {
        var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
        var isOpera = userAgent.indexOf("Opera") > -1;
        if (isOpera) {
            return "Opera"
        }; //判断是否Opera浏览器
        if (userAgent.indexOf("Firefox") > -1) {
            return "FF";
        } //判断是否Firefox浏览器
        if (userAgent.indexOf("Chrome") > -1) {
            return "Chrome";
        }
        if (userAgent.indexOf("Safari") > -1) {
            return "Safari";
        } //判断是否Safari浏览器
        if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
            return "IE";
        }; //判断是否IE浏览器
    }
    </script>
    </body>
    </html>
