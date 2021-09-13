using CDR.Services.RiskCalc.CommonTestAssets;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

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
        #region Validation

        public static IValidationRequest CreateInvalidValidationRequest()
        {
            var request = new ValidationRequest()
            {
            };
            return request;
        }

        public static IValidationRequest CreateValidValidationRequest()
        {
            var request = new ValidationRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookUnderValidation = "uploadtemp/00ed9db7-5ac2-474f-b4f5-63966a94924e/Input File_v3_Per downloadVers.xlsx",
                ValidatorBook = "uploadtemp\\4ea3cdb6-19d8-4803-8c3f-4eb77b782904\\ValidatorBook.xlsx",
                Options = new ValidationRequestOptions { WriteErrorsToBook = false },
                Created = DateTime.Now
            };
            return request;
        }

        public static IValidationRequest CreateSecondValidationRequest(bool isNull=false)
        {
            var request = new ValidationRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookUnderValidation = isNull? null:"",
                ValidatorBook = "uploadtemp\\4ea3cdb6-19d8-4803-8c3f-4eb77b782904\\ValidatorBook.xlsx",
                Options = new ValidationRequestOptions { WriteErrorsToBook = false },
                Created = DateTime.Now
            };
            return request;
        }

        public static IValidationRequest CreateValidatorBookValidationRequest(bool isNull = false)
        {
            var request = new ValidationRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookUnderValidation = "uploadtemp/00ed9db7-5ac2-474f-b4f5-63966a94924e/Input File_v3_Per downloadVers.xlsx",
                ValidatorBook = isNull ? null : "",
                Options = new ValidationRequestOptions { WriteErrorsToBook = false },
                Created = DateTime.Now
            };
            return request;
        }

        public static IValidationRequest CreateWriteErrorsToBookValidationRequest(bool IsWriteErrorsToBook)
        {
            var request = new ValidationRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookUnderValidation = "uploadtemp/00ed9db7-5ac2-474f-b4f5-63966a94924e/Input File_v3_Per downloadVers.xlsx",
                ValidatorBook = "uploadtemp\\4ea3cdb6-19d8-4803-8c3f-4eb77b782904\\ValidatorBook.xlsx",
                Options = new ValidationRequestOptions { WriteErrorsToBook = IsWriteErrorsToBook },
                Created = DateTime.Now
            };
            return request;
        }
        #endregion
    }
}
