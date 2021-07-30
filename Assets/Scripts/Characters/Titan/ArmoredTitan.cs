using Assets.Scripts.Characters.Titan.Configuration;

namespace Assets.Scripts.Characters.Titan
{
    /// <summary>
    /// Not implemented
    /// </summary>
    public class ArmoredTitan : TitanBase
    {
        public override void Initialize(TitanConfiguration configuration)
        {
            EntityService.Register(this);
        }
    }
}
