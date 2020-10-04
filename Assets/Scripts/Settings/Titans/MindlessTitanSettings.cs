using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Settings.Titans
{
    public class MindlessTitanSettings : TitanSettings
    {
        public Dictionary<MindlessTitanType, float> TypeRatio { get; set; }

        public List<MindlessTitanType> Disabled { get; set; }

        public Dictionary<MindlessTitanType, TitanSettings> TypeSettings { get; set; }

        public MindlessTitanSettings() { }

        public MindlessTitanSettings(Difficulty difficulty) : base(difficulty)
        {
            // Example on how a single type can override base class settings
            TypeSettings = new Dictionary<MindlessTitanType, TitanSettings>
            {
                {
                    MindlessTitanType.Normal, new TitanSettings
                    {
                        Speed = 7f
                    }
                },
                {
                    MindlessTitanType.Abberant, new TitanSettings
                    {
                        Speed = 16f,
                        RunSpeed = 20f
                    }
                },
                {
                    MindlessTitanType.Jumper, new TitanSettings
                    {
                        Speed = 16f,
                        RunSpeed = 20f
                    }
                },
                {
                    MindlessTitanType.Punk, new TitanSettings
                    {
                        Speed = 9f,
                        RunSpeed = 18f
                    }
                },
                {
                    MindlessTitanType.Crawler, new TitanSettings
                    {
                        Speed = 22f,
                        RunSpeed = 37f
                    }
                },
                {
                    MindlessTitanType.Stalker, new TitanSettings
                    {
                        Speed = 18f
                    }
                },
                {
                    MindlessTitanType.Burster, new TitanSettings
                    {
                        Speed = 18f
                    }
                },
            };

            switch (difficulty)
            {
                case Difficulty.Easy:
                    TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        {MindlessTitanType.Normal, 80f},
                        {MindlessTitanType.Abberant, 20f},
                        {MindlessTitanType.Jumper, 15f},
                        {MindlessTitanType.Punk, 0f},
                        {MindlessTitanType.Crawler, 0f},
                        {MindlessTitanType.Burster, 0f},
                        {MindlessTitanType.Stalker, 0f},
                        {MindlessTitanType.Abnormal, 0f }
                    };
                    Disabled = new List<MindlessTitanType> { MindlessTitanType.Punk, MindlessTitanType.Crawler, MindlessTitanType.Burster, MindlessTitanType.Stalker };
                    break;
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
