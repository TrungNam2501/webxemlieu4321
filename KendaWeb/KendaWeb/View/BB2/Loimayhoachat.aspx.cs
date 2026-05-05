using KendaWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KendaWeb.View.BB2
{
    public partial class Loimayhoachat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (txtChonngay.Text == "" )
            {
                string strdate = DateTime.Now.ToString("yyyy-MM-dd");
                txtChonngay.Text = strdate;
                txtTugio.Text = "00:00:00";
                txtDengio.Text = "23:59:59";
            }
        }

        protected void btnXemLieu_Click(object sender, EventArgs e)
        {
            string date = txtChonngay.Text.Trim().ToString();
            string tugio= txtTugio.Text.Trim().ToString();
            string dengio=txtDengio.Text.Trim().ToString();
            string may = drMay.SelectedValue.ToString();
            if (may == "")
            {
                ThongBao("Vui lòng chọn máy");
                return; 

            }
            if (tugio == "" || dengio == "")
            {
                ThongBao("Vui lòng chọn đầy đủ khoảng giờ bắt đầu và giờ kết thúc muốn tìm kiếm");
                return ;
            }
            if (date == "") 
            {
                ThongBao("Vui lòng chọn ngày bạn muốn tìm kiếm");
                return;
            }

            string giobatdau = date + " " + tugio;
            string gioketthuc = date + " " + dengio;
            string sqltruyvan = "select a.Alarm_ID, a.Alarm_OccurTime,a.Alarm_ClearTime,b.Alarm_En_Info,b.Alarm_Cn_Info,b.Alarm_Other_Info from [CWSS_S7].[dbo].[LR_Alarmlog] a , [CWSS_S7].[dbo].[Pmt_Alarm] b where a.Alarm_ID=b.Alarm_ID and CONVERT(datetime,substring(a.Alarm_OccurTime,1,10), 126) ='" + date + "' and a.Alarm_OccurTime between '" + giobatdau + "' and '" + gioketthuc + "' order by a.Alarm_OccurTime asc";
            string ConnectionString = "Data Source = "+may+"; Initial Catalog = CWSS_S7; User ID = kendakv2; Password = kenda123";
            System.Data.DataTable dtloi = Cnn.ExecuteQuery(ConnectionString, sqltruyvan);
            if(dtloi.Rows.Count > 0)
            {
                foreach (DataRow rowcu in dtloi.Rows)
                {

                    if (rowcu["Alarm_Other_Info"].ToString().Trim() == "")
                    {
                        rowcu["Alarm_Other_Info"] = "Chưa có lỗi bằng tiếng Việt";
                    }
                    else
                    {
                        continue;
                    }


                }
                gvKQ.DataSource = dtloi;
                gvKQ.DataBind();
            }
            else
            {
                ThongBao("Không có dữ liệu nào !!!");
                gvKQ.DataSource = null;
                gvKQ.DataBind();
                return;
            }
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

        protected void drMay_SelectedIndexChanged(object sender, EventArgs e)
        {
            string may = drMay.SelectedValue.ToString();
            if(may == "")
            {
                gvKQ.DataSource = null;
                gvKQ.DataBind();
                return;

            }
            else
            {
                int a = PingIp(may);

                if (a == 1)
                {
                    return;
                }
                else
                {
                    ThongBao("Máy bạn chọn đang bị tắt, hoặc lỗi mạng. Vui lòng kiểm tra máy và thử lại!!!! ");
                    gvKQ.DataSource = null;
                    gvKQ.DataBind();
                    return;
                }
            }
           

           
        }
    }
}