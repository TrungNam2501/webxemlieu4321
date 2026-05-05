namespace KendaWeb.Api.Models.DTOs;

public class DoNguocDto
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

public class DoNguocRLDto
{
    public string? Pday { get; set; }
    public string? Class { get; set; }
    public string? Machno { get; set; }
    public string? Mesid { get; set; }
    public string? Barcode { get; set; }
    public string? Partno { get; set; }
    public string? Qty { get; set; }
    public string? Bacode { get; set; }
    public string? Itnbr { get; set; }
    public string? Slipno { get; set; }
    public string? Intime { get; set; }
    public string? Indat { get; set; }
    public string? Usrno { get; set; }
}

public class DoNguocPrdebeDto
{
    public string? Mesid { get; set; }
    public string? Machno { get; set; }
    public string? Prodat { get; set; }
    public string? Partno { get; set; }
    public string? Indat { get; set; }
}
