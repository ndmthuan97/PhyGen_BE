using PhyGen.Application.Exams.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace PhyGen.Infrastructure.Service.Export
{
    public class MathmlToOmmlService : IMathmlToOmmlService
    {
        private readonly Lazy<XslCompiledTransform> _xslt;

        public MathmlToOmmlService()
        {
            _xslt = new Lazy<XslCompiledTransform>(LoadTransform);
        }

        public Task<string> ToOmmlAsync(string mathml, CancellationToken ct = default)
        {
            // parse MathML
            using var srcReader = XmlReader.Create(new StringReader(mathml), new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });

            // transform -> OMML
            using var sw = new StringWriter();
            using var destWriter = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true });

            _xslt.Value.Transform(srcReader, null, destWriter);
            destWriter.Flush();

            var omml = sw.ToString();
            if (string.IsNullOrWhiteSpace(omml))
                throw new InvalidOperationException("mml2omml returned empty output.");

            return Task.FromResult(omml);
        }

        private static XslCompiledTransform LoadTransform()
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourceName = asm
                .GetManifestResourceNames()
                .First(n => n.EndsWith("mml2omml.xsl", StringComparison.OrdinalIgnoreCase));

            using var s = asm.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException("Cannot load mml2omml.xsl embedded resource.");

            using var xr = XmlReader.Create(s);
            var xslt = new XslCompiledTransform(enableDebug: false);
            xslt.Load(xr);
            return xslt;
        }
    }
}
