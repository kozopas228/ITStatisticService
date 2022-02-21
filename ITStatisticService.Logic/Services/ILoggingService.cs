using System.Threading.Tasks;
using ITStatisticService.Logic.Domain;

namespace ITStatisticService.Logic.Services
{
    public interface ILoggingService
    {
        Task Log(ParsingResult result);
    }
}