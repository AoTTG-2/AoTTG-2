using System;
using Assets.Scripts.Gamemode;
using UnityEngine;

public class LevelInfo
{
    public string desc;
    public int enemyNumber;
    public bool hint;
    public bool horse;
    private static bool init;
    public bool lavaMode;
    public static LevelInfo[] levels;
    public string mapName;
    public Minimap.Preset minimapPreset;
    public string name;
    public bool noCrawler;
    public bool punk = true;
    public bool pvp;
    public RespawnMode respawnMode;
    public bool supply = true;
    public bool teamTitan;
    public GamemodeBase GameMode;

    public static LevelInfo getInfo(string name)
    {
        initData2();
        foreach (LevelInfo info in levels)
        {
            if (info.name == name)
            {
                return info;
            }
        }
        //HACK
        return levels[0];
    }

    private static void initData2()
    {
        if (!init)
        {
            init = true;
            levels = new LevelInfo[] { 
                new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), 
                new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo(), new LevelInfo()
             };
            levels[0].name = "The City";
            levels[0].mapName = "The City I";
            levels[0].desc = "kill all the titans with your friends.(No RESPAWN/SUPPLY/PLAY AS TITAN)";
            levels[0].enemyNumber = 10;
            levels[0].respawnMode = RespawnMode.NEVER;
            levels[0].supply = true;
            levels[0].teamTitan = true;
            levels[0].pvp = true;
            levels[0].GameMode = new WaveGamemode();

            levels[1].name = "The City II";
            levels[1].mapName = "The City I";
            levels[1].desc = "Fight the titans with your friends.(RESPAWN AFTER 10 SECONDS/SUPPLY/TEAM TITAN)";
            levels[1].enemyNumber = 10;
            levels[1].respawnMode = RespawnMode.DEATHMATCH;
            levels[1].supply = true;
            levels[1].teamTitan = true;
            levels[1].pvp = true;
            levels[1].GameMode = new KillTitansGamemode();

            levels[3].name = "The Forest";
            levels[3].mapName = "The Forest";
            levels[3].desc = "The Forest Of Giant Trees.(No RESPAWN/SUPPLY/PLAY AS TITAN)";
            levels[3].enemyNumber = 5;
            levels[3].respawnMode = RespawnMode.NEVER;
            levels[3].supply = true;
            levels[3].teamTitan = true;
            levels[3].pvp = true;
            levels[3].GameMode = new KillTitansGamemode();

            levels[4].name = "The Forest II";
            levels[4].mapName = "The Forest";
            levels[4].desc = "Survive for 20 waves.";
            levels[4].enemyNumber = 3;
            levels[4].respawnMode = RespawnMode.NEVER;
            levels[4].supply = true;
            levels[4].GameMode = new WaveGamemode();

            levels[5].name = "The Forest III";
            levels[5].mapName = "The Forest";
            levels[5].desc = "Survive for 20 waves.player will respawn in every new wave";
            levels[5].enemyNumber = 3;
            levels[5].respawnMode = RespawnMode.NEWROUND;
            levels[5].supply = true;
            levels[5].GameMode = new WaveGamemode();

            levels[6].name = "Annie";
            levels[6].mapName = "The Forest";
            levels[6].desc = "Nape Armor/ Ankle Armor:\nNormal:1000/50\nHard:2500/100\nAbnormal:4000/200\nYou only have 1 life.Don't do this alone.";
            levels[6].enemyNumber = 15;
            levels[6].respawnMode = RespawnMode.NEVER;
            levels[6].punk = false;
            levels[6].pvp = true;
            levels[6].GameMode = new KillTitansGamemode();

            levels[7].name = "Annie II";
            levels[7].mapName = "The Forest";
            levels[7].desc = "Nape Armor/ Ankle Armor:\nNormal:1000/50\nHard:3000/200\nAbnormal:6000/1000\n(RESPAWN AFTER 10 SECONDS)";
            levels[7].enemyNumber = 15;
            levels[7].respawnMode = RespawnMode.DEATHMATCH;
            levels[7].punk = false;
            levels[7].pvp = true;
            levels[7].GameMode = new KillTitansGamemode();

            levels[8].name = "Colossal Titan";
            levels[8].mapName = "Colossal Titan";
            levels[8].desc = "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\n Nape Armor:\n Normal:2000\nHard:3500\nAbnormal:5000\n";
            levels[8].enemyNumber = 2;
            levels[8].respawnMode = RespawnMode.NEVER;
            levels[8].GameMode = new TitanRushGamemode();

            levels[9].name = "Colossal Titan II";
            levels[9].mapName = "Colossal Titan";
            levels[9].desc = "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\n Nape Armor:\n Normal:5000\nHard:8000\nAbnormal:12000\n(RESPAWN AFTER 10 SECONDS)";
            levels[9].enemyNumber = 2;
            levels[9].respawnMode = RespawnMode.DEATHMATCH;
            levels[9].GameMode = new TitanRushGamemode();

            levels[10].name = "Trost";
            levels[10].mapName = "Colossal Titan";
            levels[10].desc = "Escort Titan Eren";
            levels[10].enemyNumber = 2;
            levels[10].respawnMode = RespawnMode.NEVER;
            levels[10].punk = false;
            levels[10].GameMode = new TrostGamemode();

            levels[11].name = "Trost II";
            levels[11].mapName = "Colossal Titan";
            levels[11].desc = "Escort Titan Eren(RESPAWN AFTER 10 SECONDS)";
            levels[11].enemyNumber = 2;
            levels[11].respawnMode = RespawnMode.DEATHMATCH;
            levels[11].punk = false;
            levels[11].GameMode = new TrostGamemode();

            levels[0x11].name = "The Forest IV  - LAVA";
            levels[0x11].mapName = "The Forest";
            levels[0x11].desc = "Survive for 20 waves.player will respawn in every new wave.\nNO CRAWLERS\n***YOU CAN'T TOUCH THE GROUND!***";
            levels[0x11].enemyNumber = 3;
            levels[0x11].respawnMode = RespawnMode.NEWROUND;
            levels[0x11].supply = true;
            levels[0x11].noCrawler = true;
            levels[0x11].lavaMode = true;
            levels[0x11].GameMode = new WaveGamemode();

            levels[0x12].name = "[S]Racing - Akina";
            levels[0x12].mapName = "track - akina";
            levels[0x12].desc = string.Empty;
            levels[0x12].enemyNumber = 0;
            levels[0x12].respawnMode = RespawnMode.NEVER;
            levels[0x12].supply = false;
            levels[0x12].GameMode = new RacingGamemode();

            levels[0x13].name = "Racing - Akina";
            levels[0x13].mapName = "track - akina";
            levels[0x13].desc = string.Empty;
            levels[0x13].enemyNumber = 0;
            levels[0x13].respawnMode = RespawnMode.NEVER;
            levels[0x13].supply = false;
            levels[0x13].pvp = true;
            levels[0x13].GameMode = new RacingGamemode();

            levels[20].name = "Outside The Walls";
            levels[20].mapName = "OutSide";
            levels[20].desc = "Capture Checkpoint mode.";
            levels[20].enemyNumber = 0;
            levels[20].respawnMode = RespawnMode.DEATHMATCH;
            levels[20].supply = true;
            levels[20].horse = true;
            levels[20].teamTitan = true;
            levels[20].GameMode = new CaptureGamemode();

            levels[0x15].name = "The City III";
            levels[0x15].mapName = "The City I";
            levels[0x15].desc = "Capture Checkpoint mode.";
            levels[0x15].enemyNumber = 0;
            levels[0x15].respawnMode = RespawnMode.DEATHMATCH;
            levels[0x15].supply = true;
            levels[0x15].horse = false;
            levels[0x15].teamTitan = true;
            levels[0x15].GameMode = new CaptureGamemode();

            levels[0x16].name = "Cave Fight";
            levels[0x16].mapName = "CaveFight";
            levels[0x16].desc = "***Spoiler Alarm!***";
            levels[0x16].enemyNumber = -1;
            levels[0x16].respawnMode = RespawnMode.NEVER;
            levels[0x16].supply = true;
            levels[0x16].horse = false;
            levels[0x16].teamTitan = true;
            levels[0x16].pvp = true;
            levels[0x16].GameMode = new PvPAhssGamemode();

            levels[0x17].name = "House Fight";
            levels[0x17].mapName = "HouseFight";
            levels[0x17].desc = "***Spoiler Alarm!***";
            levels[0x17].enemyNumber = -1;
            levels[0x17].respawnMode = RespawnMode.NEVER;
            levels[0x17].supply = true;
            levels[0x17].horse = false;
            levels[0x17].teamTitan = true;
            levels[0x17].pvp = true;
            levels[0x17].GameMode = new PvPAhssGamemode();

            levels[0x19].name = "Custom";
            levels[0x19].mapName = "The Forest";
            levels[0x19].desc = "Custom Map.";
            levels[0x19].enemyNumber = 1;
            levels[0x19].respawnMode = RespawnMode.NEVER;
            levels[0x19].supply = true;
            levels[0x19].teamTitan = true;
            levels[0x19].pvp = true;
            levels[0x19].punk = true;
            levels[0x19].GameMode = new KillTitansGamemode();

            levels[0x1a].name = "Custom (No PT)";
            levels[0x1a].mapName = "The Forest";
            levels[0x1a].desc = "Custom Map (No Player Titans).";
            levels[0x1a].enemyNumber = 1;
            levels[0x1a].respawnMode = RespawnMode.NEVER;
            levels[0x1a].pvp = true;
            levels[0x1a].punk = true;
            levels[0x1a].supply = true;
            levels[0x1a].teamTitan = false;
            levels[0x1a].GameMode = new KillTitansGamemode();

            levels[0].minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 731.9738f);
            levels[8].minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f);
            levels[9].minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f);
            levels[0x12].minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f);
            levels[0x13].minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f);
            levels[20].minimapPreset = new Minimap.Preset(new Vector3(2549.4f, 0f, 3042.4f), 3697.16f);
            levels[0x15].minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 734.9738f);
        }
    }
}

