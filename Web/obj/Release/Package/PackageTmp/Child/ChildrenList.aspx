<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChildrenList.aspx.cs" Inherits="Web.Child.ChildrenList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" />
    <title>资源列表</title>
    <link href="../Css/style.css" rel="stylesheet" />
    <link href="../Css/ChildThree.css" rel="stylesheet" />
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.css" />
    <style type="text/css">
    form{min-height: 667px;}
        #videotype .ui-btn-inner { padding: 10px; }
        #videotype .ui-btn-inner img { display: block; width: 100%; height: 100%; border: none; }
        html { height: 100%; width: 100%; }
        body { height: 100%; width: 100%; background-image: url(/Img/50.jpg); background-size: cover; background-repeat: repeat-y; background-attachment: fixed; }
        #OneType ul li { float: left; }
        .item{width: 100%; background-image: url('../Img/框@2x.png'); background-repeat: no-repeat; background-size: 100% 100%; float: left;padding-top: 10px; padding-bottom: 5px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="box">
            <div id="nav" style="position: fixed">
                <!-- 导航栏 -->
                <ul>
                    <li><a href="ChildIndex.aspx" target="_blank">0—3岁</a></li>
                    <li class="navclass"><a href="ChildrenPage.aspx" target="_blank">4—6岁</a></li>
                    <li><a href="VidelListType.aspx?q=-1" target="_blank">最新资源</a></li>
                </ul>
            </div>
        </div>
        <div id="listen">
            <asp:Repeater ID="Rep_child" runat="server">
                <ItemTemplate>
                    <dl style="border: 0px solid red;">
                        <dt>
                            <div class="item"><a href="#" val="<%#Eval("Id") %>">
                                <img src="<%#Eval("Cover") %>" width="87%" />
                            </a></div>
                            <%--<a href="Children.aspx?id=<%#Eval("Id") %>">
                                <img src="<%#Eval("Cover") %>" width="93%" />
                            </a>--%>
                        </dt>
                    </dl>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
<script src="http://code.jquery.com/jquery-1.8.3.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#listen a").click(function () {
            var id = $(this).attr("val");
            //二级页面进入三级页面
            if (id == 63 || id == 56 || id == 55 || id == 57 || id == 340 || id == 62 || id == 363 || id == 364 || id == 365) {//故事天地
                window.location.href = "Children.aspx?id="+id;
            }
            
            //二级页面直接进入列表页面
            if (id == 99) {//十万个为什么
                window.location.href = "ChildrenListPage.aspx?q=99";
            }//智力开发
            if (id == 346) {
                window.location.href = "ChildrenListPage.aspx?q=346";
            }//幼儿健康
            if (id == 102) {
                window.location.href = "ChildrenListPage.aspx?q=102";
            }//急救超人
            if (id == 103) {
                window.location.href = "ChildrenListPage.aspx?q=103";
            }//健康特攻队
            if (id == 104) {
                window.location.href = "ChildrenListPage.aspx?q=104";
            }//布奇奇乐园
            if (id == 105) {
                window.location.href = "ChildrenListPage.aspx?q=105";
            }//火星娃健康成长
            if (id == 106) {
                window.location.href = "ChildrenListPage.aspx?q=106";
            }//变形警车伯利
            if (id == 308) {
                window.location.href = "ChildrenListPage.aspx?q=308";
            }
            //好习惯
            if (id == 397) {
                window.location.href = "ChildrenListPage.aspx?q=397";
            }//安全宝典
            if (id == 398) {
                window.location.href = "ChildrenListPage.aspx?q=398";
            }
            if (id == 399) {
                window.location.href = "ChildrenListPage.aspx?q=399";
            }
            if (id == 400) {
                window.location.href = "ChildrenListPage.aspx?q=400";
            }
            if (id == 401) {
                window.location.href = "ChildrenListPage.aspx?q=401";
            }
            if (id == 402) {
                window.location.href = "ChildrenListPage.aspx?q=402";
            }
            if (id == 403) {
                window.location.href = "ChildrenListPage.aspx?q=403";
            }
            if (id == 404) {
                window.location.href = "ChildrenListPage.aspx?q=404";
            }
            if (id == 476) {
                window.location.href = "ChildrenList.aspx?id=476";
            }
            if (id == 405) {
                window.location.href = "ChildrenListPage.aspx?q=405";
            }
            if (id == 406) {
                window.location.href = "ChildrenListPage.aspx?q=406";
            }
            if (id == 484) {
                window.location.href = "ChildrenListPage.aspx?q=484";
            }
            if (id == 380) {
                window.location.href = "ChildrenListPage.aspx?q=380";
            }
            if (id == 381) {
                window.location.href = "ChildrenListPage.aspx?q=381";
            }
            if (id == 382) {
                window.location.href = "ChildrenListPage.aspx?q=382";
            }
            if (id == 478) {
                window.location.href = "ChildrenListPage.aspx?q=478";
            }
            if (id == 479) {
                window.location.href = "ChildrenListPage.aspx?q=479";
            }
            if (id == 480) {
                window.location.href = "ChildrenListPage.aspx?q=480";
            }
            if (id == 481) {
                window.location.href = "ChildrenListPage.aspx?q=481";
            }
            if (id == 482) {
                window.location.href = "ChildrenListPage.aspx?q=482";
            }
            if (id == 483) {
                window.location.href = "ChildrenListPage.aspx?q=483";
            }
        });
    });
</script>
</html>
