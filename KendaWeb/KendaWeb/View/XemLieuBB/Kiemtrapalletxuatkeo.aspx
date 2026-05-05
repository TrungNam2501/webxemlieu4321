<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Kiemtrapalletxuatkeo.aspx.cs" Inherits="KendaWeb.View.XemLieuBB.Kiemtrapalletxuatkeo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 0 20px;">
        <div class="container" style="height: 70px; width: 100%; text-align: center; color: black">
            <h1>Kiểm tra xuất keo theo mã Pallet</h1>
            <div class="BBflex">
                <asp:Label ID="Label2" runat="server" Font-Bold="false" Font-Size="20px" Text="Nhập mã Pallet :    "></asp:Label>
                <asp:TextBox ID="txtTimkiem" Style="width: 300px; height: 35px; margin-right: 10px" CssClass="form-control" placeholder="Nhập mã pallet cần kiểm " runat="server"></asp:TextBox>
                <asp:Button ID="btnTimkiem" runat="server" OnClick="btnTimkiem_Click" OnClientClick="showLoading();" CssClass="btn btn-info" Style="margin-right: 10px" Text="Kiểm tra" />

            </div>
            <div style="margin: 0 30px; height: 75vh; overflow: auto; border: 5px solid black;">
                <asp:GridView ID="gvKQ" runat="server" 
                    CssClass="table table-responsive table table-responsive table-bordered"
                    Style="overflow: auto; overflow: hidden; border: groove; border-bottom: none; font-size: 18px; font-weight: bold; font-family: Arial; margin-bottom: 0;" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="NotSet" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                    <HeaderStyle CssClass="header_girdview" />
                    <RowStyle CssClass="row_css checkStatus" />
                    <Columns>

                        <asp:BoundField DataField="mesid" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Mã MES" />
                        <asp:BoundField DataField="machno" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Máy" />
                        <asp:BoundField DataField="barcode" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Barcode" />
                        <asp:BoundField DataField="slipno" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Số lô" HtmlEncode="false" />
                        <asp:BoundField DataField="weight" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Số kg" HtmlEncode="false" />
                        <asp:BoundField DataField="effdat" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày hết hạn" HtmlEncode="false" />
                        <asp:BoundField DataField="class" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Ca" HtmlEncode="false" />
                        <asp:BoundField DataField="partno" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Tên keo" HtmlEncode="false" />
                        <asp:BoundField DataField="intime" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Giờ sản xuất" HtmlEncode="false" />
                        <asp:BoundField DataField="indat"  ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày sản xuất" HtmlEncode="false" />
                        <asp:BoundField DataField="usrno"  ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Số thẻ" HtmlEncode="false" />
                        <asp:BoundField DataField="pallet_no" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Mã Pallet" HtmlEncode="false" />
                        <asp:BoundField DataField="active" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="jsbang status" HeaderText="Trạng thái" HtmlEncode="true" />
                       

                    </Columns>
                </asp:GridView>
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

         <script>
             $('.checkStatus').each(function () {
                 let trangthai = $(this).find(".status").html();

                 if (trangthai == "&nbsp;") {
                     $(this).find(".status").css({ 'font-weight': 'bold', 'color': '#FF0000', 'font-family': 'Arial' }).text("Keo không xuất")
                 }
                 if (trangthai == "N") {
                     $(this).find(".status").css({ 'font-weight': 'bold', 'color': '#2456E4', 'font-family': 'Arial' }).text("Chưa xuất")
                 }
                
                 if (trangthai=="Y") {
                     $(this).find(".status").css({ 'font-weight': 'bold', 'color': 'green', 'font-family': 'Arial' }).text("Hoàn Thành");
                 }


              

             });
         </script>


</asp:Content>
