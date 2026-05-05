<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="XemLieuPLC.aspx.cs" Inherits="KendaWeb.View.XemlieuPLC.XemLieuPLC" %>

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
                <asp:DropDownList ID="Drmay" runat="server" CssClass="form-control" Style="width: 150px; text-align: center; margin-right: 10px">
                    <asp:ListItem Selected="True" Value="">----Chọn máy----</asp:ListItem>
                    <asp:ListItem Value="01"> BB-01 </asp:ListItem>
                    <asp:ListItem Value="02"> BB-02 </asp:ListItem>
                    <asp:ListItem Value="03"> BB-03 </asp:ListItem>
                    <asp:ListItem Value="04"> BB-04 </asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="drDong" runat="server" CssClass="form-control" Style="width: 150px; text-align: center; margin-right: 10px">

                    <asp:ListItem Value="1000"> 1000 dòng </asp:ListItem>
                    <asp:ListItem Value="2000"> 2000 dòng </asp:ListItem>
                    <asp:ListItem Value="3000"> 3000 dòng </asp:ListItem>
                    <asp:ListItem Value="5000"> 5000 dòng </asp:ListItem>
                    <asp:ListItem Value="7000"> 7000 dòng </asp:ListItem>
                    <asp:ListItem Value="10000"> 10000 dòng </asp:ListItem>


                </asp:DropDownList>
                <asp:TextBox ID="txtDate" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Chọn ngày---" Style="width: 150px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
                <asp:TextBox ID="txtTugio" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Từ giờ---" Style="width: 150px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
                <%--<asp:TextBox ID="txtDengio" AutoPostBack="true" OnTextChanged="txtDengio_TextChanged" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Đến giờ---" Style="width: 150px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>--%>
                <asp:TextBox ID="txtDengio" AutoPostBack="true" OnTextChanged="txtDengio_TextChanged" runat="server" autocomplete="off" CssClass="form-control" placeholder="---Đến giờ---" Style="width: 150px; margin-right: 10px; text-align: center; font-family: Arial; font-size: 14px; font-weight: bold; color: #000000"></asp:TextBox>
                <asp:Button ID="btnTruyLieu" Text="Xem liệu" CssClass="btn btn-primary" Style="margin-right: 10px" OnClick="btnTruyLieu_Click" OnClientClick="showLoading();" runat="server" />
                <asp:TextBox ID="txtTimkiem" Style="width: 200px; height: 35px; margin-right: 10px" CssClass="form-control" placeholder="Nhập mã keo tìm kiếm" runat="server"></asp:TextBox>
                <asp:Button ID="btnTimkiem" OnClick="btnTimkiem_Click" runat="server" CssClass="btn btn-info" Style="margin-right: 10px" OnClientClick="showLoading();" Text="Tìm kiếm" />
                <asp:Button Text="Xuất Excel" CssClass="btn btn-success" OnClick="btnExportExcel_Click" Style="margin-right: 10px" ID="btnExportExcel" runat="server" />
            </div>
        </div>
        <div style="margin: 0 30px; height: 80vh; overflow: auto; border: 5px solid black;">
            <asp:GridView ID="gvKQPlc" runat="server" CssClass="table tablehaile table-hover table-responsive table table-responsive table-bordered" Style="border: double; background-color: white; font-size: 16px; font-weight: bold; font-family: Arial" AutoGenerateColumns="false">
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <Columns>
                    <asp:BoundField DataField="ID" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="ID" HtmlEncode="false" />
                    <asp:BoundField DataField="Indat" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="日期<br>(Date)" HtmlEncode="false" />
                    <asp:BoundField DataField="Intime" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="时间<br>(Time)" HtmlEncode="false" />
                    <asp:BoundField DataField="Bolt_Status" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="栓仔狀態<br>(Bolt_Status)" HtmlEncode="false" />
                    <asp:BoundField DataField="Discharge_Condition" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="卸料條件 <br>(Discharge_Condition)" HtmlEncode="false" />
                    <asp:BoundField DataField="Downstream_Weight" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="下輔機收膠重量<br>(Downstream_Weight)" HtmlEncode="false" />
                    <asp:BoundField DataField="Error_Code_1" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="異常代碼（膠料溫度、過載緊急停止）<br> (Error_Code_1)" HtmlEncode="false" />
                    <asp:BoundField DataField="Error_Code_2" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="異常代碼（油粉過載、誤計量及超時）<br>(Error_Code_2)" HtmlEncode="false" />
                    <asp:BoundField DataField="Error_Code_3" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="異常代碼（機台機械問題）<br> (Error_Code_3)" HtmlEncode="false" />
                    <asp:BoundField DataField="Error_Code_4" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="異常代碼（油粉料下限）<br> (Error_Code_4)" HtmlEncode="false" />
                    <asp:BoundField DataField="Error_Code_5" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="異常代碼（集塵風機警報）<br>(Error_Code_5)" HtmlEncode="false" />
                    <asp:BoundField DataField="Finish_Num" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="完成車次<br> (Finish_Num)" HtmlEncode="false" />
                    <asp:BoundField DataField="Indoor_Status" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="加料門狀態<br>(Indoor_Stautus)" HtmlEncode="false" />
                    <asp:BoundField DataField="Mix_Action" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="密煉動作 <br> (Mix_Action)" HtmlEncode="false" />
                    <asp:BoundField DataField="Mix_energy" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="密煉能量（累計功率）<br> (Mix_Energy)" HtmlEncode="false" />
                    <asp:BoundField DataField="Mix_Power" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="馬達功率（瞬時）<br> (Mix_Power)" HtmlEncode="false" />
                    <asp:BoundField DataField="Mix_Step" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="混煉步驟 <br> (Mix_Step)" HtmlEncode="false" />
                    <asp:BoundField DataField="Mix_Time" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="密煉時間<br> (Mix_Time)" HtmlEncode="false" />
                    <asp:BoundField DataField="Mode" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="手/自動狀態<br>(Mode)" HtmlEncode="false" />
                    <asp:BoundField DataField="Outdoor_Status" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="卸料門狀態<br> (Outdoor_status)" HtmlEncode="false" />
                    <asp:BoundField DataField="Ram_Position" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="重錘位置<br> (Ram_Position)" HtmlEncode="false" />
                    <asp:BoundField DataField="Ram_Pressure" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="重錘壓力<br> (Ram_Pressure)" HtmlEncode="false" />
                    <asp:BoundField DataField="Recipe_Name" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="配方名稱 <br>(Recipe_Name)" HtmlEncode="false" />
                    <asp:BoundField DataField="Rpm_Act" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="轉速實際值 <br> (Rpm_Act)" HtmlEncode="false" />
                    <asp:BoundField DataField="Rpm_Set" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="轉速設定值 <br> (Rpm_Set)" HtmlEncode="false" />
                    <asp:BoundField DataField="Rubber_Temp" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="膠料溫度<br>(Rubber_Temp)" HtmlEncode="false" />
                    <asp:BoundField DataField="Set_Num" ItemStyle-CssClass="jsbang" ItemStyle-HorizontalAlign="Center" HeaderText="設定車次<br>(Set_Num)" HtmlEncode="false" />


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



    <script src="../../Scripts/jquery.timepicker.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtDate.ClientID%>").datepicker({ dateFormat: "yy-mm-dd" });
         })

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


</asp:Content>
