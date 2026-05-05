namespace KendaWeb.Api.Models.DTOs;

public class HoaChatDto
{
    public string? DosingId { get; set; }
    public string? PlanId { get; set; }
    public string? EquipCode { get; set; }
    public int SerialNum { get; set; }
    public string? WeightId { get; set; }
    public string? MaterialCode { get; set; }
    public string? MaterialName { get; set; }
    public decimal? RealWeight { get; set; }
    public decimal? RealError { get; set; }
    public decimal? OverWeight { get; set; }
    public decimal? OverError { get; set; }
    public string? WasteTime { get; set; }
    public string? WarningSign { get; set; }
    public string? WeightTime { get; set; }
    public string? BatchNumber { get; set; }
    public string? RecipeCode { get; set; }
}

public class BarcodeLogDto
{
    public string? ScanState { get; set; }
    public string? EquipCode { get; set; }
    public string? Material { get; set; }
    public string? ScanBar { get; set; }
    public string? ScanTime { get; set; }
    public string? Bin { get; set; }
}
