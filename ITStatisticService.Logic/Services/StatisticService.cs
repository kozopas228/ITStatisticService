using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ITStatisticService.Logic.Domain;
using ITStatisticService.Logic.Implementation.DjinniCO;
using Newtonsoft.Json.Converters;

namespace ITStatisticService.Logic.Services
{
    public class StatisticService
    {
        private readonly ILoggingService _loggingService;

        private readonly List<IParser> _parsers;

        public StatisticService(IDjinniCoParser djinniCoParser, ILoggingService loggingService)
        {
            _loggingService = loggingService;

            _parsers = new List<IParser>();
            _parsers.Add(djinniCoParser);
        }

        public async Task<double> GetAverageSalaryByTechnology(Technologies technology)
        {
            var results = new List<ParsingResult>();
            foreach (var parser in _parsers)
            {
                results.AddRange(await parser.Parse(technology));
            }
            return results.Average(x => x.Salary);
        }

        public async Task<ParsingResult> GetHighestAverageSalary()
        {
            var salariesList = new List<ParsingResult>(); 
            foreach (Technologies technology in (Technologies[]) Enum.GetValues(typeof(Technologies)))
            {
                var salary = (int)Math.Round(await GetAverageSalaryByTechnology(technology));
                salariesList.Add(new ParsingResult{Salary = salary, Technology = technology.ToString()});
            }

            var maxSalary = salariesList.Max(x => x.Salary);
            var result =  salariesList.First(x => x.Salary == maxSalary);
            await _loggingService.Log(result);
            
            return result;
        }
        
        public async Task<ParsingResult> GetLowestAverageSalary()
        {
            var salariesList = new List<ParsingResult>(); 
            foreach (Technologies technology in (Technologies[]) Enum.GetValues(typeof(Technologies)))
            {
                var salary = (int)Math.Round(await GetAverageSalaryByTechnology(technology));
                salariesList.Add(new ParsingResult{Salary = salary, Technology = technology.ToString()});
            }

            var minSalary = salariesList.Min(x => x.Salary);
            return salariesList.First(x => x.Salary == minSalary);
        }
    }
}