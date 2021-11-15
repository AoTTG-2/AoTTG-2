using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class WaveGamemodeSettings : GamemodeSettings
    {
        public WaveGamemodeSettings()
        {
            GamemodeType = GamemodeType.Wave;
        }

        public WaveGamemodeSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Wave;
            Titan.Start = 3;
            Respawn.Mode = RespawnMode.NewRound;
            StartWave = 1;
            MaxWave = 20;
            WaveIncrement = 1;
            BossWave = 5;
            BossType = MindlessTitanType.Punk;

            switch (difficulty)
            {
                case Difficulty.Easy:
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Normal, 80f },
                        { MindlessTitanType.Abberant, 20f }
                    };
                    break;
                case Difficulty.Normal:
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Normal, 80f },
                        { MindlessTitanType.Abberant, 20f },
                        { MindlessTitanType.Jumper, 20f }
                    };
                    break;
                case Difficulty.Hard:
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Normal, 15f },
                        { MindlessTitanType.Abberant, 25f },
                        { MindlessTitanType.Jumper, 25f },
                        { MindlessTitanType.Punk, 5f },
                        { MindlessTitanType.Crawler, 3f }
                    };
                    break;
                case Difficulty.Abnormal:
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Normal, 20f },
                        { MindlessTitanType.Abberant, 20f },
                        { MindlessTitanType.Jumper, 20f },
                        { MindlessTitanType.Punk, 20f },
                        { MindlessTitanType.Crawler, 5f },
                        { MindlessTitanType.Abnormal, 20f }
                    };
                    break;
                case Difficulty.Realism:
                    Titan.Mindless.TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Abnormal, 100f }
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        [UiElement("Start Wave", "What is the start wave?")]
        public int? StartWave { get; set; }
        [UiElement("Max Wave", "What is the final wave?")]
        public int? MaxWave { get; set; }
        [UiElement("Wave Increment", "How many titans will spawn per wave?")]
        public int? WaveIncrement { get; set; }
        public int? BossWave { get; set; }
        public MindlessTitanType? BossType { get; set; }
    }
}
