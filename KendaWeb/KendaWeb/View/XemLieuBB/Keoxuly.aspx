<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Keoxuly.aspx.cs" Inherits="KendaWeb.View.XemLieuBB.Keoxuly" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div style="padding: 0 20px;">
          <div class="container" style="height: 70px; width: 100%">
              <div class="BBflex">
                  <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Từ ngày :"></asp:Label>
                   <asp:TextBox ID="txtTungay" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Từ ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" />

                  <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Đến ngày : "></asp:Label>
                   <asp:TextBox ID="txtDenngay" runat="server" AutoPostBack="true" autocomplete="off" CssClass="form-control" placeholder="---Đến ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" />
                   <asp:Button ID="btnXemlieu" Text="Xem liệu" OnClick="btnXemlieu_Click"  CssClass="btn btn-danger" Style="margin-right: 10px" OnClientClick="showLoading();" runat="server"/>
               
               
              </div>
               <div style="margin: 0 30px; height: 75vh; overflow: auto; border: 5px solid black;">
            <asp:GridView ID="gvKQ" runat="server" 
                CssClass="table table-responsive table table-responsive table-bordered" 
                Style="overflow: auto; overflow: hidden; border: groove; border-bottom: none; font-size: 18px; font-weight: bold; font-family: Arial; margin-bottom: 0;" AutoGenerateColumns="true" EmptyDataRowStyle-HorizontalAlign="NotSet" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                <HeaderStyle CssClass="header_girdview" />
                <RowStyle CssClass="row_css" />
              <%--  <Columns>
                   
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
                </Columns>--%>
            </asp:GridView>
        </div>
             
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
     <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtTungay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
            $("#<%=txtDenngay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
        });
     </script>
</asp:Content>


