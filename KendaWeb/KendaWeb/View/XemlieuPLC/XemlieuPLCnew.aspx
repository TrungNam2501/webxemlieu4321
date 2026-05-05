<%@ Page Title="Xem mes PLC" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="XemlieuPLCnew.aspx.cs" Inherits="KendaWeb.View.XemlieuPLC.XemlieuPLCnew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        GridView {
            background-color: #F9F9F9;
            white-space: nowrap;
        }

        tr.GridViewHeaderStyle th {
            position: sticky;
            top: 0;
            background-color: gray;
            color: black;
            text-align: center;
            font-weight: bold;
            font-family: Arial;
            font-size: 17px;
        }

        tr.GridViewRowStyle td {
            border: solid 1px #ccc;
            vertical-align: middle;
            height: 30px;
            padding-left: 10px;
            padding-right: 10px;
            font-family: Arial;
            font-size: 15px;
            color: #444444;
            text-align: center;
        }
    </style>
    <div style="padding: 0 20px;">
        <div class="container" style="height: 70px; width: 100%">
            <div class="BBflex">
                <asp:DropDownList ID="drChonmay" runat="server" CssClass="form-control" Style="width: 150px; text-align: center; margin-right: 10px">
                    <asp:ListItem Text="Tất cả" Value="" Enabled="true"></asp:ListItem>
                    <asp:ListItem Value="01"> BB-01 </asp:ListItem>
                    <asp:ListItem Value="02"> BB-02 </asp:ListItem>
                    <asp:ListItem Value="03"> BB-03 </asp:ListItem>
                    <asp:ListItem Value="04"> BB-04 </asp:ListItem>
                    <asp:ListItem Value="05"> BB-05 </asp:ListItem>
                    <asp:ListItem Value="06"> BB-06 </asp:ListItem>
                    <asp:ListItem Value="07"> BB-07 </asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtFromDay" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Từ ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" />
                <asp:TextBox ID="txtToday" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Đến ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000" />
                <asp:Button ID="btnTruylieumes" OnClick="btnTruylieumes_Click" runat="server" Text="Xem liệu" CssClass="btn btn-danger" Style="margin-right: 10px" OnClientClick="showLoading();" />
                <asp:TextBox ID="txtTimkiem" runat="server" Style="width: 200px; height: 35px; margin-right: 10px" CssClass="form-control" placeholder="Nhập mã keo tìm kiếm"></asp:TextBox>
                <asp:Button ID="btnTimkiem" runat="server" Text="Tìm kiếm" CssClass="btn btn-success" OnClick="btnTimkiem_Click" Style="margin-right: 10px" OnClientClick="showLoading();" />

            </div>

            <div style="margin: 0 30px; height: 80vh; overflow: auto; border: 5px solid black;">
                <asp:GridView ID="gvMesid" OnRowCommand="gvMesid_RowCommand" runat="server" CssClass="table tablehaile table-hover table-responsive table table-responsive table-bordered" Style="border: double; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial" AutoGenerateColumns="false">
                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                    <RowStyle CssClass="GridViewRowStyle checkStatus" />
                    <Columns>

                        <asp:BoundField DataField="mesid" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Mesid" HtmlEncode="false" />
                        <asp:BoundField DataField="recipe_name" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Mã keo" HtmlEncode="false" />
                        <asp:BoundField DataField="machno" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Máy" HtmlEncode="false" />
                        <asp:BoundField DataField="weight" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Số mẻ cài đặt" HtmlEncode="false" />
                        <asp:BoundField DataField="finishnum" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Số mẻ hoàn thành" HtmlEncode="false" />
                        <asp:BoundField DataField="indat" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Ngày tạo Mes" HtmlEncode="false" />
                        <asp:BoundField DataField="intime" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="Thời gian tạo Mes" HtmlEncode="false" />
                        <asp:BoundField DataField="idGrouplot" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="IdGrouplot" HtmlEncode="false" />
                        <asp:BoundField DataField="FinishTag" ItemStyle-CssClass="jsbang status" ItemStyle-HorizontalAlign="Center" HeaderText="Trạng thái" HtmlEncode="false" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Xem <br> dữ liệu PLC">
                            <ItemTemplate>

                                <asp:ImageButton ImageUrl="../../image/R.png" OnClientClick="showLoading();" Width="60px" Height="25px" CommandName="btnIn" CommandArgument='<%# Eval("mesid")+","+ Eval("idGrouplot")+","+Eval("recipe_name")+","+Eval("indat")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

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
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtFromDay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
            $("#<%=txtToday.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
        });
    </script>

    <script>
        $('.checkStatus').each(function () {
            let trangthai = $(this).find(".status").html();
         

            if (trangthai == "0") {
                $(this).find(".status").css({ 'font-weight': 'bold', 'color': '#2456E4', 'font-family': 'Arial' }).text("Đang Chạy")
            } else {
                $(this).find(".status").css({ 'font-weight': 'bold', 'color': 'green', 'font-family': 'Arial' }).text("Hoàn Thành");
            }

          
        });
    </script>
</asp:Content>
