using ClosedXML.Excel;
using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace KendaWeb.View.XemLieuBB
{
    public partial class Xuatdulieubieudo : System.Web.UI.Page
    {
        private readonly Dictionary<string, string> cnnstr = new Dictionary<string, string>
        {
            { "01", @"Data Source=198.1.8.21;Initial Catalog=mfns;User ID=kendakv2;Password=kenda123" },
            { "02", @"Data Source=198.1.8.22;Initial Catalog=mfns;User ID=kendakv2;Password=kenda123" },
            { "03", @"Data Source=198.1.8.23;Initial Catalog=mfns;User ID=kendakv2;Password=kenda123" },
            { "04", @"Data Source=198.1.8.24;Initial Catalog=mfns;User ID=kendakv2;Password=kenda123" },
            { "05", @"Data Source=198.1.8.35;Initial Catalog=mfns;User ID=kendakv2;Password=kenda123" },
            { "06", @"Data Source=198.1.8.36;Initial Catalog=mfns;User ID=kendakv2;Password=kenda123" },
            { "07", @"Data Source=198.1.8.37;Initial Catalog=mfns;User ID=kendakv2;Password=kenda123" },
            { "08", @"Data Source=198.1.8.38;Initial Catalog=mfns;User ID=kendakv2;Password=kenda123" }
        };

        // Hàm dùng chung để hiển thị Alert
        private void ShowAlert(string message)
        {
            string cleanMessage = message.Replace("'", "\\'").Replace("\r", "").Replace("\n", "\\n");
            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", $"alert('{cleanMessage}');", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string cbMay = ddlMay.SelectedValue;
                string ngayChon = txtDate.Text.Trim();

                if (string.IsNullOrEmpty(cbMay) || string.IsNullOrEmpty(ngayChon))
                {
                    ShowAlert("Vui lòng chọn đầy đủ máy và ngày!");
                    return;
                }

                if (!cnnstr.TryGetValue(cbMay, out string cnnMay)) return;

                // Câu lệnh SQL lấy danh sách GroupLot
                string sql = $@"SELECT RecipeCode, SetNumber, Start_datetime, End_datetime, FinishNum, Plan_ID, MesPlanID 
                               FROM [mfns].[dbo].[Ppt_GroupLot] 
                               WHERE Start_datetime >= DATEADD(MINUTE, 390, CAST('{ngayChon}' AS DATETIME)) 
                                 AND Start_datetime < DATEADD(MINUTE, 390, DATEADD(DAY, 1, CAST('{ngayChon}' AS DATETIME))) 
                               ORDER BY Start_datetime";

                // QUAN TRỌNG: Đảm bảo thứ tự tham số (Connection String trước, SQL sau hoặc ngược lại tùy lớp Cnn của bạn)
                // Theo lỗi bạn gặp, có vẻ Cnn.ExecuteQuery yêu cầu (sql, cnnString)
                DataTable dt = Cnn.ExecuteQuery(cnnMay,sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowAlert("Không có dữ liệu trong khoảng thời gian này!");
                }

                lblMayHienTai.Text = ddlMay.SelectedItem.Text;
                lblNgayHienTai.Text = ngayChon;

                gvDuLieu.DataSource = dt;
                gvDuLieu.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        protected void btnXuat_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            // Lấy dữ liệu từ GridView (Sử dụng HttpUtility.HtmlDecode để tránh lỗi ký tự &nbsp;)
            string recipeCode = HttpUtility.HtmlDecode(row.Cells[0].Text).Trim();
            string planId = HttpUtility.HtmlDecode(row.Cells[5].Text).Trim();
            string mesid = HttpUtility.HtmlDecode(row.Cells[6].Text).Trim();

            XuatExcelQuaTrinhDuyet(planId, ddlMay.SelectedValue, recipeCode,mesid);
        }

        private void XuatExcelQuaTrinhDuyet(string planID, string mayXuat, string recipe, string mesid)
        {
            try
            {
                if (!cnnstr.TryGetValue(mayXuat, out string cnnMay)) return;

                string sql = $"SELECT * FROM [mfns].[dbo].[Ppt_curvedata] WHERE Barcode LIKE '{planID.Trim()}%'";

                // Đã sửa lại thứ tự tham số dựa trên phản hồi lỗi của bạn
                DataTable dt_Curve = Cnn.ExecuteQuery(cnnMay,sql);

                if (dt_Curve == null || dt_Curve.Rows.Count == 0)
                {
                    ShowAlert("Không tìm thấy dữ liệu biểu đồ cho Barcode này!");
                    return;
                }

                foreach (DataRow row in dt_Curve.Rows)
                {
                    if (row["curve_data"] != DBNull.Value)
                        row["curve_data"] = row["curve_data"].ToString().Replace("/", "\n");
                }

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add(dt_Curve, "CurveData");

                    // Định dạng cột curve_data (Cột 2)
                    var curveColumn = ws.Column(2);
                    curveColumn.Width = 80;
                    curveColumn.Style.Alignment.WrapText = true;
                    curveColumn.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                    ws.Columns().AdjustToContents();

                    string fileName = $"{recipe}_{mesid}.xlsx";

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        // Dùng Context.ApplicationInstance.CompleteRequest() thay cho Response.End() để tránh Exception
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    ShowAlert("Lỗi xuất file: " + ex.Message);
                }
            }
        }

        protected void gvDuLieu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (DateTime.TryParse(e.Row.Cells[2].Text, out DateTime start))
                    e.Row.Cells[2].Text = start.ToString("dd/MM/yyyy HH:mm:ss");

                if (DateTime.TryParse(e.Row.Cells[3].Text, out DateTime end))
                    e.Row.Cells[3].Text = end.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        protected void btnBieuDo_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            string recipeCode = HttpUtility.HtmlDecode(row.Cells[0].Text).Trim();
            string planId = HttpUtility.HtmlDecode(row.Cells[5].Text).Trim();
            string mesid = HttpUtility.HtmlDecode(row.Cells[6].Text).Trim();
            string mayXuat = ddlMay.SelectedValue;

            if (string.IsNullOrEmpty(planId)) return;

            try
            {
                if (!cnnstr.TryGetValue(mayXuat.Trim(), out string cnnMay)) return;

                string sql = $"SELECT Barcode, curve_data FROM [mfns].[dbo].[Ppt_curvedata] WHERE Barcode LIKE '{planId.Trim()}%'";
                DataTable dt = Cnn.ExecuteQuery(cnnMay, sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowAlert("Không tìm thấy dữ liệu biểu đồ!");
                    return;
                }

                var allBarcodes = new List<object>();
                foreach (DataRow drow in dt.Rows)
                {
                    string barcode = drow["Barcode"].ToString().Trim();
                    string rawCurve = drow["curve_data"] != DBNull.Value ? drow["curve_data"].ToString() : "";
                    string[] lines = rawCurve.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                    var time = new List<double>();
                    var temp = new List<double>();
                    var power = new List<double>();
                    var energy = new List<double>();
                    var pressure = new List<double>();
                    var rpm = new List<double>();
                    var ram = new List<double>();

                    foreach (string line in lines)
                    {
                        string[] cols = line.Trim().Split(':');
                        if (cols.Length >= 7)
                        {
                            if (double.TryParse(cols[0], out double t)) time.Add(t); else continue;
                            if (double.TryParse(cols[1], out double te)) temp.Add(te); else temp.Add(0);
                            if (double.TryParse(cols[2], out double pw)) power.Add(pw); else power.Add(0);
                            if (double.TryParse(cols[3], out double en)) energy.Add(en); else energy.Add(0);
                            if (double.TryParse(cols[4], out double pr)) pressure.Add(pr); else pressure.Add(0);
                            if (double.TryParse(cols[5], out double rp)) rpm.Add(rp); else rpm.Add(0);
                            if (double.TryParse(cols[6], out double ra)) ram.Add(ra); else ram.Add(0);
                        }
                    }

                    allBarcodes.Add(new
                    {
                        barcode = barcode,
                        time = time,
                        temp = temp,
                        power = power,
                        energy = energy,
                        pressure = pressure,
                        rpm = rpm,
                        ram = ram
                    });
                }

                var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
                string json = serializer.Serialize(allBarcodes);
                hdChartData.Value = json;
                hdChartRecipe.Value = recipeCode;
                hdChartMesid.Value = mesid;
                hdChartMay.Value = ddlMay.SelectedItem.Text;

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowChart", "showMixerChart();", true);
            }
            catch (Exception ex)
            {
                ShowAlert("Lỗi tải biểu đồ: " + ex.Message);
            }
        }
        protected void btnXuatTxt_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            // Lấy thông tin từ các cột trong GridView
            string recipeCode = HttpUtility.HtmlDecode(row.Cells[0].Text).Trim();
            string planId = HttpUtility.HtmlDecode(row.Cells[5].Text).Trim();
            string mesid = HttpUtility.HtmlDecode(row.Cells[6].Text).Trim();
            string mayXuat = ddlMay.SelectedValue;

            if (!string.IsNullOrEmpty(planId))
            {
                XuatFileTxtNguyenBan(planId, mayXuat, recipeCode, mesid);
            }
        }

        private void XuatFileTxtNguyenBan(string planID, string mayXuat, string recipe,string mesid)
        {
            try
            {
                if (!cnnstr.TryGetValue(mayXuat.Trim(), out string cnnMay)) return;

                string sql = $"SELECT * FROM [mfns].[dbo].[Ppt_curvedata] WHERE Barcode LIKE '{planID.Trim()}%'";
                DataTable dt = Cnn.ExecuteQuery(cnnMay,sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowAlert("Không tìm thấy dữ liệu để xuất TXT!");
                    return;
                }

                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    sb.AppendLine($"Barcode: {row["Barcode"]}");
                    sb.AppendLine("-----------------------------------------");
                    if (row["curve_data"] != DBNull.Value)
                    {
                        // Thay thế ký tự phân cách bằng xuống dòng thực tế
                        sb.AppendLine(row["curve_data"].ToString().Replace("/", Environment.NewLine));
                    }
                    sb.AppendLine("=========================================");
                    sb.AppendLine();
                }

                string fileName = $"{recipe}_{mesid}.txt";
                // 1. Xóa sạch buffer trước khi ghi dữ liệu mới
                Response.Clear();
                Response.ClearContent(); // Thêm dòng này để chắc chắn xóa hết nội dung cũ
                Response.ClearHeaders();

                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.ContentType = "text/plain";
                Response.Charset = "UTF-8"; // Nên chỉ định rõ encoding để tránh lỗi font

                // 2. Ghi nội dung file
                Response.Write(sb.ToString());

                // 3. QUAN TRỌNG: Kết thúc phản hồi ngay lập tức
                Response.Flush();
                // Sử dụng Response.End() là cách nhanh nhất để ngăn HTML chèn vào
                // Tuy nhiên nó sẽ gây ra ThreadAbortException (đã có catch xử lý)
                Response.End();
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                    ShowAlert("Lỗi xuất TXT: " + ex.Message);
            }
        }
    }
}