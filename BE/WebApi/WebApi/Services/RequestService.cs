using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Serilog;

namespace WebApi.Controllers;

public class RequestService
{
    public static IActionResult ReturnBadRequest(string method, string message)
    {
        Log.Error($"{method}: {message}");
        return new BadRequestObjectResult(message);
    }
}