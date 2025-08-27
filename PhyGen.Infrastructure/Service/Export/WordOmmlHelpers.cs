using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using PhyGen.Application.Exams.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Service.Export
{
    public static class WordOmmlHelpers
    {
        private static OpenXmlElement CreateOmmlElement(string ommlXml)
        {
            var wrapper = new Run();
            wrapper.InnerXml = ommlXml;

            return wrapper.FirstChild!;
        }

        private static bool LooksLikeOmmlPara(string xml)
            => xml.Contains("<m:oMathPara", StringComparison.OrdinalIgnoreCase);

        public static void AppendSegmentsToBody(
            Body body,
            List<FormulaSegment> segments)
        {
            var currentPara = new Paragraph();

            foreach (var seg in segments)
            {
                if (!seg.IsOmml)
                {
                    if (!string.IsNullOrEmpty(seg.Value))
                    {
                        currentPara.AppendChild(new Run(
                            new Text(seg.Value) { Space = SpaceProcessingModeValues.Preserve }
                        ));
                    }
                }
                else 
                {
                    var ommlElement = CreateOmmlElement(seg.Value);

                    if (LooksLikeOmmlPara(seg.Value))  
                    {
                        if (currentPara.HasChildren)
                            body.AppendChild(currentPara);

                        body.AppendChild(new Paragraph(ommlElement));
                        currentPara = new Paragraph();
                    }
                    else                                 
                    {
                        currentPara.AppendChild(new Run(ommlElement));
                    }
                }
            }

            if (currentPara.HasChildren)
                body.AppendChild(currentPara);
        }
    }
}
