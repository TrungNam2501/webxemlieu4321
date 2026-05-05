using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KendaWeb.Model;

namespace KendaWeb
{
    public class Test
    {             
        public static string ThemPallet(string pallet, string usrno)
        {
            string ConnectionStringKeoRe = "Data Source=.;Initial Catalog=TestBBToan;Integrated Security = True";
            string SelPallet = "SELECT PALLET_NO FROM [TestBBToan].[dbo].[PalletBBTest] WHERE PALLET_NO='" + pallet + "'";
            string result = string.Empty;
            System.Data.DataTable dtPallet = Cnn.ExecuteQuery(ConnectionStringKeoRe, SelPallet);

            if (dtPallet.Rows.Count == 0)
            {
                string sDat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string sqlIns = "INSERT INTO [TestBBToan].[dbo].[PalletBBTest] VALUES('" + pallet + "', '" + sDat + "', '" + usrno + "','1', 'Y') ";

                bool bQuery = Cnn.ExecuteNonQuery(ConnectionStringKeoRe, sqlIns);
                if (!bQuery)
                {
                    result = "Lỗi cập nhật Pallet\nPalletBB";
                }
            }
            return result;
        }

        public static int ThemLieuBB(string KhuVuc, string Xuong, string MaMes, string machno, string HanSuDung, string MaVach, string SoLo, string TrongLuong, string NgaySanXuat, string NgayHieuLuc, string Ca, string Loai, string CanDao, string TenKeo, string ThoiGianQuet, string NgayQuet, string NguoiQuet, string pallet, string Xuat)
        {
            string ConnectionString = "Data Source=.;Initial Catalog=TestBBToan;Integrated Security = True";
            string GetData = "insert into [TestBBToan].[dbo].[prdebeTest]" +
                        "(subno,factory,mesid,machno,daylimt,barcode,slipno,weight,prodat,effdat,class,ptype," +
                        "status,partno,intime,indat,usrno,pallet_no,active) " +
                        "values('" + KhuVuc + "', '" + Xuong + "', '" + MaMes + "', '" + machno + "', '" + HanSuDung + "', '" + MaVach + "', '" + SoLo + "', '" + TrongLuong + "', " +
                        "'" + NgaySanXuat + "', '" + NgayHieuLuc + "', '" + Ca + "', '" + Loai + "', '" + CanDao + "', " +
                        "'" + TenKeo + "', '" + ThoiGianQuet + "', '" + NgayQuet + "', '" + NguoiQuet + "', '" + pallet + "', '" + Xuat + "')";

            bool result = Cnn.ExecuteNonQuery(ConnectionString, GetData);
            if (result == true)
            {
                return 1;
                //ThongBao("Thêm dữ liệu thành công!!!");
            }
            else
            {
                //lbError.Visible = true;
                //lbError.Text = "Dữ liệu không hợp lệ!!!";
                //ResetData();
                //ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                return 0;
            }
        }

        public static int SuaLieuBB(string KhuVuc, string Xuong, string MaMes, string machno, string HanSuDung, string MaVach, string SoLo, string TrongLuong, string NgaySanXuat, string NgayHieuLuc, string Ca, string Loai, string CanDao, string TenKeo, string ThoiGianQuet, string NgayQuet, string NguoiQuet, string pallet, string Xuat)
        {
            string ConnectionString = "Data Source=.;Initial Catalog=TestBBToan;Integrated Security = True";
            string GetData = "UPDATE [TestBBToan].[dbo].[prdebeTest] " +
                            "SET subno ='" + KhuVuc + "',factory='" + Xuong + "',mesid='" + MaMes + "',machno='" + machno + "',daylimt='" + HanSuDung + "'," +
                            "slipno='" + SoLo + "',weight='" + TrongLuong + "'," +
                            "prodat='" + NgaySanXuat + "',effdat='" + NgayHieuLuc + "',class='" + Ca + "',ptype='" + Loai + "',status='" + CanDao + "',partno='" + TenKeo + "'," +
                            "intime='" + ThoiGianQuet + "',indat='" + NgayQuet + "',usrno='" + NguoiQuet + "',pallet_no='" + pallet + "',active='" + Xuat + "' " +
                            "where barcode = '" + MaVach + "'";

            bool result = Cnn.ExecuteNonQuery(ConnectionString, GetData);
            if (result == true)
            {
                return 1;
                //ThongBao("Thêm dữ liệu thành công!!!");
            }
            else
            {
                //lbError.Visible = true;
                //lbError.Text = "Dữ liệu không hợp lệ!!!";
                //ResetData();
                //ScriptManager.RegisterStartupScript(this, GetType(), "", "ShowgvThemMoi();", true);
                return 0;
            }
        }
    }
}