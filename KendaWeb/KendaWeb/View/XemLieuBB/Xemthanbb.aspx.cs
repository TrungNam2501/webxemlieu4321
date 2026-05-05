using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KendaWeb.View.XemLieuBB
{
    public partial class Xemthanbb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                
            }
        }

        private string ChonMay(string May)
        {
            string ip = string.Empty;
            switch (May)
            {
                case "01": ip = "198.1.8.21"; break;
                case "02": ip = "198.1.8.22"; break;
                case "03": ip = "198.1.8.23"; break;
                case "04": ip = "198.1.8.24"; break;
                case "05": ip = "198.1.8.35"; break;
                case "06": ip = "198.1.8.36"; break;
                case "07": ip = "198.1.8.37"; break;
                case "08": ip = "198.1.8.38"; break;
                default: return "";
            }

            if (PingIp(ip) == 1)
            {
                return $"Data Source={ip}; Initial Catalog=mfns; User ID=kendakv2; Password=kenda123";
            }
            return "";
        }

        public static int PingIp(string ip)
        {
            try
            {
                if (string.IsNullOrEmpty(ip)) return 0;
                Ping ping = new Ping();
                // Timeout 100ms thay vì 10ms để ổn định hơn
                PingReply pingresult = ping.Send(ip, 100);
                return (pingresult.Status == IPStatus.Success) ? 1 : 0;
            }
            catch { return -1; }
        }

        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "showMessage();", true);
        }

        private void LoadData()
        {
            DataTable dt = CreateSampleData();

            gvData.DataSource = dt;
            gvData.DataBind();

            // ✅ Đếm số dòng
            if (dt != null)
            {
                lblRowCount.Text = $"Tổng số dòng: {dt.Rows.Count}";
            }
            else
            {
                lblRowCount.Text = "Tổng số dòng: 0";
            }
            // ✅ Thống kê than từ dtMain
            DataTable dtThan = ThongKeThan(dt);
            gvThongKeThan.DataSource = dtThan;
            gvThongKeThan.DataBind();
        }

        private DataTable ThongKeThan(DataTable dt)
        {
            DataTable dtThan = new DataTable();
            dtThan.Columns.Add("Loaithan", typeof(string));
           
            dtThan.Columns.Add("SLthucte", typeof(string));
            dtThan.Columns.Add("SLquettem", typeof(string));

            string dataMay = ddlMachine.SelectedValue;
            string ngayChon = txtDate.Text.Trim();

            // 1. Kiểm tra việc chọn máy
            if (string.IsNullOrEmpty(dataMay) || dataMay == "0")
            {
                ThongBao("Vui lòng chọn máy cụ thể trước khi xem dữ liệu!");
                return null;
            }

            // 2. Kiểm tra định dạng ngày tháng
            if (string.IsNullOrEmpty(ngayChon) || !DateTime.TryParse(ngayChon, out _))
            {
                ThongBao("Định dạng ngày tháng không hợp lệ (yyyy-MM-dd)!");
                return null;
            }

            // 3. Lấy chuỗi kết nối và kiểm tra Ping (thực hiện bên trong ChonMay)
            string connectionString = ChonMay(dataMay);
            if (string.IsNullOrEmpty(connectionString))
            {
                ThongBao($"Không thể kết nối đến máy {dataMay}. Vui lòng kiểm tra lại mạng hoặc IP!");
                return null;
            }

          


            try
            {
                string slTongThucte = "SELECT [mater_code] ,[real_weight] FROM [mfns].[dbo].[ppt_weigh] " +
               "WHERE mater_code like '60%' and  weigh_time >= DATEADD(HOUR, 6.5, CAST('" + ngayChon + "' AS DATETIME)) " +
               "AND weigh_time <  DATEADD(HOUR, 6.5, DATEADD(DAY, 1, CAST('" + ngayChon + "' AS DATETIME)));";
                DataTable dtSLTongThucte = Cnn.ExecuteQuery(connectionString, slTongThucte);

                if (dtSLTongThucte != null && dtSLTongThucte.Rows.Count > 0 && dt !=null)
                {
                    var groupThucTe = dtSLTongThucte.AsEnumerable()
                                         .GroupBy(r => r["mater_code"].ToString().Trim())
                                         .Select(g => new
                                         {
                                             Loaithan = g.Key,
                                             SLthucte = g.Sum(x => Convert.ToDecimal(x["real_weight"]))
                                         }).ToList();

                    var groupQuetTem = dt.AsEnumerable()
                                        .GroupBy(r => r["mater_code"].ToString().Trim())
                                        .ToDictionary(
                                            g => g.Key,
                                            g => g.Sum(x => Convert.ToDecimal(x["PerPackageWeight"]))
                                        );

                    foreach (var item in groupThucTe)
                    {
                        DataRow row = dtThan.NewRow();
                        row["Loaithan"] = item.Loaithan;
                        row["SLthucte"] = item.SLthucte;

                        // so sánh đã được trim nên match chuẩn
                        if (groupQuetTem.ContainsKey(item.Loaithan))
                            row["SLquettem"] = groupQuetTem[item.Loaithan];
                        else
                            row["SLquettem"] = 0;

                        dtThan.Rows.Add(row);
                    }


                    return dtThan;
                }
                else
                {
                    ThongBao("Không tìm thấy dữ liệu cho máy và ngày đã chọn.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // 6. Bắt các lỗi SQL (Sai tên bảng, mất kết nối bất ngờ, ...)
                ThongBao("Lỗi hệ thống: " + ex.Message);
                return null;
            }




            
        }

        private DataTable CreateSampleData()
        {
            string dataMay = ddlMachine.SelectedValue;
            string ngayChon = txtDate.Text.Trim();

            // 1. Kiểm tra việc chọn máy
            if (string.IsNullOrEmpty(dataMay) || dataMay == "0")
            {
                ThongBao("Vui lòng chọn máy cụ thể trước khi xem dữ liệu!");
                return null;
            }

            // 2. Kiểm tra định dạng ngày tháng
            if (string.IsNullOrEmpty(ngayChon) || !DateTime.TryParse(ngayChon, out _))
            {
                ThongBao("Định dạng ngày tháng không hợp lệ (yyyy-MM-dd)!");
                return null;
            }

            // 3. Lấy chuỗi kết nối và kiểm tra Ping (thực hiện bên trong ChonMay)
            string connectionString = ChonMay(dataMay);
            if (string.IsNullOrEmpty(connectionString))
            {
                ThongBao($"Không thể kết nối đến máy {dataMay}. Vui lòng kiểm tra lại mạng hoặc IP!");
                return null;
            }

            // 4. Chuẩn bị câu truy vấn
            string sqlTruyvan = $@"
        SELECT a.SaveTime,a.Equip_ID, a.Mater_Code, a.Mater_Name, a.Mater_Barcode, 
               b.LotNumber, b.PerPackageWeight, b.ProductDate, b.ValidDate 
        FROM [mfns].[dbo].[Ppt_BarCodeRep] a 
        JOIN [mfns].[dbo].[Mes2RawMaterial] b ON a.Mater_Barcode = b.Barcode 
        WHERE a.Mater_Barcode LIKE '60%' 
          AND a.Mater_Type = '0' 
          AND a.SaveTime >= CONVERT(VARCHAR(19), DATEADD(MINUTE, 390, CAST('{ngayChon}' AS DATETIME)), 120) 
          AND a.SaveTime < CONVERT(VARCHAR(19), DATEADD(DAY, 1, DATEADD(MINUTE, 390, CAST('{ngayChon}' AS DATETIME))), 120) 
        ORDER BY a.SaveTime DESC;";

            try
            {
                // 5. Thực thi truy vấn
                DataTable dtTc = Cnn.ExecuteQuery(connectionString, sqlTruyvan);

                if (dtTc != null && dtTc.Rows.Count > 0)
                {
                    return dtTc;
                }
                else
                {
                    ThongBao("Không tìm thấy dữ liệu cho máy và ngày đã chọn.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // 6. Bắt các lỗi SQL (Sai tên bảng, mất kết nối bất ngờ, ...)
                ThongBao("Lỗi hệ thống: " + ex.Message);
                return null;
            }
        }

        protected void btnXemLieu_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = CreateSampleData();

            if (dt == null || dt.Rows.Count == 0)
            {
                ThongBao("Không có dữ liệu để xuất Excel!");
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Dữ Liệu Vật Tư");

                // Định nghĩa Header mới khớp với SQL
                string[] headers = { "Thời gian quét","ID Máy", "Mã NL", "Tên Nguyên Liệu", "Barcode", "Số Lô", "Trọng Lượng", "Ngày SX", "Hạn Dùng" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(1, i + 1).Value = headers[i];
                }

                // Đổ dữ liệu từ DataTable vào Excel (bắt đầu từ dòng 2)
                ws.Cell(2, 1).InsertData(dt.AsEnumerable());

                // Style header
                var headerRange = ws.Range(1, 1, 1, headers.Length);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(102, 126, 234);
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Columns().AdjustToContents();

                // Xuất file
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", $"attachment; filename=LieuBB_{ddlMachine.SelectedItem.Text}_{DateTime.Now:yyyyMMddHHmm}.xlsx");

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    Response.BinaryWrite(stream.ToArray());
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}