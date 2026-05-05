using Dapper;
using KendaWeb.Api.Models.DTOs;

namespace KendaWeb.Api.Repositories;

public interface IHoaChatRepository
{
    Task<IEnumerable<HoaChatDto>> GetHoaChatDataAsync(string connectionString, string planId);
    Task<IEnumerable<BarcodeLogDto>> GetBarcodeLogAsync(
        string connectionString, string materialName, string materialCode, string recordTime);
}

public class HoaChatRepository : IHoaChatRepository
{
    public async Task<IEnumerable<HoaChatDto>> GetHoaChatDataAsync(
        string connectionString, string planId)
    {
        var sql = @"
            SELECT w.Dosing_id AS DosingId, w.Plan_id AS PlanId,
                   w.Equip_code AS EquipCode, w.Serial_Num AS SerialNum,
                   w.Weight_ID AS WeightId, w.Material_Code AS MaterialCode,
                   n.Material_name AS MaterialName, w.Real_Weight AS RealWeight,
                   w.Real_Error AS RealError, w.Over_Weight AS OverWeight,
                   w.Over_Error AS OverError, w.Waste_Time AS WasteTime,
                   w.Warning_Sign AS WarningSign, w.Weight_Time AS WeightTime,
                   w.Batch_Number AS BatchNumber, w.Recipe_code AS RecipeCode
            FROM [CWSS_S7].[dbo].[LR_weigh] w
            INNER JOIN [CWSS_S7].[dbo].[Pmt_material] n
                ON w.Material_Code = n.Material_code
            WHERE Plan_id = @PlanId
            ORDER BY w.serial_num, w.Weight_Time";

        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        return await conn.QueryAsync<HoaChatDto>(sql, new { PlanId = planId });
    }

    public async Task<IEnumerable<BarcodeLogDto>> GetBarcodeLogAsync(
        string connectionString, string materialName, string materialCode, string recordTime)
    {
        var sql = @"
            SELECT TOP 1 Scan_State AS ScanState, Equip_Code AS EquipCode,
                   Material, Scan_Bar AS ScanBar, Scan_Time AS ScanTime, Bin
            FROM [CWSS_S7].[dbo].[LR_BarcodeLog]
            WHERE [Material] = @MaterialName
              AND SUBSTRING(CONVERT(VARCHAR, Scan_bar), 1, 5) = @MaterialCode
              AND CONVERT(DATETIME, [Scan_Time], 102) < CONVERT(DATETIME, @RecordTime, 102)
            ORDER BY Scan_Time DESC";

        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        return await conn.QueryAsync<BarcodeLogDto>(sql, new
        {
            MaterialName = materialName,
            MaterialCode = materialCode,
            RecordTime = recordTime
        });
    }
}
