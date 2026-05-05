using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Bibliography;
using KendaWeb.Model;

namespace KendaWeb.View.XemlieuPLC
{
    public partial class PLCyaml : System.Web.UI.Page
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
            xemlieutungmay();
            
        }
        private void xemlieutungmay()
        {
            try
            {
                string may = drChonmay.SelectedValue;
                string tungay = txtFromDay.Text.Trim().Replace("-", "");
                string denngay = txtToday.Text.Trim().Replace("-", "");
                if (tungay == "" || denngay == "")
                {
                    ThongBao("Vui lòng chọn ngày đầy đủ");
                    gvMesid.DataSource = null;
                    gvMesid.DataBind();
                    return;
                }
                string sqlCheckMes = " select a.Plan_Id, a.Recipe_Code,a.Equip_ID,a.Plan_Num,a.Write_Time,b.FinishTag,a.P_Date FROM [mfnsShareDB].[dbo].[IF_RtPlan2Mixing] a, [mfns].[dbo].[Ppt_GroupLot] b where CAST(a.P_Date AS DATE) BETWEEN '" + tungay + "' AND '"+denngay+"' and a.Plan_Id not like 'V%' and a.Plan_Id = b.MesPlanID and b.End_datetime is not null";
                DataTable dtMes = Model.SQL8may.ExecuteQuery(sqlCheckMes, may);
                if (dtMes.Rows.Count > 0)
                {
                    gvMesid.DataSource = dtMes;
                    gvMesid.DataBind();
                }else
                {
                    ThongBao("Ngày bạn chọn chưa có dữ liệu ");
                    gvMesid.DataSource = null;
                    gvMesid.DataBind();

                }    

            }
            catch (Exception ex)
            {
                ThongBao("Không có dữ liệu vui lòng thử lại sau!! ex.tostring");
            }
            
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            try
            {
                
                string may = drChonmay.SelectedValue;
                string tungay = txtFromDay.Text.Trim().Replace("-", "");
                string denngay = txtToday.Text.Trim().Replace("-", "");
                string timkiem = txtTimkiem.Text.Trim();

                if (tungay == "" || denngay == "")
                {
                    ThongBao("Vui lòng chọn ngày đầy đủ");
                    gvMesid.DataSource = null;
                    gvMesid.DataBind();
                    return;
                }
                string sqlCheckMes = " select a.Plan_Id, a.Recipe_Code,a.Equip_ID,a.Plan_Num,a.Write_Time,b.FinishTag,a.P_Date FROM [mfnsShareDB].[dbo].[IF_RtPlan2Mixing] a, [mfns].[dbo].[Ppt_GroupLot] b where CAST(a.P_Date AS DATE) " +
                    "BETWEEN '" + tungay + "' AND '" + denngay + "' and a.Plan_Id not like 'V%' and a.Plan_Id = b.MesPlanID and a.Recipe_Code like '"+timkiem+"%' and b.End_datetime is not null";
                DataTable dtMes = Model.SQL8may.ExecuteQuery(sqlCheckMes, may);
                if (dtMes.Rows.Count > 0)
                {
                    gvMesid.DataSource = dtMes;
                    gvMesid.DataBind();
                }
                else
                {
                    ThongBao("Ngày bạn chọn chưa có dữ liệu ");
                    gvMesid.DataSource = null;
                    gvMesid.DataBind();

                }

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
            string recipename = Dulieu[1].Trim();
            string date = Dulieu[2].Trim();

            string a = Mesid.Substring(2, 1);
            string mayplc = drChonmay.SelectedValue;
          
          
            try
            {
                string Getdataidgrouplot = "SELECT [Id],[RecipeCode],convert(nvarchar(20),[Start_datetime],108) as Start_datetime,convert(nvarchar(20),[End_datetime],108) as End_datetime,[FinishTag] FROM [mfns].[dbo].[Ppt_GroupLot] where MesPlanID ='" + Mesid + "' and RecipeCode='" + recipename + "'";

                System.Data.DataTable dtgrouplot = SQL8may.ExecuteQuery(Getdataidgrouplot,mayplc);
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
            catch (Exception ex)
            {
                ThongBao(mayplc + " không thể kết nối, hoặc máy bị tắt , vui lòng thử lại");
            }

        }
    }
}