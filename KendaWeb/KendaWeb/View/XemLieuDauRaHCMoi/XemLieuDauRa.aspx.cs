using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KendaWeb.View.XemLieuDauRaHCMoi
{
    public partial class XemLieuDauRa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void txtDenNgay_TextChanged(object sender, EventArgs e)
        {
            int datetu = int.Parse(txtTuNgay.Text.Replace("-", "").Trim());
            int dateden = int.Parse(txtDenNgay.Text.Replace("-", "").Trim());

            if (dateden >= datetu)
            {
                return;
            }
            else
            {
                ThongBao("Dữ liệu không hợp lệ!!!");
                txtDenNgay.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        private void ThongBao(string ThongBao)
        {
            lblThongbao.Text = ThongBao;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showMessage();", true);
        }
    }
}