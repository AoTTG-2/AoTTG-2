using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Gamemode.Settings;
using System.Collections.Generic;

public class LevelBuilder
{
    private static List<Level> _levels;
    public static List<Level> GetAllLevels()
    {
        if (_levels != null) return _levels;
        _levels = new List<Level>();
        AddClassicMaps();
        AddAoTTG2Maps();
        return _levels;
    }

    private static void AddClassicMaps()
    {
        _levels.Add(new Level
        {
            Name = "Test Zone",
            Description = "Classic City Map from AoTTG",
            SceneName = "Test Zone",
            Gamemodes = new List<GamemodeSettings>
            {
                new KillTitansSettings
                {
                    GamemodeType = GamemodeType.Titans,
                    Titans = 1
                }
            }
        });

        _levels.Add(new Level
        {
            Name = "The City - Classic",
            Description = "Classic City Map from AoTTG",
            SceneName = "The City I",
            Gamemodes = new List<GamemodeSettings>
            {
                new KillTitansSettings
                {
                    GamemodeType = GamemodeType.Titans,
                    Titans = 10
                },
                new EndlessSettings
                {
                    GamemodeType = GamemodeType.Endless,
                    Titans = 10
                },
                new WaveGamemodeSettings(),
                new CaptureGamemodeSettings(),
                new InfectionGamemodeSettings()
            }
        });

        _levels.Add(new Level
        {
            Name = "The Forest - Classic",
            Description = "Classic forest map",
            SceneName = "The Forest",
            Gamemodes = new List<GamemodeSettings>
            {
                new WaveGamemodeSettings(),
                new KillTitansSettings
                {
                    GamemodeType = GamemodeType.Titans,
                    Name = "Annie",
                    Description = "Classic map where you fight the Female Titan",
                    DisabledTitans = new List<MindlessTitanType> {MindlessTitanType.Punk},
                    Pvp = PvpMode.AhssVsBlades
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
                    TitanCustomSize = false,
                },
                new TrostSettings
                {
                    Name = "Trost",
                    Description = "Escort Titan Eren"
                }
            }
        });

        _levels.Add(new Level
        {
            Name = "Akina",
            Description = "Most famous racing map",
            SceneName = "track - akina",
            Gamemodes = new List<GamemodeSettings>
            {
                new RacingSettings()
            },
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
                    TitanChaseDistance = 200,
                    Horse = true,
                    Supply = true,
                    SpawnSupplyStationOnHumanCapture = true
                }
            }
        });

        _levels.Add(new Level
        {
            Name = "Custom",
            Description = "Custom Map",
            SceneName = "The Forest",
            Gamemodes = new List<GamemodeSettings>
            {
                new KillTitansSettings(),              
                new WaveGamemodeSettings(),
                new InfectionGamemodeSettings(),
                new RacingSettings(),
                new CaptureGamemodeSettings(),
                new RushSettings(),
                new EndlessSettings(),
                new PvPAhssSettings()
            }
        });

        _levels.Add(new Level
        {
            Name = "Cave Fight",
            Description = "***Spoiler Alarm!***",
            SceneName = "CaveFight",
            Gamemodes = new List<GamemodeSettings>
            {
                new PvPAhssSettings()
            }
        });

        _levels.Add(new Level
        {
            Name = "House Fight",
            Description = "***Spoiler Alarm!***",
            SceneName = "HouseFight",
            Gamemodes = new List<GamemodeSettings>
            {
                new PvPAhssSettings()
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
            Gamemodes = new List<GamemodeBase>
            {
                new KillTitansGamemode
                {
                    Titans = 10
                },
                new EndlessGamemode
                {
                    Titans = 10
                },
                new WaveGamemode()
            }
        });
    }
}