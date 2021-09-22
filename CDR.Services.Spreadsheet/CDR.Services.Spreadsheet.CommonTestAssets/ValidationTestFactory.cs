using CDR.Services.RiskCalc.CommonTestAssets;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDR.Services.Spreadsheet.CommonTestAssets
{
    public class ValidationTestFactory
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

        #region validation request

        public static IValidationRequest CreateFirstValidValidationRequestWithWriteErrorsToBookFalse()
        {
            var request = new ValidationRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookUnderValidation = "uploadtemp\\92816b95-8c72-489a-8307-cc9302384136\\CDR.Services.Spreadsheet - Sample Book (2).xlsx",
                ValidatorBook = "uploadtemp\\4ea3cdb6-19d8-4803-8c3f-4eb77b782904\\ValidatorBook.xlsx",
                Options = new ValidationRequestOptions { WriteErrorsToBook = false },
                Created = DateTimeOffset.UtcNow
            };
            return request;
        }

        public static IValidationRequest CreateValidValidationRequest()
        {
            var request = new ValidationRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookUnderValidation = "uploadtemp\\92816b95-8c72-489a-8307-cc9302384136\\CDR.Services.Spreadsheet - Sample Book (2).xlsx",
                ValidatorBook = "uploadtemp\\4ea3cdb6-19d8-4803-8c3f-4eb77b782904\\ValidatorBook.xlsx",
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
                BookUnderValidation = "uploadtemp\\92816b95-8c72-489a-8307-cc9302384136\\CDR.Services.Spreadsheet - Sample Book (2).xlsx",
                ValidatorBook = "uploadtemp\\4ea3cdb6-19d8-4803-8c3f-4eb77b782904\\ValidatorBook.xlsx",
                Options = new ValidationRequestOptions { WriteErrorsToBook = IsWriteErrorsToBook },
                Created = DateTime.Now
            };
            return request;
        }

        #endregion

        #region Invalid Requests

        // Empty Body - Should return 400 Bad Request
        public static IValidationRequest CreateEmptyValidationRequest()
        {
            return null;
        }

        // Invalid Id - Should return 400 Bad Request with Error messages
        public static IValidationRequest CreateInvalidIdValidationRequest()
        {
            var request = new ValidationRequest()
            {
                BookUnderValidation = "uploadtemp\\92816b95-8c72-489a-8307-cc9302384136\\CDR.Services.Spreadsheet - Sample Book (2).xlsx",
                ValidatorBook = "uploadtemp\\4ea3cdb6-19d8-4803-8c3f-4eb77b782904\\ValidatorBook.xlsx",
                Options = new ValidationRequestOptions { WriteErrorsToBook = false },
                Created = DateTimeOffset.UtcNow
            };

            return request;
        }

        // Invalid BookUnderValidation - Should return 400 Bad Request with Error messages
        public static IValidationRequest CreateInvalidBookUnderValidationRequest(bool isNull = false)
        {
            var request = new ValidationRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookUnderValidation = isNull ? null : "",
                ValidatorBook = "uploadtemp\\4ea3cdb6-19d8-4803-8c3f-4eb77b782904\\ValidatorBook.xlsx",
                Options = new ValidationRequestOptions { WriteErrorsToBook = false },
                Created = DateTime.Now
            };
            return request;
        }

        // Invalid ValidatorBook - Should return 400 Bad Request with Error messages
        public static IValidationRequest CreateValidatorBookValidationRequest(bool isNull = false)
        {
            var request = new ValidationRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookUnderValidation = "uploadtemp\\92816b95-8c72-489a-8307-cc9302384136\\CDR.Services.Spreadsheet - Sample Book (2).xlsx",
                ValidatorBook = isNull ? null : "",
                Options = new ValidationRequestOptions { WriteErrorsToBook = false },
                Created = DateTime.Now
            };
            return request;
        }

        // Invalid Options - Should return 400 Bad Request with Error messages
        public static IValidationRequest CreateInvalidOptionsValidationRequest()
        {
            var request = new ValidationRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookUnderValidation = "uploadtemp\\92816b95-8c72-489a-8307-cc9302384136\\CDR.Services.Spreadsheet - Sample Book (2).xlsx",
                ValidatorBook = "uploadtemp\\4ea3cdb6-19d8-4803-8c3f-4eb77b782904\\ValidatorBook.xlsx",
                Options = null,
                Created = DateTimeOffset.UtcNow
            };

            return request;
        }

        // All Invalid arguments - Should return 400 Bad Request with Error messages
        public static IValidationRequest CreateInvalidEntireValidationRequest()
        {
            var request = new ValidationRequest()
            {

            };

            return request;
        }

        #endregion
    }
}
