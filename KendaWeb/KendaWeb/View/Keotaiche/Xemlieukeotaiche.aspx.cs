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

namespace KendaWeb.View.Keotaiche
{
    public partial class Xemlieukeotaiche : System.Web.UI.Page
    {
        string ConnectionStringHome = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=erp;User ID=kendakv2;Password=kenda123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (txtFromDay.Text == "" || txtToday.Text == "")
            {
                string strdate = DateTime.Now.ToString("yyyy-MM-dd");
                txtFromDay.Text = strdate;
                txtToday.Text = strdate;
            }
        }

        protected void txtToday_TextChanged(object sender, EventArgs e)
        {
            int datetu = int.Parse(txtFromDay.Text.Replace("-", "").Trim());
            int dateden = int.Parse(txtToday.Text.Replace("-", "").Trim());

            if (dateden >= datetu)
            {
                return;
            }
            else
            {
                ThongBao("Dữ liệu không hợp lệ!!!");
                txtToday.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }

        private void ThongBaobarcode(string ThongBao)
        {
            lblThongbaobarcoderep.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessages();", true);
        }

        protected void btn_TruyLieu_Click(object sender, EventArgs e)
        {
            txtTimkiem.Text = "";
            LoadData();
        }
        public void LoadData()
        {
            try
            {
                string TuNgay = txtFromDay.Text.Replace("-", "").Trim();
                string DenNgay = txtToday.Text.Replace("-", "").Trim();
                string getdata = "SELECT [pday],[class] ,[machno],[mesid] ,[barcode],[partno],[qty],[bacode],[itnbr],[intime],[indat],[usrno]  FROM[erp].[dbo].[prdebc] where mesid = 'RL' and pday between '" + TuNgay + "' and '" + DenNgay + "' and substring(bacode,1,1) <> 'B' order by indat asc";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, getdata);
                if(dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                else
                {
                    TempData.tablePrdebc = dt;
                    gvKQ.DataSource = dt;
                    gvKQ.DataBind();
                }
            }
            catch(Exception ex)
            {
                ThongBao(ex.ToString());
            }
        }

        protected void gvKQ_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString().Trim();
            string ptype = id.Substring(0, 1);
            if (e.CommandName == "btnXem")
            {
                if (ptype == "R")
                {
                    string getdataebe = "SELECT [mesid],[machno],[daylimt],[barcode] ,[slipno],[weight] ,[prodat],[effdat],[class],[ptype],[partno],[intime],[indat],[usrno],[pallet_no] FROM [erp].[dbo].[prdebe] where barcode='" + id + "' order by indat desc";
                    System.Data.DataTable dtebe = Cnn.ExecuteQuery(ConnectionStringHome, getdataebe);
                    if (dtebe.Rows.Count == 0)
                    {
                        ThongBao("Không có dữ liệu");
                        gvPrdebe.DataSource = null;
                        gvPrdebe.DataBind();
                        return;
                    }
                    else
                    {
                        TempData.tablePrdebe = dtebe;
                        gvPrdebe.DataSource = dtebe;
                        gvPrdebe.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvEbe();", true);

                    }
                }
                else
                {
                    //string getdataebc = "Select bacode FROM [erp].[dbo].[prdebc] where barcode='" + id + "' and substring(bacode,1,2) ='RD' ";
                    //string getdataebc = "Select bacode FROM [erp].[dbo].[prdebc] where barcode='" + id + "'  ";
                    string getdataebc = "Select bacode FROM [erp].[dbo].[prdebc] where barcode='" + id + "' and substring(bacode,1,2) <> 'RR' ";
                    System.Data.DataTable dtebc = Cnn.ExecuteQuery(ConnectionStringHome, getdataebc);
                    string bacode = "";
                    if (dtebc.Rows.Count == 0)
                    {
                        ThongBao("Không có dữ liệu tem "+id+" ở ebc");
                        return;
                    }
                    else
                    {
                        foreach (DataRow item in dtebc.Rows)
                        {
                            bacode += "'" + item["bacode"].ToString().Trim() + "',";
                        }
                        string barcode = bacode.Remove(bacode.Length - 1);
                        string getdataebe = "SELECT [mesid],[machno],[daylimt],[barcode] ,[slipno],[weight] ,[prodat],[effdat],[class],[ptype],[partno],[intime],[indat],[usrno],[pallet_no] FROM [erp].[dbo].[prdebe] where barcode in ("+ barcode + ") order by indat desc";
                        System.Data.DataTable dtebe = Cnn.ExecuteQuery(ConnectionStringHome, getdataebe);
                        if(dtebe.Rows.Count == 0)
                        {
                            ThongBao("Không có dữ liệu");
                            gvPrdebe.DataSource = null;
                            gvPrdebe.DataBind();
                            return;
                        }
                        else
                        {
                            TempData.tablePrdebe = dtebe;
                            gvPrdebe.DataSource = dtebe;
                            gvPrdebe.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvEbe();", true);
                        }
                    }
                   
                }
                  
            }
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (txtTimkiem.Text == "")
            {
                ThongBao("Vui lòng nhập tiêu chuẩn cần tìm kiếm ");
                return;
            }
            else
            {
                try
                {
                    string mavach = txtTimkiem.Text.Trim();
                    //string TuNgay = txtFromDay.Text.Replace("-", "").Trim();
                    //string DenNgay = txtToDay.Text.Replace("-", "").Trim();
                    //string getdata = "SELECT [pday],[class] ,[machno],[mesid] ,[barcode],[partno],[qty],[bacode],[itnbr],[intime],[indat],[usrno]  FROM[erp].[dbo].[prdebc] where mesid = 'RL' and barcode='"+mavach+"' and pday between '" + TuNgay + "' and '" + DenNgay + "' and substring(bacode,1,1) <> 'B' order by indat asc";
                    string getdata = "SELECT [pday],[class] ,[machno],[mesid] ,[barcode],[partno],[qty],[bacode],[itnbr],[intime],[indat],[usrno]  FROM[erp].[dbo].[prdebc] where mesid = 'RL' and barcode='" + mavach + "' and substring(bacode,1,1) <> 'B' order by indat asc";
                    System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, getdata);
                    if (dt.Rows.Count == 0)
                    {
                        ThongBao("Không có dữ liệu");
                        gvKQ.DataSource = null;
                        gvKQ.DataBind();
                        return;
                    }
                    else
                    {
                        TempData.tablePrdebc = dt;
                        gvKQ.DataSource = dt;
                        gvKQ.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    ThongBao(ex.ToString());
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
                if (type == "ebc")
                {
                    fileName = "Tem keo RL prdebc " + txtToday.Text + " - " + txtFromDay.Text + ".xlsx";
                }
                if (type == "ebe")
                {
                    fileName = "Tem keo prdebe  " + txtToday.Text + " - " + txtFromDay.Text +" .xlsx";
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

                DataTable dt = TempData.tablePrdebc;
                exportExcel(dt, "ebc");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void btnExcelPrdebe_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dtebe = TempData.tablePrdebe;
                exportExcel(dtebe, "ebe");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void gvPrdebe_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string Mesid = Dulieu[0].ToString().Trim();
            string Partno = Dulieu[1].ToString().Trim();
            string Makeo = Partno.Substring(Partno.Length - 2,2);
            try
            {
                if (Makeo == "RE")
                {
                    string may = Mesid.Substring(2, 1);
                   
                    string ConnectionString = ChonMay(may);
                    if (ConnectionString == "")
                    {
                        ThongBaobarcode("Lỗi kết nối sever vui lòng thử lại");
                        return;
                    }
                    else
                    {
                        string getidgrouplot = "SELECT [recipe_name],[idGrouplot] FROM [InTem].[dbo].[KEORE] where mesid='"+Mesid+"'";
                        string ConnectionStringKeoRE = "Data Source=198.1.9.186;Initial Catalog=InTem;User ID=kendakv2;Password=kenda123";
                        System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringKeoRE, getidgrouplot);
                        if(dt.Rows.Count > 0)
                        {
                            string tenkeo = dt.Rows[0]["recipe_name"].ToString().Trim();
                            string idgrouplot=dt.Rows[0]["idGrouplot"].ToString().Trim();
                            if (Partno == tenkeo)
                            {
                                string getplanid = "SELECT  [Id],[Plan_ID] from [Ppt_GroupLot] where ID='" + idgrouplot + "'";
                                System.Data.DataTable dt1 = Cnn.ExecuteQuery(ConnectionString,getplanid);
                                if (dt1.Rows.Count > 0)
                                {
                                    string planid = dt1.Rows[0]["Plan_ID"].ToString().Trim();
                                    string getbarcoderep = "SELECT  [Equip_ID],[Plan_ID],[Recipe_Code],[Mater_Barcode] FROM [Ppt_BarCodeRep] where Plan_ID='" + planid + "' and len(Mater_Barcode)<11 and Mater_Barcode like 'RL%' group by  [Equip_ID],[Plan_ID],[Recipe_Code],[Mater_Barcode] ";
                                    System.Data.DataTable dt2 = Cnn.ExecuteQuery( ConnectionString, getbarcoderep);
                                    if( dt2.Rows.Count > 0)
                                    {
                                        gvBarcoderep.DataSource = dt2;
                                        gvBarcoderep.DataBind();
                                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvBarcoderep();", true);
                                    }
                                    else
                                    {
                                        ThongBaobarcode("Đầu máy không chạy tự động, không có dữ liệu quét tem");
                                        return;
                                    }

                                }
                                else
                                {
                                    ThongBaobarcode("Không có dữ liệu chạy trên dầu máy cán keo (Ppt_GroupLot máy hóa chất)");
                                    return;
                                }
                            }
                            else
                            {
                                ThongBaobarcode("dữ liệu in tem không trùng với mã keo trên máy (recipe_name KeoRE)");
                                return;
                            }
                        }
                        else
                        {
                            ThongBaobarcode("Không có dữ liệu in tem (idgrouplot KeoRE)");
                            return;
                        }

                    }
                }
                else
                {
                    ThongBaobarcode("Keo này không phải keo tái chế không thể tiếp tục xem");
                    return;
                }

            }
            catch (Exception ex)
            {

                ThongBaobarcode("Không có dữ liệu!!");
            }
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

        private string ChonMay(string May)
        {
            string a = string.Empty;
            switch (May)
            {
                case ("1"):
                    a = "BB_May1_8.21";
                    break;
                case ("2"):
                    a = "BB_May2_8.22";
                    break;
                case ("3"):
                    a = "BB_May3_8.23";
                    break;
                case ("4"):
                    a = "BB_May4_8.24";
                    break;
                case ("5"):
                    a = "BB_May5_8.35";
                    break;
                case ("6"):
                    a = "BB_May6_8.36";
                    break;
                case ("7"):
                    a = "BB_May7_8.37";
                    break;

            }
            switch (PingIp("198.1.9.186"))
            {
                case (1):
                    string ConnectionString = "Data Source = 198.1.9.186; Initial Catalog ="+a+"; User ID = kendakv2; Password = kenda123";
                    return ConnectionString;
                case (0):
                    return "";
                default:
                    return "";
            }
        }
    }
}