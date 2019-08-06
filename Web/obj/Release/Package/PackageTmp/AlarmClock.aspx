<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmClock.aspx.cs" Inherits="Web.AlarmClock" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>习惯养成</title>
    <link href="Css/mobiscroll_002.css" rel="stylesheet" />
    <link href="Css/mobiscroll.css" rel="stylesheet" />
    <link href="Css/mobiscroll_003.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"/>
     <style type="text/css">
    #form1{min-height:667px;}

   *{padding:0;margin:0}

   html{width:100%;height:800px;}
   body { height:100%;width:100%; background-image: url(/Img/5.jpg );
        background-size: 100% 100%; background-repeat: no-repeat; 
        background-attachment: fixed;}

   .zong{width:100%;height:600px;margin-bottom:30px;margin-top:5%;}

   .item{width:33%; background-image: url('../Img/框@2x.png');
       background-repeat: no-repeat;background-size: 100% 100%;
       float: left;padding-top: 2%; padding-bottom: 2%;
       vertical-align:middle; height:25%;
   }
   .picture{padding-left:12%;}
   .word{width:77.8%;  
       font: 13px "微软雅黑",800;background-color:#9ed3fb;
      border-top-left-radius: 4px;border-top-right-radius: 4px; border-bottom-left-radius: 4px;
      border-bottom-right-radius: 4px;
        text-align:center;margin-left:12%; height:15%; line-height:24px;}

       @media screen and (min-width:768px){
            .word{font-size:30px;}
        }
         .idclass_k {  width:34%;  font-size:13px; border:none; margin-top:5%;
           height:13%;margin-left:9.5%; border-radius:3px;
          background-image:url(Img/开启@2x.png); background-size:100% 105%; background-repeat: no-repeat;
          
         }
         .idclass_g {  background-repeat: no-repeat;
         width:34%;  font-size:13px; border:none; margin-top:5%;
           height:13%;margin-left:6.5%; border-radius:3px;
          background-image:url(Img/关闭@2x.png); background-size:100% 105%;
         }
         .staclass {
          width:34%; 
           font-size:13px; border:none; margin-top:5%;
           height:13%;margin-left:12%; border-radius:3px;
          background-image:url(Img/编辑@2x.png); background-size:100% 105%;
         }
     </style>
   <%-- //dsdsadsadad--%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="zong">
            <asp:Repeater ID="RpList" runat="server">
                <ItemTemplate>  
                    <div class="item">
                <div class="picture"align:"center"><a href='Alarmedit.aspx?id=<%#Eval("id") %>'>  <%# getimg(Eval("AlarmType").ToString()) %>  </a></div>
                <div class="word"><%#Eval("AlarmType") %></div>
                    <input type="button"   value="" name='<%#Eval("Id") %>'  class="staclass" /><input type="button" class="idclass_k" name="<%#Eval("Id") %>" <%#getrp(Convert.ToInt32(Eval("state"))) %> value="" />
                        <input type="button" <%#getrp_g(Convert.ToInt32(Eval("state"))) %> class="idclass_g" name="<%#Eval("Id") %>" value="" />
            </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="item" style="height:25%;">
                 <div class="picture" ><a href="Alarmedit.aspx?tj=1"><img src='Img/添加@2x.png' width="87%"   style="margin-top:25%"/></a></div>
               
            </div>
             
        </div>
    </form>
</body>
<script src="http://code.jquery.com/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var ua = navigator.userAgent.toLowerCase();
            if (/iphone|ipad|ipod/.test(ua)) {
                $("div.item").css("height", "28%");
                $("div.word").css("font-size", "16px");
                $("html").css("height","900px");
            }
            $(".staclass").click(function () {
                var id = $(this).attr("name");
                if (id != null) {
                    $(location).attr("href", "Alarmedit.aspx?id=" + id + "");
                } else {
                    return;
                }
            });
            $(".idclass_k").click(function () {
                var id = $(this).attr("name");
                $.ajax({
                    url: '/API/WebService.asmx/SetState_k',
                    type: 'post',
                    data: {  "id": id },
                    success: function (data) {
                    }
                });
                $(this).css('display', 'none');
                $(this).next('input').css('display', 'inline');
            })
            $(".idclass_g").click(function () {
                var thi = $(this);
                var id = $(this).attr("name");
                $.ajax({
                    url: '/API/WebService.asmx/SetState_g',
                    type: 'post',
                    data: { "id": id },
                    success: function (data) {
                    }
                });
                $(this).css('display', 'none');
                $(this).prev('input').css('display', 'inline');
            })
        });
    </script>
    
</html>
