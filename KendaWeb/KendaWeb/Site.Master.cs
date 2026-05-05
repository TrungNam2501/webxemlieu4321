using System;
using System.Collections.Generic;
using System.Web.UI;

namespace KendaWeb
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] == null)
                {
                    RedirectToLogin();
                    return;
                }
            }

            try
            {
                string name = Session["Name"]?.ToString() ?? "";
                string username = Session["UserName"]?.ToString() ?? "";
                string chucVu = Session["ChucVu"]?.ToString() ?? "";

                if (string.IsNullOrEmpty(username))
                {
                    RedirectToLogin();
                    return;
                }

                lbSoThe.Text = $"TK:{name}-{username}";

                // Check chức vụ
                if (chucVu.StartsWith("B22"))
                {
                    RunScript("LiBlock");
                }

                // Danh sách user cho từng quyền
                var duyenUsers = new HashSet<string> { "016966", "229906", "167189", "223354" };
                var plcUsers = new HashSet<string> { "213785", "005571", "014185", "013673", "020569", "014842", "018892", "023999" };
                var phongUsers = new HashSet<string> { "014292" };
                var bbUsers = new HashSet<string> { "020958", "003659", "014744", "022481", "023954", "025151", "025752" };

                if (duyenUsers.Contains(username))
                {
                    RunScript("LiBlockduyen");
                }

                if (plcUsers.Contains(username))
                {
                    RunScript("LiBlockxemlieuPLC");
                }

                if (phongUsers.Contains(username))
                {
                    RunScript("LiBlockphong");
                }

                if (bbUsers.Contains(username))
                {
                    RunScript("LiBlockbb");
                }
            }
            catch
            {
                RedirectToLogin();
            }
        }

        private void RunScript(string functionName)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", $"{functionName}();", true);
        }

        private void RedirectToLogin()
        {
            Response.Redirect("~/View/Loggin.aspx");
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            RedirectToLogin();
        }
    }
}