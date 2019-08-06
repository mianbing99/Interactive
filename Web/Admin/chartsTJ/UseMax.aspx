<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UseMax.aspx.cs" Inherits="Web.Admin.chartsTJ.UseMax" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" rel="stylesheet" href="../js/layer/btnCss/layui.css" />

</head>
<body>
    <form id="form1" runat="server">
            <div id="gv1"  style="float:left; overflow: auto; overflow-x: auto; overflow-y: hidden;white-space:nowrap;width:auto; height:420px;overflow: auto;" >
                <asp:GridView ID="GridView1" runat="server" AllowPaging="false" OnPageIndexChanging="GridView1_PageIndexChanging" 
                OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
                OnPageIndexChanged="GridView1_PageIndexChanged" CssClass=" layui-table" PageSize="20">
                </asp:GridView>
            </div>
            <div id="gv4"  style="float:left; overflow: auto; overflow-x: auto; overflow-y: hidden;white-space:nowrap;width:auto; height:420px;overflow: auto;" >
                <asp:GridView ID="GridView4" runat="server" AllowPaging="false" OnPageIndexChanging="GridView4_PageIndexChanging" 
                OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
                OnPageIndexChanged="GridView4_PageIndexChanged" CssClass=" layui-table" PageSize="20">
                </asp:GridView>
               </div>
            <div id="gv2"  style="float:left; overflow: auto; overflow-x: auto; overflow-y: hidden;white-space:nowrap;width:auto; height:420px;overflow: auto;" >
                <asp:GridView ID="GridView2" runat="server" AllowPaging="false" OnPageIndexChanging="GridView2_PageIndexChanging" 
                OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
                OnPageIndexChanged="GridView2_PageIndexChanged" CssClass=" layui-table" PageSize="20">
                </asp:GridView>
            </div>
            <div id="gv3"  style="float:left; overflow: auto; overflow-x: auto; overflow-y: hidden;white-space:nowrap;width:auto; height:420px;overflow: auto;" >
                <asp:GridView ID="GridView3" runat="server" AllowPaging="false" OnPageIndexChanging="GridView3_PageIndexChanging" 
                OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
                OnPageIndexChanged="GridView3_PageIndexChanged" CssClass=" layui-table" PageSize="20">
                </asp:GridView>
            </div>
        
   
    </form>
</body>
</html>
