<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Nhapkeore.aspx.cs" Inherits="KendaWeb.View.XemLieuBB.Nhapkeore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../Chosen/chosen.jquery.min.js"></script>
    <link href="../../Chosen/bootstrap-chosen.css" rel="stylesheet" />
    <div style="padding: 0 20px;">
        <div class="container" style="height: 70px; width: 100%">
            <div class="BBflex">
                <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Chọn mã keo RE :  "></asp:Label>
                <asp:DropDownList ID="drChonkeoRE" Style="width: 200px; height: 35px; margin-right: 10px; margin-left: 10px" CssClass="form-control" placeholder="Chọn keo RE" runat="server"></asp:DropDownList>

                <%--  <asp:TextBox ID="TxtNhaptenkeo" Style="width: 200px; height: 35px; margin-right: 10px" CssClass="form-control" placeholder="Nhập tên keo RE" runat="server"></asp:TextBox>--%>
                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Ngày bắt đầu : "></asp:Label>
                <asp:TextBox ID="txtbatdau" CssClass="form-control" onkeydown="return restrictCharacters(event)" MaxLength="10" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" runat="server"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Ngày kết thúc :"></asp:Label>
                <asp:TextBox ID="txtketthuc" CssClass="form-control" onkeydown="return restrictCharacters(event)" MaxLength="10" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" runat="server"></asp:TextBox>
                <asp:Label ID="lblsothe" Font-Bold="true" runat="server" Text=""></asp:Label>
                <asp:Button ID="Them" CssClass="btn btn-success" OnClick="Them_Click" OnClientClick="showLoading();" Style="margin-right: 10px; margin-left: 10px" runat="server" Text="Thêm mới" />
            </div>


        </div>
        <div class="container" style="height: 70px; width: 100%">
            <div class="BBflex">
                <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Nhập keo tìm kiếm :  "></asp:Label>
                <asp:TextBox ID="txtTimkiem" Style="width: 200px; height: 35px; margin-right: 10px" CssClass="form-control" placeholder="Tìm theo tên keo" runat="server"></asp:TextBox>
                <%--    <asp:TextBox  ID="TextBox1" Style="width: 200px; height: 35px; margin-right: 10px" onblur="checkTextbox()" AutoPostBack="true" OnTextChanged="txtTimkiem_TextChanged" CssClass="form-control" placeholder="Tìm theo tên keo" runat="server" ></asp:TextBox>--%>
                <asp:Button ID="btnTimkiem" runat="server" OnClick="btnTimkiem_Click" CssClass="btn btn-info" Style="margin-right: 10px; margin-left: 10px" Text="Tìm kiếm" />
            </div>
        </div>

        <div style="margin: 0 30px; height: 75vh; overflow: auto; border: 5px solid black;">
            <asp:GridView ID="gvKQRE" runat="server" OnRowCommand="gvKQRE_RowCommand"
                CssClass="table table-responsive table table-responsive table-bordered"
                Style="overflow: auto; overflow: hidden; border: groove; border-bottom: none; font-size: 18px; font-weight: bold; font-family: Arial; margin-bottom: 0;" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="NotSet" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                <HeaderStyle CssClass="header_girdview" />
                <RowStyle CssClass="row_css checkStatus" />
                <Columns>

                    <%--  <asp:BoundField DataField="ID" ItemStyle-HorizontalAlign="Center" HeaderText="ID" />--%>
                    <asp:BoundField DataField="Recipe_name" ItemStyle-HorizontalAlign="Center" HeaderText="Tên keo RE" />
                    <asp:BoundField DataField="Start_date" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày bắt dầu " />
                    <asp:BoundField DataField="ursno" ItemStyle-HorizontalAlign="Center" HeaderText="Số thẻ" HtmlEncode="false" />
                    <asp:BoundField DataField="End_date" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày kết thúc" HtmlEncode="false" />
                    <asp:BoundField DataField="Indat" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày thêm" HtmlEncode="false" />
                    <asp:BoundField DataField="Intime" ItemStyle-HorizontalAlign="Center" HeaderText="Giờ thêm" HtmlEncode="false" />
                    <asp:BoundField DataField="lock" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="jsbang status" HeaderText="Thông tin xuất" HtmlEncode="false" />
                    <asp:BoundField DataField="fixdat" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="jsbang" HeaderText="Giờ sửa" HtmlEncode="false" />

                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xóa <br> dữ liệu">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="../../image/remove.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btnxoa" CommandArgument='<%# Eval("Recipe_name")+","+ Eval("Indat")+","+ Eval("Intime")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
                                    <asp:Label ID="lblThongbao" runat="server" Style="font-family: Arial; font-weight: bold; font-size: 16px; color: black" Text=""></asp:Label>
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
            $("#<%=txtbatdau.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
            $("#<%=txtketthuc.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
        });
        function restrictCharacters(e) {
            // Lấy mã ASCII của kí tự được nhập vào
            var charCode = e.keyCode || e.which;
            // Kiểm tra nếu kí tự không phải là số và không phải là dấu "-"
            if (charCode != 189 && charCode != 45 && charCode != 8 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

    </script>
    <script>
        $('.checkStatus').each(function () {
            let trangthai = $(this).find(".status").html();

            if (trangthai == "&nbsp;") {
                $(this).find(".status").css({ 'font-weight': 'bold', 'color': '#FF0000', 'font-family': 'Arial' }).text("Keo không xuất")
            }
            if (trangthai == "N") {
                $(this).find(".status").css({ 'font-weight': 'bold', 'color': '#2456E4', 'font-family': 'Arial' }).text("Đã khóa")
            }

            if (trangthai == "Y") {
                $(this).find(".status").css({ 'font-weight': 'bold', 'color': 'green', 'font-family': 'Arial' }).text("Đã mở khóa");
            }




        });
    </script>
    <script>
        $('#<%=drChonkeoRE.ClientID%>').chosen();
        var previousValue = '';
        function checkTextbox() {
            var currentValue = document.getElementById('<%= txtTimkiem.ClientID %>').value;
             if (currentValue === '' && previousValue !== '') {
                 __doPostBack('<%= txtTimkiem.UniqueID %>', '');
            }
            previousValue = currentValue;
        }
    </script>
</asp:Content>
