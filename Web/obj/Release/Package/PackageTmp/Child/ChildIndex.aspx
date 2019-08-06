<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChildIndex.aspx.cs" Inherits="Web.Child.ChildIndex" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <title>资源列表</title>
    <link href="../Css/style.css" rel="stylesheet" />
    <link href="../Css/ChildThree.css" rel="stylesheet"/>
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.css" />
    <style type="text/css">
    form{min-height: 667px;}
        #content .ui-btn-inner { padding: 10px; }
        #content .ui-btn-inner img { display: block; width: 100%; height: 100%; border: none; }
        html { height: 100%; width: 100%; } 
        body { height: 100%; width: 100%; background-image: url(/Img/5.jpg ); background-size: 100% 100%; background-repeat: no-repeat; background-attachment: fixed; }
        #OneType ul li { float: left; }
        #boximg { height: 100px; width: 90%;margin-left:18px;border:0px solid red; }
        #boximg .ui-btn-inner img { display: block; width: 100%; height: 100%; border: none; }
        .item{width: 33%; background-image: url('../Img/框@2x.png'); background-repeat: no-repeat; background-size: 100% 100%; float: left;padding-top: 2%; padding-bottom: 2%;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="box">
            <div id="nav" style="position: fixed">
                <!-- 导航栏 -->
                <ul>
                    <li class="navclass"><a href="ChildIndex.aspx" target="_blank">0—3岁</a></li>
                    <li><a href="ChildrenPage.aspx" target="_blank">4—6岁</a></li>
                    <li><a href="VidelListType.aspx?q=-1" target="_blank">最新资源</a></li>
                </ul>
            </div>
        </div>
        <div id="content">
            <div class="item">
                <a href="Listen.aspx?id=1"><img src="../Img/Listen.png" width="87%" /></a>
            </div>
            <div class="item">
                <a href="Listen.aspx?id=2"><img src="../Img/song.png" width="87%" /></a>
            </div>
            <div class="item">
                <a href="Listen.aspx?id=3"><img src="../Img/game.png" width="87%" /></a>
            </div>
            <div class="item">
                <a href="Listen.aspx?id=4"><img src="../Img/Learn.png" width="87%" /></a>
            </div>
            <div class="item">
                <a href="Listen.aspx?id=6"><img src="../Img/yuertiandi.png" width="87%" /></a>
            </div>
        </div>
        <div id="boximg">
            
            <%--<a href="Listen.aspx?id=5">
                <img src="../Img/zaojiaoleyuan.png" width="30%" /></a>--%>
        </div>
    </form>
</body>
<script src="../Js/jquery-1.11.0.min.js"></script>
<script type="text/javascript">
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return (r[2]); return null;
    }
</script>
</html>

