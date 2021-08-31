using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.Settings.Titans;
using System.Collections.Generic;
using Assets.Scripts.CustomMaps;
using Assets.Scripts.Services;

namespace Assets.Scripts.Room
{
    /// <summary>
    /// Current way how levels are displayed. Will be replaced with #604 (Level and Settings: ScriptableObjects)
    /// </summary>
    public static class LevelBuilder
    {
        private static List<Level> _levels;
        public static List<Level> GetAllLevels()
        {
            if (_levels != null) return _levels;
            _levels = new List<Level>();
            AddClassicMaps();
            AddAoTTG2Maps();
            AddCustomMaps();
            return _levels;
        }

        private static void AddCustomMaps()
        {
            //foreach (var level in LevelHelper.GetAll())
            //{
            //    _levels.Add(new Level
            //    {
            //        Name = level.Split('_')[0],
            //        SceneName = null,
            //        AssetBundle = level,
            //        IsCustom = true,
            //        Gamemodes = new List<GamemodeSettings>
            //    {
            //        new RacingSettings(),
            //        new KillTitansSettings(),
            //        new WaveGamemodeSettings(),
            //        new InfectionGamemodeSettings(),
            //        new CaptureGamemodeSettings(),
            //        new RushSettings(),
            //        new EndlessSettings(),
            //        new PvPAhssSettings()
            //    }
            //    });
            //}

            var maps = Service.Map.GetCustomMaps();
            foreach (var map in maps)
            {
                _levels.Add(new Level
                {
                    Name = map.Name,
                    SceneName = "Custom",
                    Type = CustomMapType.CustomMap,
                    Gamemodes = new List<GamemodeSettings>
                        {
                            new RacingSettings(),
                            new KillTitansSettings(),
                            new WaveGamemodeSettings(),
                            new InfectionGamemodeSettings(),
                            new CaptureGamemodeSettings(),
                            new RushSettings(),
                            new EndlessSettings(),
                            new PvPAhssSettings()
                        }
                });
            }
        }

        private static void AddClassicMaps()
        {
            _levels.Add(new Level
            {
                Name = "Akina",
                Description = "Most famous racing map",
                SceneName = "track - akina",
                Gamemodes = new List<GamemodeSettings>
                {
                    new RacingSettings
                    {
                        IsPlayerTitanEnabled = false,
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    }
                },
            });

            _levels.Add(new Level
            {
                Name = "The City - Classic",
                Description = "Classic City Map from AoTTG",
                SceneName = "The City I",

                Gamemodes = new List<GamemodeSettings>
                {
                    new CatchGamemodeSettings()
                    {
                        GamemodeType = GamemodeType.Catch,
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new KillTitansSettings
                    {
                        GamemodeType = GamemodeType.Titans,
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new EndlessSettings
                    {
                        GamemodeType = GamemodeType.Endless,
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new WaveGamemodeSettings
                    {
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new CaptureGamemodeSettings
                    {
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new RacingSettings
                    {
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new InfectionGamemodeSettings
                    {
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                }
            });

            _levels.Add(new Level
            {
                Name = "The Forest - Classic",
                Description = "Classic forest map",
                SceneName = "The Forest",
                Gamemodes = new List<GamemodeSettings>
                {
                    new WaveGamemodeSettings
                    {
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new KillTitansSettings
                    {
                        GamemodeType = GamemodeType.Titans,
                        Name = "Annie",
                        Description = "Classic map where you fight the Female Titan",
                        Titan = new SettingsTitan
                        {
                            Mindless = new MindlessTitanSettings
                            {
                                Disabled = new List<MindlessTitanType> {MindlessTitanType.Punk}
                            }
                        },
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    }
                }
            });

            _levels.Add(new Level
            {
                Name = "Trost - Classic",
                Description = "Classic trost map",
                SceneName = "Colossal Titan",
                Gamemodes = new List<GamemodeSettings>
                {
                    new RushSettings
                    {
                        Name = "Colossal Titan",
                        Description = "Defeat the Colossal! Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.",
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new TrostSettings
                    {
                        Name = "Trost",
                        Description = "Escort Titan Eren",
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    }
                }
            });

            _levels.Add(new Level
            {
                Name = "Outside the Walls",
                Description = "Classic Outside the Walls map",
                SceneName = "OutSide",
                Gamemodes = new List<GamemodeSettings>
                {
                    new CaptureGamemodeSettings
                    {
                        Horse = new HorseSettings
                        {
                            Enabled = true
                        },
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                        Supply = true,
                        SpawnSupplyStationOnHumanCapture = true
                    }
                }
            });

            _levels.Add(new Level
            {
                Name = "Cave Fight",
                Description = "***Spoiler Alarm!***",
                SceneName = "CaveFight",
                Gamemodes = new List<GamemodeSettings>
                {
                    new PvPAhssSettings
                    {
                        IsPlayerTitanEnabled = false,
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    }
                }
            });

            _levels.Add(new Level
            {
                Name = "House Fight",
                Description = "***Spoiler Alarm!***",
                SceneName = "HouseFight",
                Gamemodes = new List<GamemodeSettings>
                {
                    new PvPAhssSettings
                    {
                        IsPlayerTitanEnabled = false,
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    }
                }
            });

            _levels.Add(new Level
            {
                Name = "Test Zone",
                Description = "Classic City Map from AoTTG",
                SceneName = "Test Zone",
                Gamemodes = new List<GamemodeSettings>
                {
                    new KillTitansSettings
                    {
                        Name = "Test",
                        Description = "Classic map where you fight the Female Titan",
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new KillTitansSettings
                    {
                        Titan = new SettingsTitan()
                        {
                            Start = 20
                        },
                        IsPlayerTitanEnabled = true,
                        Pvp = new PvPSettings
                        {
                            Cannons = true
                        },
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new WaveGamemodeSettings
                    {
                        IsPlayerTitanEnabled = true,
                        Time = new TimeSettings
                        {
                            CurrentTime = 12,
                            DayLength = 300,
                            Pause = true,
                        },
                    }

                }
            });
        }

        private static void AddAoTTG2Maps()
        {
            _levels.Add(new Level
            {
                Name = "Utgard Castle",
                Description = "",
                SceneName = "Utgard",
                Gamemodes = new List<GamemodeSettings>
                {
                    new KillTitansSettings
                    {
                        Time = new TimeSettings
                        {
                            CurrentTime = 24,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new CaptureGamemodeSettings
                    {
                        Time = new TimeSettings
                        {
                            CurrentTime = 24,
                            DayLength = 300,
                            Pause = true,
                        },
                        SpawnSupplyStationOnHumanCapture = false,
                        PvpHumanScoreLimit = 400,
                        PvpTitanScoreLimit = 400
                    },
                    new EndlessSettings
                    {
                        Time = new TimeSettings
                        {
                            CurrentTime = 24,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                    new WaveGamemodeSettings
                    {
                        Time = new TimeSettings
                        {
                            CurrentTime = 24,
                            DayLength = 300,
                            Pause = true,
                        },
                    },
                }
            });
        }
    }
}