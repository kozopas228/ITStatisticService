using System;
using System.Threading.Tasks;
using ITStatisticService.Data;
using ITStatisticService.Logic.Domain;

namespace ITStatisticService.Logic.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ApplicationContext _context;

        public LoggingService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Log(ParsingResult result)
        {
            var loggingResult = new LoggingResult
            {
                Technology = result.Technology,
                Salary = result.Salary,
                Date = DateTime.Now
                Number = DateTime.Yesterday

            };
            await _context.Results.AddAsync(loggingResult);
            await _context.SaveChangesAsync();
        }
    }
}
