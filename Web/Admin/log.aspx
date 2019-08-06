<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminPage.Master" AutoEventWireup="true" CodeBehind="log.aspx.cs" Inherits="Web.Admin.log" %>

<%@ MasterType VirtualPath="~/Admin/AdminPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_head" runat="server">
    <style type="text/css">
        .LogType { text-indent: 20px; height: 40px; line-height: 40px; }
        .LogType select { margin-right: 10px; height: 23px; line-height: 23px; width: 130px; }
        .TbLogList tr td { border-left: 1px solid #eeeeee; border-right: 1px solid #eeeeee; }
        .TbLogList thead tr { background-color: #efefef; text-align: center; font-weight: bold; }
        .TbLogList thead tr td:nth-child(1) { width: 80px; }
        .TbLogList thead tr td:nth-child(3) { width: 200px; }
        .TbLogList thead tr td:nth-child(4) { width: 50px; }
        .TbLogList thead tr td:nth-child(5) { width: 50px; }
        .TbLogList thead tr td:nth-child(6) { width: 80px; }
        .TbLogList tbody tr td { text-align: center; }
        .TbLogList tbody tr td:nth-child(1) { border-left: none; }
        .TbLogList tbody tr td:nth-child(2) { text-align: left; }
        .TbLogList tbody tr td:nth-child(6) { border-right: none; }
        .TbLogList tfoot tr { background-color: #efefef; text-align: center; }
        .LogLink { color: #0094ff; text-decoration: underline; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_body" runat="server">
    <div class="Block-Box">
        <div class="Block-Title">
            <span>
                <i class="icon-cog"></i>
            </span>
            <h4>日志管理</h4>
        </div>
        <div class="Block-Content">
            <div class="LogType">
                分类：
            </div>
            <table class="TbCommon TbLogList">
                <thead>
                    <tr>
                        <td>编号</td>
                        <td>名称</td>
                        <td>分类</td>
                        <td>排序</td>
                        <td>显示</td>
                        <td>操作</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>123</td>
                        <td>打算</td>
                        <td>佛挡杀佛</td>
                        <td>99</td>
                        <td>true</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>123</td>
                        <td>打算</td>
                        <td>佛挡杀佛</td>
                        <td>99</td>
                        <td>true</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>123</td>
                        <td>打算</td>
                        <td>佛挡杀佛</td>
                        <td>99</td>
                        <td>true</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>123</td>
                        <td>打算</td>
                        <td>佛挡杀佛</td>
                        <td>99</td>
                        <td>true</td>
                        <td></td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6" id="page1">dsad</td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_foot" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

        });
    </script>
</asp:Content>
