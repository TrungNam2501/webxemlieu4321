using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

namespace KendaWeb
{
    public partial class Loggin : System.Web.UI.Page
    {
        class UserInfo
        {
            public string Name { get; set; }
            public string Mabp { get; set; }
            public string Macv { get; set; }
            public string Password { get; set; }
        }

        Dictionary<string, UserInfo> users = new Dictionary<string, UserInfo>()
        {
            {"018892", new UserInfo{Name="Ha", Mabp="P87", Macv="P8700", Password="hakvp8720"}},
            {"023999", new UserInfo{Name="Dang", Mabp="P87", Macv="P8700", Password="dangp8720"}},
            {"014292", new UserInfo{Name="Van Phong", Mabp="P89", Macv="P8900", Password="19900326"}},
            {"016966", new UserInfo{Name="Duyen", Mabp="P80", Macv="P8000", Password="19940520"}},
            {"223354", new UserInfo{Name="宋品璁", Mabp="P89", Macv="P8900", Password="19891117"}},
            {"025752", new UserInfo{Name="Trang Anh", Mabp="P91", Macv="P9100", Password="19901020"}},
            {"025151", new UserInfo{Name="Be", Mabp="P91", Macv="P9100", Password="19870622"}},
            {"022481", new UserInfo{Name="Mong Kieu", Mabp="P91", Macv="P9100", Password="20000322"}},
            {"014185", new UserInfo{Name="Tùng", Mabp="P87", Macv="P8700", Password="19930816"}},
            {"005571", new UserInfo{Name="Thuần", Mabp="P8B", Macv="P8B00", Password="19841112"}},
            {"020569", new UserInfo{Name="Chương", Mabp="P87", Macv="P8700", Password="19850627"}},
            {"013673", new UserInfo{Name="Thu", Mabp="P87", Macv="P8700", Password="bethu"}},
            {"014842", new UserInfo{Name="Thuy", Mabp="P88", Macv="P8800", Password="thuyht"}},
            {"213785", new UserInfo{Name="Jen Hao", Mabp="P87", Macv="P8700", Password="19831110"}},
            {"003659", new UserInfo{Name="Quang Minh", Mabp="P91", Macv="P9100", Password="19770603"}},
            {"229906", new UserInfo{Name="張勻碩", Mabp="P61", Macv="P6100", Password="19870525"}},
            {"167189", new UserInfo{Name="Liu Tei", Mabp="B20", Macv="B2000", Password="19830902"}},
            {"014282", new UserInfo{Name="Giang", Mabp="B22", Macv="B2210", Password=""}},
            {"022191", new UserInfo{Name="Nam IT", Mabp="B22", Macv="B2210", Password=""}}
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            lbError.Visible = false;
            txtSoThe.Attributes.Add("autocomplete", "off");
        }

        protected void btnDangNhap_Click(object sender, EventArgs e)
        {
            string user = txtSoThe.Text.Trim();
            string pass = txtPass.Text.Trim();

            // kiểm tra user hardcode
            if (users.ContainsKey(user) && users[user].Password == pass)
            {
                LoginSuccess(user, users[user].Name, users[user].Macv);
                return;
            }

            // kiểm tra DB
            string sql = "SELECT empno, depno, name, bithdat FROM [erp].[dbo].[peremp] WHERE empno='" + user + "' AND depno LIKE 'B22%'";
            DataTable dt = Cnn.ExecuteQuery("Data Source=198.1.10.34;Initial Catalog=erp;User ID=kendakv2;Password=kenda123", sql);

            if (dt.Rows.Count == 1)
            {
                if (pass == dt.Rows[0]["bithdat"].ToString().Trim())
                {
                    LoginSuccess(
                        user,
                        dt.Rows[0]["name"].ToString().Trim(),
                        dt.Rows[0]["depno"].ToString().Trim()
                    );
                    return;
                }
            }

            lbError.Visible = true;
            lbError.Text = "Tài khoản hoặc mật khẩu không đúng!";
        }

        void LoginSuccess(string user, string name, string macv)
        {
            Session["UserName"] = user;
            Session["Name"] = name;
            Session["ChucVu"] = macv;
            Session.Timeout = 600;

            Response.Redirect("~/View/Home.aspx");
        }
    }
}