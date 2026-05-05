using System.Net.NetworkInformation;
using Microsoft.Extensions.Options;

namespace KendaWeb.Api.Configuration;

public interface IMachineRouter
{
    string? GetMfnsIp(string machineCode);
    string? GetCwssIp(string equipCode);
    Task<bool> PingAsync(string ip, int timeoutMs = 500);
}

public class MachineRouter : IMachineRouter
{
    private readonly MachineConfig _config;

    public MachineRouter(IOptions<MachineConfig> config)
    {
        _config = config.Value;
    }

    public string? GetMfnsIp(string machineCode)
    {
        return _config.MfnsMachines.TryGetValue(machineCode, out var ip) ? ip : null;
    }

    public string? GetCwssIp(string equipCode)
    {
        return _config.CwssMachines.TryGetValue(equipCode, out var ip) ? ip : null;
    }

    public async Task<bool> PingAsync(string ip, int timeoutMs = 500)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(ip, timeoutMs);
            return reply.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }
}
