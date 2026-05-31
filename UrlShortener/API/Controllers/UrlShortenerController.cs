using API.Data;
using API.Models;
using API.Models.Entity;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UrlShortenerController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult Get(string Url)
        {
            try
            {

                if (!Uri.TryCreate(Url, UriKind.Absolute, out var actualUrl))
                {
                    return BadRequest("Invalid Url");
                }

                var random = new Random();

                const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@";

                #region Description

                // Enumerable.Repeat(allowedChars, 8:
                // creates an IEnumerable<string> with 8 elements, each one being the string allowedChars.

                // .Select(x => x[random.Next(x.Length)]):
                // For each element x (which is just allowedChars), it selects one random character from it.
                // random.Next(x.Length) picks a random index between 0 and allowedChars.Length - 1.

                // .ToArray()
                // Converts the 8 random characters into a char[] array.

                // new String(...)
                // Converts the char[] into a final string.

                #endregion

                var randomStr = new string(Enumerable.Repeat(allowedChars, 8).Select(x => x[random.Next(x.Length)]).ToArray());

                var sanitizer = new HtmlSanitizer();

                UrlMapper url = new()
                {
                    ActualUrl = sanitizer.Sanitize(Url),
                    ShortenedUrl = randomStr
                };

                _db.UrlMappers.Add(url);

                if (_db.SaveChanges() > 0)
                {
                    var shortenedUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/" + randomStr;

                    return Ok(shortenedUrl);
                }
                else
                {
                    return BadRequest("Unable to generate Shortenend URL");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to generate Shortenend URL");

            }
        }
    }
}
