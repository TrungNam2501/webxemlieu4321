using KendaWeb.Api.Configuration;
using KendaWeb.Api.Models.DTOs;
using KendaWeb.Api.Repositories;

namespace KendaWeb.Api.Services;

public interface INguyenLieuService
{
    Task<ApiResponse<List<NguyenLieuDto>>> GetNguyenLieuAsync(NguyenLieuRequest request);
}

public class NguyenLieuService : INguyenLieuService
{
    private readonly INguyenLieuRepository _repo;
    private readonly IMachineRouter _machineRouter;
    private readonly IDbConnectionFactory _dbFactory;

    public NguyenLieuService(
        INguyenLieuRepository repo,
        IMachineRouter machineRouter,
        IDbConnectionFactory dbFactory)
    {
        _repo = repo;
        _machineRouter = machineRouter;
        _dbFactory = dbFactory;
    }

    public async Task<ApiResponse<List<NguyenLieuDto>>> GetNguyenLieuAsync(NguyenLieuRequest request)
    {
        var ip = _machineRouter.GetMfnsIp(request.SoMay);
        if (ip == null)
            return ApiResponse<List<NguyenLieuDto>>.Fail("Không tìm thấy máy!");

        if (!await _machineRouter.PingAsync(ip))
            return ApiResponse<List<NguyenLieuDto>>.Fail("Máy đang tắt, vui lòng mở máy và thử lại!");

        var connStr = _dbFactory.CreateMfnsConnection(ip).ConnectionString;

        var planId = await _repo.GetPlanIdAsync(connStr, request.MesId);
        if (string.IsNullOrEmpty(planId))
            return ApiResponse<List<NguyenLieuDto>>.Fail("Không tìm thấy Plan_ID!");

        var rawData = (await _repo.GetWeighDataWithBarcodeAsync(connStr, planId)).ToList();

        if (rawData.Count == 0)
            return ApiResponse<List<NguyenLieuDto>>.Fail(
                "Mã mes này chưa kết thúc hoặc đánh tay, không thể xem dữ liệu quét tem!");

        var result = new List<NguyenLieuDto>();
        foreach (var r in rawData)
        {
            result.Add(MapToNguyenLieuDto(r));
        }

        // Replace material names for codes starting with '60'
        var materialNames = (await _repo.GetMaterialNamesAsync(connStr, "60")).ToList();
        var nameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var m in materialNames)
        {
            string key = m.mater_code?.ToString()?.Trim() ?? "";
            string val = m.mater_name?.ToString()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(key))
                nameMap[key] = val;
        }

        foreach (var item in result)
        {
            if (nameMap.TryGetValue(item.MaterCode ?? "", out string? name))
                item.MaterName = name;
        }

        // Handle oil/coal data
        await AppendOilCoalDataAsync(connStr, planId, result);

        result = result
            .Where(r => !string.IsNullOrEmpty(r.BatchNo))
            .OrderBy(r => r.SerialNum)
            .ToList();

        return ApiResponse<List<NguyenLieuDto>>.Ok(result);
    }

    private static NguyenLieuDto MapToNguyenLieuDto(dynamic r)
    {
        int setNum = 0;
        int serialNum = 0;
        decimal? realWeight = null;
        decimal? errorAllow = null;

        int.TryParse(r.Set_Num?.ToString(), out setNum);
        int.TryParse(r.Serial_Num?.ToString(), out serialNum);
        if (decimal.TryParse(r.real_weight?.ToString(), out decimal rw)) realWeight = rw;
        if (decimal.TryParse(r.error_allow?.ToString(), out decimal ea)) errorAllow = ea;

        return new NguyenLieuDto
        {
            SaveTime = r.SaveTime?.ToString(),
            RecipeName = r.Recipe_Name?.ToString(),
            SetNum = setNum,
            SerialNum = serialNum,
            MaterCode = r.Mater_Code?.ToString(),
            MaterName = r.Mater_Name?.ToString(),
            MaterBarcode = r.Mater_Barcode?.ToString(),
            BatchNo = r.batchno?.ToString(),
            EquipId = r.Equip_ID?.ToString(),
            RealWeight = realWeight,
            ErrorAllow = errorAllow,
        };
    }

    private async Task AppendOilCoalDataAsync(
        string connStr, string planId, List<NguyenLieuDto> result)
    {
        var firstBarcode = await _repo.GetFirstBarcodeAsync(connStr, planId);
        if (string.IsNullOrEmpty(firstBarcode))
            return;

        var oilCoalData = (await _repo.GetOilCoalDataAsync(connStr, planId)).ToList();
        if (oilCoalData.Count == 0)
            return;

        string firstWeighTime = oilCoalData[0].weigh_time?.ToString()?.Trim() ?? "";
        if (string.IsNullOrEmpty(firstWeighTime))
            return;

        // Collect unique coal material codes
        var coalCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in oilCoalData)
        {
            string weighType = row.weigh_type?.ToString()?.Trim() ?? "";
            if (weighType == "油料")
            {
                string code = row.mater_code?.ToString()?.Trim() ?? "";
                if (!string.IsNullOrEmpty(code))
                    coalCodes.Add(code);
            }
        }

        if (coalCodes.Count == 0)
            return;

        // Build parameterized coal codes for IN clause
        var coalCodesSql = string.Join(",", coalCodes.Select(c => $"'{c}'"));
        var coalBarcodes = (await _repo.GetCoalBarcodeAsync(connStr, firstWeighTime, coalCodesSql)).ToList();

        var barcodeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in coalBarcodes)
        {
            string mcode = row.Mater_Code?.ToString()?.Trim() ?? "";
            string mbar = row.Mater_Barcode?.ToString()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(mcode) && !string.IsNullOrEmpty(mbar))
            {
                if (!barcodeMap.ContainsKey(mcode))
                    barcodeMap[mcode] = mbar;
            }
        }

        if (result.Count == 0)
            return;

        var templateItem = result[0];

        foreach (var row in oilCoalData)
        {
            string oldBarcode = row.barcode?.ToString()?.Trim() ?? "";
            int serial = 0;
            if (!string.IsNullOrEmpty(oldBarcode) && oldBarcode.Length >= 2)
                int.TryParse(oldBarcode.Substring(oldBarcode.Length - 2, 2), out serial);

            string curMater = row.mater_code?.ToString()?.Trim() ?? "";
            string materBarcode = "";
            string batchNo = templateItem.BatchNo ?? "";

            if (barcodeMap.TryGetValue(curMater, out var matchedBarcode) &&
                !string.IsNullOrEmpty(matchedBarcode))
            {
                if (matchedBarcode.Length >= 13 &&
                    matchedBarcode.StartsWith(curMater, StringComparison.OrdinalIgnoreCase))
                {
                    materBarcode = matchedBarcode;
                    batchNo = "20" + matchedBarcode.Substring(7, 6);
                }
            }

            decimal? realWeight = null;
            decimal? errorAllow = null;
            if (decimal.TryParse(row.set_weight?.ToString(), out decimal sw)) realWeight = sw;
            if (decimal.TryParse(row.error_allow?.ToString(), out decimal ea)) errorAllow = ea;

            result.Add(new NguyenLieuDto
            {
                SaveTime = row.weigh_time?.ToString()?.Trim(),
                RecipeName = templateItem.RecipeName,
                SetNum = templateItem.SetNum,
                SerialNum = serial,
                MaterCode = curMater,
                MaterName = row.mater_name?.ToString()?.Trim(),
                MaterBarcode = materBarcode,
                BatchNo = batchNo,
                EquipId = templateItem.EquipId,
                RealWeight = realWeight,
                ErrorAllow = errorAllow,
            });
        }
    }
}
