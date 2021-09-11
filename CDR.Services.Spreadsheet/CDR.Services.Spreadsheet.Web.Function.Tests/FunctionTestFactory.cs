using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;


namespace CDR.Services.Spreadsheet.Web.Function.Tests
{
    public class FunctionTestFactory
    {
        public static HttpRequest CreateHttpPostRequest<T>(T payload)
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var json = JsonConvert.SerializeObject(payload);
            sw.Write(json);
            sw.Flush();
            ms.Position = 0;
            request.Body = ms;
            return request;
        }
    }
}
