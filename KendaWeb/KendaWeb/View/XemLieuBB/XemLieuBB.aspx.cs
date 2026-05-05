using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KendaWeb.Model;
using Microsoft.Office.Interop.Excel;
using Spire.Xls;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.IO;
using ClosedXML.Excel;
using DataTable = System.Data.DataTable;
using System.Security.RightsManagement;

namespace KendaWeb
{
    public partial class XemLieuBB : System.Web.UI.Page
    {
        string ConnectionStringHome = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=erp;User ID=kendakv2;Password=kenda123;";
        string planId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/View/Loggin.aspx");
            }

            string a = Session["ChucVu"].ToString().Trim().Substring(0, 3);
            if (Session["UserName"].ToString().Trim() != "022191" && a != "B22" && a!="P87")
            {
                ListItem itemToRemove = drMay.Items.FindByValue("tatca");
                if (itemToRemove != null)
                {
                    drMay.Items.Remove(itemToRemove);
                }

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
       
        private void xemlieutatca()
        {
            try
            {
                string makeo = txtTimkiem.Text.ToString().Trim();
                string fromDay = txtFromDay.Text.ToString().Replace("-", "");
                string toDay = txtToday.Text.ToString().Replace("-", "");
                if (fromDay == "" || toDay == "")
                {
                    ThongBao("Vui lòng nhập ngày!!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                System.Data.DataTable result = new System.Data.DataTable();
                result.Columns.Add("Mã mesid");
                result.Columns.Add("Số máy");
                result.Columns.Add("Tên keo");
                result.Columns.Add("Số lô");
                result.Columns.Add("Số mẻ điều động");
                result.Columns.Add("Số mẻ hoàn thành");
                result.Columns.Add("Số ký tiêu chuẩn");
                result.Columns.Add("Số ký đã quét tem");
                result.Columns.Add("Số ký hoàn thành");
                result.Columns.Add("Số ký chênh lệch");
                result.Columns.Add("Xem liệu");

                string[] cacmay = { "01", "02", "03", "04", "05","06", "07" };

                string thongbaoID = "";
                foreach (var May in cacmay)
                {
                   
                    string ConnectionString = ChonMay(May);
                    if (ConnectionString == "")
                    {

                        continue;
                    }
                    string getData = "select k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno,cast(SUM(p.weight)as int) as wgt from[erp].[dbo].[prdebe] as p " +
                        "INNER JOIN [InTem].[dbo].[KEORE] as k " +
                        "ON p.mesid = k.mesid" +
                        " where (k.pday between  '" + fromDay + "' and '" + toDay + "') and k.idGrouplot != '' and p.mesid like'V%' and p.factory = 'V' and p.machno Like'%" + May + "'" +
                        "group by k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno";

  

                    System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, getData);
                    if (dt.Rows.Count == 0)
                    {
                        continue;
                    }
                    string grlotid = "";
                    foreach (DataRow item in dt.Rows)
                    {
                        grlotid += "'" + item["idGrouplot"].ToString().Trim() + "',";
                    }
                    string GetTc = "SELECT a.id, a.RecipeCode, a.SetNumber, a.FinishNum,cast(SUM(b.set_weight) as int) as SKTC,cast(a.FinishNum*SUM(b.set_weight) as int) as Swgt" +
                        " FROM [mfns].[dbo].[Ppt_GroupLot] a,[mfns].[dbo].[pmt_weigh] b where a.RecipeCode = b.father_code and id in (" + grlotid.Remove(grlotid.Length - 1) + ") " +
                        " group by id,RecipeCode,FinishNum,SetNumber";
                    System.Data.DataTable dtTc = Cnn.ExecuteQuery(ConnectionString, GetTc);
                   
                    if (dtTc.Rows.Count == 0)
                    {
                        
                        thongbaoID +=  May + ",";
                        ThongBao("Máy " + thongbaoID + " không tìm thấy idgrouplot vui lòng liên hệ IT !!!");
                    }
                    foreach (DataRow item in dt.Rows)
                    {
                        foreach (DataRow item1 in dtTc.Rows)
                        {
                            if (item["idGrouplot"].ToString().Trim() == item1["id"].ToString().Trim())
                            {
                                result.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["machno"].ToString().Trim().Substring(6,2), item["partno"].ToString().Trim(),item["slipno"].ToString().Trim(),
                                    item1["SetNumber"].ToString().Trim(),item1["FinishNum"].ToString().Trim(),item1["SKTC"].ToString().Trim(), item["wgt"].ToString().Trim(),item1["Swgt"].ToString().Trim(),
                                    int.Parse(item1["Swgt"].ToString().Trim())-int.Parse(item["wgt"].ToString().Trim()),""});
                            }
                        }
                    }

                }

               
                txtTimkiem.Text = "";
                TempData.tableMES = result;

                gvKQ.DataSource = result;
                gvKQ.DataBind();

            }
            catch (Exception ex)
            {
                string a = ex.ToString();
                return;
            }
               
        }
        private void xemlieutatcatimkiem()
        {
            try
            {
                string makeo = txtTimkiem.Text.ToString().Trim();
                string fromDay = txtFromDay.Text.ToString().Replace("-", "");
                string toDay = txtToday.Text.ToString().Replace("-", "");
                if (fromDay == "" || toDay == "")
                {
                    ThongBao("Vui lòng nhập ngày!!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                System.Data.DataTable result = new System.Data.DataTable();
                result.Columns.Add("Mã mesid");
                result.Columns.Add("Số máy");
                result.Columns.Add("Tên keo");
                result.Columns.Add("Số lô");
                result.Columns.Add("Số mẻ điều động");
                result.Columns.Add("Số mẻ hoàn thành");
                result.Columns.Add("Số ký tiêu chuẩn");
                result.Columns.Add("Số ký đã quét tem");
                result.Columns.Add("Số ký hoàn thành");
                result.Columns.Add("Số ký chênh lệch");
                result.Columns.Add("Xem liệu");

                string[] cacmay = { "01", "02", "03", "04", "05", "06", "07" };
                string thongbaoID = "";
                foreach (var May in cacmay)
                {

                    string ConnectionString = ChonMay(May);
                    if (ConnectionString == "")
                    {

                        continue;
                    }
                    string getData = "select k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno,cast(SUM(p.weight)as int) as wgt from[erp].[dbo].[prdebe] as p " +
                        "INNER JOIN [InTem].[dbo].[KEORE] as k " +
                        "ON p.mesid = k.mesid" +
                        " where (k.pday between  '" + fromDay + "' and '" + toDay + "') and k.idGrouplot != '' and p.mesid like'V%' and p.factory = 'V' and p.machno Like'%" + May + " 'and p.partno like '%" + makeo + "%'" +
                        "group by k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno";

                    //string getData = "select k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno,cast(SUM(p.weight)as int) as wgt from[erp].[dbo].[prdebe] as p " +
                    //  "INNER JOIN[198.1.9.186].[InTem].[dbo].[KEORE] as k " +
                    //  "ON p.mesid = k.mesid" +
                    //  " where (k.pday between  '" + fromDay + "' and '" + toDay + "') and k.idGrouplot != '' and p.mesid like'V%' and p.factory = 'V' and p.machno Like'%" + May + " 'and p.partno like '%" + makeo + "%'" +
                    //  "group by k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno";


                    System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, getData);
                    if (dt.Rows.Count == 0)
                    {
                        continue;
                    }
                    string grlotid = "";
                    foreach (DataRow item in dt.Rows)
                    {
                        grlotid += "'" + item["idGrouplot"].ToString().Trim() + "',";
                    }
                    string GetTc = "SELECT a.id, a.RecipeCode, a.SetNumber, a.FinishNum,cast(SUM(b.set_weight) as int) as SKTC,cast(a.FinishNum*SUM(b.set_weight) as int) as Swgt" +
                        " FROM [mfns].[dbo].[Ppt_GroupLot] a,[mfns].[dbo].[pmt_weigh] b where a.RecipeCode = b.father_code and id in (" + grlotid.Remove(grlotid.Length - 1) + ") " +
                        " group by id,RecipeCode,FinishNum,SetNumber";
                    System.Data.DataTable dtTc = Cnn.ExecuteQuery(ConnectionString, GetTc);
                    if (dtTc.Rows.Count == 0)
                    {

                        thongbaoID += May + ",";
                        ThongBao("Máy " + thongbaoID + " không tìm thấy idgrouplot vui lòng liên hệ IT !!!");
                    }
                    foreach (DataRow item in dt.Rows)
                    {
                        foreach (DataRow item1 in dtTc.Rows)
                        {
                            if (item["idGrouplot"].ToString().Trim() == item1["id"].ToString().Trim())
                            {
                                result.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["machno"].ToString().Trim().Substring(6,2), item["partno"].ToString().Trim(), item["slipno"].ToString().Trim(),
                                    item1["SetNumber"].ToString().Trim(),item1["FinishNum"].ToString().Trim(),item1["SKTC"].ToString().Trim(), item["wgt"].ToString().Trim(),item1["Swgt"].ToString().Trim(),
                                    int.Parse(item1["Swgt"].ToString().Trim())-int.Parse(item["wgt"].ToString().Trim()),""});
                            }
                        }
                    }

                }


                
                TempData.tableMES = result;

                gvKQ.DataSource = result;
                gvKQ.DataBind();

            }
            catch (Exception ex)
            {
                string a = ex.ToString();
                return;
            }

        }


        private void LoadData()
        {

            try
            {
                string makeo = txtTimkiem.Text.ToString().Trim();
                string fromDay = txtFromDay.Text.ToString().Replace("-", "");
                string toDay = txtToday.Text.ToString().Replace("-", "");
                if (fromDay == "" || toDay == "")
                {
                    ThongBao("Vui lòng nhập ngày!!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                System.Data.DataTable result = new System.Data.DataTable();
                result.Columns.Add("Mã mesid");
                result.Columns.Add("Số máy");
                result.Columns.Add("Tên keo");
                result.Columns.Add("Số lô");
                result.Columns.Add("Số mẻ điều động");
                result.Columns.Add("Số mẻ hoàn thành");
                result.Columns.Add("Số ký tiêu chuẩn");
                result.Columns.Add("Số ký đã quét tem");
                result.Columns.Add("Số ký hoàn thành");
                result.Columns.Add("Số ký chênh lệch");
                result.Columns.Add("Xem liệu");

                string May = drMay.SelectedValue;
                if (May == "")
                {
                    ThongBao("Vui lòng chọn máy!!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                string ConnectionString = ChonMay(May);
                if (ConnectionString == "")
                {
                    ThongBao("Máy đang tắt, vui lòng mở máy và thử lại!!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                string getData = "select k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno,cast(SUM(p.weight)as int) as wgt from[erp].[dbo].[prdebe] as p " +
                    "INNER JOIN [198.1.9.186].[InTem].[dbo].[KEORE] as k " +
                    "ON p.mesid = k.mesid" +
                    " where (k.pday between  '" + fromDay + "' and '" + toDay + "') and k.idGrouplot != ''  and p.mesid like'V%' and p.factory = 'V' and p.machno Like'%" + May + "'" +
                    "group by k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno";
                //string getData = "select k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno,cast(SUM(p.weight)as int) as wgt from[erp].[dbo].[prdebe] as p " +
                //   "INNER JOIN[198.1.9.186].[InTem].[dbo].[KEORE] as k " +
                //   "ON p.mesid = k.mesid" +
                //   " where (p.prodat between  '" + fromDay + "' and '" + toDay + "') and p.mesid like'V%' and p.factory = 'V' and p.machno Like'%" + May + "'" +
                //   "group by k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, getData);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu máy " + May);
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                string grlotid = "";
                foreach (DataRow item in dt.Rows)
                {
                    grlotid += "'" + item["idGrouplot"].ToString().Trim() + "',";
                }
                string GetTc = "SELECT a.id, a.RecipeCode, a.SetNumber, a.FinishNum,cast(SUM(b.set_weight) as int) as SKTC,cast(a.FinishNum*SUM(b.set_weight) as int) as Swgt" +
                    " FROM [mfns].[dbo].[Ppt_GroupLot] a,[mfns].[dbo].[pmt_weigh] b where a.RecipeCode = b.father_code and id in (" + grlotid.Remove(grlotid.Length - 1) + ") " +
                    " group by id,RecipeCode,FinishNum,SetNumber";
                System.Data.DataTable dtTc = Cnn.ExecuteQuery(ConnectionString, GetTc);
                if (dtTc.Rows.Count == 0)
                {
                    ThongBao("idGrouplot không tìm thấy, vui lòng liên hệ IT !!!");
                }
                foreach (DataRow item in dt.Rows)
                {
                    foreach (DataRow item1 in dtTc.Rows)
                    {
                        if (item["idGrouplot"].ToString().Trim() == item1["id"].ToString().Trim())
                        {
                            result.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["machno"].ToString().Trim().Substring(6,2), item["partno"].ToString().Trim(),item["slipno"].ToString().Trim(),
                                    item1["SetNumber"].ToString().Trim(),item1["FinishNum"].ToString().Trim(),item1["SKTC"].ToString().Trim(), item["wgt"].ToString().Trim(),item1["Swgt"].ToString().Trim(),
                                    int.Parse(item1["Swgt"].ToString().Trim())-int.Parse(item["wgt"].ToString().Trim()),""});
                        }
                    }
                }
                txtTimkiem.Text = "";
                TempData.tableMES = result;

                gvKQ.DataSource = result;
                gvKQ.DataBind();

            }
            catch (Exception ex)
            {
                string a = ex.ToString();
                return;
            }
        }

        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }

        protected void btn_TruyLieu_Click(object sender, EventArgs e)
        {
           
            txtTimkiem.Text = "";
            if (txtFromDay.Text.Trim().ToString() == "" || txtToday.Text.Trim().ToString() == "")
            {
                ThongBao("Vui lòng nhập ngày!!!");
                gvKQ.DataSource = null;
                gvKQ.DataBind();
                return;
            }

            if (drMay.SelectedValue == "tatca")
            {
                string dateString1 = txtFromDay.Text.Trim().ToString();
                string dateString2 = txtToday.Text.Trim().ToString();

                DateTime date1 = DateTime.Parse(dateString1);
                DateTime date2 = DateTime.Parse(dateString2);

                TimeSpan difference = date2 - date1;

                int daysDifference = difference.Days;
                if (daysDifference > 2)
                {
                    ThongBao("Không thể xem quá 2 ngày");
                    return;
                    
                }
                else
                {
                    xemlieutatca();
                }

               

            } 
            else
            {
                LoadData();
            }

        }

        protected void gvKQ_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string id = Dulieu[0].Trim();



            string May = Dulieu[1].Trim();
            switch (e.CommandName)
            {
                case ("btnIn"):
                    string ConnectionString = ChonMay(May);
                    string sqlIdGroup = "select idGrouplot,indat,recipe_name from KEORE where factory='V' and mesid ='" + id + "' and machno = '" + May + "'";
                    System.Data.DataTable dtKEORE = Cnn.ExecuteQuery("Data Source=198.1.9.186;Initial Catalog=InTem;User ID=kendakv2;Password=kenda123", sqlIdGroup);
                    string IdGroup = dtKEORE.Rows[0]["idGrouplot"].ToString().Trim();


                    string recipename = dtKEORE.Rows[0]["recipe_name"].ToString().Trim();

                    string newstring = recipename.Substring(recipename.Length - 1, 1);


                    string GetPlanId = "SELECT Plan_ID FROM [mfns].[dbo].[Ppt_GroupLot] where Id = '" + IdGroup + "'";
                    System.Data.DataTable dtPlanId = Cnn.ExecuteQuery(ConnectionString, GetPlanId);

                    planId = dtPlanId.Rows[0]["Plan_ID"].ToString().Trim();
                    System.Data.DataTable dtIn = new DataTable();
                    if (May == "test")
                    {
                        string GetIn = "   WITH c AS(" +
                         " SELECT b.SaveTime as test,b.Recipe_Name,b.Set_Num,b.Serial_Num,b.Mater_Code,b.Mater_Name,b.Mater_Barcode,  " +
                         "CASE WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'V' THEN '20' + SUBSTRING(Mater_Barcode, 4, 6)     " +
                         "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'A' " +
                         "THEN cast( (SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem]  WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+ cast( (SELECT Equip_ID  FROM [mfnsShareDB].[dbo].[IF_Datatem]  WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-10'+ SUBSTRING(Mater_Barcode, 6, 2)  " +
                         "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'B'" +
                         "THEN cast((SELECT Shift  FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+cast((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-11'+ SUBSTRING(Mater_Barcode, 6, 2) " +
                         "  WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'C'  " +
                         " THEN cast((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+cast( (SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-12'+ SUBSTRING(Mater_Barcode, 6, 2)   " +
                         "  WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' THEN cast( (SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+cast((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem]  WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-0' + SUBSTRING(Mater_Barcode, 5, 3)    " +
                         " ELSE '20' + SUBSTRING(Mater_Barcode, 8, 6)  END AS batchno ,b.Equip_ID,a.real_weight,a.error_allow,a.weigh_time as SaveTime " +
                         " FROM  [mfns].[dbo].[ppt_weigh] a full join [mfns].[dbo].[Ppt_BarCodeRep] b  on  a.barcode =b.Barcode   AND b.mater_code = a.mater_code " +
                         " where  b.Plan_ID = '" + planId + "'  AND substring(b.mater_code, 1, 3) <> '680' and a.barcode is not null )" +

                         " select distinct * from c where batchno is not null and Mater_Code not like '60%' or (Mater_Code like '60%' and Mater_Code=Mater_Name) order by SaveTime";


                        dtIn = Cnn.ExecuteQuery(ConnectionString, GetIn);

                        DataTable dtnameThan = Cnn.ExecuteQuery(ConnectionString, "SELECT mater_code,mater_name  FROM [mfns].[dbo].[pmt_material]  where [mater_code] like '60%'");

                        foreach (DataRow row2 in dtIn.Rows)
                        {
                            // Lấy giá trị của cột "mater_code" từ hàng hiện tại
                            string materCode2 = row2["mater_code"].ToString();

                            // Tìm hàng trong DataTable thứ nhất có cùng giá trị "mater_code"
                            DataRow[] matchingRows = dtnameThan.Select($"mater_code = '{materCode2}'");

                            // Kiểm tra xem có hàng phù hợp không
                            if (matchingRows.Length > 0)
                            {
                                // Lấy giá trị của cột "mater_name" từ hàng phù hợp đầu tiên
                                string materName1 = matchingRows[0]["mater_name"].ToString();

                                // Cập nhật giá trị của cột "mater_name" trong hàng hiện tại của DataTable thứ hai
                                row2["mater_name"] = materName1;
                            }
                        }





                        string s_pptweight_code = " SELECT TOP 1 [Barcode] FROM[mfns].[dbo].[Ppt_BarCodeRep] where Plan_ID = '" + planId + "'";
                            DataTable dt_weight_code = Cnn.ExecuteQuery(ConnectionString, s_pptweight_code);

                            if (dt_weight_code.Rows.Count != 0)
                            {
                                //    string ppt_barcode = dt_weight_code.Rows[0][0].ToString().Trim().Substring(0, dt_weight_code.Rows[0][0].ToString().Trim().Length - 3);
                                string check_oil_coal = "  SELECT a.[barcode],a.[mater_code], a.[equip_code], a.[set_weight], CONVERT(nvarchar(20),a.[weigh_time],120) as weigh_time ,a.[error_allow], a.[weigh_type], b.mater_name " +
                                    "FROM[mfns].[dbo].[ppt_weigh] a, [mfns].[dbo].[pmt_material] b where barcode like '" + planId + "%' and weigh_type='油料' and a.mater_code = b.mater_code order by weigh_time asc ";
                                DataTable dt_check_old_coal = Cnn.ExecuteQuery(ConnectionString, check_oil_coal);
                                if (dt_check_old_coal.Rows.Count != 0)
                                {
                                    string coal_barcode = "";
                                    string s_fromday = dt_check_old_coal.Rows[0]["weigh_time"].ToString().Trim();
                                    DateTime dat_point = DateTime.Parse(s_fromday);
                                    DateTime dat_check = DateTime.Parse(s_fromday);
                                    TimeSpan ts_check = new TimeSpan(06, 30, 00);
                                    string s_from = "";

                                    dat_check = dat_check.Date + ts_check;
                                    if (dat_point <= dat_check)
                                    {
                                        dat_check = dat_check.AddDays(-1);
                                    }
                                    s_from = dat_check.ToString("yyyy-MM-dd HH:mm:ss");
                                    string coal_code = "";
                                    for (int _icount = 0; _icount < dt_check_old_coal.Rows.Count; _icount++)
                                    {
                                        if (dt_check_old_coal.Rows[_icount]["weigh_type"].ToString().Trim() == "油料")
                                        {
                                            coal_code = dt_check_old_coal.Rows[_icount]["mater_code"].ToString().Trim();
                                            break;
                                        }
                                    }
                                    string s_coal_barcode = "  SELECT top 1 [Mater_Barcode],[SaveTime] FROM [mfns].[dbo].[Ppt_Oil] where  SaveTime <= '" + s_fromday + "' and Mater_Type = '0' and [Mater_Code] ='" + coal_code + "' order by SaveTime desc";



                                    DataTable dt_coal_barcode = Cnn.ExecuteQuery(ConnectionString, s_coal_barcode);
                                    if (dt_coal_barcode.Rows.Count != 0)
                                    {
                                        coal_barcode = dt_coal_barcode.Rows[0][0].ToString().Trim();
                                    }

                                    DataRow dr;
                                    int i_max = dt_check_old_coal.Rows.Count;//int.Parse(dtIn.Rows[0]["Set_Num"].ToString().Trim());
                                    for (int i_count = 0; i_count < i_max; i_count++)
                                    {
                                        dr = dtIn.NewRow();
                                        dr[0] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim(); // time
                                        dr[1] = dtIn.Rows[0][1].ToString().Trim(); // recipe
                                        dr[2] = dtIn.Rows[0][2].ToString().Trim(); // setnum
                                                                                   //dr[3] = dtIn.Rows[i_count][3].ToString().Trim(); // serial num
                                        dr[3] = int.Parse(dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim().Substring(dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim().Length - 2, 2));// serial num
                                        dr[4] = dt_check_old_coal.Rows[i_count]["mater_code"].ToString(); // mater code
                                        dr[5] = dt_check_old_coal.Rows[i_count]["mater_name"].ToString().Trim(); // mater name
                                                                                                                 //string aphla = dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim();
                                                                                                                 //string beta = coal_barcode.Substring(0, 5);
                                        if (coal_barcode.ToString() != "")
                                        {
                                            if (dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim() == coal_barcode.Substring(0, 5))
                                            {
                                                dr[6] = coal_barcode;// Mater barcode -- Coal
                                                dr[7] = "20" + coal_barcode.ToString().Trim().Substring(7, 6); // batchno
                                            }
                                            else
                                            {
                                                dr[6] = "";// Mater barcode -- Oil
                                                dr[7] = dtIn.Rows[0][7].ToString().Trim(); // batchno
                                            }
                                        }
                                        else
                                        {
                                            dr[6] = "";// Mater barcode -- Oil
                                            dr[7] = dtIn.Rows[0][7].ToString().Trim(); // batchno
                                        }

                                        dr[8] = dtIn.Rows[0][8].ToString().Trim(); // equip id
                                        dr[9] = dt_check_old_coal.Rows[i_count]["set_weight"].ToString(); // set weight
                                        dr[10] = dt_check_old_coal.Rows[i_count]["error_allow"].ToString(); // error low
                                    //dr[11] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim();

                                    dtIn.Rows.Add(dr);
                                    }
                                }

                            }

                            dtIn.DefaultView.Sort = "Serial_Num ASC";

                        
                    }
                    if ( May == "03" || May == "05" || May == "01" || May == "02" || May == "07" || May == "06"|| May == "04")
                    {
                       

                      
                        string GetIn = "WITH ranked_b AS ( " +
                                       "SELECT b.*, " +
                                       "ROW_NUMBER() OVER (PARTITION BY b.Barcode, b.mater_code, b.Mater_Type ORDER BY b.SaveTime DESC) as rn " +
                                       "FROM [mfns].[dbo].[Ppt_BarCodeRep] b " +
                                       "WHERE b.Plan_ID = '" + planId + "' " +
                                       "AND SUBSTRING(b.mater_code, 1, 3) <> '680' " +
                                       "), " +
                                       "filtered_b AS ( " +
                                       "SELECT * " +
                                       "FROM ranked_b " +
                                       "WHERE rn = 1 " +
                                       "), " +
                                       "c AS ( " +
                                       "SELECT b.SaveTime, " + /*"SELECT b.SaveTime AS test, " +*/
                                       "b.Recipe_Name, " +
                                       "b.Set_Num, " +
                                       "b.Serial_Num, " +
                                       "b.Mater_Code, " +
                                       "b.Mater_Name, " +
                                       "b.Mater_Barcode, " +
                                       "CASE " +
                                       "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'V' THEN '20' + SUBSTRING(Mater_Barcode, 4, 6) " +
                                       "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'A' " +
                                       "THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '0' + " +
                                       "CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '-10' + SUBSTRING(Mater_Barcode, 6, 2) " +
                                       "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'B' " +
                                       "THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '0' + " +
                                       "CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '-11' + SUBSTRING(Mater_Barcode, 6, 2) " +
                                       "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'C' " +
                                       "THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '0' + " +
                                       "CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '-12' + SUBSTRING(Mater_Barcode, 6, 2) " +
                                       "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' " +
                                       "THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '0' + " +
                                       "CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '-0' + SUBSTRING(Mater_Barcode, 5, 3) " +
                                       "ELSE '20' + SUBSTRING(Mater_Barcode, 8, 6) " +
                                       "END AS batchno, " +
                                       "b.Equip_ID, " +
                                       "a.real_weight, " +
                                       "a.error_allow,b.Mater_Type,a.weight_id   " + /*"a.error_allow, " +*/
                                       //"a.weigh_time AS SaveTime " +
                                       "FROM [mfns].[dbo].[ppt_weigh] a " +
                                       "LEFT JOIN filtered_b b ON a.barcode = b.Barcode " +
                                       "AND b.mater_code = a.mater_code " +
                                       "AND a.weight_id = b.Mater_Type " +
                                       "WHERE a.barcode IS NOT NULL " +
                                       ") " +
                                       "SELECT DISTINCT * " +
                                       "FROM c " +
                                       "WHERE batchno IS NOT NULL " +
                                       "AND (Mater_Code NOT LIKE '60%' " +
                                       "OR (Mater_Code LIKE '60%' AND Mater_Code = Mater_Name)) " +
                                       "ORDER BY SaveTime;";



                        dtIn = Cnn.ExecuteQuery(ConnectionString, GetIn);
                        dtIn.Columns.Remove("Mater_Type");
                        dtIn.Columns.Remove("weight_id");

                        DataTable dtnameThan = Cnn.ExecuteQuery(ConnectionString, "SELECT mater_code,mater_name  FROM [mfns].[dbo].[pmt_material]  where [mater_code] like '60%'");

                        foreach (DataRow row2 in dtIn.Rows)
                        {
                            // Lấy giá trị của cột "mater_code" từ hàng hiện tại
                            string materCode2 = row2["mater_code"].ToString();

                            // Tìm hàng trong DataTable thứ nhất có cùng giá trị "mater_code"
                            DataRow[] matchingRows = dtnameThan.Select($"mater_code = '{materCode2}'");

                            // Kiểm tra xem có hàng phù hợp không
                            if (matchingRows.Length > 0)
                            {
                                // Lấy giá trị của cột "mater_name" từ hàng phù hợp đầu tiên
                                string materName1 = matchingRows[0]["mater_name"].ToString();

                                // Cập nhật giá trị của cột "mater_name" trong hàng hiện tại của DataTable thứ hai
                                row2["mater_name"] = materName1;
                            }
                        }





                        string s_pptweight_code = " SELECT TOP 1 [Barcode] FROM[mfns].[dbo].[Ppt_BarCodeRep] where Plan_ID = '" + planId + "'";
                        DataTable dt_weight_code = Cnn.ExecuteQuery(ConnectionString, s_pptweight_code);

                        if (dt_weight_code.Rows.Count != 0)
                        {
                            //    string ppt_barcode = dt_weight_code.Rows[0][0].ToString().Trim().Substring(0, dt_weight_code.Rows[0][0].ToString().Trim().Length - 3);
                            string check_oil_coal = "  SELECT a.[barcode],a.[mater_code], a.[equip_code], a.[set_weight], CONVERT(nvarchar(20),a.[weigh_time],120) as weigh_time ,a.[error_allow], a.[weigh_type], b.mater_name " +
                                "FROM[mfns].[dbo].[ppt_weigh] a, [mfns].[dbo].[pmt_material] b where barcode like '" + planId + "%' and weigh_type='油料' and a.mater_code = b.mater_code order by weigh_time asc ";
                            DataTable dt_check_old_coal = Cnn.ExecuteQuery(ConnectionString, check_oil_coal);
                            if (dt_check_old_coal.Rows.Count != 0)
                            {
                                string coal_barcode = "";
                                string s_fromday = dt_check_old_coal.Rows[0]["weigh_time"].ToString().Trim();
                                DateTime dat_point = DateTime.Parse(s_fromday);
                                DateTime dat_check = DateTime.Parse(s_fromday);
                                TimeSpan ts_check = new TimeSpan(06, 30, 00);
                                string s_from = "";

                                dat_check = dat_check.Date + ts_check;
                                if (dat_point <= dat_check)
                                {
                                    dat_check = dat_check.AddDays(-1);
                                }
                                s_from = dat_check.ToString("yyyy-MM-dd HH:mm:ss");
                                string coal_code = "";
                                for (int _icount = 0; _icount < dt_check_old_coal.Rows.Count; _icount++)
                                {
                                    if (dt_check_old_coal.Rows[_icount]["weigh_type"].ToString().Trim() == "油料")
                                    {
                                        coal_code = dt_check_old_coal.Rows[_icount]["mater_code"].ToString().Trim();
                                        break;
                                    }
                                }
                                string s_coal_barcode = "  SELECT top 1 [Mater_Barcode],[SaveTime] FROM [mfns].[dbo].[Ppt_Oil] where  SaveTime <= '" + s_fromday + "' and Mater_Type = '0' and [Mater_Code] ='" + coal_code + "' order by SaveTime desc";



                                DataTable dt_coal_barcode = Cnn.ExecuteQuery(ConnectionString, s_coal_barcode);
                                if (dt_coal_barcode.Rows.Count != 0)
                                {
                                    coal_barcode = dt_coal_barcode.Rows[0][0].ToString().Trim();
                                }

                                DataRow dr;
                                int i_max = dt_check_old_coal.Rows.Count;//int.Parse(dtIn.Rows[0]["Set_Num"].ToString().Trim());
                                for (int i_count = 0; i_count < i_max; i_count++)
                                {
                                    dr = dtIn.NewRow();
                                    dr[0] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim(); // time
                                    dr[1] = dtIn.Rows[0][1].ToString().Trim(); // recipe
                                    dr[2] = dtIn.Rows[0][2].ToString().Trim(); // setnum
                                                                               //dr[3] = dtIn.Rows[i_count][3].ToString().Trim(); // serial num
                                    dr[3] = int.Parse(dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim().Substring(dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim().Length - 2, 2));// serial num
                                    dr[4] = dt_check_old_coal.Rows[i_count]["mater_code"].ToString(); // mater code
                                    dr[5] = dt_check_old_coal.Rows[i_count]["mater_name"].ToString().Trim(); // mater name
                                                                                                             //string aphla = dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim();
                                                                                                             //string beta = coal_barcode.Substring(0, 5);
                                    if (coal_barcode.ToString() != "")
                                    {
                                        if (dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim() == coal_barcode.Substring(0, 5))
                                        {
                                            dr[6] = coal_barcode;// Mater barcode -- Coal
                                            dr[7] = "20" + coal_barcode.ToString().Trim().Substring(7, 6); // batchno
                                        }
                                        else
                                        {
                                            dr[6] = "";// Mater barcode -- Oil
                                            dr[7] = dtIn.Rows[0][7].ToString().Trim(); // batchno
                                        }
                                    }
                                    else
                                    {
                                        dr[6] = "";// Mater barcode -- Oil
                                        dr[7] = dtIn.Rows[0][7].ToString().Trim(); // batchno
                                    }

                                    dr[8] = dtIn.Rows[0][8].ToString().Trim(); // equip id
                                    dr[9] = dt_check_old_coal.Rows[i_count]["set_weight"].ToString(); // set weight
                                    dr[10] = dt_check_old_coal.Rows[i_count]["error_allow"].ToString(); // error low

                                    //dr[11] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim();
                                    dtIn.Rows.Add(dr);
                                }
                            }

                        }

                        dtIn.DefaultView.Sort = "Serial_Num ASC";


                    }

                    if (dtIn.Rows.Count == 0)
                    {
                        ThongBao("Mã mes này chưa kết thúc hoặc đánh tay (table Ppt_BarCodeRep không có liệu) , không thể xem dữ liệu quét tem !!!"); // ko trung plan_iD
                    }
                    dtIn.AcceptChanges();
                    dtIn.DefaultView.Sort = "Serial_Num ASC";
                    dtIn = SortDataTable(dtIn, "Serial_Num");
                    TempData.tableDetail = dtIn;
                    gvNguyenLieu.DataSource = dtIn;
                    gvNguyenLieu.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvNguyenLieu();", true);
                    break;


                case ("btnOut"):
                    string GetOut = "select mesid,machno,daylimt,barcode,slipno,weight,prodat,effdat,class,partno,intime,indat,usrno,pallet_no,some_sx " +
                "from [erp].[dbo].[prdebe] where subno=4 and factory='V' and mesid='" + id + "' order by indat desc,intime desc";
                    System.Data.DataTable dtOut = Cnn.ExecuteQuery(ConnectionStringHome, GetOut);
                    if (dtOut.Rows.Count == 0)
                    {
                        ThongBao("Mã mesid không tìm thấy !!!");
                    }
                    int a = dtOut.Rows.Count;
                    SumSL.Text = a.ToString().Trim();

                    // Tim may in
                    int i = 0;
                    string[] sPrinterList = new string[System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count];
                    if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count == 0)
                        sPrinterList[i] = "";
                    else
                    {
                        drMayin.Items.Clear();
                        foreach (string printers in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                        {
                            if (printers.Substring(printers.Length - 3, 3) == "193" || printers.Substring(printers.Length - 3, 3) == "176" || printers.Substring(printers.Length - 3, 3) == "122")
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
                    TempData.prdebe = dtOut;
                    gvInTem.DataSource = dtOut;
                    gvInTem.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvInTem();", true);
                    break;
            }

        }
        static DataTable SortDataTable(DataTable dataTable, string columnName)
        {
            // Sử dụng LINQ để sắp xếp dữ liệu
            var sortedRows = dataTable.AsEnumerable()
                                .OrderBy(row => row[columnName]);

            // Tạo DataTable mới từ kết quả đã sắp xếp
            DataTable sortedDataTable = dataTable.Clone();
            foreach (DataRow row in sortedRows)
            {
                sortedDataTable.ImportRow(row);
            }

            return sortedDataTable;
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

                //if (loaikeo == "RC")
                //{
                //    worksheet.Range["C4"].Value = "精煉";
                //    worksheet.Range["C5"].Value = "Tinh luyện";
                //}

                if (ca == "1")
                {
                    Ca = "早 (Sáng)";
                }
                if (ca == "2")
                {
                    Ca = "夜 (Đêm)";
                }

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
                worksheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA5;
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

        private System.Data.DataTable DoNguoc(string barcode)
        {
            string GetData = "SELECT mesid,machno,prodat,partno,indat FROM [erp].[dbo].[prdebe] where barcode ='" + barcode + "' ";
            System.Data.DataTable sql = Cnn.ExecuteQuery(ConnectionStringHome, GetData);


            if (sql.Rows.Count == 0)
            {
                System.Data.DataTable dt = null;
                return dt;
            }
            else
            {
                string mesid = sql.Rows[0]["mesid"].ToString().Trim();
                string machno = sql.Rows[0]["machno"].ToString().Trim().Substring(6, 2);
                string prodat = sql.Rows[0]["prodat"].ToString().Trim();
                string partno = sql.Rows[0]["partno"].ToString().Replace("-", "").Trim();
                string indat = sql.Rows[0]["indat"].ToString().Trim();
                string ConnectionString = ChonMay(machno);
                if (barcode.Substring(0, 2) == "RL")
                {
                    string sqlRL = "select [pday],[class] ,[machno],[mesid] ,[barcode],[partno],[qty],[bacode],[itnbr],[intime],[indat],[usrno]  FROM [erp].[dbo].[prdebc] where barcode ='" + barcode + "'";
                    System.Data.DataTable dt = Cnn.ExecuteQueryBB(sqlRL);

                    if (dt.Rows.Count == 0)
                    {
                        dt = null;
                        return dt;
                    }
                    else
                    if (dt.Rows[0]["bacode"].ToString().Substring(0, 1) != "R")
                    {

                        string sqlRL1 = "select a.[pday],a.[class] ,a.[machno],a.[mesid] ,a.barcode,a.[partno],a.[qty],a.[bacode],a.[itnbr],b.slipno ,a.[intime],a.[indat],a.[usrno] FROM [erp].[dbo].[prdebc] a, [erp].[dbo].[prdebe] b where a.barcode ='" + dt.Rows[0]["bacode"].ToString() + "' and a.bacode=b.barcode";
                        dt = Cnn.ExecuteQueryBB(sqlRL1);

                        TempData.tableDoNguocRL = dt;
                    }
                    else
                    {
                        string sqlRL2 = "select a.[pday],a.[class] ,a.[machno],a.[mesid] ,a.barcode,a.[partno],a.[qty],a.[bacode],a.[itnbr],b.slipno ,a.[intime],a.[indat],a.[usrno]  FROM [erp].[dbo].[prdebc] a, [erp].[dbo].[prdebe] b where a.barcode ='" + barcode + "' and a.bacode=b.barcode";
                        dt = Cnn.ExecuteQueryBB(sqlRL2);
                        TempData.tableDoNguocRL = dt;
                    }
                    return dt;
                }
                else
                {
                    string maydonguoc = '0' + mesid.Substring(2, 1);

                    string connectdonguoc = ChonMay(maydonguoc);
                    string sqlIdGrlot = "select idGrouplot,indat,recipe_name from KEORE where factory='V' and mesid ='" + mesid + "' and machno = '" + maydonguoc + "'";
                    System.Data.DataTable idKEORE = Cnn.ExecuteQuery("Data Source=198.1.9.186;Initial Catalog=InTem;User ID=kendakv2;Password=kenda123", sqlIdGrlot);
                    string IdGrlot = idKEORE.Rows[0]["idGrouplot"].ToString().Trim();
                    string GetPlanIddonguoc = "SELECT Plan_ID FROM [mfns].[dbo].[Ppt_GroupLot] where Id = '" + IdGrlot + "'";
                    System.Data.DataTable dtPlanId = Cnn.ExecuteQuery(ConnectionString, GetPlanIddonguoc);
                    string planidlumlua = dtPlanId.Rows[0]["Plan_ID"].ToString().Trim();
                    System.Data.DataTable dt = new DataTable();
                    string GetDoNguoc = "";

                    string recipename = idKEORE.Rows[0]["recipe_name"].ToString().Trim();

                    string newstring = recipename.Substring(recipename.Length - 1, 1);

                    //so sanh ngay thang
                    string year = idKEORE.Rows[0]["indat"].ToString().Trim().Substring(0, 4);
                    string mm = idKEORE.Rows[0]["indat"].ToString().Trim().Substring(4, 2);
                    string dd = idKEORE.Rows[0]["indat"].ToString().Trim().Substring(6, 2);

                    DateTime date1 = new DateTime(2023, 5, 1, 0, 0, 0);
                    DateTime date2 = new DateTime(Int32.Parse(year), Int32.Parse(mm), Int32.Parse(dd), 0, 0, 0);
                    int result = DateTime.Compare(date1, date2);
                    //end

                    if (maydonguoc == "test" )
                    {
                        string GetIn = "   WITH c AS(" +
                         " SELECT b.SaveTime as test,b.Recipe_Name,b.Set_Num,b.Serial_Num,b.Mater_Code,b.Mater_Name,b.Mater_Barcode,  " +
                         "CASE WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'V' THEN '20' + SUBSTRING(Mater_Barcode, 4, 6)     " +
                         "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'A' " +
                         "THEN cast( (SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem]  WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+ cast( (SELECT Equip_ID  FROM [mfnsShareDB].[dbo].[IF_Datatem]  WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-10'+ SUBSTRING(Mater_Barcode, 6, 2)  " +
                         "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'B'" +
                         "THEN cast((SELECT Shift  FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+cast((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-11'+ SUBSTRING(Mater_Barcode, 6, 2) " +
                         "  WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'C'  " +
                         " THEN cast((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+cast( (SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-12'+ SUBSTRING(Mater_Barcode, 6, 2)   " +
                         "  WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' THEN cast( (SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+cast((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem]  WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-0' + SUBSTRING(Mater_Barcode, 5, 3)    " +
                         " ELSE '20' + SUBSTRING(Mater_Barcode, 8, 6)  END AS batchno ,b.Equip_ID,a.real_weight,a.error_allow,a.weigh_time as SaveTime " +
                         " FROM  [mfns].[dbo].[ppt_weigh] a full join [mfns].[dbo].[Ppt_BarCodeRep] b  on  a.barcode =b.Barcode   AND b.mater_code = a.mater_code " +
                         " where  b.Plan_ID = '" + planidlumlua + "'  AND substring(b.mater_code, 1, 3) <> '680' and a.barcode is not null )" +

                         " select distinct * from c where batchno is not null and Mater_Code not like '60%' or (Mater_Code like '60%' and Mater_Code=Mater_Name) order by SaveTime";


                        dt = Cnn.ExecuteQuery(ConnectionString, GetIn);


                        DataTable dtnameThan = Cnn.ExecuteQuery(ConnectionString, "SELECT mater_code,mater_name  FROM [mfns].[dbo].[pmt_material]  where [mater_code] like '60%'");

                        foreach (DataRow row2 in dt.Rows)
                        {
                            // Lấy giá trị của cột "mater_code" từ hàng hiện tại
                            string materCode2 = row2["mater_code"].ToString();

                            // Tìm hàng trong DataTable thứ nhất có cùng giá trị "mater_code"
                            DataRow[] matchingRows = dtnameThan.Select($"mater_code = '{materCode2}'");

                            // Kiểm tra xem có hàng phù hợp không
                            if (matchingRows.Length > 0)
                            {
                                // Lấy giá trị của cột "mater_name" từ hàng phù hợp đầu tiên
                                string materName1 = matchingRows[0]["mater_name"].ToString();

                                // Cập nhật giá trị của cột "mater_name" trong hàng hiện tại của DataTable thứ hai
                                row2["mater_name"] = materName1;
                            }
                        }


                        string s_pptweight_code = " SELECT TOP 1 [Barcode] FROM[mfns].[dbo].[Ppt_BarCodeRep] where Plan_ID = '" + planidlumlua + "'";
                        DataTable dt_weight_code = Cnn.ExecuteQuery(ConnectionString, s_pptweight_code);

                        if (dt_weight_code.Rows.Count != 0)
                        {
                            //    string ppt_barcode = dt_weight_code.Rows[0][0].ToString().Trim().Substring(0, dt_weight_code.Rows[0][0].ToString().Trim().Length - 3);
                            string check_oil_coal = "  SELECT a.[barcode],a.[mater_code], a.[equip_code], a.[set_weight], CONVERT(nvarchar(20),a.[weigh_time],120) as weigh_time ,a.[error_allow], a.[weigh_type], b.mater_name " +
                                "FROM[mfns].[dbo].[ppt_weigh] a, [mfns].[dbo].[pmt_material] b where barcode like '" + planidlumlua + "%' and weigh_type='油料' and a.mater_code = b.mater_code order by weigh_time asc ";
                            DataTable dt_check_old_coal = Cnn.ExecuteQuery(ConnectionString, check_oil_coal);
                            if (dt_check_old_coal.Rows.Count != 0)
                            {
                                string coal_barcode = "";
                                string s_fromday = dt_check_old_coal.Rows[0]["weigh_time"].ToString().Trim();
                                DateTime dat_point = DateTime.Parse(s_fromday);
                                DateTime dat_check = DateTime.Parse(s_fromday);
                                TimeSpan ts_check = new TimeSpan(06, 30, 00);
                                string s_from = "";

                                dat_check = dat_check.Date + ts_check;
                                if (dat_point <= dat_check)
                                {
                                    dat_check = dat_check.AddDays(-1);
                                }
                                s_from = dat_check.ToString("yyyy-MM-dd HH:mm:ss");
                                string coal_code = "";
                                for (int _icount = 0; _icount < dt_check_old_coal.Rows.Count; _icount++)
                                {
                                    if (dt_check_old_coal.Rows[_icount]["weigh_type"].ToString().Trim() == "油料")
                                    {
                                        coal_code = dt_check_old_coal.Rows[_icount]["mater_code"].ToString().Trim();
                                        break;
                                    }
                                }
                                string s_coal_barcode = "  SELECT top 1 [Mater_Barcode],[SaveTime] FROM [mfns].[dbo].[Ppt_Oil] where  SaveTime <= '" + s_fromday + "' and Mater_Type = '0' and [Mater_Code] ='" + coal_code + "' order by SaveTime desc";



                                DataTable dt_coal_barcode = Cnn.ExecuteQuery(ConnectionString, s_coal_barcode);
                                if (dt_coal_barcode.Rows.Count != 0)
                                {
                                    coal_barcode = dt_coal_barcode.Rows[0][0].ToString().Trim();
                                }

                                DataRow dr;
                                int i_max = dt_check_old_coal.Rows.Count;//int.Parse(dtIn.Rows[0]["Set_Num"].ToString().Trim());
                                for (int i_count = 0; i_count < i_max; i_count++)
                                {
                                    dr = dt.NewRow();
                                    dr[0] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim(); // time
                                    dr[1] = dt.Rows[0][1].ToString().Trim(); // recipe
                                    dr[2] = dt.Rows[0][2].ToString().Trim(); // setnum
                                                                               //dr[3] = dtIn.Rows[i_count][3].ToString().Trim(); // serial num
                                    dr[3] = int.Parse(dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim().Substring(dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim().Length - 2, 2));// serial num
                                    dr[4] = dt_check_old_coal.Rows[i_count]["mater_code"].ToString(); // mater code
                                    dr[5] = dt_check_old_coal.Rows[i_count]["mater_name"].ToString().Trim(); // mater name
                                                                                                             //string aphla = dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim();
                                                                                                             //string beta = coal_barcode.Substring(0, 5);
                                    if (coal_barcode.ToString() != "")
                                    {
                                        if (dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim() == coal_barcode.Substring(0, 5))
                                        {
                                            dr[6] = coal_barcode;// Mater barcode -- Coal
                                            dr[7] = "20" + coal_barcode.ToString().Trim().Substring(7, 6); // batchno
                                        }
                                        else
                                        {
                                            dr[6] = "";// Mater barcode -- Oil
                                            dr[7] = dt.Rows[0][7].ToString().Trim(); // batchno
                                        }
                                    }
                                    else
                                    {
                                        dr[6] = "";// Mater barcode -- Oil
                                        dr[7] = dt.Rows[0][7].ToString().Trim(); // batchno
                                    }

                                    dr[8] = dt.Rows[0][8].ToString().Trim(); // equip id
                                    dr[9] = dt_check_old_coal.Rows[i_count]["set_weight"].ToString(); // set weight
                                    dr[10] = dt_check_old_coal.Rows[i_count]["error_allow"].ToString(); // error low
                                    //dr[11] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim();
                                    dt.Rows.Add(dr);
                                }
                            }

                        }

                        dt.DefaultView.Sort = "Serial_Num ASC";


                    }
                    if ( maydonguoc == "03" || maydonguoc == "05" || maydonguoc == "01" || maydonguoc == "02" || maydonguoc == "07" || maydonguoc == "06"|| maydonguoc == "04")
                    {
                        //string GetIn = "   WITH c AS(" +
                        // " SELECT b.SaveTime as test,b.Recipe_Name,b.Set_Num,b.Serial_Num,b.Mater_Code,b.Mater_Name,b.Mater_Barcode,  " +
                        // "CASE WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'V' THEN '20' + SUBSTRING(Mater_Barcode, 4, 6)     " +
                        // "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'A' " +
                        // "THEN cast( (SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem]  WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+ cast( (SELECT Equip_ID  FROM [mfnsShareDB].[dbo].[IF_Datatem]  WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-10'+ SUBSTRING(Mater_Barcode, 6, 2)  " +
                        // "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'B'" +
                        // "THEN cast((SELECT Shift  FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+cast((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-11'+ SUBSTRING(Mater_Barcode, 6, 2) " +
                        // "  WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'C'  " +
                        // " THEN cast((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+cast( (SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-12'+ SUBSTRING(Mater_Barcode, 6, 2)   " +
                        // "  WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' THEN cast( (SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab= Mater_Barcode) AS varchar) +'0'+cast((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem]  WHERE BarCodeLab= Mater_Barcode) AS varchar) + '-0' + SUBSTRING(Mater_Barcode, 5, 3)    " +
                        // " ELSE '20' + SUBSTRING(Mater_Barcode, 8, 6)  END AS batchno ,b.Equip_ID,a.real_weight,a.error_allow,a.weigh_time as SaveTime " +
                        // " FROM  [mfns].[dbo].[ppt_weigh] a full join [mfns].[dbo].[Ppt_BarCodeRep] b  on  a.barcode =b.Barcode   AND b.mater_code = a.mater_code and a.weight_id =b.Mater_Type " +
                        // " where  b.Plan_ID = '" + planidlumlua + "'  AND substring(b.mater_code, 1, 3) <> '680' and a.barcode is not null )" +

                        // " select distinct * from c where batchno is not null and Mater_Code not like '60%' or (Mater_Code like '60%' and Mater_Code=Mater_Name) order by SaveTime";


                        string GetIn = "WITH ranked_b AS ( " +
                                     "SELECT b.*, " +
                                     "ROW_NUMBER() OVER (PARTITION BY b.Barcode, b.mater_code, b.Mater_Type ORDER BY b.SaveTime DESC) as rn " +
                                     "FROM [mfns].[dbo].[Ppt_BarCodeRep] b " +
                                     "WHERE b.Plan_ID = '" + planidlumlua + "' " +
                                     "AND SUBSTRING(b.mater_code, 1, 3) <> '680' " +
                                     "), " +
                                     "filtered_b AS ( " +
                                     "SELECT * " +
                                     "FROM ranked_b " +
                                     "WHERE rn = 1 " +
                                     "), " +
                                     "c AS ( " +
                                     "SELECT b.SaveTime, " + /*"SELECT b.SaveTime AS test, " +*/
                                     "b.Recipe_Name, " +
                                     "b.Set_Num, " +
                                     "b.Serial_Num, " +
                                     "b.Mater_Code, " +
                                     "b.Mater_Name, " +
                                     "b.Mater_Barcode, " +
                                     "CASE " +
                                     "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'V' THEN '20' + SUBSTRING(Mater_Barcode, 4, 6) " +
                                     "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'A' " +
                                     "THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '0' + " +
                                     "CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '-10' + SUBSTRING(Mater_Barcode, 6, 2) " +
                                     "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'B' " +
                                     "THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '0' + " +
                                     "CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '-11' + SUBSTRING(Mater_Barcode, 6, 2) " +
                                     "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'C' " +
                                     "THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '0' + " +
                                     "CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '-12' + SUBSTRING(Mater_Barcode, 6, 2) " +
                                     "WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' " +
                                     "THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '0' + " +
                                     "CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS varchar) + '-0' + SUBSTRING(Mater_Barcode, 5, 3) " +
                                     "ELSE '20' + SUBSTRING(Mater_Barcode, 8, 6) " +
                                     "END AS batchno, " +
                                     "b.Equip_ID, " +
                                     "a.real_weight, " +
                                     "a.error_allow,b.Mater_Type,a.weight_id " + /*"a.error_allow, " +*/
                                     //"a.weigh_time AS SaveTime " +
                                     "FROM [mfns].[dbo].[ppt_weigh] a " +
                                     "LEFT JOIN filtered_b b ON a.barcode = b.Barcode " +
                                     "AND b.mater_code = a.mater_code " +
                                     "AND a.weight_id = b.Mater_Type " +
                                     "WHERE a.barcode IS NOT NULL " +
                                     ") " +
                                     "SELECT DISTINCT * " +
                                     "FROM c " +
                                     "WHERE batchno IS NOT NULL " +
                                     "AND (Mater_Code NOT LIKE '60%' " +
                                     "OR (Mater_Code LIKE '60%' AND Mater_Code = Mater_Name)) " +
                                     "ORDER BY SaveTime;";




                        dt = Cnn.ExecuteQuery(ConnectionString, GetIn);
                        dt.Columns.Remove("Mater_Type");
                        dt.Columns.Remove("weight_id");


                        DataTable dtnameThan = Cnn.ExecuteQuery(ConnectionString, "SELECT mater_code,mater_name  FROM [mfns].[dbo].[pmt_material]  where [mater_code] like '60%'");

                        foreach (DataRow row2 in dt.Rows)
                        {
                            // Lấy giá trị của cột "mater_code" từ hàng hiện tại
                            string materCode2 = row2["mater_code"].ToString();

                            // Tìm hàng trong DataTable thứ nhất có cùng giá trị "mater_code"
                            DataRow[] matchingRows = dtnameThan.Select($"mater_code = '{materCode2}'");

                            // Kiểm tra xem có hàng phù hợp không
                            if (matchingRows.Length > 0)
                            {
                                // Lấy giá trị của cột "mater_name" từ hàng phù hợp đầu tiên
                                string materName1 = matchingRows[0]["mater_name"].ToString();

                                // Cập nhật giá trị của cột "mater_name" trong hàng hiện tại của DataTable thứ hai
                                row2["mater_name"] = materName1;
                            }
                        }


                        string s_pptweight_code = " SELECT TOP 1 [Barcode] FROM[mfns].[dbo].[Ppt_BarCodeRep] where Plan_ID = '" + planidlumlua + "'";
                        DataTable dt_weight_code = Cnn.ExecuteQuery(ConnectionString, s_pptweight_code);

                        if (dt_weight_code.Rows.Count != 0)
                        {
                            //    string ppt_barcode = dt_weight_code.Rows[0][0].ToString().Trim().Substring(0, dt_weight_code.Rows[0][0].ToString().Trim().Length - 3);
                            string check_oil_coal = "  SELECT a.[barcode],a.[mater_code], a.[equip_code], a.[set_weight], CONVERT(nvarchar(20),a.[weigh_time],120) as weigh_time ,a.[error_allow], a.[weigh_type], b.mater_name " +
                                "FROM[mfns].[dbo].[ppt_weigh] a, [mfns].[dbo].[pmt_material] b where barcode like '" + planidlumlua + "%' and weigh_type='油料' and a.mater_code = b.mater_code order by weigh_time asc ";
                            DataTable dt_check_old_coal = Cnn.ExecuteQuery(ConnectionString, check_oil_coal);
                            if (dt_check_old_coal.Rows.Count != 0)
                            {
                                string coal_barcode = "";
                                string s_fromday = dt_check_old_coal.Rows[0]["weigh_time"].ToString().Trim();
                                DateTime dat_point = DateTime.Parse(s_fromday);
                                DateTime dat_check = DateTime.Parse(s_fromday);
                                TimeSpan ts_check = new TimeSpan(06, 30, 00);
                                string s_from = "";

                                dat_check = dat_check.Date + ts_check;
                                if (dat_point <= dat_check)
                                {
                                    dat_check = dat_check.AddDays(-1);
                                }
                                s_from = dat_check.ToString("yyyy-MM-dd HH:mm:ss");
                                string coal_code = "";
                                for (int _icount = 0; _icount < dt_check_old_coal.Rows.Count; _icount++)
                                {
                                    if (dt_check_old_coal.Rows[_icount]["weigh_type"].ToString().Trim() == "油料")
                                    {
                                        coal_code = dt_check_old_coal.Rows[_icount]["mater_code"].ToString().Trim();
                                        break;
                                    }
                                }
                                string s_coal_barcode = "  SELECT top 1 [Mater_Barcode],[SaveTime] FROM [mfns].[dbo].[Ppt_Oil] where  SaveTime <= '" + s_fromday + "' and Mater_Type = '0' and [Mater_Code] ='" + coal_code + "' order by SaveTime desc";



                                DataTable dt_coal_barcode = Cnn.ExecuteQuery(ConnectionString, s_coal_barcode);
                                if (dt_coal_barcode.Rows.Count != 0)
                                {
                                    coal_barcode = dt_coal_barcode.Rows[0][0].ToString().Trim();
                                }

                                DataRow dr;
                                int i_max = dt_check_old_coal.Rows.Count;//int.Parse(dtIn.Rows[0]["Set_Num"].ToString().Trim());
                                for (int i_count = 0; i_count < i_max; i_count++)
                                {
                                    dr = dt.NewRow();
                                    dr[0] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim(); // time
                                    dr[1] = dt.Rows[0][1].ToString().Trim(); // recipe
                                    dr[2] = dt.Rows[0][2].ToString().Trim(); // setnum
                                                                             //dr[3] = dtIn.Rows[i_count][3].ToString().Trim(); // serial num
                                    dr[3] = int.Parse(dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim().Substring(dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim().Length - 2, 2));// serial num
                                    dr[4] = dt_check_old_coal.Rows[i_count]["mater_code"].ToString(); // mater code
                                    dr[5] = dt_check_old_coal.Rows[i_count]["mater_name"].ToString().Trim(); // mater name
                                                                                                             //string aphla = dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim();
                                                                                                             //string beta = coal_barcode.Substring(0, 5);
                                    if (coal_barcode.ToString() != "")
                                    {
                                        if (dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim() == coal_barcode.Substring(0, 5))
                                        {
                                            dr[6] = coal_barcode;// Mater barcode -- Coal
                                            dr[7] = "20" + coal_barcode.ToString().Trim().Substring(7, 6); // batchno
                                        }
                                        else
                                        {
                                            dr[6] = "";// Mater barcode -- Oil
                                            dr[7] = dt.Rows[0][7].ToString().Trim(); // batchno
                                        }
                                    }
                                    else
                                    {
                                        dr[6] = "";// Mater barcode -- Oil
                                        dr[7] = dt.Rows[0][7].ToString().Trim(); // batchno
                                    }

                                    dr[8] = dt.Rows[0][8].ToString().Trim(); // equip id
                                    dr[9] = dt_check_old_coal.Rows[i_count]["set_weight"].ToString(); // set weight
                                    dr[10] = dt_check_old_coal.Rows[i_count]["error_allow"].ToString(); // error low
                                    //dr[11] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim();
                                    dt.Rows.Add(dr);
                                }
                            }

                        }

                        dt.DefaultView.Sort = "Serial_Num ASC";


                    }



                    for (int z = dt.Rows.Count - 1; z >= 0; z--)
                    {
                        DataRow dr = dt.Rows[z];
                        if (dr["batchno"].ToString().Trim() == "")
                            dr.Delete();
                        dt.AcceptChanges();
                    }
                    dt = SortDataTable(dt, "Serial_Num");
                    dt.AcceptChanges();
                    TempData.tableDoNguoc = dt;
                    return dt;

                }

            }
        }

        protected void gvInTem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string daylimt = Dulieu[0].ToString().Trim();
            string barcode = Dulieu[1].Trim();
            string loaikeo = Dulieu[1].ToString().Trim().Substring(0, 2);
            string slipno = Dulieu[2].ToString().Trim();
            string soluong = Dulieu[3].ToString().Trim();
            string effdat = Dulieu[4].ToString().Trim();
            string partno = Dulieu[5].ToString().Trim();
            string intime = Dulieu[6].ToString().Trim();
            string indat = Dulieu[7].ToString().Trim();
            string pallet = Dulieu[8].ToString().Trim();
            string ca = Dulieu[9].Trim();
            string mesid = Dulieu[10].Trim();
            string Machno = Dulieu[11].ToString().Substring(6, 2);
            string pday = Dulieu[12].Trim();
            string printer = drMayin.SelectedValue.ToString();

            string maso = "";
            string ConnectionStringKeoRe = "Data Source=198.1.9.186;Initial Catalog=InTem;User ID=kendakv2;Password=kenda123";
            string sqlKEORE = "select mesid from KEORE where factory='V' and mesid ='" + mesid + "' and machno = '" + Machno + "'";
            System.Data.DataTable dtKEORE = new System.Data.DataTable();
            dtKEORE = Cnn.ExecuteQuery(ConnectionStringKeoRe, sqlKEORE);
            if (dtKEORE.Rows.Count > 0)
            {
                string sqlBarcode = "SELECT * FROM prdebe where factory='V' and prodat='" + pday + "' and partno='" + partno + "'" +
                    " and class='" + ca + "' and barcode <= '" + barcode + "' order by intime";
                System.Data.DataTable dtBar = Cnn.ExecuteQuery(ConnectionStringHome, sqlBarcode);
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

                    string sqlweight = "SELECT weight FROM KEORE where factory='V' and mesid ='" + mesid + "' and machno='" + Machno + "'"; //lấy Barcode từ trong sql
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
            if (drMayin.Text == "Vui lòng kết nối máy in!!!" || drMayin.Text == "Không tìm thấy máy in thích hợp!!!" || drMayin.Text == "")
            {

                ThongBao("Vui lòng kiểm tra kết nối máy in, và thử lại sau!!!");
            }
            else
            {
                PrintExcel(Machno, loaikeo, ca, pallet, daylimt, indat, intime, slipno, soluong, effdat, partno, maso, barcode, printer);
            }
        }

        protected void gvNguyenLieu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string b = "";
            string a = e.CommandArgument.ToString().Trim();
            if (a != "")
            {
                switch (a.Substring(0, 2))
                {
                    case "RL":
                        System.Data.DataTable dt = DoNguoc(a);
                        if (dt?.Rows?.Count > 0)
                        {
                            Comment2.Visible = false;
                            TempData.tableDoNguocRL = dt;
                            gvDoNguocRL.DataSource = dt;
                            gvDoNguocRL.DataBind();

                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguocRL();", true);
                        }
                        else
                        {
                            Comment2.Visible = true;
                            Comment2.Text = "Không có dữ liệu";

                            gvDoNguocRL.DataSource = dt;
                            gvDoNguocRL.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguocRL();", true);
                        }
                        break;
                    case "RB":
                        dt = DoNguoc(a);
                        if (dt?.Rows?.Count > 0)
                        {
                            Comment1.Visible = false;
                            TempData.tableHC = dt;
                            gvDoNguoc.DataSource = dt;
                            gvDoNguoc.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                        }
                        else
                        {
                            Comment1.Visible = true;
                            Comment1.Text = "Không có dữ liệu";

                            gvDoNguoc.DataSource = dt;
                            gvDoNguoc.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                        }
                        break;
                    case "RD":
                        dt = DoNguoc(a);
                        if (dt?.Rows?.Count > 0)
                        {
                            Comment1.Visible = false;
                            TempData.tableHC = dt;
                            gvDoNguoc.DataSource = dt;
                            gvDoNguoc.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                        }
                        else
                        {
                            Comment1.Visible = true;
                            Comment1.Text = "Không có dữ liệu";

                            gvDoNguoc.DataSource = dt;
                            gvDoNguoc.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                        }
                        break;
                    case "RC":
                        dt = DoNguoc(a);
                        if (dt?.Rows?.Count > 0)
                        {
                            Comment1.Visible = false;
                            TempData.tableHC = dt;
                            gvDoNguoc.DataSource = dt;
                            gvDoNguoc.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                        }
                        else
                        {
                            Comment1.Visible = true;
                            Comment1.Text = "Không có dữ liệu";

                            gvDoNguoc.DataSource = dt;
                            gvDoNguoc.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                        }
                        break;
                }

                switch (a.Substring(0, 1))
                {
                    case "V":
                        string GetV = "";
                        string ConnectionString = string.Empty;
                        switch (a.Substring(0, 3).Trim())
                        {
                            case ("V11"):

                                GetV = "Select w.Dosing_id,w.Plan_id,w.Equip_code,w.Serial_Num,w.Weight_ID,w.Material_Code, n.Material_name,w.Real_Weight,w.Real_Error, " +
                            "w.Over_Weight,w.Over_Error,w.Waste_Time,w.Warning_Sign,w.Weight_Time,w.Batch_Number,w.Recipe_code from[CWSS_S7].[dbo].[LR_weigh] as w " +
                            "inner join[CWSS_S7].[dbo].[Pmt_material] as n on w.Material_Code = n.Material_code where Plan_id = '" + a.Substring(0, 13).Trim() + "' order by w.serial_num, w.Weight_Time";
                                b = "198.1.8.16";
                                break;
                            case ("V13"):

                                GetV = "Select w.Dosing_id,w.Plan_id,w.Equip_code,w.Serial_Num,w.Weight_ID,w.Material_Code, n.Material_name,w.Real_Weight,w.Real_Error, " +
                            "w.Over_Weight,w.Over_Error,w.Waste_Time,w.Warning_Sign,w.Weight_Time,w.Batch_Number,w.Recipe_code from[CWSS_S7].[dbo].[LR_weigh] as w " +
                            "inner join[CWSS_S7].[dbo].[Pmt_material] as n on w.Material_Code = n.Material_code where Plan_id = '" + a.Substring(0, 13).Trim() + "' order by w.serial_num, w.Weight_Time";
                                b = "198.1.8.15";
                                break;
                            case ("V12"):
                                GetV = " Select w.Dosing_id,w.Plan_id,w.Equip_code,w.Serial_Num,w.Weight_ID,w.Material_Code, n.Material_name,w.Real_Weight,w.Real_Error, w.Over_Weight,w.Over_Error,w.Waste_Time,w.Warning_Sign,w.Weight_Time,w.Batch_Number,w.Recipe_code FROM [CWSS_S7].[dbo].[LR_weigh] as w inner join[CWSS_S7].[dbo].[Pmt_material] as n on w.Material_Code = n.Material_code where Plan_id = '" + a.Substring(0, 13).Trim() + "' order by w.serial_num, w.Weight_Time";
                                b = "198.1.8.17";
                                break;
                            case ("V14"):
                                GetV = " Select w.Dosing_id,w.Plan_id,w.Equip_code,w.Serial_Num,w.Weight_ID,w.Material_Code, n.Material_name,w.Real_Weight,w.Real_Error, w.Over_Weight,w.Over_Error,w.Waste_Time,w.Warning_Sign,w.Weight_Time,w.Batch_Number,w.Recipe_code FROM [CWSS_S7].[dbo].[LR_weigh] as w inner join[CWSS_S7].[dbo].[Pmt_material] as n on w.Material_Code = n.Material_code where Plan_id = '" + a.Substring(0, 13).Trim() + "' order by w.serial_num, w.Weight_Time";
                                b = "198.1.8.18";
                                break;
                        }
                        switch (PingIp(b))
                        {
                            case (1):
                                if (b == "198.1.8.16" || b == "198.1.8.15")
                                {
                                    ConnectionString = "Data Source = " + b + "; Initial Catalog = CWSS_S7; User ID = kendakv2; Password = kenda123";
                                }
                                if (b == "198.1.8.17" || b == "198.1.8.18")
                                {
                                    ConnectionString = "Data Source = " + b + "; Initial Catalog = CWSS_S7; User ID = kendakv2; Password = kenda123";
                                }

                                break;
                            case (0):
                                ThongBao("Máy đang tắt, vui lòng mở máy và thử lại!!!");
                                break;
                            default:
                                ThongBao("Lỗi kết nối máy chủ, vui lòng thử lại!!!");
                                break;
                        }
                        System.Data.DataTable dtHC = Cnn.ExecuteQuery(ConnectionString, GetV);
                        if (dtHC.Rows.Count == 0)
                        {
                            TextBox1.Text = "Không có dữ liệu";
                            TextBox1.Visible = true;
                            gvHC.DataSource = dtHC;
                            gvHC.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvHC();", true);
                        }
                        else
                        {
                            TextBox1.Visible = false;
                            TempData.tableHC = dtHC;
                            gvHC.DataSource = dtHC;
                            gvHC.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvHC();", true);
                        }
                        break;



                }
            }

            if (a == "" || a.Substring(0, 1) == "0" || a.Substring(0, 1) == "1" || a.Substring(0, 1) == "2" || a.Substring(0, 1) == "3" || a.Substring(0, 1) == "4" || a.Substring(0, 1) == "5" || a.Substring(0, 1) == "6" || a.Substring(0, 1) == "7" || a.Substring(0, 1) == "8" || a.Substring(0, 1) == "9")
            {

                Comment1.Text = "Không có dữ liệu";
                Comment1.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);

            }
        }
        protected void gvDoNguocRL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string b = "";
            string a = e.CommandArgument.ToString().Trim();
            //string max = Soome(a).ToString();
            switch (a.Substring(0, 2))
            {
                case "RL":
                    System.Data.DataTable dt = DoNguoc(a);
                    if (dt?.Rows?.Count > 0)
                    {
                        Comment1.Visible = false;
                        TempData.tableDoNguocRL = dt;
                        gvDoNguocRL.DataSource = dt;
                        gvDoNguocRL.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                    }
                    else
                    {
                        Comment1.Text = "Không có dữ liệu";
                        Comment1.Visible = true;
                        gvDoNguocRL.DataSource = dt;
                        gvDoNguocRL.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                    }
                    break;
                default:
                    Comment1.Text = "Không có dữ liệu";
                    Comment1.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                    break;
            }
        }
        protected void gvDoNguoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string b = "";
            string a = e.CommandArgument.ToString().Trim();
            //string max = Soome(a).ToString();
            switch (a.Substring(0, 1))
            {
                case "V":
                    string GetV = "";

                    string ConnectionString = string.Empty;
                    switch (a.Substring(0, 3).Trim())
                    {
                        case ("V11"):
                            b = "198.1.8.16";
                            GetV = "Select w.Dosing_id,w.Plan_id,w.Equip_code,w.Serial_Num,w.Weight_ID,w.Material_Code, n.Material_name,w.Real_Weight,w.Real_Error, " +
                            "w.Over_Weight,w.Over_Error,w.Waste_Time,w.Warning_Sign,w.Weight_Time,w.Batch_Number,w.Recipe_code from[CWSS_S7].[dbo].[LR_weigh] as w " +
                            "inner join[CWSS_S7].[dbo].[Pmt_material] as n on w.Material_Code = n.Material_code where Plan_id = '" + a.Substring(0, 13).Trim() + "' order by w.serial_num, w.Weight_Time";
                            break;
                        case ("V13"):
                            b = "198.1.8.15";
                            GetV = "Select w.Dosing_id,w.Plan_id,w.Equip_code,w.Serial_Num,w.Weight_ID,w.Material_Code, n.Material_name,w.Real_Weight,w.Real_Error, " +
                           "w.Over_Weight,w.Over_Error,w.Waste_Time,w.Warning_Sign,w.Weight_Time,w.Batch_Number,w.Recipe_code from[CWSS_S7].[dbo].[LR_weigh] as w " +
                           "inner join[CWSS_S7].[dbo].[Pmt_material] as n on w.Material_Code = n.Material_code where Plan_id = '" + a.Substring(0, 13).Trim() + "' order by w.serial_num, w.Weight_Time";
                            break;
                        case ("V12"):
                            b = "198.1.8.17";
                            GetV = " Select w.Dosing_id,w.Plan_id,w.Equip_code,w.Serial_Num,w.Weight_ID,w.Material_Code, n.Material_name,w.Real_Weight,w.Real_Error, w.Over_Weight,w.Over_Error,w.Waste_Time,w.Warning_Sign,w.Weight_Time,w.Batch_Number,w.Recipe_code FROM [CWSS_S7].[dbo].[LR_weigh] as w inner join [CWSS_S7].[dbo].[Pmt_material] as n on w.Material_Code = n.Material_code where Plan_id = '" + a.Substring(0, 13).Trim() + "' order by w.serial_num, w.Weight_Time";
                            break;
                        case ("V14"):
                            b = "198.1.8.18";
                            GetV = " Select w.Dosing_id,w.Plan_id,w.Equip_code,w.Serial_Num,w.Weight_ID,w.Material_Code, n.Material_name,w.Real_Weight,w.Real_Error, w.Over_Weight,w.Over_Error,w.Waste_Time,w.Warning_Sign,w.Weight_Time,w.Batch_Number,w.Recipe_code FROM [CWSS_S7].[dbo].[LR_weigh] as w inner join [CWSS_S7].[dbo].[Pmt_material] as n on w.Material_Code = n.Material_code where Plan_id = '" + a.Substring(0, 13).Trim() + "' order by w.serial_num, w.Weight_Time";
                            break;
                    }
                    switch (PingIp(b))
                    {
                        case (1):
                            if (b == "198.1.8.16" || b == "198.1.8.15")
                            {
                                ConnectionString = "Data Source = " + b + "; Initial Catalog = CWSS_S7; User ID = kendakv2; Password = kenda123";
                            }
                            if (b == "198.1.8.17" || b == "198.1.8.18")
                            {
                                ConnectionString = "Data Source = " + b + "; Initial Catalog = CWSS_S7; User ID = kendakv2; Password = kenda123";
                            }
                            break;
                        case (0):
                            ThongBao("Máy đang tắt, vui lòng mở máy và thử lại!!!");
                            break;
                        default:
                            ThongBao("Lỗi kết nối máy chủ, vui lòng thử lại!!!");
                            break;
                    }
                    System.Data.DataTable dtHC = Cnn.ExecuteQuery(ConnectionString, GetV);
                    if (dtHC.Rows.Count == 0)
                    {
                        TextBox1.Text = "Không có dữ liệu";
                        TextBox1.Visible = true;
                        gvHC.DataSource = dtHC;
                        gvHC.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvHC();", true);
                    }
                    else
                    {
                        TextBox1.Visible = false;
                        TempData.tableHC = dtHC;
                        gvHC.DataSource = dtHC;
                        gvHC.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvHC();", true);
                    }
                    break;
                case "R":
                    System.Data.DataTable dt = DoNguoc(a);
                    if (dt?.Rows?.Count > 0)
                    {
                        Comment1.Visible = false;
                        TempData.tableDoNguoc = dt;
                        gvDoNguoc.DataSource = dt;
                        gvDoNguoc.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                    }
                    else
                    {
                        Comment1.Text = "Không có dữ liệu";
                        Comment1.Visible = true;
                        gvDoNguoc.DataSource = dt;
                        gvDoNguoc.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                    }
                    break;
                default:
                    Comment1.Text = "Không có dữ liệu";
                    Comment1.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvDoNguoc();", true);
                    break;
            }
        }
        //protected void gvKQ1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    string id = e.CommandArgument.ToString().Trim();
        //    string ptype = id.Substring(0, 1);
        //    if (e.CommandName == "btnXem")
        //    {
        //        if (ptype == "R")
        //        {
        //            string getdataebe = "SELECT [mesid],[machno],[daylimt],[barcode] ,[slipno],[weight] ,[prodat],[effdat],[class],[ptype],[partno],[intime],[indat],[usrno],[pallet_no] FROM [erp].[dbo].[prdebe] where barcode='" + id + "' order by indat desc";
        //            System.Data.DataTable dtebe = Cnn.ExecuteQuery(ConnectionStringHome, getdataebe);
        //            if (dtebe.Rows.Count == 0)
        //            {
        //                ThongBao("Không có dữ liệu");
        //                gvPrdebe.DataSource = null;
        //                gvPrdebe.DataBind();
        //                return;
        //            }
        //            else
        //            {
        //                TempData.tablePrdebe = dtebe;
        //                gvPrdebe.DataSource = dtebe;
        //                gvPrdebe.DataBind();
        //                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvEbe();", true);

        //            }
        //        }
        //        else
        //        {
        //            //string getdataebc = "Select bacode FROM [erp].[dbo].[prdebc] where barcode='" + id + "' and substring(bacode,1,2) ='RD' ";
        //            //string getdataebc = "Select bacode FROM [erp].[dbo].[prdebc] where barcode='" + id + "'  ";
        //            string getdataebc = "Select bacode FROM [erp].[dbo].[prdebc] where barcode='" + id + "' and substring(bacode,1,2) <> 'RR' ";
        //            System.Data.DataTable dtebc = Cnn.ExecuteQuery(ConnectionStringHome, getdataebc);
        //            string bacode = "";
        //            if (dtebc.Rows.Count == 0)
        //            {
        //                ThongBao("Không có dữ liệu tem " + id + " ở ebc");
        //                return;
        //            }
        //            else
        //            {
        //                foreach (DataRow item in dtebc.Rows)
        //                {
        //                    bacode += "'" + item["bacode"].ToString().Trim() + "',";
        //                }
        //                string barcode = bacode.Remove(bacode.Length - 1);
        //                string getdataebe = "SELECT [mesid],[machno],[daylimt],[barcode] ,[slipno],[weight] ,[prodat],[effdat],[class],[ptype],[partno],[intime],[indat],[usrno],[pallet_no] FROM [erp].[dbo].[prdebe] where barcode in (" + barcode + ") order by indat desc";
        //                System.Data.DataTable dtebe = Cnn.ExecuteQuery(ConnectionStringHome, getdataebe);
        //                if (dtebe.Rows.Count == 0)
        //                {
        //                    ThongBao("Không có dữ liệu");
        //                    gvPrdebe.DataSource = null;
        //                    gvPrdebe.DataBind();
        //                    return;
        //                }
        //                else
        //                {
        //                    TempData.tablePrdebe = dtebe;
        //                    gvPrdebe.DataSource = dtebe;
        //                    gvPrdebe.DataBind();
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvEbe();", true);
        //                }
        //            }

        //        }

        //    }
        //}
        protected void gvHC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string may = e.Row.Cells[1].Text.Trim();

                if (may == "01")
                {
                    e.Row.Cells[1].Text = "-1";
                }
                if (may == "03")
                {
                    e.Row.Cells[1].Text = "-9";
                }
                if (may == "02")
                {
                    e.Row.Cells[1].Text = "-1 Mới";
                }
                if (may == "04")
                {
                    e.Row.Cells[1].Text = "-9 Mới";
                }
            }
        }

        protected void gvHC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            //string MaterialCode = Dulieu[0].Trim();
            //string EquipCode = Dulieu[1].Trim();
            //string RecordTime = Dulieu[2].Substring(0, 10).Trim();
            //string MaterialName = Dulieu[3].Trim();

            string MaterialCode = Dulieu[0].Substring(0, 5).Trim();
            string EquipCode = Dulieu[1].Trim();
            string RecordTime = Dulieu[2].Trim();
            string MaterialName = Dulieu[3].Trim();

            string GetTV = "";
            string ConnectionString = string.Empty;
            string a = string.Empty;
            switch (EquipCode)
            {
                case ("01"):
                    a = "198.1.8.16";
                    GetTV = "SELECT top 1 Scan_State,Equip_Code,Material,Scan_Bar,Scan_Time,Bin FROM [CWSS_S7].[dbo].[LR_BarcodeLog] where [Material] = '" + MaterialName + "' and SUBSTRING(convert(varchar,Scan_bar),1,5) ='" + MaterialCode + "'  and CONVERT(datetime, [Scan_Time], 102) <  CONVERT(DATETIME, '" + RecordTime + "', 102) order by Scan_Time desc";
                    //GetTV = "select Scan_State,Equip_Code,Material,Scan_Bar,Scan_Time,Bin FROM [CWSS_S7].[dbo].[LR_BarcodeLog] where Material='" + MaterialName + "' and  Scan_time like '" + RecordTime + "%' order by Scan_time desc";
                    break;
                case ("03"):
                    a = "198.1.8.15";
                    GetTV = "SELECT top 1 Scan_State,Equip_Code,Material,Scan_Bar,Scan_Time,Bin FROM [CWSS_S7].[dbo].[LR_BarcodeLog] where [Material] = '" + MaterialName + "' and SUBSTRING(convert(varchar,Scan_bar),1,5) ='" + MaterialCode + "'  and CONVERT(datetime, [Scan_Time], 102) <  CONVERT(DATETIME, '" + RecordTime + "', 102) order by Scan_Time desc";
                    //GetTV = "select Scan_State,Equip_Code,Material,Scan_Bar,Scan_Time,Bin FROM [CWSS_S7].[dbo].[LR_BarcodeLog] where Material='" + MaterialName + "' and  Scan_time like '" + RecordTime + "%' order by Scan_time desc";
                    break;
                case ("02"):
                    a = "198.1.8.17";
                    GetTV = "SELECT top 1 Scan_State,Equip_Code,Material,Scan_Bar,Scan_Time,Bin FROM [CWSS_S7].[dbo].[LR_BarcodeLog] where [Material] = '" + MaterialName + "' and SUBSTRING(convert(varchar,Scan_bar),1,5) ='" + MaterialCode + "'  and CONVERT(datetime, [Scan_Time], 102) <  CONVERT(DATETIME, '" + RecordTime + "', 102) order by Scan_Time desc";
                    //GetTV = "select Scan_State,Equip_Code,Material,Scan_Bar,Scan_Time,Bin FROM [CWSS_S7].[dbo].[LR_BarcodeLog] where Material='" + MaterialName + "' and  Scan_time like '" + RecordTime + "%' order by Scan_time desc";

                    break;
                case ("04"):
                    a = "198.1.8.18";
                    GetTV = "SELECT top 1 Scan_State,Equip_Code,Material,Scan_Bar,Scan_Time,Bin FROM [CWSS_S7].[dbo].[LR_BarcodeLog] where [Material] = '" + MaterialName + "' and SUBSTRING(convert(varchar,Scan_bar),1,5) ='" + MaterialCode + "'  and CONVERT(datetime, [Scan_Time], 102) <  CONVERT(DATETIME, '" + RecordTime + "', 102) order by Scan_Time desc";
                    //GetTV = "select Scan_State,Equip_Code,Material,Scan_Bar,Scan_Time,Bin FROM [CWSS_S7].[dbo].[LR_BarcodeLog] where Material='" + MaterialName + "' and  Scan_time like '" + RecordTime + "%' order by Scan_time desc";

                    break;
            }
            switch (PingIp(a))
            {
                case (1):
                    if (a == "198.1.8.16" || a == "198.1.8.15")
                    {
                        ConnectionString = "Data Source = " + a + "; Initial Catalog = CWSS_S7; User ID = kendakv2; Password = kenda123";
                    }
                    if (a == "198.1.8.17" || a == "198.1.8.18")
                    {
                        ConnectionString = "Data Source = " + a + "; Initial Catalog = CWSS_S7; User ID = kendakv2; Password = kenda123";
                    }
                    break;
                case (0):
                    ThongBao("Máy đang tắt, vui lòng mở máy và thử lại!!!");
                    break;
                default:
                    ThongBao("Lỗi kết nối máy chủ, vui lòng thử lại!!!");
                    break;
            }
            if (GetTV == "")
            {
                ThongBao("Không có liệu!!");
                return;
            }
            System.Data.DataTable dtTV = Cnn.ExecuteQuery(ConnectionString, GetTV);
            if (dtTV.Rows.Count == 0)
            {
                TextBox3.Text = "Không có dữ liệu";
                TextBox3.Visible = true;
                gvBonHC.DataSource = dtTV;
                gvBonHC.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvBonHC();", true);
            }
            else
            {
                TextBox3.Visible = false;
                TempData.tableBonHC = dtTV;
                gvBonHC.DataSource = dtTV;
                gvBonHC.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvBonHC();", true);
            }
        }

        protected void gvBonHC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string may = e.Row.Cells[1].Text.Trim();


                if (may == "01")
                {
                    e.Row.Cells[1].Text = "-1";
                }
                if (may == "03")
                {
                    e.Row.Cells[1].Text = "-9";
                }
                if (may == "02")
                {
                    e.Row.Cells[1].Text = "-1 Mới";
                }
                if (may == "04")
                {
                    e.Row.Cells[1].Text = "-9 Mới";
                }
            }
        }

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    string strSearch = txtSearch.Text.Trim();
        //    if (strSearch == "")
        //    {
        //        ThongBao("Vui lòng nhập tên keo hoặc tem quét!!!");
        //        return;
        //    }
        //    else
        //    {
        //        string getData = "SELECT subno,factory,mesid,machno,daylimt,barcode,slipno,weight,prodat,effdat,class,ptype,status,partno,intime,indat,usrno,pallet_no,active " +
        //                        "FROM [erp].[dbo].[prdebe] " +
        //                        "where barcode='" + strSearch + "' or pallet_no='" + strSearch + "'";
        //        System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, getData);
        //        Sum1.Text = dt.Rows.Count.ToString().Trim();
        //        if (dt.Rows.Count == 0)
        //        {
        //            ThongBao("Không có dữ liệu!!!");
        //            gvSearch.DataSource = null;
        //            gvSearch.DataBind();
        //        }
        //        else
        //        {
        //            int i = 0;
        //            string[] sPrinterList = new string[System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count];
        //            if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count == 0)
        //                sPrinterList[i] = "";
        //            else
        //            {
        //                //|| printers.Substring(printers.Length - 3, 3) == "05)" || printers.Substring(printers.Length - 3, 3) == "121"
        //                drMayIn1.Items.Clear();
        //                foreach (string printers in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
        //                {
        //                    if (printers.Substring(printers.Length - 3, 3) == "193" || printers.Substring(printers.Length - 3, 3) == "176" || printers.Substring(printers.Length - 3, 3) == "122")
        //                    {
        //                        sPrinterList[i] = printers;
        //                        i++;
        //                        drMayIn1.Items.Add(printers);
        //                    }
        //                }
        //                if (i == 0)
        //                {
        //                    string r = "Vui lòng kết nối máy in!!!";
        //                    drMayIn1.Items.Add(r);
        //                }
        //            }
        //            gvSearch.DataSource = dt;
        //            gvSearch.DataBind();
        //            ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvSearch();", true);
        //        }
        //    }
        //}

        protected void gvSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string daylimt = Dulieu[0].ToString().Trim();
            string barcode = Dulieu[1].Trim();
            string loaikeo = Dulieu[1].ToString().Trim().Substring(0, 2);
            string slipno = Dulieu[2].ToString().Trim();
            string soluong = Dulieu[3].ToString().Trim();
            string effdat = Dulieu[4].ToString().Trim();
            string partno = Dulieu[5].ToString().Trim();
            string intime = Dulieu[6].ToString().Trim();
            string indat = Dulieu[7].ToString().Trim();
            string pallet = Dulieu[8].ToString().Trim();
            string ca = Dulieu[9].Trim();
            string mesid = Dulieu[10].Trim();
            string Machno = Dulieu[11].ToString().Substring(6, 2);
            string pday = Dulieu[12].Trim();
            string printer = drMayIn1.SelectedValue.ToString();

            string maso = "";
            string ConnectionStringKeoRe = "Data Source=198.1.9.186;Initial Catalog=InTem;User ID=kendakv2;Password=kenda123";
            string sqlKEORE = "select mesid from KEORE where factory='V' and mesid ='" + mesid + "' and machno = '" + Machno + "'";
            System.Data.DataTable dtKEORE = new System.Data.DataTable();
            dtKEORE = Cnn.ExecuteQuery(ConnectionStringKeoRe, sqlKEORE);
            if (dtKEORE.Rows.Count > 0)
            {
                string sqlBarcode = "SELECT * FROM prdebe where factory='V' and prodat='" + pday + "' and partno='" + partno + "'" +
                    " and class='" + ca + "' and barcode <= '" + barcode + "' order by intime";
                System.Data.DataTable dtBar = Cnn.ExecuteQuery(ConnectionStringHome, sqlBarcode);
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

                    string sqlweight = "SELECT weight FROM KEORE where factory='V' and mesid ='" + mesid + "' and machno='" + Machno + "'"; //lấy Barcode từ trong sql
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
            if (drMayin.Text == "Vui lòng kết nối máy in!!!" || drMayin.Text == "Không tìm thấy máy in thích hợp!!!" || drMayin.Text == "")
            {

                ThongBao("Vui lòng kiểm tra kết nối máy in, và thử lại sau!!!");
            }
            else
            {
                PrintExcel(Machno, loaikeo, ca, pallet, daylimt, indat, intime, slipno, soluong, effdat, partno, maso, barcode, printer);
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

            if (txtTimkiem.Text == "")
            {
                try
                {
                    LoadData();
                    DataTable dt = TempData.tableMES;
                    dt.Columns.RemoveAt(9);
                    exportExcel(dt, "MES");
                }
                catch (Exception)
                {
                    ThongBao("Không có dữ liệu để xuất Excel!!!");
                    return;
                }
            }
            else
            {
                try
                {
                    LoadDataTimkiem();
                    DataTable dt = TempData.tableMES;
                    dt.Columns.RemoveAt(9);
                    exportExcel(dt, "MES");
                }
                catch (Exception)
                {
                    ThongBao("Không có dữ liệu để xuất Excel!!!");
                    return;
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
                if (type == "MES")
                {
                    fileName = "San luong BB " + txtFromDay.Text + " - " + txtToday.Text + ".xlsx";
                }
                else
                {
                    fileName = "San luong chi tiet MES.xlsx";
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

        protected void btnExcelNguyenLieu_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableDetail, "Detail");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void btnExcelDoNguoc_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableDoNguoc, "Detail");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void btnExcelHC_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableHC, "Detail");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void btnExcelBonHC_Click(object sender, EventArgs e)
        {
            try
            {
                exportExcel(TempData.tableBonHC, "Detail");
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
                exportExcel(TempData.prdebe, "Detail");
            }
            catch (Exception)
            {
                ThongBao("Không có dữ liệu để xuất Excel!!!");
                return;
            }
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (txtFromDay.Text.Trim().ToString() == "" || txtToday.Text.Trim().ToString() == "")
            {
                ThongBao("Vui lòng nhập ngày!!!");
                gvKQ.DataSource = null;
                gvKQ.DataBind();
                return;
            }


            if (drMay.SelectedValue == "tatca")
            {
                string dateString1 = txtFromDay.Text.Trim().ToString();
                string dateString2 = txtToday.Text.Trim().ToString();

                DateTime date1 = DateTime.Parse(dateString1);
                DateTime date2 = DateTime.Parse(dateString2);

                TimeSpan difference = date2 - date1;

                int daysDifference = difference.Days;
                if (daysDifference > 2)
                {
                    ThongBao("Không thể xem quá 2 ngày");
                    return;

                }
                else
                {
                    xemlieutatcatimkiem();
                    
                }



            }
            else
            {
                LoadDataTimkiem();
            }
        }
        private void LoadDataTimkiem()
        {
            try
            {
                string makeo = txtTimkiem.Text.ToString().Trim();
                string fromDay = txtFromDay.Text.ToString().Replace("-", "");
                string toDay = txtToday.Text.ToString().Replace("-", "");
                if (fromDay == "" || toDay == "")
                {
                    ThongBao("Vui lòng nhập ngày!!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                System.Data.DataTable result = new System.Data.DataTable();
                result.Columns.Add("Mã mesid");
                result.Columns.Add("Số máy");
                result.Columns.Add("Tên keo");
                result.Columns.Add("Số lô");
                result.Columns.Add("Số mẻ điều động");
                result.Columns.Add("Số mẻ hoàn thành");
                result.Columns.Add("Số ký tiêu chuẩn");
                result.Columns.Add("Số ký đã quét tem");
                result.Columns.Add("Số ký hoàn thành");
                result.Columns.Add("Số ký chênh lệch");
                result.Columns.Add("Xem liệu");

                string May = drMay.SelectedValue;
                if (May == "")
                {
                    ThongBao("Vui lòng chọn máy!!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                string ConnectionString = ChonMay(May);
                if (ConnectionString == "")
                {
                    ThongBao("Máy đang tắt, vui lòng mở máy và thử lại!!!");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                string getData = "select k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno,cast(SUM(p.weight)as int) as wgt from[erp].[dbo].[prdebe] as p " +
                    "INNER JOIN[198.1.9.186].[InTem].[dbo].[KEORE] as k " +
                    "ON p.mesid = k.mesid" +
                    " where (k.pday between  '" + fromDay + "' and '" + toDay + "') and k.idGrouplot != '' and p.mesid like'V%' and p.factory = 'V' and p.machno Like'%" + May + "' and  k.recipe_name like '%" + makeo + "%'" +
                    "group by k.idGrouplot,p.mesid,p.machno,p.partno,p.slipno";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, getData);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu tìm kiếm máy " + May);
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                string grlotid = "";
                foreach (DataRow item in dt.Rows)
                {
                    grlotid += "'" + item["idGrouplot"].ToString().Trim() + "',";
                }
                string GetTc = "SELECT a.id, a.RecipeCode, a.SetNumber, a.FinishNum,cast(SUM(b.set_weight) as int) as SKTC,cast(a.FinishNum*SUM(b.set_weight) as int) as Swgt" +
                    " FROM [mfns].[dbo].[Ppt_GroupLot] a,[mfns].[dbo].[pmt_weigh] b where a.RecipeCode = b.father_code and id in (" + grlotid.Remove(grlotid.Length - 1) + ") " +
                    " group by id,RecipeCode,FinishNum,SetNumber";
                System.Data.DataTable dtTc = Cnn.ExecuteQuery(ConnectionString, GetTc);
                if (dtTc.Rows.Count == 0)
                {
                    ThongBao("idGrouplot không tìm thấy, vui lòng liên hệ IT !!!");
                }
                foreach (DataRow item in dt.Rows)
                {
                    foreach (DataRow item1 in dtTc.Rows)
                    {
                        if (item["idGrouplot"].ToString().Trim() == item1["id"].ToString().Trim())
                        {
                            result.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["machno"].ToString().Trim().Substring(6,2), item["partno"].ToString().Trim(),item["slipno"].ToString().Trim(),
                                    item1["SetNumber"].ToString().Trim(),item1["FinishNum"].ToString().Trim(),item1["SKTC"].ToString().Trim(), item["wgt"].ToString().Trim(),item1["Swgt"].ToString().Trim(),
                                    int.Parse(item1["Swgt"].ToString().Trim())-int.Parse(item["wgt"].ToString().Trim()),""});
                        }
                    }
                }

                TempData.tableMES = result;

                gvKQ.DataSource = result;
                gvKQ.DataBind();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
                return;
            }
        }
    }
}