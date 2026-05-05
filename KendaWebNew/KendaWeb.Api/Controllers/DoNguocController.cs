using KendaWeb.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KendaWeb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoNguocController : ControllerBase
{
    private readonly IDoNguocService _service;
    private readonly IExcelExportService _excelService;

    public DoNguocController(IDoNguocService service, IExcelExportService excelService)
    {
        _service = service;
        _excelService = excelService;
    }

    [HttpGet("rl/{barcode}")]
    public async Task<IActionResult> GetDoNguocRL(string barcode)
    {
        var result = await _service.GetDoNguocRLAsync(barcode);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("rb/{barcode}")]
    public async Task<IActionResult> GetDoNguocRB(string barcode)
    {
        var result = await _service.GetDoNguocRBAsync(barcode);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("rb/{barcode}/export-excel")]
    public async Task<IActionResult> ExportExcelRB(string barcode)
    {
        var result = await _service.GetDoNguocRBAsync(barcode);

        if (!result.Success || result.Data == null)
            return BadRequest(result);

        var bytes = _excelService.ExportToExcel(result.Data, "DoNguoc", "San luong chi tiet MES.xlsx");

        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "San luong chi tiet MES.xlsx");
    }
}
