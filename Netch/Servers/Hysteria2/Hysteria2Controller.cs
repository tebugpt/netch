using System.Net;
using System.Text.Json;
using Netch.Controllers;
using Netch.Interfaces;
using Netch.Models;
using Netch.Utils;

namespace Netch.Servers;

/// <summary>
///     需要把官方 hysteria 客户端可执行文件放到 bin\hysteria.exe
///     下载地址: https://github.com/apernet/hysteria/releases (取 hysteria-windows-amd64.exe，改名为 hysteria.exe)
/// </summary>
public class Hysteria2Controller : Guard, IServerController
{
    public Hysteria2Controller() : base("hysteria.exe")
    {
    }

    protected override IEnumerable<string> StartedKeywords => new[] { "SOCKS5 server listening" };

    protected override IEnumerable<string> FailedKeywords => new[] { "level=fatal", "FATAL" };

    public override string Name => "Hysteria2";

    public ushort? Socks5LocalPort { get; set; }

    public string? LocalAddress { get; set; }

    public async Task<Socks5Server> StartAsync(Server s)
    {
        var server = (Hysteria2Server)s;

        var config = new Hysteria2Config
        {
            server = $"{await server.AutoResolveHostnameAsync()}:{server.Port}",
            auth = server.Password,
            tls = new Hysteria2TLS
            {
                sni = server.SNI.ValueOrDefault() ?? server.Hostname,
                insecure = server.Insecure
            },
            socks5 = new Hysteria2Socks5
            {
                listen = $"{this.LocalAddress()}:{this.Socks5LocalPort()}"
            }
        };

        if (!server.ObfsType.IsNullOrWhiteSpace())
        {
            config.obfs = new Hysteria2Obfs
            {
                type = server.ObfsType!,
                salamander = new Hysteria2ObfsSalamander
                {
                    password = server.ObfsPassword ?? string.Empty
                }
            };
        }

        if (!server.UpMbps.IsNullOrWhiteSpace() || !server.DownMbps.IsNullOrWhiteSpace())
        {
            config.bandwidth = new Hysteria2Bandwidth
            {
                up = server.UpMbps ?? string.Empty,
                down = server.DownMbps ?? string.Empty
            };
        }

        // Constants.TempConfig 固定是 data\last.json，hysteria (viper) 会按 .json 扩展名正确解析
        await using (var fileStream = new FileStream(Constants.TempConfig, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            await JsonSerializer.SerializeAsync(fileStream, config, Global.NewCustomJsonSerializerOptions());
        }

        await StartGuardAsync("client -c ..\\data\\last.json");
        return new Socks5Server(IPAddress.Loopback.ToString(), this.Socks5LocalPort(), server.Hostname);
    }
}
