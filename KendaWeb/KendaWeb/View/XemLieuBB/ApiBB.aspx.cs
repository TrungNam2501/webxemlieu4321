using ClosedXML.Excel;
using DocumentFormat.OpenXml.Math;
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
    public partial class ApiBB : System.Web.UI.Page
    {
        string ConnectionStringHome = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=erp;User ID=kendakv2;Password=kenda123;";
        string planId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/View/Loggin.aspx");
            }

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
            LoadData();
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
            LoadDataTimkiem();
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

        protected void gvKQ_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string id = Dulieu[0].Trim();
            string May = Dulieu[1].Trim();
            switch (e.CommandName)
            {
                case ("btnIn"):
                    string ConnectionString = ChonMay(May);
                    string GetPlanId = "SELECT Plan_ID,RecipeCode FROM [mfns].[dbo].[Ppt_GroupLot] where MesPlanID = '" + id.Trim() + "'";
                    System.Data.DataTable dtPlanId = Cnn.ExecuteQuery(ConnectionString, GetPlanId);
                    //string recipename = dtPlanId.Rows[0]["RecipeCode"].ToString().Trim();
                    planId = dtPlanId.Rows[0]["Plan_ID"].ToString().Trim();
                    System.Data.DataTable dtIn = new DataTable();
                   
                    if (May == "03" || May == "05" || May == "01" || May == "02" || May == "07" || May == "06" || May == "04" || May == "08")
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
                                       " AND (Mater_Code NOT LIKE '60%' OR (Mater_Code LIKE '60%' AND (Mater_Code = Mater_Name OR Mater_Type = 1))) ORDER BY SaveTime;";
                                       //"AND (Mater_Code NOT LIKE '60%' " +
                                       //"OR (Mater_Code LIKE '60%' AND Mater_Code = Mater_Name)) " +
                                       //"ORDER BY SaveTime;";

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
                                //string coal_code = "";
                                //for (int _icount = 0; _icount < dt_check_old_coal.Rows.Count; _icount++)
                                //{
                                //    if (dt_check_old_coal.Rows[_icount]["weigh_type"].ToString().Trim() == "油料")
                                //    {
                                //        coal_code = dt_check_old_coal.Rows[_icount]["mater_code"].ToString().Trim();
                                //        break;
                                //    }
                                //}


                                string coal_code = "";

                                for (int _icount = 0; _icount < dt_check_old_coal.Rows.Count; _icount++)
                                {
                                    if (dt_check_old_coal.Rows[_icount]["weigh_type"].ToString().Trim() == "油料")
                                    {
                                        string materCode = dt_check_old_coal.Rows[_icount]["mater_code"].ToString().Trim();

                                        // Kiểm tra nếu chưa có thì mới thêm vào
                                        if (!coal_code.Contains($"'{materCode}'"))
                                        {
                                            if (coal_code == "")
                                            {
                                                coal_code = $"'{materCode}'";
                                            }
                                            else
                                            {
                                                coal_code += $",'{materCode}'";
                                            }
                                        }
                                    }
                                }




                                //string s_coal_barcode = "  SELECT top 1 [Mater_Barcode],[SaveTime] FROM [mfns].[dbo].[Ppt_Oil] where  SaveTime <= '" + s_fromday + "' and Mater_Type = '0' and [Mater_Code] ='" + coal_code + "' order by SaveTime desc";


                                // 1) Lấy barcode mới nhất cho MỖI Mater_Code và TRẢ luôn Mater_Code để map
                                string s_coal_barcode = @"
                                                        SELECT Mater_Code, Mater_Barcode, SaveTime
                                                        FROM (
                                                            SELECT Mater_Code, Mater_Barcode, SaveTime,
                                                                   ROW_NUMBER() OVER (PARTITION BY Mater_Code ORDER BY SaveTime DESC) AS rn
                                                            FROM [mfns].[dbo].[Ppt_Oil]
                                                            WHERE SaveTime <= '" + s_fromday + @"'
                                                              AND Mater_Type = '0'
                                                              AND Mater_Code IN (" + coal_code + @")
                                                        ) t
                                                        WHERE rn = 1";

                                DataTable dt_coal_barcode = Cnn.ExecuteQuery(ConnectionString, s_coal_barcode);

                                // 2) Map: Mater_Code -> Mater_Barcode (mỗi mã 1 barcode gần nhất)
                                var barcodeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                                foreach (DataRow r in dt_coal_barcode.Rows)
                                {
                                    string mcode = r["Mater_Code"]?.ToString().Trim() ?? "";
                                    string mbar = r["Mater_Barcode"]?.ToString().Trim() ?? "";
                                    if (!string.IsNullOrEmpty(mcode) && !string.IsNullOrEmpty(mbar))
                                    {
                                        // nếu trùng khóa, giữ cái đầu vì đã là mới nhất (rn=1)
                                        if (!barcodeMap.ContainsKey(mcode)) barcodeMap[mcode] = mbar;
                                    }
                                }

                                DataRow dr;
                                int i_max = dt_check_old_coal.Rows.Count;

                                for (int i_count = 0; i_count < i_max; i_count++)
                                {
                                    dr = dtIn.NewRow();

                                    dr[0] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim();  // time
                                    dr[1] = dtIn.Rows[0][1].ToString().Trim();                                 // recipe
                                    dr[2] = dtIn.Rows[0][2].ToString().Trim();                                 // setnum

                                    // serial num: 2 ký tự cuối của barcode cũ
                                    string oldBar = dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim();
                                    int serial = 0;
                                    if (!string.IsNullOrEmpty(oldBar) && oldBar.Length >= 2)
                                    {
                                        int.TryParse(oldBar.Substring(oldBar.Length - 2, 2), out serial);
                                    }
                                    dr[3] = serial;                                                            // serial num

                                    string curMater = dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim();
                                    dr[4] = curMater;                                                          // mater code
                                    dr[5] = dt_check_old_coal.Rows[i_count]["mater_name"].ToString().Trim();   // mater name

                                    // 3) Gán barcode theo đúng mã vật liệu hiện tại
                                    if (barcodeMap.TryGetValue(curMater, out var matchedBarcode) && !string.IsNullOrEmpty(matchedBarcode))
                                    {
                                        // an toàn độ dài: cần >= 13 để Substring(7,6) hợp lệ (và có 5 ký tự đầu là mã)
                                        if (matchedBarcode.Length >= 13 && matchedBarcode.StartsWith(curMater, StringComparison.OrdinalIgnoreCase))
                                        {
                                            dr[6] = matchedBarcode;                             // Mater barcode -- Coal
                                            dr[7] = "20" + matchedBarcode.Substring(7, 6);      // batchno
                                        }
                                        else
                                        {
                                            dr[6] = "";                                         // Mater barcode -- Oil
                                            dr[7] = dtIn.Rows[0][7].ToString().Trim();          // batchno fallback
                                        }
                                    }
                                    else
                                    {
                                        dr[6] = "";                                             // không có barcode khớp
                                        dr[7] = dtIn.Rows[0][7].ToString().Trim();              // batchno fallback
                                    }

                                    dr[8] = dtIn.Rows[0][8].ToString().Trim();                 // equip id
                                    dr[9] = dt_check_old_coal.Rows[i_count]["set_weight"].ToString();   // set weight
                                    dr[10] = dt_check_old_coal.Rows[i_count]["error_allow"].ToString();  // error low

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
                            Comment1.Text = "Không có dữ liệu, barcode này là barcode không chạy theo YAML";

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
                            Comment1.Text = "Không có dữ liệu, barcode này là barcode không chạy theo YAML";

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
                            Comment1.Text = "Không có dữ liệu, barcode này là barcode không chạy theo YAML";

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
        private System.Data.DataTable DoNguoc(string barcode)
        {
            string GetData = "select  mesid,machno,prodat,partno,indat  FROM [erp].[dbo].[prdebe] where barcode ='"+barcode+"' and LEFT(mesid, 1) NOT IN ('V', 'E','') ";
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
                string partno = sql.Rows[0]["partno"].ToString().Trim();
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
                    string maydonguoc = machno;

                    string connectdonguoc = ChonMay(maydonguoc);


                    string GetPlanIddonguoc = "SELECT Plan_ID FROM [mfns].[dbo].[Ppt_GroupLot] where MesPlanID = '" + mesid + "'";
                    System.Data.DataTable dtPlanId = Cnn.ExecuteQuery(ConnectionString, GetPlanIddonguoc);
                    string planidlumlua = dtPlanId.Rows[0]["Plan_ID"].ToString().Trim();
                    System.Data.DataTable dt = new DataTable();
                    
                    string recipename = partno;
                    if (maydonguoc == "03" || maydonguoc == "05" || maydonguoc == "01" || maydonguoc == "02" || maydonguoc == "07" || maydonguoc == "06" || maydonguoc == "04" || maydonguoc == "08")
                    {
                       
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
                                     " AND (Mater_Code NOT LIKE '60%' OR (Mater_Code LIKE '60%' AND (Mater_Code = Mater_Name OR Mater_Type = 1))) ORDER BY SaveTime;";
                                     //"AND (Mater_Code NOT LIKE '60%' " +
                                     //"OR (Mater_Code LIKE '60%' AND Mater_Code = Mater_Name)) " +
                                     //"ORDER BY SaveTime;";




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
                                //string coal_code = "";
                                //for (int _icount = 0; _icount < dt_check_old_coal.Rows.Count; _icount++)
                                //{
                                //    if (dt_check_old_coal.Rows[_icount]["weigh_type"].ToString().Trim() == "油料")
                                //    {
                                //        coal_code = dt_check_old_coal.Rows[_icount]["mater_code"].ToString().Trim();
                                //        break;
                                //    }
                                //}
                                string coal_code = "";

                                for (int _icount = 0; _icount < dt_check_old_coal.Rows.Count; _icount++)
                                {
                                    if (dt_check_old_coal.Rows[_icount]["weigh_type"].ToString().Trim() == "油料")
                                    {
                                        string materCode = dt_check_old_coal.Rows[_icount]["mater_code"].ToString().Trim();

                                        // Kiểm tra nếu chưa có thì mới thêm vào
                                        if (!coal_code.Contains($"'{materCode}'"))
                                        {
                                            if (coal_code == "")
                                            {
                                                coal_code = $"'{materCode}'";
                                            }
                                            else
                                            {
                                                coal_code += $",'{materCode}'";
                                            }
                                        }
                                    }
                                }





                               // string s_coal_barcode = "  SELECT top 1 [Mater_Barcode],[SaveTime] FROM [mfns].[dbo].[Ppt_Oil] where  SaveTime <= '" + s_fromday + "' and Mater_Type = '0' and [Mater_Code] ='" + coal_code + "' order by SaveTime desc";

                                string s_coal_barcode = @"
                                 SELECT Mater_Code, Mater_Barcode, SaveTime
                                 FROM (
                                     SELECT Mater_Code, Mater_Barcode, SaveTime,
                                            ROW_NUMBER() OVER (PARTITION BY Mater_Code ORDER BY SaveTime DESC) AS rn
                                     FROM [mfns].[dbo].[Ppt_Oil]
                                     WHERE SaveTime <= '" + s_fromday + @"'
                                       AND Mater_Type = '0'
                                       AND Mater_Code IN (" + coal_code + @")
                                 ) t
                                 WHERE rn = 1";




                                DataTable dt_coal_barcode = Cnn.ExecuteQuery(ConnectionString, s_coal_barcode);
                                var barcodeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                                foreach (DataRow r in dt_coal_barcode.Rows)
                                {
                                    string mcode = r["Mater_Code"]?.ToString().Trim() ?? "";
                                    string mbar = r["Mater_Barcode"]?.ToString().Trim() ?? "";
                                    if (!string.IsNullOrEmpty(mcode) && !string.IsNullOrEmpty(mbar))
                                    {
                                        // nếu trùng khóa, giữ cái đầu vì đã là mới nhất (rn=1)
                                        if (!barcodeMap.ContainsKey(mcode)) barcodeMap[mcode] = mbar;
                                    }
                                }



                                DataRow dr;
                                int i_max = dt_check_old_coal.Rows.Count;

                                for (int i_count = 0; i_count < i_max; i_count++)
                                {
                                    dr = dt.NewRow();

                                    dr[0] = dt_check_old_coal.Rows[i_count]["weigh_time"].ToString().Trim();  // time
                                    dr[1] = dt.Rows[0][1].ToString().Trim();                                 // recipe
                                    dr[2] = dt.Rows[0][2].ToString().Trim();                                 // setnum

                                    // serial num: 2 ký tự cuối của barcode cũ
                                    string oldBar = dt_check_old_coal.Rows[i_count]["barcode"].ToString().Trim();
                                    int serial = 0;
                                    if (!string.IsNullOrEmpty(oldBar) && oldBar.Length >= 2)
                                    {
                                        int.TryParse(oldBar.Substring(oldBar.Length - 2, 2), out serial);
                                    }
                                    dr[3] = serial;                                                            // serial num

                                    string curMater = dt_check_old_coal.Rows[i_count]["mater_code"].ToString().Trim();
                                    dr[4] = curMater;                                                          // mater code
                                    dr[5] = dt_check_old_coal.Rows[i_count]["mater_name"].ToString().Trim();   // mater name

                                    // 3) Gán barcode theo đúng mã vật liệu hiện tại
                                    if (barcodeMap.TryGetValue(curMater, out var matchedBarcode) && !string.IsNullOrEmpty(matchedBarcode))
                                    {
                                        // an toàn độ dài: cần >= 13 để Substring(7,6) hợp lệ (và có 5 ký tự đầu là mã)
                                        if (matchedBarcode.Length >= 13 && matchedBarcode.StartsWith(curMater, StringComparison.OrdinalIgnoreCase))
                                        {
                                            dr[6] = matchedBarcode;                             // Mater barcode -- Coal
                                            dr[7] = "20" + matchedBarcode.Substring(7, 6);      // batchno
                                        }
                                        else
                                        {
                                            dr[6] = "";                                         // Mater barcode -- Oil
                                            dr[7] = dt.Rows[0][7].ToString().Trim();          // batchno fallback
                                        }
                                    }
                                    else
                                    {
                                        dr[6] = "";                                             // không có barcode khớp
                                        dr[7] = dt.Rows[0][7].ToString().Trim();              // batchno fallback
                                    }

                                    dr[8] = dt.Rows[0][8].ToString().Trim();                 // equip id
                                    dr[9] = dt_check_old_coal.Rows[i_count]["set_weight"].ToString();   // set weight
                                    dr[10] = dt_check_old_coal.Rows[i_count]["error_allow"].ToString();  // error low

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

        protected void btnExcelDoNguoc_Click(object sender, EventArgs e)
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

       
        private void LoadDataTimkiem()
        {
            try
            {
                string makeo = txtTimkiem.Text.ToString().Trim();
                string fromDay = txtFromDay.Text.ToString();
                string toDay = txtToday.Text.ToString();
                string fromDay1 = txtFromDay.Text.ToString().Replace("-", "");
                string toDay1 = txtToday.Text.ToString().Replace("-", "");
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
                string getData = "SELECT mesid, machno, partno, slipno, CAST(SUM(weight) AS INT) AS wgt" +
                    " FROM [erp].[dbo].[prdebe] WHERE prodat BETWEEN '" + fromDay1 + "' AND '" + toDay1 + "' " +
                    "AND machno LIKE '%" + May + "' and partno like '%"+makeo+"%' AND LEFT(mesid, 1) NOT IN ('V', 'E', '') " +
                    "GROUP BY mesid, machno, partno, slipno;";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, getData);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu máy " + May);
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                string GetTc = "SELECT a.Equip_ID, " +
                                "       a.Plan_Id, " +
                                "       a.Recipe_Code, " +
                                "       CASE " +
                                "           WHEN a.Shift_Id = 1 THEN " +
                                "               '2' " +
                                "           ELSE " +
                                "               '1' " +
                                "       END + RIGHT('0' + CAST(a.Equip_ID AS VARCHAR), 2) + '-' + SUBSTRING(a.P_Date, 6, 2) + SUBSTRING(a.P_Date, 9, 2) AS solo, " +
                                "       a.Plan_Num, " +
                                "       b.FinishNum, " +
                                "       CAST(SUM(c.set_weight) AS INT) AS SokgTC, " +
                                "       cast(b.FinishNum * SUM(c.set_weight) as int) as SokgTotal " +
                                "FROM [mfnsShareDB].[dbo].[IF_RtPlan2Mixing] a " +
                                "    JOIN [mfns].[dbo].[Ppt_GroupLot] b " +
                                "        ON a.Plan_Id = b.MesPlanID " +
                                "    JOIN [mfns].[dbo].[pmt_weigh] c " +
                                "        ON b.RecipeCode = c.father_code " +
                                "WHERE LEFT(a.Plan_Id, 1) != 'V' " +
                                "      AND b.FinishNum != '0'         " +
                                "      AND a.P_Date " +
                                "      BETWEEN '" + fromDay + "' AND '" + toDay + "' " +
                                "GROUP BY a.Equip_ID, " +
                                "         a.Plan_Id, " +
                                "         a.Recipe_Code, " +
                                "         a.Shift_Id, " +
                                "         a.P_Date, " +
                                "         a.Plan_Num, " +
                                "         b.FinishNum ";
                System.Data.DataTable dtTc = Cnn.ExecuteQuery(ConnectionString, GetTc);
                if (dtTc.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu !!! gọi IT hoặc thử lại");
                }
                foreach (DataRow item in dt.Rows)
                {
                    foreach (DataRow item1 in dtTc.Rows)
                    {
                        if (item["mesid"].ToString().Trim() == item1["Plan_Id"].ToString().Trim())
                        {
                            result.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["machno"].ToString().Trim().Substring(6,2), item["partno"].ToString().Trim(),item["slipno"].ToString().Trim(),
                                    item1["Plan_Num"].ToString().Trim(),item1["FinishNum"].ToString().Trim(),item1["SokgTC"].ToString().Trim(), item["wgt"].ToString().Trim(),item1["SokgTotal"].ToString().Trim(),
                                    int.Parse(item1["SokgTotal"].ToString().Trim())-int.Parse(item["wgt"].ToString().Trim()),""});
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

        private void LoadData()
        {
            try
            {
                string makeo = txtTimkiem.Text.ToString().Trim();
                string fromDay = txtFromDay.Text.ToString();
                string toDay = txtToday.Text.ToString();
                string fromDay1 = txtFromDay.Text.ToString().Replace("-", "");
                string toDay1 = txtToday.Text.ToString().Replace("-", "");
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
                string getData = "SELECT mesid, machno, partno, slipno, CAST(SUM(weight) AS INT) AS wgt" +
                    " FROM [erp].[dbo].[prdebe] WHERE prodat BETWEEN '" + fromDay1 + "' AND '" + toDay1 + "' " +
                    "AND machno LIKE '%"+ May + "' AND LEFT(mesid, 1) NOT IN ('V', 'E', '') " +
                    "GROUP BY mesid, machno, partno, slipno;";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, getData);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu máy " + May);
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
                string  GetTc = "SELECT a.Equip_ID, " +
                                "       a.Plan_Id, " +
                                "       a.Recipe_Code, " +
                                "       CASE " +
                                "           WHEN a.Shift_Id = 1 THEN " +
                                "               '2' " +
                                "           ELSE " +
                                "               '1' " +
                                "       END + RIGHT('0' + CAST(a.Equip_ID AS VARCHAR), 2) + '-' + SUBSTRING(a.P_Date, 6, 2) + SUBSTRING(a.P_Date, 9, 2) AS solo, " +
                                "       a.Plan_Num, " +
                                "       b.FinishNum, " +
                                "       CAST(SUM(c.set_weight) AS INT) AS SokgTC, " +
                                "       cast(b.FinishNum * SUM(c.set_weight) as int) as SokgTotal " +
                                "FROM [mfnsShareDB].[dbo].[IF_RtPlan2Mixing] a " +
                                "    JOIN [mfns].[dbo].[Ppt_GroupLot] b " +
                                "        ON a.Plan_Id = b.MesPlanID " +
                                "    JOIN [mfns].[dbo].[pmt_weigh] c " +
                                "        ON b.RecipeCode = c.father_code " +
                                "WHERE LEFT(a.Plan_Id, 1) != 'V' " +
                                "      AND b.FinishNum != '0'         " +
                                "      AND a.P_Date " +
                                "      BETWEEN '"+fromDay+"' AND '"+toDay+"' " +
                                "GROUP BY a.Equip_ID, " +
                                "         a.Plan_Id, " +
                                "         a.Recipe_Code, " +
                                "         a.Shift_Id, " +
                                "         a.P_Date, " +
                                "         a.Plan_Num, " +
                                "         b.FinishNum ";
                System.Data.DataTable dtTc = Cnn.ExecuteQuery(ConnectionString, GetTc);
                if (dtTc.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu !!! gọi IT hoặc thử lại");
                }
                foreach (DataRow item in dt.Rows)
                {
                    foreach (DataRow item1 in dtTc.Rows)
                    {
                        if (item["mesid"].ToString().Trim() == item1["Plan_Id"].ToString().Trim())
                        {
                            result.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["machno"].ToString().Trim().Substring(6,2), item["partno"].ToString().Trim(),item["slipno"].ToString().Trim(),
                                    item1["Plan_Num"].ToString().Trim(),item1["FinishNum"].ToString().Trim(),item1["SokgTC"].ToString().Trim(), item["wgt"].ToString().Trim(),item1["SokgTotal"].ToString().Trim(),
                                    int.Parse(item1["SokgTotal"].ToString().Trim())-int.Parse(item["wgt"].ToString().Trim()),""});
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
                case ("08"):
                    a = "198.1.8.38";
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
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
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
    }
}