<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
         CodeBehind="Xemthanbb.aspx.cs" Inherits="KendaWeb.View.XemLieuBB.Xemthanbb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
   <style>
    :root {
        --primary-gradient: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
        --secondary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        --excel-gradient: linear-gradient(45deg, #217346, #1e623b);
        --shadow: 0 10px 25px rgba(0, 0, 0, 0.08);
    }

    body {
        background-color: #f8f9fa;
        font-family: "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
    }

    .main-container {
        padding: 30px 15px;
        max-width: 1450px;
        margin: 0 auto;
    }

    h3.page-title {
        color: #2d3436;
        font-weight: 800;
        text-transform: uppercase;
        letter-spacing: 1px;
        margin-bottom: 35px;
    }

    /* Filter Panel */
    .filter-panel {
        background: #ffffff;
        border-radius: 16px;
        padding: 32px 30px;
        margin-bottom: 40px;
        box-shadow: var(--shadow);
        border: 1px solid rgba(0, 0, 0, 0.06);
    }

    .filter-row {
        display: flex;
        flex-wrap: wrap;
        gap: 22px;
        align-items: flex-end;
    }

    .filter-item {
        flex: 1;
        min-width: 220px;
    }

    .filter-item-btns {
        flex: 1.6;
        min-width: 340px;
        display: flex;
        gap: 14px;
    }

    .form-label {
        color: #555;
        font-size:1.2rem;
        font-weight: 700;
        margin-bottom: 9px;
    }

    .form-control, .form-select {
        border: 2px solid #e0e0e0;
        border-radius: 10px;
        height: 52px;
        padding: 0 16px;
        font-size: 15.5px;
        transition: all 0.25s;
    }

    .form-control:focus, .form-select:focus {
        border-color: #667eea;
        box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.12);
    }

    /* Ô ngày đẹp giống hình */
    #<%= txtDate.ClientID %> {
        background: white;
        border: 2px solid #a0d8ff;
        border-radius: 12px;
        font-size: 16px;
        font-weight: 500;
        padding-left: 18px;
        box-shadow: 0 3px 12px rgba(79, 172, 254, 0.15);
    }

    /* Button */
    .btn-custom {
        border: none;
        border-radius: 10px;
        color: white;
        font-weight: 600;
        height: 52px;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
        transition: all 0.3s;
        font-size: 15.5px;
    }

    .btn-xem {
        background: var(--secondary-gradient);
        flex: 1;
    }

    .btn-excel {
        background: var(--excel-gradient);
        flex: 1;
    }

    .btn-custom:hover {
        transform: translateY(-3px);
        box-shadow: 0 8px 20px rgba(0, 0, 0, 0.18);
    }

    /* GridView */
    .table-card {
        background: #fff;
        border-radius: 16px;
        overflow: hidden;
        box-shadow: var(--shadow);
    }

    .grid-scroll-area {
        max-height: 550px;
        overflow-y: auto;
        overflow-x: auto;
    }

    .modern-grid th {
        position: sticky;
        top: 0;
        z-index: 10;                    /* Giữ header sticky */
        background: #f8f9fb !important;
        color: #2d3436 !important;
        font-weight: 700;
        text-transform: uppercase;
        font-size: 13px;
        padding: 20px 15px !important;
        box-shadow: 0 2px 4px rgba(0,0,0,0.08);
    }

    .modern-grid td {
        padding: 17px 15px !important;
        vertical-align: middle;
        font-size: 14.5px;
    }

    .status-badge {
        padding: 6px 14px;
        border-radius: 30px;
        font-size: 13px;
        font-weight: 600;
    }

    /* ==================== FIX LỊCH DATEPICKER ==================== */
    /* Quan trọng nhất: Đẩy lịch Datepicker lên trên cùng */
    #ui-datepicker-div {
        z-index: 999999 !important;
    }

    .ui-datepicker {
        z-index: 999999 !important;
    }

    /* Đảm bảo lịch không bị che bởi bất kỳ phần tử nào khác */
    .ui-widget.ui-widget-content.ui-datepicker {
        z-index: 999999 !important;
    }

    /* Fix thêm cho trường hợp có nhiều stacking context */
    .ui-datepicker-div, .ui-datepicker {
        position: absolute !important;
        z-index: 999999 !important;
    }


    /* Tinh chỉnh riêng cho bảng Thống kê */
    .stats-card {
        border-top: 4px solid #667eea; /* Tạo điểm nhấn ở viền trên */
        transition: all 0.3s ease;
    }

    .stats-header-title {
        color: #4a5568;
        font-weight: 800;
        text-transform: uppercase;
        letter-spacing: 0.5px;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 10px;
    }

    /* Màu sắc đặc biệt cho các cột số liệu */
    .col-thuc-te {
        color: #2d3436;
        font-weight: 700;
    }

    .col-quet-tem {
        color: #00b894; /* Màu xanh lá hiện đại */
        font-weight: 700;
    }

    /* Hiệu ứng dòng khi di chuột vào */
    .modern-grid tr:hover td {
        background-color: #f1f7ff !important;
        transition: 0.2s;
    }

    /* Badge cho loại than */
    .coal-badge {
        background: #f1f2f6;
        color: #2f3542;
        padding: 4px 12px;
        border-radius: 6px;
        border-left: 3px solid #747d8c;
    }
   
    
</style>

    <div class="main-container">
        <div class="text-center">
            <h3 class="page-title">
                <%--<span style="font-size: 26px; margin-right: 12px">🏭</span> --%>
                GIÁM SÁT SỬ DỤNG TEM THAN SẢN XUẤT
            </h3>
        </div>

        <div class="filter-panel">
            <div class="filter-row">
                <!-- Chọn máy -->
                <div class="filter-item">
                    <label class="form-label">CHỌN MÁY BB</label>
                    <asp:DropDownList ID="ddlMachine" CssClass="form-select" runat="server">
                        <asp:ListItem Value="">-- Chọn máy sản xuất BB --</asp:ListItem>
                        <asp:ListItem Value="01">Máy sản xuất BB1</asp:ListItem>
                        <asp:ListItem Value="02">Máy sản xuất BB2</asp:ListItem>
                        <asp:ListItem Value="03">Máy sản xuất BB3</asp:ListItem>
                        <asp:ListItem Value="04">Máy sản xuất BB4</asp:ListItem>
                        <asp:ListItem Value="05">Máy sản xuất BB5</asp:ListItem>
                        <asp:ListItem Value="06">Máy sản xuất BB6</asp:ListItem>
                        <asp:ListItem Value="07">Máy sản xuất BB7</asp:ListItem>
                        <asp:ListItem Value="08">Máy sản xuất BB8</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <!-- Thời gian truy xuất (giống hình) -->
                <div class="filter-item">
                    <label class="form-label">THỜI GIAN TRUY XUẤT 
                        <small style="color:#666; font-size:12px;">(Tính từ 6h30 hôm nay - 6h30 hôm sau)</small>
                    </label>
                    <asp:TextBox ID="txtDate" CssClass="form-control" runat="server" 
                                 placeholder="2026-03-31" />
                </div>

                <!-- Tìm kiếm nhanh -->
                <div class="filter-item">
                    <label class="form-label">🔍 TÌM KIẾM NHANH</label>
                    <input type="text" id="txtQuickSearch" class="form-control" 
                           placeholder="Nhập nội dung để lọc..." onkeyup="filterGridView()" />
                </div>

                <!-- Hai nút nằm ngang -->
                <div class="filter-item-btns">
                    <asp:LinkButton ID="btnXemLieu" runat="server" CssClass="btn-custom btn-xem" 
                                    OnClick="btnXemLieu_Click">
                        <i class="fa fa-search"></i> TRUY XUẤT DỮ LIỆU
                    </asp:LinkButton>
                    
                    <asp:LinkButton ID="btnExportExcel" runat="server" CssClass="btn-custom btn-excel" 
                                    OnClick="btnExportExcel_Click">
                        <i class="fa fa-file-excel"></i> XUẤT BÁO CÁO
                    </asp:LinkButton>
                </div>
            </div>
        </div>
        <asp:Label ID="lblRowCount" runat="server" ForeColor="Blue"></asp:Label>
        <!-- Bảng dữ liệu -->
        <div class="table-card">
            <div class="grid-scroll-area">
                
                <asp:GridView ID="gvData" runat="server" CssClass="table modern-grid" 
                    AutoGenerateColumns="False" EmptyDataText="Hiện chưa có dữ liệu" GridLines="None">
                    <Columns>
                         <asp:BoundField DataField="SaveTime" HeaderText="Thời gian quét" ItemStyle-Font-Bold="true" />
                        <%-- ID Thiết bị --%>
                        <asp:BoundField DataField="Equip_ID" HeaderText="ID Máy" ItemStyle-Font-Bold="true" />
        
                        <%-- Thông tin Nguyên liệu --%>
                        <asp:BoundField DataField="Mater_Code" HeaderText="Mã Nguyên Liệu" />
                        <asp:BoundField DataField="Mater_Name" HeaderText="Tên Nguyên Liệu" />
                        <asp:BoundField DataField="Mater_Barcode" HeaderText="Mã Vạch (Barcode)" />
        
                        <%-- Thông tin Lô hàng --%>
                        <asp:BoundField DataField="LotNumber" HeaderText="Số Lô" />
        
                        <%-- Trọng lượng (Sử dụng TemplateField để giữ style của bạn) --%>
                        <asp:TemplateField HeaderText="Trọng Lượng/Gói">
                            <ItemTemplate>
                                <div style="font-weight: 700; color: #4facfe">
                                    <%# Eval("PerPackageWeight") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Ngày tháng --%>
                        <asp:BoundField DataField="ProductDate" HeaderText="Ngày Sản Xuất" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:BoundField DataField="ValidDate" HeaderText="Hạn Sử Dụng" DataFormatString="{0:dd/MM/yyyy HH:mm}" />


                        
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <!-- THỐNG KÊ THAN - GIỐNG HỆT TABLE DATA -->
<div class="table-card stats-card" style="margin-top: 40px;">
    <div class="p-4 border-bottom bg-light">
        <h5 class="stats-header-title">
            <i class="fa fa-chart-pie" style="color: #667eea;"></i>
            THỐNG KÊ SỬ DỤNG THAN TRONG NGÀY
        </h5>
    </div>
    
    <div class="grid-scroll-area" style="max-height: 400px;">
        <asp:GridView ID="gvThongKeThan" runat="server" 
                      CssClass="table modern-grid"
                      AutoGenerateColumns="False" 
                      GridLines="None"
                      EmptyDataText="Chưa có dữ liệu thống kê than">
            <Columns>
                <asp:TemplateField HeaderText="Loại Than">
                    <ItemTemplate>
                        <span class="coal-badge">
                            <%# Eval("Loaithan") %>
                        </span>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                </asp:TemplateField>
                
                <asp:BoundField DataField="SLthucte" DataFormatString="{0:N0} kg" 
                                HeaderText="SL Thực Tế" 
                                ItemStyle-CssClass="col-thuc-te" 
                                ItemStyle-HorizontalAlign="Center" />
                                
                <asp:BoundField DataField="SLquettem" DataFormatString="{0:N0} kg" 
                                HeaderText="SL Quét Đổ Bồn" 
                                ItemStyle-CssClass="col-quet-tem" 
                                ItemStyle-HorizontalAlign="Center" />
            </Columns>
        </asp:GridView>
    </div>
</div>
    </div>
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

    <!-- Script Datepicker + Filter -->
    <script type="text/javascript">
        function filterGridView() {
            var input = document.getElementById("txtQuickSearch");
            var filter = input.value.toLowerCase();
            var table = document.querySelector(".modern-grid");
            if (!table) return;
            var tr = table.getElementsByTagName("tr");
            for (var i = 1; i < tr.length; i++) {
                var visible = false;
                var td = tr[i].getElementsByTagName("td");
                for (var j = 0; j < td.length; j++) {
                    if (td[j] && td[j].innerHTML.toLowerCase().indexOf(filter) > -1) {
                        visible = true;
                        break;
                    }
                }
                tr[i].style.display = visible ? "" : "none";
            }
        }

        function initDatePicker() {
            $("#<%= txtDate.ClientID %>").datepicker({
                dateFormat: "yy-mm-dd",
                closeText: "Đóng",
                prevText: "Trước",
                nextText: "Sau",
                currentText: "Hôm nay",
                monthNames: ["Tháng 1","Tháng 2","Tháng 3","Tháng 4","Tháng 5","Tháng 6","Tháng 7","Tháng 8","Tháng 9","Tháng 10","Tháng 11","Tháng 12"],
                dayNamesMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
                firstDay: 1,
                changeMonth: true,
                changeYear: true
            });
        }

        $(document).ready(function () {
            initDatePicker();
            // Set ngày mặc định là hôm nay nếu chưa có giá trị
            if (!$("#<%= txtDate.ClientID %>").val()) {
                var today = new Date();
                var dd = String(today.getDate()).padStart(2, '0');
                var mm = String(today.getMonth() + 1).padStart(2, '0');
                var yyyy = today.getFullYear();
                $("#<%= txtDate.ClientID %>").val(yyyy + '-' + mm + '-' + dd);
            }
        });

        // Hỗ trợ sau Postback (UpdatePanel nếu có)
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm) {
            prm.add_endRequest(function () {
                initDatePicker();
            });
        }
    </script>
</asp:Content>