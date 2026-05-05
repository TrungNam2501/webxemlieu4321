namespace KendaWeb.Api.Configuration;

public class MachineConfig
{
    public Dictionary<string, string> MfnsMachines { get; set; } = new();
    public Dictionary<string, string> CwssMachines { get; set; } = new();
    public string MfnsDbTemplate { get; set; } = string.Empty;
    public string CwssDbTemplate { get; set; } = string.Empty;
}
