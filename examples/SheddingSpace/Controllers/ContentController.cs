using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NoisyWeb.Services;

namespace NoisyWeb.Controllers
{

  

    [ApiController]
    [Route("api/[controller]")]

    public class ContentController : Controller
    {

        public IOpenAiService _openAiService;

        public ContentController(IOpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

      [HttpPost]
        [Route("enhanceText")]
        public IActionResult PostEnhanceContent([FromBody] JObject vals)
        {

            var enhancedText = _openAiService.EnhanceText(vals.GetValue("text").ToString(), (int)vals.GetValue("wordCount")).Result;

            JObject jEnhanced = new JObject();
            jEnhanced.Add("enhancedText", enhancedText);
            return Ok(jEnhanced);
        }

        
    }
}
