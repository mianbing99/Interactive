<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addVideo.aspx.cs" Inherits="Web.Admin.AddAll.addVideo" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title></title>
    <link rel="stylesheet" href="../js/layer/btnCss/layui.css"/>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.11.3.js"></script>
    <script type="text/javascript" src="../js/layer/layer.js"></script>
    <script>

    </script>
    <style>
    .bantouming {background: rgba(24,153,140, 0);}
    .touming {background: rgba(255,255,255, 0);}
    </style>

</head>
<body class="touming">
    <form id="form1" runat="server">
    <div class="VideoType" runat="server" id="insertInto"  style="padding-left:20px">
            <div id="M1" runat="server" style="float:left;width:50%">
                    第一步:<a href="addVideo_模板.xls" style="color:#ff0000">下载模板</a>(如果你已经下载了,可以跳到第二步)
                    <br /><br />
                    第二步:将您要上传的数据复制到模板中,可以换名字,随意换.
                    <br /><br />
                    第三步:选择Excel
                    <asp:FileUpload ID="FileUpload1" runat="server" 
                        CssClass="layui-btn layui-btn-primary layui-btn-mini"
                         Width="180px" onchange="bt2css()"/>
                    <br />
                   <br />
                    第四步:检查Excel
                    <asp:Button ID="Button8" runat="server" Text="检查" OnClick="Button8_Click" 
                        CssClass="layui-btn layui-btn-primary layui-btn-mini"  />
                    <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
                    <br />
                    <br />
                    第五步:开始导入&nbsp
                    <asp:Button ID="Button1" runat="server" Text="上传" OnClick="Button1_Click" 
                        CssClass="layui-btn layui-btn-primary layui-btn-mini" />
                </div>


                <div id="gv1" runat="server" style="overflow: auto; overflow-x: auto; overflow-y: hidden;white-space:nowrap;width:800px; height:360px; overflow:auto;" >           
                <asp:GridView ID="GridView1" runat="server" Width="800px" Height="360px" 
                    BackColor="White" BorderColor="#999999" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Visible="false"
                    OnRowDeleting="GridView1_RowDeleting">  
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle CssClass="sectiontableentry2" BackColor="#EEEEEE" ForeColor="Black" />  
                    <AlternatingRowStyle CssClass="sectiontableentry1" BackColor="#DCDCDC" />  
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#000065" />
                    </asp:GridView> 
            </div>
     
        </div>
    </form>
    <script>
        function bt2css() {
            document.getElementById("<%=Button8.ClientID %>").removeAttribute("disabled");
            document.getElementById("<%=Button8.ClientID %>").className = "layui-btn  layui-btn-mini";
            document.getElementById("<%=Button1.ClientID %>").className = "layui-btn-disabled";
            document.getElementById("<%=Label1.ClientID %>").textContent = "";
            //Button2.CssClass = 'layui-btn  layui-btn-mini'
        }
    </script>
</body>
</html>


