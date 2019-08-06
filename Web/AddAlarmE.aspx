<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAlarmE.aspx.cs" Inherits="Web.AddAlarmE" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="content">
            <p>
                <asp:Label ID="Lab_time" runat="server" Text="时 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;间："></asp:Label>
                <asp:TextBox ID="Text_Time" runat="server" Width="150px" Height="20px"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="Lab_Content" runat="server" Text="提示标签："></asp:Label>
                <asp:TextBox ID="Text_Content" runat="server" Width="150px" Height="20px" MaxLength="15"></asp:TextBox><br />
                <asp:Label ID="Lab_tishi" runat="server" Text="友情提示：限制长度15个字符以内！" ForeColor="Red"></asp:Label>
            </p>
            <p>
                <asp:Label ID="Lab_Repeat" runat="server" Text="是否重复："></asp:Label>
                <select name="Repeat" id="YesandNo" style="width: 154px; height: 27px">
                    <option value="0">是</option>
                    <option value="1">否</option>
                </select>
            </p>
            <p>
                <asp:Label ID="Lab_Date" runat="server" Text="重复时间："></asp:Label>
                <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="false" RepeatColumns="4" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0" Text="周一" />
                    <asp:ListItem Value="1" Text="周二" />
                    <asp:ListItem Value="2" Text="周三" />
                    <asp:ListItem Value="3" Text="周四" />
                    <asp:ListItem Value="4" Text="周五" />
                    <asp:ListItem Value="5" Text="周六" />
                    <asp:ListItem Value="6" Text="周日" />
                </asp:CheckBoxList>
            </p>
            <p>
                <asp:Label ID="Lab_count" runat="server" Text="重复次数："></asp:Label>
                <select name="count" id="Frequency" style="width: 154px; height: 27px">
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="5">5</option>
                    <option value="10">10</option>
                    <option value="15">15</option>
                </select>
            </p>
            <p>
                <asp:Label ID="Lab_Interval" runat="server" Text="闹钟间隔："></asp:Label>
                <select name="interval" id="Interval" style="width: 154px; height: 27px">
                    <option value="1">1</option>
                    <option value="3">3</option>
                    <option value="5">5</option>
                    <option value="10">10</option>
                    <option value="15">15</option>
                </select>
            </p>
            <br />
            <p class="button">
                <asp:Button ID="But_xiugai" runat="server" Text="确&nbsp;&nbsp;&nbsp;定" Height="40px" Width="238px" Font-Size="22px" BackColor="#3399ff" BorderStyle="None" ForeColor="White" OnClick="But_xiugai_Click" />
            </p>
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
        $("#Text_Time").mobiscroll(optTime).time(optTime);
        //$("#Text_Content").blur(function () {
        //    checkNum();
        //});
        //function checkNum() {
        //    var inputNum = parseInt($("#Text_Content").val());
        //    if (inputNum > 15) {
        //        $("#Lab_tishi").show();
        //        //$span = "<span style='color:red'>出库数量不能大于库存量</span>";
        //        //$("#Text_Content").parent().append($("#Lab_tishi"));
        //        return false;
        //    }
        //    else {
        //        $("#Lab_tishi").hide();
        //        return true;
        //    }
        //}

    });
</script>
</html>
