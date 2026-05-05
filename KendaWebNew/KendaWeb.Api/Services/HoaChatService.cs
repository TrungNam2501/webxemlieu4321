using KendaWeb.Api.Configuration;
using KendaWeb.Api.Models.DTOs;
using KendaWeb.Api.Repositories;

namespace KendaWeb.Api.Services;

public interface IHoaChatService
{
    Task<ApiResponse<List<HoaChatDto>>> GetHoaChatAsync(string barcode);
    Task<ApiResponse<List<BarcodeLogDto>>> GetBarcodeLogAsync(
        string equipCode, string materialName, string materialCode, string recordTime);
}

public class HoaChatService : IHoaChatService
{
    private readonly IHoaChatRepository _repo;
    private readonly IMachineRouter _machineRouter;
    private readonly IDbConnectionFactory _dbFactory;

    public HoaChatService(
        IHoaChatRepository repo,
        IMachineRouter machineRouter,
        IDbConnectionFactory dbFactory)
    {
        _repo = repo;
        _machineRouter = machineRouter;
        _dbFactory = dbFactory;
    }

    public async Task<ApiResponse<List<HoaChatDto>>> GetHoaChatAsync(string barcode)
    {
        if (string.IsNullOrEmpty(barcode) || barcode.Length < 3)
            return ApiResponse<List<HoaChatDto>>.Fail("Barcode không hợp lệ!");

        var prefix = barcode.Substring(0, 3).Trim();
        var ip = GetCwssIpByPrefix(prefix);

        if (ip == null)
            return ApiResponse<List<HoaChatDto>>.Fail("Không tìm thấy máy hóa chất!");

        if (!await _machineRouter.PingAsync(ip))
            return ApiResponse<List<HoaChatDto>>.Fail("Máy đang tắt, vui lòng mở máy và thử lại!");

        var connStr = _dbFactory.CreateCwssConnection(ip).ConnectionString;
        var planId = barcode.Length >= 13 ? barcode.Substring(0, 13).Trim() : barcode;

        var data = (await _repo.GetHoaChatDataAsync(connStr, planId)).ToList();

        if (data.Count == 0)
            return ApiResponse<List<HoaChatDto>>.Fail("Không có dữ liệu");

        // Map equipment display names
        foreach (var item in data)
        {
            item.EquipCode = MapEquipCodeDisplay(item.EquipCode ?? "");
        }

        return ApiResponse<List<HoaChatDto>>.Ok(data);
    }

    public async Task<ApiResponse<List<BarcodeLogDto>>> GetBarcodeLogAsync(
        string equipCode, string materialName, string materialCode, string recordTime)
    {
        var ip = _machineRouter.GetCwssIp(equipCode);
        if (ip == null)
            return ApiResponse<List<BarcodeLogDto>>.Fail("Không tìm thấy máy!");

        if (!await _machineRouter.PingAsync(ip))
            return ApiResponse<List<BarcodeLogDto>>.Fail("Máy đang tắt!");

        var connStr = _dbFactory.CreateCwssConnection(ip).ConnectionString;
        var materialCode5 = materialCode.Length >= 5 ? materialCode.Substring(0, 5) : materialCode;

        var data = (await _repo.GetBarcodeLogAsync(connStr, materialName, materialCode5, recordTime)).ToList();

        if (data.Count == 0)
            return ApiResponse<List<BarcodeLogDto>>.Fail("Không có dữ liệu");

        return ApiResponse<List<BarcodeLogDto>>.Ok(data);
    }

    private string? GetCwssIpByPrefix(string prefix)
    {
        return prefix switch
        {
            "V11" => _machineRouter.GetCwssIp("01"),  // 198.1.8.16
            "V13" => _machineRouter.GetCwssIp("03"),  // 198.1.8.15
            "V12" => _machineRouter.GetCwssIp("02"),  // 198.1.8.17
            "V14" => _machineRouter.GetCwssIp("04"),  // 198.1.8.18
            _ => null
        };
    }

    private static string MapEquipCodeDisplay(string equipCode)
    {
        return equipCode switch
        {
            "01" => "-1",
            "03" => "-9",
            "02" => "-1 Mới",
            "04" => "-9 Mới",
            _ => equipCode
        };
    }
}
