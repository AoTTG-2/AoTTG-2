using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using System.Collections.Generic;

public class LevelBuilder
{
    private static List<Level> _levels;
    public static List<Level> GetAllLevels()
    {
        if (_levels != null) return _levels;
        _levels = new List<Level>();
        AddClassicMaps();
        return _levels;
    }

    private static void AddClassicMaps()
    {
        _levels.Add(new Level
        {
            Name = "The City - Classic",
            Description = "Classic City Map from AoTTG",
            SceneName = "The City I",
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
                new WaveGamemode(),
                new CaptureGamemode(),
                new InfectionGamemode()
            }
        });

        _levels.Add(new Level
        {
            Name = "The Forest - Classic",
            Description = "Classic forest map",
            SceneName = "The Forest",
            Gamemodes = new List<GamemodeBase>
            {
                new KillTitansGamemode
                {
                    Name = "Annie",
                    Description = "Classic map where you fight the Female Titan",
                    Titans = 15,
                    Punks = false,
                    Pvp = PvpMode.AhssVsBlades
                },
                new WaveGamemode()
            }
        });

        _levels.Add(new Level
        {
            Name = "Trost - Classic",
            Description = "Classic trost map",
            SceneName = "Colossal Titan",
            Gamemodes = new List<GamemodeBase>
            {
                new TitanRushGamemode
                {
                    Name = "Colossal Titan",
                    Description = "Defeat the Colossal! Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate."
                },
                new TrostGamemode
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
            Gamemodes = new List<GamemodeBase>
            {
                new RacingGamemode()
            },
        });

        _levels.Add(new Level
        {
            Name = "Outside the Walls",
            Description = "Classic Outside the Walls map",
            SceneName = "OutSide",
            Gamemodes = new List<GamemodeBase>
            {
                new CaptureGamemode
                {
                    TitanChaseDistance = 200,
                    Horse = true,
                    Supply = true,
                    SpawnSupplyStationOnHumanCapture = true
                }
            },
        });

        _levels.Add(new Level
        {
            Name = "Custom",
            Description = "Custom Map",
            SceneName = "The Forest",
            Gamemodes = new List<GamemodeBase>
            {
                new KillTitansGamemode(),
                new WaveGamemode(),
                new InfectionGamemode(),
                new RacingGamemode(),
                new CaptureGamemode(),
                new EndlessGamemode(),
                new TitanRushGamemode(),
                new PvPAhssGamemode()
            }
        });

        _levels.Add(new Level
        {
            Name = "Cave Fight",
            Description = "***Spoiler Alarm!***",
            SceneName = "CaveFight",
            Gamemodes = new List<GamemodeBase>
            {
                new PvPAhssGamemode()
            }
        });

        _levels.Add(new Level
        {
            Name = "House Fight",
            Description = "***Spoiler Alarm!***",
            SceneName = "HouseFight",
            Gamemodes = new List<GamemodeBase>
            {
                new PvPAhssGamemode()
            }
        });
    }
}