using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ITStatisticService.Logic.Domain;

namespace ITStatisticService.Logic.Implementation.DjinniCO
{
    public class DjinniCoParser : IDjinniCoParser
    {
        public IParserSettings ParserSettings { get; set; }
        
        public HttpClient HttpClient { get; set; }

        public DjinniCoParser(HttpClient client, IDjinniCoParserSettings parserSettings)
        {
            this.HttpClient = client;
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

                // if no pages left
                var pager = document.QuerySelector(".pager");
                if (pager?.Children.Length == 1 && pager.Children.First().InnerHtml == "наступна" && i != 1)
                {
                    break;
                }

                IEnumerable<string> allSalariesInPage = document
                    .QuerySelectorAll(".public-salary-item")
                    .Select(x => x.TextContent);

                foreach (string salary in allSalariesInPage)
                {
                    if (salary.Split("-").Length > 2)
                    {
                        continue;
                    }
                    resultList.Add(new ParsingResult{Salary = ParseSalary(salary), Technology = technology.ToString()});
                }
            }

            return resultList;
        }

        private int ParseSalary(string salary)
        {
            int averageSalary;
            
            if (salary.Contains("-"))
            {
                var splited = salary.Split("-");
                var firstSalary = Int32.Parse(splited[0].Substring(1));
                var secondSalary = Int32.Parse(splited[1]);
                        
                averageSalary = (firstSalary + secondSalary) / 2;
            }
            else
            {
                var splited = salary.Split("$");
                averageSalary = Int32.Parse(splited[1]);
            }

            return averageSalary;
        }
    }
}