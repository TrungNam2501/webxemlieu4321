using ClosedXML.Excel;
using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KendaWeb.View.BB2
{
    public partial class Hoachatcantay : System.Web.UI.Page
    {

        string ConnectionStringHome = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=BB;User ID=kendakv2;Password=kenda123;";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (txtTuNgay.Text == "" || txtDenNgay.Text == "")
            {
                string strdate = DateTime.Now.ToString("yyyy-MM-dd");
                txtTuNgay.Text = strdate;
                txtDenNgay.Text = strdate;
            }

        }

        protected void gvHCcantay_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string PlanId = e.CommandArgument.ToString();
            if (e.CommandName == "btnIn")
            {
                string getData = "SELECT [Plan_id],[Equip_code] ,[Weight_ID],[Material_Code],[Material_Name],[Set_Weight],[Set_Error] FROM [BB].[dbo].[LR_recipe] WHERE Plan_id='" + PlanId + "'";
                DataTable dtLrrecipe = Cnn.ExecuteQuery(ConnectionStringHome, getData);
                if (dtLrrecipe.Rows.Count == 0)
                {
                    string script = "alert(\"Không có dữ liệu!!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
                else
                {
                    TempData.tableLrRecipeCantay = dtLrrecipe;
                    gvInHC.DataSource = dtLrrecipe;
                    gvInHC.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvInHC();", true);
                }
            }
            if (e.CommandName == "btnOut")
            {
                string getData = "SELECT * FROM [BB].[dbo].[LR_weigh] WHERE Plan_id='" + PlanId + "'";
                DataTable dtLrweigh = Cnn.ExecuteQuery(ConnectionStringHome, getData);
                if (dtLrweigh.Rows.Count == 0)
                {
                    string script = "alert(\"Không có dữ liệu!!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
                else
                {
                    TempData.tableLrWeighCantay = dtLrweigh;
                    gvOutHC.DataSource = dtLrweigh;
                    gvOutHC.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvOutHC();", true);
                }
            }
        }
        protected void btnXemLieu_Click(object sender, EventArgs e)
        {
            txtTimkiem.Text = "";
            string TuNgay = txtTuNgay.Text;
            string DenNgay = txtDenNgay.Text;
            string sqlcantay = "SELECT [Plan_Id], [Equip_Code], " +
                "CASE WHEN [Equip_Code] = '01' THEN N'-1 cũ' " +
                "WHEN [Equip_Code] = '02' THEN N'-1 mới' " +
                "WHEN [Equip_Code] = '03' THEN N'-9 cũ' " +
                "WHEN [Equip_Code] = '04' THEN N'-9 mới' " +
                "ELSE [Equip_Code] END AS TenMay," +
                " [Recipe_Name], [Shift_Id], [Plan_Num], [Real_Num], [Start_Date], [End_Date], [Plan_Date] FROM " +
                "[BB].[dbo].[LR_plan] WHERE IF_FLAG = '4' AND CAST(Start_Date AS date) BETWEEN '"+TuNgay+"' AND '"+DenNgay+"' ORDER BY Plan_Id, Start_Date;";
            DataTable dataTable = Cnn.ExecuteQuery(ConnectionStringHome, sqlcantay);    
            if (dataTable.Rows.Count == 0)
            {
                gvHCcantay.DataSource = null;
                gvHCcantay.DataBind();
                string script = "alert(\"Không có dữ liệu!\");";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            }  
            else
            {
                TempData.tableLrPlanCantay = dataTable;
                gvHCcantay.DataSource = dataTable;
                gvHCcantay.DataBind();
            }
        }

        protected void gvInHC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string PlanId = e.CommandArgument.ToString();
            if(e.CommandName== "btnIfscan2mes")
            {
                string sqltemquet = "SELECT  [Plan_Id],[EquipCode],[MaterialCode] ,[ScanStr],[Qty],[Qtyw],[Indat],[Empno] FROM [BB].[dbo].[IF_ScanMes] where Plan_Id ='"+PlanId+"'";
                DataTable dtTemquet = Cnn.ExecuteQuery(ConnectionStringHome, sqltemquet);
                if(dtTemquet.Rows.Count== 0)
                {
                    string script = "alert(\"Không có dữ liệu quét hóa chất!! \");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
                else
                {
                    TempData.tableIfscan2Mes = dtTemquet;
                    gvBarcodeLog.DataSource = dtTemquet;
                    gvBarcodeLog.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvLrbarcodelog();", true);
                }
            }
        }

        protected void gvBarcodeLog_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string scanbar = e.CommandArgument.ToString();
            if (e.CommandName == "btnMes2row")
            {
                string getData = " SELECT [EquipCode],[Barcode],[MaterialCode],[PackageCount],[PerPackageWeight],[LotNumber],[ProductDate],[ValidDate],[ProductName],[RecordTime] FROM [BB].[dbo].[Mes2RawMaterial] WHERE Barcode='" + scanbar + "'";
                    
                DataTable dtMes2RawMaterial = Cnn.ExecuteQuery(ConnectionStringHome, getData);
                if (dtMes2RawMaterial.Rows.Count == 0)
                {
                    string script = "alert(\"Không có dữ liệu!!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
                else
                {
                    TempData.tableMes2RawMaterialCantay = dtMes2RawMaterial;
                    gvScanbar.DataSource = dtMes2RawMaterial;
                    gvScanbar.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvScanbar();", true);
                }
            }

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            string timkiem =txtTimkiem.Text;
            string TuNgay = txtTuNgay.Text;
            string DenNgay = txtDenNgay.Text;
            string sqlcantay = "SELECT [Plan_Id], [Equip_Code], " +
                "CASE WHEN [Equip_Code] = '01' THEN N'-1 cũ' " +
                "WHEN [Equip_Code] = '02' THEN N'-1 mới' " +
                "WHEN [Equip_Code] = '03' THEN N'-9 cũ' " +
                "WHEN [Equip_Code] = '04' THEN N'-9 mới' " +
                "ELSE [Equip_Code] END AS TenMay," +
                " [Recipe_Name], [Shift_Id], [Plan_Num], [Real_Num], [Start_Date], [End_Date], [Plan_Date] FROM " +
                "[BB].[dbo].[LR_plan] WHERE  Recipe_Name like '%"+timkiem+"%' AND IF_FLAG = '4' AND CAST(Start_Date AS date) BETWEEN '" + TuNgay + "' AND '" + DenNgay + "' ORDER BY Plan_Id, Start_Date;";
            DataTable dataTable = Cnn.ExecuteQuery(ConnectionStringHome, sqlcantay);
            if (dataTable.Rows.Count == 0)
            {
                gvHCcantay.DataSource = null;
                gvHCcantay.DataBind();
                string script = "alert(\"Không có dữ liệu!\");";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            }
            else
            {
                TempData.tableLrPlanCantay = dataTable;
                gvHCcantay.DataSource = dataTable;
                gvHCcantay.DataBind();
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
                    fileName = "LrRecipeCantay.xlsx";
                }
                if (type == "lrlot")
                {
                    fileName = "LrWeighCantay.xlsx";
                }
             
                
                if (type == "lrbarcodelog")
                {
                    fileName = "Haodungifscan2mes.xlsx";
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

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = TempData.tableLrPlanCantay;
                exportExcel(dt, "lrplan");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }

        protected void btnExcelLrRecipe_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableLrRecipeCantay, "lrrecipe");
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
                exportExcel(TempData.tableIfscan2Mes, "lrbarcodelog");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void btnExcelMes2RawMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableMes2RawMaterialCantay, "mes2rawmaterial");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void btnExcelLrLot_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableLrWeighCantay, "lrlot");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }
    }
}