using System;
using ITStatisticService.Logic.Domain;

namespace ITStatisticService.Logic.Implementation.DjinniCO
{
    public class DjinniCoParserSettings : IDjinniCoParserSettings
    {
        public string BaseUrl { get; set; }
        
        public string CertainUrl { get; set; }

        public int PagesAmount { get; set; }
        
        public string TechnologyUrlMap(Technologies technology)
        {
            switch (technology)
            {
                case Technologies.Java:
                    return "java";
                case Technologies.Php:
                    return "php";
                case Technologies.Python:
                    return "python";
                case Technologies.Qa:
                    return "qa";
                case Technologies.DotNet:
                    return "dotnet";
                case Technologies.JavaScript:
                    return "javascript";
                case Technologies.CPlusPlus:
                    return "cplusplus";
                default:
                    throw new ArgumentException("wrong argument");
            }
        }
    }
}