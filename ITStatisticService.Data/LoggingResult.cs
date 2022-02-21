using System;

namespace ITStatisticService.Data
{
    public class LoggingResult
    {
        public Guid Id { get; set; }
        public string Technology { get; set; }
        public int Salary { get; set; }
        public DateTime Date { get; set; }
    }
}