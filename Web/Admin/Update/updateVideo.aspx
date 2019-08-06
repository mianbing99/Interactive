<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="updateVideo.aspx.cs" Inherits="Web.Admin.Update.updateVideo" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title></title>
    <script type="text/javascript" src="js/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="js/layer/layer.js"></script>
    <script>

    </script>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <style>
        table tr { 
        valign="top"
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="createbutton" runat="server" Text="批量创建按钮" 
            onclick="createbutton_Click" Enabled="False" />


    <asp:Table ID="HolderTable"  runat="server" Height="86px" Width="501px" CssClass="mgtop">
        
    </asp:Table>


    <div class="VideoType" runat="server" id="insertInto" >
            <div>
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <%--<asp:Button ID="Button7" runat="server" onclick="Button7_Click" Text="导入" style="height: 21px" />--%>
                <asp:Button ID="Button8" runat="server" Text="检查" OnClick="Button8_Click"  />
                <asp:Button ID="Button1" runat="server" Text="上传" OnClick="Button1_Click" />
                <a href="Sheet1.xls" >下载模板</a>    
                
                <div id="gv1"  style="overflow: auto; overflow-x: auto; overflow-y: hidden;white-space:nowrap;width:auto; height:380px; overflow:auto;" >           
                <asp:GridView ID="GridView1" runat="server" Width="500px" Height="350px" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">  
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
        </div>
    </form>
</body>
</html>


