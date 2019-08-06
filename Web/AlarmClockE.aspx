<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmClockE.aspx.cs" Inherits="Web.AlarmClockE" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>作息管理</title>
    <link href="Css/mobiscroll_002.css" rel="stylesheet" />
    <link href="Css/mobiscroll.css" rel="stylesheet" />
    <link href="Css/mobiscroll_003.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <style type="text/css">
        #top a {font-size:20px;text-decoration: none; color:red;} 
        #content { width: 100%; height: 100%;  text-decoration: none;}
        #addtime{ width:100%; height:32px;}
        .table { width: 100%; padding: 0; margin: 0; }
        th { font: bold 12px "Trebuchet MS", Verdana, Arial, Helvetica, sans-serif; color: #4f6b72; border-right: 1px solid #C1DAD7; border-bottom: 1px solid #C1DAD7; border-top: 1px solid #C1DAD7; letter-spacing: 2px; text-transform: uppercase; text-align: left; padding: 6px 6px 6px 12px; background: #CAE8EA no-repeat; }
        td { border-right: 1px solid #C1DAD7; border-bottom: 1px solid #C1DAD7; background: #fff; font-size: 14px; padding: 6px 6px 6px 12px; color: #4f6b72; }
        td.alt { background: #F5FAFA; color: #797268; }
        th.spec, td.spec { border-left: 1px solid #C1DAD7; }
        html > body td { font-size: 14px; }
        tr.select th, tr.select td { background-color: #CAE8EA; color: #797268; }
        #top { margin-left: 10px; margin-top: 5px; color:red; font-size:20px ;height: 40px;line-height:40px; }
        /*#box { left: 0px; position: fixed; width: 100%; height: 40px; background-color: #cae8ea; color: #FFF; text-align: center; font-size: 40px; font-weight: bold; bottom: 0px; line-height: 30px; }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="content">
            <div id="top">
                <asp:Label ID="Label1" runat="server" Text="添加闹钟"><a href="AddAlarmE.aspx" >添加闹钟</a></asp:Label>
            </div>
            <div id="box">
                <%--<asp:Button ID="Add_Clock" runat="server" Text="保  存" Width="100%" BorderStyle="None" Font-Size="20px" BackColor="#cae8ea" z-index="100" ForeColor="red" OnClick="Add_Clock_Click" />--%>
            </div>
            <div>
                <table class="table" cellspacing="0" summary="The technical specifications of the Apple PowerMac G5 series">
                    <tr>
                        <th class="spec">闹钟列表</th>
                        <th>闹钟编辑</th>
                        <th>操作删除</th>
                    </tr>
                    <asp:Repeater ID="Rep_operation" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="spec"><%#Eval("ClockTime")%></td>
                                <td class="spec"><a href="Alarmedit.aspx?id=<%#Eval("Id")%>">编辑</a></td>
                                <td><a href="AlarmClock.aspx?id=<%#Eval("Id") %>" onclick="return confirm('确认删除吗？')">删除</a></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </form>
</body>
<script src="Js/jquery-1.11.0.min.js"></script>
<script src="Js/mobiscroll_002.js"></script>
<script src="Js/mobiscroll_004.js"></script>
<script src="Js/mobiscroll.js"></script>
<script src="Js/mobiscroll_003.js"></script>
<script src="Js/mobiscroll_005.js"></script>
<script type="text/javascript">
    $(function () {
        [self.navigationItem, setHidesBackButton = TRUE, animated = NO];
        var currYear = (new Date()).getFullYear();
        var opt = {};
        opt.date = { preset: 'date' };
        opt.datetime = { preset: 'datetime' };
        opt.time = { preset: 'time' };
        opt.default = {
            theme: 'android-ics light', //皮肤样式
            display: 'modal', //显示方式
            mode: 'scroller', //日期选择模式
            dateFormat: 'yyyy-mm-dd',
            lang: 'zh',
            showNow: true,
            startYear: currYear - 10, //开始年份
            endYear: currYear + 10 //结束年份
        };
        $("#appDate").mobiscroll($.extend(opt['date'], opt['default']));
        var optDateTime = $.extend(opt['datetime'], opt['default']);
        var optTime = $.extend(opt['time'], opt['default']);
        $("#appDateTime").mobiscroll(optDateTime).datetime(optDateTime);
        $("#Text_Add").mobiscroll(optTime).time(optTime);
    });

</script>
</html>
