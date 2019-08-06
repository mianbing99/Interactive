<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Children.aspx.cs" Inherits="Web.Child.Children" %>

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
        #content .ui-btn-inner { padding: 10px; }
        #content .ui-btn-inner img { display: block; width: 100%; height: 100%; border: none; }
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
            <asp:Repeater ID="Rep_children" runat="server">
                <ItemTemplate>
                    <dl style="border: 0px solid red;">
                        <dt>
                            <div class="item"><a href="#" val="<%#Eval("Id") %>">
                                <img src="<%#Eval("Cover") %>" width="87%" />
                            </a></div>
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
            //三级页面直接进入页面 sadsadsad
            if (id == "492") {
                window.location.href = "Children.aspx?id=492";
            } else {
                window.location.href = "ChildrenListPage.aspx?q=" + id;
            }
            
        });
        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return (r[2]); return null;
        }
    });
</script>
</html>
