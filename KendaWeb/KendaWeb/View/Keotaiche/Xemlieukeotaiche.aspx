<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Xemlieukeotaiche.aspx.cs" Inherits="KendaWeb.View.Keotaiche.Xemlieukeotaiche" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 0 20px;">
         <div class="container" style="height: 70px; width: 100%">
              <div class="BBflex">
                  <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Từ ngày :"></asp:Label>
                   <asp:TextBox ID="txtFromDay" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Từ ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" />

                  <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Đến ngày : "></asp:Label>
                   <asp:TextBox ID="txtToday" runat="server" AutoPostBack="true" OnTextChanged="txtToday_TextChanged"  autocomplete="off" CssClass="form-control" placeholder="---Đến ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" />
                   <asp:Button ID="btn_TruyLieu" Text="Xem liệu" OnClick="btn_TruyLieu_Click" CssClass="btn btn-danger" Style="margin-right: 10px" OnClientClick="showLoading();" runat="server"/>
                   <asp:TextBox  ID="txtTimkiem" Style="width: 200px; height: 35px; margin-right: 10px" CssClass="form-control" placeholder="Tìm mã theo mã vạch" runat="server" ></asp:TextBox>
                <asp:Button ID="btnTimkiem"  runat="server"  OnClick="btnTimkiem_Click"      CssClass="btn btn-info" Style="margin-right: 10px" Text="Tìm kiếm" />
                <asp:Button Text="Xuất Excel" OnClick="btnExportExcel_Click" CssClass="btn btn-success" Style="margin-right: 10px"  ID="btnExportExcel" runat="server" />
              </div>
             
         </div>
         <div style="margin: 0 30px; height: 75vh; overflow: auto; border: 5px solid black;">
            <asp:GridView ID="gvKQ" runat="server" OnRowCommand="gvKQ_RowCommand"
                CssClass="table table-responsive table table-responsive table-bordered" 
                Style="overflow: auto; overflow: hidden; border: groove; border-bottom: none; font-size: 18px; font-weight: bold; font-family: Arial; margin-bottom: 0;" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="NotSet" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
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
                    <asp:BoundField DataField="indat" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày sản xuất" HtmlEncode="false" />
                    <asp:BoundField DataField="intime" ItemStyle-HorizontalAlign="Center" HeaderText="Giờ sản xuất" HtmlEncode="false" />
                    <asp:BoundField DataField="usrno" ItemStyle-HorizontalAlign="Center" HeaderText="Số thẻ" HtmlEncode="false"/>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem <br> dữ liệu">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btnXem" CommandArgument='<%# Eval("bacode")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
         <div>

            <table id="tbl_gvEbe" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvEbe1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <asp:Button ID="btnExcelPrdebe" OnClick="btnExcelPrdebe_Click"   runat="server" CssClass="btn btn-success"  Text="Xuất file Excel  " />
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvEbe();">X</button>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:TextBox ID="txtid" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvPrdebe" runat="server"  OnRowCommand="gvPrdebe_RowCommand"
                                            CssClass="table table-responsive table table-responsive table-bordered"
                                            Style="width: 150%; border: groove; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial;" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="header_girdview" />
                                            <RowStyle CssClass="row_css" />
                                            <Columns>
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
                                                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem chi tiết">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="../../image/R.png" Width="60px" Height="25px" OnClientClick="showLoading();" CommandName="btnKeoRR" CommandArgument='<%#Eval("mesid")+","+ Eval("partno")%>' runat="server" />
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
              <table id="tbl_gvbarcoderep" style="display: none; position: fixed; top: 0; left: 0; right: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
                <tr>
                    <td>
                        <table id="tbl_gvbarcoderep1" style="background-color: gray; margin-left: auto; margin-right: auto; box-shadow: 0 0 10px 2px gray; border-radius: 10px; width: 95%; height: 70%;">
                            <tr>
                                <td style="height: 40px; padding-left: 10px; color: black;">
                                    <span style="font-size: 30px; font-weight: 600; margin-left: 20px;">Tem tái chế quét keo RE</span>
                                    <button type="button" class="btn btn-danger" style="float: right; margin-right: 10px" onclick="closeShowgvBarcoderep();">X</button>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: gainsboro; vertical-align: top; padding: 10px; max-width: 1080px">
                                    <div style="height: 100%; width: 100%; overflow: hidden; overflow: auto; border-style: solid;">
                                        <asp:TextBox ID="TextBox3" CssClass="cc" runat="server" Visible="false"></asp:TextBox>
                                        <asp:GridView ID="gvBarcoderep" runat="server" 
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
         <div>
            <table id="tblMessagess" style="display: none; position: absolute; top: 0; left: 0; width: 100%; height: 100%;">
                <tr>
                    <td>
                        <table id="tblMessagess1" style="text-align: center; font-size: 24px; font-weight: bold; background-color: #b6d7ff; margin-left: auto; margin-right: auto; box-shadow: #999 -4px -3px 10px 4px; border-radius: 5px; width: 500px; height: 200px;">
                            <tr>
                                <td style="height: 35px; padding-left: 10px; color: black;">Thông báo
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #b5b6dd; vertical-align: top; padding: 10px; text-align: center; margin-top: auto; margin-bottom: auto;">
                                    <asp:Label ID="lblThongbaobarcoderep" runat="server" Style="font-family: Arial; font-weight: bold; font-size: 16px; color: black" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 1px; background-color: #b6d7ff; text-align: right; padding: 5px; border-radius: 0 0 5px 5px;">
                                    <button type="button" class="btn btn-danger" onclick="closeMessagess();">Đóng</button>

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


</asp:Content>
