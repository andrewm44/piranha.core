using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NoisyWeb.Services
{
    public class OpenAiService : IOpenAiService
    {

        string _apiKey = "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";

        string _endpoint = "https://api.openai.com/v1/chat/completions";
        public async Task<string> EnhanceText(string inText, int numWords)
        {
           // throw new NotImplementedException();

   
            var prompt = $"extend this text to {numWords} words : {inText}";

            string extendedText = ExtendTextAsync(prompt).Result;

            return extendedText;
        }

        private async Task<string> ExtendTextAsync(string prompt)
        {

            string extendedText = "";

            try
            {
                using var client = new HttpClient();

                // Add the OpenAI API key to the request headers
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                // Create the request body as a JSON object

                JObject jRequest = new JObject();

                jRequest.Add(new JProperty("model", "gpt-4o"));
                JArray jArrMessages = new JArray();
                JObject jMessage1 = new JObject();
                jMessage1.Add(new JProperty("role", "user"));
                JArray JArrContent = new JArray();
                JObject jContent1 = new JObject();
                jContent1.Add(new JProperty("type", "text"));
                jContent1.Add(new JProperty("text", prompt));
                JArrContent.Add(jContent1);
                jMessage1.Add("content", JArrContent);
                jArrMessages.Add(jMessage1);
                jRequest.Add("messages", jArrMessages);

                jRequest.Add(new JProperty("temperature", 1));
                jRequest.Add(new JProperty("max_tokens", 1024));
                // jRequest.Add("max_tokens", 1024);
                //   jRequest.Add("max_tokens", 1024);

                /*
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",  // You can also use "gpt-4" here
                    prompt = prompt,
                    max_tokens = 150,  // Define how much text to generate (in tokens)
                    temperature = 0.7  // Controls randomness (higher = more creative)
                };
                */

                // Serialize the request body into JSON format
               
             //   string jsonBody = JsonSerializer.Serialize(jRequest);

                // Prepare the HTTP request
                var requestContent = new StringContent(jRequest.ToString(Formatting.None), Encoding.UTF8, "application/json");

                // Send the POST request to OpenAI's completions endpoint
                HttpResponseMessage response = await client.PostAsync(_endpoint, requestContent);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content
                string responseContent = await response.Content.ReadAsStringAsync();

                // Parse the response and extract the generated text
               JObject jResp = JObject.Parse(responseContent);

                var c = (JArray)jResp.GetValue("choices");
                JObject c0 = (JObject)c[0];

                dynamic jsonObject = JsonConvert.DeserializeObject(c0.ToString());

                // Extract the value of "content"
                extendedText = jsonObject.message.content;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // Initialize HttpClient
        
            return extendedText.Trim();
        }


    }
}
