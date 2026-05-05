using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KendaWeb.Model;
using System.Net.NetworkInformation;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.IO;

namespace KendaWeb
{
    public partial class IT : System.Web.UI.Page
    {
        string ConnectionString = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=erp;User ID=kendakv2;Password=kenda123";
        string ConnectionStringKeoRe = "Data Source=198.1.9.186;Initial Catalog=InTem;User ID=kendakv2;Password=kenda123";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/View/Loggin.aspx");
            }
            txtSearch.Attributes.Add("onkeypress", "return clickButton(event,'" + btnSearch.ClientID + "')");

            tbTrongLuong.Attributes.Add("onkeypress", "return clickButton(event,'" + btnCreate.ClientID + "')");
            tbMaPalet.Attributes.Add("onkeypress", "return clickButton(event,'" + btnCreate.ClientID + "')");
            tbKV.Attributes.Add("onkeypress", "return clickButton(event,'" + btnCreate.ClientID + "')");
            tbX.Attributes.Add("onkeypress", "return clickButton(event,'" + btnCreate.ClientID + "')");
            tbMay.Attributes.Add("onkeypress", "return clickButton(event,'" + btnCreate.ClientID + "')");
            tbSoLo.Attributes.Add("onkeypress", "return clickButton(event,'" + btnCreate.ClientID + "')");
            tbThoiGianQuet.Attributes.Add("onkeypress", "return clickButton(event,'" + btnCreate.ClientID + "')");
            tbNgayQuet.Attributes.Add("onkeypress", "return clickButton(event,'" + btnCreate.ClientID + "')");
            tbNguoiQuet.Attributes.Add("onkeypress", "return clickButton(event,'" + btnCreate.ClientID + "')");

            tbTrongLuong1.Attributes.Add("onkeypress", "return clickButton(event,'" + btnSua.ClientID + "')");
            tbMaPalet1.Attributes.Add("onkeypress", "return clickButton(event,'" + btnSua.ClientID + "')");
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

        private string ChonMay(string May)
        {
            string a = string.Empty;
            switch (May)
            {
                case ("01"):
                    a = "198.1.8.21";
                    break;
                case ("02"):
                    a = "198.1.8.22";
                    break;
                case ("03"):
                    a = "198.1.8.23";
                    break;
                case ("04"):
                    a = "198.1.8.24";
                    break;
                case ("05"):
                    a = "198.1.8.35";
                    break;
                case ("06"):
                    a = "198.1.8.36";
                    break;
                case ("07"):
                    a = "198.1.8.37";
                    break;

            }
            switch (PingIp(a))
            {
                case (1):
                    string ConnectionString = "Data Source = " + a + "; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                    return ConnectionString;
                case (0):
                    return "";
                default:
                    return "";
            }
        }

        private void LoadData()
        {
            try
            {
                string May = drMay.SelectedValue;
                string strdate = txtChonNgay.Text.ToString().Replace("-", "");
                if (May == "")
                {
                    ThongBao("Vui lòng chọn máy!!!");
                    return;
                }
                if (strdate == "")
                {
                    ThongBao("Vui lòng nhập ngày!!!");
                    return;
                }

                string GetData = "select * from [erp].[dbo].[prdebe]" +
                    " where prodat = '" + strdate + "' and mesid like'V%' and factory = 'V' and machno Like'%" + May + "'";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString, GetData);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu máy "+May+" !!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                else
                {
                    gvKQ.DataSource = dt;
                    gvKQ.DataBind();
                }
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
                return;
            }
        }

        protected void btn_TruyLieu_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            //|| printers.Substring(printers.Length - 3, 3) == "05)"
            int i = 0;
            string[] sPrinterList = new string[System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count];
            if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count == 0)
                sPrinterList[i] = "";
            else
            {
                drMayin.Items.Clear();
                foreach (string printers in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (printers.Substring(printers.Length - 3, 3) == "193" || printers.Substring(printers.Length - 3, 3) == "176" || printers.Substring(printers.Length - 3, 3) == "122" || printers.Substring(printers.Length - 3, 3) == "05)")
                    {
                        sPrinterList[i] = printers;
                        i++;
                        drMayin.Items.Add(printers);
                    }
                }
                if (i == 0)
                {
                    string r = "Không tìm thấy máy in thích hợp!!!";
                    drMayin.Items.Add(r);
                }
            }
            lbError.Visible = false;
            tbTenKeo.Text = "";
            tbNgaySanXuat.Text = "";
            ResetData();
            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
        }

        private string KiemTraGioHanKeo(string Soluong, string tenkeo, string machno, string mesid, string partno, string MaVach)
        {
            string Plan_Id = "";
            string result = "";
            string idGrouplot = "";
            string br1 = "";
            string makeo = MaVach.Substring(0, 2);
            string Machno = machno.Substring(6, 2);

            string ConnectionStringMay = ChonMay(Machno);
            #region kiem tra gioi han keo

            if (float.Parse(Soluong) < 30)
            {
                return "Lỗi! Trọng lượng không phù hợp!";
            }



            string ktKeo2 = "select * from [InTem].[dbo].[rubnod] where [rubno_7] ='" + tenkeo.Trim() + "'";
            System.Data.DataTable kiemkeo2 = Cnn.ExecuteQuery(ConnectionStringKeoRe, ktKeo2);

            if (Machno == "01")
            {

                string plannum = "SELECT Plan_Num FROM IF_RtPlan2Mixing where Plan_Id ='" + mesid + "'";
                System.Data.DataTable plann = Cnn.ExecuteQuery(ConnectionStringMay, plannum);
                if (plann.Rows.Count > 0)
                    br1 = plann.Rows[0][0].ToString().Trim();
                //haile mod, kiem tra gioi han keo va insert IF_RtPlan2Mixing
                string checkmes = " Select mesid,[weight],recipe_name,idGrouplot from [InTem].[dbo].[KEORE] " +
                    "where subno='4' and factory ='V' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and mesid ='" + mesid + "' ";
                System.Data.DataTable dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe, checkmes);

                if (dtMEsid.Rows.Count == 0)
                {
                    result = "MES không tồn tại [KEORE], tạo MES khác!";
                    return result;
                }
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                Plan_Id = dtMEsid.Rows[0][0].ToString().Trim();


                string recipe_name = dtMEsid.Rows[0][2].ToString().Trim();
                string planqty = dtMEsid.Rows[0][1].ToString().Trim();
                //--------------------------2021-07-07----------------------------------
                string DateNow = DateTime.Now.ToString("yyyy-MM-dd 06:30:00").ToString();
                string timeH = DateTime.Now.ToString("HHmm");
                if (int.Parse(timeH) <= 630)
                {
                    DateNow = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 18:30:00").ToString();
                }
                if (idGrouplot == "")
                {
                    string getIdGroupLot = "select MAX(id) from [mfns].[dbo].[Ppt_GroupLot] " +
                        "where RecipeName='" + recipe_name.Replace("-", "").ToUpper().Trim() + "' and [SetNumber]='" + br1 + "' " +
                        "and (CONVERT(datetime,Start_datetime) >= '" + DateNow + "' or CONVERT(datetime,End_datetime) >= '" + DateNow + "')";
                    System.Data.DataTable dtIdGroupLot = Cnn.ExecuteQuery(ConnectionStringMay, getIdGroupLot);

                    if (dtIdGroupLot.Rows.Count > 0)
                    {
                        bool udpKeore = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, "Update [InTem].[dbo].[KEORE] set idGrouplot='" + dtIdGroupLot.Rows[0][0].ToString().Trim() + "' where mesid='" + mesid + "' and [recipe_name]='" + recipe_name + "' and [machno]='" + Machno + "' ");
                    }
                }

                dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe, checkmes);
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                if (idGrouplot == "")
                {
                    result = "Lỗi ko có idGrouplot Liên hệ IT!";
                    return result;
                }
                //----------------------------
                string ktMESplan = " select b.FinishNum * (select sum(set_weight) from[mfns].[dbo].[pmt_weigh] where father_code = b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b " +
                    " where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "' and SetNumber = '" + planqty + "' and End_datetime is not null";
                System.Data.DataTable ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);
                if (ktplan.Rows.Count == 0)
                {
                    string getSetnumber = "select setnumber from [mfns].[dbo].[Ppt_GroupLot] where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "'  and End_datetime is not null ";
                    System.Data.DataTable setNumber = Cnn.ExecuteQuery(ConnectionStringMay, getSetnumber);
                    if (setNumber.Rows.Count > 0)
                    {
                        if (setNumber.Rows[0][0].ToString().Trim() != planqty)
                        {
                            string Update = "Update [InTem].[dbo].[KEORE] set [weight]='" + setNumber.Rows[0][0].ToString().Trim() + "' where mesid='" + Plan_Id + "' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and idGrouplot='" + idGrouplot + "' ";
                            bool UpdatePlan = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, Update);

                            ktMESplan = "select b.FinishNum * (select sum(set_weight) from [mfns].[dbo].[pmt_weigh] where father_code=b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b where id='" + idGrouplot + "'";
                            ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);

                            if (ktplan.Rows.Count == 0)
                            {
                                result = "Liên hệ phòng khống chế tạo lại MES khác!";
                                return result;
                            }
                        }
                    }
                    else
                    {
                        result = "Liên hệ phòng khống chế tạo lại MES khác!";
                        return result;
                    }
                }

                if (makeo == "RD" || makeo == "RB" || kiemkeo2.Rows.Count > 0)
                {
                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString, LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(ktplan.Rows[0][0].ToString().Trim());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MES này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
                else
                {
                    string layGioiHanKeo = "SELECT isnull(sum(set_weight)*b.weight,0) FROM [mfns].[dbo].[pmt_weigh] a,[198.1.9.186].[InTem].[dbo].[KEORE] b where b.subno='4' and factory='V' and REPLACE(b.recipe_name,'-','') = a.father_code COLLATE database_default and b.mesid='" + Plan_Id + "' group by b.[weight]";
                    System.Data.DataTable GetWgt = Cnn.ExecuteQuery(ConnectionStringMay, layGioiHanKeo);

                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";

                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString, LayKeoQuetTem);
                    float GioiHanKeo = float.Parse(GetWgt.Rows[0][0].ToString());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
            }
            else if (Machno == "02")
            {
                string plannum = "SELECT Plan_Num FROM IF_RtPlan2Mixing where Plan_Id ='" + mesid + "'";
                System.Data.DataTable plann = Cnn.ExecuteQuery(ConnectionStringMay, plannum);
                if (plann.Rows.Count > 0)
                    br1 = plann.Rows[0][0].ToString().Trim();
                //haile mod, kiem tra gioi han keo va insert IF_RtPlan2Mixing
                string checkmes = " Select mesid,[weight],recipe_name,idGrouplot from [InTem].[dbo].[KEORE] where subno='4' and factory ='V' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and mesid ='" + mesid + "' ";
                System.Data.DataTable dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe, checkmes);

                if (dtMEsid.Rows.Count == 0)
                {
                    result = "MES không tồn tại [KEORE], tạo MES khác!";
                    return result;
                }

                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                Plan_Id = dtMEsid.Rows[0][0].ToString().Trim();

                string recipe_name = dtMEsid.Rows[0][2].ToString().Trim();
                string planqty = dtMEsid.Rows[0][1].ToString().Trim();

                //--------------------------2021-07-07----------------------------------
                string DateNow = DateTime.Now.ToString("yyyy-MM-dd 06:30:00").ToString();
                string timeH = DateTime.Now.ToString("HHmm");
                if (int.Parse(timeH) <= 630)
                {
                    DateNow = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 18:30:00").ToString();
                }
                if (idGrouplot == "")
                {
                    string getIdGroupLot = "select MAX(id) from [mfns].[dbo].[Ppt_GroupLot] where RecipeName='" + recipe_name.Replace("-", "").ToUpper().Trim() + "' and [SetNumber]='" + br1 + "' and (CONVERT(datetime,Start_datetime) >= '" + DateNow + "' or CONVERT(datetime,End_datetime) >= '" + DateNow + "')";
                    System.Data.DataTable dtIdGroupLot = Cnn.ExecuteQuery(ConnectionStringMay, getIdGroupLot);

                    if (dtIdGroupLot.Rows.Count > 0)
                    {
                        bool udpKeore = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, "Update [InTem].[dbo].[KEORE] set idGrouplot='" + dtIdGroupLot.Rows[0][0].ToString().Trim() + "' where mesid='" + mesid + "' and [recipe_name]='" + recipe_name + "' and [machno]='" + Machno + "' ");
                    }
                }
                dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe, checkmes);
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                if (idGrouplot == "")
                {
                    result = "Lỗi ko có idGrouplot Liên hệ IT!";
                    return result;
                }
                //----------------------------

                string ktMESplan = " select b.FinishNum * (select sum(set_weight) from[mfns].[dbo].[pmt_weigh] where father_code = b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b " +
                    " where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "' and SetNumber = '" + planqty + "' and End_datetime is not null";
                System.Data.DataTable ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);
                if (ktplan.Rows.Count == 0)
                {
                    string getSetnumber = "select setnumber from [mfns].[dbo].[Ppt_GroupLot] where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "'  and End_datetime is not null ";
                    System.Data.DataTable setNumber = Cnn.ExecuteQuery(ConnectionStringMay, getSetnumber);
                    if (setNumber.Rows.Count > 0)
                    {
                        if (setNumber.Rows[0][0].ToString().Trim() != planqty)
                        {
                            string Update = "Update [InTem].[dbo].[KEORE] set [weight]='" + setNumber.Rows[0][0].ToString().Trim() + "' where mesid='" + Plan_Id + "' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and idGrouplot='" + idGrouplot + "' ";
                            bool UpdatePlan = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, Update);

                            ktMESplan = "select b.FinishNum * (select sum(set_weight) from [mfns].[dbo].[pmt_weigh] where father_code=b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b where id='" + idGrouplot + "'";
                            ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);

                            if (ktplan.Rows.Count == 0)
                            {
                                result = "Liên hệ phòng khống chế tạo lại MES khác!";
                                return result;
                            }
                        }
                    }
                    else
                    {
                        result = "Liên hệ phòng khống chế tạo lại MES khác!";
                        return result;
                    }
                }

                if (makeo == "RD" || makeo == "RB" || kiemkeo2.Rows.Count > 0)
                {
                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString, LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(ktplan.Rows[0][0].ToString().Trim());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
                else
                {
                    string layGioiHanKeo = "SELECT isnull(sum(set_weight)*b.weight,0) FROM [mfns].[dbo].[pmt_weigh] a,[198.1.9.186].[InTem].[dbo].[KEORE] b where b.subno='4' and factory='V' and REPLACE(b.recipe_name,'-','') = a.father_code COLLATE database_default and b.mesid='" + Plan_Id + "' group by b.[weight]";
                    System.Data.DataTable GetWgt = Cnn.ExecuteQuery(ConnectionStringMay, layGioiHanKeo);

                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString, LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(GetWgt.Rows[0][0].ToString());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
            }
            //namnho
            else if (Machno == "03")
            {
                string plannum = "SELECT Plan_Num FROM IF_RtPlan2Mixing where Plan_Id ='" + mesid + "'";
                System.Data.DataTable plann = Cnn.ExecuteQuery(ConnectionStringMay,plannum);
                if (plann.Rows.Count > 0)
                    br1 = plann.Rows[0][0].ToString().Trim();
                //haile mod, kiem tra gioi han keo va insert IF_RtPlan2Mixing
                string checkmes = " Select mesid,[weight],recipe_name,idGrouplot from [InTem].[dbo].[KEORE] where subno='4' and factory ='V' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and mesid ='" + mesid + "' ";
                System.Data.DataTable dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe ,checkmes);

                if (dtMEsid.Rows.Count == 0)
                {
                    result = "MES không tồn tại [KEORE], tạo MES khác!";
                    return result;
                }

                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                Plan_Id = dtMEsid.Rows[0][0].ToString().Trim();

                string recipe_name = dtMEsid.Rows[0][2].ToString().Trim();
                string planqty = dtMEsid.Rows[0][1].ToString().Trim();

                //--------------------------2021-07-07----------------------------------
                string DateNow = DateTime.Now.ToString("yyyy-MM-dd 06:30:00").ToString();
                string timeH = DateTime.Now.ToString("HHmm");
                if (int.Parse(timeH) <= 630)
                {
                    DateNow = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 18:30:00").ToString();
                }
                if (idGrouplot == "")
                {
                    string getIdGroupLot = "select MAX(id) from [mfns].[dbo].[Ppt_GroupLot] where RecipeName='" + recipe_name.Replace("-", "").ToUpper().Trim() + "' and [SetNumber]='" + br1 + "' and (CONVERT(datetime,Start_datetime) >= '" + DateNow + "' or CONVERT(datetime,End_datetime) >= '" + DateNow + "')";
                    System.Data.DataTable dtIdGroupLot = Cnn.ExecuteQuery(ConnectionStringMay, getIdGroupLot);

                    if (dtIdGroupLot.Rows.Count > 0)
                    {
                        bool udpKeore = Cnn.ExecuteNonQuery(ConnectionStringKeoRe,"Update [InTem].[dbo].[KEORE] set idGrouplot='" + dtIdGroupLot.Rows[0][0].ToString().Trim() + "' where mesid='" + mesid + "' and [recipe_name]='" + recipe_name + "' and [machno]='" + Machno + "' ");
                    }
                }
                dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe ,checkmes);
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                if (idGrouplot == "")
                {
                    result = "Lỗi ko có idGrouplot Liên hệ IT!";
                    return result;
                }
                //----------------------------

                string ktMESplan = " select b.FinishNum * (select sum(set_weight) from[mfns].[dbo].[pmt_weigh] where father_code = b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b " +
                    " where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "' and SetNumber = '" + planqty + "' and End_datetime is not null";
                System.Data.DataTable ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);
                if (ktplan.Rows.Count == 0)
                {
                    string getSetnumber = "select setnumber from [mfns].[dbo].[Ppt_GroupLot] where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "'  and End_datetime is not null ";
                    System.Data.DataTable setNumber = Cnn.ExecuteQuery(ConnectionStringMay, getSetnumber);
                    if (setNumber.Rows.Count > 0)
                    {
                        if (setNumber.Rows[0][0].ToString().Trim() != planqty)
                        {
                            string Update = "Update [InTem].[dbo].[KEORE] set [weight]='" + setNumber.Rows[0][0].ToString().Trim() + "' where mesid='" + Plan_Id + "' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and idGrouplot='" + idGrouplot + "' ";
                            bool UpdatePlan = Cnn.ExecuteNonQuery(ConnectionStringKeoRe,Update);

                            ktMESplan = "select b.FinishNum * (select sum(set_weight) from [mfns].[dbo].[pmt_weigh] where father_code=b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b where id='" + idGrouplot + "'";
                            ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);

                            if (ktplan.Rows.Count == 0)
                            {
                                result = "Liên hệ phòng khống chế tạo lại MES khác!";
                                return result;
                            }
                        }
                    }
                    else
                    {
                        result = "Liên hệ phòng khống chế tạo lại MES khác!";
                        return result;
                    }
                }

                if (makeo == "RD" || makeo == "RB" || kiemkeo2.Rows.Count > 0)
                {
                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString,LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(ktplan.Rows[0][0].ToString().Trim());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
                else
                {
                    string layGioiHanKeo = "SELECT isnull(sum(set_weight)*b.weight,0) FROM [mfns].[dbo].[pmt_weigh] a,[198.1.9.186].[InTem].[dbo].[KEORE] b where b.subno='4' and factory='V' and REPLACE(b.recipe_name,'-','') = a.father_code COLLATE database_default and b.mesid='" + Plan_Id + "' group by b.[weight]";
                    System.Data.DataTable GetWgt = Cnn.ExecuteQuery(ConnectionStringMay, layGioiHanKeo);

                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString,LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(GetWgt.Rows[0][0].ToString());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
            }
            else if (Machno == "04")
            {
                string plannum = "SELECT Plan_Num FROM IF_RtPlan2Mixing where Plan_Id ='" + mesid + "'";
                System.Data.DataTable plann = Cnn.ExecuteQuery(ConnectionStringMay, plannum);
                if (plann.Rows.Count > 0)
                    br1 = plann.Rows[0][0].ToString().Trim();
                //haile mod, kiem tra gioi han keo va insert IF_RtPlan2Mixing
                string checkmes = " Select mesid,[weight],recipe_name,idGrouplot from [InTem].[dbo].[KEORE] where subno='4' and factory ='V' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and mesid ='" + mesid + "' ";
                System.Data.DataTable dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe,checkmes);

                if (dtMEsid.Rows.Count == 0)
                {
                    result = "MES không tồn tại [KEORE], tạo MES khác!";
                    return result;
                }
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                Plan_Id = dtMEsid.Rows[0][0].ToString().Trim();


                string recipe_name = dtMEsid.Rows[0][2].ToString().Trim();
                string planqty = dtMEsid.Rows[0][1].ToString().Trim();

                //--------------------------2021-07-07----------------------------------
                string DateNow = DateTime.Now.ToString("yyyy-MM-dd 06:30:00").ToString();
                string timeH = DateTime.Now.ToString("HHmm");
                if (int.Parse(timeH) <= 630)
                {
                    DateNow = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 18:30:00").ToString();
                }
                if (idGrouplot == "")
                {
                    string getIdGroupLot = "select MAX(id) from [mfns].[dbo].[Ppt_GroupLot] where RecipeName='" + recipe_name.Replace("-", "").ToUpper().Trim() + "' and [SetNumber]='" + br1 + "' and (CONVERT(datetime,Start_datetime) >= '" + DateNow + "' or CONVERT(datetime,End_datetime) >= '" + DateNow + "')";
                    System.Data.DataTable dtIdGroupLot = Cnn.ExecuteQuery(ConnectionStringMay, getIdGroupLot);

                    if (dtIdGroupLot.Rows.Count > 0)
                    {
                        bool udpKeore = Cnn.ExecuteNonQuery(ConnectionStringKeoRe,"Update [InTem].[dbo].[KEORE] set idGrouplot='" + dtIdGroupLot.Rows[0][0].ToString().Trim() + "' where mesid='" + mesid + "' and [recipe_name]='" + recipe_name + "' and [machno]='" + Machno + "' ");
                    }
                }
                dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe,checkmes);
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                if (idGrouplot == "")
                {
                    result = "Lỗi ko có idGrouplot Liên hệ IT!";
                    return result;
                }
                //----------------------------

                string ktMESplan = " select b.FinishNum * (select sum(set_weight) from[mfns].[dbo].[pmt_weigh] where father_code = b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b " +
                    " where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "' and SetNumber = '" + planqty + "' and End_datetime is not null";
                System.Data.DataTable ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);
                if (ktplan.Rows.Count == 0)
                {
                    string getSetnumber = "select setnumber from [mfns].[dbo].[Ppt_GroupLot] where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "'  and End_datetime is not null ";
                    System.Data.DataTable setNumber = Cnn.ExecuteQuery(ConnectionStringMay, getSetnumber);
                    if (setNumber.Rows.Count > 0)
                    {
                        if (setNumber.Rows[0][0].ToString().Trim() != planqty)
                        {
                            string Update = "Update [InTem].[dbo].[KEORE] set [weight]='" + setNumber.Rows[0][0].ToString().Trim() + "' where mesid='" + Plan_Id + "' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and idGrouplot='" + idGrouplot + "' ";
                            bool UpdatePlan = Cnn.ExecuteNonQuery(ConnectionStringKeoRe,Update);

                            ktMESplan = "select b.FinishNum * (select sum(set_weight) from [mfns].[dbo].[pmt_weigh] where father_code=b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b where id='" + idGrouplot + "'";
                            ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);

                            if (ktplan.Rows.Count == 0)
                            {
                                result = "Liên hệ phòng khống chế tạo lại MES khác!";
                                return result;
                            }
                        }
                    }
                    else
                    {
                        result = "Liên hệ phòng khống chế tạo lại MES khác!";
                        return result;
                    }
                }

                if (makeo == "RD" || makeo == "RB" || kiemkeo2.Rows.Count > 0)
                {
                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString, LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(ktplan.Rows[0][0].ToString().Trim());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
                else
                {
                    string layGioiHanKeo = "SELECT isnull(sum(set_weight)*b.weight,0) FROM [mfns].[dbo].[pmt_weigh] a,[198.1.9.186].[InTem].[dbo].[KEORE] b where b.subno='4' and factory='V' and REPLACE(b.recipe_name,'-','') = a.father_code COLLATE database_default and b.mesid='" + Plan_Id + "' group by b.[weight]";
                    System.Data.DataTable GetWgt = Cnn.ExecuteQuery(ConnectionStringMay, layGioiHanKeo);

                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString, LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(GetWgt.Rows[0][0].ToString());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
            }

            //--------------May 05 - 18/03/2021---
            //-----------------------------------
            else if (Machno == "05")
            {
                string plannum = "SELECT Plan_Num FROM IF_RtPlan2Mixing where Plan_Id ='" + mesid + "'";
                System.Data.DataTable plann = Cnn.ExecuteQuery(ConnectionStringMay, plannum);
                if (plann.Rows.Count > 0)
                    br1 = plann.Rows[0][0].ToString().Trim();
                //haile mod, kiem tra gioi han keo va insert IF_RtPlan2Mixing
                string checkmes = " Select mesid,[weight],recipe_name,idGrouplot from [InTem].[dbo].[KEORE] where subno='4' and factory ='V' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and mesid ='" + mesid + "' ";
                System.Data.DataTable dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe, checkmes);

                if (dtMEsid.Rows.Count == 0)
                {
                    result = "MES không tồn tại [KEORE], tạo MES khác!";
                    return result;
                }
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                Plan_Id = dtMEsid.Rows[0][0].ToString().Trim();


                string recipe_name = dtMEsid.Rows[0][2].ToString().Trim();
                string planqty = dtMEsid.Rows[0][1].ToString().Trim();

                //--------------------------2021-07-07----------------------------------
                string DateNow = DateTime.Now.ToString("yyyy-MM-dd 06:30:00").ToString();
                string timeH = DateTime.Now.ToString("HHmm");
                if (int.Parse(timeH) <= 630)
                {
                    DateNow = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 18:30:00").ToString();
                }
                if (idGrouplot == "")
                {
                    string getIdGroupLot = "select MAX(id) from [mfns].[dbo].[Ppt_GroupLot] where RecipeName='" + recipe_name.Replace("-", "").ToUpper().Trim() + "' and [SetNumber]='" + br1 + "' and (CONVERT(datetime,Start_datetime) >= '" + DateNow + "' or CONVERT(datetime,End_datetime) >= '" + DateNow + "')";
                    System.Data.DataTable dtIdGroupLot = Cnn.ExecuteQuery(ConnectionStringMay, getIdGroupLot);

                    if (dtIdGroupLot.Rows.Count > 0)
                    {
                        bool udpKeore = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, "Update [InTem].[dbo].[KEORE] set idGrouplot='" + dtIdGroupLot.Rows[0][0].ToString().Trim() + "' where mesid='" + mesid + "' and [recipe_name]='" + recipe_name + "' and [machno]='" + Machno + "' ");
                    }
                }
                dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe,checkmes);
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                if (idGrouplot == "")
                {
                    result = "Lỗi ko có idGrouplot Liên hệ IT!";
                    return result;
                }
                //----------------------------

                string ktMESplan = " select b.FinishNum * (select sum(set_weight) from[mfns].[dbo].[pmt_weigh] where father_code = b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b " +
                    " where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "' and SetNumber = '" + planqty + "' and End_datetime is not null";
                System.Data.DataTable ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);
                if (ktplan.Rows.Count == 0)
                {
                    string getSetnumber = "select setnumber from [mfns].[dbo].[Ppt_GroupLot] where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "'  and End_datetime is not null ";
                    System.Data.DataTable setNumber = Cnn.ExecuteQuery(ConnectionStringMay, getSetnumber);
                    if (setNumber.Rows.Count > 0)
                    {
                        if (setNumber.Rows[0][0].ToString().Trim() != planqty)
                        {
                            string Update = "Update [InTem].[dbo].[KEORE] set [weight]='" + setNumber.Rows[0][0].ToString().Trim() + "' where mesid='" + Plan_Id + "' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and idGrouplot='" + idGrouplot + "' ";
                            bool UpdatePlan = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, Update);

                            ktMESplan = "select b.FinishNum * (select sum(set_weight) from [mfns].[dbo].[pmt_weigh] where father_code=b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b where id='" + idGrouplot + "'";
                            ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);

                            if (ktplan.Rows.Count == 0)
                            {
                                result = "Liên hệ phòng khống chế tạo lại MES khác!";
                                return result;
                            }
                        }
                    }
                    else
                    {
                        result = "Liên hệ phòng khống chế tạo lại MES khác!";
                        return result;
                    }
                }

                if (makeo == "RD" || makeo == "RB" || kiemkeo2.Rows.Count > 0)
                {
                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString, LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(ktplan.Rows[0][0].ToString().Trim());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
                else
                {
                    string layGioiHanKeo = "SELECT isnull(sum(set_weight)*b.weight,0) FROM [mfns].[dbo].[pmt_weigh] a,[198.1.9.186].[InTem].[dbo].[KEORE] b where b.subno='4' and factory='V' and REPLACE(b.recipe_name,'-','') = a.father_code COLLATE database_default and b.mesid='" + Plan_Id + "' group by b.[weight]";
                    System.Data.DataTable GetWgt = Cnn.ExecuteQuery(ConnectionStringMay, layGioiHanKeo);

                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString, LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(GetWgt.Rows[0][0].ToString());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
            }

            //-----May 06- 24/03/2021-----------
            else if (Machno == "06")
            {
                string plannum = "SELECT Plan_Num FROM IF_RtPlan2Mixing where Plan_Id ='" + mesid + "'";
                System.Data.DataTable plann = Cnn.ExecuteQuery(ConnectionStringMay,plannum);
                if (plann.Rows.Count > 0)
                    br1 = plann.Rows[0][0].ToString().Trim();
                //haile mod, kiem tra gioi han keo va insert IF_RtPlan2Mixing
                string checkmes = " Select mesid,[weight],recipe_name,idGrouplot from [InTem].[dbo].[KEORE] where subno='4' and factory ='V' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and mesid ='" + mesid + "' ";
                System.Data.DataTable dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe,checkmes);

                if (dtMEsid.Rows.Count == 0)
                {
                    result = "MES không tồn tại [KEORE], tạo MES khác!";
                    return result;
                }
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                Plan_Id = dtMEsid.Rows[0][0].ToString().Trim();


                string recipe_name = dtMEsid.Rows[0][2].ToString().Trim();
                string planqty = dtMEsid.Rows[0][1].ToString().Trim();

                //--------------------------2021-07-07----------------------------------
                string DateNow = DateTime.Now.ToString("yyyy-MM-dd 06:30:00").ToString();
                string timeH = DateTime.Now.ToString("HHmm");
                if (int.Parse(timeH) <= 630)
                {
                    DateNow = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 18:30:00").ToString();
                }
                if (idGrouplot == "")
                {
                    string getIdGroupLot = "select MAX(id) from [mfns].[dbo].[Ppt_GroupLot] where RecipeName='" + recipe_name.Replace("-", "").ToUpper().Trim() + "' and [SetNumber]='" + br1 + "' and (CONVERT(datetime,Start_datetime) >= '" + DateNow + "' or CONVERT(datetime,End_datetime) >= '" + DateNow + "')";
                    System.Data.DataTable dtIdGroupLot = Cnn.ExecuteQuery(ConnectionStringMay, getIdGroupLot);

                    if (dtIdGroupLot.Rows.Count > 0)
                    {
                        bool udpKeore = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, "Update [InTem].[dbo].[KEORE] set idGrouplot='" + dtIdGroupLot.Rows[0][0].ToString().Trim() + "' where mesid='" + mesid + "' and [recipe_name]='" + recipe_name + "' and [machno]='" + Machno + "' ");
                    }
                }
                dtMEsid = Cnn.ExecuteQuery(ConnectionStringKeoRe, checkmes);
                idGrouplot = dtMEsid.Rows[0][3].ToString().Trim();
                if (idGrouplot == "")
                {
                    result = "Lỗi ko có idGrouplot Liên hệ IT!";
                    return result;
                }
                //----------------------------

                string ktMESplan = " select b.FinishNum * (select sum(set_weight) from[mfns].[dbo].[pmt_weigh] where father_code = b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b " +
                    " where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "' and SetNumber = '" + planqty + "' and End_datetime is not null";
                System.Data.DataTable ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);
                if (ktplan.Rows.Count == 0)
                {
                    string getSetnumber = "select setnumber from [mfns].[dbo].[Ppt_GroupLot] where id = '" + idGrouplot + "' and RecipeName = '" + recipe_name.Replace("-", "") + "'  and End_datetime is not null ";
                    System.Data.DataTable setNumber = Cnn.ExecuteQuery(ConnectionStringMay, getSetnumber);
                    if (setNumber.Rows.Count > 0)
                    {
                        if (setNumber.Rows[0][0].ToString().Trim() != planqty)
                        {
                            string Update = "Update [InTem].[dbo].[KEORE] set [weight]='" + setNumber.Rows[0][0].ToString().Trim() + "' where mesid='" + Plan_Id + "' and machno ='" + Machno + "' and replace(Recipe_Name,'-','') = '" + partno.Replace("-", "") + "' and idGrouplot='" + idGrouplot + "' ";
                            bool UpdatePlan = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, Update);

                            ktMESplan = "select b.FinishNum * (select sum(set_weight) from [mfns].[dbo].[pmt_weigh] where father_code=b.RecipeName) from [mfns].[dbo].[Ppt_GroupLot] b where id='" + idGrouplot + "'";
                            ktplan = Cnn.ExecuteQuery(ConnectionStringMay, ktMESplan);

                            if (ktplan.Rows.Count == 0)
                            {
                                result = "Liên hệ phòng khống chế tạo lại MES khác!";
                                return result;
                            }
                        }
                    }
                    else
                    {
                        result = "Liên hệ phòng khống chế tạo lại MES khác!";
                        return result;
                    }
                }

                if (makeo == "RD" || makeo == "RB" || kiemkeo2.Rows.Count > 0)
                {
                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString, LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(ktplan.Rows[0][0].ToString().Trim());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
                else
                {
                    string layGioiHanKeo = "SELECT isnull(sum(set_weight)*b.weight,0) FROM [mfns].[dbo].[pmt_weigh] a,[198.1.9.186].[InTem].[dbo].[KEORE] b where b.subno='4' and factory='V' and REPLACE(b.recipe_name,'-','') = a.father_code COLLATE database_default and b.mesid='" + Plan_Id + "' group by b.[weight]";
                    System.Data.DataTable GetWgt = Cnn.ExecuteQuery(ConnectionStringMay, layGioiHanKeo);

                    string LayKeoQuetTem = " select ISNULL(sum([weight]),0) from [erp].[dbo].[prdebe] where subno='4' and factory='V' and machno ='" + machno + "' and mesid ='" + Plan_Id + "' ";
                    System.Data.DataTable getWeightSX = Cnn.ExecuteQuery(ConnectionString,LayKeoQuetTem);

                    float GioiHanKeo = float.Parse(GetWgt.Rows[0][0].ToString());
                    float KeoSX = float.Parse(getWeightSX.Rows[0][0].ToString());
                    float KeoVo = KeoSX + float.Parse(Soluong);

                    if (KeoVo > GioiHanKeo)
                    {
                        if (GioiHanKeo < KeoSX)
                        {
                            result = "MESS này quá số lượng kế hoạch, không thể quét tiếp!";
                            return result;
                        }
                        else
                        {
                            result = "Lỗi! MES này chỉ quét được " + (GioiHanKeo - KeoSX).ToString().Trim() + "KG nữa!";
                            return result;
                        }
                    }
                }
            }
            //----------------------------------
            #endregion
            return result;
        }


        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string NgaySanXuat = tbNgaySanXuat.Text.ToString().Replace("-", "").Trim();
                string TenKeo = tbTenKeo.Text.ToString().Trim();
                string MaMes = drMes.SelectedValue.ToString().Trim();
                string Ca = drCa.SelectedValue.ToString().Trim();
                string CanDao = drCanDao.SelectedValue.ToString().Trim();
                string ThoiGianQuet = tbThoiGianQuet.Text.ToString().Trim();
                string NgayQuet = tbNgayQuet.Text.ToString().Trim();
                string NguoiQuet = Session["UserName"].ToString().Trim();

                string TrongLuong = tbTrongLuong.Text.ToString().Trim();
                string pallet = tbMaPalet.Text.ToString().Trim();

                string KhuVuc = tbKV.Text.ToString().Trim();
                string Xuong = tbX.Text.ToString().Trim();
                string SoLo = tbSoLo.Text.ToString().Trim();
                string Xuat = drXuat.SelectedValue.ToString().Trim();

                string machno = tbMay.Text.ToString().Trim();
                string may = machno.Substring(6, 2);

                string MaVach = "";

                string NgayHieuLuc = "";
                string HanSuDung = "";
                string Loai = "";
                string makeo = "";
                string tenkeo = "";

                string bufff = NgayQuet + "_" + ThoiGianQuet;

                DateTime myDatetime = DateTime.ParseExact(bufff, "yyyyMMdd_HH:mm:ss", CultureInfo.InvariantCulture);

                string smonth = NgaySanXuat.Substring(4, 2).ToString().Trim();
                switch (smonth)
                {
                    case "10": smonth = "A"; break;
                    case "11": smonth = "B"; break;
                    case "12": smonth = "C"; break;
                    default: smonth = smonth.Substring(1, 1); break;
                }
                string spday = NgaySanXuat.Substring(2, 2) + smonth + NgaySanXuat.Substring(NgaySanXuat.Length - 2, 2);
                
                string sqlKEORE = "select mesid, pday from KEORE where subno='4' and factory ='V'  and mesid ='" + MaMes + "' and machno = '" + may + "'";
                System.Data.DataTable dtKEORE = Cnn.ExecuteQuery(ConnectionStringKeoRe, sqlKEORE);

                if (dtKEORE.Rows.Count > 0)
                {

                    //if (dtKEORE.Rows[0][1].ToString().Trim() != pday.Trim())
                    //{
                    //    string s = "Mes quá giờ không quét được";
                    //}

                    switch (TenKeo.Trim().Substring(6).ToUpper())
                    {
                        case "RM": makeo = "RD"; Loai = "3"; break;
                        case "1": makeo = "RB"; Loai = "2"; break;
                        case "9": makeo = "RD"; Loai = "3"; break;
                        case "2": makeo = "RC"; Loai = "2"; break;
                        case "3": makeo = "RC"; Loai = "2"; break;
                        case "4": makeo = "RC"; Loai = "2"; break;
                        case "5": makeo = "RC"; Loai = "2"; break;
                        case "RE": makeo = "RR"; Loai = "3"; break;
                        case "92": makeo = "RD"; Loai = "3"; break;// Sua theo barcode moi

                        case "1-EDGE": makeo = "RB"; Loai = "2"; break;
                        case "2-EDGE": makeo = "RC"; Loai = "2"; break;
                        case "3-EDGE": makeo = "RC"; Loai = "2"; break;
                        case "4-EDGE": makeo = "RC"; Loai = "2"; break;
                        case "5-EDGE": makeo = "RC"; Loai = "2"; break;
                        case "9-EDGE": makeo = "RD"; Loai = "3"; break;

                        case "1EDGE": tenkeo = TenKeo.Substring(0, 5) + "-1-EDGE"; makeo = "RB"; Loai = "2"; break;
                        case "2EDGE": tenkeo = TenKeo.Substring(0, 5) + "-2-EDGE"; makeo = "RC"; Loai = "2"; break;
                        case "3EDGE": tenkeo = TenKeo.Substring(0, 5) + "-3-EDGE"; makeo = "RC"; Loai = "2"; break;
                        case "4EDGE": tenkeo = TenKeo.Substring(0, 5) + "-4-EDGE"; makeo = "RC"; Loai = "2"; break;
                        case "5EDGE": tenkeo = TenKeo.Substring(0, 5) + "-5-EDGE"; makeo = "RC"; Loai = "2"; break;
                        case "9EDGE": tenkeo = TenKeo.Substring(0, 5) + "-9-EDGE"; makeo = "RD"; Loai = "3"; break;

                        case "1THU": tenkeo = TenKeo.Substring(0, 5) + "-1THU"; makeo = "RB"; Loai = "2"; break;
                        case "2THU": tenkeo = TenKeo.Substring(0, 5) + "-2THU"; makeo = "RC"; Loai = "2"; break;
                        case "3THU": tenkeo = TenKeo.Substring(0, 5) + "-3THU"; makeo = "RC"; Loai = "2"; break;
                        case "4THU": tenkeo = TenKeo.Substring(0, 5) + "-4THU"; makeo = "RC"; Loai = "2"; break;
                        case "5THU": tenkeo = TenKeo.Substring(0, 5) + "-5THU"; makeo = "RC"; Loai = "2"; break;
                        case "9THU": tenkeo = TenKeo.Substring(0, 5) + "-9THU"; makeo = "RD"; Loai = "3"; break;
                    }

                    //string ktkeo2 = "select * from [InTem].[dbo].[rubnod_Ptype] where rubno_7 ='" + TenKeo.Trim() + "'";
                    //System.Data.DataTable ktke = Cnn.ExecuteQuery(ConnectionStringKeoRe, ktkeo2);

                    //if (ktke.Rows.Count > 0)
                    //{
                    //    makeo = "RB";
                    //    Loai = "2";
                    //}

                    // Sua theo barcode moi
                    string ktptype = "SELECT [ptype],[rubno_7] FROM[InTem].[dbo].[rubnod_Ptype] WHERE rubno_7 = '" + TenKeo.Trim() + "'";
                    System.Data.DataTable KT_ptype = Cnn.ExecuteQuery(ConnectionStringKeoRe,ktptype);
                    if (KT_ptype.Rows.Count >= 2)
                    {
                        lbError.Visible = true;
                        lbError.Text = "Liên hệ phòng thí nghiệm (a Thuần) đóng 1 tiêu chuẩn";
                        ResetData();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                    }
                    else
                    {
                        if (KT_ptype.Rows.Count > 0)
                        {
                            makeo = "RB";
                            Loai = KT_ptype.Rows[0][0].ToString().Trim();
                        }
                        // else KT_ptype không có data thì không làm gì cả, giữ nguyên makeo, ptype theo xử lý đầu vào.
                    }

                    string sqleffdat = "select mesid from KEORE where subno='4' and factory ='V' and comp= 'N'  and mesid ='" + MaMes + "' and machno = '" + machno + "'";
                    dtKEORE = Cnn.ExecuteQuery(ConnectionStringKeoRe,sqleffdat);
                    if (dtKEORE.Rows.Count > 0)
                    {
                        string getKeo = "select expday from [erp].[dbo].[prdexp] where subno='4' and factory='V' and  ptype ='" + makeo + "' and rubno='" + TenKeo.Substring(0, 5) + "'";
                        System.Data.DataTable keo = Cnn.ExecuteQuery(ConnectionString,getKeo);

                        if (keo.Rows.Count == 0)
                        {

                            lbError.Visible = true;
                            lbError.Text = "Mã keo không được sử dụng.\n Liên hệ Duyên phòng chế tạo (755) !";
                            ResetData();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                        }
                        else
                        {
                            int day = int.Parse(keo.Rows[0][0].ToString().Trim());
                            HanSuDung = day.ToString().Trim();
                            NgayHieuLuc = DateTime.Now.AddDays(day).ToString("yyyyMMdd");
                        }
                    }
                    //string getKeo = "select expday from [erp].[dbo].[prdexp] where subno='4' and factory='V' and  ptype ='" + makeo + "' and rubno='" + TenKeo.Substring(0, 5) + "'";
                    //System.Data.DataTable keo = Cnn.ExecuteQuery(ConnectionString, getKeo);
                    //if (keo.Rows.Count == 0)
                    //{
                    //    lbError.Visible = true;
                    //    lbError.Text = "Không có Hạn Sử Dụng. Liên hệ phòng đảm bảo chất lượng, Sunkist(Bình Sung)!";
                    //    ResetData();
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                    //}
                    //else
                    //{
                    //    int day = int.Parse(keo.Rows[0][0].ToString().Trim());
                    //    HanSuDung = day.ToString().Trim();
                    //    NgayHieuLuc = myDatetime.AddDays(day).ToString("yyyyMMdd");
                    //}

                    string sqlBarcode = " select MAX(SUBSTRING(Barcode,8,3)) num from prdebe where subno = '4' and factory = 'V' and barcode like '" + makeo + "%' and prodat = '" + NgaySanXuat + "'";
                    System.Data.DataTable dtBar = Cnn.ExecuteQuery(ConnectionString, sqlBarcode);
                    //lấy barcode theo từng loại keo

                    if (dtBar.Rows.Count == 1 && dtBar.Rows[0][0].ToString().Trim() == "")
                        MaVach = makeo + spday + "001";
                    else
                        MaVach = makeo + spday + (int.Parse(dtBar.Rows[0][0].ToString()) + 1).ToString("000");
                }
                else
                {
                    lbError.Visible = true;
                    lbError.Text = "Mã MES đã bị đóng! Liên hệ IT mở!";
                    ResetData();
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                }

                string resultAA =  KiemTraGioHanKeo(TrongLuong, tenkeo, machno, MaMes, TenKeo, MaVach);
                if (resultAA == "")
                {
                    if (tbKV.Text == "" || tbX.Text == "" || tbMay.Text == "" || tbSoLo.Text == "" || tbThoiGianQuet.Text == "" || tbNgayQuet.Text == "" || tbNguoiQuet.Text == "")
                    {
                        lbError.Visible = true;
                        lbError.Text = "Vui lòng nhập đủ dữ liệu!!!";
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                    }
                    else
                    {
                        //int result = Test.ThemLieuBB(KhuVuc, Xuong, MaMes, machno, HanSuDung, MaVach, SoLo, TrongLuong, NgaySanXuat, NgayHieuLuc, Ca, Loai, CanDao, TenKeo, ThoiGianQuet, NgayQuet, NguoiQuet, pallet, Xuat);

                        //if (result == 1)
                        //{
                        //    ThongBao("Thêm dữ liệu thành công!!!");
                        //}
                        //else
                        //{
                        //    lbError.Visible = true;
                        //    lbError.Text = "Dữ liệu không hợp lệ!!!";
                        //    ResetData();
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                        //}

                        string GetData = "insert into [erp].[dbo].[prdebe]" +
                                    "(subno,factory,mesid,machno,daylimt,barcode,slipno,weight,prodat,effdat,class,ptype," +
                                    "status,partno,intime,indat,usrno,pallet_no,active) " +
                                    "values('" + KhuVuc + "', '" + Xuong + "', '" + MaMes + "', '" + machno + "', '" + HanSuDung + "', '" + MaVach + "', '" + SoLo + "', '" + TrongLuong + "', " +
                                    "'" + NgaySanXuat + "', '" + NgayHieuLuc + "', '" + Ca + "', '" + Loai + "', '" + CanDao + "', " +
                                    "'" + TenKeo + "', '" + ThoiGianQuet + "', '" + NgayQuet + "', '" + NguoiQuet + "', '" + pallet + "', '" + Xuat + "')";

                        bool result = Cnn.ExecuteNonQuery(ConnectionString, GetData);
                        if (result == true)
                        {
                            ThongBao("Thêm dữ liệu thành công!!!");
                        }
                        else
                        {
                            lbError.Visible = true;
                            lbError.Text = "Dữ liệu không hợp lệ!!!";
                            ResetData();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                        }
                    } 
                }
                else
                {
                    lbError.Visible = true;
                    lbError.Text = resultAA;                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                }
            }
            catch(Exception ex)
            {
                lbError.Visible = true;
                lbError.Text = "Mã mesid không được để trống, xin kiểm tra lại!!!";
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
            }
        }

        private void ResetData()
        {
            tbMay.Text = "";
            tbNgayQuet.Text = "";
            tbNguoiQuet.Text = "";
            tbThoiGianQuet.Text = "";
            tbKV.Text = "";
            tbX.Text = "";
            tbSoLo.Text = "";
            tbTrongLuong.Text = "";
            tbMaPalet.Text = "";
            tbSoKyChenhLech.Text = "";
            drMes.Items.Clear();

            tbKV1.Text = "";
            tbX1.Text = "";
            tbMes1.Text = "";
            tbMay1.Text = "";
            tbHanSuDung1.Text = "";
            tbMaVach1.Text = "";
            tbSoLo1.Text = "";
            tbTrongLuong1.Text = "";
            tbNgaySanXuat1.Text = "";
            tbNgayHieuLuc1.Text = "";
            tbLoai1.Text = "";
            tbTenKeo1.Text = "";
            tbThoiGianQuet1.Text = "";
            tbNgayQuet1.Text = "";
            tbNguoiQuet1.Text = "";
            tbMaPalet1.Text = "";
        }

        private void LoadForm(string mesid)
        {
            try 
            {
                string GetData = "SELECT subno,factory,mesid,machno,daylimt,barcode,slipno,weight,prodat,effdat,class,ptype,status,partno,intime,indat,usrno,pallet_no,active " +
                                "FROM [erp].[dbo].[prdebe] where mesid = '" + mesid.ToString().Trim() + "'";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString, GetData);
                tbMay.Text = dt.Rows[0]["machno"].ToString().Trim(); tbMay.Enabled = false;
                tbNgayQuet.Text = dt.Rows[0]["indat"].ToString().Trim(); tbNgayQuet.Enabled = false;
                tbNguoiQuet.Text = dt.Rows[0]["usrno"].ToString().Trim(); tbNguoiQuet.Enabled = false;
                tbThoiGianQuet.Text = dt.Rows[0]["intime"].ToString().Trim(); tbThoiGianQuet.Enabled = false;
                drCa.SelectedValue = dt.Rows[0]["class"].ToString().Trim(); drCa.Enabled = false;
                drCanDao.SelectedValue = dt.Rows[0]["status"].ToString().Trim(); drCanDao.Enabled = false;

                tbKV.Text = dt.Rows[0]["subno"].ToString().Trim(); tbKV.Enabled = false;
                tbX.Text = dt.Rows[0]["factory"].ToString().Trim(); tbX.Enabled = false;
                tbSoLo.Text = dt.Rows[0]["slipno"].ToString().Trim(); tbSoLo.Enabled = false;
                drXuat.SelectedValue = dt.Rows[0]["active"].ToString().Trim(); drXuat.Enabled = false;
                float SKCL = SoKyChenhlech(mesid, tbMay.Text);
                if (SKCL == -1)
                {
                    lbError.Visible = true;
                    lbError.Text = "Máy đang tắt, vui lòng mở máy và thử lại!!!";
                    ResetData();
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                }
                else
                {
                    tbSoKyChenhLech.Text = SKCL.ToString().Trim(); tbSoKyChenhLech.Enabled = false;
                }
                if (int.Parse(tbSoKyChenhLech.Text) <= 0)
                {
                    tbTrongLuong.ReadOnly = true; tbTrongLuong.Enabled = false;
                    tbMaPalet.ReadOnly = true; tbMaPalet.Enabled = false;
                }
                else
                {
                    tbTrongLuong.ReadOnly = false; tbTrongLuong.Enabled = true;
                    tbMaPalet.ReadOnly = false; tbMaPalet.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                lbError.Visible = true;
                lbError.Text = "?????????";
                ResetData();
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
            }
        }

        private float SoKyChenhlech(string mesid, string may)
        {
            string GetGrLotID = "select k.idGrouplot as idGrouplot, cast(SUM(p.weight)as int) as skqt " +
            "from[erp].[dbo].[prdebe] as p INNER JOIN[198.1.9.186].[InTem].[dbo].[KEORE] as k ON p.mesid = k.mesid " +
            "where p.mesid = '" + mesid.ToString().Trim() + "' group by k.idGrouplot,p.mesid,p.machno,p.partno";
            System.Data.DataTable dtID = Cnn.ExecuteQuery(ConnectionString, GetGrLotID);
            string GrlotID = dtID.Rows[0]["idGrouplot"].ToString().Trim();
            int SKQT = int.Parse(dtID.Rows[0]["skqt"].ToString().Trim());
            string ConnectionStringMay = ChonMay(may.ToString().Trim().Substring(6, 2));
            if (ConnectionStringMay == "")
            {
                return -1;
            }
            string GetSoKCL = "SELECT a.id, a.RecipeCode, a.SetNumber, a.FinishNum,cast(SUM(b.set_weight) as int) as SKTC,cast(a.FinishNum*SUM(b.set_weight) as int) as Swgt " +
                "FROM[mfns].[dbo].[Ppt_GroupLot] a,[mfns].[dbo].[pmt_weigh] b where a.RecipeCode = b.father_code and id = '" + GrlotID + "' group by id,RecipeCode,FinishNum,SetNumber";
            System.Data.DataTable dtSoKy = Cnn.ExecuteQuery(ConnectionStringMay, GetSoKCL);
            int SKTC = int.Parse(dtSoKy.Rows[0]["Swgt"].ToString().Trim());
            float SKCL = SKTC - SKQT;
            return SKCL;
        }

        protected void tbNgaySanXuat_TextChanged(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=erp;User ID=kendakv2;Password=kenda123";
            string GetData = "select mesid from [erp].[dbo].[prdebe] where prodat = '" + tbNgaySanXuat.Text.ToString().Replace("-", "").Trim() + "' and partno = '" + tbTenKeo.Text.ToString().Trim() + "' group by mesid";
            System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString, GetData);
            if (dt.Rows.Count == 0)
            {
                lbError.Visible = true;
                lbError.Text = "Không có dữ liệu";
                ResetData();
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
            }
            else
            {
                lbError.Visible = false;
                drMes.DataSource = dt;
                drMes.DataTextField = "mesid";
                drMes.DataValueField = "mesid";
                drMes.DataBind();
                LoadForm(drMes.SelectedValue);
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
            }
        }

        protected void drMes_TextChanged(object sender, EventArgs e)
        {
            LoadForm(drMes.SelectedValue);
            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
        }

        protected void tbTenKeo_TextChanged(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=erp;User ID=kendakv2;Password=kenda123";
            string GetData = "select mesid from [erp].[dbo].[prdebe] where prodat = '" + tbNgaySanXuat.Text.ToString().Replace("-", "").Trim() + "' and partno = '" + tbTenKeo.Text.ToString().Trim() + "' group by mesid";
            System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString, GetData);
            if (dt.Rows.Count == 0)
            {
                lbError.Visible = true;
                lbError.Text = "Không có dữ liệu";
                ResetData();
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
            }
            else
            {
                lbError.Visible = false;
                drMes.DataSource = dt;
                drMes.DataTextField = "mesid";
                drMes.DataValueField = "mesid";
                drMes.DataBind();
                LoadForm(drMes.SelectedValue);
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string strSearch = txtSearch.Text.Trim();
            if (strSearch == "")
            {
                ThongBao("Vui lòng nhập tem quét hoặc mã Pallet!!!");
                return;
            }
            else
            {
                string getData = "SELECT subno,factory,mesid,machno,daylimt,barcode,slipno,weight,prodat,effdat,class,ptype,status,partno,intime,indat,usrno,pallet_no,active " +
                                "FROM [erp].[dbo].[prdebe] " +
                                "where barcode='" + strSearch + "' or pallet_no='" + strSearch + "'";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionString, getData);              
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu!!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                }
                else
                {                   
                    gvKQ.DataSource = dt;
                    gvKQ.DataBind();
                }
            }
        }

        protected void gvKQ_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string KhuVuc = Dulieu[0].Trim();
            string Xuong = Dulieu[1].Trim();
            string Mes = Dulieu[2].Trim();
            string May = Dulieu[3].Trim();
            string HanSuDung = Dulieu[4].Trim();
            string MaVach = Dulieu[5].Trim();
            string SoLo = Dulieu[6].Trim();
            string TrongLuong = Dulieu[7].Trim();
            string NgaySanXuat = Dulieu[8].Trim();
            string NgayHieuLuc = Dulieu[9].Trim();
            string Ca = Dulieu[10].Trim();
            string Loai = Dulieu[11].Trim();
            string CanDao = Dulieu[12].Trim();
            string TenKeo = Dulieu[13].Trim();
            string ThoiGianQuet = Dulieu[14].Trim();
            string NgayQuet = Dulieu[15].Trim();
            string NguoiQuet = Session["UserName"].ToString().Trim();
            //string NguoiQuet = Dulieu[16].Trim();
            string MaPalet = Dulieu[17].Trim();
            string Xuat = Dulieu[18].Trim();

            switch (e.CommandName)
            {
                case "btnEdit":
                    ResetData();
                    tbKV1.Text = KhuVuc; tbKV1.Enabled = false;
                    tbX1.Text = Xuong; tbX1.Enabled = false;
                    tbMes1.Text = Mes; tbMes1.Enabled = false;
                    tbMay1.Text = May; tbMay1.Enabled = false;
                    tbHanSuDung1.Text = HanSuDung; tbHanSuDung1.Enabled = false;
                    tbMaVach1.Text = MaVach; tbMaVach1.Enabled = false;
                    tbSoLo1.Text = SoLo; tbSoLo1.Enabled = false;
                    tbTrongLuong1.Text = TrongLuong;
                    tbNgaySanXuat1.Text = NgaySanXuat; tbNgaySanXuat1.Enabled = false;
                    tbNgayHieuLuc1.Text = NgayHieuLuc; tbNgayHieuLuc1.Enabled = false;
                    drCa1.SelectedValue = Ca; drCa1.Enabled = false;
                    tbLoai1.Text = Loai; tbLoai1.Enabled = false;
                    drCanDao1.Text = CanDao; drCanDao1.Enabled = false;
                    tbTenKeo1.Text = TenKeo; tbTenKeo1.Enabled = false;
                    tbThoiGianQuet1.Text = ThoiGianQuet; tbThoiGianQuet1.Enabled = false;
                    tbNgayQuet1.Text = NgayQuet; tbNgayQuet1.Enabled = false;
                    tbNguoiQuet1.Text = NguoiQuet; tbNguoiQuet1.Enabled = false;
                    tbMaPalet1.Text = MaPalet;
                    drXuat1.Text = Xuat;
                    Session["TL"] = tbTrongLuong1.Text;

                    if (SoKyChenhlech(Mes.ToString().Trim(), May) == -1)
                    {
                        lbError1.Visible = true;
                        lbError1.Text = "Máy đang tắt, vui lòng mở máy và thử lại!!!";
                    }
                    else
                    {
                        tbSoKyChenhLech1.Text = SoKyChenhlech(Mes.ToString().Trim(), May).ToString().Trim(); tbSoKyChenhLech1.Enabled = false;
                    }
                    if (int.Parse(tbSoKyChenhLech1.Text) <= 0)
                    {
                        tbTrongLuong1.ReadOnly = true; tbTrongLuong1.Enabled = false;
                        tbMaPalet1.ReadOnly = true; tbMaPalet1.Enabled = false;
                    }
                    else
                    {
                        tbTrongLuong1.ReadOnly = false; tbTrongLuong1.Enabled = true;
                        tbMaPalet1.ReadOnly = false; tbMaPalet1.Enabled = true;
                    }
                    int i = 0;
                    string[] sPrinterList = new string[System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count];
                    if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count == 0)
                        sPrinterList[i] = "";
                    else
                    {
                        drMayIn1.Items.Clear();
                        foreach (string printers in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                        {
                            if (printers.Substring(printers.Length - 3, 3) == "193" || printers.Substring(printers.Length - 3, 3) == "176" || printers.Substring(printers.Length - 3, 3) == "122" || printers.Substring(printers.Length - 3, 3) == "05)")
                            {
                                sPrinterList[i] = printers;
                                i++;
                                drMayIn1.Items.Add(printers);
                            }
                        }
                        if (i == 0)
                        {
                            string r = "Vui lòng kết nối máy in!!!";
                            drMayIn1.Items.Add(r);
                        }
                    }
                    lbError1.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvSua();", true);
                    break;
                case "btnDelete":
                    break;
            }
        }

        protected void btnSua_Click(object sender, EventArgs e)
        {
            string KhuVuc = tbKV1.Text.ToString().Trim();
            string Xuong = tbX1.Text.ToString().Trim();
            string MaMes = tbMes1.Text.ToString().Trim();
            string machno = tbMay1.Text.ToString().Trim();
            string HanSuDung = tbHanSuDung1.ToString().Trim();
            string MaVach = tbMaVach1.Text.ToString().Trim();
            string SoLo = tbSoLo1.Text.ToString().Trim();
            string TrongLuong = tbTrongLuong1.Text.ToString().Trim();
            string NgaySanXuat = tbNgaySanXuat1.Text.ToString().Trim();
            string NgayHieuLuc = tbNgayHieuLuc1.Text.ToString().Trim();
            string Ca = drCa1.SelectedValue.ToString().Trim();
            string Loai = tbLoai1.Text.ToString().Trim();
            string CanDao = drCanDao1.SelectedValue.ToString().Trim();
            string TenKeo = tbTenKeo1.Text.ToString().Trim();
            string ThoiGianQuet = tbThoiGianQuet1.Text.ToString().Trim();
            string NgayQuet = tbNgayQuet1.Text.ToString().Trim();
            string NguoiQuet = Session["UserName"].ToString().Trim();
            string pallet = tbMaPalet1.Text.ToString().Trim();
            string Xuat = drXuat1.SelectedValue.ToString().Trim();


            int result = Test.SuaLieuBB(KhuVuc, Xuong, MaMes, machno, HanSuDung, MaVach, SoLo, TrongLuong, NgaySanXuat, NgayHieuLuc, Ca, Loai, CanDao, TenKeo, ThoiGianQuet, NgayQuet, NguoiQuet, pallet, Xuat);

            if (result == 1)
            {
                ThongBao("Cập nhật thành công !!!");
                ResetData();
            }
            else
            {
                lbError1.Visible = true;
                lbError1.Text = "Cập nhật không thành công, thử lại!!!";
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvSua();", true);
            }

            //string GetData = "UPDATE [erp].[dbo].[prdebe] " +
            //                "SET subno ='" + KhuVuc + "',factory='" + Xuong + "',mesid='" + MaMes + "',machno='" + machno + "',daylimt='" + HanSuDung + "'," +
            //                "slipno='" + SoLo + "',weight='" + TrongLuong + "'," +
            //                "prodat='" + NgaySanXuat + "',effdat='" + NgayHieuLuc + "',class='" + Ca + "',ptype='" + Loai + "',status='" + CanDao + "',partno='" + TenKeo + "'," +
            //                "intime='" + ThoiGianQuet + "',indat='" + NgayQuet + "',usrno='" + NguoiQuet + "',pallet_no='" + pallet + "',active='" + Xuat + "' " +
            //                "where barcode = '" + MaVach + "'";
            //bool result = Cnn.ExecuteNonQuery(ConnectionString, GetData);
            //if (result == true)
            //{
            //    ThongBao("Cập nhật thành công !!!");
            //    ResetData();
            //}
            //else
            //{
            //    lbError1.Visible = true;
            //    lbError1.Text = "Cập nhật không thành công, thử lại!!!";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvSua();", true);
            //}
        }

        protected void tbTrongLuong_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbSoKyChenhLech.Text == "")
                {
                    tbSoKyChenhLech.Text = "0";
                }
                if (tbTrongLuong.Text == "")
                {
                    lbError.Visible = true;
                    lbError.Text = "Vui lòng nhập trọng lượng!!!";
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                }
                else
                {
                    if (int.Parse(tbTrongLuong.Text) > int.Parse(tbSoKyChenhLech.Text))
                    {
                        lbError.Visible = true;
                        lbError.Text = "Trọng lượng nhập vào không được lớn hơn số ký chênh lệch!!!";
                        tbTrongLuong.Text = "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                    }
                    else if (int.Parse(tbTrongLuong.Text) <= 0)
                    {
                        lbError.Visible = true;
                        lbError.Text = "Trọng lượng nhập vào phải lớn hơn 0!!!";
                        tbTrongLuong.Text = "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                    }
                    else
                    {
                        lbError.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                    }
                }
            }
            catch (Exception)
            {
                lbError.Visible = true;
                lbError.Text = "Trọng lượng nhập vào không phù hợp!!!";
                tbTrongLuong.Text = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
            }
        }

        protected void btnInLieu_Click(object sender, EventArgs e)
        {
            string KhuVuc = tbKV1.Text.ToString().Trim();
            string Xuong = tbX1.Text.ToString().Trim();
            string MaMes = tbMes1.Text.ToString().Trim();
            string machno = tbMay1.Text.ToString().Trim();
            string HanSuDung = tbHanSuDung1.Text.ToString().Trim();
            string MaVach = tbMaVach1.Text.ToString().Trim();
            string SoLo = tbSoLo1.Text.ToString().Trim();
            string TrongLuong = tbTrongLuong1.Text.ToString().Trim();
            string NgaySanXuat = tbNgaySanXuat1.Text.ToString().Trim();
            string NgayHieuLuc = tbNgayHieuLuc1.Text.ToString().Trim();
            string Ca = drCa1.SelectedValue.ToString().Trim();
            //string Loai = tbLoai1.Text.ToString().Trim();
            //string CanDao = drCanDao1.SelectedValue.ToString().Trim();
            string TenKeo = tbTenKeo1.Text.ToString().Trim();
            string ThoiGianQuet = tbThoiGianQuet1.Text.ToString().Trim();
            string NgayQuet = tbNgayQuet1.Text.ToString().Trim();
 /*           string NguoiQuet = Session["UserName"].ToString().Trim()*/;
            string pallet = tbMaPalet1.Text.ToString().Trim();
            //string Xuat = drXuat1.SelectedValue.ToString().Trim();
            string printer = drMayIn1.SelectedValue.ToString();
            string may = machno.Substring(6, 2);
            string maso = "";
            string sqlKEORE2 = "select mesid from KEORE where factory='V' and mesid ='" + MaMes + "' and machno = '" + may + "'";
            System.Data.DataTable dtKEORE2 = new System.Data.DataTable();
            string ConnectionStringKeoRe = "Data Source=198.1.9.186;Initial Catalog=InTem;User ID=kendakv2;Password=kenda123";
            dtKEORE2 = Cnn.ExecuteQuery(ConnectionStringKeoRe, sqlKEORE2);
            if (dtKEORE2.Rows.Count > 0)
            {
                string sqlBarcode = "SELECT * FROM prdebe where factory='V' and prodat='" + NgaySanXuat + "' and partno='" + TenKeo + "'" +
                    " and class='" + Ca + "' and barcode <= '" + MaVach + "' order by intime";
                System.Data.DataTable dtBar = Cnn.ExecuteQuery(ConnectionString, sqlBarcode);
                //lấy barcode theo từng loại keo
                int a = 1;
                int b = 2;
                if (dtBar.Rows.Count > 0)
                {
                    for (int i = 0; i < dtBar.Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            a += 2;
                            b = a + 1;
                        }
                    }

                    string sqlweight = "SELECT weight FROM KEORE where factory='V' and mesid ='" + MaMes + "' and machno='" + may + "'"; //lấy Barcode từ trong sql
                    System.Data.DataTable dtweight = Cnn.ExecuteQuery(ConnectionStringKeoRe, sqlweight);
                    int weight = Convert.ToInt32(dtweight.Rows[0][0].ToString());
                    try
                    {
                        if (weight == 1)
                        {
                            maso = a.ToString();
                        }
                        else
                        {
                            maso = a.ToString() + "-" + b.ToString();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            if (drMayIn1.Text == "Vui lòng kết nối máy in!!!")
            {
                lbError.Visible = true;
                lbError.Text = "Vui lòng chọn máy in !!!";
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);

            }
            else
            {
                PrintExcel(may, MaVach.Substring(0, 2).Trim(), Ca, pallet, HanSuDung, NgayQuet, ThoiGianQuet, SoLo, TrongLuong, NgayHieuLuc, TenKeo, maso, MaVach, printer);
            }
        }

        protected void tbTrongLuong1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double sktt = int.Parse(tbSoKyChenhLech1.Text) + double.Parse(Session["TL"].ToString().Trim());
                if (int.Parse(tbTrongLuong1.Text) > sktt || int.Parse(tbTrongLuong1.Text) <= 0)
                {
                    lbError1.Visible = true;
                    lbError1.Text = "Trọng lượng nhập vào quá lớn!!!";
                    tbTrongLuong1.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvSua();", true);
                }
                else
                {
                    lbError1.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvSua();", true);
                }
            }
            catch
            {
                lbError1.Visible = true;
                lbError1.Text = "Trọng lượng nhập vào không phù hợp!!!";
                tbTrongLuong1.Text = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvSua();", true);
            }
        }

        protected void btnIn1_Click(object sender, EventArgs e)
        {
            try
            {
                string NgaySanXuat = tbNgaySanXuat.Text.ToString().Replace("-", "").Trim();
                string TenKeo = tbTenKeo.Text.ToString().Trim();
                string MaMes = drMes.SelectedValue.ToString().Trim();
                string Ca = drCa.SelectedValue.ToString().Trim();
                string CanDao = drCanDao.SelectedValue.ToString().Trim();
                string ThoiGianQuet = tbThoiGianQuet.Text.ToString().Trim();
                string NgayQuet = tbNgayQuet.Text.ToString().Trim();
                string NguoiQuet = Session["UserName"].ToString().Trim();

                string TrongLuong = tbTrongLuong.Text.ToString().Trim();
                string pallet = tbMaPalet.Text.ToString().Trim();

                string KhuVuc = tbKV.Text.ToString().Trim();
                string Xuong = tbX.Text.ToString().Trim();
                string SoLo = tbSoLo.Text.ToString().Trim();
                string Xuat = drXuat.SelectedValue.ToString().Trim();

                string machno = tbMay.Text.ToString().Trim();
                string may = machno.Substring(6, 2);

                string MaVach = "";

                string NgayHieuLuc = "";
                string HanSuDung = "";
                string Loai = "";
                string makeo = "";
                string tenkeo = "";

                string bufff = NgayQuet + "_" + ThoiGianQuet;

                DateTime myDatetime = DateTime.ParseExact(bufff, "yyyyMMdd_HH:mm:ss", CultureInfo.InvariantCulture);

                string smonth = NgaySanXuat.Substring(4, 2).ToString().Trim();
                switch (smonth)
                {
                    case "10": smonth = "A"; break;
                    case "11": smonth = "B"; break;
                    case "12": smonth = "C"; break;
                    default: smonth = smonth.Substring(1, 1); break;
                }
                string spday = NgaySanXuat.Substring(2, 2) + smonth + NgaySanXuat.Substring(NgaySanXuat.Length - 2, 2);

                string sqlKEORE = "select mesid, pday from KEORE where subno='4' and factory ='V'  and mesid ='" + MaMes + "' and machno = '" + may + "'";
                System.Data.DataTable dtKEORE = Cnn.ExecuteQuery(ConnectionStringKeoRe, sqlKEORE);

                if (dtKEORE.Rows.Count > 0)
                {

                    //if (dtKEORE.Rows[0][1].ToString().Trim() != pday.Trim())
                    //{
                    //    string s = "Mes quá giờ không quét được";
                    //}

                    switch (TenKeo.Trim().Substring(6).ToUpper())
                    {
                        case "RM": makeo = "RD"; Loai = "3"; break;
                        case "1": makeo = "RB"; Loai = "2"; break;
                        case "9": makeo = "RD"; Loai = "3"; break;
                        case "2": makeo = "RC"; Loai = "2"; break;
                        case "3": makeo = "RC"; Loai = "2"; break;
                        case "4": makeo = "RC"; Loai = "2"; break;
                        case "5": makeo = "RC"; Loai = "2"; break;
                        case "RE": makeo = "RR"; Loai = "3"; break;
                        case "92": makeo = "RD"; Loai = "3"; break;// Sua theo barcode moi

                        case "1-EDGE": makeo = "RB"; Loai = "2"; break;
                        case "2-EDGE": makeo = "RC"; Loai = "2"; break;
                        case "3-EDGE": makeo = "RC"; Loai = "2"; break;
                        case "4-EDGE": makeo = "RC"; Loai = "2"; break;
                        case "5-EDGE": makeo = "RC"; Loai = "2"; break;
                        case "9-EDGE": makeo = "RD"; Loai = "3"; break;

                        case "1EDGE": tenkeo = TenKeo.Substring(0, 5) + "-1-EDGE"; makeo = "RB"; Loai = "2"; break;
                        case "2EDGE": tenkeo = TenKeo.Substring(0, 5) + "-2-EDGE"; makeo = "RC"; Loai = "2"; break;
                        case "3EDGE": tenkeo = TenKeo.Substring(0, 5) + "-3-EDGE"; makeo = "RC"; Loai = "2"; break;
                        case "4EDGE": tenkeo = TenKeo.Substring(0, 5) + "-4-EDGE"; makeo = "RC"; Loai = "2"; break;
                        case "5EDGE": tenkeo = TenKeo.Substring(0, 5) + "-5-EDGE"; makeo = "RC"; Loai = "2"; break;
                        case "9EDGE": tenkeo = TenKeo.Substring(0, 5) + "-9-EDGE"; makeo = "RD"; Loai = "3"; break;

                        case "1THU": tenkeo = TenKeo.Substring(0, 5) + "-1THU"; makeo = "RB"; Loai = "2"; break;
                        case "2THU": tenkeo = TenKeo.Substring(0, 5) + "-2THU"; makeo = "RC"; Loai = "2"; break;
                        case "3THU": tenkeo = TenKeo.Substring(0, 5) + "-3THU"; makeo = "RC"; Loai = "2"; break;
                        case "4THU": tenkeo = TenKeo.Substring(0, 5) + "-4THU"; makeo = "RC"; Loai = "2"; break;
                        case "5THU": tenkeo = TenKeo.Substring(0, 5) + "-5THU"; makeo = "RC"; Loai = "2"; break;
                        case "9THU": tenkeo = TenKeo.Substring(0, 5) + "-9THU"; makeo = "RD"; Loai = "3"; break;
                    }

                    //string ktkeo2 = "select * from [InTem].[dbo].[rubnod_Ptype] where rubno_7 ='" + TenKeo.Trim() + "'";
                    //System.Data.DataTable ktke = Cnn.ExecuteQuery(ConnectionStringKeoRe, ktkeo2);

                    //if (ktke.Rows.Count > 0)
                    //{
                    //    makeo = "RB";
                    //    Loai = "2";
                    //}

                    // Sua theo barcode moi
                    string ktptype = "SELECT [ptype],[rubno_7] FROM[InTem].[dbo].[rubnod_Ptype] WHERE rubno_7 = '" + TenKeo.Trim() + "'";
                    System.Data.DataTable KT_ptype = Cnn.ExecuteQuery(ConnectionStringKeoRe, ktptype);
                    if (KT_ptype.Rows.Count >= 2)
                    {
                        lbError.Visible = true;
                        lbError.Text = "Liên hệ phòng thí nghiệm (a Thuần) đóng 1 tiêu chuẩn";
                        ResetData();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                    }
                    else
                    {
                        if (KT_ptype.Rows.Count > 0)
                        {
                            makeo = "RB";
                            Loai = KT_ptype.Rows[0][0].ToString().Trim();
                        }
                        // else KT_ptype không có data thì không làm gì cả, giữ nguyên makeo, ptype theo xử lý đầu vào.
                    }

                    string sqleffdat = "select mesid from KEORE where subno='4' and factory ='V' and comp= 'N'  and mesid ='" + MaMes + "' and machno = '" + machno + "'";
                    dtKEORE = Cnn.ExecuteQuery(ConnectionStringKeoRe, sqleffdat);
                    if (dtKEORE.Rows.Count > 0)
                    {
                        string getKeo = "select expday from [erp].[dbo].[prdexp] where subno='4' and factory='V' and  ptype ='" + makeo + "' and rubno='" + TenKeo.Substring(0, 5) + "'";
                        System.Data.DataTable keo = Cnn.ExecuteQuery(ConnectionString, getKeo);

                        if (keo.Rows.Count == 0)
                        {

                            lbError.Visible = true;
                            lbError.Text = "Mã keo không được sử dụng.\n Liên hệ Duyên phòng chế tạo (755) !";
                            ResetData();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                        }
                        else
                        {
                            int day = int.Parse(keo.Rows[0][0].ToString().Trim());
                            HanSuDung = day.ToString().Trim();
                            NgayHieuLuc = DateTime.Now.AddDays(day).ToString("yyyyMMdd");
                        }
                    }
                    //string getKeo = "select expday from [erp].[dbo].[prdexp] where subno='4' and factory='V' and  ptype ='" + makeo + "' and rubno='" + TenKeo.Substring(0, 5) + "'";
                    //System.Data.DataTable keo = Cnn.ExecuteQuery(ConnectionString, getKeo);
                    //if (keo.Rows.Count == 0)
                    //{
                    //    lbError.Visible = true;
                    //    lbError.Text = "Không có Hạn Sử Dụng. Liên hệ phòng đảm bảo chất lượng, Sunkist(Bình Sung)!";
                    //    ResetData();
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                    //}
                    //else
                    //{
                    //    int day = int.Parse(keo.Rows[0][0].ToString().Trim());
                    //    HanSuDung = day.ToString().Trim();
                    //    NgayHieuLuc = myDatetime.AddDays(day).ToString("yyyyMMdd");
                    //}

                    string sqlBarcode = " select MAX(SUBSTRING(Barcode,8,3)) num from prdebe where subno = '4' and factory = 'V' and barcode like '" + makeo + "%' and prodat = '" + NgaySanXuat + "'";
                    System.Data.DataTable dtBar = Cnn.ExecuteQuery(ConnectionString, sqlBarcode);
                    //lấy barcode theo từng loại keo

                    if (dtBar.Rows.Count == 1 && dtBar.Rows[0][0].ToString().Trim() == "")
                        MaVach = makeo + spday + "001";
                    else
                        MaVach = makeo + spday + (int.Parse(dtBar.Rows[0][0].ToString()) + 1).ToString("000");
                }
                else
                {
                    lbError.Visible = true;
                    lbError.Text = "Mã MES đã bị đóng! Liên hệ IT mở!";
                    ResetData();
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                }

                string resultAA = KiemTraGioHanKeo(TrongLuong, tenkeo, machno, MaMes, TenKeo, MaVach);
                if (resultAA == "")
                {
                    if (tbKV.Text == "" || tbX.Text == "" || tbMay.Text == "" || tbSoLo.Text == "" || tbThoiGianQuet.Text == "" || tbNgayQuet.Text == "" || tbNguoiQuet.Text == "")
                    {
                        lbError.Visible = true;
                        lbError.Text = "Vui lòng nhập đủ dữ liệu!!!";
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                    }
                    else
                    {
                        string printer = drMayin.SelectedValue.ToString();
                        string maso = "";
                        string sqlKEORE2 = "select mesid from KEORE where factory='V' and mesid ='" + MaMes + "' and machno = '" + may + "'";
                        System.Data.DataTable dtKEORE2 = new System.Data.DataTable();
                        dtKEORE2 = Cnn.ExecuteQuery(ConnectionStringKeoRe, sqlKEORE2);
                        if (dtKEORE2.Rows.Count > 0)
                        {
                            string sqlBarcode = "SELECT * FROM prdebe where factory='V' and prodat='" + NgaySanXuat + "' and partno='" + TenKeo + "'" +
                                " and class='" + Ca + "' and barcode <= '" + MaVach + "' order by intime";
                            System.Data.DataTable dtBar = Cnn.ExecuteQuery(ConnectionString, sqlBarcode);
                            //lấy barcode theo từng loại keo
                            int a = 1;
                            int b = 2;
                            if (dtBar.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtBar.Rows.Count; i++)
                                {
                                    if (i > 0)
                                    {
                                        a += 2;
                                        b = a + 1;
                                    }
                                }

                                string sqlweight = "SELECT weight FROM KEORE where factory='V' and mesid ='" + MaMes + "' and machno='" + may + "'"; //lấy Barcode từ trong sql
                                System.Data.DataTable dtweight = Cnn.ExecuteQuery(ConnectionStringKeoRe, sqlweight);
                                int weight = Convert.ToInt32(dtweight.Rows[0][0].ToString());
                                try
                                {
                                    if (weight == 1)
                                    {
                                        maso = a.ToString();
                                    }
                                    else
                                    {
                                        maso = a.ToString() + "-" + b.ToString();
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        if (drMayIn1.Text == "Vui lòng kết nối máy in!!!")
                        {
                            lbError.Visible = true;
                            lbError.Text = "Vui lòng chọn máy in !!!";
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);

                        }
                        else
                        {
                            PrintExcel(may, Loai, Ca, pallet, HanSuDung, NgayQuet, ThoiGianQuet, SoLo, TrongLuong, NgayHieuLuc, TenKeo, maso, MaVach, printer);
                        }
                    }
                }
                else
                {
                    lbError.Visible = true;
                    lbError.Text = resultAA;
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                }
            }
            catch (Exception ex)
            {
                lbError.Visible = true;
                lbError.Text = "Mã mesid không được để trống, xin kiểm tra lại!!!";
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
            }
        }

        private string KiemTraPallet(string pallet, string usrno, string mesid)
        {
            string result = string.Empty;
            if (pallet.Length < 6)
            {
                result = "Pallet không đủ ký tự, nhập lại!!!";
            }
            else if (pallet.Substring(0, 2) != "VB" && pallet.Substring(0, 2) != "EB" && pallet.Substring(0, 2) != "VC" && pallet.Substring(0, 2) != "VD" && pallet.Substring(0, 2) != "VE")
            {
                result = "Pallet không hợp lệ, nhập lại!!!";
            }
            else
            {
                //result = Test.ThemPallet(pallet, usrno);
                string SelPallet = "SELECT PALLET_NO FROM [InTem].[dbo].[PalletBB] WHERE PALLET_NO='" + pallet + "'";
                System.Data.DataTable dtPallet = Cnn.ExecuteQuery(ConnectionStringKeoRe, SelPallet);

                if (dtPallet.Rows.Count == 0)
                {
                    string sDat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    string sqlIns = "INSERT INTO [InTem].[dbo].[PalletBB] VALUES('" + pallet + "', '" + sDat + "', '" + usrno + "','1', 'Y') ";

                    bool bQuery = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, sqlIns);
                    if (!bQuery)
                    {
                        result = "Lỗi cập nhật Pallet\nPalletBB";
                    }
                }

                //-------------------namnho--20200917-----------------------//
                string ktXuatPallet = "select top 1 active from [dbo].[prdebe] where subno ='4' and factory='V' and pallet_no='" + pallet.Trim() + "' order by indat desc, intime desc";
                System.Data.DataTable kttpallet = Cnn.ExecuteQuery(ConnectionString, ktXuatPallet);
                if (kttpallet.Rows.Count > 0)
                {
                    if (kttpallet.Rows[0][0].ToString() == "N")
                    {
                        result = "Pallet này chưa xuất, không được trùng pallet";
                    }
                }
                string ktTrungPallet = "select * from [dbo].[prdebe] where subno ='4' and factory='V' and mesid='" + mesid.Trim() + "' and pallet_no='" + pallet.Trim() + "' ";
                System.Data.DataTable kttpallet1 = Cnn.ExecuteQuery(ConnectionString, ktTrungPallet);
                if (kttpallet1.Rows.Count > 0)
                {
                    result = "1 Pallet chỉ được quét 1 lần cho 1 mã MES";
                }
            }
            return result;
        }

        protected void tbMaPalet_TextChanged(object sender, EventArgs e)
        {
            string pallet = tbMaPalet.Text.Trim();
            string mesid = drMes.SelectedValue.Trim();
            string usrno = tbNguoiQuet.Text.Trim();
            string result = KiemTraPallet(pallet, usrno, mesid);
            lbError.Visible = true;
            lbError.Text = result;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
        }

        protected void tbMaPalet1_TextChanged(object sender, EventArgs e)
        {
            string pallet = tbMaPalet1.Text.Trim();
            string mesid = tbMes1.Text.Trim();
            string usrno = tbNguoiQuet1.Text.Trim();
            string result = KiemTraPallet(pallet, usrno, mesid);
            lbError1.Visible = true;
            lbError1.Text = result;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvSua();", true);
        }



        private void PrintExcel(string Machno, string loaikeo, string ca, string pallet, string daylimt, string indat, string intime, string slipno, string soluong, string effdat, string partno, string maso, string barcode, string printer)
        {
            //Mo Excel
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;

            try
            {
                string tenloaithe = "";
                string kichthuoc = "";
                string KVS = "";
                string Ca = "";
                string tentheloaikeoTQ = "";
                if (loaikeo == "RC")
                {
                    tenloaithe = "Thẻ biểu thị keo tinh luyện";
                    tentheloaikeoTQ = "精煉膠標示卡";
                    KVS = "KVS3J1C001.9  Rev.5";
                    kichthuoc = "(180mm×130mm×0.08mm)";
                }

                if (loaikeo == "RD" || loaikeo == "RR")
                {
                    tenloaithe = "Thẻ biểu thị keo xúc tiến";
                    tentheloaikeoTQ = "加促膠標示卡";
                    KVS = "KVS3J1C001.8  Rev.5";
                    kichthuoc = "(180mm×130mm×0.08mm)";
                }

                if (loaikeo == "RB")
                {
                    tenloaithe = "Thẻ biểu thị keo cán luyên";
                    tentheloaikeoTQ = "混煉膠標示卡";
                    KVS = "KVS3J1C001.7  Rev.5";
                    kichthuoc = "(180mm×130mm×0.08mm)";
                }


                if (ca == "1")
                {
                    Ca = "早 (Sáng)";
                }
                if (ca == "2")
                {
                    Ca = "夜 (Đêm)";
                }



                //if (loaikeo == "RC")
                //{
                //    worksheet.Range["C4"].Value = "精煉";
                //    worksheet.Range["C5"].Value = "Tinh luyện";
                //}

                Range range = worksheet.get_Range("B2", "P21");
                range.BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic, Color.Black);
                range.Style.Font.Name = "Arial";
                range.Style.Font.Size = 13;
                range.Style.Font.Color = Color.Black;
                range.Interior.Color = ColorTranslator.ToOle(Color.White);
                range.NumberFormat = "@";

                worksheet.Range["C2"].Value = " 建大橡膠（越南）有限公司";
                worksheet.Range["C3"].Value = "Công ty Cao su Kenda(Việt Nam)";
                worksheet.Range["E4"].Value = tentheloaikeoTQ;
                worksheet.Range["C5"].Value = "Machine BB37" + Machno;
                worksheet.Range["E5"].Value = tenloaithe;
                worksheet.Range["N5"].Value = pallet;
                worksheet.Range["C6"].Value = "日限  Thời hạn sử dụng:  " + daylimt + "日" + daylimt + "Ngày";
                worksheet.Range["C7"].Value = "禁止雨淋，油污，置地，及粉水未乾";
                worksheet.Range["C8"].Value = "Cấm ướt mưa, dính dầu, để lên đất, bột nước chưa khô";
                worksheet.Range["C9"].Value = "生產日期 Ngày Tháng Sản Xuất";
                worksheet.Range["C10"].Value = indat + " " + intime;
                worksheet.Range["C11"].Value = "批號";
                worksheet.Range["C12"].Value = "Số lô";
                worksheet.Range["E11"].Value = slipno;
                worksheet.Range["I11"].Value = "重量";
                worksheet.Range["I12"].Value = "Trọng lượng";
                worksheet.Range["L11"].WrapText = true;
                worksheet.Range["L11"].Value = soluong + "kg";
                worksheet.Range["C13"].Value = "有效日";
                worksheet.Range["C14"].Value = "Ngày hiệu lực";
                worksheet.Range["E13"].Value = effdat + " " + intime;
                worksheet.Range["I13"].Value = "班別";
                worksheet.Range["I14"].Value = "Ca";
                worksheet.Range["L13"].WrapText = true;
                worksheet.Range["L13"].Value = Ca;
                worksheet.Range["C15"].Value = "規格";
                worksheet.Range["C16"].Value = "Quy Cách";
                worksheet.Range["D15"].Value = partno;
                worksheet.Range["G15"].Value = "編號順序";
                worksheet.Range["G17"].Value = "Thứ tự mã số";
                worksheet.Range["I15"].WrapText = true;
                worksheet.Range["I15"].Value = maso;
                worksheet.Range["K15"].Value = "判 定";
                worksheet.Range["K17"].Value = "Phán định";
                worksheet.Range["M15"].WrapText = true;
                worksheet.Range["M15"].Value = "";
                worksheet.Range["C17"].Value = "委託";
                worksheet.Range["C18"].Value = "Ủy thác";
                worksheet.Range["C20"].Value = KVS;
                worksheet.Range["K20"].Value = "*" + barcode + "*";
                worksheet.Range["C21"].Value = kichthuoc;
                worksheet.Range["K21"].Value = "*" + barcode + "*";

                //Dinh dang Excel

                worksheet.Range["E4"].Font.Bold = true;
                worksheet.Range["E5"].Font.Bold = true;
                worksheet.Range["K20"].Font.Name = "Code39AzaleaWide2";
                worksheet.Range["K20"].Font.Size = 28;


                worksheet.get_Range("C2:O2").MergeCells = true;
                worksheet.get_Range("C3:O3").MergeCells = true;
                worksheet.get_Range("C5:D5").MergeCells = true;
                worksheet.get_Range("E4:L4").MergeCells = true;
                worksheet.get_Range("E5:L5").MergeCells = true;
                worksheet.get_Range("N5:O5").MergeCells = true;
                worksheet.get_Range("C6:O6").MergeCells = true;
                worksheet.get_Range("C7:O7").MergeCells = true;
                worksheet.get_Range("C8:O8").MergeCells = true;
                worksheet.get_Range("C9:O9").MergeCells = true;
                worksheet.get_Range("C10:O10").MergeCells = true;
                worksheet.get_Range("C11:D11").MergeCells = true;
                worksheet.get_Range("C12:D12").MergeCells = true;
                worksheet.get_Range("E11:H12").MergeCells = true;
                worksheet.get_Range("I11:K11").MergeCells = true;
                worksheet.get_Range("I12:K12").MergeCells = true;
                worksheet.get_Range("L11:O12").MergeCells = true;
                worksheet.get_Range("C13:D13").MergeCells = true;
                worksheet.get_Range("C14:D14").MergeCells = true;
                worksheet.get_Range("E13:H14").MergeCells = true;
                worksheet.get_Range("I13:K13").MergeCells = true;
                worksheet.get_Range("I14:K14").MergeCells = true;
                worksheet.get_Range("L13:O14").MergeCells = true;
                worksheet.get_Range("D15:F16").MergeCells = true;
                worksheet.get_Range("G15:H16").MergeCells = true;
                worksheet.get_Range("G17:H18").MergeCells = true;
                worksheet.get_Range("I15:J18").MergeCells = true;
                worksheet.get_Range("K15:L16").MergeCells = true;
                worksheet.get_Range("K17:L18").MergeCells = true;
                worksheet.get_Range("M15:O18").MergeCells = true;
                worksheet.get_Range("D17:F18").MergeCells = true;
                worksheet.get_Range("C20:G20").MergeCells = true;
                worksheet.get_Range("K20:O20").MergeCells = true;
                worksheet.get_Range("C21:G21").MergeCells = true;
                worksheet.get_Range("K21:O21").MergeCells = true;

                worksheet.Range["A1"].ColumnWidth = 1;
                worksheet.Range["B1"].ColumnWidth = 1;
                worksheet.Range["C1"].ColumnWidth = 10;
                worksheet.Range["D1"].ColumnWidth = 3;
                worksheet.Range["E1"].ColumnWidth = 3;
                worksheet.Range["F1"].ColumnWidth = 3;
                worksheet.Range["G1"].ColumnWidth = 5;
                worksheet.Range["H1"].ColumnWidth = 6;
                worksheet.Range["I1"].ColumnWidth = 5;
                worksheet.Range["J1"].ColumnWidth = 5;
                worksheet.Range["K1"].ColumnWidth = 2;
                worksheet.Range["L1"].ColumnWidth = 7;
                worksheet.Range["M1"].ColumnWidth = 5;
                worksheet.Range["N1"].ColumnWidth = 5;
                worksheet.Range["O1"].ColumnWidth = 2;
                worksheet.Range["P1"].ColumnWidth = 1;
                worksheet.Range["Q1"].ColumnWidth = 1;

                worksheet.Range["A1"].RowHeight = 5;
                worksheet.Range["A2"].RowHeight = 20.5;
                worksheet.Range["A7"].RowHeight = 22.5;
                worksheet.Range["A8"].RowHeight = 22.5;
                worksheet.Range["A19"].RowHeight = 5;
                worksheet.Range["A20"].RowHeight = 30;
                worksheet.Range["A22"].RowHeight = 5.5;

                worksheet.get_Range("A1:P22").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.get_Range("A1:P22").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                worksheet.get_Range("C6:O18").Borders.LineStyle = XlLineStyle.xlContinuous;

                worksheet.get_Range("C6:O18").BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlThin);
                worksheet.Range["C11"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["C13"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["C15"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["C17"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["D11"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["D13"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["I11"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["I13"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["J11"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["J13"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["K11"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["K13"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["C7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["D7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["E7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["F7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["G7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["H7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["I7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["J7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["K7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["L7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["M7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["N7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["O7"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["G15"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["H15"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["G16"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
                worksheet.Range["H16"].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;

                worksheet.Range["C7"].VerticalAlignment = XlVAlign.xlVAlignBottom;
                worksheet.Range["C8"].VerticalAlignment = XlVAlign.xlVAlignTop;
                worksheet.Range["G15"].VerticalAlignment = XlVAlign.xlVAlignBottom;
                worksheet.Range["G17"].VerticalAlignment = XlVAlign.xlVAlignTop;

                worksheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                worksheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4;
                worksheet.PageSetup.TopMargin = 0.5;
                worksheet.PageSetup.LeftMargin = 0.5;
                worksheet.PageSetup.RightMargin = 0.5;
                worksheet.PageSetup.BottomMargin = 0.5;
                worksheet.PageSetup.HeaderMargin = 0;
                worksheet.PageSetup.FooterMargin = 0;
                worksheet.PageSetup.CenterHorizontally = true;
                worksheet.PageSetup.CenterVertically = true;

                //---------------Intem--------------
                worksheet.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printer, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                ThongBao("Xuất Excel thành công !!!");
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                ThongBao("Lỗi file Excel");
            }
        }
    }
}