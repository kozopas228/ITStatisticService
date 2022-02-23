using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ITStatisticService.Logic.Domain;

namespace ITStatisticService.Logic.Implementation.WorkUA
{
    public class WorkUaParser : IWorkUaParser
    {
        public IParserSettings ParserSettings { get; set; }
        public HttpClient HttpClient { get; set; }

        public WorkUaParser(HttpClient httpClient, IWorkUaParserSettings parserSettings)
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
                    .Replace("{technology}", mappedToUrlTechnology)
                    .Replace("{pageNumber}", i.ToString());
                string url = $"{ParserSettings.BaseUrl}{replacedCertainUrl}";
                
                IHtmlDocument document = await HtmlParsingHelper.GetHtmlDocument(HttpClient, url);

                IEnumerable<string> allSalariesInPage = document
                    .QuerySelectorAll(".add-top-xs")
                    .Where(x => x.PreviousElementSibling?.TagName == "DIV")
                    .Select(x => x.PreviousElementSibling?.QuerySelector("b")?.TextContent);

                foreach (var salary in allSalariesInPage)
                {
                    if(salary != null)
                        resultList.Add(new ParsingResult{Salary = ParseSalary(salary), Technology = technology.ToString()});
                }
                
                // if no pages left
                if (allSalariesInPage.Count() < 13)
                {
                    break;
                }
                
            }

            return resultList;
        }

        private int ParseSalary(string salary)
        {
            salary = salary
                .Replace("грн", "")
                .Replace(" ", "")
                .Replace(" ", "")
                .Replace(" ", "");
            int result;
            if (salary.Contains("–"))
            {
                var splited = salary.Split("–");
                result = (Int32.Parse(splited[0]) + Int32.Parse(splited[1])) / 2;
            }
            else
            {
                result = Int32.Parse(salary);
            }

            result = result / 28;
            return result;
        }
    }
}