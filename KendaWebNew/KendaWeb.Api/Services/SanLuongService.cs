using KendaWeb.Api.Configuration;
using KendaWeb.Api.Models.DTOs;
using KendaWeb.Api.Repositories;

namespace KendaWeb.Api.Services;

public interface ISanLuongService
{
    Task<ApiResponse<List<SanLuongDto>>> GetSanLuongAsync(SanLuongRequest request);
}

public class SanLuongService : ISanLuongService
{
    private readonly ISanLuongRepository _repo;
    private readonly IMachineRouter _machineRouter;
    private readonly IDbConnectionFactory _dbFactory;

    public SanLuongService(
        ISanLuongRepository repo,
        IMachineRouter machineRouter,
        IDbConnectionFactory dbFactory)
    {
        _repo = repo;
        _machineRouter = machineRouter;
        _dbFactory = dbFactory;
    }

    public async Task<ApiResponse<List<SanLuongDto>>> GetSanLuongAsync(SanLuongRequest request)
    {
        if (string.IsNullOrEmpty(request.FromDay) || string.IsNullOrEmpty(request.ToDay))
            return ApiResponse<List<SanLuongDto>>.Fail("Vui lòng nhập ngày!");

        if (string.IsNullOrEmpty(request.May))
            return ApiResponse<List<SanLuongDto>>.Fail("Vui lòng chọn máy!");

        var ip = _machineRouter.GetMfnsIp(request.May);
        if (ip == null)
            return ApiResponse<List<SanLuongDto>>.Fail($"Không tìm thấy máy {request.May}!");

        if (!await _machineRouter.PingAsync(ip))
            return ApiResponse<List<SanLuongDto>>.Fail("Máy đang tắt, vui lòng mở máy và thử lại!");

        var mfnsConnStr = _dbFactory.CreateMfnsConnection(ip).ConnectionString;

        var prdebeData = await _repo.GetPrdebeDataAsync(
            request.FromDay, request.ToDay, request.May, request.MaKeoTimKiem);
        var prdebeList = prdebeData.ToList();

        if (prdebeList.Count == 0)
            return ApiResponse<List<SanLuongDto>>.Fail($"Không có dữ liệu máy {request.May}");

        var tcData = await _repo.GetTieuChuanDataAsync(
            mfnsConnStr, request.FromDay, request.ToDay);
        var tcList = tcData.ToList();

        if (tcList.Count == 0)
            return ApiResponse<List<SanLuongDto>>.Fail("Không có dữ liệu! Gọi IT hoặc thử lại.");

        var result = new List<SanLuongDto>();

        foreach (var item in prdebeList)
        {
            string mesid = item.mesid?.ToString()?.Trim() ?? "";
            foreach (var tc in tcList)
            {
                string planId = tc.Plan_Id?.ToString()?.Trim() ?? "";
                if (mesid == planId)
                {
                    string machno = item.machno?.ToString()?.Trim() ?? "";
                    string soMay = machno.Length >= 8 ? machno.Substring(6, 2) : machno;
                    int sokgTotal = int.Parse(tc.SokgTotal?.ToString() ?? "0");
                    int wgt = int.Parse(item.wgt?.ToString() ?? "0");

                    result.Add(new SanLuongDto
                    {
                        MaMesid = mesid,
                        SoMay = soMay,
                        TenKeo = item.partno?.ToString()?.Trim() ?? "",
                        SoLo = item.slipno?.ToString()?.Trim() ?? "",
                        SoMeDieuDong = tc.Plan_Num?.ToString()?.Trim() ?? "",
                        SoMeHoanThanh = tc.FinishNum?.ToString()?.Trim() ?? "",
                        SoKyTieuChuan = tc.SokgTC?.ToString()?.Trim() ?? "",
                        SoKyDaQuetTem = wgt.ToString(),
                        SoKyHoanThanh = sokgTotal.ToString(),
                        SoKyChenhLech = (sokgTotal - wgt).ToString()
                    });
                }
            }
        }

        return ApiResponse<List<SanLuongDto>>.Ok(result);
    }
}
