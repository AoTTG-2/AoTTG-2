using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class RCAction
{
    private int actionClass;
    private int actionType;
    private RCEvent nextEvent;
    private RCActionHelper[] parameters;

    public RCAction(int category, int type, RCEvent next, RCActionHelper[] helpers)
    {
        this.actionClass = category;
        this.actionType = type;
        this.nextEvent = next;
        this.parameters = helpers;
    }

    public void callException(string str)
    {
        FengGameManagerMKII.instance.chatRoom.addLINE(str);
    }

    public void doAction()
    {
        switch (this.actionClass)
        {
            case 0:
                this.nextEvent.checkEvent();
                break;

            case 1:
            {
                string key = this.parameters[0].returnString(null);
                int num2 = this.parameters[1].returnInt(null);
                switch (this.actionType)
                {
                    case 0:
                        if (!FengGameManagerMKII.intVariables.ContainsKey(key))
                        {
                            FengGameManagerMKII.intVariables.Add(key, num2);
                        }
                        else
                        {
                            FengGameManagerMKII.intVariables[key] = num2;
                        }
                        return;

                    case 1:
                        if (!FengGameManagerMKII.intVariables.ContainsKey(key))
                        {
                            this.callException("Variable not found: " + key);
                        }
                        else
                        {
                            FengGameManagerMKII.intVariables[key] = ((int) FengGameManagerMKII.intVariables[key]) + num2;
                        }
                        return;

                    case 2:
                        if (!FengGameManagerMKII.intVariables.ContainsKey(key))
                        {
                            this.callException("Variable not found: " + key);
                        }
                        else
                        {
                            FengGameManagerMKII.intVariables[key] = ((int) FengGameManagerMKII.intVariables[key]) - num2;
                        }
                        return;

                    case 3:
                        if (!FengGameManagerMKII.intVariables.ContainsKey(key))
                        {
                            this.callException("Variable not found: " + key);
                        }
                        else
                        {
                            FengGameManagerMKII.intVariables[key] = ((int) FengGameManagerMKII.intVariables[key]) * num2;
                        }
                        return;

                    case 4:
                        if (!FengGameManagerMKII.intVariables.ContainsKey(key))
                        {
                            this.callException("Variable not found: " + key);
                        }
                        else
                        {
                            FengGameManagerMKII.intVariables[key] = ((int) FengGameManagerMKII.intVariables[key]) / num2;
                        }
                        return;

                    case 5:
                        if (!FengGameManagerMKII.intVariables.ContainsKey(key))
                        {
                            this.callException("Variable not found: " + key);
                        }
                        else
                        {
                            FengGameManagerMKII.intVariables[key] = ((int) FengGameManagerMKII.intVariables[key]) % num2;
                        }
                        return;

                    case 6:
                        if (!FengGameManagerMKII.intVariables.ContainsKey(key))
                        {
                            this.callException("Variable not found: " + key);
                        }
                        else
                        {
                            FengGameManagerMKII.intVariables[key] = (int) Math.Pow((double) ((int) FengGameManagerMKII.intVariables[key]), (double) num2);
                        }
                        return;

                    case 12:
                        if (!FengGameManagerMKII.intVariables.ContainsKey(key))
                        {
                            FengGameManagerMKII.intVariables.Add(key, UnityEngine.Random.Range(num2, this.parameters[2].returnInt(null)));
                        }
                        else
                        {
                            FengGameManagerMKII.intVariables[key] = UnityEngine.Random.Range(num2, this.parameters[2].returnInt(null));
                        }
                        return;
                }
                break;
            }
            case 2:
            {
                string str2 = this.parameters[0].returnString(null);
                bool flag2 = this.parameters[1].returnBool(null);
                switch (this.actionType)
                {
                    case 11:
                        if (!FengGameManagerMKII.boolVariables.ContainsKey(str2))
                        {
                            this.callException("Variable not found: " + str2);
                        }
                        else
                        {
                            FengGameManagerMKII.boolVariables[str2] = !((bool) FengGameManagerMKII.boolVariables[str2]);
                        }
                        return;

                    case 12:
                        if (!FengGameManagerMKII.boolVariables.ContainsKey(str2))
                        {
                            FengGameManagerMKII.boolVariables.Add(str2, Convert.ToBoolean(UnityEngine.Random.Range(0, 2)));
                        }
                        else
                        {
                            FengGameManagerMKII.boolVariables[str2] = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                        }
                        return;

                    case 0:
                        if (!FengGameManagerMKII.boolVariables.ContainsKey(str2))
                        {
                            FengGameManagerMKII.boolVariables.Add(str2, flag2);
                        }
                        else
                        {
                            FengGameManagerMKII.boolVariables[str2] = flag2;
                        }
                        return;
                }
                break;
            }
            case 3:
            {
                string str3 = this.parameters[0].returnString(null);
                switch (this.actionType)
                {
                    case 7:
                    {
                        string str5 = string.Empty;
                        for (int i = 1; i < this.parameters.Length; i++)
                        {
                            str5 = str5 + this.parameters[i].returnString(null);
                        }
                        if (!FengGameManagerMKII.stringVariables.ContainsKey(str3))
                        {
                            FengGameManagerMKII.stringVariables.Add(str3, str5);
                        }
                        else
                        {
                            FengGameManagerMKII.stringVariables[str3] = str5;
                        }
                        return;
                    }
                    case 8:
                    {
                        string str6 = this.parameters[1].returnString(null);
                        if (!FengGameManagerMKII.stringVariables.ContainsKey(str3))
                        {
                            this.callException("No Variable");
                        }
                        else
                        {
                            FengGameManagerMKII.stringVariables[str3] = ((string) FengGameManagerMKII.stringVariables[str3]) + str6;
                        }
                        return;
                    }
                    case 9:
                        this.parameters[1].returnString(null);
                        if (!FengGameManagerMKII.stringVariables.ContainsKey(str3))
                        {
                            this.callException("No Variable");
                        }
                        else
                        {
                            FengGameManagerMKII.stringVariables[str3] = ((string) FengGameManagerMKII.stringVariables[str3]).Replace(this.parameters[1].returnString(null), this.parameters[2].returnString(null));
                        }
                        return;

                    case 0:
                    {
                        string str4 = this.parameters[1].returnString(null);
                        if (!FengGameManagerMKII.stringVariables.ContainsKey(str3))
                        {
                            FengGameManagerMKII.stringVariables.Add(str3, str4);
                        }
                        else
                        {
                            FengGameManagerMKII.stringVariables[str3] = str4;
                        }
                        return;
                    }
                }
                break;
            }
            case 4:
            {
                string str9 = this.parameters[0].returnString(null);
                float num4 = this.parameters[1].returnFloat(null);
                switch (this.actionType)
                {
                    case 0:
                        if (!FengGameManagerMKII.floatVariables.ContainsKey(str9))
                        {
                            FengGameManagerMKII.floatVariables.Add(str9, num4);
                        }
                        else
                        {
                            FengGameManagerMKII.floatVariables[str9] = num4;
                        }
                        return;

                    case 1:
                        if (!FengGameManagerMKII.floatVariables.ContainsKey(str9))
                        {
                            this.callException("No Variable");
                        }
                        else
                        {
                            FengGameManagerMKII.floatVariables[str9] = ((float) FengGameManagerMKII.floatVariables[str9]) + num4;
                        }
                        return;

                    case 2:
                        if (!FengGameManagerMKII.floatVariables.ContainsKey(str9))
                        {
                            this.callException("No Variable");
                        }
                        else
                        {
                            FengGameManagerMKII.floatVariables[str9] = ((float) FengGameManagerMKII.floatVariables[str9]) - num4;
                        }
                        return;

                    case 3:
                        if (!FengGameManagerMKII.floatVariables.ContainsKey(str9))
                        {
                            this.callException("No Variable");
                        }
                        else
                        {
                            FengGameManagerMKII.floatVariables[str9] = ((float) FengGameManagerMKII.floatVariables[str9]) * num4;
                        }
                        return;

                    case 4:
                        if (!FengGameManagerMKII.floatVariables.ContainsKey(str9))
                        {
                            this.callException("No Variable");
                        }
                        else
                        {
                            FengGameManagerMKII.floatVariables[str9] = ((float) FengGameManagerMKII.floatVariables[str9]) / num4;
                        }
                        return;

                    case 5:
                        if (!FengGameManagerMKII.floatVariables.ContainsKey(str9))
                        {
                            this.callException("No Variable");
                        }
                        else
                        {
                            FengGameManagerMKII.floatVariables[str9] = ((float) FengGameManagerMKII.floatVariables[str9]) % num4;
                        }
                        return;

                    case 6:
                        if (!FengGameManagerMKII.floatVariables.ContainsKey(str9))
                        {
                            this.callException("No Variable");
                        }
                        else
                        {
                            FengGameManagerMKII.floatVariables[str9] = (float) Math.Pow((double) ((int) FengGameManagerMKII.floatVariables[str9]), (double) num4);
                        }
                        return;

                    case 12:
                        if (!FengGameManagerMKII.floatVariables.ContainsKey(str9))
                        {
                            FengGameManagerMKII.floatVariables.Add(str9, UnityEngine.Random.Range(num4, this.parameters[2].returnFloat(null)));
                        }
                        else
                        {
                            FengGameManagerMKII.floatVariables[str9] = UnityEngine.Random.Range(num4, this.parameters[2].returnFloat(null));
                        }
                        return;
                }
                break;
            }
            case 5:
            {
                string str10 = this.parameters[0].returnString(null);
                PhotonPlayer player = this.parameters[1].returnPlayer(null);
                if (this.actionType == 0)
                {
                    if (!FengGameManagerMKII.playerVariables.ContainsKey(str10))
                    {
                        FengGameManagerMKII.playerVariables.Add(str10, player);
                    }
                    else
                    {
                        FengGameManagerMKII.playerVariables[str10] = player;
                    }
                    break;
                }
                break;
            }
            case 6:
            {
                string str11 = this.parameters[0].returnString(null);
                TITAN titan = this.parameters[1].returnTitan(null);
                if (this.actionType == 0)
                {
                    if (!FengGameManagerMKII.titanVariables.ContainsKey(str11))
                    {
                        FengGameManagerMKII.titanVariables.Add(str11, titan);
                    }
                    else
                    {
                        FengGameManagerMKII.titanVariables[str11] = titan;
                    }
                    break;
                }
                break;
            }
            case 7:
            {
                PhotonPlayer targetPlayer = this.parameters[0].returnPlayer(null);
                switch (this.actionType)
                {
                    case 0:
                    {
                        int iD = targetPlayer.ID;
                        if (FengGameManagerMKII.heroHash.ContainsKey(iD))
                        {
                            HERO hero = (HERO) FengGameManagerMKII.heroHash[iD];
                            hero.markDie();
                            hero.photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, this.parameters[1].returnString(null) + " " });
                        }
                        else
                        {
                            this.callException("Player Not Alive");
                        }
                        return;
                    }
                    case 1:
                        FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", targetPlayer, new object[0]);
                        return;

                    case 2:
                        FengGameManagerMKII.instance.photonView.RPC("spawnPlayerAtRPC", targetPlayer, new object[] { this.parameters[1].returnFloat(null), this.parameters[2].returnFloat(null), this.parameters[3].returnFloat(null) });
                        return;

                    case 3:
                    {
                        int num6 = targetPlayer.ID;
                        if (FengGameManagerMKII.heroHash.ContainsKey(num6))
                        {
                            HERO hero2 = (HERO) FengGameManagerMKII.heroHash[num6];
                            hero2.photonView.RPC("moveToRPC", targetPlayer, new object[] { this.parameters[1].returnFloat(null), this.parameters[2].returnFloat(null), this.parameters[3].returnFloat(null) });
                        }
                        else
                        {
                            this.callException("Player Not Alive");
                        }
                        return;
                    }
                    case 4:
                    {
                        Hashtable propertiesToSet = new Hashtable();
                        propertiesToSet.Add(PhotonPlayerProperty.kills, this.parameters[1].returnInt(null));
                        targetPlayer.SetCustomProperties(propertiesToSet);
                        return;
                    }
                    case 5:
                    {
                        Hashtable hashtable2 = new Hashtable();
                        hashtable2.Add(PhotonPlayerProperty.deaths, this.parameters[1].returnInt(null));
                        targetPlayer.SetCustomProperties(hashtable2);
                        return;
                    }
                    case 6:
                    {
                        Hashtable hashtable3 = new Hashtable();
                        hashtable3.Add(PhotonPlayerProperty.max_dmg, this.parameters[1].returnInt(null));
                        targetPlayer.SetCustomProperties(hashtable3);
                        return;
                    }
                    case 7:
                    {
                        Hashtable hashtable4 = new Hashtable();
                        hashtable4.Add(PhotonPlayerProperty.total_dmg, this.parameters[1].returnInt(null));
                        targetPlayer.SetCustomProperties(hashtable4);
                        return;
                    }
                    case 8:
                    {
                        Hashtable hashtable5 = new Hashtable();
                        hashtable5.Add(PhotonPlayerProperty.name, this.parameters[1].returnString(null));
                        targetPlayer.SetCustomProperties(hashtable5);
                        return;
                    }
                    case 9:
                    {
                        Hashtable hashtable6 = new Hashtable();
                        hashtable6.Add(PhotonPlayerProperty.guildName, this.parameters[1].returnString(null));
                        targetPlayer.SetCustomProperties(hashtable6);
                        return;
                    }
                    case 10:
                    {
                        Hashtable hashtable7 = new Hashtable();
                        hashtable7.Add(PhotonPlayerProperty.RCteam, this.parameters[1].returnInt(null));
                        targetPlayer.SetCustomProperties(hashtable7);
                        return;
                    }
                    case 11:
                    {
                        Hashtable hashtable8 = new Hashtable();
                        hashtable8.Add(PhotonPlayerProperty.customInt, this.parameters[1].returnInt(null));
                        targetPlayer.SetCustomProperties(hashtable8);
                        return;
                    }
                    case 12:
                    {
                        Hashtable hashtable9 = new Hashtable();
                        hashtable9.Add(PhotonPlayerProperty.customBool, this.parameters[1].returnBool(null));
                        targetPlayer.SetCustomProperties(hashtable9);
                        return;
                    }
                    case 13:
                    {
                        Hashtable hashtable10 = new Hashtable();
                        hashtable10.Add(PhotonPlayerProperty.customString, this.parameters[1].returnString(null));
                        targetPlayer.SetCustomProperties(hashtable10);
                        return;
                    }
                    case 14:
                    {
                        Hashtable hashtable11 = new Hashtable();
                        hashtable11.Add(PhotonPlayerProperty.RCteam, this.parameters[1].returnFloat(null));
                        targetPlayer.SetCustomProperties(hashtable11);
                        return;
                    }
                }
                break;
            }
            case 8:
                switch (this.actionType)
                {
                    case 0:
                    {
                        TITAN titan2 = this.parameters[0].returnTitan(null);
                        object[] parameters = new object[] { this.parameters[1].returnPlayer(null).ID, this.parameters[2].returnInt(null) };
                        titan2.photonView.RPC("titanGetHit", titan2.photonView.owner, parameters);
                        return;
                    }
                    case 1:
                        FengGameManagerMKII.instance.spawnTitanAction(this.parameters[0].returnInt(null), this.parameters[1].returnFloat(null), this.parameters[2].returnInt(null), this.parameters[3].returnInt(null));
                        return;

                    case 2:
                        FengGameManagerMKII.instance.spawnTitanAtAction(this.parameters[0].returnInt(null), this.parameters[1].returnFloat(null), this.parameters[2].returnInt(null), this.parameters[3].returnInt(null), this.parameters[4].returnFloat(null), this.parameters[5].returnFloat(null), this.parameters[6].returnFloat(null));
                        return;

                    case 3:
                    {
                        TITAN titan3 = this.parameters[0].returnTitan(null);
                        int num7 = this.parameters[1].returnInt(null);
                        titan3.currentHealth = num7;
                        if (titan3.maxHealth == 0)
                        {
                            titan3.maxHealth = titan3.currentHealth;
                        }
                        titan3.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, new object[] { titan3.currentHealth, titan3.maxHealth });
                        return;
                    }
                    case 4:
                    {
                        TITAN titan4 = this.parameters[0].returnTitan(null);
                        if (titan4.photonView.isMine)
                        {
                            titan4.moveTo(this.parameters[1].returnFloat(null), this.parameters[2].returnFloat(null), this.parameters[3].returnFloat(null));
                        }
                        else
                        {
                            titan4.photonView.RPC("moveToRPC", titan4.photonView.owner, new object[] { this.parameters[1].returnFloat(null), this.parameters[2].returnFloat(null), this.parameters[3].returnFloat(null) });
                        }
                        return;
                    }
                }
                break;

            case 9:
                switch (this.actionType)
                {
                    case 0:
                        FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, new object[] { this.parameters[0].returnString(null), string.Empty });
                        return;

                    case 1:
                        FengGameManagerMKII.instance.gameWin2();
                        if (this.parameters[0].returnBool(null))
                        {
                            FengGameManagerMKII.intVariables.Clear();
                            FengGameManagerMKII.boolVariables.Clear();
                            FengGameManagerMKII.stringVariables.Clear();
                            FengGameManagerMKII.floatVariables.Clear();
                            FengGameManagerMKII.playerVariables.Clear();
                            FengGameManagerMKII.titanVariables.Clear();
                        }
                        return;

                    case 2:
                        FengGameManagerMKII.instance.gameLose2();
                        if (this.parameters[0].returnBool(null))
                        {
                            FengGameManagerMKII.intVariables.Clear();
                            FengGameManagerMKII.boolVariables.Clear();
                            FengGameManagerMKII.stringVariables.Clear();
                            FengGameManagerMKII.floatVariables.Clear();
                            FengGameManagerMKII.playerVariables.Clear();
                            FengGameManagerMKII.titanVariables.Clear();
                        }
                        return;

                    case 3:
                        if (this.parameters[0].returnBool(null))
                        {
                            FengGameManagerMKII.intVariables.Clear();
                            FengGameManagerMKII.boolVariables.Clear();
                            FengGameManagerMKII.stringVariables.Clear();
                            FengGameManagerMKII.floatVariables.Clear();
                            FengGameManagerMKII.playerVariables.Clear();
                            FengGameManagerMKII.titanVariables.Clear();
                        }
                        FengGameManagerMKII.instance.restartGame2(false);
                        return;
                }
                break;
        }
    }

    public enum actionClasses
    {
        typeVoid,
        typeVariableInt,
        typeVariableBool,
        typeVariableString,
        typeVariableFloat,
        typeVariablePlayer,
        typeVariableTitan,
        typePlayer,
        typeTitan,
        typeGame
    }

    public enum gameTypes
    {
        printMessage,
        winGame,
        loseGame,
        restartGame
    }

    public enum playerTypes
    {
        killPlayer,
        spawnPlayer,
        spawnPlayerAt,
        movePlayer,
        setKills,
        setDeaths,
        setMaxDmg,
        setTotalDmg,
        setName,
        setGuildName,
        setTeam,
        setCustomInt,
        setCustomBool,
        setCustomString,
        setCustomFloat
    }

    public enum titanTypes
    {
        killTitan,
        spawnTitan,
        spawnTitanAt,
        setHealth,
        moveTitan
    }

    public enum varTypes
    {
        set,
        add,
        subtract,
        multiply,
        divide,
        modulo,
        power,
        concat,
        append,
        remove,
        replace,
        toOpposite,
        setRandom
    }
}

