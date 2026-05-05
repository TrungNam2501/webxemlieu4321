using Dapper;
using KendaWeb.Api.Configuration;
using KendaWeb.Api.Models.DTOs;

namespace KendaWeb.Api.Repositories;

public interface IDoNguocRepository
{
    Task<DoNguocPrdebeDto?> FindPrdebeByBarcodeAsync(string barcode);
    Task<IEnumerable<DoNguocRLDto>> GetDoNguocRLAsync(string barcode);
    Task<IEnumerable<DoNguocRLDto>> GetDoNguocRLByBacodeAsync(string bacode);
    Task<IEnumerable<DoNguocRLDto>> GetDoNguocRLWithSlipAsync(string barcode);
    Task<string?> GetPlanIdAsync(string connectionString, string mesId);
}

public class DoNguocRepository : IDoNguocRepository
{
    private readonly IDbConnectionFactory _dbFactory;

    public DoNguocRepository(IDbConnectionFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<DoNguocPrdebeDto?> FindPrdebeByBarcodeAsync(string barcode)
    {
        var sql = @"
            SELECT mesid AS Mesid, machno AS Machno, prodat AS Prodat,
                   partno AS Partno, indat AS Indat
            FROM [erp].[dbo].[prdebe]
            WHERE barcode = @Barcode
              AND LEFT(mesid, 1) NOT IN ('V', 'E', '')";

        using var conn = _dbFactory.CreateErpConnection();
        return await conn.QueryFirstOrDefaultAsync<DoNguocPrdebeDto>(sql, new { Barcode = barcode });
    }

    public async Task<IEnumerable<DoNguocRLDto>> GetDoNguocRLAsync(string barcode)
    {
        var sql = @"
            SELECT [pday] AS Pday, [class] AS Class, [machno] AS Machno,
                   [mesid] AS Mesid, [barcode] AS Barcode, [partno] AS Partno,
                   [qty] AS Qty, [bacode] AS Bacode, [itnbr] AS Itnbr,
                   [intime] AS Intime, [indat] AS Indat, [usrno] AS Usrno
            FROM [erp].[dbo].[prdebc]
            WHERE barcode = @Barcode";

        using var conn = _dbFactory.CreateBbConnection();
        return await conn.QueryAsync<DoNguocRLDto>(sql, new { Barcode = barcode });
    }

    public async Task<IEnumerable<DoNguocRLDto>> GetDoNguocRLByBacodeAsync(string bacode)
    {
        var sql = @"
            SELECT a.[pday] AS Pday, a.[class] AS Class, a.[machno] AS Machno,
                   a.[mesid] AS Mesid, a.barcode AS Barcode, a.[partno] AS Partno,
                   a.[qty] AS Qty, a.[bacode] AS Bacode, a.[itnbr] AS Itnbr,
                   b.slipno AS Slipno, a.[intime] AS Intime, a.[indat] AS Indat,
                   a.[usrno] AS Usrno
            FROM [erp].[dbo].[prdebc] a
            JOIN [erp].[dbo].[prdebe] b ON a.bacode = b.barcode
            WHERE a.barcode = @Bacode";

        using var conn = _dbFactory.CreateBbConnection();
        return await conn.QueryAsync<DoNguocRLDto>(sql, new { Bacode = bacode });
    }

    public async Task<IEnumerable<DoNguocRLDto>> GetDoNguocRLWithSlipAsync(string barcode)
    {
        var sql = @"
            SELECT a.[pday] AS Pday, a.[class] AS Class, a.[machno] AS Machno,
                   a.[mesid] AS Mesid, a.barcode AS Barcode, a.[partno] AS Partno,
                   a.[qty] AS Qty, a.[bacode] AS Bacode, a.[itnbr] AS Itnbr,
                   b.slipno AS Slipno, a.[intime] AS Intime, a.[indat] AS Indat,
                   a.[usrno] AS Usrno
            FROM [erp].[dbo].[prdebc] a
            JOIN [erp].[dbo].[prdebe] b ON a.bacode = b.barcode
            WHERE a.barcode = @Barcode";

        using var conn = _dbFactory.CreateBbConnection();
        return await conn.QueryAsync<DoNguocRLDto>(sql, new { Barcode = barcode });
    }

    public async Task<string?> GetPlanIdAsync(string connectionString, string mesId)
    {
        var sql = @"SELECT Plan_ID FROM [mfns].[dbo].[Ppt_GroupLot] WHERE MesPlanID = @MesId";
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        return await conn.QueryFirstOrDefaultAsync<string>(sql, new { MesId = mesId });
    }
}
