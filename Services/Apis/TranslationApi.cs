using System.Text;
using System.Text.Json.Serialization;

namespace news_apis
{
    public class TranslationApi
    {

        private readonly IConfiguration Configuration; // this configuration allows us to use the env variable in appsettings.json

        public TranslationApi()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private record TranslationApiResponse
        {
            [JsonPropertyName("translatedText")]
            public List<string>? TranslatedText { get; init; }

            [JsonPropertyName("originalText")]
            public string? OriginalText { get; init; }
        }
        public async Task<string> Translate(string text, string targetLang)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", Configuration["TranslationApis:SwiftTranslate"]);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "swift-translate.p.rapidapi.com");
            text = text.Replace("%", " percent");
            Console.WriteLine("Translating " + text + " into " + targetLang);
            if (targetLang == "zh") targetLang = "zh-cn";

            var requestData = new
            {
                text = text,
                sourceLang = "auto",
                targetLang = targetLang
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://swift-translate.p.rapidapi.com/translate", content);

            if (response.IsSuccessStatusCode)
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<TranslationApiResponse>(await response.Content.ReadAsStringAsync());
                if (data.TranslatedText == null) return "Error translating text";
                return data.TranslatedText[0];
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} while translating \"{text}\"");
                return "Error translating text";
            }
        }
        public async Task<string> CheckLanguage(string text)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", Configuration["TranslationApis:SwiftTranslate"]);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "swift-translate.p.rapidapi.com");

            var requestData = new
            {
                text = text,
                sourceLang = "auto",
                targetLang = "en"
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://swift-translate.p.rapidapi.com/translate", content);



            if (response.IsSuccessStatusCode)
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<TranslationApiResponse>(await response.Content.ReadAsStringAsync());
                if (data.TranslatedText == null) return "Error checking language";
                return data.TranslatedText[1];
            }
            return "";
        }
    }
}