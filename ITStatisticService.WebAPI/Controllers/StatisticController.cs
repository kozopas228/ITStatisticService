using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITStatisticService.Logic.Domain;
using ITStatisticService.Logic.Implementation.RabotaUA;
using ITStatisticService.Logic.Implementation.WorkUA;
using ITStatisticService.Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITStatisticService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticController : ControllerBase
    {
        private readonly StatisticService _service;
        private readonly IWorkUaParser _workUaParser;

        public StatisticController(StatisticService service, IWorkUaParser workUaParser)
        {
            _service = service;
            _workUaParser = workUaParser;
        }

        [HttpGet("GetAverageSalaryByTechnology")]
        public async Task<IActionResult> GetAverageSalaryByTechnology(Technologies technology)
        {
            var salary = await _service.GetAverageSalaryByTechnology(technology);
            return new JsonResult(new ParsingResult{ Salary = (int)Math.Round(salary), Technology = technology.ToString() });
        }
        
        [HttpGet("GetHighestAverageSalary")]
        public async Task<IActionResult> GetHighestAverageSalary()
        {
            var salary = await _service.GetHighestAverageSalary();
            return new JsonResult(salary);
        }
        
        [HttpGet("GetLowestAverageSalary")]
        public async Task<IActionResult> GetLowestAverageSalary()
        {
            var salary = await _service.GetLowestAverageSalary();
            return new JsonResult(salary);
        }

        [HttpOptions]
        public async Task<IActionResult> Test()
        {
            return new JsonResult(await _workUaParser.Parse(Technologies.Java));
        }
    }
}