<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Web.Admin.login" %>

<!DOCTYPE html>
<html lang=en>
    <head>
        <title>e家亲后台</title>
	    <meta charset="UTF-8">
	    <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <link rel="SHORTCUT ICON" href="img/favicon.ico">
        <style>
            html, body, div{
                height: 100%;
                width: 100%;
            }
	        body{
		        background-image: url('img/index/shuaideyibi.png');
		        background-repeat: no-repeat;
		        width: 100%;
                height: 100%;
		        background-size:100% 120%;
		        -moz-background-size:100% 100%;
		        overflow: hidden;
		        position: relative;
	        }

	        #loginPanle{
                background-color: white;
		        width: 20%;
                height: 54%;
		        background-size:100% 100%;
		        position: relative;
		        top: 160px;
		        left: 65%;
                border-radius: 5px;
	        }
	        .input{
		        border: 2px solid #D3D3D3;
		        border-radius: 5px;
		        width: 80%;
		        height: 33px;
		        position: relative;
		        top: 80px;
		        left: 10%;
                background-color: #DDDDDD;
	        }
	        .input input{
		        border: none;
		        border-radius: 5px;
		        width: 84%;
		        float: right;
		        height: 30px;
		        font-size: 1.1em;
	        }
	        .input img{
		        border-top-left-radius: 2px;
		        border-bottom-left-radius: 2px;
	        }
	        .submit{
		        width: 80%;
		        position: relative;
		        top: 30px;
		        left: 10%;
                	padding: 0px;
	        }
	        .submit button{
		        background-color: white;
		        border: none;
		        padding: 0px;
	        }
        </style>
    </head>
    <body>
        <div>
		    <div id='loginPanle'>
			    <form id="Login" name="Login" action="?Action=Login" method="post">
                    <div style="float: left; position: absolute; top: 10%; left: 27%;"><img src="img/index/后台管理系统@2x.png" /></div>
				    <div class="input">
					    <img src="img/index/id.png" width="16%"/>
					    <input type="text" name="TxbPid" placeholder="  请输入你的用户名"/>
				    </div>
				    <div style="height: 40px"></div>
				    <div class="input">
					    <img src="img/index/pwd.png" width="16%"/>
					    <input type="password" name="TxbPwd" placeholder="  请输入你的密码"/>
				    </div>
				    <div style="height: 100px"></div>
				    <div class="submit">
					    <button type="submit"><img src="img/index/login.png" width="100%" height="50px"/></button>
				    </div>
			    </form>
		    </div>
	    </div>
    </body>
</html>