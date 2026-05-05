using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using KendaWeb.Model;

namespace KendaWeb.View.XemlieuPLC
{
    public partial class XemLieuPLC : System.Web.UI.Page
    {
        string ConnectionString186 = "Data Source=198.1.9.186;Initial Catalog=BB_Machine_Floor3;User ID=kendakv2;Password=kenda123";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }
        public static int PingIp(string ip)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pingresult = ping.Send(ip, 10);
                if (pingresult.Status.ToString() == "Success")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        protected void btnTruyLieu_Click(object sender, EventArgs e)
        {


            if (txtTugio.Text.Trim() == "" && txtDengio.Text.Trim() == "")
            {
                txtTimkiem.Text = "";
                LoadDatakhonggio();
            }
            else
            {
                txtTimkiem.Text = "";
                LoadDatacogio();
            }
        }
        private void LoadDatakhonggio()
        {
            try
            {
                string maytam = Drmay.SelectedValue;
                if (maytam == "")
                {
                    ThongBao("Vui lòng chọn máy!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string may = "Machine_BB" + maytam;
                string dongtam = drDong.SelectedValue;
                string dong = "";
                if (dongtam == "1000")
                {
                    dong = "Top 1000";
                }
                if (dongtam == "2000")
                {
                    dong = "Top 2000";
                }
                if (dongtam == "3000")
                {
                    dong = "Top 3000";
                }
                if (dongtam == "5000")
                {
                    dong = "Top 5000";
                }
                if (dongtam == "7000")
                {
                    dong = "Top 7000";
                }
                if (dongtam == "10000")
                {
                    dong = "Top 10000";
                }
              
                string day = txtDate.Text.ToString().Replace("-", "").Trim();
                if (day == "")
                {
                    ThongBao("Vui lòng chọn ngày!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string getdataplc = " SELECT " + dong + " [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], " +
                    "[Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], " +
                    "[Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num] " +
                    " FROM[BB_Machine_Floor3].[dbo].[" + may + "] where indat='" + day + "'  order by intime desc ";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString186, getdataplc);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                else
                {
                    TempData.tablePlc = dt;
                    gvKQPlc.DataSource = dt;
                    gvKQPlc.DataBind();
                }
            }
            catch (Exception ex)
            {
                ThongBao(ex.ToString());
            }


        }
        private void LoadDatacogio()
        {
            try
            {
                string tugio = txtTugio.Text.Trim();
                string dengio = txtDengio.Text.Trim();
                if (tugio == "" || dengio == "")
                {
                    ThongBao("Vui lòng nhập đủ giờ ở 2 ô");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string maytam = Drmay.SelectedValue;
                if (maytam == "")
                {
                    ThongBao("Vui lòng chọn máy!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string may = "Machine_BB" + maytam;
                string dongtam = drDong.SelectedValue;
                string dong = "";
                if (dongtam == "1000")
                {
                    dong = "Top 1000";
                }
                if (dongtam == "2000")
                {
                    dong = "Top 2000";
                }
                if (dongtam == "3000")
                {
                    dong = "Top 3000";
                }
                if (dongtam == "5000")
                {
                    dong = "Top 5000";
                }
                if (dongtam == "7000")
                {
                    dong = "Top 7000";
                }
                if (dongtam == "10000")
                {
                    dong = "Top 10000";
                }
                string day = txtDate.Text.ToString().Replace("-", "").Trim();
                if (day == "")
                {
                    ThongBao("Vui lòng chọn ngày!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string getdataplc = " SELECT " + dong + " [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], " +
                    "[Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], " +
                    "[Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num] " +
                    " FROM[BB_Machine_Floor3].[dbo].[" + may + "] where indat='" + day + "' and (intime BETWEEN '" + tugio + "' and '" + dengio + "' )  order by intime desc ";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString186, getdataplc);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                else
                {
                    TempData.tablePlc = dt;
                    gvKQPlc.DataSource = dt;
                    gvKQPlc.DataBind();
                }
            }
            catch (Exception ex)
            {
                ThongBao(ex.ToString());
            }

        }

        protected void txtDengio_TextChanged(object sender, EventArgs e)
        {
            int timetu = int.Parse(txtTugio.Text.Replace(":", "").Trim());
            int timeden = int.Parse(txtDengio.Text.Replace(":", "").Trim());

            if (timeden >= timetu)
            {
                return;
            }
            else
            {
                ThongBao("Dữ liệu giờ không hợp lệ!!! Chọn lại");
                txtTugio.Text = "";
                txtDengio.Text = "";

            }
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (txtTugio.Text.Trim() == "" && txtDengio.Text.Trim() == "")
            {

                LoadDatakhonggiotimkien();
            }
            else
            {

                LoadDatacogiotimkien();
            }
        }
        private void LoadDatakhonggiotimkien()
        {
            try
            {
                string partno = txtTimkiem.Text.Trim();
                if (partno == "")
                {
                    ThongBao("Vui lòng nhập mã keo tìm kiếm!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string maytam = Drmay.SelectedValue;
                if (maytam == "")
                {
                    ThongBao("Vui lòng chọn máy!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string may = "Machine_BB" + maytam;
                string dongtam = drDong.SelectedValue;
                string dong = "";
                if (dongtam == "1000")
                {
                    dong = "Top 1000";
                }
                if (dongtam == "2000")
                {
                    dong = "Top 2000";
                }
                if (dongtam == "3000")
                {
                    dong = "Top 3000";
                }
                if (dongtam == "5000")
                {
                    dong = "Top 5000";
                }
                if (dongtam == "7000")
                {
                    dong = "Top 7000";
                }
                if (dongtam == "10000")
                {
                    dong = "Top 10000";
                }
                string day = txtDate.Text.ToString().Replace("-", "").Trim();
                if (day == "")
                {
                    ThongBao("Vui lòng chọn ngày!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string getdataplc = " SELECT " + dong + " [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], " +
                    "[Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], " +
                    "[Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num] " +
                    " FROM[BB_Machine_Floor3].[dbo].[" + may + "] where indat='" + day + "' and Recipe_Name='" + partno + "'  order by intime desc ";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString186, getdataplc);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                else
                {
                    TempData.tablePlc = dt;
                    gvKQPlc.DataSource = dt;
                    gvKQPlc.DataBind();
                }
            }
            catch (Exception ex)
            {
                ThongBao(ex.ToString());
            }

        }
        private void LoadDatacogiotimkien()
        {
            try
            {
                string partno = txtTimkiem.Text.Trim();
                if (partno == "")
                {
                    ThongBao("Vui lòng nhập mã keo tìm kiếm!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string tugio = txtTugio.Text.Trim();
                string dengio = txtDengio.Text.Trim();
                if (tugio == "" || dengio == "")
                {
                    ThongBao("Vui lòng nhập đủ giờ ở 2 ô");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string maytam = Drmay.SelectedValue;
                if (maytam == "")
                {
                    ThongBao("Vui lòng chọn máy!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string may = "Machine_BB" + maytam;
                string dongtam = drDong.SelectedValue;
                string dong = "";
                if (dongtam == "1000")
                {
                    dong = "Top 1000";
                }
                if (dongtam == "2000")
                {
                    dong = "Top 2000";
                }
                if (dongtam == "3000")
                {
                    dong = "Top 3000";
                }
                if (dongtam == "5000")
                {
                    dong = "Top 5000";
                }
                if (dongtam == "7000")
                {
                    dong = "Top 7000";
                }
                if (dongtam == "10000")
                {
                    dong = "Top 10000";
                }
                string day = txtDate.Text.ToString().Replace("-", "").Trim();
                if (day == "")
                {
                    ThongBao("Vui lòng chọn ngày!!!");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                string getdataplc = " SELECT " + dong + " [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], " +
                    "[Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], " +
                    "[Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num] " +
                    " FROM[BB_Machine_Floor3].[dbo].[" + may + "] where indat='" + day + "' and Recipe_Name='" + partno + "' and (intime BETWEEN '" + tugio + "' and '" + dengio + "' )  order by intime desc ";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString186, getdataplc);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                else
                {
                    TempData.tablePlc = dt;
                    gvKQPlc.DataSource = dt;
                    gvKQPlc.DataBind();
                }
            }
            catch (Exception ex)
            {
                ThongBao(ex.ToString());
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
                if (type == "plc")
                {
                    fileName = "Lieuplc" + txtDate.Text + " - máy " + Drmay.SelectedValue + ".xlsx";
                }
                if (type == "plccogio")
                {
                    fileName = "Lieuplc -" + txtDate.Text + "-" + txtTugio.Text + " - " + txtDengio.Text + " - máy " + Drmay.SelectedValue + ".xlsx";
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

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

            try
            {

                //if (txtTugio.Text.Trim() == "" && txtDengio.Text.Trim() == "")
                //{
                //    DataTable dt = TempData.tablePlc;
                //    exportExcel(dt, "plc");
                //}
                //else
                //{
                //    DataTable dt = TempData.tablePlc;
                //    exportExcel(dt, "plccogio");
                //}


                string dong = drDong.SelectedValue;
                if (dong == "1000" || dong == "2000" || dong == "5000" || dong=="10000" ||dong=="3000"||dong=="7000")
                {
                    if (txtTugio.Text.Trim() == "" && txtDengio.Text.Trim() == "")
                    {
                        DataTable dt = TempData.tablePlc;
                        exportExcel(dt, "plc");
                    }
                    else
                    {
                        DataTable dt = TempData.tablePlc;
                        exportExcel(dt, "plccogio");
                    }
                }
                else
                {
                    ThongBao("Vượt quá 10000 dòng , không thể xuất Excel");
                }



            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }
    }
}