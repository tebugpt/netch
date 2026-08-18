using Netch.Forms;

namespace Netch.Servers;

[Fody.ConfigureAwait(true)]
public class Hysteria2Form : ServerForm
{
    public Hysteria2Form(Hysteria2Server? server = default)
    {
        server ??= new Hysteria2Server();
        Server = server;

        CreateTextBox("Password", "Password (auth)", s => true, s => server.Password = s, server.Password);
        CreateTextBox("SNI", "SNI", s => true, s => server.SNI = s, server.SNI ?? "");
        CreateCheckBox("Insecure", "跳过证书验证 (insecure)", b => server.Insecure = b, server.Insecure);
        CreateTextBox("ObfsType", "混淆类型 (可留空, 目前仅 salamander)", s => true, s => server.ObfsType = s, server.ObfsType ?? "");
        CreateTextBox("ObfsPassword", "混淆密码", s => true, s => server.ObfsPassword = s, server.ObfsPassword ?? "");
        CreateTextBox("UpMbps", "上行带宽 (如 10 mbps, 可留空)", s => true, s => server.UpMbps = s, server.UpMbps ?? "");
        CreateTextBox("DownMbps", "下行带宽 (如 50 mbps, 可留空)", s => true, s => server.DownMbps = s, server.DownMbps ?? "");
    }

    protected override string TypeName { get; } = "Hysteria2";
}
