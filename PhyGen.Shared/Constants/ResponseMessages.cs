using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Shared.Constants
{
    public static class ResponseMessages
    {
        private static readonly Dictionary<StatusCode, string> _messages = new Dictionary<StatusCode, string>
        {
            // Success messages
            { StatusCode.RequestProcessedSuccessfully, "Request processed successfully." },

            // Error messages
            { StatusCode.ModelInvalid, "Model is invalid. Please check the request body." },

            // Curriculum messages
            { StatusCode.CurriculumNotFound, "Curriculum with Id does not exist." },
            { StatusCode.CurriculumSameName, "Curriculum with the same name already exists." },
        };

        public static string GetMessage(StatusCode code) => _messages[code];
    }
}
