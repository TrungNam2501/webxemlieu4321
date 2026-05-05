<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="XemLieuDauRa.aspx.cs" Inherits="KendaWeb.View.XemLieuDauRaHCMoi.XemLieuDauRa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 0 20px;">
        <div class="container" style="height: 70px; width: 100%">
            <div class="BBflex">
                <asp:DropDownList ID="Drmay" runat="server" CssClass="form-control" Style="width: 170px; text-align: center; margin-right: 10px">
                    <asp:ListItem Selected="True" Value="">----Chọn máy----</asp:ListItem>
                    <asp:ListItem Value="198.1.8.17"> Máy -1 mới </asp:ListItem>
                    <asp:ListItem Value="198.1.8.18"> Máy -9 mới </asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label1" runat="server" Text="Từ ngày : "></asp:Label>
                <asp:TextBox ID="txtTuNgay" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Chọn ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="Đến ngày : "></asp:Label>
                <asp:TextBox ID="txtDenNgay" runat="server" autocomplete="off" AutoPostBack="true" OnTextChanged="txtDenNgay_TextChanged" CssClass="form-control" placeholder="---Chọn ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
                <asp:Button ID="btnXemLieu" runat="server"  Text="Xem liệu"  CssClass="btn btn-danger" Style="margin-right: 10px" OnClientClick="showLoading();" />
            </div>
        </div>

        <table id="tblLoading" style="display: none; position: absolute; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
            <tr>
                <td style="vertical-align: middle;">
                    <div id="loading">
                        <div class="container1">
                            <div class="ring"></div>
                            <div class="ring"></div>
                            <div class="ring"></div>
                            <p>Xin vui lòng đợi.......</p>
                        </div>
                    </div>
                </td>
            </tr>
        </table>

        <div>
            <table id="tblMessages" style="display: none; position: absolute; top: 0; left: 0; width: 100%; height: 100%;">
                <tr>
                    <td>
                        <table id="tblMessages1" style="text-align: center; font-size: 24px; font-weight: bold; background-color: #b6d7ff; margin-left: auto; margin-right: auto; box-shadow: #999 -4px -3px 10px 4px; border-radius: 5px; width: 500px; height: 200px;">
                            <tr>
                                <td style="height: 35px; padding-left: 10px; color: black;">Thông báo
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #b5b6dd; vertical-align: top; padding: 10px; text-align: center; margin-top: auto; margin-bottom: auto;">
                                    <%--<asp:Label ID="lblThongbao" runat="server" Style="font-family: Arial; font-weight: bold; font-size: 16px; color: black" Text=""></asp:Label>--%>
                                    <asp:Label ID="lblThongbao" runat="server"  Style="font-family: Arial; font-weight: bold; font-size: 16px; color: black" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 1px; background-color: #b6d7ff; text-align: right; padding: 5px; border-radius: 0 0 5px 5px;">
                                    <button type="button" class="btn btn-danger" onclick="closeMessages();">Đóng</button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
     <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtTuNgay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
            $("#<%=txtDenNgay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
        });
     </script>
</asp:Content>
