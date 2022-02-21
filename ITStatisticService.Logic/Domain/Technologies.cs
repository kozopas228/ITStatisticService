using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ITStatisticService.Logic.Domain
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Technologies
    {
        DotNet,
        Java,
        JavaScript,
        Python,
        Php,
        CPlusPlus,
        Qa
    }
}