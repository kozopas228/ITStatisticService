namespace ITStatisticService.Logic.Domain
{
    public interface IParserSettings
    {
        public string BaseUrl { get; set; }

        public string CertainUrl { get; set; }

        public int PagesAmount { get; set; }
        
        public string TechnologyUrlMap(Technologies technology);
    }
}