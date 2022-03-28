using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Credits
{
    [CreateAssetMenu(fileName = "Contributor", menuName = "Settings/Contributor", order = 2)]
    public class Contributor : BaseSettings
    {
        public string Role;
        public string Quote;
        public MindlessTitanType TitanType;
        public int Health;
        public float Size;
        public Color Color;
        //TODO: Eventually additional titan configuration should be here

        public override void Override(BaseSettings settings)
        {
            
        }
    }
}
