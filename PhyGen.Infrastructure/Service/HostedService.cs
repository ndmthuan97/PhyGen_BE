using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Service
{
    public class NodeMathmlHostedService : IHostedService
    {
        private Process? _proc;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var workdir = Path.Combine(AppContext.BaseDirectory, "latex-mathml");
            var psi = new ProcessStartInfo
            {
                FileName = "node",               // hoặc "node.exe"
                Arguments = "server.js",         // hoặc "npm start"
                WorkingDirectory = workdir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            // nếu muốn port cố định nội bộ
            psi.Environment["PORT"] = "3101";

            _proc = Process.Start(psi);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try { _proc?.Kill(true); } catch { }
            return Task.CompletedTask;
        }
    }
}
