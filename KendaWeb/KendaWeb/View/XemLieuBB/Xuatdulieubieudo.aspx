<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
         CodeBehind="Xuatdulieubieudo.aspx.cs" Inherits="KendaWeb.View.XemLieuBB.Xuatdulieubieudo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        /* ====================== GIAO DIỆN HIỆN ĐẠI ====================== */
        /* Đảm bảo lịch luôn nằm trên cùng */
        .ui-datepicker {
            z-index: 9999 !important;
            box-shadow: 0 5px 15px rgba(0,0,0,0.2); /* Thêm đổ bóng cho đẹp và dễ nhìn */
            border: 1px solid #ddd !important;
        }
        .main-container {
            max-width: 1400px;
            margin: 20px auto;
            padding: 0 20px;
        }

        .card {
            background: white;
            border-radius: 12px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
            overflow: hidden;
            margin-bottom: 25px;
        }

        .card-header {
            background: linear-gradient(135deg, #007bff, #00a2ff);
            color: white;
            padding: 18px 25px;
            font-size: 18px;
            font-weight: 600;
        }

        .filter-panel {
            padding: 25px;
            background: #f8fbff;
            border-bottom: 1px solid #e9ecef;
        }

        .filter-row {
            display: flex;
            align-items: center;
            gap: 25px;
            flex-wrap: wrap;
        }

        .filter-group {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .filter-group label {
            font-weight: 600;
            color: #2c3e50;
            min-width: 85px;
            font-size: 15px;
        }

        .form-control {
            padding: 10px 14px;
            border: 1px solid #ced4da;
            border-radius: 8px;
            font-size: 15px;
            transition: all 0.3s;
        }

        .form-control:focus {
            border-color: #007bff;
            box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.15);
            outline: none;
        }

        .btn-search {
            background: linear-gradient(135deg, #007bff, #00a2ff);
            color: white;
            border: none;
            padding: 12px 28px;
            border-radius: 8px;
            font-weight: 600;
            font-size: 15px;
            cursor: pointer;
            transition: all 0.3s;
            box-shadow: 0 3px 8px rgba(0, 123, 255, 0.3);
        }

        .btn-search:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 12px rgba(0, 123, 255, 0.4);
        }

        /* Bảng dữ liệu hiện đại */
        .data-table {
            width: 100%;
            border-collapse: collapse;
            background: white;
            font-size: 14.5px;
        }

        .data-table th {
                background: linear-gradient(135deg, #0088cc, #00aaff);
                color: white;
                padding: 14px 10px;
                text-align: center;
                font-weight: 600;
                border-bottom: 3px solid #006699;
                position: sticky;
                top: 0;
                /* Sửa từ 10 xuống 1 hoặc 2 để không đè lên menu MasterPage */
                z-index: 1; 
            }

        .data-table td {
            padding: 12px 10px;
            border-bottom: 1px solid #e9ecef;
            vertical-align: middle;
        }

        .data-table tr:nth-child(even) {
            background-color: #f8fbff;
        }

        .data-table tr:hover {
            background-color: #e6f4ff;
            transition: background 0.2s;
        }

        .data-table tr.highlight td {
            background-color: #a1f0ff !important;
            font-weight: 500;
        }

        .xuatkho {
            background-color: #f1f3f5;
            text-align: center;
            width: 110px;
        }

        .btn-xuat {
            background: linear-gradient(135deg, #6c757d, #495057);
            color: white;
            border: none;
            padding: 8px 18px;
            border-radius: 6px;
            font-size: 13px;
            font-weight: 500;
            cursor: pointer;
            transition: all 0.3s;
        }

        .btn-xuat:hover {
            background: linear-gradient(135deg, #495057, #343a40);
            transform: translateY(-1px);
        }

        .info-bar {
            background: #e9f5ff;
            padding: 12px 25px;
            border-bottom: 1px solid #cce5ff;
            font-size: 15.5px;
            color: #2c3e50;
            font-weight: 500;
        }

        @media (max-width: 992px) {
            .filter-row {
                flex-direction: column;
                align-items: stretch;
                gap: 15px;
            }
        }
        .form-control {
    background-color: #ffffff;
    border: 1px solid #ccc;
    border-radius: 8px;
    padding: 6px 10px;
    box-shadow: 0 2px 5px rgba(0,0,0,0.1);
}

label {
    font-weight: 600;
    margin-right: 8px;
}
.btn-xuat-txt {
    background: linear-gradient(135deg, #28a745, #218838); /* Màu xanh lá cho TXT */
    color: white;
    border: none;
    padding: 8px 15px;
    border-radius: 6px;
    font-size: 13px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.3s;
}

.btn-xuat-txt:hover {
    background: linear-gradient(135deg, #218838, #1e7e34);
    transform: translateY(-1px);
    box-shadow: 0 4px 8px rgba(40, 167, 69, 0.3);
}

/* Điều chỉnh lại chiều rộng cột thao tác để chứa đủ 3 nút */
.xuatkho {
    width: 280px !important;
}

.btn-bieudo {
    background: linear-gradient(135deg, #e65100, #ff6d00);
    color: white;
    border: none;
    padding: 8px 15px;
    border-radius: 6px;
    font-size: 13px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.3s;
}

.btn-bieudo:hover {
    background: linear-gradient(135deg, #bf360c, #e65100);
    transform: translateY(-1px);
    box-shadow: 0 4px 8px rgba(230, 81, 0, 0.3);
}
/* Đẩy ô tìm kiếm sang bên phải */
.search-container {
    display: flex;
    justify-content: flex-end;
    padding: 0 25px 10px 25px;
}

#txtQuickSearch {
    border: 1px solid #007bff;
    background-image: url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/svgs/solid/search.svg'); /* Có thể thêm icon nếu muốn */
}
header, .navbar, #menu-container, .main-header {
    z-index: 9999 !important;
    position: relative; /* Đảm bảo z-index có tác dụng */
}
    </style>

    <div class="main-container">

        <div class="card">

            <!-- Header Card -->
            <div class="card-header">
                <i class="fas fa-chart-bar"></i> Xuất Dữ Liệu Biểu Đồ Kenda
            </div>

            <!-- Filter Panel -->
            <div class="filter-panel">
                <div class="filter-row">
                    

                     <div class="filter-group">
                         <label>Chọn máy:</label>
                         <asp:DropDownList ID="ddlMay" CssClass="form-control" runat="server">
                              <asp:ListItem Value="">-- Chọn máy BB --</asp:ListItem>
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


                    <div class="filter-group">
                        <label>Chọn ngày:</label>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" 
                                     Width="170px" placeholder="2026-03-31"   />
                    </div>

                    <div class="filter-group">
                        <asp:Button ID="btnTimKiem" runat="server" Text="Load dữ liệu" 
                                   CssClass="btn-search" OnClick="btnTimKiem_Click" />
                    </div>
                    <div class="filter-group" style="margin-bottom: 15px; margin-left: auto;">
                        <label>Tìm nhanh:</label>
                        <input type="text" id="txtQuickSearch" class="form-control" 
                               placeholder="Nhập mã máy, mã kế hoạch..." 
                               onkeyup="filterGridView()" style="width: 300px;" />
                    </div>
                </div>
            </div>

            <!-- Thông tin hiện tại -->
            <div class="info-bar">
                Máy hiện tại: 
                <asp:Label ID="lblMayHienTai" runat="server" Text="" Font-Bold="true" ForeColor="#007bff"></asp:Label> &nbsp;&nbsp;&nbsp;
                Ngày: 
                <asp:Label ID="lblNgayHienTai" runat="server" Text="" Font-Bold="true" ForeColor="#007bff"></asp:Label>
            </div>

            <!-- Hidden fields cho dữ liệu biểu đồ -->
            <asp:HiddenField ID="hdChartData" runat="server" />
            <asp:HiddenField ID="hdChartRecipe" runat="server" />
            <asp:HiddenField ID="hdChartMesid" runat="server" />
            <asp:HiddenField ID="hdChartMay" runat="server" />

            <!-- Bảng dữ liệu -->
            <div style="padding: 10px 25px 25px 25px;">
                <asp:GridView ID="gvDuLieu" runat="server" 
                              CssClass="data-table" 
                              AutoGenerateColumns="false"
                              OnRowDataBound="gvDuLieu_RowDataBound"
                              EmptyDataText="Không có dữ liệu cho máy và ngày đã chọn."
                              ShowHeaderWhenEmpty="true"
                              GridLines="None">
                <Columns>
                        <%-- Liệt kê các cột dữ liệu trước --%>
                        <asp:BoundField DataField="RecipeCode" HeaderText="Mã Recipe" />
                        
                        <asp:BoundField DataField="Start_datetime" HeaderText="TG Bắt đầu" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                        <asp:BoundField DataField="End_datetime" HeaderText="TG Kết thúc" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                        <asp:BoundField DataField="SetNumber" HeaderText="Số mẻ cài đặt" />
                        <asp:BoundField DataField="FinishNum" HeaderText="Số mẻ hoàn thành" />
                        <asp:BoundField DataField="Plan_ID" HeaderText="Mã chạy máy" />
                        <asp:BoundField DataField="MesPlanID" HeaderText="Mã kế hoạch" />

                        <%-- Cột nút bấm để sau cùng --%>
                       <%-- <asp:TemplateField HeaderText="Xuất dữ liệu">
                            <ItemTemplate>
                                <asp:Button ID="btnXuat" runat="server" Text="Xuất" 
                                            CssClass="btn-xuat" 
                                            CommandArgument='<%# Eval("MesPlanID") %>' 
                                            OnClick="btnXuat_Click" />
                            </ItemTemplate>
                            <ItemStyle CssClass="xuatkho" HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Thao tác">
                        <ItemTemplate>
                            <div style="display: flex; gap: 8px; justify-content: center;">
                                <%-- Nút Xuất Excel hiện tại --%>
                                <asp:Button ID="btnXuat" runat="server" Text="Excel" 
                                            CssClass="btn-xuat" 
                                            CommandArgument='<%# Eval("MesPlanID") %>' 
                                            OnClick="btnXuat_Click" />

                                <%-- Nút Xuất TXT mới thêm --%>
                                <asp:Button ID="btnXuatTxt" runat="server" Text="TXT" 
                                            CssClass="btn-xuat-txt" 
                                            CommandArgument='<%# Eval("MesPlanID") %>' 
                                            OnClick="btnXuatTxt_Click" />

                                <%-- Nút Xem Biểu Đồ Mixer Curve --%>
                                <asp:Button ID="btnBieuDo" runat="server" Text="Biểu đồ" 
                                            CssClass="btn-bieudo" 
                                            CommandArgument='<%# Eval("MesPlanID") %>' 
                                            OnClick="btnBieuDo_Click" />
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="xuatkho" HorizontalAlign="Center" Width="180px" />
                    </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>

    <!-- ============= MODAL BIỂU ĐỒ MIXER CURVE ============= -->
    <div id="chartModal" style="display:none; position:fixed; top:0; left:0; width:100%; height:100%;
         background:rgba(0,0,0,0.7); z-index:10000; overflow:auto;">
        <div style="background:white; margin:20px auto; width:95%; max-width:1500px; border-radius:12px;
             box-shadow:0 10px 40px rgba(0,0,0,0.3); overflow:hidden;">
            <!-- Header -->
            <div style="background:linear-gradient(135deg,#e65100,#ff6d00); color:white; padding:15px 25px;
                 display:flex; justify-content:space-between; align-items:center;">
                <div>
                    <span style="font-size:20px; font-weight:700;">Mixer Curve</span>
                    <span id="chartTitle" style="margin-left:20px; font-size:15px; opacity:0.9;"></span>
                </div>
                <button onclick="closeChart();" style="background:rgba(255,255,255,0.2); border:none; color:white;
                        font-size:22px; width:36px; height:36px; border-radius:50%; cursor:pointer; line-height:36px;">&#10005;</button>
            </div>
            <!-- Toolbar chọn barcode + checkbox toggle đường -->
            <div id="chartToolbar" style="padding:12px 25px; background:#f8f9fa; border-bottom:1px solid #e9ecef;
                 display:flex; flex-wrap:wrap; align-items:center; gap:15px;">
                <label style="font-weight:600; margin-right:5px;">Chọn mẻ:</label>
                <select id="selBarcode" onchange="renderSelectedBarcode();" 
                        style="padding:6px 12px; border:1px solid #ccc; border-radius:6px; font-size:14px;"></select>
                <span style="margin-left:20px; font-weight:600;">Hiển thị:</span>
                <label style="cursor:pointer;"><input type="checkbox" class="cbLine" value="temp" checked /> <span style="color:#d32f2f;">&#9632;</span> Nhiệt độ</label>
                <label style="cursor:pointer;"><input type="checkbox" class="cbLine" value="power" checked /> <span style="color:#4a148c;">&#9632;</span> Công suất</label>
                <label style="cursor:pointer;"><input type="checkbox" class="cbLine" value="pressure" checked /> <span style="color:#2e7d32;">&#9632;</span> Áp suất</label>
                <label style="cursor:pointer;"><input type="checkbox" class="cbLine" value="energy" checked /> <span style="color:#f9a825;">&#9632;</span> Năng lượng</label>
                <label style="cursor:pointer;"><input type="checkbox" class="cbLine" value="rpm" checked /> <span style="color:#1565c0;">&#9632;</span> Tốc độ</label>
                <label style="cursor:pointer;"><input type="checkbox" class="cbLine" value="ram" checked /> <span style="color:#212121;">&#9632;</span> Chày nén</label>
            </div>
            <!-- Thông tin thống kê -->
            <div id="chartStats" style="padding:8px 25px; background:#fffde7; border-bottom:1px solid #fff9c4;
                 font-size:14px; font-weight:500; color:#5d4037;"></div>
            <!-- Canvas biểu đồ -->
            <div style="padding:15px 25px 25px 25px;">
                <canvas id="mixerChart" style="width:100%; height:500px;"></canvas>
            </div>
        </div>
    </div>

    <!-- Chart.js CDN -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.7/dist/chart.umd.min.js"></script>

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
    function filterGridView() {
        // Lấy giá trị từ ô input
        var input = document.getElementById("txtQuickSearch");
        var filter = input.value.toLowerCase();

        // Tìm đến bảng dữ liệu (GridView sử dụng class data-table)
        var table = document.querySelector(".data-table");
        if (!table) return;

        var tr = table.getElementsByTagName("tr");

        // Lặp qua tất cả các hàng (bỏ qua hàng tiêu đề i=1)
        for (var i = 1; i < tr.length; i++) {
            // Không lọc hàng "EmptyDataText" nếu có
            if (tr[i].classList.contains("empty-row")) continue;

            var visible = false;
            var td = tr[i].getElementsByTagName("td");

            // Chạy qua từng cột trong hàng để kiểm tra từ khóa
            for (var j = 0; j < td.length; j++) {
                if (td[j]) {
                    var textValue = td[j].textContent || td[j].innerText;
                    if (textValue.toLowerCase().indexOf(filter) > -1) {
                        visible = true;
                        break;
                    }
                }
            }

            // Hiển thị hoặc ẩn hàng
            tr[i].style.display = visible ? "" : "none";
        }
    }

    // ============= MIXER CURVE CHART =============
    var _chartData = null;
    var _mixerChart = null;

    function showMixerChart() {
        var raw = document.getElementById('<%= hdChartData.ClientID %>').value;
        if (!raw) { alert('Không có dữ liệu biểu đồ!'); return; }

        _chartData = JSON.parse(raw);
        var recipe = document.getElementById('<%= hdChartRecipe.ClientID %>').value;
        var mesid = document.getElementById('<%= hdChartMesid.ClientID %>').value;
        var may = document.getElementById('<%= hdChartMay.ClientID %>').value;

        document.getElementById('chartTitle').innerText =
            'Recipe: ' + recipe + '  |  MES: ' + mesid + '  |  ' + may;

        // Populate barcode dropdown
        var sel = document.getElementById('selBarcode');
        sel.innerHTML = '';
        for (var i = 0; i < _chartData.length; i++) {
            var opt = document.createElement('option');
            opt.value = i;
            opt.text = _chartData[i].barcode + ' (Mẻ ' + (i + 1) + ')';
            sel.appendChild(opt);
        }
        // Add "Tất cả" option for overlay comparison
        if (_chartData.length > 1) {
            var optAll = document.createElement('option');
            optAll.value = 'all';
            optAll.text = 'So sánh tất cả (' + _chartData.length + ' mẻ)';
            sel.appendChild(optAll);
        }

        // Bind checkbox events
        var cbs = document.querySelectorAll('.cbLine');
        for (var c = 0; c < cbs.length; c++) {
            cbs[c].onchange = function () { renderSelectedBarcode(); };
        }

        document.getElementById('chartModal').style.display = 'block';
        document.body.style.overflow = 'hidden';

        renderSelectedBarcode();
    }

    function closeChart() {
        document.getElementById('chartModal').style.display = 'none';
        document.body.style.overflow = '';
        if (_mixerChart) { _mixerChart.destroy(); _mixerChart = null; }
    }

    // Close modal on ESC or background click
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && document.getElementById('chartModal').style.display === 'block') closeChart();
    });
    document.getElementById('chartModal').addEventListener('click', function (e) {
        if (e.target === this) closeChart();
    });

    function getVisibleLines() {
        var v = {};
        var cbs = document.querySelectorAll('.cbLine');
        for (var c = 0; c < cbs.length; c++) { v[cbs[c].value] = cbs[c].checked; }
        return v;
    }

    function renderSelectedBarcode() {
        var sel = document.getElementById('selBarcode');
        var idx = sel.value;
        var vis = getVisibleLines();

        if (_mixerChart) { _mixerChart.destroy(); _mixerChart = null; }

        if (idx === 'all') {
            renderOverlayChart(vis);
        } else {
            renderSingleChart(parseInt(idx), vis);
        }
    }

    function renderSingleChart(idx, vis) {
        var d = _chartData[idx];
        var datasets = [];

        // Nhiệt độ (Temperature) - Đỏ - trục Y trái
        if (vis.temp) {
            datasets.push({
                label: 'Nhiệt độ (°C)',
                data: d.temp,
                borderColor: '#d32f2f',
                backgroundColor: 'rgba(211,47,47,0.05)',
                borderWidth: 2,
                pointRadius: 0,
                tension: 0.3,
                yAxisID: 'yLeft',
                order: 2
            });
        }
        // Công suất (Power) - Tím đậm - trục Y trái
        if (vis.power) {
            datasets.push({
                label: 'Công suất (kW)',
                data: d.power,
                borderColor: '#4a148c',
                backgroundColor: 'rgba(74,20,140,0.05)',
                borderWidth: 2,
                pointRadius: 0,
                tension: 0.3,
                yAxisID: 'yLeft',
                order: 3
            });
        }
        // Chày nén (Ram Position) - Đen - trục Y trái
        if (vis.ram) {
            datasets.push({
                label: 'Chày nén (Ram)',
                data: d.ram,
                borderColor: '#212121',
                backgroundColor: 'rgba(33,33,33,0.05)',
                borderWidth: 1.5,
                pointRadius: 0,
                tension: 0.3,
                yAxisID: 'yLeft',
                order: 5
            });
        }
        // Tốc độ (RPM) - Xanh dương - trục Y trái
        if (vis.rpm) {
            datasets.push({
                label: 'Tốc độ (RPM)',
                data: d.rpm,
                borderColor: '#1565c0',
                backgroundColor: 'rgba(21,101,192,0.05)',
                borderWidth: 2,
                pointRadius: 0,
                tension: 0.3,
                yAxisID: 'yLeft',
                order: 4
            });
        }
        // Áp suất (Pressure) - Xanh lá - trục Y phải
        if (vis.pressure) {
            datasets.push({
                label: 'Áp suất (MPa)',
                data: d.pressure,
                borderColor: '#2e7d32',
                backgroundColor: 'rgba(46,125,50,0.05)',
                borderWidth: 2,
                pointRadius: 0,
                tension: 0.3,
                yAxisID: 'yRight',
                order: 6
            });
        }
        // Năng lượng (Energy) - Vàng - trục Y phải
        if (vis.energy) {
            datasets.push({
                label: 'Năng lượng (kWh)',
                data: d.energy,
                borderColor: '#f9a825',
                backgroundColor: 'rgba(249,168,37,0.05)',
                borderWidth: 2,
                pointRadius: 0,
                tension: 0.3,
                yAxisID: 'yRight',
                order: 7
            });
        }

        // Thống kê
        var maxTemp = d.temp.length > 0 ? Math.max.apply(null, d.temp) : 0;
        var maxEnergy = d.energy.length > 0 ? Math.max.apply(null, d.energy) : 0;
        var maxPower = d.power.length > 0 ? Math.max.apply(null, d.power) : 0;
        var duration = d.time.length > 0 ? d.time[d.time.length - 1] : 0;

        document.getElementById('chartStats').innerHTML =
            'Barcode: <b>' + d.barcode + '</b> &nbsp;|&nbsp; ' +
            'Nhiệt độ max: <b>' + maxTemp.toFixed(1) + ' °C</b> &nbsp;|&nbsp; ' +
            'Năng lượng: <b>' + maxEnergy.toFixed(1) + ' kWh</b> &nbsp;|&nbsp; ' +
            'Công suất max: <b>' + maxPower.toFixed(1) + ' kW</b> &nbsp;|&nbsp; ' +
            'Thời gian: <b>' + duration + ' s</b>';

        createChart(d.time, datasets);
    }

    function renderOverlayChart(vis) {
        var datasets = [];
        var colors = ['#d32f2f', '#1565c0', '#2e7d32', '#f9a825', '#4a148c', '#e65100', '#00695c'];

        for (var i = 0; i < _chartData.length; i++) {
            var d = _chartData[i];
            var color = colors[i % colors.length];
            datasets.push({
                label: 'Mẻ ' + (i + 1) + ' (' + d.barcode.slice(-3) + ') - Nhiệt độ',
                data: d.temp,
                borderColor: color,
                borderWidth: 2,
                pointRadius: 0,
                tension: 0.3,
                yAxisID: 'yLeft'
            });
        }

        document.getElementById('chartStats').innerHTML =
            '<b>So sánh nhiệt độ ' + _chartData.length + ' mẻ</b> — Kiểm tra tính đồng nhất quá trình luyện';

        var maxLen = 0;
        for (var j = 0; j < _chartData.length; j++) {
            if (_chartData[j].time.length > maxLen) maxLen = _chartData[j].time.length;
        }
        var labels = [];
        for (var k = 1; k <= maxLen; k++) labels.push(k);

        createChart(labels, datasets);
    }

    function createChart(labels, datasets) {
        var ctx = document.getElementById('mixerChart').getContext('2d');

        // Tính max trục Y phải
        var maxRight = 12;
        for (var i = 0; i < datasets.length; i++) {
            if (datasets[i].yAxisID === 'yRight') {
                var mx = Math.max.apply(null, datasets[i].data);
                if (mx > maxRight) maxRight = Math.ceil(mx / 5) * 5 + 5;
            }
        }

        _mixerChart = new Chart(ctx, {
            type: 'line',
            data: { labels: labels, datasets: datasets },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: { duration: 500 },
                interaction: {
                    mode: 'index',
                    intersect: false
                },
                plugins: {
                    legend: {
                        position: 'top',
                        labels: {
                            usePointStyle: true,
                            padding: 15,
                            font: { size: 13, weight: '600' }
                        }
                    },
                    tooltip: {
                        backgroundColor: 'rgba(0,0,0,0.85)',
                        titleFont: { size: 14, weight: 'bold' },
                        bodyFont: { size: 13 },
                        padding: 12,
                        callbacks: {
                            title: function (items) {
                                return 'Thời gian: ' + items[0].label + ' s';
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: 'Thời gian luyện (giây)',
                            font: { size: 14, weight: 'bold' }
                        },
                        ticks: {
                            maxTicksLimit: 30,
                            font: { size: 12 }
                        },
                        grid: { color: 'rgba(0,0,0,0.06)' }
                    },
                    yLeft: {
                        type: 'linear',
                        position: 'left',
                        min: 0,
                        max: 200,
                        title: {
                            display: true,
                            text: 'Nhiệt độ (°C) / Công suất (kW) / RPM / Ram',
                            font: { size: 13, weight: 'bold' },
                            color: '#d32f2f'
                        },
                        ticks: {
                            stepSize: 20,
                            font: { size: 12 },
                            color: '#d32f2f'
                        },
                        grid: {
                            color: 'rgba(255,235,59,0.3)',
                            lineWidth: 1
                        }
                    },
                    yRight: {
                        type: 'linear',
                        position: 'right',
                        min: 0,
                        max: maxRight,
                        title: {
                            display: true,
                            text: 'Áp suất (MPa) / Năng lượng (kWh)',
                            font: { size: 13, weight: 'bold' },
                            color: '#2e7d32'
                        },
                        ticks: {
                            font: { size: 12 },
                            color: '#2e7d32'
                        },
                        grid: { drawOnChartArea: false }
                    }
                }
            }
        });
    }
</script>
</asp:Content>