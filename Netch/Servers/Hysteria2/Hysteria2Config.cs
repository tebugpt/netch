#nullable disable
namespace Netch.Servers;

/// <summary>
///     对应 hysteria2 官方客户端配置 (server/auth/tls/obfs/bandwidth/socks5)
///     hysteria 底层用 viper 解析配置，会按文件扩展名自动识别 json/yaml，
///     Netch 统一使用 Constants.TempConfig（data\last.json），所以这里直接用 JSON 序列化即可。
/// </summary>
public class Hysteria2Config
{
    public string server { get; set; }

    public string auth { get; set; }

    public Hysteria2TLS tls { get; set; } = new();

    public Hysteria2Obfs obfs { get; set; }

    public Hysteria2Bandwidth bandwidth { get; set; }

    public Hysteria2Socks5 socks5 { get; set; } = new();

    /// <summary>
    ///     懒加载模式：只有在实际有流量时才与服务器建连
    /// </summary>
    public bool lazy { get; set; } = true;
}

public class Hysteria2TLS
{
    public string sni { get; set; }

    public bool insecure { get; set; }
}

public class Hysteria2Obfs
{
    public string type { get; set; } = "salamander";

    public Hysteria2ObfsSalamander salamander { get; set; } = new();
}

public class Hysteria2ObfsSalamander
{
    public string password { get; set; }
}

public class Hysteria2Bandwidth
{
    public string up { get; set; }

    public string down { get; set; }
}

public class Hysteria2Socks5
{
    public string listen { get; set; }
}
