using System;
using ITStatisticService.Logic.Domain;
using ITStatisticService.Logic.Implementation.DjinniCO;

namespace ITStatisticService.Logic.Implementation.RabotaUA
{
    public class RabotaUaParserSettings : IRabotaUaParserSettings
    {
        public string BaseUrl { get; set; }
        public string CertainUrl { get; set; }
        public int PagesAmount { get; set; }
        public string TechnologyUrlMap(Technologies technology)
        {
            switch (technology)
            {
                case Technologies.Java:
                    return "";
                case Technologies.Php:
                    return "";
                case Technologies.Python:
                    return "";
                case Technologies.Qa:
                    return "";
                case Technologies.DotNet:
                    return ".net";
                case Technologies.JavaScript:
                    return "";
                case Technologies.CPlusPlus:
                    return "";
                default:
                    throw new ArgumentException("wrong argument");
            }
        }
    }
}