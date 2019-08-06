<%@ Page Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="addType.aspx.cs" 
    Inherits="Web.Admin.AddAll.addType" 
    SmartNavigation="true" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../js/layer/btnCss/layui.css"/>
    <style>
        .fl { float:left}
        .fr { position:absolute;}
        .ml {margin-left:20px;}
        .mt {margin-top:10px;}
        .gcenter { margin:0 auto;text-align:center;}
        .touming {background: rgba(255,204,51, 0); }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" CssClass="fl">
                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                    <ParentNodeStyle Font-Bold="False" />
                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                </asp:TreeView>
            </ContentTemplate>
            </asp:UpdatePanel>
        <div style=" width:300px; height:300px;margin-left:250px; position:absolute; left: 0px; top: 0px;">
            在选中节点下添加类型<br/><br/>
            第一步:点击选中类型<br/><br/>
            第二步:上传图片<br/>
            <div class="layui-form-item">
                 <asp:FileUpload ID="FileUpload1"  runat="server" 
                     CssClass="layui-btn layui-btn-primary layui-btn-mini" onchange="btnshow()"/><br/>
            </div>
           
            <div class="layui-form-item">
                 <asp:Button ID="Button3" runat="server" Text="上传图片" 
                     CssClass="layui-btn layui-btn-primary layui-btn-mini mt" OnClick="Button3_Click"/><br/>
                </div>
            第三步:填写数据<br/>
                <label class="layui-form-label">标题:</label>
                <div class="layui-input-block">
                  <input type="text" id="title" runat="server" name="title" lay-verify="title" 
                      autocomplete="off" placeholder="请输入标题" class="layui-input touming"/>
                </div><br/>
                <label class="layui-form-label">优先级:</label>
                <div class="layui-input-block">
                  <input type="text" id="sort" runat="server" name="title" lay-verify="title" autocomplete="off" 
                      placeholder="越小位置就越前,1~9999" class="layui-input touming"/>
                  <input id="viewsortpanl" type="button" value="查看其它优先级" 
                      class="layui-btn layui-btn-primary layui-btn-mini mt" />
                   
                  
                </div>
            
             <div class="layui-form-item mt">
                <asp:Button ID="Button2" runat="server" Text="在选中类型的子级下新增类型" 
                    CssClass="layui-btn layui-btn-big layui-btn-primary layui-btn-radius "
                     OnClick="Button2_Click"/>
                 </div>
                返回的Tid:为添加视频中的Tid
                <input id="Text1" type="text" class=" touming" runat="server" readonly="true" style="width:50px; height:20px;" />
            <div style="width:120px; height:120px; position:absolute;left: 320px; top: 50px;">
                <img id="img_c" runat="server" style="width:215px;height:275px"/>
            </div>
          </div>
         <div id="sort_view" style="display:none">
             <asp:GridView ID="GridView1" runat="server" CssClass="gcenter layui-table"></asp:GridView>
         </div>
        <input type="text" id="img" runat="server" name="title1" 
                     readonly="true" lay-verify="title" autocomplete="off" 
                     class="layui-input fl" style="display:none;" size="20"/>
    </div>
        <script>
            function createlabel(text) {
                var mydiv = document.getElementById("sort_view");
                var input = document.createElement("label");
                var input2 = document.createElement("br");
                input.textContent = text;
                input.classList = "layui-form-block";
                input.style.marginLeft = "10px";
                mydiv.appendChild(input)
                mydiv.appendChild(input2)
            }
        </script>
    </form>
    <script type="text/javascript" src="../js/jquery-1.11.3.js" ></script>
    <script type="text/javascript" src="../js/layer/layer.js" ></script>
    <script>
        $("#viewsortpanl").click(function () {
            layer.open({
                type: 1,
                area: ['200px', '450px'],
                shadeClose: true, //点击遮罩关闭
                content: $("#sort_view")
            });
        });
        function btnshow()
        {
            document.getElementById("<%=Button3.ClientID %>").removeAttribute("disabled");
            document.getElementById("<%=Button3.ClientID %>").className = "layui-btn  layui-btn-mini";
            document.getElementById("img").value = "";
        }

    </script>
</body>
</html>
