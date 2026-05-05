using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KendaWeb.View.XemLieuBB
{
    public partial class Kiemtrapalletxuatkeo : System.Web.UI.Page
    {
        string ConnectionStringHome = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=erp;User ID=kendakv2;Password=kenda123";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            string pallet = txtTimkiem.Text.Trim().ToString();
            if(pallet == "")
            {
                ThongBao("Vui lòng nhập mã pallet");
                return;
            }
            string sql = "select [mesid],[machno],[barcode],[slipno],[weight],[effdat],[class],[partno],[intime] ,[indat],[usrno],[pallet_no],[active] from [erp].[dbo].[prdebe] where pallet_no='" + pallet + "' order by indat desc, intime desc ";

            //string sql = "select [mesid],[machno],[barcode],[slipno],[weight],[effdat],[class],[partno],[intime] ,[indat],[usrno],[pallet_no],CASE  WHEN [active]  = 'Y' THEN '0'  WHEN [active] = 'N' THEN '1'  ELSE '' END AS trangthai from [erp].[dbo].[prdebe] where pallet_no='" + pallet + "' and active ='N' ";
            System.Data.DataTable dt = Cnn.ExecuteQuery(ConnectionStringHome, sql);
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
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }

     
    }
}