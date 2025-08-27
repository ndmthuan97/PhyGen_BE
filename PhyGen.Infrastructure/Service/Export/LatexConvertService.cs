using Microsoft.Extensions.Configuration;
using PhyGen.Application.Exams.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Service.Export
{
    public class LatexConvertService : ILatexConvertService
    {
        private readonly HttpClient _http;
        private readonly string _endpoint;

        public LatexConvertService(HttpClient http, IConfiguration cfg)
        {
            _http = http;
            _endpoint = cfg["LatexMathmlService:Endpoint"]
                        ?? throw new InvalidOperationException("LatexMathmlService:Endpoint is not configured");
        }

        public async Task<string> ToMathMLAsync(string latex, bool displayMode = false, CancellationToken ct = default)
        {
            try { 
                var payload = new { latex, displayMode };
                using var res = await _http.PostAsJsonAsync(_endpoint, payload, ct);
                if (!res.IsSuccessStatusCode)
                {
                    var err = await res.Content.ReadAsStringAsync(ct);
                    return $"<!-- convert-error: {res.StatusCode}; latex: {latex} -->";
                }

                using var stream = await res.Content.ReadAsStreamAsync(ct);
                var json = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
                return json.RootElement.TryGetProperty("mathml", out var m)
                ? m.GetString() ?? "<!-- empty mathml -->"
                : "<!-- missing mathml -->";
            }
            catch (Exception ex)
            {
                return $"<!-- convert-exception: {ex.Message} -->";
            }
        }
    }
}
