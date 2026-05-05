<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Loggin.aspx.cs" Inherits="KendaWeb.Loggin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <asp:PlaceHolder runat="server">
     <%: Scripts.Render("~/bundles/jquery") %>
         <%: Scripts.Render("~/bundles/jquery_ui") %>
        <%: Scripts.Render("~/bundles/bootstrap") %>    
        <%: Scripts.Render("~/bundles/main") %>
    </asp:PlaceHolder>

    <webopt:bundlereference runat="server" path="~/Content/css" />
</head>
<body style="background-image:url('../image/aaa.png');background-size:cover; height:94vh; width:100%;">
    <form id="form1" runat="server">
        <div style="display:flex; align-items:center; justify-content:center;height:100vh;width:100%">
            <div style="border: 5px solid black; height: 200px; width:450px; background-color:whitesmoke">
              <%--  <div style="height:30%;width:100%; position:relative;">
                    <img style="height: 100px; width: auto; position: absolute; top: 10px; left: 50%; transform: translateX(-50%);" src="../image/logo.png" alt="logo" />
                </div>--%>
                 <div style="height:15%;width:100%;display:flex; align-items:center; justify-content:center;">
                     <asp:Label ID="Label1" Text="Đăng Nhập" Font-Size="24px" Font-Bold="true" runat="server" />
                </div>
               <div style="height:25%;width:100%;display:flex; align-items:center; justify-content:center;">
                   <p style="font-size:18px; color:black; font-weight:700; margin: 0; width:100px;padding-left:5px">Số thẻ: </p>
                   <asp:TextBox runat="server" ID="txtSoThe" CssClass="form-control" Style="width:70%;" />
                </div>
                 <div style="height:25%;width:100%;display:flex; align-items:center; justify-content:center;">
                   <p style="font-size:18px; color:black; font-weight:700; margin: 0; width:100px;padding-left:5px">Mật khẩu: </p>
                   <asp:TextBox runat="server" ID="txtPass" CssClass="form-control" TextMode="Password" Style="width:70%;" />
                </div>
                <div style="height:25%;width:100%;display:flex; align-items:center; justify-content:center;">
                    <asp:Button Text="Đăng nhập" ID="btnDangNhap" OnClick="btnDangNhap_Click" CssClass="btn btn-warning" runat="server" />
                </div>
                 <div style="height:10%;width:100%;display:flex; align-items:center; justify-content:center;">
                    <asp:Label ID="lbError" runat="server" />
                </div>    
            </div>
        </div>
    </form>
</body>
</html>
