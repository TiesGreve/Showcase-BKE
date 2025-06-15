using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaptchaController : Controller
    {
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] CaptchaModel captcha)
        {
            Env.Load();
            var jsonData = new
            {
                secrect = Environment.GetEnvironmentVariable("CAPTCHA_SECRET"),
                response = captcha.Recaptcha
            };

            var json = System.Text.Json.JsonSerializer.Serialize(jsonData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var client = new HttpClient();

            // IHttpClientFactory

            var result = await client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_secretKey}&response={captcha.Recaptcha}", content);
            var responseBody = await result.Content.ReadAsStringAsync();

            var recaptchaResult = JsonConvert.DeserializeObject<CaptchaResponse>(responseBody);

            if (!recaptchaResult.Success)
            {
                Log.Information("Fail captcha request");
                return BadRequest("Invalid captcha");
            }
            return Ok(responseBody);
        }
        public class CaptchaResponse
        {
            public bool Success { get; set; }
        }
    }
}
