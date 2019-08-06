<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Web.Admin.index" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>e家亲后台</title>
    <meta name="renderer" content="ie-comp" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="SHORTCUT ICON" href="img/favicon.ico">
    <link href="js/layer/btnCss/layui.css" rel="stylesheet"/>
    <link href="/Admin/css/base.css" rel="stylesheet" />
    <link href="/Js/font/font.css" rel="stylesheet" />

    <link rel="stylesheet" href="js/laydate/need/laydate.css"/>
    <link rel="stylesheet" href="css/style.css"/>
    <link rel="stylesheet" href="js/layer/btnCss/layui.css"/>
    <script type="text/javascript" src="js/common_l.js" ></script>
    <script type="text/javascript" src="js/laydate/laydate.js" ></script>
    <style>
        .mr10 { margin-right:30px; }
        .txtrig { text-align:right}
        .txtleft { text-align:left}
        .cbl_h {float:left }
        .mt10 { margin-top:10px}
        .ck { }
        .auto-style11 {
            width: 184px;text-align:right;
        }
        .auto-style12 {
            width: 600px;
        }
        .touming {background: rgba(255,204,51, 0);}
        </style>
    </head>
    <body style="background-color:transparent">
        <form runat="server">
    <div id ="all" style="margin:10px;" class="inner">
    <div id ="son" class="son" runat="server" >
    <div style="display:none">
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="视频列表" 
            meta:resourcekey="Button1Resource1"/>
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="视频路径" 
            meta:resourcekey="Button2Resource1"/>
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="视频类别" 
            meta:resourcekey="Button3Resource1"/>
    </div>
        <div id="select" runat="server" style="width:600px; padding-bottom:5px">
            <label >模糊查询: </label>
            <input id="TextBox2" runat="server" type="text" class="touming"  />
            <asp:Button ID="select_btn" runat="server" Text="查询" OnClick="select_btn_Click" 
                OnClientClick="return checks1()"
                meta:resourcekey="select_btnResource1" CssClass="layui-btn layui-btn-primary layui-btn-mini" />
                <br />
           锁定查询: <asp:DropDownList ID="table_ddl" runat="server" 
               AutoPostBack="True" Width="100px" CssClass="touming" 
                        OnSelectedIndexChanged="table_ddl_SelectedIndexChanged" 
               meta:resourcekey="table_ddlResource1">
                        <asp:ListItem meta:resourcekey="ListItemResource1" Value="Video">名称表
                        </asp:ListItem>
                        <asp:ListItem meta:resourcekey="ListItemResource2" Value="VideoUrl">路径表
                        </asp:ListItem>
                        <asp:ListItem meta:resourcekey="ListItemResource3" Value="VideoType">类型表
                        </asp:ListItem>
                    </asp:DropDownList>
            <asp:Label ID="columns_lbl" runat="server" Text="列" meta:resourcekey="columns_lblResource1"> </asp:Label>
            <asp:DropDownList ID="columns_ddl" runat="server" AutoPostBack="True" Width="100px"
                OnSelectedIndexChanged="columns_ddl_SelectedIndexChanged" CssClass="touming"
                meta:resourcekey="columns_ddlResource1">
            </asp:DropDownList>
            <asp:Label ID="Label5" runat="server" Text="为" meta:resourcekey="Label5Resource1"> </asp:Label>
            <asp:TextBox ID="TextBox1" runat="server" Width="96px" CssClass="touming"
                meta:resourcekey="TextBox1Resource1"> </asp:TextBox>
            <span id="chuxian_panduan" runat="server">
                <asp:Label ID="Label6" runat="server" Text="~" meta:resourcekey="Label6Resource1"> </asp:Label>
                <asp:TextBox ID="TextBox3" runat="server" Width="54px" CssClass="touming"
                    meta:resourcekey="TextBox3Resource1"> </asp:TextBox>
            </span>
         <asp:Button ID="Button4" runat="server" Text="查询" OnClick="Button4_Click" 
             OnClientClick="return checks2()" meta:resourcekey="Button4Resource1" 
             CssClass="layui-btn layui-btn-primary layui-btn-mini"/>
        <div runat="server" id="flselect">
          分类查询:
        <asp:DropDownList ID="type1" runat="server" AutoPostBack="True" CssClass="touming"
            OnSelectedIndexChanged="type1_SelectedIndexChanged">
             <asp:ListItem>全部</asp:ListItem>
             <asp:ListItem Value="老人专区">老人专区</asp:ListItem>
             <asp:ListItem Value="幼儿专区">幼儿专区</asp:ListItem>
         </asp:DropDownList>
         <asp:DropDownList ID="type2" runat="server" AutoPostBack="True" CssClass="touming"
             OnSelectedIndexChanged="type2_SelectedIndexChanged">
             <asp:ListItem>全部</asp:ListItem>
                </asp:DropDownList>
         <asp:DropDownList ID="type3" runat="server" AutoPostBack="True" CssClass="touming"
             OnSelectedIndexChanged="type3_SelectedIndexChanged">
             <asp:ListItem>全部</asp:ListItem>
                </asp:DropDownList>
         <asp:DropDownList ID="type4" runat="server" AutoPostBack="True" CssClass="touming"
             OnSelectedIndexChanged="type4_SelectedIndexChanged">
             <asp:ListItem>全部</asp:ListItem>
                </asp:DropDownList>
         <asp:DropDownList ID="type5" runat="server" AutoPostBack="True" CssClass="touming">
             <asp:ListItem>全部</asp:ListItem>
                </asp:DropDownList>
         </div>
        </div>
            <div runat="server" id="zjselect" style="display:none">
                多条件查询:
                <input id="dtjselect_btn" type="button" value="查询" style="" 
                    class="layui-btn layui-btn-primary layui-btn-mini" /> 
            </div>
        <div id="updata_PL" class="none" style="padding-left:20px">
                <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="Label8" runat="server"></asp:Label>
                <asp:DropDownList ID="DropDownList1" runat="server" >
                </asp:DropDownList>
                全部改为<asp:TextBox ID="TextBox4" runat="server" style="width: 148px"></asp:TextBox>
            <input  type="button" value="修改" onclick="isok()" class="layui-btn  layui-btn-mini" />

        </div>
        <div id="zsgc_img" runat="server" >
            <table style="margin-bottom:5px;">
            <tr>
<%--                <td>查询<asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Admin/images/43-search.png" 
                      OnClick="ImageButton4_Click" Width="20px" Height="19px" style="float:left"/></td>--%>
                <%--<td><asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Admin/images/update.png"
                      OnClick="ImageButton1_Click" OnClientClick="return checkupdatenull()" /></td>--%>
                <%--<td><asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Admin/images/add.png" 
                      OnClick="ImageButton3_Click"  /></td>--%>
                <td><input id="img_update" type="button" value="修改" 
                    class="layui-btn layui-btn-primary layui-btn-mini" style="color:orangered"/></td>
                <td><input id="img_add" type="button" value="增加"
                    class="layui-btn layui-btn-primary layui-btn-mini" style="color:green"/></td>
                <%--<td><asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Admin/images/dele.png"
                      OnClick="ImageButton2_Click" OnClientClick="return confirm('确定执行删除？')"  /></td>--%>
                <td><input id="img_delete" type="button" value="删除" 
                    class="layui-btn layui-btn-primary layui-btn-mini"style="color:red"/></td>
                <%--<td>删除<img id="img_dele" src="images/153-Delete.png" width="20" height="20" /></td>--%>
            </tr>
            </table>
            <input id="btn_dqgs" type="button" value="导出当前格式" 
                class="layui-btn layui-btn-primary layui-btn-radius" style="width:auto"/>
            <input id="btn_dcxx" type="button" value="导出详细格式" 
                class="layui-btn layui-btn-primary layui-btn-radius"/>
            <input id="btn_uctj" type="button" value="导出用户使用记录"
                class="layui-btn layui-btn-primary layui-btn-radius" style="display:none"/>&nbsp;
<%--            <input id="btn_sjtj" type="button" value="数据统计" class="layui-btn layui-btn-primary layui-btn-mini"/>--%>
        </div>
        <div id="gv1"  style="overflow: auto; overflow-x: auto; overflow-y: hidden;
            white-space:nowrap;width:auto; height:400px; overflow:auto;" >
        <asp:GridView ID="GridView1" runat="server"  AllowPaging="True" 
            OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated"  
            OnPageIndexChanged="GridView1_PageIndexChanged" 
            OnPageIndexChanging="GridView1_PageIndexChanging" 
            OnRowCancelingEdit="GridView1_RowCancelingEdit" 
            OnRowEditing="GridView1_RowEditing" CssClass="inner layui-table"
            OnRowUpdating="GridView1_RowUpdating" EnableTheming="True" OnDataBound="GridView1_DataBound"
             meta:resourcekey="GridView1Resource1" EmptyDataText="没有找到该数据" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" >
            <Columns>
                <asp:CommandField ButtonType="Link"  HeaderText="" ShowHeader="True" SelectText="详情" 
                    ShowSelectButton="True">
                    <HeaderStyle Width="1%" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CommandField>
                    <asp:TemplateField meta:resourcekey="TemplateFieldResource2" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <input id="cbItem" type="checkbox"
                                onchange="cbItem_CheckedChanged(this)" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <input id="cbPage" type="checkbox"
                                 onchange="SelectAll(this)" />
                        </HeaderTemplate>
                        <HeaderStyle Width="40px"></HeaderStyle>
                    </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <table>
                 <tr><td>
                 </td>
                 </tr>
                </table>
            </EmptyDataTemplate>
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
                        跳到<asp:DropDownList ID="listPageCount" runat="server" AutoPostBack="True"  
                            OnSelectedIndexChanged="listPageCount_SelectedIndexChanged" 
                            CssClass="touming">
                          </asp:DropDownList>
                        显示条数:<asp:DropDownList ID="pageNum" runat="server" AutoPostBack="True" 
                            Width="50px" CssClass="touming"
                             OnSelectedIndexChanged="PageNum_SelectedIndexChanged" >
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                    <asp:ListItem>100</asp:ListItem>
                                 </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </PagerTemplate>
         </asp:GridView>
            <br />
            </div>
    </div>

        </div>
     <div id="delete" style="padding-left:10px;text-align:center" class="none">
         <label>您确定删除吗?</label><br/>
         您确定删除id为:<asp:TextBox ID="TextBox10" runat="server" ></asp:TextBox>的数据吗?<br/>
         <asp:Button ID="btn_delete" runat="server" Text="确定" OnClick="btn_delete_Click" />
     </div>
    <div id="dc_utj_div" class="none" style="padding-left:10px;text-align:center">
         开始日:<input id="s_date_utj" class="laydate-icon-dahong"  
             onclick="laydate({ istime: true, format: 'YYYY-MM-DD hh:mm:ss',min: laydate.now(-7),max: laydate.now(0) })" runat="server"><br />
         结束日:<input id="e_date_utj" class="laydate-icon-dahong"  
             onclick="laydate({ istime: true, format: 'YYYY-MM-DD hh:mm:ss',min: laydate.now(-7),max: laydate.now(0) })" runat="server"><br />
        <asp:Button ID="Button9" runat="server" Text="导出" OnClick="Button9_Click1" 
            CssClass="mt10 layui-btn layui-btn-mini" />
    </div>
    <div id="quedingdaochu" style="text-align:center; font-family:微软雅黑;" class="none">
        <div style="text-align:center">
        当前选择的是[<%=currentType %>]<br/>
            若要更改,请点击查询选择[分类查询]<br/>
            勾选以下方框来选择需要导出的列<br/>
                    确定继续吗?<br />
        <asp:Button ID="Button12" runat="server" Text="确定" OnClick="Button12_Click" 
            CssClass="layui-btn  layui-btn-mini" />
            </div>
        <div class="layui-input-inline">
        <asp:CheckBoxList ID="CheckBoxList4" runat="server" CssClass=" cbl_h txtleft">
            <asp:ListItem Selected="True">id</asp:ListItem>
            <asp:ListItem Selected="True">标题</asp:ListItem>
            <asp:ListItem Selected="False">图片</asp:ListItem>
            <asp:ListItem Selected="False">类型</asp:ListItem>
            <asp:ListItem Selected="True">一级类型</asp:ListItem>
            <asp:ListItem Selected="True">二级类型</asp:ListItem>
            <asp:ListItem Selected="True">三级类型</asp:ListItem>
            <asp:ListItem Selected="True">四级类型</asp:ListItem>
            <asp:ListItem Selected="True">五级类型</asp:ListItem>
        </asp:CheckBoxList>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" CssClass=" cbl_h txtleft">
            <asp:ListItem Selected="True">信道1</asp:ListItem>
            <asp:ListItem Selected="True">优先1</asp:ListItem>
            <asp:ListItem Selected="False">原标题1</asp:ListItem>
            <asp:ListItem Selected="True">路径1</asp:ListItem>
            <asp:ListItem Selected="True">状态1</asp:ListItem>
            <asp:ListItem Selected="False">清晰度1</asp:ListItem>
            <asp:ListItem Selected="False">广告1</asp:ListItem>
        </asp:CheckBoxList>
        <asp:CheckBoxList ID="CheckBoxList2" runat="server" CssClass="cbl_h txtleft">
            <asp:ListItem Selected="True">信道2</asp:ListItem>
            <asp:ListItem Selected="True">优先2</asp:ListItem>
            <asp:ListItem Selected="False">原标题2</asp:ListItem>
            <asp:ListItem Selected="True">路径2</asp:ListItem>
            <asp:ListItem Selected="True">状态2</asp:ListItem>
            <asp:ListItem Selected="False">清晰度2</asp:ListItem>
            <asp:ListItem Selected="False">广告2</asp:ListItem>
        </asp:CheckBoxList>
        <asp:CheckBoxList ID="CheckBoxList3" runat="server" CssClass="cbl_h txtleft">
            <asp:ListItem Selected="True">信道3</asp:ListItem>
            <asp:ListItem Selected="True">优先3</asp:ListItem>
            <asp:ListItem Selected="False">原标题3</asp:ListItem>
            <asp:ListItem Selected="True">路径3</asp:ListItem>
            <asp:ListItem Selected="True">状态3</asp:ListItem>
            <asp:ListItem Selected="False">清晰度3</asp:ListItem>
            <asp:ListItem Selected="False">广告3</asp:ListItem>
        </asp:CheckBoxList>
            </div>
        <br/>
        <br/>
    </div>
    <div id="admin_yz_div" style="padding-left:10px; text-align:center; font-family:微软雅黑" class="none">
        密码:<asp:TextBox ID="TextBox5" runat="server" TextMode="Password" ></asp:TextBox><br /><br />
        <asp:Button ID="Button11" runat="server" Text="确定" OnClick="Button11_Click"  />
    </div>
    <div id="zjselect_div" style="margin-left:10px" class="none">
        <asp:Button ID="zj_selectmh" runat="server" Text="查询" OnClick="zj_selectmh_Click" 
            style="height: 21px" />
            模糊查询: <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="zj_selectsd" runat="server" Text="查询" OnClick="zj_selectsd_Click"/>
            锁定查询:
            <asp:Label ID="Label4" runat="server" Text="表" ForeColor="Red"></asp:Label>
            <asp:Label ID="Label1" runat="server" Text=" 列"></asp:Label>
        <select id="Select1" onchange="select_change()" runat="server">
        </select>
            <asp:Label ID="Label2" runat="server" Text=" 为" ></asp:Label>
            <asp:TextBox ID="TextBox7" runat="server" Width="96px"></asp:TextBox>
            <span id="Span1" runat="server">
                <asp:Label ID="Label3" runat="server" Text="~" ></asp:Label>
                <asp:TextBox ID="TextBox8" runat="server" Width="54px"></asp:TextBox>
            </span>
    </div>
    <div id="manyselect" style="margin-left:10px;width:auto;" class="none">
        <div style="text-align:center">
            <input id="admin_show" type="button" value="多条件查询" 
                class="layui-btn layui-btn-big layui-btn-primary layui-btn-radius" 
                onclick="user_manyselect.style.display = ''; admin_manyselect.style.display='none'"  />
            <input id="user_show" type="button" value="高级" 
                class="layui-btn layui-btn-big layui-btn-primary layui-btn-radius" />
        </div>
        <div id="admin_manyselect" style="display:none" >
            <div id="v_div" style="width:280px;float:left">
                Video:<br />
                Id:<input id="Id" type="text" style="width:150px" /><br />
                Tid:<input id="Tid" type="text"style="width:150px" /><br />
                Title:<input id="Title" type="text" style="width:150px"/><br />
                Cover:<input id="Cover" type="text" style="width:150px"/><br />
                Describe:<input id="Describe" type="text"style="width:150px" /><br />
                CreateDate:<input id="CreateDate" type="text" style="width:150px"/><br />
                Sort:<input id="Sort" type="text" style="width:150px"/><br />
                State:<input id="State_txt" type="text" style="width:150px"/><br />
                Definition:<input id="Definition_txt" type="text"style="width:150px" /><br />
                Advertisement:<input id="Advertisement_txt" type="text" style="width:150px"/><br />
            </div>
            <div id="u_div" style="width:280px;float:left">
                VideoUrl:<br />
                Id:<input id="uId" type="text" style="width:150px" /><br />
                Vid:<input id="uTid" type="text"style="width:150px" /><br />
                Source:<input id="uTitle" type="text" style="width:150px"/><br />
                Path:<input id="uCover" type="text" style="width:150px"/><br />
                TempPath:<input id="uDescribe" type="text"style="width:150px" /><br />
                State:<input id="uCreateDate" type="text" style="width:150px"/><br />
                Sort:<input id="uSort" type="text" style="width:150px"/><br />
                CreateDate:<input id="uState_txt" type="text" style="width:150px"/><br />
                Definition:<input id="uDefinition_txt" type="text"style="width:150px" /><br />
                Advertisement:<input id="uAdvertisement_txt" type="text" style="width:150px"/><br />
            </div>
            <div id="t_div" style="width:280px;float:left">
                VideoType:<br />
                Id:<input id="tId" type="text" style="width:150px" /><br />
                Tid:<input id="tTid" type="text"style="width:150px" /><br />
                Title:<input id="tTitle" type="text" style="width:150px"/><br />
                Cover:<input id="tCover" type="text" style="width:150px"/><br />
                Sort:<input id="tSort" type="text" style="width:150px"/><br />
                State:<input id="tState_txt" type="text" style="width:150px"/><br />
            </div>
            <asp:Button ID="admin_ms_btn" runat="server" Text="查询"  CssClass=" mr10"  />
        </div>
        <div id="user_manyselect" style="width:500px; margin-right: 0px;">
            不想查的可以不填
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style11">信道名称:</td>
                    <td class="auto-style12">
                        <asp:DropDownList ID="ddl_manys_xdm" runat="server" style="width:154px">
                            <asp:ListItem>请选择</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="auto-style11">标题:</td>
                    <td class="auto-style12"><input id="user_spmc" type="text" style="width:150px" /></td>
                </tr>
                 <tr>
                    <td class="auto-style11">状态:</td>
                    <td class="auto-style12"><input id="user_spzt" type="text" style="width:150px" /></td>
                </tr> 
                <tr>
                    <td class="auto-style11">路径:</td>
                    <td class="auto-style12"><input id="user_xdlj" type="text" style="width:150px" /></td>
                </tr>
                <tr>
                    <td class="auto-style11">广告:</td>
                    <td class="auto-style12"><asp:DropDownList ID="ddl_manys_gg" style="width:154px" 
                        runat="server">
                            <asp:ListItem>请选择</asp:ListItem>
                         <asp:ListItem Value="0">0</asp:ListItem>
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="auto-style11">优先级:</td>
                    <td class="auto-style12"><input id="user_xdyxj" type="text"  style="width:150px" /></td>
                </tr>
                <tr>
                    <td class="auto-style11">清晰度:</td>
                    <td class="auto-style12"><asp:DropDownList ID="ddl_manys_qxd" style="width:154px" 
                        runat="server">
                            <asp:ListItem>请选择</asp:ListItem>
                         <asp:ListItem Value="0">0</asp:ListItem>
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        </asp:DropDownList></td>
                </tr> 
                <tr>
                    <td class="auto-style11">信道建立时间:</td>
                    <td class="auto-style12"><input id="xdjlDate1" style="float:left;width:132px;" 
                        class="laydate-icon-dahong"  
                        onclick="laydate({ istime: true, format: 'YYYY-MM-DD hh:mm' })" runat="server">
                        <label style="float:left;">~</label>
                        <input id="xdjlDate2" style="float:left;width:132px;" class="laydate-icon-dahong"  
                            onclick="    laydate({ istime: true, format: 'YYYY-MM-DD hh:mm' })" 
                            runat="server"></td>
                </tr>
                <tr>
                    <td class="auto-style11">视频建立时间:</td>
                    <td class="auto-style12"><input id="spjlDate1" style="float:left;width:132px;" 
                        class="laydate-icon-dahong"  
                        onclick="laydate({ istime: true, format: 'YYYY-MM-DD hh:mm' })" runat="server">
                        <label style="float:left;">~</label>
                        <input id="spjlDate2" style="float:left;width:132px;" class="laydate-icon-dahong"  
                            onclick="    laydate({ istime: true, format: 'YYYY-MM-DD hh:mm' })" 
                            runat="server"></td>
                </tr> 
                <tr>
                    <td class="auto-style11"></td>
                    <td class="auto-style12">多重查询暂不支持类型,查询类型请点选择分类查询</td>
                </tr>
                <tr>
                    <td class="auto-style11">
                        <select id="select_table" runat="server">
                            <option>请选择要查询的表</option>
                            <option>视频表</option>
                            <option>信道表</option>
                            <option>类型表</option>
                        </select></td>
                    <td class="auto-style12"><asp:Button ID="user_ms_btn" runat="server" Text="查询" 
                        CssClass=" txtrig layui-btn layui-btn-primary layui-btn-mini" 
                        OnClick="user_ms_btn_Click" OnClientClick="return checkselecttable()"/></td>
                </tr>
            </table>
            <label style="font-size:30px;color:#ff0000">尚未成功,高难度...正在研究...</label>
        </div>
    </div>
    <div id="dqgsqd" class="none" style="text-align:center">
        <div style="margin-bottom:10px">
        确定导出当前表格中数据?
            </div>
        <asp:Button ID="Button5" runat="server" Text="确定" OnClick="Button5_Click" CssClass="layui-btn  layui-btn-mini" />
 
    </div>
         <div class="none">
            <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
             </div>
             <div id="div_updateqd" style="margin-top:10px; text-align:center;display:none">
                <div style="margin-bottom:10px">
                您确定修改吗?
                    </div>
                <asp:Button ID="btn_update" runat="server" Text="确定" OnClick="btn_update_Click" CssClass="layui-btn layui-btn-small" />
        </div>
        <script type="text/javascript" src="js/jquery-1.11.3.js"></script>
    <script type="text/javascript" src="js/layer/layer.js" ></script>
    <script>
        function gofullinfo(vindex)
        {
            layer.open({
                type: 2,
                title: false,
                shadeClose:false,
                shade: false,
                maxmin: true, //开启最大化最小化按钮
                area: ['1100px', '450px'],
                content: 'fullInfo.aspx?index=' + vindex
            });
        }
        
     </script>
        </form>
        <script type="text/javascript" src="js/jquery-1.11.3.js"></script>
    <script type="text/javascript" src="js/layer/layer.js" ></script>
    <script>
        var lm;
        function closeloding() {
            layer.close(lm);
        }
        function SelectAll(aControl) {
            var tempControl = aControl;
            var isChecked = tempControl.checked;
            elem = tempControl.form.elements;
            for (i = 0; i < elem.length; i++)
                if (elem[i].type == "checkbox" && elem[i].id != tempControl.id) {
                    if (elem[i].checked != isChecked) {
                        elem[i].click();
                    }
                }
        }
        function checkupdatenull() {
            if (num <= 0) {
                layer.alert('请勾选下方需要修改的方框', {
                    title: false,
                    icon: 2,
                    shadeClose: true,
                    skin: 'layer-ext-moon'
                });
                return false;
            } else {
                return true;
            }
        }
        var num = 0;
        function cbItem_CheckedChanged(aControl) {
            var isChecked = aControl.checked;
            if (isChecked) {
                num++;
            } else {
                num--;
            }
        }
        var idjihe = new Array();
        var n = 0;
        function getId(row_index) {
            if (window.event.srcElement.id == "cbItem") {
                var grid_view = document.getElementById('<%=GridView1.ClientID %>');
                var rows = grid_view.rows;
                var personID;
                personID = rows[row_index].cells[2].innerHTML;
                if (window.event.srcElement.checked) {
                    if (personID != "undefined") {
                        idjihe[n] = personID; n++;
                    }
                } else {
                    removeByValue(idjihe, personID);
                    n--;
                }

            }
        }
        function isok() {
            layer.open({
                type: 1,
                title: "确定修改?",
                area: ['300px', '130px'],
                shadeClose: true,
                content: $("#div_updateqd")
            });
        }
        function removeByValue(arr, val) {
            for (var j = 0; j < arr.length; j++) {
                if (arr[j] == val) {
                    arr.splice(j, 1);
                    break;
                }
            }
        }
        $(function () {
            $("#btn_dqgs").click(function () {
                if (checkqx("export")) {
                    layer.open({
                        type: 1,
                        area: ['200px', '110px'],
                        shadeClose: true, //点击遮罩关闭
                        content: $("#dqgsqd")
                    });
                }
            })
            $("#user_show").click(function () {
                layer.prompt({ title: '输入任何口令，并确认', formType: 1 }, function (pass, index) {
                    layer.close(index);
                });
            })
            $("#btn_dcxx").click(function () {
                if (checkqx("export")) {
                    layer.open({
                        type: 1,
                        area: ['400px', '410px'],
                        shadeClose: true, //点击遮罩关闭
                        content: $("#quedingdaochu")
                    });
                }
            })

            $("#test").click(function () {
                layer.open({
                    type: 2,
                    title: '很多时候，我们想最大化看，比如像这个页面。',
                    shade: false,
                    maxmin: true, //开启最大化最小化按钮
                    area: ['893px', '600px'],
                    content: 'WebForm1.aspx'
                });
            });
            $("#btn_uctj").click(function () {
                if (checkqx("export")) {
                    layer.alert("暂时关闭");
                    //layer.open({
                    //    type: 1,
                    //    area: ['300px', '150px'],
                    //    shadeClose: true, //点击遮罩关闭
                    //    content: $("#dc_utj_div")
                    //});
                }
            });
            $("#dtjselect_btn").click(function () {
                layer.open({
                    type: 1,
                    area: ['600px', '550px'],
                    shadeClose: true, //点击遮罩关闭
                    content: $("#manyselect")
                });
            })
            $("#img_add").click(function () {
                if (checkqx("add")) {
                    window.location = "/Admin/AddResource.aspx";
                }
            });
            var tmp = new Array();
            var m = 0;
            $("#img_update").click(function () {
                if (checkqx("update")) {
                    if (num <= 0) {
                        document.getElementById("<%=TextBox9.ClientID%>").value = "";
                        layer.alert('请勾选下方需要修改的方框', {
                            title: false,
                            icon: 0,
                            shadeClose: true,
                            skin: 'layer-ext-moon'
                        });
                    } else {
                        var temps = "";
                        for (m = 0; m < idjihe.length; m++) {
                            temps += idjihe[m] + ",";
                        }
                        document.getElementById("<%=TextBox9.ClientID%>").value = temps; m = 0;
                        document.getElementById("<%=Label7.ClientID%>").textContent = "您当前选中" + num + "条数据";
                        document.getElementById("<%=Label8.ClientID%>").textContent = "列名:";
                        layer.open({
                            type: 1, area: ['500px', '320px'],
                            shadeClose: true,
                            content: $('#updata_PL')
                        });
                    }

                }
            })
            $("#img_delete").click(function () {
                if (checkqx("delete")) {
                    layer.prompt({ title: '输入任何口令，并确认', formType: 1 }, function (pass, index) {
                        if (pass == "admin") {
                            if (num <= 0) {
                                document.getElementById("<%=TextBox10.ClientID%>").value = "";
                                layer.alert('请勾选下方需要删除的方框', {
                                    title: false,
                                    icon: 0,
                                    shadeClose: true,
                                    skin: 'layer-ext-moon'
                                });
                            } else {
                                var temps = "";
                                for (m = 0; m < idjihe.length; m++) {
                                    temps += idjihe[m] + ",";
                                }
                                document.getElementById("<%=TextBox10.ClientID%>").value = temps; m = 0;
                                layer.open({
                                    type: 1,
                                    area: ['400px', '250px'],
                                    shadeClose: true, //点击遮罩关闭
                                    content: $("#delete")
                                });
                            }
                        } else {
                            layer.msg("口令错误");
                        }
                    });
                }
            })
        });
        function checkqx(type) {
            var state = "<%=state%>";
            var export1 = "<%=export%>";
            var add = "<%=add%>";
            var delete1 = "<%=delete%>";
            var update = "<%=update%>";
            if (state != 1) {
                layer.alert('您的账号异常!', {
                    icon: 5, shadeClose: true, title: false,
                    skin: 'layer-ext-moon'
                });
                return false;
            } else {
                var temp;
                switch (type) {
                    case "add": temp = add; break;
                    case "delete": temp = delete1; break;
                    case "update": temp = update; break;
                    case "export": temp = export1; break;
                }
                if (temp != 1) {
                    layer.alert('您的账号没有权限!', {
                        icon: 5, shadeClose: true, title: false,
                        skin: 'layer-ext-moon'
                    });
                    return false;
                } else {
                    return true;
                }
            }
        }
        
        
        function checks1() {
            var txt = document.getElementById("<%=TextBox2.ClientID %>").value;
            if (txt != "") {
                lm = layer.load(1, {
                    shade: [0.1, '#fff'] //0.1透明度的白色背景
                });
                return true;
            } else {
                layer.alert('请输入搜索条件', {
                    icon: 0, shadeClose: true, title: false,
                    skin: 'layer-ext-moon'
                });
                return false;
            }
        }
        function checks2() {
            var txt = document.getElementById("<%=TextBox1.ClientID %>").value;
            idjihe.length = 0;
            if (txt != "") {
                lm = layer.load(1, {
                    shade: [0.1, '#fff'] //0.1透明度的白色背景
                });
                return true;
            } else {
                layer.alert('请输入搜索条件', {
                    icon: 0, shadeClose: true, title: false,
                    skin: 'layer-ext-moon'
                });
                return false;
            }
        }
        function UserAddVerify() {

            return true;
        }
        function checkselecttable() {
            var table = document.getElementById("select_table").value;
            if (table == "请选择要查询的表") {
                layer.alert('在按钮左侧,请选择要查询的表', {
                    icon: 0, shadeClose: true, title: false,
                    skin: 'layer-ext-moon'
                });
                return false;
            } else
                return true;
        }
    </script>
    </body>
    
</html>


