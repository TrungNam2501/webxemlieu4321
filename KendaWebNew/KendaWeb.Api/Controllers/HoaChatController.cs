using KendaWeb.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KendaWeb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HoaChatController : ControllerBase
{
    private readonly IHoaChatService _service;
    private readonly IExcelExportService _excelService;

    public HoaChatController(IHoaChatService service, IExcelExportService excelService)
    {
        _service = service;
        _excelService = excelService;
    }

    [HttpGet("{barcode}")]
    public async Task<IActionResult> GetHoaChat(string barcode)
    {
        var result = await _service.GetHoaChatAsync(barcode);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("barcode-log")]
    public async Task<IActionResult> GetBarcodeLog(
        [FromQuery] string equipCode,
        [FromQuery] string materialName,
        [FromQuery] string materialCode,
        [FromQuery] string recordTime)
    {
        var result = await _service.GetBarcodeLogAsync(equipCode, materialName, materialCode, recordTime);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{barcode}/export-excel")]
    public async Task<IActionResult> ExportExcel(string barcode)
    {
        var result = await _service.GetHoaChatAsync(barcode);

        if (!result.Success || result.Data == null)
            return BadRequest(result);

        var bytes = _excelService.ExportToExcel(result.Data, "HoaChat", "San luong chi tiet MES.xlsx");

        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "San luong chi tiet MES.xlsx");
    }
}
