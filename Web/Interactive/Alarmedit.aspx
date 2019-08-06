<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Alarmedit.aspx.cs" Inherits="Web.Interactive.Alarmedit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>闹钟编辑</title>
    <link href="/Css/mobiscroll_002.css" rel="stylesheet" />
    <link href="/Css/mobiscroll.css" rel="stylesheet" />
    <link href="/Css/mobiscroll_003.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <style type="text/css">
        html { height: 100%; width: 98%; }
        body { height: 100%; width: 100%; background-image: url(/Img/5.jpg); background-size: 105% 120%; background-repeat: no-repeat; }
        #conet { height: 600px; width: 100%; background-image: url(/Img/成员管理框@2x.png); background-size: 102% 100%; background-repeat: no-repeat; margin:0px; position:relative; left:-2% }
        #time { width: 86%; margin-top: 2%; border: 0px solid red; margin-left: 7%; padding-top: 1%;}
        .p { margin-left: 35px; }
        .text { margin-left: 35px; }
        #box { left: 0px; position: fixed; width: 100%; height: 40px; background-color: #cae8ea; color: #FFF; text-align: center; font-size: 40px; font-weight: bold; bottom: 0px; line-height: 30px; }
        #p_zidingyi { display: none; }
        .but_l {  background-image:url(/Img/删除.png);  background-size: 103% 105%; background-repeat: no-repeat; margin: 0px; padding: 0px; border-radius: 5px; border: none; width: 24%; height: 32px; margin-top:6%; position:relative; left:-33%; }
        .but_z { width: 24%; height: 32px; border-radius: 5px; background-image: url(/Img/取消.png); background-repeat: no-repeat; border: none; background-size: 103% 105%; margin-left: 10.5%; margin-top:5%; position:relative; left:-41%;}
        .but_r { width: 24%; height: 32px; border-radius: 5px; background-image: url(/Img/确定.png); background-repeat: no-repeat; border: none; background-size: 103% 105%; margin-left: 10.5%; margin-top:5%; position:relative; left:63%; }
        #Text_ClockTime { background-image: url(/Img/一周内框.png); background-size: 100% 100%; border: none; width: 128%; }
        .xialakuang { width: 128%; height: 30px; background-image: url(/Img/一周内框.png); background-size: 100% 100%; appearance: none; -moz-appearance: none; -webkit-appearance: none; border: none; border-radius: 3px; background-repeat: no-repeat; }
        .input { border: #A4E3F8 solid 1px; border-radius: 3px; width: 128%; height:30px; }
        .lab { width: 28%; text-align: justify; }
        p { width: 100%; font-size:18px; }
        .fuxuan {margin-left:3%; width: 100%;}
        .spw { width: 55%;display:inline-block; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="conet">
            <div id="time">
                <div style="margin-top:5%">
                <p style="display:inline; ">
                    <asp:Label ID="Lab_type" runat="server" Text="闹钟类型:" CssClass="lab" BorderStyle="None"></asp:Label>
                    <div class="spw">
                        <asp:DropDownList ID="Drop_type" CssClass="xialakuang" name="Repeat" runat="server">
                            <asp:ListItem Value="起床">起床</asp:ListItem>
                            <asp:ListItem Value="吃饭">吃饭</asp:ListItem>
                            <asp:ListItem Value="上学">上学</asp:ListItem>
                            <asp:ListItem Value="午休">午休</asp:ListItem>
                            <asp:ListItem Value="运动">运动</asp:ListItem>
                            <asp:ListItem Value="做作业">做作业</asp:ListItem>
                            <asp:ListItem Value="洗澡">洗澡</asp:ListItem>
                            <asp:ListItem Value="睡觉">睡觉</asp:ListItem>
                            <asp:ListItem Value="自定义">自定义</asp:ListItem>
                        </asp:DropDownList></div>
                </p>
                    </div>
                <div style="margin-top:5%">
                <p id="p_zidingyi">
                    <asp:Label ID="Lab_zidingyi" runat="server" Text="闹钟名称:" CssClass="lab" BorderStyle="None"></asp:Label>
                    <span class="spw">
                        <asp:TextBox ID="Text_zidingyi" CssClass="input" runat="server" name="Text_ClockTime" Height="26px"></asp:TextBox></span>
                </p>
                    </div>
                 <div style="margin-top:5%">
                <p>
                    <asp:Label ID="Lab_Time" runat="server" CssClass="lab" Text="提醒时间:" BorderStyle="None"></asp:Label>
                    <span class="spw">
                        <input type="text"  id="Text_ClockTime" class="input" runat="server"/>
                        <%--<asp:TextBox ID="Text_ClockTime" runat="server" BorderStyle="None" name="Text_ClockTime" Height="30px"></asp:TextBox>--%></span>
                </p>  </div>
                 <div style="margin-top:5%">
                <p>
                    <asp:Label ID="Lab_content" runat="server" CssClass="lab" Text="提示标签:" BorderStyle="None"></asp:Label>
                    <span class="spw">
                        <input type="text"  id="Text_content" style="background-color:white;" class="input" runat="server"/>
                        <%--<asp:TextBox ID="Text_content" CssClass="input" runat="server" name="Text_Content" Height="26px" MaxLength="15"></asp:TextBox>--%></span>
                </p>  </div>
                 <div style="margin-top:5%">
                <p>
                    <asp:Label ID="Lab_repater" runat="server" CssClass="lab" Text="是否重复:" BorderStyle="None"></asp:Label>
                    <span class="spw">
                        <asp:DropDownList ID="Drop_repater" CssClass="xialakuang" name="Repeat" runat="server">
                            <asp:ListItem Value="0">否</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                        </asp:DropDownList></span>
                </p>  </div>
                 <div style="margin-top:5%">
                <p>
                    <asp:Label ID="Lab_WeekDate" CssClass="lab" runat="server" Text="重复时间:"></asp:Label>
                </p>  </div>
                <p>
                    <asp:CheckBoxList ID="Chbke_Week" runat="server" class="fuxuan"  AutoPostBack="false" RepeatColumns="4" RepeatDirection="Horizontal">
                        <asp:ListItem Value="0" Text="周一" />
                        <asp:ListItem Value="1" Text="周二" />
                        <asp:ListItem Value="2" Text="周三" />
                        <asp:ListItem Value="3" Text="周四" />
                        <asp:ListItem Value="4" Text="周五" />
                        <asp:ListItem Value="5" Text="周六" />
                        <asp:ListItem Value="6" Text="周日" />
                    </asp:CheckBoxList>
                </p>  
                 <div style="margin-top:5%">
                <p>
                    <asp:Label ID="Lab_count" runat="server" CssClass="lab" Text="重复次数:" BorderStyle="None"></asp:Label>
                    <span class="spw">
                        <asp:DropDownList ID="Drop_Frequency" CssClass="xialakuang" runat="server">
                            <asp:ListItem Value="1">1</asp:ListItem>
                            <asp:ListItem Value="2">2</asp:ListItem>
                            <asp:ListItem Value="3">3</asp:ListItem>
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="15">15</asp:ListItem>
                        </asp:DropDownList></span>
                </p>  </div>
                 <div style="margin-top:5%">
                <p>
                    <asp:Label ID="Lab_jiange" runat="server" CssClass="lab" Text="提醒间隔:" BorderStyle="None"></asp:Label>
                    <span class="spw">
                        <asp:DropDownList ID="Drop_Interval" CssClass="xialakuang" runat="server">
                            <asp:ListItem Value="3">3</asp:ListItem>
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                        </asp:DropDownList></span>
                </p>
                     <p>
                         <asp:Button ID="But_xiugai" CssClass="but_r" runat="server" OnClick="But_xiugai_Click" />
                    <asp:Button ID="But_delete" CssClass="but_l" runat="server"  OnClick="But_delete_Click" />
                <asp:Button ID="But_quxiao" CssClass="but_z" runat="server" OnClick="But_quxiao_Click" />
                
                         </p>
            </div>
    </form>
</body>
<script src="/Js/jquery-1.11.0.min.js"></script>
<script src="/Js/mobiscroll_002.js"></script>
<script src="/Js/mobiscroll_004.js"></script>
<script src="/Js/mobiscroll.js"></script>
<script src="/Js/mobiscroll_003.js"></script>
<script src="/Js/mobiscroll_005.js"></script>
<script type="text/javascript">
    var sta = "<%=stat %>";
    if (sta == "true") {
        $("#But_delete").css("display", "none");
        $("#But_quxiao").css("width", "30%");
        $("#But_quxiao").css("left", "-40 %"); 
        $("#But_xiugai").css("width", "30%");
        $("#But_xiugai").css("left", "45%");
    } else {
        $("#But_delete").css("display", "inline");
        $("#But_quxiao").css("width", "24%");
        $("#But_quxiao").css("left", "-35%");
        $("#But_xiugai").css("width", "24%");
        $("#But_xiugai").css("left", "63%");
    }
    $("#But_xiugai").click(function () {
        if ($("#Text_ClockTime").val() == null || $("#Text_ClockTime").val() == "") {
            alert("请选择时间！");
            return false;
        }
    })
    $(function () {
        window.alert = function (name) {
            var iframe = document.createElement("IFRAME");
            iframe.style.display = "none";
            iframe.setAttribute("src", 'data:text/plain,');
            document.documentElement.appendChild(iframe);
            window.frames[0].window.alert(name);
            iframe.parentNode.removeChild(iframe);
        };
        var ua = navigator.userAgent.toLowerCase();
        if (/iphone|ipad|ipod/.test(ua)) {
            $("#Text_zidingyi").css("width", "128%");
            $("#Text_ClockTime").css("width", "128%");
            $("#Text_content").css("width", "128%");
            $("#Text_content").css("height", "28px"); 
            $("#Text_zidingyi").css("width", "128%");
            $("#Text_zidingyi").css("font-size", "15px");
            $("#conet").css("height", "650px");
            $("#Drop_type").css("font-size", "15px");
            $("#Drop_Interval").css("font-size", "15px");
            $("#Drop_Frequency").css("font-size", "15px");
            $("#Drop_repater").css("font-size", "15px");
            $("#Text_content").css("font-size", "15px");
            $("#Text_ClockTime").css("font-size", "15px");
            //$("p").css("font-size"," px");
                //$("#conet").css("background-size", "95% 100%");
        } 
        if ($("#Drop_type").val() == "自定义") {
            $("#p_zidingyi").css('display', 'inline');
        }
        var currYear = (new Date()).getFullYear();
        var opt = {};
        opt.date = { preset: 'date' };
        opt.datetime = { preset: 'datetime' };
        opt.time = { preset: 'time' };
        opt.default = {
            theme: 'android-ics light', //皮肤样式 width: 100%; 
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
        $("#Text_ClockTime").mobiscroll(optTime).time(optTime);
    });
    $("#Drop_type").change(function () {
        if ($(this).val() == "自定义") {
            $("#p_zidingyi").css('display', 'inline');
        }
        else {
            $("#p_zidingyi").css('display', 'none');
            $("#Text_zidingyi").text("");
        }
    })
</script>
</html>
