namespace KendaWeb.Api.Models.DTOs;

public class SanLuongDto
{
    public string MaMesid { get; set; } = string.Empty;
    public string SoMay { get; set; } = string.Empty;
    public string TenKeo { get; set; } = string.Empty;
    public string SoLo { get; set; } = string.Empty;
    public string SoMeDieuDong { get; set; } = string.Empty;
    public string SoMeHoanThanh { get; set; } = string.Empty;
    public string SoKyTieuChuan { get; set; } = string.Empty;
    public string SoKyDaQuetTem { get; set; } = string.Empty;
    public string SoKyHoanThanh { get; set; } = string.Empty;
    public string SoKyChenhLech { get; set; } = string.Empty;
}

public class SanLuongRequest
{
    public string May { get; set; } = string.Empty;
    public string FromDay { get; set; } = string.Empty;
    public string ToDay { get; set; } = string.Empty;
    public string? MaKeoTimKiem { get; set; }
}
