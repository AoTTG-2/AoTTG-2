using UnityEngine;

public abstract class GamemodeBase
{
    public GamemodeType GamemodeType;

    //Titan Specific logic might be moved into a abstract Gamemode which implements an abstract TitanGamemode. Some gamemodes may not need titans, like Blades vs Blades pvp
    public int Titans = 25;
    public int TitanLimit = 25;
    public float TitanChaseDistance = 100f;
    public bool TitanChaseDistanceEnabled = true;

    public bool SpawnTitansOnFemaleTitanDefeat = true;
    public float FemaleTitanDespawnTimer = 5f;
    public float FemaleTitanHealthModifier = 1f;

    public int HumanScore = 0;
    public int TitanScore = 0;

    public float RespawnTime = 5f;
    public bool AhssAirReload = true;
    public bool PlayerTitanShifters = true;

    public bool RestartOnTitansKilled = true;

    public virtual void OnPlayerKilled(int id)
    {
        if (IsAllPlayersDead())
        {
            FengGameManagerMKII.instance.gameLose2();
        }
    }

    public virtual void OnPlayerSpawned(GameObject player)
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) return;

    }

    public virtual GameObject GetPlayerSpawnLocation(string tag = "playerRespawn")
    {
        var objArray = GameObject.FindGameObjectsWithTag(tag);
        return objArray[Random.Range(0, objArray.Length)];
    }

    public virtual string GetVictoryMessage(float timeUntilRestart)
    {
        return "Humanity Win!\nGame Restart in " + ((int)timeUntilRestart) + "s\n\n";
    }

    public virtual void OnTitanKilled(string titanName, bool onPlayerLeave)
    {
        if (RestartOnTitansKilled && IsAllTitansDead())
        {
            OnAllTitansDead();
        }
    }

    public virtual string GetGamemodeStatusTop(int time = 0, int totalRoomTime = 0)
    {
        var content = "Titan Left: ";
        var length = GameObject.FindGameObjectsWithTag("titan").Length;
        content = content + length + "  Time : ";
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            length = time;
            content += length.ToString();
        }
        else
        {
            length = totalRoomTime - (time);
            content += length.ToString();
        }

        return content;
    }

    public virtual string GetGamemodeStatusTopRight(int time = 0, int totalRoomTime = 0)
    {
        return string.Concat("Humanity ", HumanScore, " : Titan ", TitanScore, " ");

    }

    public virtual void OnAllTitansDead() { }

    public virtual void OnLevelWasLoaded(LevelInfo info) { }

    public virtual void OnGameWon()
    {
        FengGameManagerMKII.instance.gameEndCD = FengGameManagerMKII.instance.gameEndTotalCDtime;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            var parameters = new object[] { HumanScore };
            FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
            if (((int)FengGameManagerMKII.settings[0xf4]) == 1)
            {
                //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
            }
        }
    }

    public virtual void OnNetGameWon(int score)
    {
        HumanScore = score;
        FengGameManagerMKII.instance.gameEndCD = FengGameManagerMKII.instance.gameEndTotalCDtime;
    }

    public virtual GameObject SpawnNonAiTitan(Vector3 position, GameObject randomTitanRespawn)
    {
        return PhotonNetwork.Instantiate("TITAN_VER3.1", position, randomTitanRespawn.transform.rotation, 0);
    }

    internal bool IsAllPlayersDead()
    {
        int num = 0;
        int num2 = 0;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 1)
            {
                num++;
                if (RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]))
                {
                    num2++;
                }
            }
        }
        return (num == num2);
    }

    internal bool IsAllTitansDead()
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if ((obj2.GetComponent<TITAN>() != null) && !obj2.GetComponent<TITAN>().hasDie)
            {
                return false;
            }
            if (obj2.GetComponent<FEMALE_TITAN>() != null)
            {
                return false;
            }
        }
        return true;
    }
}
