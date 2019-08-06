<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fullInfo.aspx.cs" Inherits="Web.Admin.fullInfo" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" rel="stylesheet"  href="js/layer/btnCss/layui.css"/>
    <style>
.td{border:solid #add9c0; border-width:0px 1px 1px 0px; padding:5px 0px;}
.table{border:solid #add9c0; border-width:1px 0px 0px 1px;}
.gcenter { margin:0 auto;text-align:center;}
.bgi1 {     
        background-image: url(../admin/img/index/noise.png),url(../admin/img/index/1.jpg);
        background-repeat: repeat, no-repeat;
        background-position: left top;
        background-size: auto, cover;}
    </style>
</head>
<body class="bgi1">
    <form id="form1" runat="server">
         <div id="gv2" runat="server" style="overflow: auto; overflow-x: auto; margin-top:20px;
            overflow-y: hidden;white-space:nowrap;width:auto;overflow:auto; " >
           <label style="color:#ff0000">信道:</label>
           <asp:GridView ID="GridView2" CssClass="layui-table " runat="server" 
               OnRowDataBound="GridView2_RowDataBound" OnRowCancelingEdit="GridView2_RowCancelingEdit" 
               OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating">
               <Columns>
                   <asp:TemplateField  ShowHeader="False" meta:resourcekey="TemplateFieldResource1">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" 
                            CommandName="Update" Text="更新" meta:resourcekey="LinkButton1Resource1"  
                            OnClientClick="return confirm('确定继续执行么？')" > </asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                            CommandName="Cancel" Text="取消" meta:resourcekey="LinkButton2Resource1"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                            CommandName="Edit" Text="编辑" meta:resourcekey="LinkButton1Resource2"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

               </Columns>
                </asp:GridView>
       </div>
        <div id="gv1" runat="server" style="overflow: auto; overflow-x: auto; overflow-y: hidden;
            white-space:nowrap;width:auto; overflow:auto;margin-top:20px" >
        <label style="color:#ff0000">视频名称:</label>
        <asp:GridView ID="GridView1" runat="server" CssClass="layui-table " OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDataBound="GridView1_RowDataBound" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
            <Columns>
                   <asp:TemplateField  ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton7" runat="server" 
                            CommandName="Update" Text="更新" 
                            OnClientClick="return confirm('确定继续执行么？')" > </asp:LinkButton>
                        <asp:LinkButton ID="LinkButton8" runat="server" CausesValidation="False" 
                            CommandName="Cancel" Text="取消" ></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton9" runat="server" CausesValidation="False" 
                            CommandName="Edit" Text="编辑"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

               </Columns>
        </asp:GridView>
    </div>
        <br />
        <div id="gv3" runat="server" style="overflow: auto; overflow-x: auto; overflow-y: hidden;
            white-space:nowrap;width:auto;overflow:auto;margin-top:20px">
            <label style="color:#ff0000">类型:</label>
            <asp:GridView ID="GridView3" CssClass="layui-table " runat="server" OnRowCancelingEdit="GridView3_RowCancelingEdit" OnRowEditing="GridView3_RowEditing" OnRowUpdating="GridView3_RowUpdating" OnRowDataBound="GridView3_RowDataBound">
                <Columns>
                   <asp:TemplateField  ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton4" runat="server" 
                            CommandName="Update" Text="更新"  
                            OnClientClick="return confirm('确定继续执行么？')" > </asp:LinkButton>
                        <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" 
                            CommandName="Cancel" Text="取消"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton6" runat="server" CausesValidation="False" 
                            CommandName="Edit" Text="编辑"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

               </Columns>
            </asp:GridView>

        </div>
        <br />


    </form>
</body>
</html>
