<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NavigationTest.aspx.cs" Inherits="Web.Admin.NavigationTest" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>e家亲后台</title>
    <link rel="stylesheet" href="js/layui/css/layui.css" />
    <style>
        html,body,#head,#items{
		    margin: 0px;
		    padding: 0px;
            width: 100%;
		    overflow: hidden;
	    }
	    #head{
		    height: 12%;
		    background-image: url('img/indexe/head.png');
		    background-repeat: no-repeat;
		    background-size: 100% 100%;
            float: left;
            position: absolute;
	    }
	    #left{
		    width: 13%;
		    height: 88%;
		    background-image: url('img/indexe/left-back.png');
		    background-repeat: no-repeat;
		    background-size: 100% 100%;
		    margin-top: 1px;
            float: left;
            position: absolute;
            top: 12.1%;
	    }
        #center{
            width: 87%;
		    height: 88%;
            float: right;
            position: absolute;
            top: 12.1%;
            right: 0px;
        }
	    #items{
		    list-style: none;
	    }
	    .item{
		    width: 100%;
		    border-bottom: 1px solid white;
	    }
	    dl,dt,dd{
		    padding: 0px;
		    margin: 0px;
	    }
	    dd{
		    display: none;
	    }
        #ifr { 
            background-color: #efefef;
            
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div id="head"></div>
	    <div id="panle">
		    <div id="left">
			    <ul id="items">
				    <li class="item"><img src="img/indexe/sy-on.png" width="100%" height="8%" name="sy" id="home"/></li>
				    <li class="item"><dl>
					    <dt><img src="img/indexe/zygl.png" width="100%" height="8%" name="zygl"/></dt>
					    <dd><img src="img/indexe/qbzy.png" width="100%" height="8%" name="qbzy" id="alls"/></dd>
					    <dd><img src="img/indexe/分割线.png" width="100%"/></dd>
					    <dd><img src="img/indexe/zjzy.png" width="100%" height="8%" name="zjzy" id="adds"/></dd>
					    <dd><img src="img/indexe/分割线.png" width="100%"/></dd>
					    <dd><img src="img/indexe/yhtj.png" width="100%" height="8%" name="yhtj" id="utj"/></dd>
				    </dl></li>
				    <li class="item"><dl>
					    <dt><img src="img/indexe/zhgl.png" width="100%" height="8%" name="zhgl"/></dt>
					    <dd><img src="img/indexe/xgmm.png" width="100%" height="8%" name="xgmm" id="updatepwd"/></dd>
					    <dd><img src="img/indexe/分割线.png" width="100%"/></dd>
					    <dd><img src="img/indexe/zczh.png" width="100%" height="8%" name="zczh" id="regedit"/></dd>
					    <dd><img src="img/indexe/分割线.png" width="100%"/></dd>
					    <dd><img src="img/indexe/ggqx.png" width="100%" height="8%" name="ggqx" id="updateqx"/></dd>
				    </dl></li>
				    <li class="item"><dl>
					    <dt><img src="img/indexe/czjl.png" width="100%" height="8%" name="czjl"/></dt>
					    <dd><img src="img/indexe/gly.png" width="100%" height="8%" name="gly" id="admino"/></dd>
				    </dl></li>
				    <li class="item"><img src="img/indexe/tc.png" width="100%" height="8%" name="tc" id="exit"/></li>
			    </ul>
		    </div>
		    <div id="center">
			    <iframe id="ifr" frameborder="no" allowtransparency="0"  scrolling="auto" style="width:100%;height:100%;" src="Home.aspx"></iframe>
		    </div>
	    </div>
    </form>
<!--<ul class="layui-nav">
  <li class="layui-nav-item"><a href="javascript:void(0);" id="home">首页</a></li>
  <li class="layui-nav-item">
    <a href="javascript:;">资源管理</a>
    <dl class="layui-nav-child site-demo-button">
  <dd><a href="javascript:void(0);" id="alls">全部资源</a></dd>
  <dd><a href="javascript:void(0);" id="adds">增加资源</a></dd>
  <dd><a href="javascript:void(0);" id="utj">用户统计</a></dd>
    </dl>
  </li>
  <li class="layui-nav-item">
    <a href="javascript:;">账号管理</a>
    <dl class="layui-nav-child">
      <dd><a href="javascript:void(0);" id="updatepwd">修改密码</a></dd>
      <dd><a href="javascript:void(0);" id="regedit">注册账号</a></dd>
      <dd><a href="javascript:void(0);" id="updateqx">更改账号权限</a></dd>
    </dl>
  </li>
  <li class="layui-nav-item">
    <a href="javascript:;">操作记录</a>
    <dl class="layui-nav-child site-demo-button">
      <dd><a href="javascript:void(0);" id="admino">管理员操作记录</a></dd>
    </dl>
  </li>
    <li class="layui-nav-item">
        <a href="javascript:;" >检测视频失效</a>
        <dl class="layui-nav-child">
          <dd><a href="javascript:void(0);" id="TestVideoPlay">检测优酷视频</a></dd>
          <dd><a href="javascript:void(0);" id="guanbi">关闭优酷视频</a></dd>
          <dd><a href="javascript:void(0);" id="dakai">打开优酷视频</a></dd>
        </dl>
    </li>
      <li class="layui-nav-item"><a href="/Admin/loginout.aspx" id="exit">退出
          </a></li>
</ul>
  <div class="layui-tab-content">
      <iframe id="ifr" class="layui-tab-item layui-show layui-show" frameborder="no" allowtransparency="0"  scrolling="auto" style="width:100%;height:600px;" src="Home.aspx"></iframe>
  </div>-->
    <script src="js/jquery-1.11.3.js"></script>
    <script src="js/layer/layer.js"></script>
    <script src="js/layui/layui.js"></script>
<script>
    window.onload = function () {
        
    }
    $("#alls").click(function () {
        img_click();
        $(this).attr('src', 'img/indexe/qbzy-on.png')
        document.getElementById("ifr").src = "index.aspx";
        //ifr.src = "index.aspx";
    });
    $("#home").click(function () {
        img_click();
        $(this).attr('src', 'img/indexe/sy-on.png')
        document.getElementById("ifr").src = "Home.aspx";
        //ifr.src = "Home.aspx";
    });
    $("#adds").click(function () {
        img_click();
        $(this).attr('src', 'img/indexe/zjzy-on.png')
        document.getElementById("ifr").src = "AddResource.aspx";
        //ifr.src = "AddResource.aspx";
    });
    $("#utj").click(function () {
        img_click();
        $(this).attr('src', 'img/indexe/yhtj-on.png')
        //document.getElementById("ifr").style.height = "1300px";
        document.getElementById("ifr").src = "AllLine.aspx";
        //ifr.src = "AllLine.aspx";
    });
    $("#admino").click(function () {
        img_click();
        $(this).attr('src', 'img/indexe/gly-on.png')
        document.getElementById("ifr").src = "DownLoad.aspx";
        //ifr.src = "DownLoad.aspx";
    });
    $("#exit").click(function () {
        location.href = 'loginout.aspx'
    });
    $(function () {
        $("#regedit").click(function () {
            img_click();
            $(this).attr('src', 'img/indexe/zczh-on.png')
            layer.open({
                type: 2,
                title: '注册账号',
                shade: false,
                maxmin: false, //开启最大化最小化按钮
                anim: 0,
                area: ['380px', '510px'],
                content: 'titleOperation/Register.aspx'
            });
        });
        $("#updateqx").click(function () {
            img_click();
            $(this).attr('src', 'img/indexe/ggqx-on.png')
            layer.open({
                type: 2,
                title: '更改账号权限',
                shade: false,
                maxmin: false, //开启最大化最小化按钮
                area: ['600px', '600px'],
                content: 'titleOperation/ChangeAccountQX.aspx'
            });
        });
        $("#updatepwd").click(function () {
            img_click();
            $(this).attr('src', 'img/indexe/xgmm-on.png')
            layer.open({
                type: 2,
                title: '修改密码',
                shade: false,
                maxmin: false, //开启最大化最小化按钮
                area: ['330px', '320px'],
                content: 'titleOperation/ChangePwd.aspx'
            });
        })
    });
    $('[name=zygl]').click(function () {
        $(this).parent().parent('dl').find('dd').fadeToggle(300)
    });

    $('[name=zhgl]').click(function () {
        $(this).parent().parent('dl').find('dd').fadeToggle(300)
    });

    $('[name=czjl]').click(function () {
        $(this).parent().parent('dl').find('dd').fadeToggle(300)
    });
    function img_click() {
        $('[name=sy]').attr('src', 'img/indexe/sy.png')
        $('[name=qbzy]').attr('src', 'img/indexe/qbzy.png')
        $('[name=zjzy]').attr('src', 'img/indexe/zjzy.png')
        $('[name=yhtj]').attr('src', 'img/indexe/yhtj.png')
        $('[name=xgmm]').attr('src', 'img/indexe/xgmm.png')
        $('[name=zczh]').attr('src', 'img/indexe/zczh.png')
        $('[name=ggqx]').attr('src', 'img/indexe/ggqx.png')
        $('[name=gly]').attr('src', 'img/indexe/gly.png')
    }
</script>
</body>
    
</html>

