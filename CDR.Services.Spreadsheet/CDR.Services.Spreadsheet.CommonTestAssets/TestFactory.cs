using CDR.Services.RiskCalc.CommonTestAssets;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CDR.Services.Spreadsheet.CommonTestAssets
{
    public class TestFactory
    {
        public static ILogger<T> CreateLogger<T>(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger<T> logger = type switch
            {
                LoggerTypes.List => new ListLogger<T>(),
                LoggerTypes.Trace => new TraceLogger<T>(),
                _ => NullLoggerFactory.Instance.CreateLogger<T>(),
            };
            return logger;
        }

        public static IValidationRequest CreateValidValidationRequest()
        {
            var request = new ValidationRequest()
            {
              
            };

            return request;
        }

        public static IValidationRequest CreateInvalidValidationRequest()
        {
            var request = new ValidationRequest()
            {

            };

            return request;
        }

        public static IValidationRequest CreateSecondValidValidationRequest()
        {
            var request = new ValidationRequest()            {
                
              
            };

            return request;
        }

        public static IValidationRequest CreateThirdValidValidationRequest()
        {
            var request = new ValidationRequest()
            {
              
            };

            return request;
        }

        public static IValidationRequest CreateFourthValidValidationRequest()
        {
            var request = new ValidationRequest()
            {
                
            };

            return request;
        }

        public static IValidationRequest CreateFifthValidValidationRequest()
        {
            var request = new ValidationRequest()
            {
               
            };

            return request;
        }
    }
}
