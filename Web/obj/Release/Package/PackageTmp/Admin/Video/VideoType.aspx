<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminPage.Master" AutoEventWireup="true" CodeBehind="VideoType.aspx.cs" Inherits="Web.Admin.Video.VideoType" %>

<%@ MasterType VirtualPath="~/Admin/AdminPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_head" runat="server">
    <style type="text/css">
        .TbTypeSourceSort tr td { border-left: 1px solid #eeeeee; border-right: 1px solid #eeeeee; }
        .TbTypeSourceSort thead tr { background-color: #efefef; text-align: center; font-weight: bold; }
        .TbTypeSourceSort tbody tr td:nth-child(1) { text-align: center; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_body" runat="server">
    <div class="Block-Box">
        <div class="Block-Title">
            <span>
                <i class="icon-th-list"></i>
            </span>
            <h4>资源类目</h4>
        </div>
        <div class="Block-Content">
            <table class="TbCommon TbTypeSort">
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_foot" runat="server">
    <script src="/Admin/Content/laytpl/laytpl.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            InitTypeSourceSort();
        });
        function InitTypeSourceSort() {
            var loadmsg = layer.load(2);
            $.ajax({
                url: '/API/AdminVideo.asmx/GetSource',
                type: 'post',
                success: function (data) {
                    layer.close(loadmsg);
                    if (data.StateCode == 0) {
                        BindSource(data.Content);
                    } else {
                        layer.msg(data.Content, { icon: 7, time: 2000 });
                    }
                },
                error: function (e) {
                    layer.close(loadmsg);
                    layer.msg(e.responseText, { icon: 2, time: 2000 });
                }
            });
        }
        function BindSource(data) {
            var data = {
                Source: data,
            };
            var gettpl = $("#Tb-Title").html();
            laytpl(gettpl).render(data, function (html) {
                $(".TbTypeSort").append(html);
            });
        }
    </script>
    <script id="Tb-Title" type="text/html">
            <thead>
                <tr>
                    <%--   Source: data.Content,
           Type: _title,
           Sort:"",--%>
                    <td>编号</td>
                    <td>分类名称</td>
                    {{# for(var i = 0, len = d.Source.length; i < len; i++){ }}
                        <td>{{d.Source[i].Source}}</td>
                    {{# } }}
                </tr>
            </thead>
    </script>
    <script id="Tb-Body" type="text/html">
        <tbody>
            {{# for(var i = 0, len = d.Type.length; i < len; i++){ }}
                <tr>
                    <td>{{d.Type[i].Id}}</td>
                    <td>{{d.Type[i].Title}}</td>

                    <%--   <td>佛挡杀佛</td>
                    <td>99</td>
                    <td>true</td>
                    <td></td>--%>
                </tr>
            {{# } }}
        </tbody>
    </script>
</asp:Content>
