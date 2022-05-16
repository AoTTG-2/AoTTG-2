using Assets.Scripts.Settings.Gamemodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace Assets.Scripts.Settings.Converter
{
    public class GamemodeContractResolver : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(GamemodeSettings).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    public class GamemodeConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new GamemodeContractResolver() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(GamemodeSettings));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            switch ((GamemodeType) jo["GamemodeType"].Value<int>())
            {
                case GamemodeType.Titans:
                    return JsonConvert.DeserializeObject<KillTitansSettings>(jo.ToString(), SpecifiedSubclassConversion);
                case GamemodeType.Endless:
                    return JsonConvert.DeserializeObject<EndlessSettings>(jo.ToString(), SpecifiedSubclassConversion);
                case GamemodeType.Catch:
                    return JsonConvert.DeserializeObject<CatchGamemodeSettings>(jo.ToString(), SpecifiedSubclassConversion);
                case GamemodeType.Capture:
                    return JsonConvert.DeserializeObject<CaptureGamemodeSettings>(jo.ToString(), SpecifiedSubclassConversion);
                case GamemodeType.Wave:
                    return JsonConvert.DeserializeObject<WaveGamemodeSettings>(jo.ToString(), SpecifiedSubclassConversion);
                case GamemodeType.Racing:
                    return JsonConvert.DeserializeObject<RacingSettings>(jo.ToString(), SpecifiedSubclassConversion);
                case GamemodeType.Trost:
                    return JsonConvert.DeserializeObject<TrostSettings>(jo.ToString(), SpecifiedSubclassConversion);
                case GamemodeType.TitanRush:
                    return JsonConvert.DeserializeObject<RushSettings>(jo.ToString(), SpecifiedSubclassConversion);
                case GamemodeType.PvpAhss:
                    return JsonConvert.DeserializeObject<PvPAhssSettings>(jo.ToString(), SpecifiedSubclassConversion);
                case GamemodeType.Infection:
                    return JsonConvert.DeserializeObject<InfectionGamemodeSettings>(jo.ToString(), SpecifiedSubclassConversion);
                default:
                    throw new Exception();
            }
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}


