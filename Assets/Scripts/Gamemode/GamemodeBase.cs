using UnityEngine;

public abstract class GamemodeBase
{
    public GamemodeType GamemodeType;

    //Titan Specific logic might be moved into a abstract Gamemode which implements an abstract TitanGamemode. Some gamemodes may not need titans, like Blades vs Blades pvp
    public int Titans = 25;
    public int TitanLimit = 100;

    public float RespawnTime = 5f;
    public bool AhssAirReload = true;

    public bool RestartOnTitansKilled = true;

    public virtual void OnPlayerKilled(int id)
    {
        if (FengGameManagerMKII.instance.isPlayerAllDead2())
        {
            FengGameManagerMKII.instance.gameLose2();
        }
    }

    public virtual void OnTitanKilled(string titanName)
    {
        if (RestartOnTitansKilled && IsAllTitansDead())
        {
            OnAllTitansDead();
        }
    }

    public virtual void OnAllTitansDead() { }

    public virtual void OnLevelWasLoaded(LevelInfo info) { }

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
