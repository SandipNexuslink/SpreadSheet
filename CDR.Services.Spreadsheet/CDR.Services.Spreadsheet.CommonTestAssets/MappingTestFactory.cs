using CDR.Services.RiskCalc.CommonTestAssets;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDR.Services.Spreadsheet.CommonTestAssets
{
    public class MappingTestFactory
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

        #region Valid Requests

        #region Upsert will work on BookToMap
        public static IMappingRequest CreateFirstValidMappingRequestWithoutBookToPersist()
        {
            var request = new MappingRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                Options = new MappingRequestOptions { Mapper = new SpreadSheet.SpreadsheetMapper() },
                Created = DateTimeOffset.UtcNow
            };
            return request;
        }

        public static IMappingRequest CreateSecondValidMappingRequestWithoutBookToPersist()
        {
            var request = new MappingRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                Options = new MappingRequestOptions { Mapper = new SpreadSheet.SpreadsheetMapper() },
                Created = DateTimeOffset.UtcNow
            };
            return request;
        }

        #endregion

        #region Upsert will work on Option
        public static IMappingRequest CreateFirstValidMappingRequestWithOptions()
        {
            var request = new MappingRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                Options = new MappingRequestOptions { Mapper = new SpreadSheet.SpreadsheetMapper() },
                Created = DateTimeOffset.UtcNow
            };
            return request;
        }

        public static IMappingRequest CreateSecondValidMappingRequest()
        {
            var request = new MappingRequest()
            {
                Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269337",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMI.liquid",
                BookToMap = "uploadtemp\\92816b95-8c72-489a-8307-cc9302384136\\BMISample.xlsx",
                Options = new MappingRequestOptions { Mapper = new SpreadSheet.SpreadsheetMapper() },
                Created = DateTimeOffset.UtcNow
            };
            return request;
        }

#endregion

#endregion

#region Invalid Requests

// Empty Body - Should return 400 Bad Request
public static IMappingRequest CreateEmptyMappingRequest()
{
    return null;
}

// Invalid Id - Should return 400 Bad Request with Error messages
public static IMappingRequest CreateInvalidIdMappingRequest()
{
    var request = new MappingRequest()
    {
        BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
        Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
        Options = new MappingRequestOptions { Mapper = new SpreadSheet.SpreadsheetMapper() },
        Created = DateTimeOffset.UtcNow
    };

    return request;
}

// Invalid BookToMap - Should return 400 Bad Request with Error messages
public static IMappingRequest CreateInvalidBookToMapMappingRequest()
{
    var request = new MappingRequest()
    {
        Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
        BookToMap = string.Empty,
        Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
        Options = new MappingRequestOptions { Mapper = new SpreadSheet.SpreadsheetMapper() },
        Created = DateTimeOffset.UtcNow
    };

    return request;
}

// Invalid Template - Should return 400 Bad Request with Error messages
public static IMappingRequest CreateInvalidTemplateMappingRequest()
{
    var request = new MappingRequest()
    {
        Id = "fa7f9c6d-a08a-4be8-82b3-ba64b8269338",
        BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
        Template = string.Empty,
        Options = new MappingRequestOptions { Mapper = new SpreadSheet.SpreadsheetMapper() },
        Created = DateTimeOffset.UtcNow
    };

    return request;
}

// All Invalid arguments - Should return 400 Bad Request with Error messages
public static IMappingRequest CreateInvalidEntireMappingRequest()
{
    var request = new MappingRequest()
    {

    };

    return request;
}

    #endregion
}
}
