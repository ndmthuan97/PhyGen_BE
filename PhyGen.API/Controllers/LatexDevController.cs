using Microsoft.AspNetCore.Mvc;
using PhyGen.Application.Exams.Interfaces;

[ApiController]
[Route("api/dev/latex")]
public class LatexDevController : ControllerBase
{
    private readonly IFormulaConvertPipeline _pipeline;

    public LatexDevController(IFormulaConvertPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public record ConvertDto(string latex, bool displayMode);

    [HttpPost("to-omml")]
    public async Task<IActionResult> ToOmml([FromBody] ConvertDto dto, CancellationToken ct)
    {
        var omml = await _pipeline.LatexToOmmlAsync(dto.latex, dto.displayMode, ct);
        return Ok(new { omml });
    }
}
