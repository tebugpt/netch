using System.Text.RegularExpressions;
using System.Web;
using Netch.Interfaces;
using Netch.Models;
using Netch.Utils;

namespace Netch.Servers;

public class Hysteria2Util : IServerUtil
{
    public ushort Priority { get; } = 4;

    public string TypeName { get; } = "Hysteria2";

    public string FullName { get; } = "Hysteria2";

    public string ShortName { get; } = "HY2";

    /// <summary>
    ///     官方两种写法都支持: hysteria2:// 和 hy2://
    /// </summary>
    public string[] UriScheme { get; } = { "hysteria2", "hy2" };

    public Type ServerType { get; } = typeof(Hysteria2Server);

    public void Edit(Server s)
    {
        new Hysteria2Form((Hysteria2Server)s).ShowDialog();
    }

    public void Create()
    {
        new Hysteria2Form().ShowDialog();
    }

    public string GetShareLink(Server s)
    {
        var server = (Hysteria2Server)s;
        var parameter = new Dictionary<string, string>();

        if (server.Insecure)
            parameter.Add("insecure", "1");

        if (!server.SNI.IsNullOrWhiteSpace())
            parameter.Add("sni", server.SNI!);

        if (!server.ObfsType.IsNullOrWhiteSpace())
        {
            parameter.Add("obfs", server.ObfsType!);
            if (!server.ObfsPassword.IsNullOrWhiteSpace())
                parameter.Add("obfs-password", Uri.EscapeDataString(server.ObfsPassword!));
        }

        var query = string.Join("&", parameter.Select(p => $"{p.Key}={p.Value}"));

        return $"hysteria2://{Uri.EscapeDataString(server.Password)}@{server.Hostname}:{server.Port}/" +
               $"{(query.Length > 0 ? $"?{query}" : "")}" +
               $"{(!server.Remark.IsNullOrWhiteSpace() ? $"#{Uri.EscapeDataString(server.Remark)}" : "")}";
    }

    public IServerController GetController()
    {
        return new Hysteria2Controller();
    }

    public IEnumerable<Server> ParseUri(string text)
    {
        var data = new Hysteria2Server();

        text = text.Replace("/?", "?");
        if (text.Contains("#"))
        {
            data.Remark = Uri.UnescapeDataString(text.Split('#')[1]);
            text = text.Split('#')[0];
        }

        if (text.Contains("?"))
        {
            var parameter = HttpUtility.ParseQueryString(text.Split('?')[1]);
            text = text.Substring(0, text.IndexOf("?", StringComparison.Ordinal));

            data.Insecure = parameter.Get("insecure") is "1" or "true";
            // 官方字段是 sni，部分生成器会写成 peer
            data.SNI = parameter.Get("sni") ?? parameter.Get("peer");
            data.ObfsType = parameter.Get("obfs");
            data.ObfsPassword = parameter.Get("obfs-password");
        }

        text = text.TrimEnd('/');

        var finder = new Regex(@"^(?:hysteria2|hy2)://(?<auth>.+?)@(?<server>.+):(?<port>\d+)$");
        var match = finder.Match(text);
        if (!match.Success)
            throw new FormatException();

        data.Password = Uri.UnescapeDataString(match.Groups["auth"].Value);
        data.Hostname = match.Groups["server"].Value;
        data.Port = ushort.Parse(match.Groups["port"].Value);

        return new[] { data };
    }

    public bool CheckServer(Server s)
    {
        return true;
    }
}
