using UnityEngine;

public class WaveGamemode : GamemodeBase
{
    public int Wave = 1;
    public int MaxWave = 20;
    public int HighestWave = 1;
    public int WaveIncrement = 2;

    public WaveGamemode()
    {
        GamemodeType = GamemodeType.Wave;
        TitanChaseDistanceEnabled = false;
        Titans = 3;
        RespawnMode = RespawnMode.NEWROUND;
    }

    public override string GetGamemodeStatusTop(int totalRoomTime = 0, int timeLeft = 0)
    {
        var content = "Titan Left: ";
        object[] objArray = new object[4];
        objArray[0] = content;
        var length = GameObject.FindGameObjectsWithTag("titan").Length;
        objArray[1] = length.ToString();
        objArray[2] = " Wave : ";
        objArray[3] = Wave;
        return string.Concat(objArray);
    }

    public override string GetGamemodeStatusTopRight(int time = 0, int totalRoomTime = 0)
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            var content = "Time : ";
            var length = totalRoomTime;
            return content + length.ToString();
        }
        return base.GetGamemodeStatusTopRight(time, totalRoomTime);
    }

    public override string GetVictoryMessage(float timeUntilRestart)
    {
        return $"Survive All Waves!\nGame Restart in {(int) timeUntilRestart}s\n\n";
    }

    public override void OnTitanKilled(string titanName, bool onPlayerLeave)
    {
        if (!IsAllTitansDead()) return;
        Wave++;
        var level = FengGameManagerMKII.level;
        if (!(((LevelInfo.getInfo(level).respawnMode != RespawnMode.NEWROUND) && (!level.StartsWith("Custom") || (RCSettings.gameType != 1))) || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)))
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 2)
                {
                    FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player);
                }
            }
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            //this.sendChatContentInfo("<color=#A8FF24>Wave : " + this.wave + "</color>");
        }
        if (Wave > HighestWave)
        {
            HighestWave = Wave;
        }
        if (PhotonNetwork.isMasterClient)
        {
            FengGameManagerMKII.instance.RequireStatus();
        }
        if (!(((RCSettings.maxWave != 0) || (Wave <= MaxWave)) && ((RCSettings.maxWave <= 0) || (Wave <= RCSettings.maxWave))))
        {
            FengGameManagerMKII.instance.gameWin2();
        }
        else
        {
            int abnormal = 90;
            if (FengGameManagerMKII.instance.difficulty == 1)
            {
                abnormal = 70;
            }
            if (!LevelInfo.getInfo(level).punk)
            {
                FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", abnormal, Wave + 2, false);
            }
            else if (Wave == 5)
            {
                FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", abnormal, 1, true);
            }
            else if (Wave == 10)
            {
                FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", abnormal, 2, true);
            }
            else if (Wave == 15)
            {
                FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", abnormal, 3, true);
            }
            else if (Wave == 20)
            {
                FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", abnormal, 4, true);
            }
            else
            {
                FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", abnormal, Wave + 2, false);
            }
        }
    }
}
