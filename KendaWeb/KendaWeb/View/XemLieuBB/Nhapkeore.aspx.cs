using DocumentFormat.OpenXml.Spreadsheet;
using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KendaWeb.View.XemLieuBB
{
    public partial class Nhapkeore : System.Web.UI.Page
    {
        string cnn = "Data Source= 198.1.10.33; Initial Catalog=erp;User ID= kendakv2;Password= kenda123;";
        string cnnBB = "Data Source= 198.1.10.33; Initial Catalog=BB;User ID= kendakv2;Password= kenda123;";
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtTimkiem.TextChanged += new EventHandler(txtTimkiem_TextChanged);

            if (!IsPostBack)
            {
                if (txtbatdau.Text == "" || txtketthuc.Text == "")
                {
                    string strdate = DateTime.Now.ToString("yyyy-MM-dd");
                    txtbatdau.Text = strdate;
                    txtketthuc.Text = strdate;

                }
                keoLoad();
                loaddulieu();
            }


            //TxtNhaptenkeo.Focus();
        }

        private void keoLoad()
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/View/Loggin.aspx");
            }
            lblsothe.Text = "Số thẻ: "+ Session["UserName"].ToString();
            string sql = "select partno from [erp].[dbo].[prdebe] where partno like '%-RE' group by partno";

            DataTable dt = Cnn.ExecuteQuery(cnn, sql);
            drChonkeoRE.DataTextField = "partno";
            drChonkeoRE.DataSource = dt;
            drChonkeoRE.DataBind();
        }

        protected void Them_Click(object sender, EventArgs e)
        {
            string tenkeo = drChonkeoRE.Text.Trim();
            string tungay = txtbatdau.Text.Trim().Replace("-", "");
            string denngay = txtketthuc.Text.Trim().Replace("-", "");
         

            string input = lblsothe.Text.Trim();
            string[] parts = input.Split(':');
            string usrno = parts[1].Trim(); 
            string indat = DateTime.Now.ToString("yyyyMMdd");
            string intime = DateTime.Now.ToString("HH:mm:ss");
            if (tenkeo == "" || tungay == "" || denngay == "")
            {
                ThongBao("Không được bỏ trống !!!");
                return;
            }
            //string laymaxid = "select max(ID) FROM [BB].[dbo].[NXRE]";
            //DataTable maxid = Cnn.ExecuteQuery(cnn, laymaxid);
            //string insertid = (int.Parse(maxid.Rows[0][0].ToString().Trim()) + 1).ToString();
            string insert = "  insert into [BB].[dbo].[NXRE] values ('" + tenkeo + "','" + tungay + "','" + usrno + "','" + denngay + "','" + indat + "','" + intime + "','N',null)";
            bool a = Cnn.ExecuteNonQuery(cnnBB, insert);
            if (a == true)
            {
                ThongBao("Thêm dữ liệu thành công !!!");
                loaddulieu();


            }
            else
            {
                ThongBao("Thêm liệu thất bại !!!");
                loaddulieu();
            }
        }

        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }
        private void loaddulieu()
        {

            string sql = "SELECT  [Recipe_name],[Start_date],[ursno],[End_date],[Indat],[Intime],[Lock],[fixdat]  FROM [BB].[dbo].[NXRE] order by indat desc, intime desc";

            DataTable dt = Cnn.ExecuteQuery(cnnBB, sql);
            gvKQRE.DataSource = dt;
            gvKQRE.DataBind();
        }

        protected void gvKQRE_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string ten = e.CommandArgument.ToString().Trim();
            string[] Dulieu = e.CommandArgument.ToString().Split(new char[] { ',' });
            string Recipe_name = Dulieu[0].ToString().Trim();
            string indat = Dulieu[1].ToString().Trim();
            string intime = Dulieu[2].ToString().Trim();

            if (e.CommandName == "btnxoa")
            {
                string sqldel = "  delete  [BB].[dbo].[NXRE] where Recipe_name ='"+Recipe_name+"' and indat ='"+indat+"' and intime ='"+intime+"' ";
                bool delete = Cnn.ExecuteNonQuery(cnnBB, sqldel);
                if (delete == true)
                {
                    ThongBao("Xóa dữ liệu keo " + ten + " thành công !!!");
                    loaddulieu();
                }
                else
                {
                    ThongBao("Xóa dữ liệu id " + ten + " Thất bại !!!");
                    loaddulieu();
                }
            }
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            string timkiem = txtTimkiem.Text;
            string sqltimkiem = "SELECT  [Recipe_name],[Start_date],[ursno],[End_date],[Indat],[Intime],[Lock],[fixdat]  FROM [BB].[dbo].[NXRE] where Recipe_name like '%" + timkiem + "%' order by indat desc, intime desc";
            DataTable dttimkiem = Cnn.ExecuteQuery(cnnBB, sqltimkiem);
            if(dttimkiem.Rows.Count > 0)
            {
                gvKQRE.DataSource = dttimkiem;
                gvKQRE.DataBind();

            }
            else
            {
                gvKQRE.DataSource = null;
                gvKQRE.DataBind();
                ThongBao("Không có keo tìm kiếm");
            }

        }

        //protected void txtTimkiem_TextChanged(object sender, EventArgs e)
        //{
        //    //if (txtTimkiem.Text.Trim() != "")
        //    //{ 
        //    //    btnTimkiem_Click(sender, e);
        //    //    return;
        //    //}
        //    btnTimkiem_Click(sender, e);
        //    //loaddulieu();

        //}
    }
}