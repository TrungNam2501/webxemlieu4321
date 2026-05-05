using KendaWeb.Api.Models.DTOs;
using KendaWeb.Api.Repositories;

namespace KendaWeb.Api.Services;

public interface IInTemService
{
    Task<ApiResponse<InTemResponse>> GetInTemAsync(string mesId);
}

public class InTemService : IInTemService
{
    private readonly IInTemRepository _repo;

    public InTemService(IInTemRepository repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<InTemResponse>> GetInTemAsync(string mesId)
    {
        if (string.IsNullOrEmpty(mesId))
            return ApiResponse<InTemResponse>.Fail("Vui lòng nhập mã MES!");

        var items = (await _repo.GetInTemByMesIdAsync(mesId)).ToList();

        if (items.Count == 0)
            return ApiResponse<InTemResponse>.Fail("Mã mesid không tìm thấy!");

        return ApiResponse<InTemResponse>.Ok(new InTemResponse
        {
            TotalCount = items.Count,
            Items = items
        });
    }
}
