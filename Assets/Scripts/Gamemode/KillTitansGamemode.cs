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

    public override void OnPlayerKilled(int id)
    {
        base.OnPlayerKilled(id);
        // If we wanted to, KillTitansGamemode could add additional logic besides the default logic defined in our base Gamemode class, or completely override it.
    }

    public override void OnLevelWasLoaded(LevelInfo info)
    {
        if (!PhotonNetwork.isMasterClient) return;

        PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
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