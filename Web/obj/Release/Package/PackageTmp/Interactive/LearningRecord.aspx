<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LearningRecord.aspx.cs" Inherits="Web.Interactive.LearningRecord" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>使用轨迹</title>
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <style type="text/css">
        body { background-color: #efefef; background: url('../Img/50.jpg'); background-repeat: repeat-y; background-size: 100% 100%; }
        #TabMain { width: 100%; height: 500px; margin: 10px 0; border-radius: 10px; }
        .tabItemContainer { width: 20%; height: 500px; float: left; }
        .tabBodyContainer { width: 100%; height: 550px; float: left; background-color: #fff; border: 1px solid #ccc; -webkit-border-radius: 0 5px 5px 0; -moz-border-radius: 0 5px 5px 0; border-radius: 10px;}
        .tabItemContainer > li { list-style: none; text-align: center; }
        .tabItemContainer > li > a { float: left; width: 100%; padding: 30px 0 30px 0; font: 15px "微软雅黑", Arial, Helvetica, sans-serif; color: #808080; cursor: pointer; text-decoration: none; border: 1px solid transparent; }
        .tabItemCurrent { background-color: #fff;  border: 1px solid #ccc !important; border-right: 1px solid #fff !important; position: relative; -webkit-border-radius: 5px 0 0 5px; -moz-border-radius: 5px 0 0 5px; border-radius: 5px 0 0 5px; }
        .tabItemContainer > li > a:hover { color: #333; }
        .tabBodyItem { position: absolute; width: 95.8%; height: 500px; display: none;  border-radius: 15px;}
        .tabBodyItem > p { font: 13px "微软雅黑", Arial, Helvetica, sans-serif; text-align: center; margin-top: 30px; }
        .tabBodyItem > p > a { text-decoration: none; color: #0F3; }
        .tabBodyCurrent { display: block; }
        .table { width: 100%; padding: 0; margin: 0; float:left;}
        th { font: bold 12px "Trebuchet MS", Verdana, Arial, Helvetica, sans-serif; color: #4f6b72; border-right: 1px solid #C1DAD7; border-bottom: 1px solid #C1DAD7; border-top: 1px solid #C1DAD7; letter-spacing: 2px; text-transform: uppercase;
         text-align:center; padding: 6px 6px 6px 12px; background: #CAE8EA no-repeat; border-radius:6px; }
        td { border-right: 1px solid #C1DAD7; border-bottom: 1px solid #C1DAD7; background: #fff; font-size: 14px; padding: 6px 6px 6px 12px; color: #4f6b72; text-align:center; }
        td.alt { background: #F5FAFA; color: #797268; }
        th.spec, td.spec { border-left: 1px solid #C1DAD7; border-radius:0px; }
        html > body td { font-size: 14px; }
        tr.select th, tr.select td { background-color: #CAE8EA; color: #797268; }
        #mains{ width: 100%; font: 18px "黑体"; height: 50px; background-color: white; border: 1px solid #6986AA; border-radius: 10px; opacity: 0.9; }
        #tabs{ width: 70%; height: 40px; float: right; margin-top: 8px; margin-right: 5px; border-radius: 6px;border:none; font: 18px "黑体";
        appearance:none; -moz-appearance:none; -webkit-appearance:none;background-image:url(/Img/一周内框.png); background-size:100% 100%;
        }
        .but { width: 18%;border-radius:3px; height:30px; border:none;}
        .dingwei { position:absolute;  left:0px;top:113%; width:101.5%}
        .spandiv { border-radius: 5px; width:16%; position:absolute; left:40%;top:0%; border:#55C0F8 solid 1px; text-align:center; height:28px;line-height:27px; background-color:#0358EF;}
        .th_r { border-radius:0px; border-top-right-radius:7px; font-size:15px; }
        #hqys { display:none; border-radius: 5px; width:100%; position:absolute; left:-1%; border:#55C0F8 solid 1px; text-align:center; height:26px;line-height:27px; background-color:#0358EF;}
    </style>
</head>
<body>
    <form id="form1" method="post" runat="server">
        <div>
            <div id="mains"><label style="float: left; margin-top: 15px; margin-left: 15px;">周期选择</label>
            <select id="tabs" runat="server"  >
                <option value="1">今天记录</option>
                <option value="2">昨天记录</option>
                <option value="3">最近一周</option>
                <option value="4">一月以内</option>
                <option value="5">更久...</option>
            </select></div>
        <div id="TabMain">
            <div class="tabBodyContainer">
                <asp:Label ID="Lab_ID" runat="server" Text=""></asp:Label>
                <%--今天--%>
                <div class="tabBodyItem tabBodyCurrent" id="Today">
                    <div style=" height:600px; ">
                    <table class="table" id="tableOne" cellspacing="0" summary="The technical specifications of the Apple PowerMac G5 series">
                        <thead>
                            <tr>
                                <th class="spec" style="font-size:16px; border-top-left-radius:7px; ">使用功能</th>
                                <th class="th_r">使用时间</th>
                            </tr>
                        </thead>
                        <asp:Repeater ID="Rep_today" runat="server">
                            <ItemTemplate>
                                <tr style="height:34.3px" >
                                    <td class="spec" ><%#Eval("appName")%></td>
                                    <td><%#Eval("createTime")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                        </div>
                    <br />
                    <div class="dingwei">
                    <a href="LearningRecord.aspx?yeshu1=1" id="First"><input type="button" id="But_First" style="background-image:url(/Img/首页.png); background-size:105% 105%" class="but" /></a><input type="button" style="margin-left:1.5%;background-image:url(/Img/上一页.png); background-size:105% 105%;"  class="but"  id="Up" value="" />
                     <div class="spandiv"><input type="text" value="<%=ys1%>" id="hqys"  onkeyup='this.value=this.value.replace(/\D/gi,"")' /><span id="spanPageNum"><%=ys1 %></span>/<span id="spanTotalPage"><%=cont1 %></span></div>
                 <input type="button" id="Down" style="margin-left:20%;background-image:url(/Img/下一页.png); background-size:105% 105%"  class="but"  value="" /><input type="button" class="but" id="Last" style="margin-left:1%;background-image:url(/Img/末页.png); background-size:105% 105%"  value="" /></div>
                </div>
                
            </div>
        </div>
        </div>
    </form>
</body>
<script src="http://code.jquery.com/jquery-1.8.3.min.js"></script>

    <script type="text/javascript">
        var AFirst = $("#First");
        var AUp = $("#Up");
        var ADown = $("#Down");
        var ALast = $("#Last");
        var sta = parseInt("<%=sta%>"); 
        var ys = parseInt("<%=ys1%>");
        //$(".spandiv").click(function () {
        //    $("#hqys").css("display", "inline");
        //    $("#spanPageNum").css("display", "none");
        //    $("#spanTotalPage").css("display", "none");
        //})
        //$("#hqys").blur(function () {
        //    $("#hqys").css("display", "none");
        //    $("#spanPageNum").css("display", "inline");
        //    $("#spanTotalPage").css("display", "inline");
        //    var hqys = $(this).val();
        //    var con = parseInt($("#spanTotalPage").text());
        //    if (hqys<1) {
        //        window.location.href = "StudyStation.aspx?yeshu" + sta + "=1";
        //    }
        //    else if (hqys > con)
        //    {
        //                hqys = con; 
        //    }
        //        window.location.href = "StudyStation.aspx?yeshu"+sta+"="+hqys+"";
        //})
        $(document).ready(function () {
            if (sta == 2) {
                 $("#tabs").val(2);
                 var cont2 = "<%=cont2%>";
                 $("#spanTotalPage").html(cont2);
                 $("#First").attr('href', 'LearningRecord.aspx?yeshu2=1');
            } else if (sta == 3) {
                $("#tabs").val(3);
                var cont3 = "<%=cont3%>";
                $("#spanTotalPage").html(cont3);
                $("#First").attr('href', 'LearningRecord.aspx?yeshu3=1');
            } else if (sta == 4) {
                $("#tabs").val(4);
                var cont4 = "<%=cont4%>";
                $("#spanTotalPage").html(cont4);
                $("#First").attr('href', 'LearningRecord.aspx?yeshu4=1');
            } else if (sta == 5) {
                $("#tabs").val(5);
                var cont5 = "<%=cont5%>";
                $("#spanTotalPage").html(cont5);
                $("#First").attr('href', 'LearningRecord.aspx?yeshu5=1');
            } else {
                $("#tabs").val(1);
            }
        });
        // 上一页翻页
        AUp.click(function () {
            if (sta == "2") {
                var cont = parseInt("<%=cont2%>");
                if (ys != 1) {
                    ys -= 1;
                    if (ys < 1) {
                        ys = 1;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu2=" + ys + "";
                }
              
            } else if (sta == "3") {
                var cont = parseInt("<%=cont3%>");
                if (ys != 1) {
                    ys -= 1;
                    if (ys < 1) {
                        ys = 1;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu3=" + ys + "";
                }
                
            } else if (sta == "4") {
                var cont = parseInt("<%=cont4%>");
                if (ys != 1) {
                    ys -= 1;
                    if (ys < 1) {
                        ys = 1;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu4=" + ys + "";
                }
              
            } else if (sta == "5") {
                var cont = parseInt("<%=cont5%>");
                if (ys != 1) {
                    ys -= 1;
                    if (ys < 1) {
                        ys = 1;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu5=" + ys + "";
                }

            } else {
                var cont = parseInt("<%=cont1%>");
                ys -= 1;
                if (ys != 1) {
                    ys -= 1;
                    if (ys < 1) {
                        ys = 1;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu1=" + ys + "";
                }
              
            }
        });
        // 下一页翻页
        ADown.click(function () {
            if (sta == "2") {
                var cont = parseInt("<%=cont2%>");
                if (ys != cont) {
                    ys += 1;
                    if (ys > cont) {
                        ys = cont;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu2=" + ys + "";
                }
               
            } else if (sta == "3") {
                var cont = parseInt("<%=cont3%>");
                if (ys != cont) {
                    ys += 1;
                    if (ys > cont) {
                        ys = cont;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu3=" + ys + "";
                }
               
            } else if (sta == "4") {
                var cont = parseInt("<%=cont4%>");
                if (ys != cont) {
                    ys += 1;
                    if (ys > cont) {
                        ys = cont;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu4=" + ys + "";
                }
              
            } else if (sta == "5") {
                var cont = parseInt("<%=cont5%>");
                if (ys != cont) {
                    ys += 1;
                    if (ys > cont) {
                        ys = cont;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu5=" + ys + "";
                }
                
            } else {
                var cont = parseInt("<%=cont1%>");
                if (ys != cont) {
                    ys += 1;
                    if (ys > cont) {
                        ys = cont;
                    }
                    window.location.href = "LearningRecord.aspx?yeshu1=" + ys + "";
                }
               
            }
        })
        ALast.click(function () {
            if (sta == "2") {
                window.location.href = "LearningRecord.aspx?yeshu2=<%=cont2 %>"; 
            } else if (sta == "3") {
                window.location.href = "LearningRecord.aspx?yeshu3=<%=cont3 %>"; 
            } else if (sta == "4") {
                window.location.href = "LearningRecord.aspx?yeshu4=<%=cont4 %> "; 
            } else if (sta == "5") {
                window.location.href = "LearningRecord.aspx?yeshu5=<%=cont5 %>"; 
            } else {
                window.location.href = "LearningRecord.aspx?yeshu1=<%=cont1 %>"; 
            }
        })
        $("#tabs").change(function () {
            var state = $(this).val();
            if (state == 2) {
                window.location.href = "LearningRecord.aspx?yeshu2=1";
            } else if (state == 3) {
                window.location.href = "LearningRecord.aspx?yeshu3=1";
            }
            else if (state == 4) {
                window.location.href = "LearningRecord.aspx?yeshu4=1";
            }
            else if (state == 5) {
                window.location.href = "LearningRecord.aspx?yeshu5=1";
            } else {
                window.location.href = "LearningRecord.aspx?yeshu1=1";
            }
        })
        
    </script>
</html>
