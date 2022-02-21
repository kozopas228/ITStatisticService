using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace ITStatisticService.Logic.Domain
{
    public class HtmlParsingHelper
    {
        public static async Task<IHtmlDocument> GetHtmlDocument(HttpClient client, string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);

            string source;
            if (response.IsSuccessStatusCode)
            {
                source = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"error occured with status code {response.StatusCode}");
            }

            var htmlParser = new HtmlParser();
            var document = htmlParser.ParseDocument(source);

            return document;
        }
    }
}