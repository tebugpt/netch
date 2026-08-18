using Netch.Models;

namespace Netch.Servers;

public class Hysteria2Server : Server
{
    public override string Type { get; } = "Hysteria2";

    public override string MaskedData()
    {
        return "";
    }

    /// <summary>
    ///     认证密码 (auth)
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     SNI，留空则使用 Hostname
    /// </summary>
    public string? SNI { get; set; }

    /// <summary>
    ///     跳过证书验证 (insecure)
    /// </summary>
    public bool Insecure { get; set; }

    /// <summary>
    ///     混淆类型，目前仅支持 salamander，留空则不启用混淆
    /// </summary>
    public string? ObfsType { get; set; }

    /// <summary>
    ///     混淆密码
    /// </summary>
    public string? ObfsPassword { get; set; }

    /// <summary>
    ///     上行带宽，例如 "10 mbps"，留空则不设置（使用 BBR）
    /// </summary>
    public string? UpMbps { get; set; }

    /// <summary>
    ///     下行带宽，例如 "50 mbps"，留空则不设置（使用 BBR）
    /// </summary>
    public string? DownMbps { get; set; }
}
