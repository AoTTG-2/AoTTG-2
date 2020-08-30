using Assets.Scripts.Gamemode;
using System;
using System.Collections.Generic;
using Assets.Scripts.Characters.Titan;

namespace Assets.Scripts.Settings.Titans
{
    public class MindlessTitanSettings : TitanSettings
    {
        public Dictionary<MindlessTitanType, float> TypeRatio { get; set; }

        public List<MindlessTitanType> Disabled { get; set; }

        public MindlessTitanSettings() { }

        public MindlessTitanSettings(Difficulty difficulty) : base(difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        {MindlessTitanType.Normal, 80f},
                        {MindlessTitanType.Abberant, 40f},
                        {MindlessTitanType.Jumper, 25f},
                        {MindlessTitanType.Punk, 15f},
                        {MindlessTitanType.Crawler, 5f},
                        {MindlessTitanType.Burster, 0f},
                        {MindlessTitanType.Stalker, 0f},
                        { MindlessTitanType.Abnormal, 5f }
                    };
                    Disabled = new List<MindlessTitanType>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
