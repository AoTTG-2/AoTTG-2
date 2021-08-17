using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.New.Types;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Settings.New
{
    [CreateAssetMenu(fileName = "Gamemode", menuName = "Settings/Gamemode", order = 1)]
    public class GamemodeSetting : BaseSettings
    {
        [SerializeField] private new string name;

        public new string Name
        {
            get => name ?? GamemodeType.ToString();
            set => name = value;
        }

        public string Description;

        public GamemodeType GamemodeType { get; protected set; }

        public BoolSetting LavaMode;
        public EnumSetting<TeamMode> Single;
        //public MultipleEnumSetting<TeamMode> TeamMode;
        //public IntSetting Test;

        public override void Override(BaseSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
