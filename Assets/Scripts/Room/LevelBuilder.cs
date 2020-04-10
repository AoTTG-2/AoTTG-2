using Assets.Scripts.Gamemode;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder
{
    private static List<Level> levels;
    public static List<Level> GetAllLevels()
    {
        if (levels != null) return levels;
        levels = new List<Level>();
        AddClassicMaps();
        return levels;
    }

    private static void AddClassicMaps()
    {
        levels.Add(new Level
        {
            Name = "The City - Classic",
            Description = "Classic City Map from AoTTG",
            Minimap = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 731.9738f),
            SceneName = "The City I",
            Gamemodes = new List<GamemodeBase>
            {
                new KillTitansGamemode
                {
                    Titans = 10,
                    Pvp = true,
                },
                new EndlessGamemode
                {
                    Titans = 10,
                    Pvp = false,
                },
                new WaveGamemode(),
                new CaptureGamemode()
            }
        });

        levels.Add(new Level
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
                    Pvp = true
                },
                new WaveGamemode()
            }
        });

        levels.Add(new Level
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

        levels.Add(new Level
        {
            Name = "Akina",
            Description = "Most famous racing map",
            SceneName = "track - akina",
            Gamemodes = new List<GamemodeBase>
            {
                new RacingGamemode()
            },
            Minimap = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f)
        });

        levels.Add(new Level
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
                }
            },
            Minimap = new Minimap.Preset(new Vector3(2549.4f, 0f, 3042.4f), 3697.16f)
        });

        levels.Add(new Level
        {
            Name = "Custom",
            Description = "Custom Map",
            SceneName = "The Forest",
            Gamemodes = new List<GamemodeBase>
            {
                new KillTitansGamemode()
            }
        });

        levels.Add(new Level
        {
            Name = "Cave Fight",
            Description = "***Spoiler Alarm!***",
            SceneName = "CaveFight",
            Gamemodes = new List<GamemodeBase>
            {
                new PvPAhssGamemode()
            }
        });

        levels.Add(new Level
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