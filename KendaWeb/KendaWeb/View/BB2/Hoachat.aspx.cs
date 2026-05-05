using ClosedXML.Excel;
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

namespace KendaWeb.View.BB2
{
    public partial class Hoachat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (txtTuNgay.Text == "" || txtDenNgay.Text == "")
            {
                string strdate = DateTime.Now.ToString("yyyy-MM-dd");
                txtTuNgay.Text = strdate;
                txtDenNgay.Text = strdate;
            }
        }

        protected void gvKQHCMoi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string PlanId = e.CommandArgument.ToString();
            if (e.CommandName == "btnIn")
            {
                string[] may = CheckMay();
                if (may[0] != null)
                {
                    ThongBao(may[0]);
                }
                else
                {
                    string connectionstring = "Data Source=" + may[1] + ";Initial Catalog=CWSS_S7;User ID=kendakv2;Password=kenda123";
                    string getData = "SELECT [Plan_id],[Equip_code] ,[Weight_ID],[Material_Code],[Material_Name],[Set_Weight],[Set_Error] FROM [CWSS_S7].[dbo].[LR_recipe] WHERE Plan_id='" + PlanId + "'";
                    DataTable dtLrrecipe = Cnn.ExecuteQuery(connectionstring, getData);
                    if (dtLrrecipe.Rows.Count == 0)
                    {
                        string script = "alert(\"Không có dữ liệu!!(Mes bạn chọn là mes trong giai đoạn thử nghiệm)\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                    else
                    {
                        TempData.tableLrRecipe = dtLrrecipe;
                        gvInHC.DataSource = dtLrrecipe;
                        gvInHC.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvInHC();", true);
                    }
                }
            }
            if (e.CommandName == "btnOut")
            {
                string[] may = CheckMay();
                if (may[0] != null)
                {
                    ThongBao(may[0]);
                }
                else
                {
                    string connectionstring = "Data Source=" + may[1] + ";Initial Catalog=CWSS_S7;User ID=kendakv2;Password=kenda123";
                    string getData = "  SELECT [Plan_id],[Equip_code] ,[Serial_Num],[Prd_date],[Real_weight],[Real_Error],[Waste_Time] ,[Net_Weight] FROM [CWSS_S7].[dbo].[LR_lot] WHERE Plan_id='" + PlanId + "' ORDER BY Serial_Num asc";
                    DataTable dtLrlot = Cnn.ExecuteQuery(connectionstring, getData);
                    if (dtLrlot.Rows.Count == 0)
                    {
                        string script = "alert(\"Không có dữ liệu!! (Mes bạn chọn là mes trong giai đoạn thử nghiệm)\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                    else
                    {
                        TempData.tableLrLot = dtLrlot;
                        gvOutHC.DataSource = dtLrlot;
                        gvOutHC.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvOutHC();", true);
                    }
                }
            }

        }

        protected void txtDenNgay_TextChanged(object sender, EventArgs e)
        {
            int datetu = int.Parse(txtTuNgay.Text.Replace("-", "").Trim());
            int dateden = int.Parse(txtDenNgay.Text.Replace("-", "").Trim());

            if (dateden >= datetu)
            {
                return;
            }
            else
            {
                ThongBao("Dữ liệu không hợp lệ!!!");
                txtDenNgay.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

        }
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }
        public string[] CheckMay()
        {
            string[] result = new string[2];
            if (drMay.SelectedValue == "")
            {
                result[0] = "Vui lòng chọn máy!!!";
                return result;
            }
            try
            {
                Ping ping = new Ping();
                PingReply pingresult = ping.Send(drMay.SelectedValue, 10);
                if (pingresult.Status.ToString() == "Success")
                {
                    result[1] = drMay.SelectedValue;
                    return result;
                }
                else
                {
                    result[0] = "Máy đang tắt, bật máy lên và thử lại!!!";
                    return result;
                }
            }
            catch (Exception)
            {
                result[0] = "Không thể kết nối đến máy chủ!!!";
                return result;
            }
        }


        protected void btnXemLieu_Click(object sender, EventArgs e)
        {
            txtTimkiem.Text = "";
            LoadData();
        }
        public void LoadData()
        {
            string planid = "";
            string[] may = CheckMay();
            if (may[0] != null)
            {
                ThongBao(may[0]);
                gvKQHCMoi.DataSource = null;
                gvKQHCMoi.DataBind();
            }
            else
            {


                if (may[1] == "198.1.8.17" || may[1] == "198.1.8.18" || may[1] == "198.1.8.15" || may[1] == "198.1.8.16")
                {
                    switch (may[1])
                    {

                        case "198.1.8.15":
                            planid = "V13";
                            break;
                        case "198.1.8.16":
                            planid = "V11";
                            break;
                        case "198.1.8.17":
                            planid = "V12";
                            break;
                        case "198.1.8.18":
                            planid = "V14";
                            break;

                    }
                    string connectionstring = "Data Source=" + may[1] + ";Initial Catalog=CWSS_S7;User ID=kendakv2;Password=kenda123";
                    string TuNgay = txtTuNgay.Text;
                    string DenNgay = txtDenNgay.Text;

                    //string getData = " SELECT [Plan_Id],[Equip_Code],[Recipe_Name],[Shift_Id],[Plan_Num],[Real_Num] ,[Plan_Batch] ,[Start_Date]  ,[End_Date],[Plan_Date] FROM [CWSS_S7].[dbo].[LR_plan] WHERE Plan_Id LIKE '" + planid + "%' AND End_Date != '' AND Cast(Start_Date as date) between '" + TuNgay + "' and '" + DenNgay + "' order by Start_Date, Plan_Id";
                    string getData = @"
SELECT 
    p.[Plan_Id],
    p.[Equip_Code],
    p.[Recipe_Name],
    p.[Shift_Id],
    p.[Plan_Num],
    p.[Real_Num],
    p.[Plan_Batch],
    p.[Start_Date],
    p.[End_Date],
    p.[Plan_Date],
    -- Tổng Real_weight theo Plan_Id
    (
        SELECT SUM(l.Real_weight)
        FROM [CWSS_S7].[dbo].[LR_lot] l
        WHERE l.Plan_id = p.Plan_Id
    ) AS Real_Weight,
    -- Plan_Num * tổng Set_Weight theo Recipe_Name
    p.Plan_Num * (
        SELECT SUM(w.Set_Weight)
        FROM [CWSS_S7].[dbo].[Pmt_weigh] w
        WHERE w.Recipe_ID = p.Recipe_Name
    ) AS sokgdieudong
FROM [CWSS_S7].[dbo].[LR_plan] p
WHERE p.Plan_Id LIKE '" + planid + @"%' 
  AND p.End_Date != '' 
  AND CAST(p.Start_Date AS date) BETWEEN '" + TuNgay + @"' AND '" + DenNgay + @"'
ORDER BY p.Start_Date, p.Plan_Id";

                    DataTable dt = Cnn.ExecuteQuery(connectionstring, getData);
                    if (dt.Rows.Count == 0)
                    {
                        gvKQHCMoi.DataSource = null;
                        gvKQHCMoi.DataBind();
                        string script = "alert(\"Không có dữ liệu!\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                    else
                    {
                        TempData.tableLrPlan = dt;
                        gvKQHCMoi.DataSource = dt;
                        gvKQHCMoi.DataBind();
                    }

                }
                else
                {
                    string connectionstring = "Data Source=" + may[1] + ";Initial Catalog=CWSS_S7;User ID=kendakv2;Password=kenda123";
                    string TuNgay = txtTuNgay.Text;
                    string DenNgay = txtDenNgay.Text;

                    string getData = " SELECT [Plan_Id],[Equip_Code],[Recipe_Name],[Shift_Id],[Plan_Num],[Real_Num],[Plan_Batch] ,[Start_Date] ,[End_Date],[Plan_Date] FROM [CWSS_S7].[dbo].[LR_plan] WHERE Plan_Id LIKE 'V12%' AND End_Date != '' AND Cast(Start_Date as date) between '" + TuNgay + "' and '" + DenNgay + "' order by Start_Date, Plan_Id";
                    DataTable dt = Cnn.ExecuteQuery(connectionstring, getData);
                    if (dt.Rows.Count == 0)
                    {
                        gvKQHCMoi.DataSource = null;
                        gvKQHCMoi.DataBind();
                        string script = "alert(\"Không có dữ liệu!\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                    else
                    {
                        TempData.tableLrPlan = dt;
                        gvKQHCMoi.DataSource = dt;
                        gvKQHCMoi.DataBind();
                    }

                }
            }
        }

        protected void btnExcelLrplan_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = TempData.tableLrPlan;
                exportExcel(dt, "lrplan");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            string planid = "";
            string[] may = CheckMay();
            if (may[0] != null)
            {
                ThongBao(may[0]);
                gvKQHCMoi.DataSource = null;
                gvKQHCMoi.DataBind();
            }
            else
            {
                if (txtTimkiem.Text == "")
                {
                    ThongBao("Vui lòng nhập tiêu chuẩn cần tìm kiếm ");
                    return;
                }
                else
                {
                    if (may[1] == "198.1.8.17" || may[1] == "198.1.8.18" || may[1] == "198.1.8.15" || may[1] == "198.1.8.16")
                    {
                        switch (may[1])
                        {

                            case "198.1.8.15":
                                planid = "V13";
                                break;
                            case "198.1.8.16":
                                planid = "V11";
                                break;
                            case "198.1.8.17":
                                planid = "V12";
                                break;
                            case "198.1.8.18":
                                planid = "V14";
                                break;

                        }
                        string connectionstring = "Data Source=" + may[1] + ";Initial Catalog=CWSS_S7;User ID=kendakv2;Password=kenda123";
                        string TuNgay = txtTuNgay.Text.Trim().ToString();
                        string DenNgay = txtDenNgay.Text.Trim().ToString();
                        string recipename = txtTimkiem.Text.Trim().ToString();
                        //string getData = " SELECT [Plan_Id],[Equip_Code],[Recipe_Name],[Shift_Id],[Plan_Num],[Real_Num], [Plan_Batch] ,[Start_Date]  ,[End_Date],[Plan_Date] FROM [CWSS_S7].[dbo].[LR_plan] WHERE Plan_Id LIKE '" + planid + "%' AND End_Date != '' AND Cast(Start_Date as date) between '" + TuNgay + "' and '" + DenNgay + "' and Recipe_Name LIKE '%" + recipename + "%' order by Start_Date, Plan_Id";
                        string getData = @"
SELECT 
    p.[Plan_Id],
    p.[Equip_Code],
    p.[Recipe_Name],
    p.[Shift_Id],
    p.[Plan_Num],
    p.[Real_Num],
    p.[Plan_Batch],
    p.[Start_Date],
    p.[End_Date],
    p.[Plan_Date],
    -- Tổng Real_weight theo Plan_Id
    (
        SELECT SUM(l.Real_weight)
        FROM [CWSS_S7].[dbo].[LR_lot] l
        WHERE l.Plan_id = p.Plan_Id
    ) AS Real_Weight,
    -- Plan_Num * tổng Set_Weight theo Recipe_Name
    p.Plan_Num * (
        SELECT SUM(w.Set_Weight)
        FROM [CWSS_S7].[dbo].[Pmt_weigh] w
        WHERE w.Recipe_ID = p.Recipe_Name
    ) AS sokgdieudong
FROM [CWSS_S7].[dbo].[LR_plan] p
WHERE p.Plan_Id LIKE '" + planid + @"%' 
  AND p.End_Date != '' 
  AND CAST(p.Start_Date AS date) BETWEEN '" + TuNgay + @"' AND '" + DenNgay + @"' 
    and p.Recipe_Name LIKE '%" + recipename + "%' ORDER BY p.Start_Date, p.Plan_Id";
                        DataTable dt = Cnn.ExecuteQuery(connectionstring, getData);
                        if (dt.Rows.Count == 0)
                        {
                            gvKQHCMoi.DataSource = null;
                            gvKQHCMoi.DataBind();
                            string script = "alert(\"khong co du lieu!\");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                        }
                        else
                        {
                            TempData.tableLrPlan = dt;
                            gvKQHCMoi.DataSource = dt;
                            gvKQHCMoi.DataBind();
                        }

                    }
                    else
                    {
                        string connectionstring = "Data Source=" + may[1] + ";Initial Catalog=CWSS_S7;User ID=kendakv2;Password=kenda123";
                        string TuNgay = txtTuNgay.Text.Trim().ToString();
                        string DenNgay = txtDenNgay.Text.Trim().ToString();
                        string recipename = txtTimkiem.Text.Trim().ToString();

                        string getData = " SELECT [Plan_Id],[Equip_Code],[Recipe_Name],[Shift_Id],[Plan_Num],[Real_Num] ,[Start_Date] ,[Plan_Batch] ,[End_Date],[Plan_Date] FROM [CWSS_S7].[dbo].[LR_plan] WHERE Plan_Id LIKE 'V12%' AND End_Date != '' AND Cast(Start_Date as date) between '" + TuNgay + "' and '" + DenNgay + "' and Recipe_Name LIKE '%" + recipename + "%' order by Start_Date, Plan_Id";
                        DataTable dt = Cnn.ExecuteQuery(connectionstring, getData);
                        if (dt.Rows.Count == 0)
                        {
                            gvKQHCMoi.DataSource = null;
                            gvKQHCMoi.DataBind();
                            string script = "alert(\"khong co du lieu!\");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                        }
                        else
                        {
                            TempData.tableLrPlan = dt;
                            gvKQHCMoi.DataSource = dt;
                            gvKQHCMoi.DataBind();
                        }

                    }
                }
            }


        }

        protected void btnExcelLrRecipe_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableLrRecipe, "lrrecipe");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void gvInHC_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string[] dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string Scanbar = dulieu[0].ToString().Trim().Substring(0, 5);
            string Material = dulieu[1].ToString().Trim();
            string PlanID = dulieu[2].ToString().Trim();
            string datemes = PlanID.Substring(3, 6);
            string namdate = datemes.Substring(0, 2);
            string thangdate = datemes.Substring(2, 2);
            string ngaydate = datemes.Substring(4, 2);
            string scantime = "20" + namdate + "-" + thangdate + "-" + ngaydate;
            if (e.CommandName == "btnLRbarcodeLog")
            {
                string[] may = CheckMay();
                if (may[0] != null)
                {
                    ThongBao(may[0]);
                }
                else
                {
                    string TuNgay = txtTuNgay.Text;
                    string DenNgay = txtDenNgay.Text;
                    string connectionstring = "Data Source=" + may[1] + ";Initial Catalog=CWSS_S7;User ID=kendakv2;Password=kenda123";
                    //string getData = "SELECT [Equip_Code],[Scan_Time],[Scan_Bar],[Material],[Bin],[Scan_State] FROM [CWSS_S7].[dbo].[LR_BarcodeLog] WHERE Scan_Bar LIKE '"+Scanbar+"%' AND Material LIKE '%"+Material+"%' AND Cast(Scan_Time as date) between '"+TuNgay+"' and '"+DenNgay+"'";
                    //string getData = "SELECT [Equip_Code],[Scan_Time],[Scan_Bar],[Material],[Bin],[Scan_State] FROM [CWSS_S7].[dbo].[LR_BarcodeLog] WHERE Scan_Bar LIKE '" + Scanbar + "%' AND Material LIKE '%" + Material + "%' AND Scan_Time LIKE '%" + scantime + "%'";
                    string getData = "  SELECT top 3 [Equip_Code],[Scan_Time],[Scan_Bar],[Material],[Bin],[Scan_State] FROM [CWSS_S7].[dbo].[LR_BarcodeLog] where [Material] = '" + Material + "' and SUBSTRING(convert(varchar,Scan_bar),1,5) ='" + Scanbar + "'  and CONVERT(datetime, [Scan_Time], 102) <  CONVERT(DATETIME, '"+scantime+"', 102) order by Scan_Time desc";

                    DataTable dtLrbarcodelog = Cnn.ExecuteQuery(connectionstring, getData);
                    if (dtLrbarcodelog.Rows.Count == 0)
                    {
                        string script = "alert(\"Không có dữ liệu quét mở bồn hóa chất!! (Mes bạn chọn là mes trong giai đoạn thử nghiệm hoặc bồn hóa chất đã đủ nguyên liệu)\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                    else
                    {
                        TempData.tableLrBarcodelog = dtLrbarcodelog;
                        gvBarcodeLog.DataSource = dtLrbarcodelog;
                        gvBarcodeLog.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvLrbarcodelog();", true);
                    }
                }
            }
        }

        protected void btnExcelLrLot_Click(object sender, EventArgs e)

        {

            try
            {
                exportExcel(TempData.tableLrLot, "lrlot");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void gvOutHC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string PlanId = Dulieu[0].ToString().Trim();
            string SerialNum = Dulieu[1].Trim();
            if (e.CommandName == "btnLrWeight")
            {
                string[] may = CheckMay();
                if (may[0] != null)
                {
                    ThongBao(may[0]);
                }
                else
                {
                    string connectionstring = "Data Source=" + may[1] + ";Initial Catalog=CWSS_S7;User ID=kendakv2;Password=kenda123";
                    string getData = " SELECT [Plan_id],[Equip_code],[Serial_Num],[Weight_ID],[Material_Code],[Real_Weight],[Real_Error],[Over_Weight],[Waste_Time],[Weight_Time]  FROM [CWSS_S7].[dbo].[LR_weigh] WHERE Plan_id='" + PlanId + "' AND Serial_Num='" + SerialNum + "' ORDER BY Weight_Time DESC";
                    DataTable dtLrweight = Cnn.ExecuteQuery(connectionstring, getData);
                    if (dtLrweight.Rows.Count == 0)
                    {
                        string script = "alert(\"Không có dữ liệu!! (Mes bạn chọn là mes trong giai đoạn thử nghiệm)\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                    else
                    {
                        TempData.tableLrWeight = dtLrweight;
                        gvInBao.DataSource = dtLrweight;
                        gvInBao.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvInBaoHC();", true);
                    }
                }

            }

        }

        protected void btnExcellrweight_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableLrWeight, "lrweight");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void btnExcelBarcodelog_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableLrBarcodelog, "lrbarcodelog");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }

        }

        protected void gvBarcodeLog_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string scanbar = e.CommandArgument.ToString();
            if (e.CommandName == "btnMes2row")
            {
                string[] may = CheckMay();
                if (may[0] != null)
                {
                    ThongBao(may[0]);
                }
                else
                {
                    string connectionstring = "Data Source=" + may[1] + ";Initial Catalog=CWSS_S7;User ID=kendakv2;Password=kenda123";
                    string getData = " SELECT [EquipCode],[Barcode],[MaterialCode],[PackageCount],[PerPackageWeight],[LotNumber],[ProductDate],[ValidDate],[ProductName],[RecordTime] FROM [CWSS_S7].[dbo].[Mes2RawMaterial] WHERE Barcode='" + scanbar + "'"+
                        "  union  SELECT [EquipCode],[Barcode],[MaterialCode],[PackageCount],[PerPackageWeight],[LotNumber],[ProductDate],[ValidDate],[ProductName],[RecordTime] FROM [CWSS_S7].[dbo].[Mes2RawMaterial_test] WHERE Barcode like '"+scanbar+"%'";
                    DataTable dtMes2RawMaterial = Cnn.ExecuteQuery(connectionstring, getData);
                    if (dtMes2RawMaterial.Rows.Count == 0)
                    {
                        string script = "alert(\"Không có dữ liệu!! (Mes bạn chọn là mes trong giai đoạn thử nghiệm)\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                    else
                    {
                        TempData.tableMes2RawMaterial = dtMes2RawMaterial;
                        gvScanbar.DataSource = dtMes2RawMaterial;
                        gvScanbar.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvScanbar();", true);
                    }
                }
            }
        }
        private void exportExcel(DataTable dt, string type)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                wb.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                string fileName = "";
                if (type == "lrplan")
                {
                    fileName = "San luong lrPlan" + txtTuNgay.Text + " - " + txtDenNgay.Text + ".xlsx";
                }
                if (type == "lrrecipe")
                {
                    fileName = "Nguyen lieu can quet LrRecipe.xlsx";
                }
                if (type == "lrlot")
                {
                    fileName = "Bao hoa chat lam duoc LrLot.xlsx";
                }
                if (type == "lrweight")
                {
                    fileName = "Chi tiet can nguyen lieu cua tung bao hoa chat LrWeight.xlsx";
                }
                if (type == "lrbarcodelog")
                {
                    fileName = "Chi tiet quet mo bon hoa chat trong ngay LrBarcodeLog.xlsx";
                }
                if (type == "mes2rawmaterial")
                {
                    fileName = "Chi tiet nguyen lieu tem vang quet vao Mes2RawMaterial.xlsx";
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);

                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void btnExcelMes2RawMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableMes2RawMaterial, "mes2rawmaterial");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }
    }
}