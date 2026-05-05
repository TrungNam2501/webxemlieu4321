using Dapper;
using KendaWeb.Api.Configuration;
using KendaWeb.Api.Models.DTOs;

namespace KendaWeb.Api.Repositories;

public interface ISanLuongRepository
{
    Task<IEnumerable<dynamic>> GetPrdebeDataAsync(string fromDay, string toDay, string may, string? maKeo = null);
    Task<IEnumerable<dynamic>> GetTieuChuanDataAsync(string connectionString, string fromDay, string toDay);
}

public class SanLuongRepository : ISanLuongRepository
{
    private readonly IDbConnectionFactory _dbFactory;
    private readonly ILogger<SanLuongRepository> _logger;

    public SanLuongRepository(IDbConnectionFactory dbFactory, ILogger<SanLuongRepository> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<dynamic>> GetPrdebeDataAsync(
        string fromDay, string toDay, string may, string? maKeo = null)
    {
        var fromDay1 = fromDay.Replace("-", "");
        var toDay1 = toDay.Replace("-", "");

        var sql = @"
            SELECT mesid, machno, partno, slipno, CAST(SUM(weight) AS INT) AS wgt
            FROM [erp].[dbo].[prdebe]
            WHERE prodat BETWEEN @FromDay AND @ToDay
              AND machno LIKE @MayPattern
              AND LEFT(mesid, 1) NOT IN ('V', 'E', '')";

        if (!string.IsNullOrEmpty(maKeo))
        {
            sql += " AND partno LIKE @MaKeoPattern";
        }

        sql += " GROUP BY mesid, machno, partno, slipno;";

        var parameters = new
        {
            FromDay = fromDay1,
            ToDay = toDay1,
            MayPattern = $"%{may}",
            MaKeoPattern = $"%{maKeo}%"
        };

        _logger.LogInformation("[GetPrdebeDataAsync] SQL: {Sql}", sql);
        _logger.LogInformation("[GetPrdebeDataAsync] Params: FromDay={FromDay}, ToDay={ToDay}, MayPattern={MayPattern}, MaKeoPattern={MaKeoPattern}",
            fromDay1, toDay1, $"%{may}", $"%{maKeo}%");

        using var conn = _dbFactory.CreateErpConnection();
        _logger.LogInformation("[GetPrdebeDataAsync] ConnectionString: {ConnStr}",
            conn.ConnectionString?.Replace("Password=", "Password=***"));

        try
        {
            var result = (await conn.QueryAsync(sql, parameters)).ToList();
            _logger.LogInformation("[GetPrdebeDataAsync] Result count: {Count}", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GetPrdebeDataAsync] Query failed");
            throw;
        }
    }

    public async Task<IEnumerable<dynamic>> GetTieuChuanDataAsync(
        string connectionString, string fromDay, string toDay)
    {
        var sql = @"
            SELECT a.Equip_ID,
                   a.Plan_Id,
                   a.Recipe_Code,
                   CASE WHEN a.Shift_Id = 1 THEN '2' ELSE '1' END
                     + RIGHT('0' + CAST(a.Equip_ID AS VARCHAR), 2)
                     + '-' + SUBSTRING(a.P_Date, 6, 2)
                     + SUBSTRING(a.P_Date, 9, 2) AS solo,
                   a.Plan_Num,
                   b.FinishNum,
                   CAST(SUM(c.set_weight) AS INT) AS SokgTC,
                   CAST(b.FinishNum * SUM(c.set_weight) AS INT) AS SokgTotal
            FROM [mfnsShareDB].[dbo].[IF_RtPlan2Mixing] a
                JOIN [mfns].[dbo].[Ppt_GroupLot] b ON a.Plan_Id = b.MesPlanID
                JOIN [mfns].[dbo].[pmt_weigh] c ON b.RecipeCode = c.father_code
            WHERE LEFT(a.Plan_Id, 1) != 'V'
              AND b.FinishNum != '0'
              AND a.P_Date BETWEEN @FromDay AND @ToDay
            GROUP BY a.Equip_ID, a.Plan_Id, a.Recipe_Code,
                     a.Shift_Id, a.P_Date, a.Plan_Num, b.FinishNum";

        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        return await conn.QueryAsync(sql, new { FromDay = fromDay, ToDay = toDay });
    }
}
