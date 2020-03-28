using UnityEngine;

public class CaptureGamemode : GamemodeBase
{
    public CaptureGamemode()
    {
        GamemodeType = GamemodeType.Capture;
        RespawnTime = 20f;
        PlayerTitanShifters = false;
        Titans = 0;
        TitanLimit = 25;
        TitanChaseDistance = 120f;
        SpawnTitansOnFemaleTitanDefeat = false;
        FemaleTitanDespawnTimer = 20f;
        FemaleTitanHealthModifier = 0.8f;
    }

    public int PvpTitanScoreLimit = 200;
    public int PvpHumanScoreLimit = 200;

    public int PvpTitanScore;
    public int PvpHumanScore;

    public bool SpawnSupplyStationOnHumanCapture;

    private const string HumanStart = "CheckpointStartHuman";
    private const string TitanStart = "CheckpointStartTitan";

    public override string GetGamemodeStatusTop(int time = 0, int totalRoomTime = 0)
    {
        string str2 = "| ";
        for (int i = 0; i < PVPcheckPoint.chkPts.Count; i++)
        {
            str2 = str2 + (PVPcheckPoint.chkPts[i] as PVPcheckPoint).getStateString() + " ";
        }
        str2 = str2 + "|";
        var length = totalRoomTime - time;
        return $"{PvpTitanScoreLimit - PvpTitanScore} {str2} {PvpHumanScoreLimit - PvpHumanScore} \nTime : {length}";
    }

    public override void OnTitanKilled(string titanName, bool onPlayerLeave)
    {
        if (titanName != string.Empty)
        {
            switch (titanName)
            {
                case "Titan":
                    PvpHumanScore++;
                    break;
                case "Aberrant":
                    PvpHumanScore += 2;
                    break;
                case "Jumper":
                    PvpHumanScore += 3;
                    break;
                case "Crawler":
                    PvpHumanScore += 4;
                    break;
                case "Female Titan":
                    PvpHumanScore += 10;
                    break;
                default:
                    PvpHumanScore += 3;
                    break;
            }
        }
        CheckWinConditions();
        object[] parameters = { PvpHumanScore, PvpTitanScore };
        FengGameManagerMKII.instance.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
    }

    private void CheckWinConditions()
    {
        if (PvpTitanScore >= PvpTitanScoreLimit)
        {
            PvpTitanScore = PvpTitanScoreLimit;
            FengGameManagerMKII.instance.gameLose2();
        }
        else if (PvpHumanScore >= PvpHumanScoreLimit)
        {
            PvpHumanScore = PvpHumanScoreLimit;
            FengGameManagerMKII.instance.gameWin2();
        }
    }

    public override void OnLevelWasLoaded(LevelInfo info, bool isMasterClient = false)
    {
        if (!FengGameManagerMKII.instance.needChooseSide && (int) FengGameManagerMKII.settings[0xf5] == 0)
        {
            if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
            {
                FengGameManagerMKII.instance.checkpoint = GameObject.Find(TitanStart);
            }
            else
            {
                FengGameManagerMKII.instance.checkpoint = GameObject.Find(HumanStart);
            }
        }

        if (isMasterClient && FengGameManagerMKII.Level.SceneName == "OutSide")
        {
            GameObject[] objArray3 = GameObject.FindGameObjectsWithTag("titanRespawn");
            if (objArray3.Length <= 0)
            {
                return;
            }
            for (int i = 0; i < objArray3.Length; i++)
            {
                spawnTitanRaw(objArray3[i].transform.position, objArray3[i].transform.rotation).GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_CRAWLER, true);
            }
        }
    }

    public override GameObject SpawnNonAiTitan(Vector3 position, GameObject randomTitanRespawn)
    {
        return PhotonNetwork.Instantiate("TITAN_VER3.1", FengGameManagerMKII.instance.checkpoint.transform.position + new Vector3(Random.Range(-20, 20), 2f, Random.Range(-20, 20)), FengGameManagerMKII.instance.checkpoint.transform.rotation, 0);
    }

    public override GameObject GetPlayerSpawnLocation(string tag = "playerRespawn")
    {
        if (FengGameManagerMKII.instance.checkpoint == null)
        {
            FengGameManagerMKII.instance.checkpoint = GameObject.Find("CheckpointStartHuman");
        }
        return FengGameManagerMKII.instance.checkpoint;
    }

    public override void OnPlayerSpawned(GameObject player)
    {
        var transform = player.transform;
        transform.position += new Vector3(Random.Range(-20, 20), 2f, Random.Range(-20, 20));
    }

    private GameObject spawnTitanRaw(Vector3 position, Quaternion rotation)
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            return (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_VER3.1"), position, rotation);
        }
        return PhotonNetwork.Instantiate("TITAN_VER3.1", position, rotation, 0);
    }

    public override void OnPlayerKilled(int id)
    {
        if (id != 0)
        {
            PvpTitanScore += 2;
        }
        CheckWinConditions();
        object[] parameters = { PvpHumanScore, PvpTitanScore };
        FengGameManagerMKII.instance.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
    }
}
