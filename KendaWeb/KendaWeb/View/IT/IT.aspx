<%@ Page Title="IT" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IT.aspx.cs" Inherits="KendaWeb.IT" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: beige;">
        <div class="BBflex" style="height: 50px">
            <div class="BBflex" style="width: 15%;">
                <span style="display: block">Chọn máy:</span>
                <asp:DropDownList ID="drMay" runat="server" CssClass="form-control" Style="width: 170px; text-align: center;">
                    <asp:ListItem Selected="True" Value="">----Chọn máy----</asp:ListItem>
                    <asp:ListItem Value="01"> Máy 01 </asp:ListItem>
                    <asp:ListItem Value="02"> Máy 02 </asp:ListItem>
                    <asp:ListItem Value="03"> Máy 03 </asp:ListItem>
                    <asp:ListItem Value="04"> Máy 04 </asp:ListItem>
                    <asp:ListItem Value="05"> Máy 05 </asp:ListItem>
                    <asp:ListItem Value="06"> Máy 06 </asp:ListItem>
                    <asp:ListItem Value="07"> Máy 07 </asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="BBflex" style="width: 20%;">
                <span style="display: block">Chọn ngày:</span>
                <asp:TextBox ID="txtChonNgay" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Chọn ngày---" Style="width: 250px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" />
            </div>
            <asp:Button ID="btn_TruyLieu" Text="Xem liệu" OnClick="btn_TruyLieu_Click" CssClass="btn btn-danger" runat="server" />
        </div>
        <div class="BBflex1" style="padding: 0 35px;">
            <div class="BBflex" style="width: 400px">
                <asp:TextBox ID="txtSearch" Style="width: 300px; height: 35px;" CssClass="form-control" placeholder="---Nhập tem quét hoặc mã Pallet---" runat="server" />
                <asp:Button Text="Tìm kiếm" OnClick="btnSearch_Click" CssClass="btn btn-info" ID="btnSearch" runat="server" />
            </div>
            <asp:Button Text="Thêm mới" CssClass="btn btn-info" ID="btnThemMoi" OnClick="btnThemMoi_Click" runat="server" />
        </div>

        <div style="margin: 0 30px; height: 750px; overflow: auto; border: 5px solid black; background-color: gainsboro;">
            <asp:GridView ID="gvKQ" runat="server"
                CssClass="table table-responsive table table-responsive table-bordered" OnRowCommand="gvKQ_RowCommand"
                Style="border: groove; border-bottom: none; font-size: 18px; font-weight: bold; font-family: Arial; margin-bottom: 0;"
                AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="NotSet" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                <HeaderStyle CssClass="header_girdview" />
                <RowStyle CssClass="row_css" />
                <Columns>
                    <asp:BoundField DataField="Subno" ItemStyle-HorizontalAlign="Center" HeaderText="Khu vực" />
                    <asp:BoundField DataField="Factory" ItemStyle-HorizontalAlign="Center" HeaderText="Xưởng" />
                    <asp:BoundField DataField="mesid" ItemStyle-HorizontalAlign="Center" HeaderText="Mã MES" />
                    <asp:BoundField DataField="machno" ItemStyle-CssClass="cc1" HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" HeaderText="Máy" />
                    <asp:BoundField DataField="daylimt" ItemStyle-HorizontalAlign="Center" HeaderText="Hạn Sử Dụng" />
                    <asp:BoundField DataField="barcode" ItemStyle-HorizontalAlign="Center" HeaderText="Mã Vạch" />
                    <asp:BoundField DataField="slipno" ItemStyle-CssClass="cc1" HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" HeaderText="Số Lô" />
                    <asp:BoundField DataField="weight" ItemStyle-HorizontalAlign="Center" HeaderText="Trọng Lượng" />
                    <asp:BoundField DataField="prodat" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày Sản Xuất" />
                    <asp:BoundField DataField="effdat" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày Hiệu Lực" />
                    <asp:BoundField DataField="class" ItemStyle-HorizontalAlign="Center" HeaderText="Ca" />
                    <asp:BoundField DataField="ptype" ItemStyle-HorizontalAlign="Center" HeaderText="Loại" />
                    <asp:BoundField DataField="status" ItemStyle-HorizontalAlign="Center" HeaderText="Cán đảo" />
                    <asp:BoundField DataField="partno" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" HeaderText="Tên KEO" />
                    <asp:BoundField DataField="intime" ItemStyle-HorizontalAlign="Center" HeaderText="Thời Gian Quét" />
                    <asp:BoundField DataField="indat" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày Quét" />
                    <asp:BoundField DataField="usrno" ItemStyle-HorizontalAlign="Center" HeaderText="Người Quét" />
                    <asp:BoundField DataField="pallet_no" ItemStyle-HorizontalAlign="Center" HeaderText="Mã Palet" />
                    <asp:BoundField DataField="active" ItemStyle-HorizontalAlign="Center" HeaderText="Xuất" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <div class="BBflex">
                                <asp:Button CommandName="btnEdit" CommandArgument='<%# Eval("Subno")+","+ Eval("Factory")+","+ Eval("mesid")
                                        +","+ Eval("machno")+","+ Eval("daylimt")+","+ Eval("barcode")+","+ Eval("slipno")+","+ Eval("weight")
                                        +","+ Eval("prodat")+","+ Eval("effdat")+","+ Eval("class")+","+ Eval("ptype")+","+ Eval("status")
                                        +","+ Eval("partno")+","+ Eval("intime")+","+ Eval("indat")+","+ Eval("usrno")+","+ Eval("pallet_no")
                                        +","+ Eval("active")%>'
                                    Text="Sửa" runat="server" />
                                <%--                                <asp:Button CommandName="btnDelete" CommandArgument='<%# Eval("Subno")+","+ Eval("Factory")+","+ Eval("mesid")
                                        +","+ Eval("machno")+","+ Eval("daylimt")+","+ Eval("barcode")+","+ Eval("slipno")+","+ Eval("weight")
                                        +","+ Eval("prodat")+","+ Eval("effdat")+","+ Eval("class")+","+ Eval("ptype")+","+ Eval("status")
                                        +","+ Eval("partno")+","+ Eval("intime")+","+ Eval("indat")+","+ Eval("usrno")+","+ Eval("pallet_no")
                                        +","+ Eval("active")%>'
                                    Text="Xóa" runat="server" />--%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <table id="tbl_gvThemMoi" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
            <tr>
                <td>
                    <table id="tbl_gvThemMoi1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 50%; height: 30%;">
                        <tr>
                            <td style="height: 40px; padding-left: 10px; color: black;">
                                <span style="font-size: 24px; font-weight: 600; margin-left: 20px;">Thêm mới dữ liệu</span>
                                <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvThemMoi();">X</button>
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color: gainsboro; vertical-align: top; padding: 10px;">
                                <div style="height: 420px; width: 100%; border-style: solid;">
                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                    </asp:ScriptManager>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <script type="text/javascript">    
                                                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                                                    $("#<%=tbNgaySanXuat.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
                                                })
                                            </script>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Tên KEO" runat="server" />
                                                    <asp:TextBox ID="tbTenKeo" OnTextChanged="tbTenKeo_TextChanged" AutoPostBack="true" Width="150px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Ngày Sản Xuất" runat="server" />
                                                    <asp:TextBox ID="tbNgaySanXuat" OnTextChanged="tbNgaySanXuat_TextChanged" AutoPostBack="true" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Chọn ngày---" Style="width: 150px; height: 30px; display: inline-block" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Mã MES" runat="server" />
                                                    <asp:DropDownList ID="drMes" OnTextChanged="drMes_TextChanged" AutoPostBack="true" Width="150px" Height="30px" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Khu Vực" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbKV" Width="50px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Xưởng" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbX" Width="50px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Máy" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbMay" Width="100px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Số Lô" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbSoLo" Width="100px" runat="server" />
                                                </div>
                                            </div>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Ca" runat="server" />
                                                    <asp:DropDownList ID="drCa" Height="30px" runat="server">
                                                        <asp:ListItem Value="1">Ca ngày</asp:ListItem>
                                                        <asp:ListItem Value="2">Ca đêm</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div>
                                                    <asp:Label Text="Cán đảo" runat="server" />
                                                    <asp:DropDownList ID="drCanDao" Height="30px" runat="server">
                                                        <asp:ListItem Value="Y">Y</asp:ListItem>
                                                        <asp:ListItem Value="N">N</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div>
                                                    <asp:Label Text="Xuất" runat="server" />
                                                        <asp:DropDownList ID="drXuat" Height="30px" runat="server">
                                                            <asp:ListItem Value="Y">Y</asp:ListItem>
                                                            <asp:ListItem Value="N">N</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                            </div>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Thời Gian Quét" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbThoiGianQuet" Width="150px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Ngày Quét" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbNgayQuet" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Người Quét" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbNguoiQuet" runat="server" />
                                                </div>
                                            </div>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Số ký chênh lệch" runat="server" />
                                                    <asp:TextBox ID="tbSoKyChenhLech" ReadOnly="true" Enabled="false" Width="100px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Trọng Lượng" runat="server" />
                                                    <asp:TextBox ID="tbTrongLuong" Width="100px" OnTextChanged="tbTrongLuong_TextChanged" AutoPostBack="true" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Mã Palet" runat="server" />
                                                    <asp:TextBox ID="tbMaPalet" OnTextChanged="tbMaPalet_TextChanged" AutoPostBack="true" Width="100px" runat="server" />
                                                </div>
                                            </div>
                                            <div class="BBflex" style="height: 75px">
                                                <asp:Button Text="Tạo mới" ID="btnCreate" Width="300px" Height="50px" Style="margin-right: 20px;" CssClass="btn btn-info" OnClick="btnCreate_Click" runat="server" />
                                                <asp:Button Text="IN" ID="btnIn1" Width="300px" Height="50px" CssClass="btn btn-info" OnClick="btnIn1_Click" runat="server" />
                                                <asp:DropDownList ID="drMayin" CssClass="form-control" runat="server" Style="margin-left: 50px; width: 200px; text-align: center; display: inline">
                                                </asp:DropDownList>
                                            </div>
                                            <div>
                                                <asp:Label ID="lbError" Text="" Style="color: red; display: block; font-size: 18px; width: 100%; height: 20px; text-align: center;" Visible="false" runat="server" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <table id="tbl_gvSua" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
            <tr>
                <td>
                    <table id="tbl_gvSua1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 50%; height: 30%;">
                        <tr>
                            <td style="height: 40px; padding-left: 10px; color: black;">
                                <span style="font-size: 24px; font-weight: 600; margin-left: 20px;">Chỉnh Sửa dữ liệu</span>
                                <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvSua();">X</button>
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color: gainsboro; vertical-align: top; padding: 10px;">
                                <div style="height: 500px; width: 100%; border-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Khu Vực" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbKV1" Width="50px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Xưởng" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbX1" Width="50px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Mã MES" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbMes1" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Máy" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbMay1" runat="server" />
                                                </div>
                                            </div>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Hạn Sử Dụng" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbHanSuDung1" Width="50px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Mã Vạch" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbMaVach1" Width="100px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Số Lô" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbSoLo1" Width="100px" runat="server" />
                                                </div>
                                            </div>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Ngày Sản Xuất" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbNgaySanXuat1" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Ngày Hiệu Lực" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbNgayHieuLuc1" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Ca" runat="server" />
                                                    <asp:DropDownList ID="drCa1" runat="server">
                                                        <asp:ListItem Value="1">Ca ngày</asp:ListItem>
                                                        <asp:ListItem Value="2">Ca đêm</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Tên KEO" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbTenKeo1" Width="100px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Loại" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbLoai1" Width="50px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:DropDownList ID="drCanDao1" Height="30px" runat="server">
                                                        <asp:ListItem Value="Y">Y</asp:ListItem>
                                                        <asp:ListItem Value="N">N</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div>
                                                    <asp:DropDownList ID="drXuat1" Height="30px" runat="server">
                                                        <asp:ListItem Value="Y">Y</asp:ListItem>
                                                        <asp:ListItem Value="N">N</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Thời Gian Quét" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbThoiGianQuet1" Width="150px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Ngày Quét" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbNgayQuet1" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Chọn ngày---" Style="width: 150px; display: inline-block" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Người Quét" runat="server" />
                                                    <asp:TextBox ReadOnly="true" ID="tbNguoiQuet1" runat="server" />
                                                </div>
                                            </div>
                                            <div class="BBflex2">
                                                <div>
                                                    <asp:Label Text="Số ký giới hạn tăng tối đa" runat="server" />
                                                    <asp:TextBox ID="tbSoKyChenhLech1" ReadOnly="true" Enabled="false" Width="100px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Trọng Lượng" runat="server" />
                                                    <asp:TextBox ID="tbTrongLuong1" OnTextChanged="tbTrongLuong1_TextChanged" AutoPostBack="true" Width="100px" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Mã Palet" runat="server" />
                                                    <asp:TextBox ID="tbMaPalet1" OnTextChanged="tbMaPalet1_TextChanged" AutoPostBack="true" Width="100px" runat="server" />
                                                </div>
                                            </div>
                                            <div class="BBflex" style="height: 75px">
                                                <asp:Button Text="Cập nhật" ID="btnSua" Width="300px" Height="50px" Style="margin-right: 20px;" CssClass="btn btn-info" OnClick="btnSua_Click" runat="server" />
                                                <asp:Button Text="IN" ID="btnInLieu" Width="300px" Height="50px" CssClass="btn btn-info" OnClick="btnInLieu_Click" runat="server" />
                                                <asp:DropDownList ID="drMayIn1" CssClass="form-control" runat="server" Style="margin-left: 50px; width: 200px; text-align: center; display: inline">
                                                </asp:DropDownList>
                                            </div>
                                            <div>
                                                <asp:Label ID="lbError1" Text="" Style="color: red; display: block; font-size: 18px; width: 100%; height: 20px; text-align: center;" Visible="false" runat="server" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
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
                                    <asp:Label ID="lblThongbao" runat="server" Style="font-family: Arial; font-weight: bold; font-size: 16px; color: black"></asp:Label>
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
            $("#<%=txtChonNgay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
            $("#<%=tbNgaySanXuat.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
        });
    </script>
</asp:Content>
