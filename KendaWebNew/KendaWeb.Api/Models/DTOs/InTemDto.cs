namespace KendaWeb.Api.Models.DTOs;

public class InTemDto
{
    public string? Mesid { get; set; }
    public string? Machno { get; set; }
    public string? Daylimt { get; set; }
    public string? Barcode { get; set; }
    public string? Slipno { get; set; }
    public string? Weight { get; set; }
    public string? Prodat { get; set; }
    public string? Effdat { get; set; }
    public string? Class { get; set; }
    public string? Partno { get; set; }
    public string? Intime { get; set; }
    public string? Indat { get; set; }
    public string? Usrno { get; set; }
    public string? PalletNo { get; set; }
    public string? SomeSx { get; set; }
}

public class InTemResponse
{
    public int TotalCount { get; set; }
    public List<InTemDto> Items { get; set; } = new();
}
