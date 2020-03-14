using UnityEngine;

public abstract class GamemodeBase
{
    public GamemodeType GamemodeType;

    //Titan Specific logic might be moved into a abstract Gamemode which implements an abstract TitanGamemode. Some gamemodes may not need titans, like Blades vs Blades pvp
    public int Titans = 25;
    public int TitanLimit = 25;

    public int HumanScore = 0;
    public int TitanScore = 0;

    public float RespawnTime = 5f;
    public bool AhssAirReload = true;
    public bool PlayerTitanShifters = true;

    public bool RestartOnTitansKilled = true;

    public virtual void OnPlayerKilled(int id)
    {
        if (FengGameManagerMKII.instance.isPlayerAllDead2())
        {
            FengGameManagerMKII.instance.gameLose2();
        }
    }

    public virtual GameObject GetPlayerSpawnLocation(string tag = "playerRespawn")
    {
        var objArray = GameObject.FindGameObjectsWithTag(tag);
        return objArray[Random.Range(0, objArray.Length)];
    }

    public virtual void OnTitanKilled(string titanName)
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

    public virtual GameObject SpawnNonAiTitan(Vector3 position, GameObject randomTitanRespawn)
    {
        return PhotonNetwork.Instantiate("TITAN_VER3.1", position, randomTitanRespawn.transform.rotation, 0);
    }

    private static bool IsAllTitansDead()
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
