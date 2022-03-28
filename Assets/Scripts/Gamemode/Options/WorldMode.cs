using Assets.Scripts.Gamemode.Racing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assets.Scripts.Gamemode.Options
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorldMode
    {
        /// <summary>
        /// Doesn't modify anything
        /// </summary>
        Default,
        /// <summary>
        /// Enable Lava Mode within the World
        /// </summary>
        Lava,
        /// <summary>
        /// Enable Water Mode within the World. All <see cref="RacingKillTrigger"/> will no longer kill players and become hookable
        /// </summary>
        Water
    }
}
