<%@ Page Title="BB" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="XemLieuBB.aspx.cs" Inherits="KendaWeb.XemLieuBB" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 0 20px;">
        <div class="container" style="height: 70px; width: 100%">
            <div class="BBflex">
                <asp:DropDownList ID="drMay" runat="server" CssClass="form-control" Style="width: 170px; text-align: center; margin-right: 10px">
                    <asp:ListItem Selected="True" Value="">----Chọn máy----</asp:ListItem>
                    <asp:ListItem Value="01"> Máy 01 </asp:ListItem>
                    <asp:ListItem Value="02"> Máy 02 </asp:ListItem>
                    <asp:ListItem Value="03"> Máy 03 </asp:ListItem>
                    <asp:ListItem Value="04"> Máy 04 </asp:ListItem>
                    <asp:ListItem Value="05"> Máy 05 </asp:ListItem>
                    <asp:ListItem Value="06"> Máy 06 </asp:ListItem>
                    <asp:ListItem Value="07"> Máy 07 </asp:ListItem>
                    <asp:ListItem Value="tatca">Xem tất cả </asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtFromDay" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Từ ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" />
                <asp:TextBox ID="txtToday" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Đến ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" />
                <asp:Button ID="btn_TruyLieu" Text="Xem liệu" CssClass="btn btn-danger" Style="margin-right: 10px" OnClientClick="showLoading();" runat="server" OnClick="btn_TruyLieu_Click" />
                <%-- <asp:TextBox ID="txtSearch" Style="width: 300px; height: 35px; margin-right: 10px" CssClass="form-control" placeholder="Tìm kiếm" runat="server" />
                 <asp:Button Text="Tìm kiếm" CssClass="btn btn-info" OnClientClick="showLoading();" OnClick="btnSearch_Click" ID="btnSearch" runat="server" />--%>
                <%--                 <asp:TextBox ID="TextBox2" runat="server" placeholder="Tìm kiếm..." CssClass="form-control" oninput="Search_Gridview(this)"></asp:TextBox>--%>
                <asp:TextBox ID="txtTimkiem" Style="width: 300px; height: 35px; margin-right: 10px" CssClass="form-control" placeholder="Nhập mã keo tìm kiếm" runat="server"></asp:TextBox>
                <asp:Button ID="btnTimkiem" runat="server" OnClick="btnTimkiem_Click" CssClass="btn btn-info" Style="margin-right: 10px" Text="Tìm kiếm" />
                <asp:Button Text="Xuất Excel" CssClass="btn btn-success" Style="margin-right: 10px" OnClick="btnExportExcel_Click" ID="btnExportExcel" runat="server" />

            </div>
        </div>
        <div style="margin: 0 30px; height: 75vh; overflow: auto; border: 5px solid black;">
            <asp:GridView ID="gvKQ" runat="server"
                CssClass="table table-responsive table table-responsive table-bordered" OnRowCommand="gvKQ_RowCommand"
                Style="overflow: auto; overflow: hidden; border: groove; border-bottom: none; font-size: 18px; font-weight: bold; font-family: Arial; margin-bottom: 0;" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="NotSet" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                <HeaderStyle CssClass="header_girdview" />
                <RowStyle CssClass="row_css" />
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem <br> nguyên liệu <br> quét vào">
                        <ItemTemplate>
                            <%--<asp:ImageButton ImageUrl="~/images/R.png" Width="60px" Height="25px" CommandName="btnIn" CommandArgument='<%# Eval("Mã mesid")%>' runat="server" />--%>
                            <asp:ImageButton ImageUrl="../../image/R.png" OnClientClick="showLoading();" Width="60px" Height="25px" CommandName="btnIn" CommandArgument='<%# Eval("Mã mesid")+","+ Eval("Số máy")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Mã mesid" ItemStyle-HorizontalAlign="Center" HeaderText="Mã mesid" />
                    <asp:BoundField DataField="Số máy" ItemStyle-HorizontalAlign="Center" HeaderText="Số máy" />
                    <asp:BoundField DataField="Tên keo" ItemStyle-HorizontalAlign="Center" HeaderText="Tên keo" />
                    <asp:BoundField DataField="Số lô" ItemStyle-HorizontalAlign="Center" HeaderText="Số lô" />
                    <asp:BoundField DataField="Số mẻ điều động" ItemStyle-HorizontalAlign="Center" HeaderText="Số mẻ <br> điều động" HtmlEncode="false" />
                    <asp:BoundField DataField="Số mẻ hoàn thành" ItemStyle-HorizontalAlign="Center" HeaderText="Số mẻ <br> hoàn thành" HtmlEncode="false" />
                    <asp:BoundField DataField="Số ký tiêu chuẩn" ItemStyle-HorizontalAlign="Center" HeaderText="Số ký <br> tiêu chuẩn" HtmlEncode="false" />
                    <asp:BoundField DataField="Số ký đã quét tem" ItemStyle-HorizontalAlign="Center" HeaderText="Số ký <br> đã quét tem" HtmlEncode="false" />
                    <asp:BoundField DataField="Số ký hoàn thành" ItemStyle-HorizontalAlign="Center" HeaderText="Số ký <br> hoàn thành" HtmlEncode="false" />
                    <asp:BoundField DataField="Số ký chênh lệch" ItemStyle-HorizontalAlign="Center" HeaderText="Số ký <br> chênh lệch" HtmlEncode="false" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem <br> dữ liệu <br> in tem">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btnOut" CommandArgument='<%# Eval("Mã mesid")+","+ Eval("Số máy")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <table id="tbl_gvInTem" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvInTem1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <span style="font-size: 24px; font-weight: 600; margin-left: 20px;">Tổng số lượng:</span>
                                    <asp:Label ID="SumSL" runat="server" Style="font-size: 24px; font-weight: 600; margin-left: 20px;" Text="Label"></asp:Label>
                                    <%--      <span style="font-size: 24px; font-weight: 600; margin-left: 20px;">Chọn máy in:</span>--%>
                                    <asp:DropDownList ID="drMayin" Visible="false" CssClass="form-control" runat="server" Style="width: 200px; text-align: center; display: inline">
                                        <%-- ham dong may in visible=false--%>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnExcelPrdebe" runat="server" CssClass="btn btn-success" OnClick="btnExcelPrdebe_Click" Text="Xuất file Excel  " />
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvInTem();">X</button>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:TextBox ID="txtid" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvInTem" runat="server" OnRowCommand="gvInTem_RowCommand"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
                                                <%--            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xuất Excel">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="../../image/exportExcel.png" Width="30px" Height="30px" CommandName="btnCt" OnClientClick="showLoading();"
                                                            CommandArgument='<%# Eval("daylimt")+","+ Eval("barcode")+","+ Eval("slipno")+","+ Eval("weight")+","+ Eval("effdat")+","+ Eval("partno")+","+ Eval("intime")+","+ Eval("indat")+","+Eval("Pallet_no")+","+ Eval("class")+","+Eval("mesid")+","+ Eval("machno")+","+ Eval("prodat")%>'
                                                            runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:BoundField DataField="mesid" ItemStyle-HorizontalAlign="Center" HeaderText="Mã MES" />
                                                <asp:BoundField DataField="machno" ItemStyle-HorizontalAlign="Center" HeaderText="Máy" />
                                                <asp:BoundField DataField="daylimt" ItemStyle-HorizontalAlign="Center" HeaderText="Hạn <br> Sử <br> Dụng" HtmlEncode="false" />
                                                <asp:BoundField DataField="barcode" ItemStyle-HorizontalAlign="Center" HeaderText="Mã Vạch" />
                                                <asp:BoundField DataField="slipno" ItemStyle-HorizontalAlign="Center" HeaderText="Số Lô" />
                                                <asp:BoundField DataField="weight" ItemStyle-HorizontalAlign="Center" HeaderText="Trọng Lượng" />
                                                <asp:BoundField DataField="prodat" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày <br> Sản <br> Xuất" HtmlEncode="false" />
                                                <asp:BoundField DataField="effdat" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày <br> Hiệu <br> Lực" HtmlEncode="false" />
                                                <asp:BoundField DataField="class" ItemStyle-HorizontalAlign="Center" HeaderText="Ca" />
                                                <asp:BoundField DataField="partno" ItemStyle-HorizontalAlign="Center" HeaderText="Tên KEO" />
                                                <asp:BoundField DataField="intime" ItemStyle-HorizontalAlign="Center" HeaderText="Thời <br> Gian <br> Quét" HtmlEncode="false" />
                                                <asp:BoundField DataField="indat" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày Quét" />
                                                <asp:BoundField DataField="usrno" ItemStyle-HorizontalAlign="Center" HeaderText="Người Quét" />
                                                <asp:BoundField DataField="pallet_no" ItemStyle-HorizontalAlign="Center" HeaderText="Mã Palet" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>


        <div>
            <table id="tbl_gvNguyenLieu" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvNguyenLieu1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <asp:Button Text="Xuất Excel" CssClass="btn btn-success" Style="margin-right: 10px" OnClick="btnExcelNguyenLieu_Click" ID="btnExcelNguyenLieu" runat="server" />
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvNguyenLieu();">X</button>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:TextBox ID="txtid1" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvNguyenLieu" runat="server" OnRowCommand="gvNguyenLieu_RowCommand"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
                                                <asp:BoundField DataField="SaveTime" ItemStyle-HorizontalAlign="Center" HeaderText="Thời gian quét" />
                                                <asp:BoundField DataField="Equip_ID" ItemStyle-HorizontalAlign="Center" HeaderText="Tên máy" />
                                                <asp:BoundField DataField="Mater_Code" ItemStyle-HorizontalAlign="Center" HeaderText="Mã nguyên liệu" />
                                                <asp:BoundField DataField="Mater_Name" ItemStyle-HorizontalAlign="Center" HeaderText="Tên nguyên liệu" />
                                                <asp:BoundField DataField="Set_Num" ItemStyle-HorizontalAlign="Center" HeaderText="Số mẻ điều động" />
                                                <asp:BoundField DataField="Serial_Num" ItemStyle-HorizontalAlign="Center" HeaderText="Số mẻ hoàn thành" />
                                                <asp:BoundField DataField="real_weight" ItemStyle-HorizontalAlign="Center" HeaderText="Số ký quét" />
                                               <%-- <asp:BoundField DataField="error_allow" ItemStyle-HorizontalAlign="Center" HeaderText="Lệch phương" />--%>
                                                <asp:BoundField DataField="Mater_Barcode" ItemStyle-HorizontalAlign="Center" HeaderText="Tem quét" />
                                                <%--Hiển thị thêm số lô ngày 2022-06-03 Namnho mod--%>
                                                <asp:BoundField DataField="batchno" ItemStyle-HorizontalAlign="Center" HeaderText="Số lô" />
                                                <%--Namnho mod--%>
                                                <asp:TemplateField  ItemStyle-HorizontalAlign="Center"  ItemStyle-VerticalAlign="Middle" HeaderText="Xem chi tiết">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btnDoNguoc" CommandArgument='<%# Eval("Mater_Barcode")%>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table id="tbl_gvDoNguoc" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvDoNguoc1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <asp:Button Text="Xuất Excel" CssClass="btn btn-success" Style="margin-right: 10px" OnClick="btnExcelDoNguoc_Click" ID="btnExcelDoNguoc" runat="server" />
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvDoNguoc();">X</button>
                                    <button style="margin-right: 10px; height: 33px; width: 75px; text-align: center; padding: 0; float: right;" class="btn btn-info back">Quay lại</button>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:TextBox ID="Comment1" CssClass="cc" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvDoNguoc" runat="server" OnRowCommand="gvDoNguoc_RowCommand"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
                                                  <asp:BoundField DataField="SaveTime" ItemStyle-HorizontalAlign="Center" HeaderText="Thời gian quét" />
                                                <asp:BoundField DataField="Equip_ID" ItemStyle-HorizontalAlign="Center" HeaderText="Tên máy" />
                                                <asp:BoundField DataField="Mater_Code" ItemStyle-HorizontalAlign="Center" HeaderText="Mã nguyên liệu" />
                                                <asp:BoundField DataField="Mater_Name" ItemStyle-HorizontalAlign="Center" HeaderText="Tên nguyên liệu" />
                                                <asp:BoundField DataField="Set_Num" ItemStyle-HorizontalAlign="Center" HeaderText="Số mẻ điều động" />
                                                <asp:BoundField DataField="Serial_Num" ItemStyle-HorizontalAlign="Center" HeaderText="Số mẻ hoàn thành" />
                                                <asp:BoundField DataField="real_weight" ItemStyle-HorizontalAlign="Center" HeaderText="Số ký quét" />
                                             
                                                <asp:BoundField DataField="Mater_Barcode" ItemStyle-HorizontalAlign="Center" HeaderText="Tem quét" />
                                             
                                                <%--Hiển thị thêm số lô ngày 2022-06-03 Namnho mod--%>
                                                <asp:BoundField DataField="batchno" ItemStyle-HorizontalAlign="Center" HeaderText="Số lô" />
                                                <%--Namnho mod--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem chi tiết">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" OnClientClick="showLoading();" Height="25px" CommandName="btnDoNguoc" CommandArgument='<%# Eval("Mater_Barcode")%>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <%--ADẤD--%>

            <table id="tbl_gvDoNguocRL" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvDoNguocRL1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <asp:Button Text="Xuất Excel" CssClass="btn btn-success" Style="margin-right: 10px" OnClick="btnExcelDoNguoc_Click" ID="Button1" runat="server" />
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvDoNguocRL();">X</button>
                                    <button style="margin-right: 10px; height: 33px; width: 75px; text-align: center; padding: 0; float: right;" class="btn btn-info back">Quay lại</button>
                                </td>
                            </tr>
                            <tr>
                                <td id="sdasdaisodaoiduioasd" style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">                         
                                        <asp:TextBox ID="Comment2" CssClass="cc" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvDoNguocRL" runat="server" OnRowCommand="gvDoNguocRL_RowCommand"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
                                                <asp:BoundField DataField="pday" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày làm" />
                                                <asp:BoundField DataField="class" ItemStyle-HorizontalAlign="Center" HeaderText="Ca" />
                                                <asp:BoundField DataField="machno" ItemStyle-HorizontalAlign="Center" HeaderText="Máy" />
                                                <asp:BoundField DataField="mesid" ItemStyle-HorizontalAlign="Center" HeaderText="Mã Mes" HtmlEncode="false" />
                                                <asp:BoundField DataField="barcode" ItemStyle-HorizontalAlign="Center" HeaderText="Mã vạch" HtmlEncode="false" />
                                                <asp:BoundField DataField="partno" ItemStyle-HorizontalAlign="Center" HeaderText="Quy cách" HtmlEncode="false" />
                                              
                                                <asp:BoundField DataField="qty" ItemStyle-HorizontalAlign="Center" HeaderText="Số lượng" HtmlEncode="false" />
                                                <asp:BoundField DataField="bacode" ItemStyle-HorizontalAlign="Center" HeaderText="Mã nguyên liệu" HtmlEncode="false" />
                                                <asp:BoundField DataField="itnbr" ItemStyle-HorizontalAlign="Center" HeaderText="Tên keo" HtmlEncode="false" />
                                                  <asp:BoundField DataField="slipno" ItemStyle-HorizontalAlign="Center" HeaderText="Số lô" HtmlEncode="false" />
                                                <asp:BoundField DataField="indat" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày sản xuất" HtmlEncode="false" />
                                                <asp:BoundField DataField="intime" ItemStyle-HorizontalAlign="Center" HeaderText="Giờ sản xuất" HtmlEncode="false" />
                                                <asp:BoundField DataField="usrno" ItemStyle-HorizontalAlign="Center" HeaderText="Số thẻ" HtmlEncode="false" />

                                                <%--Namnho mod--%>
                                                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem chi tiết">--%>
                                               <%--     <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" OnClientClick="showLoading();" Height="25px" CommandName="btnDoNguoc" CommandArgument='<%# Eval("bacode")%>' runat="server" />
                                                    </ItemTemplate>--%>
                                                <%--</asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <%--SDASD--%>

            <table id="tbl_gvHC" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvHC1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <span style="font-size: 30px; font-weight: 600; margin-left: 20px;">Dữ liệu hóa chất</span>
                                    <asp:Button Text="Xuất Excel" CssClass="btn btn-success" Style="margin-right: 10px" OnClick="btnExcelHC_Click" ID="btnExcelHC" runat="server" />
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvHC();">X</button>
                                    <button style="margin-right: 10px; height: 33px; width: 75px; text-align: center; padding: 0; float: right;" class="btn btn-info back">Quay lại</button>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:TextBox ID="TextBox1" CssClass="cc" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvHC" runat="server" OnRowDataBound="gvHC_RowDataBound" OnRowCommand="gvHC_RowCommand"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
                                                <asp:BoundField DataField="Plan_id" ItemStyle-HorizontalAlign="Center" HeaderText="Mã bao hóa chất" />
                                                <asp:BoundField DataField="Equip_code" ItemStyle-HorizontalAlign="Center" HeaderText="Mã máy" />
                                                <asp:BoundField DataField="Serial_Num" ItemStyle-HorizontalAlign="Center" HeaderText="STT bao" />
                                                <asp:BoundField DataField="Material_Code" ItemStyle-HorizontalAlign="Center" HeaderText="Mã hóa chất" />
                                                <asp:BoundField DataField="Material_name" ItemStyle-HorizontalAlign="Center" HeaderText="Tên hóa chất" />
                                                <asp:BoundField DataField="Real_Weight" ItemStyle-HorizontalAlign="Center" HeaderText="Số cân" />
                                                <asp:BoundField DataField="Real_Error" ItemStyle-HorizontalAlign="Center" HeaderText="Lệch phương" />
                                                <asp:BoundField DataField="Weight_Time" ItemStyle-HorizontalAlign="Center" HeaderText="Thời gian hoàn thành" />
                                                <asp:BoundField DataField="Recipe_code" ItemStyle-HorizontalAlign="Center" HeaderText="Bao hóa chất" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem dữ liệu <br> quét bồn <br> hóa chất">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btn-chat" CommandArgument='<%# Eval("Material_Code")+","+ Eval("Equip_code")+","+ Eval("Weight_Time")+","+ Eval("Material_name")%>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table id="tbl_gvBonHC" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvBonHC1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <span style="font-size: 30px; font-weight: 600; margin-left: 20px;">Dữ liệu quét bồn hóa chất</span>
                                    <asp:Button Text="Xuất Excel" CssClass="btn btn-success" Style="margin-right: 10px" OnClick="btnExcelBonHC_Click" ID="btnExcelBonHC" runat="server" />
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvBonHC();">X</button>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:TextBox ID="TextBox3" CssClass="cc" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvBonHC" runat="server" OnRowDataBound="gvBonHC_RowDataBound"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="true">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
                                                <%-- <asp:BoundField DataField="Plan_id" ItemStyle-HorizontalAlign="Center" HeaderText="Plan_id" />
                                                <asp:BoundField DataField="EquipCode" ItemStyle-HorizontalAlign="Center" HeaderText="Mã máy" />
                                                <asp:BoundField DataField="MaterialCode" ItemStyle-HorizontalAlign="Center" HeaderText="Mã hóa chất" />
                                                <asp:BoundField DataField="ScanStr" ItemStyle-HorizontalAlign="Center" HeaderText="Mã tem vàng" />
                                                <asp:BoundField DataField="RecordTime" ItemStyle-HorizontalAlign="Center" HeaderText="Thời gian mở thùng" />
                                                <asp:BoundField DataField="closeTime" ItemStyle-HorizontalAlign="Center" HeaderText="Thời gian đóng thùng" />--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table id="tbl_gvSearch" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvSearch1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <span style="font-size: 24px; font-weight: 600; margin-left: 20px;">Tổng số lượng:</span>
                                    <asp:Label ID="Sum1" runat="server" Style="font-size: 24px; font-weight: 600; margin-left: 20px;" Text="Label"></asp:Label>
                                    <span style="font-size: 24px; font-weight: 600; margin-left: 20px;">Chọn máy in:</span>
                                    <asp:DropDownList ID="drMayIn1" CssClass="form-control" runat="server" Style="width: 200px; text-align: center; display: inline">
                                    </asp:DropDownList>
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvSearch();">X</button>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:GridView ID="gvSearch" runat="server" OnRowCommand="gvSearch_RowCommand"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xuất Excel">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="../../image/exportExcel.png" Width="30px" Height="30px" CommandName="btnCt" OnClientClick="showLoading();"
                                                            CommandArgument='<%# Eval("daylimt")+","+ Eval("barcode")+","+ Eval("slipno")+","+ Eval("weight")+","+ Eval("effdat")+","+ Eval("partno")+","+ Eval("intime")+","+ Eval("indat")+","+Eval("Pallet_no")+","+ Eval("class")+","+Eval("mesid")+","+ Eval("machno")+","+ Eval("prodat")%>'
                                                            runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

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
            $("#<%=txtFromDay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
            $("#<%=txtToday.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
        });
    </script>
    <script type="text/javascript">
        document.onkeydown = function (e) {
            if (e.ctrlKey && (e.keyCode === 85)) {
                return false;
            }
        }

        function Search_Gridview(strKey) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getEleme / Scripts / main.jsntById("<%=gvKQ.ClientID %>");
            var rowData;

            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
        oninput = "Search_Gridview(this)"
    </script>
    <script>
        function ValidateKeypress(numcheck, e) {
            var keynum, keychar, numcheck;
            if (window.event) {//IE
                keynum = e.keyCode;
            }
            else if (e.which) {// Netscape/Firefox/Opera
                keynum = e.which;
            }
            if (keynum == 8 || keynum == 127 || keynum == null || keynum == 9 || keynum == 0 || keynum == 13) return true;
            keychar = String.fromCharCode(keynum);
            var result = numcheck.test(keychar);
            return result;
        }
    </script>
    <%-- thuuoctinh textbox nhap so  onkeypress="return ValidateKeypress(/\d/,event);"--%>
</asp:Content>
