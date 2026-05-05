using KendaWeb.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KendaWeb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InTemController : ControllerBase
{
    private readonly IInTemService _service;
    private readonly IExcelExportService _excelService;

    public InTemController(IInTemService service, IExcelExportService excelService)
    {
        _service = service;
        _excelService = excelService;
    }

    [HttpGet("{mesId}")]
    public async Task<IActionResult> GetInTem(string mesId)
    {
        var result = await _service.GetInTemAsync(mesId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{mesId}/export-excel")]
    public async Task<IActionResult> ExportExcel(string mesId)
    {
        var result = await _service.GetInTemAsync(mesId);

        if (!result.Success || result.Data == null)
            return BadRequest(result);

        var bytes = _excelService.ExportToExcel(
            result.Data.Items, "InTem", "San luong chi tiet MES.xlsx");

        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "San luong chi tiet MES.xlsx");
    }
}
