<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Web.Admin.Home" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>e家亲后台</title>
    <meta name="renderer" content="ie-comp" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="SHORTCUT ICON" href="img/favicon.ico">
    <link href="js/layer/btnCss/layui.css" rel="stylesheet"/>
    <link href="/Admin/css/base.css" rel="stylesheet" />
    <link href="/Js/font/font.css" rel="stylesheet" />

        <link href="css/home/bootstrap.min.css" rel="stylesheet">
        <link href="css/home/style.css" rel="stylesheet">
        <link href="css/home/icons.css" rel="stylesheet">
    <style>
        .none {display:none }
        .show {display:block }
        .white { color:white}
        .oldvschilddiv {float: left;width: 40%;}
        .oldvschildlab {float: left;
        font-size:26px;
                        text-align: center;
                        height: 50px;
                        line-height: 50px;
                        overflow: hidden;}
        .tworowlabel {  color:black;font-size:20px;}

    </style>
    </head>
    <body>
        <form runat="server">
    <div style="padding: 10px;">
    <div>
    <%--            <div class="media" id="top-menu">
                    <div class="pull-left tm-icon">
                        <a data-drawer="messages" class="drawer-toggle" href="">
                            <i class="sa-top-message"></i>
                            <i class="n-count animated white">5</i>
                            <span>客户反馈</span>
                        </a>
                    </div>
                    <div class="pull-left tm-icon">
                        <a data-drawer="notifications" class="drawer-toggle" href="">
                            <i class="sa-top-updates"></i>
                            <i class="n-count animated white">9</i>
                            <span>正在制作</span>
                        </a>
                    </div>

                    

                    <div id="time" class="pull-right" style="color:white">
                        <span id="hours"></span>
                        :
                        <span id="min"></span>
                        :
                        <span id="sec"></span>
                    </div>
                </div>
                <!-- Messages Drawer -->
                <div id="messages" class="tile drawer animated">
                    <div id="listview_r" class="listview narrow">
                        <div class="media">
                            <a href="">Send a New Message</a>
                            <span class="drawer-close">×</span>
                            
                        </div>

                    </div>
                </div>
                
        <div id="notifications" class="tile drawer animated">
                    <div class="listview narrow">
                        <div class="media">
                            <a href="">Send a New notifications</a>
                            <span class="drawer-close">×</span>
                            
                        </div>
                        <div class="overflow" style="height: 254px; overflow: hidden; outline: none;" tabindex="5001">
                            <div class="media">
                                <div class="pull-left">
                                    <img width="40" src="css/img/profile-pics/5.jpg" alt="">
                                </div>
                                <div class="media-body">
                                    <small class="text-muted">Nadin Jackson - 2 Hours ago</small><br>
                                    <a class="t-overflow" href="">Mauris consectetur urna nec tempor adipiscing. Proin sit amet nisi ligula. Sed eu adipiscing lectus</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>--%>
        <!--<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />-->
        </div>
    <%--回复弹出层--%>
    <div id="r_div" style="display:none;text-align:center">
        <label style="color:red;margin-left:10px;margin-right:10px;">您的发言将代表公司,请您注意言辞!</label>
        <div style="margin-left:10px;margin-right:10px;">
            <label>客户-<input type="hidden" id="kid" runat="server" />-<label id="kehuname"></label>:</label>
            <br />
            <label id="fullinfo"></label>
           
        </div>
        <textarea runat="server" id="info_i" placeholder="请输入内容" class="layui-textarea"></textarea>
        <asp:Button ID="gor" runat="server" Text="回复" OnClick="gor_Click" CssClass="layui-btn layui-btn-primary layui-btn-small" />
    </div>
    <%--点击总数+在线人数+新增用户--%>
    <div class="block-area">
            <div class="row">
                <div class="col-md-3 col-xs-6">
                    <div class="tile quick-stats">
                        <div class="oldvschilddiv"><img src="css/img/see.png" style="height:40px;margin-left:10px;margin-top: 10px;"/></div>
                        <div><label style="font-size: 20px;">昨日播放量</label></div>
                        <div class="oldvschildlab"><label id="whatchc" runat="server"><%=clickcc %></label></div>
                        
                    </div>
                </div>
                <div class="col-md-3 col-xs-6">
                    <div class="tile quick-stats">
                        <div class="oldvschilddiv"><img src="css/img/onlineUser.png" style="height:40px;margin-left:10px;margin-top: 10px;"/></div>
                        <div><label style="font-size: 20px;">昨日在线人数</label></div>
                        <div class="oldvschildlab"><label id="onlinec" runat="server"><%=onlinecc %></label></div>
                        
                    </div>
                </div>
                <div class="col-md-3 col-xs-6">
                    <div class="tile quick-stats">
                        <div class="oldvschilddiv"><img src="css/img/newUser.png" style="height:40px;margin-left:10px;margin-top: 10px;"/></div>
                         <div><label style="font-size: 20px;">昨日新增/总用户</label></div>
                        <div class="oldvschildlab"><label id="newuserc" runat="server"><lable name="addu">0</lable>/<lable name="alluser">0</lable><!--<%=addu %>/<%=alluser %>--></label></div>
                       
                    </div>
                    
                </div>
                <div class="col-md-3 col-xs-6">
                    <div class="tile quick-stats">
                        <div style="text-align:center">
                            <div style="float: left;width: 20%;"><img src="css/img/child.png" style="height:40px;margin-left:10px"/></div>
                            <div class="oldvschildlab" style="width:25%"><label>7</label></div>
                            <div class="oldvschildlab" style="width:10%"><label>:</label></div>
                            <div class="oldvschildlab" style="width:25%"><label>3</label></div>
                            <div style="float: left;width: 20%;"><img src="css/img/old.png" style="height:40px;margin-right:10px"/></div>
                            <div style="margin-left:10px"><label>幼老专区占比</label></div>
                     </div>

                    </div>
                </div>
                </div>
        </div>
            <%--MVP--%>
        <div class="block-area">
                    <div class="row">
                        <div class="col-md-3 col-xs-6">
                            <div class="tile quick-stats">
                                
                                <div  style="text-align:center">
                                    
                                    <label style="font-size:20px">视频播放次数最多</label><br/>
                                    <label style="color:darkred" id="lab_t" runat="server"></label><br/>
                                    <label style="font-size:18px"  runat="server" id="h2_titlec"></label><br/>
                                    
                                </div>
                                
                            </div>
                        </div>

                        <div class="col-md-3 col-xs-6">
                            <div class="tile quick-stats media">
                                <div  style="text-align:center">
                                    <label style="font-size:20px">类型播放次数最多</label><br/>
                                    <label  style="font-size:16px;color:darkred" id="lab_type" runat="server"></label><br/>
                                    <label style="font-size:18px"  runat="server" id="h2_typec"></label><br/>
                                    
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3 col-xs-6">
                            <div class="tile quick-stats media">

                            

                                <div  style="text-align:center">
                                    
                                    <label style="font-size:20px">地域播放次数最多</label><br/>
                                    <label  style="font-size:16px;color:darkred" id="lab_addr" runat="server"></label><br/>
                                    <label style="font-size:18px"  runat="server" id="h2_addrc"></label><br/>
                                    
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3 col-xs-6">
                            <div class="tile quick-stats media">
      
                                 <div  style="text-align:center">
                                    
                                    <label style="font-size:20px">使用率最高用户</label><br/>
                                    <label class="tworowlabel" id="lab_ip" runat="server" style="color:darkred;font-size:20px;margin-top:5px"></label>-
                                        <input id="showpwd" type="button" value="******" style="color:darkred;border:none;background-color:transparent" title="显示密码" onclick="showmaxpwd()" />
                                        <label class="tworowlabel" id="lab_ip2" runat="server" style="display:none"></label><br/>
                                        <label style="font-size:18px"  runat="server" id="h2_ipc"></label><br/>
                                    
                                </div>
                            </div>
                        </div>

                    </div>

                </div>
    </div>
            <script>
               <%-- function addhtml(result) {
                    var data = "<% =String.Join(" ", result) %>";
                    var listview_r = document.getElementById("listview_r");
                    if (listview_r) {
                        listview_r.innerHTML = data;
                    }
                }--%>
    </script>
            </form>
        <script src="js/jquery-1.11.3.js"></script>
        <script src="js/layer/layer.js"></script>
        <script src="js/home/scroll.min.js"></script> <!-- Custom Scrollbar -->
        <script src="js/jquery-session.js"></script>
        <script src="js/home/functions.js"></script>
        <script>
            //var is = false;//收否展开
            //function selectReply(index) {
            //    if (is) {
            //        document.getElementById("oneinfo" + index).style.display = "none";
            //        is = false;
            //    } else {
            //        document.getElementById("oneinfo" + index).style.display = "block";
            //        is = true;
            //    }
            //}
            <%--function showreply(str) {
                var id = str;
                var title = document.getElementById("t" + id).textContent;
                var content = document.getElementById("c" + id).textContent;
                var title1 = new Array();
                title1 = title.split('--');
                document.getElementById('<%=kid.ClientID%>').value = title1[0];
                kehuname.textContent = title1[1];
                fullinfo.textContent = content;
                layer.open({
                    type: 1,
                    title: "回复客户",
                    area: ['400px', '400px'],
                    shadeClose: true,
                    content: $("#r_div")
                });
            }--%>
            $(function () {
                if ($.session.get('addu') == null || $.session.get('addu') == '') {
                    UoloadData();
                } else {
                    $('[name=addu]').text($.session.get('addu'));
                }
                if ($.session.get('alluser') == null || $.session.get('alluser') == '') {
                    UoloadData2();
                } else {
                    $('[name=alluser]').text($.session.get('alluser'));
                }
            })
            function UoloadData() {
                $.ajax({
                    type: "Post",
                    url: "Home.aspx/getMVP2",
                    data: "",
                    datatype: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        $.session.set('addu', result.d);
                        $('[name=addu]').text(result.d);
                    }
                });
            }
            function UoloadData2() {
                $.ajax({
                    type: "Post",
                    url: "Home.aspx/getMVP3",
                    data: "",
                    datatype: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        $.session.set('alluser', result.d);
                        $('[name=alluser]').text(result.d);
                    }
                });
            }
            function showmaxpwd() {
                if (checkqx("export")) {
                    if (showpwd.value == "******")
                    {
                        showpwd.value = lab_ip2.innerText;
                    }
                    else {
                        showpwd.value ="******"
                    }
                }
            }
            function checkqx(type) {
                var state = "<%=state%>";
                var export1 = "<%=export%>";
                var add = "<%=add%>";
                var delete1 = "<%=delete%>";
                var update = "<%=update%>";
                if (state != 1) {
                    layer.alert('您的账号异常!', {
                        icon: 5, shadeClose: true, title: false,
                        skin: 'layer-ext-moon'
                    });
                    return false;
                } else {
                    var temp;
                    switch (type) {
                        case "add": temp = add; break;
                        case "delete": temp = delete1; break;
                        case "update": temp = update; break;
                        case "export": temp = export1; break;
                    }
                    if (temp != 1) {
                        layer.alert('您的账号没有权限!', {
                            icon: 5, shadeClose: true, title: false,
                            skin: 'layer-ext-moon'
                        });
                        return false;
                    } else {
                        return true;
                    }
                }
            }
    </script>

</body>
     
    </html>
       


       