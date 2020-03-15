using UnityEngine;

public class KillTitansGamemode : GamemodeBase
{
    public KillTitansGamemode()
    {
        GamemodeType = GamemodeType.Titans;
        RestartOnTitansKilled = true;
    }

    public override void OnAllTitansDead()
    {
        FengGameManagerMKII.instance.gameWin2();
        Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }

    public override void OnLevelWasLoaded(LevelInfo info, bool isMasterClient = false)
    {
        if (!isMasterClient) return;

        if ((info.name == "Annie") || (info.name == "Annie II"))
        {
            PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
        }
        else
        {
            int num4 = 90;
            if (FengGameManagerMKII.instance.difficulty == 1)
            {
                num4 = 70;
            }
            FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", num4, info.enemyNumber + 20, false);
        }

    }
}