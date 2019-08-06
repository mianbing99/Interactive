<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminPage.Master" AutoEventWireup="true" CodeBehind="VideoList.aspx.cs" Inherits="Web.Admin.Video.VideoList" %>

<%@ MasterType VirtualPath="~/Admin/AdminPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_head" runat="server">
    <link href="/Admin/Content/laypage/laypage.css" rel="stylesheet" />
    <link href="/Admin/css/inserthtml.com.radios.css" rel="stylesheet" />
    <style type="text/css">
        .VideoType { text-indent:20px; height:40px; line-height:40px;}
        .VideoType select {margin-right:10px; height:23px; line-height:23px;  width:130px; border:1px solid #ccc;}
        .TbVideoList tr td { border-left: 1px solid #eeeeee; border-right: 1px solid #eeeeee; }
        .TbVideoList thead tr { background-color: #efefef; text-align: center; font-weight: bold; }
        .TbVideoList thead tr td:nth-child(1) { width: 80px; }
        .TbVideoList thead tr td:nth-child(3) { width: 200px; }
        .TbVideoList thead tr td:nth-child(4) { width: 50px; }
        .TbVideoList thead tr td:nth-child(5) { width: 50px; }
        .TbVideoList thead tr td:nth-child(6) { width: 80px; }
        .TbVideoList tbody tr td { text-align: center; }
        .TbVideoList tbody tr td:nth-child(1) { border-left: none; }
        .TbVideoList tbody tr td:nth-child(2) { text-align: left; }
        .TbVideoList tbody tr td:nth-child(6) { border-right: none; }
        .TbVideoList tfoot tr { background-color: #efefef; text-align: center; }
        .VideoLink { color: #0094ff; text-decoration: underline; }

        .VideoUpd { text-indent: 20px; height: 40px; line-height: 40px; width: 100%; }
        .VideoUpd input { text-indent: 5px; border: 1px solid #eeeeee; width: 400px; height: 25px; line-height: 25px; }
        .TbVideoUrlList { line-height: 20px; }
        .TbVideoUrlList tr td { border-left: 1px solid #eeeeee; border-right: 1px solid #eeeeee; }
        .TbVideoUrlList tr td { line-height: 1.5em; }
        .TbVideoUrlList thead tr { background-color: #ededed; text-align: center; font-weight: bold; }
        .TbVideoUrlList thead tr td:nth-child(1) { width: 80px; border-left: none; }
        .TbVideoUrlList thead tr td:nth-child(2) { width: 100px; }
        .TbVideoUrlList tbody tr td:nth-child(3) { text-align: left; }
        .TbVideoUrlList tbody tr td:nth-child(4) { width: 50px; }
        .TbVideoUrlList tbody tr td:nth-child(5) { width: 80px; border-right: none; }
        .TbVideoUrlList tbody tr td { text-align: center; }
        .TbVideoUrlList tbody tr:last-child td { border-bottom: none; }
        .TbVideoUrlList tfoot tr td { text-align: center; }
        .auto-style1 {
            width: 101px;
        }
        .auto-style2 {
            height: 40px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_body" runat="server">
    <div class="Block-Box">
        <div class="Block-Title">
            <span>
                <i class="icon-th-list"></i>
            </span>
            <h4>资源列表</h4>
            
        </div>
        <div class="Block-Content">
            <div class="VideoType">
                分类：
            </div>
            <table class="TbCommon TbVideoList">
                <thead>
                    <tr>
                        <td class="auto-style1">编号</td>
                        <td>名称</td>
                        <td>分类</td>
                        <td>排序</td>
                        <td>显示</td>
                        <td>操作</td>
                    </tr>
                </thead>
                <tbody>
<%--                    <tr>
                        <td class="auto-style2">123</td>
                        <td class="auto-style2">打算</td>
                        <td class="auto-style2">佛挡杀佛</td>
                        <td class="auto-style2">99</td>
                        <td class="auto-style2">true</td>
                        <td class="auto-style2"></td>
                    </tr>--%>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6" id="page1"></td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_foot" runat="server">
    <script src="/Admin/Content/laypage/laypage.js"></script>
    <script src="/Admin/Content/laytpl/laytpl.js"></script>
    <script type="text/javascript">
        var index = 1;
        var size = 9;
        var page = 0;
        var tid = 0;
        var level = 0;
        $(document).ready(function () {
            InitVideoType();
            InitVideos();
            $(".VideoType").delegate("select", "change", function () {
                var id = $(this).find("option:selected").val();
                var lv = parseInt($(this).attr("lev"))+1;
                var selectcount = $(".VideoType select").length;
                for (var i = lv; i < selectcount; i++) {
                    $(".VideoType select[lev='" + i + "']").remove();
                }
               if (id != "-1") {
                   tid = id;
                   index = 1;
                   level = lv;
                   InitVideoType()
               } else {
                   layer.msg("请选择分类！", { icon: 7, time: 2000 });

               }
            });
            $(".TbVideoList").delegate(".VideoLink", "click", function () {
                var loadmsg = layer.load(2);
                var _id = $(this).attr("val");
                var _title = $($(this).parents("tr")).children("td")[1].innerHTML;
                $.ajax({
                    url: '/API/AdminVideo.asmx/GetVideoUrl',
                    type: 'post',
                    data: { "vid": _id },
                    success: function (data) {
                        layer.close(loadmsg);
                        if (data.StateCode == 0) {
                            var data = {
                                title: _title,
                                list: data.Content
                            };
                            var gettpl = $("#demo").html();
                            laytpl(gettpl).render(data, function (html) {
                                var temppage = layer.open({
                                    title: "更新视频资源",
                                    type: 1,
                                    skin: "layui-layer-molv", //加上边框
                                    area: "500px", //宽高
                                    maxmin: true,
                                    content: html
                                });
                                layer.full(temppage);
                            });
                            $(".TbVideoUrlList").delegate("a[id^='DelVideo_']", "click", function () {
                                //var loadmsg = layer.load(2);
                                var _id = $(this).attr("id").split("_")[1];
                                $.ajax({
                                    url: '/API/AdminVideo.asmx/DelVideoUrl',
                                    type: 'post',
                                    data: { "vid": _id },
                                    success: function (data) {
                                        layer.close(loadmsg);
                                        if (data.StateCode == 0) {
                                            layer.msg(data.Content, { icon: 1, time: 2000 });
                                        } else {
                                            layer.msg(data.Content, { icon: 7, time: 2000 });
                                        }
                                    },
                                    error: function (e) {
                                        layer.close(loadmsg);
                                        layer.msg(e.responseText, { icon: 2, time: 2000 });
                                    }
                                });
                            });
                        } else {
                            layer.msg(data.Content, { icon: 7, time: 2000 });
                        }
                    },
                    error: function (e) {
                        layer.close(loadmsg);
                        layer.msg(e.responseText, { icon: 2, time: 2000 });
                    }
                });
            });
        });
        
        function InitVideos() {
            var loadmsg = layer.load(2);
            $.ajax({
                url: '/API/AdminVideo.asmx/GetVideoList',
                type: 'post',
                data: { "Tid": tid, "Index": index, "Size": size },
                success: function (data) {
                    layer.close(loadmsg);
                    if (data.StateCode == 0) {
                        page = Math.ceil(data.Content.Count / size);
                        BindVideos(data.Content.Videos);
                        if (index == 1 && page > 0) {
                            laypage({
                                cont: 'page1', //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                                pages: page, //通过后台拿到的总页数
                                curr: index, //初始化当前页
                                jump: function (e, first) { //触发分页后的回调
                                    if (!first) {
                                        index = e.curr;
                                        InitVideos();
                                    }
                                }
                            });
                        }
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
        function BindVideos(data) {
            $(".TbVideoList tbody tr").remove();
            for (var i = 0; i < data.length; i++) {
                var html = '<tr>';
                html += '<td>' + data[i].Id + '</td>';
                html += '<td>' + data[i].Title + '</td>';
                html += '<td>' + data[i].Type + '</td>';
                html += '<td>' + data[i].Sort + '</td>';
                var ck = data[i].State == true ? "checked" : ""
                html += '<td><input type="checkbox" id="checkbox-10-' + data[i].Id + '"  ' + ck + ' /><label for="checkbox-10-' + data[i].Id + '"></label></td>';
                html += '<td><a href="javascript:void(0);" val="' + data[i].Id + '" class="VideoLink">详情</a></td>';
                html += '</tr>';
                $(".TbVideoList tbody").append(html);
            }
        }
        function InitVideoType() {
            var loadmsg = layer.load(2);
            $.ajax({
                async:false,
                url: '/API/AdminVideo.asmx/GetVideoType',
                type: 'post',
                data: { "Tid": tid},
                success: function (data) {
                    layer.close(loadmsg);
                    if (data.StateCode == 0) {
                        BindVideoType(data.Content);
                    } else if (data.StateCode==201) {
                        InitVideos();
                    } else{
                        layer.msg(data.Content, { icon: 7, time: 2000 });
                    }
                },
                error: function (e) {
                    layer.close(loadmsg);
                    layer.msg(e.responseText, { icon: 2, time: 2000 });
                }
            });
        }
        function BindVideoType(data) {
            var html = '<select lev="' + level + '">';
            html += '<option value=\"-1\">请选择</option>';
            for (var i = 0; i < data.length; i++) {
                html+='<option value=\"'+data[i].Id+'\">'+data[i].Title+'</option>';
            }
            html += '</select>';
            $(".VideoType").append(html);
        }

        $(function () {
            $("#Button1").click(function () {
                layer.open({
                    type: 1,
                    area: ['700px', '300px'],
                    shadeClose: true, //点击遮罩关闭
                    content: '\<\div style="padding:20px;">自定义内容\<\/div>'
                    //content: $("#openupdata")
                });
            })
        });
    </script>

    <script id="demo" type="text/html">
        <div class="VideoUpd">
            标题：<span contenteditable="true">{{ d.title }}</span>
        </div>
        <table class="TbCommon TbVideoUrlList">
            <thead>
                <tr>
                    <td>编号</td>
                    <td>来源</td>
                    <td>路径</td>
                    <td>排序</td>
                    <td>操作</td>
                </tr>
            </thead>
            <tbody>
                {{# for(var i = 0, len = d.list.length; i < len; i++){ }}
                     <tr>
                         <td>{{ d.list[i].Id }}</td>
                         <td><p contenteditable="true">{{ d.list[i].Source }}</p></td>
                         <td><p contenteditable="true">{{ d.list[i].Path }}</p></td>
                         <td><p contenteditable="true">{{ d.list[i].Sort }}</p></td>
                         <td><a href="javascript:void(0);" id="DelVideo_{{ d.list[i].Id }}" class="VideoLink">删除</a></td>
                     </tr>
                {{# } }}
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="5"><a href="javascript:void(0);" class="VideoLink">更新视频资源</a></td>
                </tr>
            </tfoot>
        </table>
    </script>
</asp:Content>
