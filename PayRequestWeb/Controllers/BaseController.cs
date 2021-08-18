using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PayRequestWeb.Helpers;
using PayRequestWeb.Models;
using System.Net;

namespace PayRequestWeb.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult Success(object data, string message = "", int httpStatusCode = (int)HttpStatusCode.OK)
        {
            var responseDto = new SuccessResponse
            {
                Data = data ?? new object(),
                Message = message,
                Success = true
            };

            var json = JsonConvert.SerializeObject(responseDto);
            return new RawJsonActionResult(json, httpStatusCode);
        }

        protected IActionResult BadRequest(string message = "", int httpStatusCode = (int)HttpStatusCode.BadRequest)
        {
            var responseDto = new SuccessResponse
            {
                Data = new object(),
                Message = message,
                Success = false
            };

            var json = JsonConvert.SerializeObject(responseDto);
            return new RawJsonActionResult(json, httpStatusCode);
        }

        protected IActionResult InternalError(string message = "", int httpStatusCode = (int)HttpStatusCode.InternalServerError)
        {
            var responseDto = new SuccessResponse
            {
                Message = message,
                Success = false
            };

            var json = JsonConvert.SerializeObject(responseDto);
            return new RawJsonActionResult(json, httpStatusCode);
        }
    }
}