using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace KendaWeb.View.XemlieuPLC
{
    public partial class LieuPLC : System.Web.UI.Page
    {
        string ConnectionString186 = "Data Source=198.1.9.186;Initial Catalog=BB_Machine_Floor3;User ID=kendakv2;Password=kenda123";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                xuatdulieup();
            }
           
        }
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }
        private void xuatdulieup()
        {
            string date = Request.QueryString["date"];
            string starttime = Request.QueryString["starttime"];
            string endtimetam = Request.QueryString["endtime"];
            DateTime sttime = DateTime.ParseExact(starttime, "HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime entime = DateTime.ParseExact(endtimetam, "HH:mm:ss", CultureInfo.InvariantCulture);
            if (sttime > entime)
            {
                loaddata2ngay();
            }
            else
            {
                loaddatatrongngay(); 
            }

        }
        private void loaddata2ngay()
        {
            string maytam = Request.QueryString["may"];
            string recipenameplc = Request.QueryString["recipenameplc"];
            bientam.recipetam = recipenameplc;
            string starttime = Request.QueryString["starttime"];
            bientam.starttimetam = starttime;
            string date = Request.QueryString["date"];
            bientam.datetam = date;
            string namcong1 = date.Substring(0, 6);
            int daycong1 = int.Parse(date.Substring(6, 2));
            string dayngayhomsau = "";
            int dayngaymoi = daycong1 + 1;
            if (dayngaymoi.ToString().Length < 2)
            {
                dayngayhomsau = "0" + dayngaymoi.ToString();
            }
            else
            {
                dayngayhomsau = dayngaymoi.ToString();
            }
            string ngayhomsau = namcong1 + dayngayhomsau;
            bientam.ngayhomsau = ngayhomsau;
            //conver +5 phut
            string endtimetam = Request.QueryString["endtime"];
            int gio = int.Parse(endtimetam.Substring(0, 2));
            int phut = int.Parse(endtimetam.Substring(3, 2));
            int giay = int.Parse(endtimetam.Substring(6, 2));
            int nam = int.Parse(date.Substring(0, 4));
            int thang = int.Parse(date.Substring(4, 2));
            int ngay = int.Parse(date.Substring(6, 2));
            DateTime dateValue = new DateTime(nam, thang, ngay, gio, phut, giay);
            DateTime datetest;
            datetest = dateValue.AddMinutes(5);
            string endtime = datetest.ToString("HH:mm:ss");
            //convert + 5 phuts
            bientam.endtimetam = endtime;

            txtMay.Text = "Máy: BB-" + maytam + "  ";
            txtMaKeo.Text = "Mã keo: " + recipenameplc + "  ";
            txtNgaySX.Text = "Ngày SX: " + date + "  ";
            txtStatime.Text = starttime;
            txtEndtime.Text = endtimetam;
            string may = "Machine_BB" + maytam;
            bientam.maytam = may;
            string getdataplc = "  SELECT Top 1000 [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], [Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], [Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num]  FROM[BB_Machine_Floor3].[dbo].["+may+"] where indat='"+date+"' and (intime BETWEEN '"+ starttime + "' and '23:59:59' ) and [Recipe_Name]='"+ recipenameplc + "' " +
                                " union" +
                                " SELECT Top 1000[ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], [Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], [Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num] FROM[BB_Machine_Floor3].[dbo].["+may+"] where indat = '"+ngayhomsau+"' and(intime BETWEEN '00:00:00' and '"+endtime+"') and[Recipe_Name] = '"+ recipenameplc + "'  order by ID desc ";

            System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString186, getdataplc);
            
            if (dt.Rows.Count == 0)
            {
                dt= Cnn.ExecuteQuery(ConnectionString186, getdataplc);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                else
                {
                    return;
                }
                   
            }
            if(dt.Rows.Count != 0)
            {
                TempData.tablePlc = dt;
                gvKQPlc.DataSource = dt;
                gvKQPlc.DataBind();
            }
            //if (dt.Rows.Count == 0)
            //{
            //    ThongBao("Không có dữ liệu");
            //    gvKQPlc.DataSource = null;
            //    gvKQPlc.DataBind();
            //    return;
            //}
            //else
            //{
            //    TempData.tablePlc = dt;
            //    gvKQPlc.DataSource = dt;
            //    gvKQPlc.DataBind();
            //}

        }
        private void loaddatatrongngay()
        {
            string maytam = Request.QueryString["may"];
            string recipenameplc = Request.QueryString["recipenameplc"];
            bientam.recipetam = recipenameplc;
           string starttime = Request.QueryString["starttime"];
            bientam.starttimetam = starttime;
            string date = Request.QueryString["date"];
            
            string dateoke = date.Replace("-", "");
            bientam.datetam = dateoke;
            //conver +5 phut
            string endtimetam = Request.QueryString["endtime"];
            int gio = int.Parse(endtimetam.Substring(0, 2));
            int phut = int.Parse(endtimetam.Substring(3, 2));
            int giay = int.Parse(endtimetam.Substring(6, 2));
            int nam = int.Parse(dateoke.Substring(0, 4));
            int thang = int.Parse(dateoke.Substring(4, 2));
            int ngay = int.Parse(dateoke.Substring(6, 2));    

            DateTime dateValue = new DateTime(nam, thang, ngay, gio, phut,giay);
            DateTime datetest;
            datetest = dateValue.AddMinutes(5);
            string endtime = datetest.ToString("HH:mm:ss"); 
            //convert + 5 phuts
            bientam.endtimetam = endtime;
          
            txtMay.Text = "Máy: BB-" + maytam+"  ";
            txtMaKeo.Text = "Mã keo: " + recipenameplc + "  ";
            txtNgaySX.Text = "Ngày SX: " + date + "  ";
            txtStatime.Text = starttime;
            txtEndtime.Text = endtimetam;
            string may = "Machine_BB" + maytam;
            bientam.maytam=may;
            string getdataplc = " SELECT Top 1000 [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], " +
                    "[Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], " +
                    "[Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num] " +
                    " FROM[BB_Machine_Floor3].[dbo].[" + may + "] where indat='" + dateoke + "' and  CAST(Intime AS TIME) BETWEEN '" + starttime + "' and '" + endtime + "'  and [Recipe_Name] like '"+recipenameplc+"%'  ";
            System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString186, getdataplc);
      
           
            if (dt.Rows.Count == 0)
            {
                dt = Cnn.ExecuteQuery(ConnectionString186, getdataplc);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    gvKQPlc.DataSource = null;
                    gvKQPlc.DataBind();
                    return;
                }
                else
                {
                    return;
                }


            }
            if (dt.Rows.Count != 0)
            {
                TempData.tablePlc = dt;
                gvKQPlc.DataSource = dt;
                gvKQPlc.DataBind();
            }

          

            //if (dt.Rows.Count == 0)
            //{
            //    ThongBao("Không có dữ liệu");
            //    gvKQPlc.DataSource = null;
            //    gvKQPlc.DataBind();
            //    return;
            //}

            //else
            //{
            //    TempData.tablePlc = dt;
            //    gvKQPlc.DataSource = dt;
            //    gvKQPlc.DataBind();
            //}
        }

        protected void btnXem_Click(object sender, EventArgs e)
        {
            string date = bientam.datetam;
            string starttime = bientam.starttimetam;
            string endtimetam = bientam.endtimetam;
            DateTime sttime = DateTime.ParseExact(starttime, "HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime entime = DateTime.ParseExact(endtimetam, "HH:mm:ss", CultureInfo.InvariantCulture);
            if (sttime > entime)
            {
                xemlieutheodong2ngay();
            }
            else
            {
                xemlieutheodongtrongngay();
            }


        }

        private void xemlieutheodongtrongngay()
        {
            gvKQPlc.DataSource = null;
            gvKQPlc.DataBind();
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
            if (dongtam == "")
            {
                dong = "";
            }
            //string getdataplc = " SELECT " + dong + " [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], " +
            //       "[Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], " +
            //       "[Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num] " +
            //       " FROM[BB_Machine_Floor3].[dbo].[" + bientam.maytam + "] where indat='" + bientam.datetam + "' and (intime BETWEEN '" + bientam.starttimetam + "' and '" + bientam.endtimetam + "' ) and [Recipe_Name]='" + bientam.recipetam + "'  order by intime desc ";
            //System.Data.DataTable dtdong = Cnn.ExecuteQuery(ConnectionString186, getdataplc);


            string getdataplc = " SELECT " + dong + " [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], " +
                   "[Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], " +
                   "[Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num] " +
            " FROM[BB_Machine_Floor3].[dbo].[" + bientam.maytam + "] where indat='" + bientam.datetam + "' and  CAST(Intime AS TIME) BETWEEN '" + bientam.starttimetam + "' and '" + bientam.endtimetam + "'  and [Recipe_Name] like '" + bientam.recipetam + "%'  order by intime desc ";
            System.Data.DataTable dtdong = Cnn.ExecuteQuery(ConnectionString186, getdataplc);

            if (dtdong.Rows.Count == 0)
            {
                ThongBao("Không có dữ liệu");
                gvKQPlc.DataSource = null;
                gvKQPlc.DataBind();
                return;
            }
            else
            {
                TempData.tablePlc = dtdong;
                gvKQPlc.DataSource = dtdong;
                gvKQPlc.DataBind();
            }

        }
        private void xemlieutheodong2ngay()
        {
            gvKQPlc.DataSource = null;
            gvKQPlc.DataBind();
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
            if (dongtam == "")
            {
                dong = "";
            }
            string getdataplc = "  SELECT  " + dong + " [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], [Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], [Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num]  FROM[BB_Machine_Floor3].[dbo].[" + bientam.maytam + "] where indat='" + bientam.datetam + "' and (intime BETWEEN '" + bientam.starttimetam + "' and '23:59:59' ) and [Recipe_Name]='" + bientam.recipetam + "' " +
                               " union" +
                               " SELECT  " + dong + "  [ID],[Indat],[Intime],[Bolt_Status],[Discharge_Condition],[Downstream_Weight],[Error_Code_1],[Error_Code_2],[Error_Code_3], [Error_Code_4],[Error_Code_5],[Finish_Num],[Indoor_Status],[Mix_Action],[Mix_energy],[Mix_Power],[Mix_Step], [Mix_Time],[Mode],[Outdoor_Status],[Ram_Position],[Ram_Pressure],[Recipe_Name],[Rpm_Act],[Rpm_Set],[Rubber_Temp],[Set_Num] FROM[BB_Machine_Floor3].[dbo].[" + bientam.maytam + "] where indat = '" + bientam.ngayhomsau + "' and(intime BETWEEN '00:00:00' and '" + bientam.endtimetam + "') and[Recipe_Name] = '" + bientam.recipetam + "'  order by ID desc ";

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
                    fileName = "Lieuplc" + txtNgaySX.Text+ " " +txtMay.Text + "" + txtMaKeo.Text + ".xlsx";
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

        protected void btnXuatexcel_Click(object sender, EventArgs e)
        {
            try
            {
                string dong = drDong.SelectedValue;
                if (dong == "1000" || dong == "2000" || dong == "5000" || dong == "10000" || dong == "3000" || dong == "7000")
                {
                   
                        DataTable dtxuatexcel = TempData.tablePlc;
                        exportExcel(dtxuatexcel, "plc");
                  
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