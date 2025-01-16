using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assets.Scripts.Gamemode.Options
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TeamMode
    {
        Disabled,
        NoSort,
        LockBySize,
        LockBySkill
    }
}
