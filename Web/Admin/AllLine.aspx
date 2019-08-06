<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllLine.aspx.cs" Inherits="Web.Admin.AllLine" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <link href="js/layer/btnCss/layui.css" rel="stylesheet"/>
    </head>
    <body><form runat="server">
        <div style="text-align:center;margin-bottom:10px">
            <input id="Button1" type="button" value="点击量走势" class="layui-btn layui-btn-primary layui-btn-radius" />
            <input id="Button3" type="button" value="每日用户人数走势" class="layui-btn layui-btn-primary layui-btn-radius" />
            <input id="Button2" type="button" value="活跃时间" class="layui-btn layui-btn-primary layui-btn-radius" />
            <input id="Button4" type="button" value="视频分类占比" class="layui-btn layui-btn-primary layui-btn-radius" />
            <input id="Button5" type="button" value="类型汇总" class="layui-btn layui-btn-primary layui-btn-radius" />
        </div>
    <iframe id="iframe1" onload="closelayer()"  frameborder="no" allowtransparency="0" style="width:100%;height:550px"></iframe>
        </form>
          <script type="text/javascript" src="js/jquery-1.11.3.js"></script>
    <script type="text/javascript" src="js/layer/layer.js"></script>
    <script charset="GBK" src="chartsJs/jquery.js"></script>
    <script charset="GBK" src="chartsJs/highcharts.js"></script>
    <script charset="GBK" src="chartsJs/exporting.js"></script>
<script type="text/javascript">
    var lm;
    window.onload = function () {
        if(myBrowser()!="IE")
        lm = layer.load(1, {
            shade: [0.1, '#fff'] //0.1透明度的白色背景
        });
        //iframe1.src = "chartsTJ/Line1.aspx";
        document.getElementById("iframe1").src = "chartsTJ/Line1.aspx";
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
            //iframe1.src = "chartsTJ/Line1.aspx";
            document.getElementById("iframe1").src = "chartsTJ/Line1.aspx";
        });
        $("#Button2").click(function () {
            if (myBrowser() != "IE")
            lm = layer.load(1, {
                shade: [0.1, '#fff'] //0.1透明度的白色背景
            });
            //iframe1.src = "chartsTJ/ActiveTime.aspx";
            document.getElementById("iframe1").src = "chartsTJ/ActiveTime.aspx";
        })
        $("#Button3").click(function () {
            iframe1.src = "chartsTJ/Line3.aspx";
        })
        $("#Button4").click(function () {
            if (myBrowser() != "IE")
            lm = layer.load(1, {
                shade: [0.1, '#fff'] //0.1透明度的白色背景
            });
            //iframe1.src = "chartsTJ/BingType.aspx";
            document.getElementById("iframe1").src = "chartsTJ/BingType.aspx";
        })
        $("#Button5").click(function () {
            if (myBrowser() != "IE")
            lm = layer.load(1, {
                shade: [0.1, '#fff'] //0.1透明度的白色背景
            });
            //iframe1.src = "chartsTJ/UseMax.aspx";
            document.getElementById("iframe1").src = "chartsTJ/UseMax.aspx";
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

  
