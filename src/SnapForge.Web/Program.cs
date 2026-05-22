using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Http.Features;

Console.OutputEncoding = Encoding.UTF8;

var openBrowser = !args.Any(arg => string.Equals(arg, "--no-browser", StringComparison.OrdinalIgnoreCase));
var webArgs = args
    .Where(arg => !string.Equals(arg, "--no-browser", StringComparison.OrdinalIgnoreCase))
    .ToArray();
var launchUrl = HasExplicitUrls(webArgs)
    ? null
    : $"http://127.0.0.1:{GetAvailablePort()}";

var builder = WebApplication.CreateBuilder(webArgs);

if (launchUrl is not null)
{
    builder.WebHost.UseUrls(launchUrl);
}

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".SnapForge.Web.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 15 * 1024 * 1024;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseSession();

app.MapRazorPages();

if (openBrowser)
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var url = ResolveLaunchUrl(app, launchUrl);

        Console.WriteLine("SnapForge Web запущен.");
        Console.WriteLine($"Откройте в браузере: {url}");
        Console.WriteLine("Чтобы остановить приложение, закройте это окно или нажмите Ctrl+C.");

        OpenBrowser(url);
    });
}

await app.RunAsync();

static bool HasExplicitUrls(IReadOnlyCollection<string> args)
{
    if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"))
        || !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DOTNET_URLS")))
    {
        return true;
    }

    return args.Any(arg =>
        string.Equals(arg, "--urls", StringComparison.OrdinalIgnoreCase)
        || arg.StartsWith("--urls=", StringComparison.OrdinalIgnoreCase));
}

static int GetAvailablePort()
{
    var listener = new TcpListener(IPAddress.Loopback, port: 0);
    listener.Start();

    try
    {
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }
    finally
    {
        listener.Stop();
    }
}

static string ResolveLaunchUrl(WebApplication app, string? fallbackUrl)
{
    return fallbackUrl
        ?? app.Urls.FirstOrDefault(url => url.StartsWith("http://127.0.0.1:", StringComparison.OrdinalIgnoreCase))
        ?? app.Urls.FirstOrDefault(url => url.StartsWith("http://localhost:", StringComparison.OrdinalIgnoreCase))
        ?? app.Urls.FirstOrDefault()
        ?? "http://127.0.0.1:5000";
}

static void OpenBrowser(string url)
{
    try
    {
        Process.Start(new ProcessStartInfo(url)
        {
            UseShellExecute = true
        });
    }
    catch (Exception exception) when (exception is InvalidOperationException or Win32Exception)
    {
        Console.WriteLine($"Не удалось открыть браузер автоматически: {exception.Message}");
    }
}
