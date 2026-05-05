<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hoachatcantay.aspx.cs" Inherits="KendaWeb.View.BB2.Hoachatcantay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 0 20px;">

        <div class="container" style="height: 70px; width: 100%">
            <div class="BBflex">
                <asp:Label ID="Label1" runat="server" Text="Từ ngày : "></asp:Label>
                <asp:TextBox ID="txtTuNgay" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Chọn ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="Đến ngày : "></asp:Label>
                <asp:TextBox ID="txtDenNgay" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Chọn ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
                <asp:Button ID="btnXemLieu" runat="server" OnClick="btnXemLieu_Click" Text="Xem liệu" CssClass="btn btn-danger" Style="margin-right: 10px" OnClientClick="showLoading();" />
                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="btn btn-success" Style="margin-right: 10px" Text="Xuất excel" />
                <asp:TextBox ID="txtTimkiem" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" CssClass="form-control" placeholder="Nhập tiêu chuẩn cần tìm kiếm" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" OnClick="ImageButton1_Click" ImageUrl="~/Image/search-more.png" Width="50px" Height="50px" />
            </div>
        </div>

        <div class="container" style="height: 35px; width: 100%">
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label9" runat="server" Text="Bảng xem mes hóa chất Cân tay" Font-Size="25px"></asp:Label>

        </div>
        <div style="margin: 0 30px; height: 75vh; overflow: auto; border: 5px solid black;">
            <asp:GridView ID="gvHCcantay" OnRowCommand="gvHCcantay_RowCommand" runat="server" CssClass="table table-responsive table table-responsive table-bordered"
                Style="overflow: auto; overflow: hidden; border: groove; border-bottom: none; font-size: 18px; font-weight: bold; font-family: Arial; margin-bottom: 0;" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="NotSet" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                <HeaderStyle CssClass="header_girdview" />
                <RowStyle CssClass="row_css" />
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem <br> tiêu chuẩn">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btnIn" CommandArgument='<%# Eval("Plan_Id")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Plan_Id" ItemStyle-HorizontalAlign="Center" HeaderText="Plan_Id" />
                    <asp:BoundField DataField="Equip_Code" ItemStyle-HorizontalAlign="Center" HeaderText="Equip Code" />
                    <asp:BoundField DataField="TenMay" ItemStyle-HorizontalAlign="Center" HeaderText="Tên máy" />
                    <asp:BoundField DataField="Recipe_Name" ItemStyle-HorizontalAlign="Center" HeaderText="Recipe Name" />
                    <asp:BoundField DataField="Shift_Id" ItemStyle-HorizontalAlign="Center" HeaderText="Shift Id" />
                    <asp:BoundField DataField="Plan_Num" ItemStyle-HorizontalAlign="Center" HeaderText="Plan Num" />
                    <asp:BoundField DataField="Real_Num" ItemStyle-HorizontalAlign="Center" HeaderText="Real Num" />
                    <asp:BoundField DataField="Start_Date" ItemStyle-HorizontalAlign="Center" HeaderText="Start Date" />
                    <asp:BoundField DataField="End_Date" ItemStyle-HorizontalAlign="Center" HeaderText="End Date" />
                    <asp:BoundField DataField="Plan_Date" ItemStyle-HorizontalAlign="Center" HeaderText="Plan Date" />

                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem <br> dữ liệu <br> bao HC">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btnOut" CommandArgument='<%# Eval("Plan_Id")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <table id="tbl_gvInHC" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvInHC1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <asp:Label ID="Label10" runat="server" Text="Bảng xem hóa chất có trong tiêu chuẩn của mes" Font-Size="25px"></asp:Label>
                                    <asp:Label ID="Label4" runat="server" Text="(LR_recipe)" Font-Size="25px" Font-Bold="true"></asp:Label>
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowHCIn();">X</button>
                                    <asp:Button ID="btnExcelLrRecipe" OnClick="btnExcelLrRecipe_Click" runat="server" Text="Xuất Excel" Style="margin-right: 10px; height: 33px; width: 75px; text-align: center; padding: 0; float: right;" class="btn btn-info" />
                                    <%--<button style="margin-right: 10px; height: 33px; width: 75px; text-align: center; padding: 0; float:right;" class="btn btn-info back">Quay lại</button>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:TextBox ID="txtError" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvInHC" runat="server" OnRowCommand="gvInHC_RowCommand"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
                                                <asp:BoundField DataField="Plan_Id" ItemStyle-HorizontalAlign="Center" HeaderText="Plan Id" />
                                                <asp:BoundField DataField="Equip_Code" ItemStyle-HorizontalAlign="Center" HeaderText="Equip Code" />
                                                <asp:BoundField DataField="Weight_ID" ItemStyle-HorizontalAlign="Center" HeaderText="Weight ID" />
                                                <asp:BoundField DataField="Material_Code" ItemStyle-HorizontalAlign="Center" HeaderText="Material Code" />
                                                <asp:BoundField DataField="Material_Name" ItemStyle-HorizontalAlign="Center" HeaderText="Material Name" />
                                                <asp:BoundField DataField="Set_Weight" ItemStyle-HorizontalAlign="Center" HeaderText="Set Weight" />
                                                <asp:BoundField DataField="Set_Error" ItemStyle-HorizontalAlign="Center" HeaderText="Set Error" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem <br> Số kg sử dụng <br> và tem quét">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btnIfscan2mes" CommandArgument='<%#Eval("Plan_Id")%>' runat="server" />
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
        </div>
        <div>
            <table id="tbl_BarcodeLog" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_BarcodeLog1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <asp:Label ID="Label13" runat="server" Text="Hao dùng tem theo mes " Font-Size="25px"></asp:Label>
                                    
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowBarcodeLog();">X</button>
                                    <asp:Button ID="btnExcelBarcodelog" OnClick="btnExcelBarcodelog_Click" runat="server" Text="Xuất excel" Style="margin-right: 10px; height: 33px; width: 75px; text-align: center; padding: 0; float: right;" class="btn btn-info" />
                                   
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:TextBox ID="TextBox3" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvBarcodeLog" runat="server" OnRowCommand="gvBarcodeLog_RowCommand"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
                                                <asp:BoundField DataField="Plan_Id" ItemStyle-HorizontalAlign="Center" HeaderText="Plan_Id" />
                                                <asp:BoundField DataField="EquipCode" ItemStyle-HorizontalAlign="Center" HeaderText="EquipCode" />
                                                <asp:BoundField DataField="MaterialCode" ItemStyle-HorizontalAlign="Center" HeaderText="MaterialCode" />
                                                <asp:BoundField DataField="ScanStr" ItemStyle-HorizontalAlign="Center" HeaderText="ScanStr" />
                                                <asp:BoundField DataField="Qty" ItemStyle-HorizontalAlign="Center" HeaderText="Số kg tem" />
                                                <asp:BoundField DataField="Qtyw" ItemStyle-HorizontalAlign="Center" HeaderText="Số kg sử dụng" />
                                                <asp:BoundField DataField="Indat" ItemStyle-HorizontalAlign="Center" HeaderText="Indat" />
                                                <asp:BoundField DataField="Empno" ItemStyle-HorizontalAlign="Center" HeaderText="Empno" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem <br> chi tiết <br> tem vàng">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btnMes2row" CommandArgument='<%# Eval("ScanStr")%>' runat="server" />
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
        </div>
        <div>
    <table id="tbl_scanBar" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
        <tr>
            <td>
                <table id="tbl_scanBar1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                    <tr>
                        <td style="height: 40px; padding-left: 10px; color: black;">
                            <asp:Label ID="Label14" runat="server" Text="Bảng xem chi tiết bao nguyên liệu quét mở bồn hóa chất" Font-Size="25px"></asp:Label>
                            <asp:Label ID="Label8" runat="server" Text="(Mes2RawMaterial)" Font-Size="25px" Font-Bold="true"></asp:Label>
                            <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowScanbar();">X</button>
                            <asp:Button ID="btnExcelMes2RawMaterial" OnClick="btnExcelMes2RawMaterial_Click" runat="server"  Text="Xuất Excel" Style="margin-right: 10px; height: 33px; width: 75px; text-align: center; padding: 0; float: right;" class="btn btn-info" />
                            <%--<button style="margin-right: 10px; height: 33px; width: 75px; text-align: center; padding: 0; float:right;" class="btn btn-info back">Quay lại</button>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                            <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                <asp:TextBox ID="TextBox4" runat="server" Visible="false"></asp:TextBox>
                                <asp:GridView ID="gvScanbar" runat="server"
                                    CssClass="table table-responsive table table-responsive table-bordered"
                                    Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="header_girdview" />
                                    <RowStyle CssClass="row_css" />
                                    <Columns>
                                        <asp:BoundField DataField="EquipCode" ItemStyle-HorizontalAlign="Center" HeaderText="EquipCode" />
                                        <asp:BoundField DataField="Barcode" ItemStyle-HorizontalAlign="Center" HeaderText="Barcode" />
                                        <asp:BoundField DataField="MaterialCode" ItemStyle-HorizontalAlign="Center" HeaderText="MaterialCode" />
                                        <asp:BoundField DataField="PackageCount" ItemStyle-HorizontalAlign="Center" HeaderText="PackageCount" />
                                        <asp:BoundField DataField="PerPackageWeight" ItemStyle-HorizontalAlign="Center" HeaderText="PerPackageWeight" />
                                        <asp:BoundField DataField="LotNumber" ItemStyle-HorizontalAlign="Center" HeaderText="SlipNo" />
                                        <asp:BoundField DataField="ProductDate" ItemStyle-HorizontalAlign="Center" HeaderText="ProductDate" />
                                        <asp:BoundField DataField="ValidDate" ItemStyle-HorizontalAlign="Center" HeaderText="ValidDate" />
                                        <asp:BoundField DataField="ProductName" ItemStyle-HorizontalAlign="Center" HeaderText="ProductName" />
                                        <asp:BoundField DataField="RecordTime" ItemStyle-HorizontalAlign="Center" HeaderText="RecordTime" />


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
    <table id="tbl_gvOutHC" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
        <tr>
            <td>
                <table id="tbl_gvOutHC1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                    <tr>
                        <td style="height: 40px; padding-left: 10px; color: black;">
                            <asp:Label ID="Label11" runat="server" Text="Bảng xem số lần cân của mỗi bao" Font-Size="25px"></asp:Label>
                            <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowOutHC();">X</button>
                            <asp:Button ID="btnExcelLrLot" OnClick="btnExcelLrLot_Click" runat="server" Text="Xuất excel" Style="margin-right: 10px; height: 33px; width: 75px; text-align: center; padding: 0; float: right;" class="btn btn-info" />
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                            <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                <asp:GridView ID="gvOutHC" runat="server" 
                                    CssClass="table table-responsive table table-responsive table-bordered"
                                    Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="true">
                                    <HeaderStyle CssClass="header_girdview" />
                                    <RowStyle CssClass="row_css" />
                                    
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
            $("#<%=txtTuNgay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
            $("#<%=txtDenNgay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
        });
    </script>
</asp:Content>
