using Netch.Models;

namespace Netch.Servers;

public class VMessServer : Server
{
    private string _tlsSecureType = VMessGlobal.TLSSecure[0];

    public override string Type { get; } = "VMess";

    public override string MaskedData()
    {
        var maskedData = $"{EncryptMethod} + {TransferProtocol} + {PacketEncoding} + {FakeType}";
        switch (TransferProtocol)
        {
            case "tcp":
            case "ws":
            case "httpupgrade":
            case "xhttp":
                maskedData += $" + {TLSSecureType}";
                break;
            case "quic":
                maskedData += $" + {QUICSecure}";
                break;
            case "grpc":
                break;
            case "kcp":
                break;
        }

        return maskedData;
    }

    /// <summary>
    ///     用户 ID
    /// </summary>
    public string UserID { get; set; } = string.Empty;

    /// <summary>
    ///     额外 ID
    /// </summary>
    public int AlterID { get; set; }

    /// <summary>
    ///     加密方式
    /// </summary>
    public virtual string EncryptMethod { get; set; } = VMessGlobal.EncryptMethods[0];

    /// <summary>
    ///     传输协议
    /// </summary>
    public virtual string TransferProtocol { get; set; } = VMessGlobal.TransferProtocols[0];

    /// <summary>
    ///     包传输格式
    /// </summary>
    public virtual string PacketEncoding { get; set; } = VMessGlobal.PacketEncodings[2];

    /// <summary>
    ///     伪装类型
    /// </summary>
    public virtual string FakeType { get; set; } = VMessGlobal.FakeTypes[0];

    /// <summary>
    ///     伪装域名
    /// </summary>
    public string? Host { get; set; }

    /// <summary>
    ///     传输路径
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    ///     QUIC 加密方式
    /// </summary>
    public string? QUICSecure { get; set; } = VMessGlobal.QUIC[0];

    /// <summary>
    ///     QUIC 加密密钥
    /// </summary>
    public string? QUICSecret { get; set; } = string.Empty;

    /// <summary>
    ///     TLS 底层传输安全
    /// </summary>
    public string TLSSecureType
    {
        get => _tlsSecureType;
        set
        {
            if (value == "")
                value = "none";

            _tlsSecureType = value;
        }
    }

    /// <summary>
    ///     Mux 多路复用
    /// </summary>
    public bool? UseMux { get; set; }

    public string? ServerName { get; set; } = string.Empty;

    /// <summary>
    ///     Reality Public Key
    /// </summary>
    public string? PublicKey { get; set; }

    /// <summary>
    ///     Reality Short ID
    /// </summary>
    public string? ShortId { get; set; }

    /// <summary>
    ///     Reality Fingerprint
    /// </summary>
    public string? Fingerprint { get; set; } = "chrome";

    /// <summary>
    ///     Reality SpiderX
    /// </summary>
    public string? SpiderX { get; set; }

    /// <summary>
    ///     xhttp Mode
    /// </summary>
    public string? XhttpMode { get; set; } = "auto";

    /// <summary>
    ///     XTLS Flow 流控 (如 xtls-rprx-vision，留空则不启用，仅 VLESS 有效)
    /// </summary>
    public string? Flow { get; set; } = "";

    /// <summary>
    ///     ALPN (e.g. h2 或 h2,http/1.1)
    /// </summary>
    public string? Alpn { get; set; }
}

public class VMessGlobal
{
    public static readonly List<string> EncryptMethods = new()
    {
        "auto",
        "none",
        "aes-128-gcm",
        "chacha20-poly1305",
        "zero"
    };

    public static readonly List<string> QUIC = new()
    {
        "none",
        "aes-128-gcm",
        "chacha20-poly1305"
    };

    public static readonly List<string> PacketEncodings = new()
    {
        "none",
        "packet",
        "xudp"
    };

    public static readonly List<string> TransferProtocols = new()
    {
        "tcp",
        "kcp",
        "ws",
        "h2",
        "quic",
        "grpc",
        "httpupgrade",
        "xhttp"
    };

    public static readonly List<string> FakeTypes = new()
    {
        "none",
        "http",
        "srtp",
        "utp",
        "wechat-video",
        "dtls",
        "wireguard",
        "gun",
        "multi"
    };

    public static readonly List<string> TLSSecure = new()
    {
        "none",
        "tls"
    };
}
