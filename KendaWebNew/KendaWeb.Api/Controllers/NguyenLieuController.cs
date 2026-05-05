using KendaWeb.Api.Models.DTOs;
using KendaWeb.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KendaWeb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NguyenLieuController : ControllerBase
{
    private readonly INguyenLieuService _service;
    private readonly IExcelExportService _excelService;

    public NguyenLieuController(INguyenLieuService service, IExcelExportService excelService)
    {
        _service = service;
        _excelService = excelService;
    }

    [HttpGet("{mesId}")]
    public async Task<IActionResult> GetNguyenLieu(string mesId, [FromQuery] string soMay)
    {
        var request = new NguyenLieuRequest
        {
            MesId = mesId,
            SoMay = soMay
        };

        var result = await _service.GetNguyenLieuAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{mesId}/export-excel")]
    public async Task<IActionResult> ExportExcel(string mesId, [FromQuery] string soMay)
    {
        var request = new NguyenLieuRequest
        {
            MesId = mesId,
            SoMay = soMay
        };

        var result = await _service.GetNguyenLieuAsync(request);

        if (!result.Success || result.Data == null)
            return BadRequest(result);

        var bytes = _excelService.ExportToExcel(result.Data, "NguyenLieu", "San luong chi tiet MES.xlsx");

        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "San luong chi tiet MES.xlsx");
    }
}
