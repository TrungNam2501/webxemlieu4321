using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KendaWeb.View.XemLieuBB
{
    public partial class Keoxuly : System.Web.UI.Page
    {
        string ConnectionStringBB = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=BB;User ID=kendakv2;Password=kenda123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (txtTungay.Text == "" || txtDenngay.Text == "")
            {
                string strdate = DateTime.Now.ToString("yyyy-MM-dd");
                txtTungay.Text = strdate;
                txtDenngay.Text = strdate;
            }
        }

        protected void btnXemlieu_Click(object sender, EventArgs e)
        {
            
            LoadData();
        }
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }
        public void LoadData()
        {
            try
            {
                string TuNgay = txtTungay.Text.Trim();
                string DenNgay = txtDenngay.Text.Trim();
                string getdata = "SELECT * FROM [BB].[dbo].[BarcodeRep_Missing]  where End_datetime  between '"+TuNgay+"' and '"+DenNgay+"' order by End_datetime desc";
                System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringBB, getdata);
                if (dt.Rows.Count == 0)
                {
                    ThongBao("Không có dữ liệu");
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
                ThongBao(ex.ToString());
            }
        }

    }
}