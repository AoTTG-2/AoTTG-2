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
            Disabled = new List<MindlessTitanType>();
            switch (difficulty)
            {
                case Difficulty.Easy:
                    TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        {MindlessTitanType.Normal, 80f},
                        {MindlessTitanType.Abberant, 20f},
                        {MindlessTitanType.Jumper, 15f}
                    };
                    TypeSettings = new Dictionary<MindlessTitanType, TitanSettings>
                    {
                        {
                            MindlessTitanType.Normal, new TitanSettings
                            {
                                Speed = 6f
                            }
                        },
                        {
                            MindlessTitanType.Abberant, new TitanSettings
                            {
                                Speed = 14f,
                                RunSpeed = 18f
                            }
                        },
                        {
                            MindlessTitanType.Jumper, new TitanSettings
                            {
                                Speed = 15f,
                                RunSpeed = 18f
                            }
                        },
                        {
                            MindlessTitanType.Punk, new TitanSettings
                            {
                                Speed = 8f,
                                RunSpeed = 15f
                            }
                        },
                        {
                            MindlessTitanType.Crawler, new TitanSettings
                            {
                                Speed = 18f,
                                RunSpeed = 30f
                            }
                        },
                        {
                            MindlessTitanType.Stalker, new TitanSettings
                            {
                                Speed = 15f
                            }
                        },
                        {
                            MindlessTitanType.Burster, new TitanSettings
                            {
                                Speed = 15f
                            }
                        },
                        {
                            MindlessTitanType.Abnormal, new TitanSettings
                            {
                                Speed = 14f,
                                RunSpeed = 18f
                            }
                        }
                    };
                    Disabled = new List<MindlessTitanType> { MindlessTitanType.Punk, MindlessTitanType.Crawler, MindlessTitanType.Burster, MindlessTitanType.Stalker };
                    break;
                case Difficulty.Normal:
                    TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        {MindlessTitanType.Normal, 50f},
                        {MindlessTitanType.Abberant, 20f},
                        {MindlessTitanType.Jumper, 15f},
                        {MindlessTitanType.Crawler, 5f }
                    };
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
                        {
                            MindlessTitanType.Abnormal, new TitanSettings
                            {
                                Speed = 16f,
                                RunSpeed = 20f
                            }
                        }
                    };
                    break;
                case Difficulty.Hard:
                    TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        {MindlessTitanType.Normal, 25f},
                        {MindlessTitanType.Abberant, 20f},
                        {MindlessTitanType.Jumper, 15f},
                        {MindlessTitanType.Punk, 5f },
                        {MindlessTitanType.Crawler, 5f },
                        {MindlessTitanType.Abnormal, 2f },
                    };
                    TypeSettings = new Dictionary<MindlessTitanType, TitanSettings>
                    {
                        {
                            MindlessTitanType.Normal, new TitanSettings
                            {
                                Speed = 9f
                            }
                        },
                        {
                            MindlessTitanType.Abberant, new TitanSettings
                            {
                                Speed = 18f,
                                RunSpeed = 22f
                            }
                        },
                        {
                            MindlessTitanType.Jumper, new TitanSettings
                            {
                                Speed = 18f,
                                RunSpeed = 23f
                            }
                        },
                        {
                            MindlessTitanType.Punk, new TitanSettings
                            {
                                Speed = 10f,
                                RunSpeed = 24f
                            }
                        },
                        {
                            MindlessTitanType.Crawler, new TitanSettings
                            {
                                Speed = 25f,
                                RunSpeed = 40f
                            }
                        },
                        {
                            MindlessTitanType.Stalker, new TitanSettings
                            {
                                Speed = 22f
                            }
                        },
                        {
                            MindlessTitanType.Burster, new TitanSettings
                            {
                                Speed = 22f
                            }
                        },
                        {
                            MindlessTitanType.Abnormal, new TitanSettings
                            {
                                Speed = 18f,
                                RunSpeed = 23f
                            }
                        }
                    };
                    break;
                case Difficulty.Abnormal:
                    TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        {MindlessTitanType.Normal, 50f},
                        {MindlessTitanType.Abberant, 40f},
                        {MindlessTitanType.Jumper, 25f},
                        {MindlessTitanType.Punk, 15f},
                        {MindlessTitanType.Crawler, 5f},
                        { MindlessTitanType.Abnormal, 5f }
                    };
                    TypeSettings = new Dictionary<MindlessTitanType, TitanSettings>
                    {
                        {
                            MindlessTitanType.Normal, new TitanSettings
                            {
                                Speed = 10f
                            }
                        },
                        {
                            MindlessTitanType.Abberant, new TitanSettings
                            {
                                Speed = 20f,
                                RunSpeed = 24f
                            }
                        },
                        {
                            MindlessTitanType.Jumper, new TitanSettings
                            {
                                Speed = 20f,
                                RunSpeed = 24f
                            }
                        },
                        {
                            MindlessTitanType.Punk, new TitanSettings
                            {
                                Speed = 12f,
                                RunSpeed = 25f
                            }
                        },
                        {
                            MindlessTitanType.Crawler, new TitanSettings
                            {
                                Speed = 30f,
                                RunSpeed = 50f
                            }
                        },
                        {
                            MindlessTitanType.Stalker, new TitanSettings
                            {
                                Speed = 25f
                            }
                        },
                        {
                            MindlessTitanType.Burster, new TitanSettings
                            {
                                Speed = 25f
                            }
                        },
                        {
                            MindlessTitanType.Abnormal, new TitanSettings
                            {
                                Speed = 20f,
                                RunSpeed = 25f
                            }
                        }
                    };
                    Disabled = new List<MindlessTitanType>();
                    break;
                case Difficulty.Realism:
                    TypeSettings = new Dictionary<MindlessTitanType, TitanSettings>
                    {
                        {
                            MindlessTitanType.Normal, new TitanSettings
                            {
                                Speed = 10f
                            }
                        },
                        {
                            MindlessTitanType.Abberant, new TitanSettings
                            {
                                Speed = 20f,
                                RunSpeed = 24f
                            }
                        },
                        {
                            MindlessTitanType.Jumper, new TitanSettings
                            {
                                Speed = 20f,
                                RunSpeed = 24f
                            }
                        },
                        {
                            MindlessTitanType.Punk, new TitanSettings
                            {
                                Speed = 12f,
                                RunSpeed = 25f
                            }
                        },
                        {
                            MindlessTitanType.Crawler, new TitanSettings
                            {
                                Speed = 30f,
                                RunSpeed = 50f
                            }
                        },
                        {
                            MindlessTitanType.Stalker, new TitanSettings
                            {
                                Speed = 25f
                            }
                        },
                        {
                            MindlessTitanType.Burster, new TitanSettings
                            {
                                Speed = 25f
                            }
                        },
                        {
                            MindlessTitanType.Abnormal, new TitanSettings
                            {
                                Speed = 20f,
                                RunSpeed = 25f
                            }
                        }
                    };
                    TypeRatio = new Dictionary<MindlessTitanType, float>
                    {
                        { MindlessTitanType.Abnormal, 100f }
                    };
                    Disabled = new List<MindlessTitanType>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
