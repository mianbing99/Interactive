<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeAccountQX.aspx.cs" Inherits="Web.Admin.titleOperation.ChangeAccountQX" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../js/layer/btnCss/layui.css" />
    <style type="text/css">
        .ml { margin-left:10px}
        .bgi2 {     
        background-image: url(../img/index/noise.png),url(../img/index/2.jpg);
        background-repeat: repeat, no-repeat;
        background-position: left top;
        background-size: auto, cover;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="updateqx_div" style="text-align:center;">
                    <asp:Button ID="getuser_btn" runat="server" Text="查看所有用户" OnClick="getuser_btn_Click" OnClientClick="return checkqx()"  CssClass="layui-btn layui-btn-primary layui-btn-small"/>
                    <div id="btn_jihe" runat="server" style="height:100px; margin-top:10px">

                    </div>

               <div id="updateuser_div" runat="server" style="width:350px;display:none">
                   
                    <div class="layui-form-item">
                            <label class="layui-form-label">用户名:</label>
                        <div class="layui-input-block" style="width:350px;">
                            <asp:TextBox runat="server" ID="txt_name" CssClass="layui-input" Enabled="false"></asp:TextBox>

                         </div>
                        </div>


                    <div  class="layui-form-item">
                            <label id="lb_qx" class="layui-form-label">权限:</label>
                            <div class="layui-form-label" style="width:auto">
                                <asp:CheckBox ID="cb_select" runat="server" Text="查" />
                                <asp:CheckBox ID="cb_add" runat="server" Text="增" />
                                <asp:CheckBox ID="cb_update" runat="server" Text="改" />
                                <asp:CheckBox ID="cb_dele" runat="server" Text="删"/>
                                <asp:CheckBox ID="cb_export" runat="server" Text="导出"/>
                                <asp:CheckBox ID="cb_regedit" runat="server" Text="注册"/>
                                </div>
                            </div>

                            <div class="layui-form-item">
                            <label id="lb_state" class="layui-form-label">状态:</label>
                            <div class="layui-form-label" >
                                 <asp:DropDownList ID="ddl_s" runat="server">
                                    <asp:ListItem Value="0">0关闭</asp:ListItem>
                                    <asp:ListItem Value="1">1正常</asp:ListItem>
                                    <asp:ListItem Value="2">2异常</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                                    
                            </div>
                            <div class="layui-form-item">
                            <label id="lb_phone" class="layui-form-label">手机:</label>
                            <div class="layui-input-block" style="width:350px;">
                                  <input id="txt_phone" runat="server" type="text"  lay-verify="title" autocomplete="off" placeholder="请输入手机号" class="layui-input" maxlength="11" onkeyup="this.value=this.value.replace(/\D/g,'')">
                                </div>
                            </div>

                           <div class="layui-form-item">
                            <label id="lb_email" class="layui-form-label">邮箱:</label>
                            <div class="layui-input-block" style="width:350px;">
                                  <input id="txt_email" runat="server" type="text"  lay-verify="title" autocomplete="off" placeholder="请输入邮箱" class="layui-input" >
                                </div>
                            </div>
                            <div>
                                <div style="text-align:center">
                                <input id="btn_r_ok" type="button" value="确定" onclick="checkchange()" class="layui-btn layui-btn-small" />
                                <div style="float:right">
                                <input id="btn_r_dele" type="button" value="删除这个用户" onclick="deleteuser()" class="layui-btn layui-btn-small layui-btn-danger" />
                                </div>
                                </div>
                             </div>
                </div>
              </div>
            <div style="display:none" id="qrinfo">
                    <div id="qrinfo2">
                    </div>
                <div id="btn_oorc" style="margin-top:10px; text-align:center">
                <asp:Button ID="btn_okchang" runat="server" Text="确定修改" OnClick="btn_okchang_Click" CssClass="layui-btn layui-btn-small" />
                
                </div>
            </div>
            <div id="btn_duser" style="margin-top:10px; text-align:center;display:none">
                <div style="margin-bottom:10px">
                您确定删除 <%=goname%>这个用户吗?
                    </div>
                <asp:Button ID="btn_deleteUser" runat="server" Text="确定" OnClick="btn_deleteUser_Click" CssClass="layui-btn layui-btn-small" />
                
                </div>
        <input id="Hidden1" type="hidden" runat="server"  />
         </div>
    </form>
    <script type="text/javascript" src="../js/jquery-1.11.3.js"></script>
    <script src="/Admin/js/common.js"></script>
    <script type="text/javascript" src="../js/layer/layer.js"></script>
    <script>
        function checkchange() {
            var txt_name = document.getElementById("txt_name").value;
            var cb_select = document.getElementById("cb_select").checked;
            var cb_add = document.getElementById("cb_add").checked;
            var cb_update = document.getElementById("cb_update").checked;
            var cb_dele = document.getElementById("cb_dele").checked;
            var cb_regedit = document.getElementById("cb_regedit").checked;
            var cb_export = document.getElementById("cb_export").checked;
            var ddl_s = document.getElementById("ddl_s").value;
            var txt_phone = document.getElementById("txt_phone").value;
            var txt_email = document.getElementById("txt_email").value;
            var afterChange = txt_name + "," + txt_email + "," + txt_phone + "," + ddl_s + "," + cb_add + "," + cb_update + "," + cb_select + "," + cb_dele + "," + cb_regedit + "," + cb_export;
            afterChange = afterChange.split(',');
            var admininfo = ['<% =String.Join("','", info) %>'];
            var befotChangeinfo = new Array();
            var afterChangeinfo = new Array();
            var j = 0, title;
            delh();
            for (var i = 0; i < admininfo.length; i++) {
                if (admininfo[i] != afterChange[i]) {
                    befotChangeinfo[i] = admininfo[i];
                    afterChangeinfo[i] = afterChange[i];
                    switch (i)
                    {
                        case 0: title = "用户名"; break;
                        case 1: title = "邮箱"; break;
                        case 2: title = "手机"; break;
                        case 3: title = "状态"; break;
                        case 4: title = "增加"; break;
                        case 5: title = "修改"; break;
                        case 6: title = "查询"; break;
                        case 7: title = "删除"; break;
                        case 8: title = "注册"; break;
                        case 9: title = "导出"; break;
                    }
                    title = "  " + title + ":" + befotChangeinfo[i] + " 改为 " + afterChangeinfo[i];
                    document.getElementById("Hidden1").value += title;
                    createc(title);
                }
            }
            if (befotChangeinfo.length > 0) {
               var index= layer.open({
                    type: 1,
                    title: "您确定更改这些信息吗",
                    area: ['400px', '300px'],
                    shadeClose: true,
                    content: $("#qrinfo")
                });
            } else {
                layer.alert('您没有更改信息', {
                    icon: 0, title: false, shadeClose: true,
                    skin: 'layer-ext-moon'
                });
                return false;
            }
        }
        function createc(text)
        {
            var mydiv = document.getElementById("qrinfo2");
            var input = document.createElement("label");
            var input2 = document.createElement("br");
                input.textContent = text;
                input.classList = "layui-form-block";
                input.style.marginLeft = "30px";
                mydiv.appendChild(input)
                mydiv.appendChild(input2)
        }
        function delh() { document.getElementById("qrinfo2").innerHTML = "";}
        function checkqx() {
            var state = "<%=state%>";
            var register = "<%=register%>";
            if (state != 1) {
                layer.alert('抱歉,您的账号异常', {
                    icon: 5, title: false,
                    skin: 'layer-ext-moon'
                });
                return false;
            } else {
                if (register != 1) {
                    layer.alert('抱歉,您没有权限', {
                        icon: 5, title: false,
                        skin: 'layer-ext-moon'
                    });
                    return false;
                } else {
                    return true;
                }
            }

        }
        function deleteuser()
        {
            var dusername = document.getElementById("txt_name").value;
            layer.prompt({ title: '输入任何口令，并确认', formType: 1 }, function (pass, index) {
                if (pass == "admin") {
                    layer.open({
                        type: 1,
                        title: "删除用户?",
                        area: ['300px', '130px'],
                        shadeClose: true,
                        content: $("#btn_duser")
                    });
                    layer.close(index);
                } else {
                    layer.msg("口令错误");
                    layer.close(index);
                }
            });
                
        }
    </script>
</body>
</html>