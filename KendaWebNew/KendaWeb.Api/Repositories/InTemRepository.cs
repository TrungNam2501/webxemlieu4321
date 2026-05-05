using Dapper;
using KendaWeb.Api.Configuration;
using KendaWeb.Api.Models.DTOs;

namespace KendaWeb.Api.Repositories;

public interface IInTemRepository
{
    Task<IEnumerable<InTemDto>> GetInTemByMesIdAsync(string mesId);
}

public class InTemRepository : IInTemRepository
{
    private readonly IDbConnectionFactory _dbFactory;

    public InTemRepository(IDbConnectionFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<IEnumerable<InTemDto>> GetInTemByMesIdAsync(string mesId)
    {
        var sql = @"
            SELECT mesid AS Mesid, machno AS Machno, daylimt AS Daylimt,
                   barcode AS Barcode, slipno AS Slipno, weight AS Weight,
                   prodat AS Prodat, effdat AS Effdat, class AS Class,
                   partno AS Partno, intime AS Intime, indat AS Indat,
                   usrno AS Usrno, pallet_no AS PalletNo, some_sx AS SomeSx
            FROM [erp].[dbo].[prdebe]
            WHERE subno = 4 AND factory = 'V' AND mesid = @MesId
            ORDER BY indat DESC, intime DESC";

        using var conn = _dbFactory.CreateErpConnection();
        return await conn.QueryAsync<InTemDto>(sql, new { MesId = mesId });
    }
}
