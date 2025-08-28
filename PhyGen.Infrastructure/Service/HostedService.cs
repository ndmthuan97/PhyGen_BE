using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public sealed class NodeMathmlHostedService : IHostedService, IDisposable
{
    private readonly ILogger<NodeMathmlHostedService> _logger;
    private Process? _proc;
    private CancellationTokenSource? _cts;

    public NodeMathmlHostedService(ILogger<NodeMathmlHostedService> logger)
    {
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var workdir = Path.Combine(AppContext.BaseDirectory, "latex-mathml");
        if (!Directory.Exists(workdir))
        {
            _logger.LogError("latex-mathml folder not found at {dir}", workdir);
            return; // vẫn cho API chạy, nhưng LatexConvertService sẽ bị 422/500
        }

        // 1) Nếu chưa có node_modules => npm ci
        var nodeModules = Path.Combine(workdir, "node_modules");
        if (!Directory.Exists(nodeModules))
        {
            _logger.LogInformation("node_modules not found -> running npm ci ...");
            var ok = await RunCmdAsync(
                workdir,
                isWindows: OperatingSystem.IsWindows(),
                // chạy npm ci
                windowsCmd: "cmd.exe",
                windowsArgs: "/c npm ci",
                linuxCmd: "/bin/bash",
                linuxArgs: "-lc \"npm ci\"",
                cancellationToken: cancellationToken);

            if (!ok)
            {
                _logger.LogError("npm ci failed -> latex-mathml will not be started");
                return;
            }
        }

        // 2) Start Node server
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _proc = StartNode(workdir);

        if (_proc == null || _proc.HasExited)
        {
            _logger.LogError("Failed to start Node server");
            return;
        }

        _logger.LogInformation("Node latex-mathml started (PID: {pid})", _proc.Id);

        // 3) Chờ port 3101 sẵn sàng (retry ~10s)
        var ok2 = await WaitPortReady("http://127.0.0.1:3101/health", TimeSpan.FromSeconds(10), _cts.Token);
        if (!ok2)
        {
            _logger.LogWarning("Node latex-mathml may not be ready yet.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_proc != null && !_proc.HasExited)
            {
                _logger.LogInformation("Stopping Node latex-mathml (PID: {pid}) ...", _proc.Id);
                _proc.Kill(true);
                _proc.Dispose();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while killing node process");
        }

        _cts?.Cancel();
        _cts?.Dispose();
        return Task.CompletedTask;
    }

    private Process? StartNode(string workdir)
    {
        var isWin = OperatingSystem.IsWindows();

        // Ở local dev có thể chạy "node server.js".
        // Trên Azure Linux/Windows vẫn tương tự nếu Node có sẵn trong PATH.
        var psi = new ProcessStartInfo
        {
            FileName = isWin ? "cmd.exe" : "/bin/bash",
            Arguments = isWin ? "/c node server.js" : "-lc \"node server.js\"",
            WorkingDirectory = workdir,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };
        psi.Environment["PORT"] = "3101";

        var p = new Process { StartInfo = psi, EnableRaisingEvents = true };
        p.OutputDataReceived += (_, e) => { if (e.Data != null) Console.WriteLine("[mathml] " + e.Data); };
        p.ErrorDataReceived += (_, e) => { if (e.Data != null) Console.Error.WriteLine("[mathml:err] " + e.Data); };

        try
        {
            if (!p.Start())
                return null;
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            return p;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot start node server");
            return null;
        }
    }

    private static async Task<bool> RunCmdAsync(
        string workdir,
        bool isWindows,
        string windowsCmd, string windowsArgs,
        string linuxCmd, string linuxArgs,
        CancellationToken cancellationToken)
    {
        var psi = new ProcessStartInfo
        {
            FileName = isWindows ? windowsCmd : linuxCmd,
            Arguments = isWindows ? windowsArgs : linuxArgs,
            WorkingDirectory = workdir,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        using var p = new Process { StartInfo = psi };
        p.Start();
        var tcs = new TaskCompletionSource<bool>();

        p.EnableRaisingEvents = true;
        p.Exited += (_, __) => tcs.TrySetResult(p.ExitCode == 0);

        using var reg = cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));
        return await tcs.Task.ConfigureAwait(false);
    }

    private static async Task<bool> WaitPortReady(string url, TimeSpan timeout, CancellationToken token)
    {
        var start = DateTime.UtcNow;

        using var http = new HttpClient { Timeout = TimeSpan.FromMilliseconds(800) };
        while (DateTime.UtcNow - start < timeout && !token.IsCancellationRequested)
        {
            try
            {
                var res = await http.GetAsync(url, token);
                if (res.IsSuccessStatusCode) return true;
            }
            catch { }

            await Task.Delay(500, token);
        }

        return false;
    }

    public void Dispose()
    {
        _proc?.Dispose();
        _cts?.Dispose();
    }
}
