<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePwd.aspx.cs" Inherits="Web.Admin.titleOperation.ChangePwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../js/layer/btnCss/layui.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
                    <div id="updatepwd_div" style="width:300px">
                            <div class="layui-form-item" >
                            <label class="layui-form-label">账号:</label>
                            <label class="layui-form-label" style="color:red"><%=name%></label>
                            </div>

                            <div class="layui-form-item">
                            <label class="layui-form-label">原密码:</label>
                            <div class="layui-input-block">
                                  <input id="old_pwd" runat="server" type="password" name="password" lay-verify="title" autocomplete="off" placeholder="请输入原密码" class="layui-input"/>
                                </div>
                            </div>

                            <div class="layui-form-item">
                            <label class="layui-form-label">新密码:</label>
                            <div class="layui-input-block">
                                  <input id="new_pwd" runat="server" type="password" name="password" lay-verify="title" autocomplete="off" placeholder="请输入新密码" class="layui-input"/>
                                </div>
                            </div>

                            <div class="layui-form-item">
                            <label class="layui-form-label">确认新密码:</label>
                            <div class="layui-input-block">
                                  <input id="new_pwd2" runat="server" type="password" name="password" lay-verify="title" autocomplete="off" placeholder="请确认新密码" class="layui-input"/>
                                </div>
                            </div>

                            <div style="text-align:center">
                            <asp:Button ID="btn_changepwd" runat="server" Text="确定更改" OnClick="btn_changepwd_Click" OnClientClick="return checknull()" CssClass="layui-btn  layui-btn-small"/>
                            </div>
                </div>
    </div>
    </form>
        <script src="../js/jquery-1.11.3.js"></script>
    <script type="text/javascript" src="../js/layer/layer.js"></script>
    <script>
        function checknull()
        {
            var oldpwd = document.getElementById("old_pwd").value;
            var newpwd = document.getElementById("new_pwd").value;
            var newpwd2 = document.getElementById("new_pwd2").value;
            if (oldpwd != "" && newpwd != "" && newpwd2 != "") {
                if (newpwd == newpwd2) {
                    if (oldpwd == newpwd) {
                        layer.alert('新密码不能与旧密码相同', {
                            icon: 2, title: false,
                            skin: 'layer-ext-moon'
                        });
                        return false;
                    } else {
                        return true;
                    }
                    
                } else {
                    layer.alert('两次新密码输入不同', {
                        icon: 2, title: false,
                        skin: 'layer-ext-moon' 
                    });
                    return false;
                }
            } else {
                layer.alert('您还有信息没有输入', {
                    icon: 2, title: false,
                    skin: 'layer-ext-moon'
                });
                return false;
            }
        }
         </script>
</body>
</html>
