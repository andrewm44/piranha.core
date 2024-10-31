using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NoisyWeb.Services;
using System.Net.Http;
using HtmlAgilityPack;
using X.Web.Sitemap;

namespace NoisyWeb.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class ContentController : Controller
    {

        public IOpenAiService _openAiService;
        private readonly IHttpClientFactory _factory;

        public ContentController(IOpenAiService openAiService, IHttpClientFactory factory)
        {
            _openAiService = openAiService;
            _factory = factory;
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

        [HttpGet]
        [Route("embedCard")]
        public IActionResult GetEmbedCard([FromQuery] string linkUrl)
        {
            string respCard = "";

            respCard = GetEmbeddableCardAsync(linkUrl).Result;
            return Ok(respCard);
        }


        private async Task<string> GetEmbeddableCardAsync(string url)
        {

            using var hclient = _factory.CreateClient();
            // Fetch the HTML content from the URL
            var response = await hclient.GetStringAsync(url);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            // Extract metadata
            string title = GetMetaContent(htmlDoc, "og:title") ?? GetMetaContent(htmlDoc, "title");
            string description = GetMetaContent(htmlDoc, "og:description") ?? GetMetaContent(htmlDoc, "description");
            string imageUrl = GetMetaContent(htmlDoc, "og:image");

            // Fallback if no title, description, or image is found
            title ??= "No Title Available";
            description ??= "No Description Available";
            imageUrl ??= "https://via.placeholder.com/150";

            // Generate embeddable HTML card
            var htmlCard = $@"
            <div style='border:1px solid #ccc; border-radius:5px; width:300px; font-family:sans-serif; overflow:hidden;'>
                <a href='{url}' target='_blank' style='text-decoration:none; color:inherit;'>
                    <div style='width:100%; height:150px; background-image:url({imageUrl}); background-size:cover;'></div>
                    <div style='padding:10px;'>
                        <h3 style='margin:0; font-size:16px; font-weight:bold;'>{title}</h3>
                        <p style='margin:5px 0; font-size:14px; color:#555;'>{description}</p>
                    </div>
                </a>
            </div>";

            return htmlCard;
        }

        private string GetMetaContent(HtmlDocument htmlDoc, string propertyName)
        {
            var metaNode = htmlDoc.DocumentNode.SelectSingleNode($"//meta[@property='{propertyName}']") ??
                           htmlDoc.DocumentNode.SelectSingleNode($"//meta[@name='{propertyName}']");
            return metaNode?.GetAttributeValue("content", null);
        }
    }
}
