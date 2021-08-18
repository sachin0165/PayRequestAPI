using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace PayRequestWeb.Helpers
{
    public class RawJsonActionResult : IActionResult
    {
        private readonly string _jsonString;
        private readonly int _statusCode;

        public RawJsonActionResult(object value, int statusCode)
        {
            if (value != null)
            {
                _jsonString = value.ToString();
            }
            _statusCode = statusCode;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            response.StatusCode = _statusCode;
            response.ContentType = MediaTypeNames.Application.Json;

            using (TextWriter writer = new HttpResponseStreamWriter(response.Body, Encoding.UTF8))
            {
                writer.Write(_jsonString);
            }

            return Task.CompletedTask;
        }
    }
}
