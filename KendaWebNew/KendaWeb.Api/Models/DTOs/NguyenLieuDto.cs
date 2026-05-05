namespace KendaWeb.Api.Models.DTOs;

public class NguyenLieuDto
{
    public string? SaveTime { get; set; }
    public string? RecipeName { get; set; }
    public int SetNum { get; set; }
    public int SerialNum { get; set; }
    public string? MaterCode { get; set; }
    public string? MaterName { get; set; }
    public string? MaterBarcode { get; set; }
    public string? BatchNo { get; set; }
    public string? EquipId { get; set; }
    public decimal? RealWeight { get; set; }
    public decimal? ErrorAllow { get; set; }
}

public class NguyenLieuRequest
{
    public string MesId { get; set; } = string.Empty;
    public string SoMay { get; set; } = string.Empty;
}
