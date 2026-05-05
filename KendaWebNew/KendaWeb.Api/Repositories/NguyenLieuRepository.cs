using Dapper;
using KendaWeb.Api.Configuration;

namespace KendaWeb.Api.Repositories;

public interface INguyenLieuRepository
{
    Task<string?> GetPlanIdAsync(string connectionString, string mesId);
    Task<string?> GetRecipeCodeAsync(string connectionString, string mesId);
    Task<IEnumerable<dynamic>> GetWeighDataWithBarcodeAsync(string connectionString, string planId);
    Task<IEnumerable<dynamic>> GetMaterialNamesAsync(string connectionString, string codePrefix);
    Task<string?> GetFirstBarcodeAsync(string connectionString, string planId);
    Task<IEnumerable<dynamic>> GetOilCoalDataAsync(string connectionString, string planId);
    Task<IEnumerable<dynamic>> GetCoalBarcodeAsync(string connectionString, string fromDay, string coalCodes);
}

public class NguyenLieuRepository : INguyenLieuRepository
{
    public async Task<string?> GetPlanIdAsync(string connectionString, string mesId)
    {
        var sql = @"SELECT Plan_ID, RecipeCode
                    FROM [mfns].[dbo].[Ppt_GroupLot]
                    WHERE MesPlanID = @MesId";
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        var row = await conn.QueryFirstOrDefaultAsync(sql, new { MesId = mesId });
        return row?.Plan_ID?.ToString()?.Trim();
    }

    public async Task<string?> GetRecipeCodeAsync(string connectionString, string mesId)
    {
        var sql = @"SELECT RecipeCode
                    FROM [mfns].[dbo].[Ppt_GroupLot]
                    WHERE MesPlanID = @MesId";
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        var row = await conn.QueryFirstOrDefaultAsync(sql, new { MesId = mesId });
        return row?.RecipeCode?.ToString()?.Trim();
    }

    public async Task<IEnumerable<dynamic>> GetWeighDataWithBarcodeAsync(
        string connectionString, string planId)
    {
        var sql = @"
            WITH ranked_b AS (
                SELECT b.*,
                    ROW_NUMBER() OVER (
                        PARTITION BY b.Barcode, b.mater_code, b.Mater_Type
                        ORDER BY b.SaveTime DESC
                    ) AS rn
                FROM [mfns].[dbo].[Ppt_BarCodeRep] b
                WHERE b.Plan_ID = @PlanId
                  AND SUBSTRING(b.mater_code, 1, 3) <> '680'
            ),
            filtered_b AS (
                SELECT * FROM ranked_b WHERE rn = 1
            ),
            c AS (
                SELECT b.SaveTime,
                    b.Recipe_Name,
                    b.Set_Num,
                    b.Serial_Num,
                    b.Mater_Code,
                    b.Mater_Name,
                    b.Mater_Barcode,
                    CASE
                        WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'V'
                            THEN '20' + SUBSTRING(Mater_Barcode, 4, 6)
                        WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'A'
                            THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS VARCHAR)
                                + '0'
                                + CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS VARCHAR)
                                + '-10' + SUBSTRING(Mater_Barcode, 6, 2)
                        WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'B'
                            THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS VARCHAR)
                                + '0'
                                + CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS VARCHAR)
                                + '-11' + SUBSTRING(Mater_Barcode, 6, 2)
                        WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R' AND SUBSTRING(Mater_Barcode, 5, 1) = 'C'
                            THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS VARCHAR)
                                + '0'
                                + CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS VARCHAR)
                                + '-12' + SUBSTRING(Mater_Barcode, 6, 2)
                        WHEN SUBSTRING(Mater_Barcode, 1, 1) = 'R'
                            THEN CAST((SELECT Shift FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS VARCHAR)
                                + '0'
                                + CAST((SELECT Equip_ID FROM [mfnsShareDB].[dbo].[IF_Datatem] WHERE BarCodeLab = Mater_Barcode) AS VARCHAR)
                                + '-0' + SUBSTRING(Mater_Barcode, 5, 3)
                        ELSE '20' + SUBSTRING(Mater_Barcode, 8, 6)
                    END AS batchno,
                    b.Equip_ID,
                    a.real_weight,
                    a.error_allow,
                    b.Mater_Type,
                    a.weight_id
                FROM [mfns].[dbo].[ppt_weigh] a
                LEFT JOIN filtered_b b ON a.barcode = b.Barcode
                    AND b.mater_code = a.mater_code
                    AND a.weight_id = b.Mater_Type
                WHERE a.barcode IS NOT NULL
            )
            SELECT DISTINCT *
            FROM c
            WHERE batchno IS NOT NULL
              AND (Mater_Code NOT LIKE '60%'
                   OR (Mater_Code LIKE '60%' AND (Mater_Code = Mater_Name OR Mater_Type = 1)))
            ORDER BY SaveTime";

        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        return await conn.QueryAsync(sql, new { PlanId = planId });
    }

    public async Task<IEnumerable<dynamic>> GetMaterialNamesAsync(
        string connectionString, string codePrefix)
    {
        var sql = @"SELECT mater_code, mater_name
                    FROM [mfns].[dbo].[pmt_material]
                    WHERE mater_code LIKE @Prefix";
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        return await conn.QueryAsync(sql, new { Prefix = $"{codePrefix}%" });
    }

    public async Task<string?> GetFirstBarcodeAsync(string connectionString, string planId)
    {
        var sql = @"SELECT TOP 1 [Barcode]
                    FROM [mfns].[dbo].[Ppt_BarCodeRep]
                    WHERE Plan_ID = @PlanId";
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        return await conn.QueryFirstOrDefaultAsync<string>(sql, new { PlanId = planId });
    }

    public async Task<IEnumerable<dynamic>> GetOilCoalDataAsync(
        string connectionString, string planId)
    {
        var sql = @"
            SELECT a.[barcode], a.[mater_code], a.[equip_code], a.[set_weight],
                   CONVERT(NVARCHAR(20), a.[weigh_time], 120) AS weigh_time,
                   a.[error_allow], a.[weigh_type], b.mater_name
            FROM [mfns].[dbo].[ppt_weigh] a
            JOIN [mfns].[dbo].[pmt_material] b ON a.mater_code = b.mater_code
            WHERE barcode LIKE @PlanIdPattern
              AND weigh_type = N'油料'
            ORDER BY weigh_time ASC";

        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        return await conn.QueryAsync(sql, new { PlanIdPattern = $"{planId}%" });
    }

    public async Task<IEnumerable<dynamic>> GetCoalBarcodeAsync(
        string connectionString, string fromDay, string coalCodes)
    {
        // coalCodes is a comma-separated list of codes (already validated)
        // We use a dynamic IN clause with individual parameters
        var sql = $@"
            SELECT Mater_Code, Mater_Barcode, SaveTime
            FROM (
                SELECT Mater_Code, Mater_Barcode, SaveTime,
                       ROW_NUMBER() OVER (PARTITION BY Mater_Code ORDER BY SaveTime DESC) AS rn
                FROM [mfns].[dbo].[Ppt_Oil]
                WHERE SaveTime <= @FromDay
                  AND Mater_Type = '0'
                  AND Mater_Code IN ({coalCodes})
            ) t
            WHERE rn = 1";

        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        return await conn.QueryAsync(sql, new { FromDay = fromDay });
    }
}
