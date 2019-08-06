<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminPage.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Web.Admin.index" %>
<%@ MasterType VirtualPath="~/Admin/AdminPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_head" runat="server">
    <style type="text/css">
        .TbIndex tr td:first-child { text-align:right; width:180px; }
        .TbIndex tr td:nth-child(2) { text-indent:10px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_body" runat="server">
    <div class="Block-Box">
        <div class="Block-Title">
            <span>
                <i class="icon-cog"></i>
            </span>
            <h4>WEB服务器信息</h4>
        </div>
        <div class="Block-Content">
            <table class="TbCommon TbIndex">
                <tr>
                    <td>服务器名称</td>
                    <td><%= WEB.Name %></td>
                </tr>
                <tr>
                    <td>服务器IP地址</td>
                    <td><%= WEB.IP %></td>
                </tr>
                <tr>
                    <td>服务器域名</td>
                    <td><%= WEB.WebName %></td>
                </tr>
                <tr>
                    <td>.NET解释引擎版本</td>
                    <td><%= WEB.NetVersion %></td>
                </tr>
                <tr>
                    <td>服务器操作系统版本</td>
                    <td><%= WEB.SystemVersion %></td>
                </tr>
                <tr>
                    <td>服务器IIS版本</td>
                    <td><%= WEB.IISVersion %></td>
                </tr>
                <tr>
                    <td>HTTP访问端口</td>
                    <td><%= WEB.HttpPort %></td>
                </tr>
                <tr>
                    <td>虚拟目录的绝对路径</td>
                    <td><%= WEB.Path1 %></td>
                </tr>
                <tr>
                    <td>执行文件的绝对路径</td>
                    <td><%= WEB.Path2 %></td>
                </tr>
                <tr>
                    <td>虚拟目录Session总数</td>
                    <td><%= WEB.SessionCount %></td>
                </tr>
                <tr>
                    <td>虚拟目录Application总数</td>
                    <td><%= WEB.ApplicationCount %></td>
                </tr>
                <tr>
                    <td>域名主机</td>
                    <td><%= WEB.DomainHosting %></td>
                </tr>
                <tr>
                    <td>服务器区域语言</td>
                    <td><%= WEB.Language %></td>
                </tr>
                <tr>
                    <td>用户信息</td>
                    <td><%= WEB.UserInfo %></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_foot" runat="server">
</asp:Content>


