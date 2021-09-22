using CDR.Services.RiskCalc.CommonTestAssets;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;

namespace CDR.Services.Spreadsheet.CommonTestAssets
{
    public class ReverseMappingTestFactory
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
        public static IReverseMappingRequest CreateFirstValidReverseMappingRequestWithoutBookToPersist()
        {
            var request = new ReverseMappingRequest()
            {
                Id = new System.Guid("fa7f9c6d-a08a-4be8-82b3-ba64b8269338"),
                Payload = JsonConvert.DeserializeObject("{'Age':35,'Height':1.8,'Weight':100,'TestA2':'This is BookToMap','TestA3':50}"),
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                BookToPersist = string.Empty,
                Created = DateTimeOffset.UtcNow
            };
            return request;
        }

        public static IReverseMappingRequest CreateSecondValidReverseMappingRequestWithoutBookToPersist()
        {
            var request = new ReverseMappingRequest()
            {
                Id = new System.Guid("fa7f9c6d-a08a-4be8-82b3-ba64b8269338"),
                Payload = JsonConvert.DeserializeObject("{'Age':45,'Height':1.9,'Weight':90,'TestA2':'This is BookToMap','TestA3':50}"),
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                BookToPersist = string.Empty,
                Created = DateTimeOffset.UtcNow
            };
            return request;
        }

        #endregion

        #region Upsert will work on BookToPersist
        public static IReverseMappingRequest CreateFirstValidReverseMappingRequestWithBookToPersist()
        {
            var request = new ReverseMappingRequest()
            {
                Id = new System.Guid("fa7f9c6d-a08a-4be8-82b3-ba64b8269338"),
                Payload = JsonConvert.DeserializeObject("{'Age':35,'Height':1.8,'Weight':100,'TestA2':'This is BookToPersist','TestA3':50}"),
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                BookToPersist = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISamplePersist.xlsx",
                Created = DateTimeOffset.UtcNow
            };
            return request;
        }

        public static IReverseMappingRequest CreateSecondValidReverseMappingRequestWithBookToPersist()
        {
            var request = new ReverseMappingRequest()
            {
                Id = new System.Guid("fa7f9c6d-a08a-4be8-82b3-ba64b8269338"),
                Payload = JsonConvert.DeserializeObject("{'Age':45,'Height':1.9,'Weight':90,'TestA2':'This is BookToPersist','TestA3':50}"),
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                BookToPersist = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISamplePersist.xlsx",
                Created = DateTimeOffset.UtcNow
            };
            return request;
        }
        #endregion

        #endregion

        #region Invalid Requests

        // Empty Body - Should return 400 Bad Request
        public static IReverseMappingRequest CreateEmptyReverseMappingRequest()
        {
            return null;
        }

        // Invalid Id - Should return 400 Bad Request with Error messages
        public static IReverseMappingRequest CreateInvalidIdReverseMappingRequest()
        {
            var request = new ReverseMappingRequest()
            {
                Payload = JsonConvert.DeserializeObject("{'Age':45,'Height':1.9,'Weight':90,'TestA2':'This is test','TestA3':50}"),
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                BookToPersist = string.Empty,
                Created = DateTimeOffset.UtcNow
            };

            return request;
        }

        // Invalid BookToMap - Should return 400 Bad Request with Error messages
        public static IReverseMappingRequest CreateInvalidBookToMapReverseMappingRequest()
        {
            var request = new ReverseMappingRequest()
            {
                Id = new System.Guid("fa7f9c6d-a08a-4be8-82b3-ba64b8269338"),
                Payload = JsonConvert.DeserializeObject("{'Age':45,'Height':1.9,'Weight':90,'TestA2':'This is test','TestA3':50}"),
                BookToMap = string.Empty,
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                BookToPersist = string.Empty,
                Created = DateTimeOffset.UtcNow
            };

            return request;
        }

        // Invalid Payload - Should return 400 Bad Request with Error messages
        public static IReverseMappingRequest CreateInvalidPayloadReverseMappingRequest()
        {
            var request = new ReverseMappingRequest()
            {
                Id = new System.Guid("fa7f9c6d-a08a-4be8-82b3-ba64b8269338"),
                Payload = string.Empty,
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISampleCells.liquid",
                BookToPersist = string.Empty,
                Created = DateTimeOffset.UtcNow
            };

            return request;
        }

        // Invalid Template - Should return 400 Bad Request with Error messages
        public static IReverseMappingRequest CreateInvalidTemplateReverseMappingRequest()
        {
            var request = new ReverseMappingRequest()
            {
                Id = new System.Guid("fa7f9c6d-a08a-4be8-82b3-ba64b8269338"),
                Payload = JsonConvert.DeserializeObject("{'Age':45,'Height':1.9,'Weight':90,'TestA2':'This is test','TestA3':50}"),
                BookToMap = "uploadtemp\\be98188d-db05-4b8c-93d3-97041eb924e8\\BMISample.xlsx",
                Template = string.Empty,
                BookToPersist = string.Empty,
                Created = DateTimeOffset.UtcNow
            };

            return request;
        }

        // All Invalid arguments - Should return 400 Bad Request with Error messages
        public static IReverseMappingRequest CreateInvalidEntireReverseMappingRequest()
        {
            var request = new ReverseMappingRequest()
            {
                
            };

            return request;
        }

        #endregion
    }
}
