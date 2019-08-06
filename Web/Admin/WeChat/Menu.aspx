<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminPage.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Web.Admin.WeChat.Menu" %>

<%@ MasterType VirtualPath="~/Admin/AdminPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_body" runat="server">
    <div class="Block-Box">
        <div class="Block-Title">
            <span>
                <i class="icon-cog"></i>
            </span>
            <h4>微信菜单</h4>
        </div>
        <div class="Block-Content">
            <a href="javascript:void(0);" id="UpdateWeChatMenu" class="Color_Lv">更新微信菜单</a>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_foot" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#UpdateWeChatMenu").click(function () {
                $.ajax({
                    url: '/API/WeChat.asmx/UpdateMenu',
                    type: 'post',
                    dataType: 'json',
                    success: function (data) {
                        alert(data.errmsg);
                    },
                    error: function (e) {
                        //alert(e.responseText);
                    }
                });
            });
        });
    </script>
</asp:Content>
