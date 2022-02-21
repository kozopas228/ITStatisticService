using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ITStatisticService.Logic.Domain
{
    public interface IParser
    {
        public IParserSettings ParserSettings { get; set; }
        
        public HttpClient HttpClient { get; set; }

        public Task<IEnumerable<ParsingResult>> Parse(Technologies technology);
    }
}