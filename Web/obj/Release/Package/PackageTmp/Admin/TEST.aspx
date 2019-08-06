<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TEST.aspx.cs" Inherits="Web.Admin.TEST" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
<title>新建网页 1</title>
</head>

<body>   
    <form runat="server">

<div>  
      
        <fieldset>  
<legend>加密</legend>  
            <asp:FileUpload ID="FileUpload1" runat="server" />  
            <asp:Button ID="Button1" runat="server"  
                Text="加密提交数据" OnClick="Button1_Click" />  
  
        </fieldset>  
   <br />  
      
                <fieldset>  
<legend>解密</legend>  
  
                    <asp:FileUpload ID="FileUpload2" runat="server" />  
  
            <asp:Button ID="Button2" runat="server"  
                Text="解密提交数据" OnClick="Button2_Click"  />  
  
                </fieldset>  
    </div>  
        </form>
</body>

</HTML>
