<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Loimayhoachat.aspx.cs" Inherits="KendaWeb.View.BB2.Loimayhoachat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 0 20px;">
        <div class="container" style="height: 70px; width: 100%">
            <div class="BBflex">
                <asp:DropDownList ID="drMay" runat="server" OnSelectedIndexChanged="drMay_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" Style="width: 170px; text-align: center; margin-right: 10px">
                    <asp:ListItem Selected="True" Value="">----Chọn máy----</asp:ListItem>
                    <asp:ListItem Value="198.1.8.17"> Máy -1 mới </asp:ListItem>
                    <asp:ListItem Value="198.1.8.16"> Máy -1 cũ </asp:ListItem>
                    <asp:ListItem Value="198.1.8.18"> Máy -9 mới </asp:ListItem>
                    <asp:ListItem Value="198.1.8.15"> Máy -9 cũ </asp:ListItem>

                </asp:DropDownList>
                <asp:Label ID="Label1" runat="server" Text="Chọn ngày : "></asp:Label>
                <asp:TextBox ID="txtChonngay" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Chọn ngày---" Style="width: 250px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="Từ giờ : "></asp:Label>
                <asp:TextBox ID="txtTugio" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Từ giờ---" Style="width: 150px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
               <asp:Label ID="Label3" runat="server" Text="Đến giờ : "></asp:Label>
                <asp:TextBox ID="txtDengio" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Đến giờ---" Style="width: 150px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
                <asp:Button ID="btnXemLieu" OnClick="btnXemLieu_Click" runat="server" Text="Xem liệu" CssClass="btn btn-danger" Style="margin-right: 10px" OnClientClick="showLoading();" />
            </div>
        </div>
        <div class="container" style="height: 35px; width: 100%">
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label9" runat="server" Text="Bảng xem lỗi máy hóa chất" Font-Size="25px"></asp:Label>
            <asp:Label ID="Label4" runat="server" Text="(LR_Alarmlog)" Font-Size="25px" Font-Bold="true"></asp:Label>
        </div>
         <div style="margin: 0 30px; height: 75vh; overflow: auto; border: 5px solid black;">
              <asp:GridView ID="gvKQ" runat="server"
                CssClass="table tablehaile table-hover table-responsive table table-responsive table-bordered" Style="border: double; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial" AutoGenerateColumns="false">
                <HeaderStyle CssClass="header_girdview" />
                <RowStyle CssClass="row_css" />
                <Columns>
                    <asp:BoundField DataField="Alarm_ID" ItemStyle-HorizontalAlign="Center" HeaderText="Mã Lỗi" />
                    <asp:BoundField DataField="Alarm_OccurTime" ItemStyle-HorizontalAlign="Center" HeaderText="Thời gian lỗi" />
                    <asp:BoundField DataField="Alarm_ClearTime" ItemStyle-HorizontalAlign="Center" HeaderText="Thời gian xử lý lỗi" />
                    <asp:BoundField DataField="Alarm_En_Info" ItemStyle-HorizontalAlign="Center" HeaderText="English error" />
                    <asp:BoundField DataField="Alarm_Cn_Info" ItemStyle-HorizontalAlign="Center" HeaderText="China error" />
                    <asp:BoundField DataField="Alarm_Other_Info" ItemStyle-HorizontalAlign="Center" HeaderText="Lỗi tiếng Việt" />
                   
                  
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
            $("#<%=txtChonngay.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
            
        });
        $(document).ready(function () {
            $('#<%=txtTugio.ClientID%>').timepicker({
                 timeFormat: 'HH:mm:ss',
                 interval: 30,
                 //minTime: '10',
                 //maxTime: '6:00pm',
                 //defaultTime: '11',
                 //startTime: '10:00',
                 dynamic: false,
                 dropdown: true,
                 scrollbar: true
             });
             $('#<%=txtDengio.ClientID%>').timepicker({
                 timeFormat: 'HH:mm:ss',
                 interval: 30,
                 //minTime: '10',
                 //maxTime: '6:00pm',
                 //defaultTime: '11',
                 //startTime: '10:00',
                 dynamic: false,
                 dropdown: true,
                 scrollbar: true
             });
        });
     
       
    </script>
     <script src="../../Scripts/jquery.timepicker.min.js"></script>
   
       

       
</asp:Content>
