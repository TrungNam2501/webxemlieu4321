using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KendaWeb.View.XemlieuPLC
{
    public partial class XemlieuPLCnew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (txtFromDay.Text == "" || txtToday.Text == "")
            {
                string strdate = DateTime.Now.ToString("yyyy-MM-dd");
                txtFromDay.Text = strdate;
                txtToday.Text = strdate;
            }
        }
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }




        protected void btnTruylieumes_Click(object sender, EventArgs e)
        {
            txtTimkiem.Text = "";
            if (drChonmay.SelectedValue == "")
            {
                xemlieutatca();

            }
            else
            {
                xemlieutungmay();
            }
            
        }
        private void xemlieutungmay()
        {
            try
            {
                string may = "";
                string maytam = drChonmay.SelectedValue;
                if (maytam != "")
                {
                    may = "and machno = '" + maytam + "' ";
                }
                string tungay = txtFromDay.Text.Trim().Replace("-", "");
                string denngay = txtToday.Text.Trim().Replace("-", "");
                if (tungay == "" || denngay == "")
                {
                    ThongBao("Vui lòng chọn ngày đầy đủ");
                    gvMesid.DataSource = null;
                    gvMesid.DataBind();
                    return;
                }
                System.Data.DataTable dtmesidall = new System.Data.DataTable();
                dtmesidall.Columns.Add("mesid");
                dtmesidall.Columns.Add("recipe_name");
                dtmesidall.Columns.Add("machno");
                dtmesidall.Columns.Add("weight");
                dtmesidall.Columns.Add("finishnum");
                dtmesidall.Columns.Add("indat");
                dtmesidall.Columns.Add("intime");
                dtmesidall.Columns.Add("idGrouplot");
                dtmesidall.Columns.Add("FinishTag");
                string getDataMesBB = "SELECT [mesid],[recipe_name],[weight],[machno],[indat],[intime],[idGrouplot] FROM [InTem].[dbo].[KEORE] where (indat between '" + tungay + "' and '" + denngay + "') and idGrouplot!= ''   " + may + "  order by mesid asc, indat asc , intime asc";
                string ConnectionString = "Data Source = 198.1.9.186; Initial Catalog = InTem; User ID = kendakv2; Password = kenda123";
                System.Data.DataTable dtmesBB = Cnn.ExecuteQuery(ConnectionString, getDataMesBB);

                if (dtmesBB.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    return;
                }
                System.Data.DataTable dtfinish = new System.Data.DataTable();
                dtfinish.Columns.Add("finishnum");
                dtfinish.Columns.Add("recipe");
                dtfinish.Columns.Add("Id");
                dtfinish.Columns.Add("FinishTag");
                foreach (DataRow item in dtmesBB.Rows)
                {
                    try
                    {
                        string idGrouplot = item["idGrouplot"].ToString().Trim();
                        string may123 = item["machno"].ToString().Trim();
                        string recipename = item["recipe_name"].ToString().Trim().Replace("-", "");
                        string mesidd = item["mesid"].ToString().Trim();
                        string Connectfinistnum = "";


                        switch (may123)
                        {
                            case ("01"):

                                Connectfinistnum = "Data Source = 198.1.8.21; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("02"):
                                Connectfinistnum = "Data Source = 198.1.8.22; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("03"):
                                Connectfinistnum = "Data Source = 198.1.8.23; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("04"):
                                Connectfinistnum = "Data Source = 198.1.8.24; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("05"):
                                Connectfinistnum = "Data Source = 198.1.8.35; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("06"):
                                Connectfinistnum = "Data Source = 198.1.8.36; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("07"):
                                Connectfinistnum = "Data Source = 198.1.8.37; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                        }

                        string Getdatafinish = "SELECT [FinishNum],[RecipeCode],[Id],[FinishTag] FROM [mfns].[dbo].[Ppt_GroupLot] where Id='" + idGrouplot + "' and RecipeCode='" + recipename + "'";

                        System.Data.DataTable finishnum = Cnn.ExecuteQuery(Connectfinistnum, Getdatafinish);
                        if (finishnum.Rows.Count == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (recipename == finishnum.Rows[0][1].ToString().Trim())
                            {
                                dtfinish.Rows.Add(new object[] { finishnum.Rows[0][0].ToString(), finishnum.Rows[0][1].ToString(), finishnum.Rows[0][2].ToString(), finishnum.Rows[0][3].ToString() });

                            }
                            else
                            {
                               continue;
                            }


                        }
                    }
                    catch(Exception ex)
                    {
                        continue;
                    }
                   
                }
                foreach (DataRow item in dtmesBB.Rows)
                {
                    foreach (DataRow item1 in dtfinish.Rows)
                    {
                        if (item["recipe_name"].ToString().Trim().Replace("-", "") == item1["recipe"].ToString().Trim() && item["idGrouplot"].ToString().Trim() == item1["Id"].ToString().Trim())
                        {
                            dtmesidall.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["recipe_name"].ToString().Trim(), item["machno"].ToString().Trim(), item["weight"].ToString().Trim(), item1["finishnum"].ToString().Trim(), item["indat"].ToString().Trim(), item["intime"].ToString().Trim(), item["idGrouplot"].ToString().Trim(), item1["FinishTag"].ToString().Trim() });
                        }
                    }
                }
                gvMesid.DataSource = dtmesidall;
                gvMesid.DataBind();


            }
            catch (Exception ex)
            {
                ThongBao("Không có dữ liệu vui lòng thử lại sau!! ex.tostring");
            }

        }
        private void xemlieutatca()
        {
            try
            {
                string tungay = txtFromDay.Text.Trim().Replace("-", "");
                string denngay = txtToday.Text.Trim().Replace("-", "");
                if (tungay == "" || denngay == "")
                {
                    ThongBao("Vui lòng chọn ngày đầy đủ");
                    gvMesid.DataSource = null;
                    gvMesid.DataBind();
                    return;
                }
                System.Data.DataTable dtmesidall = new System.Data.DataTable();
                dtmesidall.Columns.Add("mesid");
                dtmesidall.Columns.Add("recipe_name");
                dtmesidall.Columns.Add("machno");
                dtmesidall.Columns.Add("weight");
                dtmesidall.Columns.Add("finishnum");
                dtmesidall.Columns.Add("indat");
                dtmesidall.Columns.Add("intime");
                dtmesidall.Columns.Add("idGrouplot");
                dtmesidall.Columns.Add("FinishTag");

                string getDataMesBB = "SELECT [mesid],[recipe_name],[weight],[machno],[indat],[intime],[idGrouplot] FROM [InTem].[dbo].[KEORE] where (indat between '" + tungay + "' and '" + denngay + "') and idGrouplot!= ''  order by mesid asc, indat asc , intime asc";
                string ConnectionString = "Data Source = 198.1.9.186; Initial Catalog = InTem; User ID = kendakv2; Password = kenda123";
                System.Data.DataTable dtmesBB = Cnn.ExecuteQuery(ConnectionString, getDataMesBB);
                if (dtmesBB.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    return;
                }
                //string Groupmesid = "";
                System.Data.DataTable dtmesidallfinih = new System.Data.DataTable();
                dtmesidallfinih.Columns.Add("GroupLotid");
                dtmesidallfinih.Columns.Add("may");
                dtmesidallfinih.Columns.Add("keo");
                dtmesidallfinih.Columns.Add("mesid");
                foreach (DataRow item in dtmesBB.Rows)
                {
                    //Groupmesid += "" + item["mesid"].ToString().Trim() + item["idGrouplot"].ToString().Trim() + ",";

                    dtmesidallfinih.Rows.Add(new object[] { item["idGrouplot"].ToString().Trim(), item["machno"].ToString().Trim(), item["recipe_name"].ToString().Trim(), item["mesid"].ToString().Trim() });

                }
                //string[] DulieuGroupmesid =Groupmesid.Split(new char[] { ',' });
                //tạo bảng finish num
                System.Data.DataTable dtfinish = new System.Data.DataTable();
                dtfinish.Columns.Add("finishnum");
                dtfinish.Columns.Add("recipe");
                dtfinish.Columns.Add("Id");
                dtfinish.Columns.Add("FinishTag");
                //end
                foreach (DataRow item in dtmesidallfinih.Rows)
                {
                    try
                    {
                        string idGrouplot = item["GroupLotid"].ToString().Trim();
                        string may = item["may"].ToString().Trim();
                        string recipename = item["keo"].ToString().Trim().Replace("-", "");
                        string mesidd = item["mesid"].ToString().Trim();
                        string Connectfinistnum = "";
                        switch (may)
                        {
                            case ("01"):

                                Connectfinistnum = "Data Source = 198.1.8.21; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("02"):
                                Connectfinistnum = "Data Source = 198.1.8.22; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("03"):
                                Connectfinistnum = "Data Source = 198.1.8.23; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("04"):
                                Connectfinistnum = "Data Source = 198.1.8.24; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("05"):
                                Connectfinistnum = "Data Source = 198.1.8.35; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("06"):
                                Connectfinistnum = "Data Source = 198.1.8.36; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("07"):
                                Connectfinistnum = "Data Source = 198.1.8.37; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                           
                        }

                        string Getdatafinish = "SELECT [FinishNum],[RecipeCode],[Id],[FinishTag] FROM [mfns].[dbo].[Ppt_GroupLot] where Id='" + idGrouplot + "' and RecipeCode='" + recipename + "'";

                        System.Data.DataTable finishnum = Cnn.ExecuteQuery(Connectfinistnum, Getdatafinish);
                        if (finishnum.Rows.Count == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (recipename == finishnum.Rows[0][1].ToString().Trim())
                            {
                                dtfinish.Rows.Add(new object[] { finishnum.Rows[0][0].ToString(), finishnum.Rows[0][1].ToString(), finishnum.Rows[0][2].ToString(), finishnum.Rows[0][3].ToString() });

                            }
                            else
                            {

                                continue;
                            }


                        }
                    }
                    catch(Exception ex)
                    {
                        continue;
                    }
                    


                }

                foreach (DataRow item in dtmesBB.Rows)
                {
                    foreach (DataRow item1 in dtfinish.Rows)
                    {
                        if (item["recipe_name"].ToString().Trim().Replace("-", "") == item1["recipe"].ToString().Trim() && item["idGrouplot"].ToString().Trim()== item1["Id"].ToString().Trim())
                        {
                            dtmesidall.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["recipe_name"].ToString().Trim(), item["machno"].ToString().Trim(), item["weight"].ToString().Trim(), item1["finishnum"].ToString().Trim(), item["indat"].ToString().Trim(), item["intime"].ToString().Trim(), item["idGrouplot"].ToString().Trim(), item1["FinishTag"].ToString().Trim() });
                        }
                    }
                }
                gvMesid.DataSource = dtmesidall;
                gvMesid.DataBind();

            }
            catch (Exception ex)
            {
                ThongBao("Không có dữ liệu vui lòng thử lại sau!! ex.tostring");
            }

        }

        protected void gvMesid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string Mesid = Dulieu[0].Trim();
            string idGrouplot = Dulieu[1].Trim();
            string recipename = Dulieu[2].Trim().Replace("-", "");
            string date = Dulieu[3].Trim();

            string a = Mesid.Substring(2, 1);
            string mayplc = "";
             string ConnectionString = "";
            switch (a)
            {
                case ("1"):
                    ConnectionString = "Data Source = 198.1.8.21; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                    
                    mayplc = "01";
                    break;
                case ("2"):
                    ConnectionString = "Data Source = 198.1.8.22; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                    ThongBao("Máy 2 chưa sửa liên hệ 吳惠東(Wu Huidong)");
                    mayplc = "02";
                    return;
                    break;
                case ("3"):
                    ConnectionString = "Data Source = 198.1.8.23; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                   
                    mayplc = "03";
                    break;
                case ("4"):
                    ConnectionString = "Data Source = 198.1.8.24; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                    
                    mayplc = "04";
                    break;
                case ("5"):
                    ConnectionString = "Data Source = 198.1.8.35; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                  
                    mayplc = "05";
                    break;
                case ("6"):
                    ConnectionString = "Data Source = 198.1.8.36; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                    
                    mayplc = "06";
                    break;
                case ("7"):
                    ConnectionString = "Data Source = 198.1.8.37; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                    
                    mayplc = "07";
                    break;

            }
            try
            {
                string Getdataidgrouplot = "SELECT [Id],[RecipeCode],convert(nvarchar(20),[Start_datetime],108) as Start_datetime,convert(nvarchar(20),[End_datetime],108) as End_datetime,[FinishTag] FROM [mfns].[dbo].[Ppt_GroupLot] where Id='" + idGrouplot + "' and RecipeCode='" + recipename + "'";

                System.Data.DataTable dtgrouplot = Cnn.ExecuteQuery(ConnectionString, Getdataidgrouplot);
                string recipenameplc = "";
                string time1 = "";
                string time2 = "";
                if (dtgrouplot.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    return;
                }
                else
                {
                    recipenameplc = dtgrouplot.Rows[0][1].ToString().Trim();

                    time1 = dtgrouplot.Rows[0][2].ToString().Trim();
                    time2 = dtgrouplot.Rows[0][3].ToString().Trim();
                    if (recipename == recipenameplc)
                    {
                        Page.ClientScript.RegisterStartupScript(
                        this.GetType(), "OpenWindow", "window.open('LieuPLC.aspx?may=" + mayplc + "&recipenameplc=" + recipenameplc + "&starttime=" + time1 + "&endtime=" + time2 + "&date=" + date + "','_newtab');", true);
                    }
                    else
                    {
                        ThongBao("Mã keo không trùng với idGrouplot đầu máy , liên hệ IT !!!");
                        return;
                    }


                }
            }
            catch(Exception ex)
            {
                ThongBao(mayplc + " không thể kết nối, hoặc máy bị tắt , vui lòng thử lại");
            }
           
           





        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (drChonmay.SelectedValue == "")
            {
                timkiemlieutatca();

            }
            else
            {
                timkiemlieutungmay();
            }
        }
        private void timkiemlieutatca()
        {
            try
            {
                string tungay = txtFromDay.Text.Trim().Replace("-", "");
                string denngay = txtToday.Text.Trim().Replace("-", "");
                string keotimkiem = txtTimkiem.Text.Trim();
                if (tungay == "" || denngay == "")
                {
                    ThongBao("Vui lòng chọn ngày đầy đủ");
                    gvMesid.DataSource = null;
                    gvMesid.DataBind();
                    return;
                }
                System.Data.DataTable dtmesidall = new System.Data.DataTable();
                dtmesidall.Columns.Add("mesid");
                dtmesidall.Columns.Add("recipe_name");
                dtmesidall.Columns.Add("machno");
                dtmesidall.Columns.Add("weight");
                dtmesidall.Columns.Add("finishnum");
                dtmesidall.Columns.Add("indat");
                dtmesidall.Columns.Add("intime");
                dtmesidall.Columns.Add("idGrouplot");
                dtmesidall.Columns.Add("FinishTag");

                string getDataMesBB = "SELECT [mesid],[recipe_name],[weight],[machno],[indat],[intime],[idGrouplot] FROM [InTem].[dbo].[KEORE] where (indat between '" + tungay + "' and '" + denngay + "') and idGrouplot!= '' and  [recipe_name] like '%"+keotimkiem+"%'  order by mesid asc, indat asc , intime asc";
                string ConnectionString = "Data Source = 198.1.9.186; Initial Catalog = InTem; User ID = kendakv2; Password = kenda123";
                System.Data.DataTable dtmesBB = Cnn.ExecuteQuery(ConnectionString, getDataMesBB);
                if (dtmesBB.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    return;
                }
                //string Groupmesid = "";
                System.Data.DataTable dtmesidallfinih = new System.Data.DataTable();
                dtmesidallfinih.Columns.Add("GroupLotid");
                dtmesidallfinih.Columns.Add("may");
                dtmesidallfinih.Columns.Add("keo");
                dtmesidallfinih.Columns.Add("mesid");
                foreach (DataRow item in dtmesBB.Rows)
                {
                    //Groupmesid += "" + item["mesid"].ToString().Trim() + item["idGrouplot"].ToString().Trim() + ",";

                    dtmesidallfinih.Rows.Add(new object[] { item["idGrouplot"].ToString().Trim(), item["machno"].ToString().Trim(), item["recipe_name"].ToString().Trim(), item["mesid"].ToString().Trim() });

                }
                //string[] DulieuGroupmesid =Groupmesid.Split(new char[] { ',' });
                //tạo bảng finish num
                System.Data.DataTable dtfinish = new System.Data.DataTable();
                dtfinish.Columns.Add("finishnum");
                dtfinish.Columns.Add("recipe");
                dtfinish.Columns.Add("Id");
                dtfinish.Columns.Add("FinishTag");
                //end
                foreach (DataRow item in dtmesidallfinih.Rows)
                {
                    try
                    {
                        string idGrouplot = item["GroupLotid"].ToString().Trim();
                        string may = item["may"].ToString().Trim();
                        string recipename = item["keo"].ToString().Trim().Replace("-", "");
                        string mesidd = item["mesid"].ToString().Trim();
                        string cata = "";
                        //switch (may)
                        //{
                        //    case ("01"):
                        //        cata = "BB_May1_8.21";
                        //        break;
                        //    case ("02"):
                        //        cata = "BB_May2_8.22";
                        //        break;
                        //    case ("03"):
                        //        cata = "BB_May3_8.23";
                        //        break;
                        //    case ("04"):
                        //        cata = "BB_May4_8.24";
                        //        break;
                        //    case ("05"):
                        //        cata = "BB_May5_8.35";
                        //        break;
                        //    case ("06"):
                        //        cata = "BB_May6_8.36";
                        //        break;
                        //    case ("07"):
                        //        cata = "BB_May7_8.37";
                        //        break;
                        string Connectfinistnum = "";
                        switch (may)
                        {
                            case ("01"):

                                Connectfinistnum = "Data Source = 198.1.8.21; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("02"):
                                Connectfinistnum = "Data Source = 198.1.8.22; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("03"):
                                Connectfinistnum = "Data Source = 198.1.8.23; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("04"):
                                Connectfinistnum = "Data Source = 198.1.8.24; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("05"):
                                Connectfinistnum = "Data Source = 198.1.8.35; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("06"):
                                Connectfinistnum = "Data Source = 198.1.8.36; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                            case ("07"):
                                Connectfinistnum = "Data Source = 198.1.8.37; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";
                                break;
                        }

                        string Getdatafinish = "SELECT [FinishNum],[RecipeCode],[Id],[FinishTag] FROM [mfns].[dbo].[Ppt_GroupLot] where Id='" + idGrouplot + "' and RecipeCode='" + recipename + "'";

                        System.Data.DataTable finishnum = Cnn.ExecuteQuery(Connectfinistnum, Getdatafinish);
                        if (finishnum.Rows.Count == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (recipename == finishnum.Rows[0][1].ToString().Trim())
                            {
                                dtfinish.Rows.Add(new object[] { finishnum.Rows[0][0].ToString(), finishnum.Rows[0][1].ToString(), finishnum.Rows[0][2].ToString(), finishnum.Rows[0][3].ToString() });

                            }
                            else
                            {

                                continue;
                            }


                        }

                    }
                    catch(Exception ex)
                    {
                        continue;
                    }
                    


                }

                foreach (DataRow item in dtmesBB.Rows)
                {
                    foreach (DataRow item1 in dtfinish.Rows)
                    {
                        if (item["recipe_name"].ToString().Trim().Replace("-", "") == item1["recipe"].ToString().Trim() && item["idGrouplot"].ToString().Trim() == item1["Id"].ToString().Trim())
                        {
                            dtmesidall.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["recipe_name"].ToString().Trim(), item["machno"].ToString().Trim(), item["weight"].ToString().Trim(), item1["finishnum"].ToString().Trim(), item["indat"].ToString().Trim(), item["intime"].ToString().Trim(), item["idGrouplot"].ToString().Trim(), item1["FinishTag"].ToString().Trim() });
                        }
                    }
                }
                gvMesid.DataSource = dtmesidall;
                gvMesid.DataBind();

            }
            catch (Exception ex)
            {
                ThongBao("Không có dữ liệu vui lòng thử lại sau!! ex.tostring");
            }

        }
        private void timkiemlieutungmay()
        {
            try
            {
                string may = "";
                string maytam = drChonmay.SelectedValue;
                if (maytam != "")
                {
                    may = "and machno = '" + maytam + "' ";
                }
                string tungay = txtFromDay.Text.Trim().Replace("-", "");
                string denngay = txtToday.Text.Trim().Replace("-", "");
                string keotimkiem = txtTimkiem.Text.Trim();
                if (tungay == "" || denngay == "")
                {
                    ThongBao("Vui lòng chọn ngày đầy đủ");
                    gvMesid.DataSource = null;
                    gvMesid.DataBind();
                    return;
                }
                System.Data.DataTable dtmesidall = new System.Data.DataTable();
                dtmesidall.Columns.Add("mesid");
                dtmesidall.Columns.Add("recipe_name");
                dtmesidall.Columns.Add("machno");
                dtmesidall.Columns.Add("weight");
                dtmesidall.Columns.Add("finishnum");
                dtmesidall.Columns.Add("indat");
                dtmesidall.Columns.Add("intime");
                dtmesidall.Columns.Add("idGrouplot");
                dtmesidall.Columns.Add("FinishTag");
                string getDataMesBB = "SELECT [mesid],[recipe_name],[weight],[machno],[indat],[intime],[idGrouplot] FROM [InTem].[dbo].[KEORE] where (indat between '" + tungay + "' and '" + denngay + "') and idGrouplot!= ''   " + may + " and  [recipe_name] like '%" + keotimkiem + "%'  order by mesid asc, indat asc , intime asc";
                string ConnectionString = "Data Source = 198.1.9.186; Initial Catalog = InTem; User ID = kendakv2; Password = kenda123";
                System.Data.DataTable dtmesBB = Cnn.ExecuteQuery(ConnectionString, getDataMesBB);

                if (dtmesBB.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
                    return;
                }
                System.Data.DataTable dtfinish = new System.Data.DataTable();
                dtfinish.Columns.Add("finishnum");
                dtfinish.Columns.Add("recipe");
                dtfinish.Columns.Add("Id");
                dtfinish.Columns.Add("FinishTag");
                foreach (DataRow item in dtmesBB.Rows)
                {
                    try
                    {
                        string idGrouplot = item["idGrouplot"].ToString().Trim();
                        string may123 = item["machno"].ToString().Trim();
                        string recipename = item["recipe_name"].ToString().Trim().Replace("-", "");
                        string mesidd = item["mesid"].ToString().Trim();

                        string Connectfinistnum = "";
                        switch (may123)
                        {
                            case ("01"):
                                Connectfinistnum = "Data Source = 198.1.8.21; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";

                                break;
                            case ("02"):
                                Connectfinistnum = "Data Source = 198.1.8.22; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";

                                break;
                            case ("03"):
                                Connectfinistnum = "Data Source = 198.1.8.23; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";

                                break;
                            case ("04"):
                                Connectfinistnum = "Data Source = 198.1.8.24; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";

                                break;
                            case ("05"):
                                Connectfinistnum = "Data Source = 198.1.8.35; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";

                                break;
                            case ("06"):
                                Connectfinistnum = "Data Source = 198.1.8.36; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";

                                break;
                            case ("07"):
                                Connectfinistnum = "Data Source = 198.1.8.37; Initial Catalog = mfns; User ID = kendakv2; Password = kenda123";

                                break;
                        }

                        string Getdatafinish = "SELECT [FinishNum],[RecipeCode],[Id],[FinishTag] FROM [mfns].[dbo].[Ppt_GroupLot] where Id='" + idGrouplot + "' and RecipeCode='" + recipename + "'";

                        System.Data.DataTable finishnum = Cnn.ExecuteQuery(Connectfinistnum, Getdatafinish);
                        if (finishnum.Rows.Count == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (recipename == finishnum.Rows[0][1].ToString().Trim())
                            {
                                dtfinish.Rows.Add(new object[] { finishnum.Rows[0][0].ToString(), finishnum.Rows[0][1].ToString(), finishnum.Rows[0][2].ToString(), finishnum.Rows[0][3].ToString() });

                            }
                            else
                            {

                                continue;
                            }


                        }


                    }
                    catch (Exception ex) 
                    {
                        continue;
                    }
                    

                }
                foreach (DataRow item in dtmesBB.Rows)
                {
                    foreach (DataRow item1 in dtfinish.Rows)
                    {
                        if (item["recipe_name"].ToString().Trim().Replace("-", "") == item1["recipe"].ToString().Trim() && item["idGrouplot"].ToString().Trim() == item1["Id"].ToString().Trim())
                        {
                            dtmesidall.Rows.Add(new object[] { item["mesid"].ToString().Trim(), item["recipe_name"].ToString().Trim(), item["machno"].ToString().Trim(), item["weight"].ToString().Trim(), item1["finishnum"].ToString().Trim(), item["indat"].ToString().Trim(), item["intime"].ToString().Trim(), item["idGrouplot"].ToString().Trim(), item1["FinishTag"].ToString().Trim() });
                        }
                    }
                }
                gvMesid.DataSource = dtmesidall;
                gvMesid.DataBind();


            }
            catch (Exception ex)
            {
                ThongBao("Không có dữ liệu vui lòng thử lại sau!! ex.tostring");
            }
        }
    }
}