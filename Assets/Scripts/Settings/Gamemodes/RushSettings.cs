using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class RushSettings : GamemodeSettings
    {
        [UiElement("Titan frequency", "1 titan will spawn per Interval")]
        public int? TitanInterval { get; set; }

        [UiElement("Titan Group Internval", "X titans will spawn per Interval")]
        public int? TitanGroupInterval { get; set; }
        [UiElement("Titan Group Size", "X titans will spawn per Interval")]
        public int? TitanGroupSize { get; set; }

        public RushSettings()
        {
            GamemodeType = GamemodeType.TitanRush;
        }

        public RushSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.TitanRush;
            Titan.Start = 2;
            TitanInterval = 7;
            TitanGroupInterval = 0;
            TitanGroupSize = 0;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    TitanInterval = 10;
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Normal, 80f },
                        { MindlessTitanType.Abberant, 20f }
                    };
                    break;
                case Difficulty.Normal:
                    TitanInterval = 7;
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Normal, 60f },
                        { MindlessTitanType.Abberant, 40f }
                    };
                    break;
                case Difficulty.Hard:
                    TitanInterval = 6;
                    TitanGroupInterval = 30;
                    TitanGroupSize = 3;
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Normal, 20f },
                        { MindlessTitanType.Abberant, 80f }
                    };
                    break;
                case Difficulty.Abnormal:
                    TitanInterval = 5;
                    TitanGroupInterval = 30;
                    TitanGroupSize = 5;
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Abberant, 100f }
                    };
                    break;
                case Difficulty.Realism:
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Abnormal, 100f }
                    };
                    TitanInterval = 5;
                    TitanGroupInterval = 30;
                    TitanGroupSize = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
