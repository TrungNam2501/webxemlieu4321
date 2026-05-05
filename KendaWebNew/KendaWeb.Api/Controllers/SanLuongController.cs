using KendaWeb.Api.Models.DTOs;
using KendaWeb.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KendaWeb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SanLuongController : ControllerBase
{
    private readonly ISanLuongService _service;
    private readonly IExcelExportService _excelService;

    public SanLuongController(ISanLuongService service, IExcelExportService excelService)
    {
        _service = service;
        _excelService = excelService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSanLuong(
        [FromQuery] string may,
        [FromQuery] string fromDay,
        [FromQuery] string toDay,
        [FromQuery] string? maKeo = null)
    {
        var request = new SanLuongRequest
        {
            May = may,
            FromDay = fromDay,
            ToDay = toDay,
            MaKeoTimKiem = maKeo
        };

        var result = await _service.GetSanLuongAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> ExportExcel(
        [FromQuery] string may,
        [FromQuery] string fromDay,
        [FromQuery] string toDay,
        [FromQuery] string? maKeo = null)
    {
        var request = new SanLuongRequest
        {
            May = may,
            FromDay = fromDay,
            ToDay = toDay,
            MaKeoTimKiem = maKeo
        };

        var result = await _service.GetSanLuongAsync(request);

        if (!result.Success || result.Data == null)
            return BadRequest(result);

        var bytes = _excelService.ExportToExcel(
            result.Data, "MES", $"San luong BB {fromDay} - {toDay}.xlsx");

        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"San luong BB {fromDay} - {toDay}.xlsx");
    }
}
