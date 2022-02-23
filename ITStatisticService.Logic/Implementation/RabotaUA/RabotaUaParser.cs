using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ITStatisticService.Logic.Domain;

namespace ITStatisticService.Logic.Implementation.RabotaUA
{
    public class RabotaUaParser : IRabotaUaParser
    {
        //%d1%83%d0%ba%d1%80%d0%b0%d0%b8%d0%bd%d0%b0
        public IParserSettings ParserSettings { get; set; }
        public HttpClient HttpClient { get; set; }

        public RabotaUaParser(HttpClient httpClient, IRabotaUaParserSettings parserSettings)
        {
            this.HttpClient = httpClient;
            this.ParserSettings = parserSettings;
        }
        public async Task<IEnumerable<ParsingResult>> Parse(Technologies technology)
        {
            var resultList = new List<ParsingResult>();
            for (int i = 1; i <= ParserSettings.PagesAmount; i++)
            {
                string mappedToUrlTechnology = ParserSettings.TechnologyUrlMap(technology);
                string replacedCertainUrl = ParserSettings.CertainUrl
                    .Replace("{technology}", mappedToUrlTechnology);

                if (i != 1)
                {
                    replacedCertainUrl = replacedCertainUrl.Replace("{pageNumber}", i.ToString());
                }
                else
                {
                    replacedCertainUrl = replacedCertainUrl.Replace("/pg{pageNumber}", "");
                }
                
                string url = $"{ParserSettings.BaseUrl}{replacedCertainUrl}";

                IHtmlDocument document = await HtmlParsingHelper.GetHtmlDocument(HttpClient, url);

                
                IEnumerable<string> allSalariesInPage = document
                    .QuerySelectorAll(".salary")
                    .Select(x => x.TextContent);
                
                var salariesHtml = document
                    .QuerySelectorAll(".salary")
                    .Select(x => x.InnerHtml)
                    .Select(x => x.Replace("&nbsp;", ""));
                
                
                
                

                // if no pages left
                var nextButton = document.QuerySelector(".nextBtn");
                if (nextButton.Children.Length == 0)
                {
                    break;
                }
            }

            return resultList;
        }
    }
}