<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Web.Admin.titleOperation.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../js/layer/btnCss/layui.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
                    <div id="regedit_div" runat="server" class="none" style="width:350px">
                    <div class="layui-form-item">
                            <label class="layui-form-label">用户名:</label>
                            <div class="layui-input-block">
                                  <input id="txt_name" runat="server" type="text"  lay-verify="title" autocomplete="off" placeholder="请输入用户名" class="layui-input" maxlength="16"/>
                                </div>
                            </div>
                            
                    <div class="layui-form-item">
                            <label class="layui-form-label">密码:</label>
                            <div class="layui-input-block">
                                  <input id="txt_pwd" runat="server" type="password" lay-verify="title" autocomplete="off" placeholder="请输入密码" class="layui-input"/>
                                </div>
                            </div>

                    <div class="layui-form-item">
                            <label class="layui-form-label">确认密码:</label>
                            <div class="layui-input-block">
                                  <input id="txt_pwd_qr" runat="server" type="password" lay-verify="title" autocomplete="off" placeholder="确认密码" class="layui-input"/>
                                </div>
                            </div>
                    <div  class="layui-form-item">
                            <label class="layui-form-label">权限:</label>
                            <div class="layui-form-label" style="width:auto">
                                <asp:CheckBox ID="cb_select" runat="server" Text="查" Checked="true"/>
                                <asp:CheckBox ID="cb_add" runat="server" Text="增" Checked="true" />
                                <asp:CheckBox ID="cb_update" runat="server" Text="改" Checked="true"/>
                                <asp:CheckBox ID="cb_dele" runat="server" Text="删"/>
                                <asp:CheckBox ID="cb_export" runat="server" Text="导出"/>
                                <asp:CheckBox ID="cb_regedit" runat="server" Text="注册"/>
                                </div>
                            </div>


                            <div class="layui-form-item">
                            <label class="layui-form-label">状态:</label>
                            <div class="layui-form-label">
                                 <asp:DropDownList ID="ddl_s" runat="server">
                                    <asp:ListItem Value="1">1正常</asp:ListItem>
                                    <asp:ListItem Value="0">0关闭</asp:ListItem>
                                    <asp:ListItem Value="2">2异常</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>



                            <div class="layui-form-item">
                            <label class="layui-form-label">手机:</label>
                            <div class="layui-input-block">
                                  <input id="txt_phone" runat="server" type="text"  lay-verify="title" autocomplete="off" placeholder="请输入手机号" class="layui-input" maxlength="11" onkeyup="this.value=this.value.replace(/\D/g,'')"/>
                                </div>
                            </div>
                                
                                         <div class="layui-form-item">
                            <label class="layui-form-label">邮箱:</label>
                            <div class="layui-input-block">
                                  <input id="txt_email" runat="server" type="text"  lay-verify="title" autocomplete="off" placeholder="请输入邮箱" class="layui-input" />
                                </div>
                            </div>

                            <div style="text-align:center">
                            <asp:Button ID="btn_r_ok" runat="server" Text="确定" OnClick="btn_r_ok_Click" OnClientClick="return checkRegister()" CssClass="layui-btn  layui-btn-small"/>
                            </div>
                </div>
    </div>
    </form>
    <script src="../js/jquery-1.11.3.js"></script>
    <script type="text/javascript" src="../js/layer/layer.js"></script>
    <script>
        function checkRegister() {
            var pwd = document.getElementById("txt_pwd").value;
            var pwd2 = document.getElementById("txt_pwd_qr").value;
            var name = document.getElementById("txt_name").value;
            var state="<%=state%>";
            var register = "<%=register%>";
            if (state != 1) {
                layer.alert('抱歉,你的账号异常', {
                    icon: 5, title: false,
                    skin: 'layer-ext-moon'
                });
                return false;
            } else {
                if (register != 1) {
                    layer.alert('抱歉你没有权限', {
                        icon: 5, title: false,
                        skin: 'layer-ext-moon'
                    });
                    return false;
                } else {
                    if (pwd == "" || pwd2 == "" || name == "") {
                        layer.alert('您还有信息没有输入', {
                            icon: 2, title: false,
                            skin: 'layer-ext-moon'
                        });
                        return false;
                    } else {
                        if (pwd == pwd2) {


                        } else {
                            layer.alert('两次密码输入不同', {
                                icon: 2, title: false,
                                skin: 'layer-ext-moon'
                            });
                            return false;
                        }
                    }
                }
            }
            
        }
        </script>
</body>
</html>
