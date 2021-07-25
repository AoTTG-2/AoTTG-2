using System;
using UnityEngine;

[Obsolete("Appears to be unused")]
public class PlayerInfoPHOTON
{
    public int airKills;
    public int assistancePt;
    public bool dead;
    public int die;
    public string guildname = string.Empty;
    public string id;
    public int kills;
    public int maxDamage;
    public string name = "Guest";
    public PhotonPlayer networkplayer;
    public string resourceId = "not choose";
    public bool SET;
    public int totalCrawlerKills;
    public int totalDamage;
    public int totalDeaths;
    public int totalJumperKills;
    public int totalKills;
    public int totalKillsInOneLifeAB;
    public int totalKillsInOneLifeHard;
    public int totalKillsInOneLifeNormal;
    public int totalNonAIKills;

    public void initAsGuest()
    {
        this.name = "GUEST" + UnityEngine.Random.Range(0, 0x186a0);
        this.kills = 0;
        this.die = 0;
        this.maxDamage = 0;
        this.totalDamage = 0;
        this.assistancePt = 0;
        this.dead = false;
        this.resourceId = "not choose";
        this.SET = false;
        this.totalKills = 0;
        this.totalDeaths = 0;
        this.totalKillsInOneLifeNormal = 0;
        this.totalKillsInOneLifeHard = 0;
        this.totalKillsInOneLifeAB = 0;
        this.airKills = 0;
        this.totalCrawlerKills = 0;
        this.totalJumperKills = 0;
        this.totalNonAIKills = 0;
    }
}

