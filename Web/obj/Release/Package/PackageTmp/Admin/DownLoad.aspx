<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownLoad.aspx.cs" Inherits="Web.Admin.DownLoad" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <link href="js/layer/btnCss/layui.css" rel="stylesheet"/>
    <link rel="stylesheet" href="js/laydate/need/laydate.css"/>
    <script src="js/laydate/laydate.js"></script>
    <style>
    .gcenter { margin:0 auto;}
        .none {display:none;text-align:center}
        .touming {background: rgba(255,204,51, 0);}
        .mt {margin-top:10px }
    </style>
    </head>
    <body><form runat="server">
    
    
            <%--查看导出数据记录--%>
            <div id="dlexcel_dc" style="padding-left:10px;">
                <div id="btn" style="margin-bottom:10px;text-align:center">
                    <asp:Button ID="Button1" runat="server" Text="导出数据记录" CssClass="layui-btn layui-btn-primary layui-btn-radius" OnClick="Button1_Click" />
                    <asp:Button ID="Button5" runat="server" Text="增加表记录" CssClass="layui-btn layui-btn-primary layui-btn-radius" OnClick="Button5_Click"/>
                    <asp:Button ID="Button2" runat="server" Text="修改表记录" CssClass="layui-btn layui-btn-primary layui-btn-radius" OnClick="Button2_Click"/>
                    <asp:Button ID="Button3" runat="server" Text="删除表记录" CssClass="layui-btn layui-btn-primary layui-btn-radius" OnClick="Button3_Click"/>
                    <asp:Button ID="Button4" runat="server" Text="账号注册修改记录" CssClass="layui-btn layui-btn-primary layui-btn-radius" OnClick="Button4_Click"/>
                    <div style="margin-top:10px">
                    <label id="lab" runat="server" style="color:#ff0000;font-size:16px;margin-top:10px"> </label>    
                    </div>

                </div>
                    
                    <div id="div_1"style="overflow: auto; overflow-x: auto; overflow-y: hidden;
                    white-space:nowrap; overflow:auto; margin-bottom:20px;">
                       
                        <asp:GridView ID="GridView1" runat="server" CssClass="gcenter layui-table mt" 
                            OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
                            AllowPaging="True" OnPageIndexChanged="GridView1_PageIndexChanged" 
                            OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="15">
                            <PagerTemplate>
                                <table id="ptl_Table" runat="server">
                                    <tr runat="server"><td runat="server">
                                        第<asp:Label ID="lblPageIndex" runat="server" 
                                            Text="<%# ((GridView)Container.Parent.Parent).PageIndex+1 %>"> </asp:Label>页
                                        共<asp:Label ID="lblPageCount" runat="server" 
                                            Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label>页
                                        <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" 
                                            CommandArgument="First"
                                             CommandName="Page">首页</asp:LinkButton>
                                        <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" 
                                            CommandArgument="Prev"
                                             CommandName="Page">上一页</asp:LinkButton>
                                        <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" 
                                            CommandArgument="Next"
                                             CommandName="Page">下一页</asp:LinkButton>
                                        <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" 
                                            CommandArgument="Last"
                                             CommandName="Page">尾页</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </PagerTemplate>
                        </asp:GridView> 
                        </div>
                <div runat="server" id="dc_btn" class="none">
                    <div style="margin-top:20px;margin-bottom:10px">
                    开始日:<input id="s_date" class="laydate-icon-dahong touming"
                         onclick="laydate({ istime: true, format: 'YYYY-MM-DD hh:mm:ss' })" runat="server"/><br />
                     结束日:<input id="e_date" class="laydate-icon-dahong touming"  
                         onclick="laydate({ istime: true, format: 'YYYY-MM-DD hh:mm:ss' });" runat="server"/><br />
            
                        </div>
                    <asp:Button ID="btn_dlexcel_dc" runat="server" Text="导出" OnClick="btn_dlexcel_dc_Click" 
                        CssClass="layui-btn layui-btn-primary layui-btn-mini " />
                        </div>
            </div>
        </form></body>
    </html>

