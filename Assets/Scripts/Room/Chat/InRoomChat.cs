using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using Assets.Scripts.Gamemode.Options;
using UnityEngine;
using UnityEngine.UI;

public class InRoomChat : Photon.MonoBehaviour
{
    private bool AlignBottom = true;
    public static readonly string ChatRPC = "Chat";
    private string inputLine = string.Empty;
    public bool IsVisible = true;
    public static List<string> messages = new List<string>();
    private Vector2 scrollPos = Vector2.zero;
    public InputField ChatInputField;
    public Text ChatText;

    public void addLINE(string newLine)
    {
        messages.Add(newLine);
    }

    public void AddLine(string newLine)
    {
        messages.Add(newLine);
    }

    public void OnGUI()
    {
        int num4;
        if (!this.IsVisible || (PhotonNetwork.connectionState != ConnectionState.Connected))
        {
            return;
        }
        if (Event.current.type == EventType.KeyDown)
        {
            if ((((Event.current.keyCode != KeyCode.Tab) && (Event.current.character != '\t')) || IN_GAME_MAIN_CAMERA.isPausing) || (FengGameManagerMKII.inputRC.humanKeys[InputCodeRC.chat] == KeyCode.Tab))
            {
                goto Label_00E1;
            }
            Event.current.Use();
            goto Label_013D;
        }
        if ((Event.current.type == EventType.KeyUp) && (((Event.current.keyCode != KeyCode.None) && (Event.current.keyCode == FengGameManagerMKII.inputRC.humanKeys[InputCodeRC.chat])) && (GUI.GetNameOfFocusedControl() != "ChatInput")))
        {
            this.inputLine = string.Empty;
            ChatInputField.gameObject.GetComponent<Text>().text = string.Empty;
            goto Label_013D;
        }
    Label_00E1:
        if ((Event.current.type == EventType.KeyDown) && ((Event.current.keyCode == KeyCode.KeypadEnter) || (Event.current.keyCode == KeyCode.Return)))
        {
            if (!string.IsNullOrEmpty(this.inputLine))
            {
                string str2;
                if (this.inputLine == "\t")
                {
                    this.inputLine = string.Empty;
                    ChatInputField.gameObject.GetComponent<Text>().text = string.Empty;
                    return;
                }
                if (FengGameManagerMKII.RCEvents.ContainsKey("OnChatInput"))
                {
                    string key = (string)FengGameManagerMKII.RCVariableNames["OnChatInput"];
                    if (FengGameManagerMKII.stringVariables.ContainsKey(key))
                    {
                        FengGameManagerMKII.stringVariables[key] = this.inputLine;
                    }
                    else
                    {
                        FengGameManagerMKII.stringVariables.Add(key, this.inputLine);
                    }
                    ((RCEvent)FengGameManagerMKII.RCEvents["OnChatInput"]).checkEvent();
                }
                if (!this.inputLine.StartsWith("/"))
                {
                    str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]).hexColor();
                    if (str2 == string.Empty)
                    {
                        str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]);
                        if (PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam] != null)
                        {
                            if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 1)
                            {
                                str2 = "<color=#00FFFF>" + str2 + "</color>";
                            }
                            else if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 2)
                            {
                                str2 = "<color=#FF00FF>" + str2 + "</color>";
                            }
                        }
                    }
                    object[] parameters = new object[] { this.inputLine, str2 };
                    FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
                }
                else if (this.inputLine == "/cloth")
                {
                    this.addLINE(ClothFactory.GetDebugInfo());
                }
                else if (this.inputLine.StartsWith("/aso"))
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        switch (this.inputLine.Substring(5))
                        {
                            case "kdr":
                                if (!FengGameManagerMKII.Gamemode.SaveKDROnDisconnect)
                                {
                                    FengGameManagerMKII.Gamemode.SaveKDROnDisconnect = true;
                                    this.addLINE("<color=#FFCC00>KDRs will be preserved from disconnects.</color>");
                                }
                                else
                                {
                                    FengGameManagerMKII.Gamemode.SaveKDROnDisconnect = false;
                                    this.addLINE("<color=#FFCC00>KDRs will not be preserved from disconnects.</color>");
                                }
                                break;
                        }
                    }
                }
                else
                {
                    object[] objArray3;
                    if (this.inputLine == "/pause")
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { true });
                            objArray3 = new object[] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
                            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: not master client</color>");
                        }
                    }
                    else if (this.inputLine == "/unpause")
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { false });
                            objArray3 = new object[] { "<color=#FFCC00>MasterClient has unpaused the game.</color>", "" };
                            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: not master client</color>");
                        }
                    }
                    else if (this.inputLine == "/checklevel")
                    {
                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                        {
                            this.addLINE(RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.currentLevel]));
                        }
                    }
                    else if (this.inputLine == "/isrc")
                    {
                        if (FengGameManagerMKII.masterRC)
                        {
                            this.addLINE("is RC");
                        }
                        else
                        {
                            this.addLINE("not RC");
                        }
                    }
                    else if (this.inputLine == "/ignorelist")
                    {
                        foreach (int num2 in FengGameManagerMKII.ignoreList)
                        {
                            this.addLINE(num2.ToString());
                        }
                    }
                    else if (this.inputLine.StartsWith("/room"))
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            if (this.inputLine.Substring(6).StartsWith("max"))
                            {
                                int num3 = Convert.ToInt32(this.inputLine.Substring(10));
                                FengGameManagerMKII.instance.maxPlayers = num3;
                                PhotonNetwork.room.maxPlayers = num3;
                                objArray3 = new object[] { "<color=#FFCC00>Max players changed to " + this.inputLine.Substring(10) + "!</color>", "" };
                                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            }
                            else if (this.inputLine.Substring(6).StartsWith("time"))
                            {
                                FengGameManagerMKII.instance.addTime(Convert.ToSingle(this.inputLine.Substring(11)));
                                objArray3 = new object[] { "<color=#FFCC00>" + this.inputLine.Substring(11) + " seconds added to the clock.</color>", "" };
                                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            }
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: not master client</color>");
                        }
                    }
                    else if (this.inputLine.StartsWith("/resetkd"))
                    {
                        Hashtable hashtable;
                        if (this.inputLine == "/resetkdall")
                        {
                            if (PhotonNetwork.isMasterClient)
                            {
                                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                                {
                                    hashtable = new Hashtable();
                                    hashtable.Add(PhotonPlayerProperty.kills, 0);
                                    hashtable.Add(PhotonPlayerProperty.deaths, 0);
                                    hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
                                    hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
                                    player.SetCustomProperties(hashtable);
                                }
                                objArray3 = new object[] { "<color=#FFCC00>All stats have been reset.</color>", "" };
                                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            }
                            else
                            {
                                this.addLINE("<color=#FFCC00>error: not master client</color>");
                            }
                        }
                        else
                        {
                            hashtable = new Hashtable();
                            hashtable.Add(PhotonPlayerProperty.kills, 0);
                            hashtable.Add(PhotonPlayerProperty.deaths, 0);
                            hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
                            hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
                            PhotonNetwork.player.SetCustomProperties(hashtable);
                            this.addLINE("<color=#FFCC00>Your stats have been reset. </color>");
                        }
                    }
                    else if (this.inputLine.StartsWith("/pm"))
                    {
                        string[] strArray = this.inputLine.Split(new char[] { ' ' });
                        PhotonPlayer targetPlayer = PhotonPlayer.Find(Convert.ToInt32(strArray[1]));
                        str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]).hexColor();
                        if (str2 == string.Empty)
                        {
                            str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]);
                            if (PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam] != null)
                            {
                                if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 1)
                                {
                                    str2 = "<color=#00FFFF>" + str2 + "</color>";
                                }
                                else if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 2)
                                {
                                    str2 = "<color=#FF00FF>" + str2 + "</color>";
                                }
                            }
                        }
                        string str3 = RCextensions.returnStringFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.name]).hexColor();
                        if (str3 == string.Empty)
                        {
                            str3 = RCextensions.returnStringFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.name]);
                            if (targetPlayer.CustomProperties[PhotonPlayerProperty.RCteam] != null)
                            {
                                if (RCextensions.returnIntFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.RCteam]) == 1)
                                {
                                    str3 = "<color=#00FFFF>" + str3 + "</color>";
                                }
                                else if (RCextensions.returnIntFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.RCteam]) == 2)
                                {
                                    str3 = "<color=#FF00FF>" + str3 + "</color>";
                                }
                            }
                        }
                        string str4 = string.Empty;
                        for (num4 = 2; num4 < strArray.Length; num4++)
                        {
                            str4 = str4 + strArray[num4] + " ";
                        }
                        FengGameManagerMKII.instance.photonView.RPC("ChatPM", targetPlayer, new object[] { str2, str4 });
                        this.addLINE("<color=#FFC000>TO [" + targetPlayer.ID.ToString() + "]</color> " + str3 + ":" + str4);
                    }
                    else if (this.inputLine.StartsWith("/team"))
                    {
                        if (FengGameManagerMKII.Gamemode.TeamMode == TeamMode.NoSort)
                        {
                            if ((this.inputLine.Substring(6) == "1") || (this.inputLine.Substring(6) == "cyan"))
                            {
                                FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, new object[] { 1 });
                                this.addLINE("<color=#00FFFF>You have joined team cyan.</color>");
                                foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Player"))
                                {
                                    if (obj2.GetPhotonView().isMine)
                                    {
                                        obj2.GetComponent<Hero>().markDie();
                                        obj2.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                                    }
                                }
                            }
                            else if ((this.inputLine.Substring(6) == "2") || (this.inputLine.Substring(6) == "magenta"))
                            {
                                FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, new object[] { 2 });
                                this.addLINE("<color=#FF00FF>You have joined team magenta.</color>");
                                foreach (GameObject obj3 in GameObject.FindGameObjectsWithTag("Player"))
                                {
                                    if (obj3.GetPhotonView().isMine)
                                    {
                                        obj3.GetComponent<Hero>().markDie();
                                        obj3.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                                    }
                                }
                            }
                            else if ((this.inputLine.Substring(6) == "0") || (this.inputLine.Substring(6) == "individual"))
                            {
                                FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, new object[] { 0 });
                                this.addLINE("<color=#00FF00>You have joined individuals.</color>");
                                foreach (GameObject obj4 in GameObject.FindGameObjectsWithTag("Player"))
                                {
                                    if (obj4.GetPhotonView().isMine)
                                    {
                                        obj4.GetComponent<Hero>().markDie();
                                        obj4.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                                    }
                                }
                            }
                            else
                            {
                                this.addLINE("<color=#FFCC00>error: invalid team code. Accepted values are 0,1, and 2.</color>");
                            }
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: teams are locked or disabled. </color>");
                        }
                    }
                    else if (this.inputLine == "/restart")
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            objArray3 = new object[] { "<color=#FFCC00>MasterClient has restarted the game!</color>", "" };
                            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            FengGameManagerMKII.instance.restartRC();
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: not master client</color>");
                        }
                    }
                    else if (this.inputLine.StartsWith("/specmode"))
                    {
                        if (((int)FengGameManagerMKII.settings[0xf5]) == 0)
                        {
                            FengGameManagerMKII.settings[0xf5] = 1;
                            FengGameManagerMKII.instance.EnterSpecMode(true);
                            this.addLINE("<color=#FFCC00>You have entered spectator mode.</color>");
                        }
                        else
                        {
                            FengGameManagerMKII.settings[0xf5] = 0;
                            FengGameManagerMKII.instance.EnterSpecMode(false);
                            this.addLINE("<color=#FFCC00>You have exited spectator mode.</color>");
                        }
                    }
                    else if (this.inputLine.StartsWith("/fov"))
                    {
                        int num6 = Convert.ToInt32(this.inputLine.Substring(5));
                        Camera.main.fieldOfView = num6;
                        this.addLINE("<color=#FFCC00>Field of vision set to " + num6.ToString() + ".</color>");
                    }
                    else if (this.inputLine == "/colliders")
                    {
                        int num7 = 0;
                        foreach (TITAN titan in FengGameManagerMKII.instance.getTitans())
                        {
                            if (titan.myTitanTrigger.isCollide)
                            {
                                num7++;
                            }
                        }
                        FengGameManagerMKII.instance.chatRoom.addLINE(num7.ToString());
                    }
                    else
                    {
                        int num8;
                        if (this.inputLine.StartsWith("/spectate"))
                        {
                            num8 = Convert.ToInt32(this.inputLine.Substring(10));
                            foreach (GameObject obj5 in GameObject.FindGameObjectsWithTag("Player"))
                            {
                                if (obj5.GetPhotonView().owner.ID == num8)
                                {
                                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj5, true, false);
                                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
                                }
                            }
                        }
                        else if (!this.inputLine.StartsWith("/kill"))
                        {
                            object[] objArray5;
                            if (this.inputLine.StartsWith("/revive"))
                            {
                                if (PhotonNetwork.isMasterClient)
                                {
                                    if (this.inputLine == "/reviveall")
                                    {
                                        objArray5 = new object[] { "<color=#FFCC00>All players have been revived.</color>", string.Empty };
                                        FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray5);
                                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                                        {
                                            if (((player.CustomProperties[PhotonPlayerProperty.dead] != null) && RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead])) && (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 2))
                                            {
                                                FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player, new object[0]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        num8 = Convert.ToInt32(this.inputLine.Substring(8));
                                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                                        {
                                            if (player.ID == num8)
                                            {
                                                this.addLINE("<color=#FFCC00>Player " + num8.ToString() + " has been revived.</color>");
                                                if (((player.CustomProperties[PhotonPlayerProperty.dead] != null) && RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead])) && (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 2))
                                                {
                                                    objArray5 = new object[] { "<color=#FFCC00>You have been revived by the master client.</color>", string.Empty };
                                                    FengGameManagerMKII.instance.photonView.RPC("Chat", player, objArray5);
                                                    FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player, new object[0]);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    this.addLINE("<color=#FFCC00>error: not master client</color>");
                                }
                            }
                            else if (this.inputLine.StartsWith("/unban"))
                            {
                                if (FengGameManagerMKII.OnPrivateServer)
                                {
                                    FengGameManagerMKII.ServerRequestUnban(this.inputLine.Substring(7));
                                }
                                else if (PhotonNetwork.isMasterClient)
                                {
                                    int num9 = Convert.ToInt32(this.inputLine.Substring(7));
                                    if (FengGameManagerMKII.banHash.ContainsKey(num9))
                                    {
                                        objArray5 = new object[] { "<color=#FFCC00>" + ((string)FengGameManagerMKII.banHash[num9]) + " has been unbanned from the server. </color>", string.Empty };
                                        FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray5);
                                        FengGameManagerMKII.banHash.Remove(num9);
                                    }
                                    else
                                    {
                                        this.addLINE("error: no such player");
                                    }
                                }
                                else
                                {
                                    this.addLINE("<color=#FFCC00>error: not master client</color>");
                                }
                            }
                            else if (this.inputLine.StartsWith("/rules"))
                            {
                                this.addLINE("<color=#FFCC00>Currently activated gamemodes:</color>");
                                if (FengGameManagerMKII.Gamemode.Horse)
                                {
                                    this.addLINE("<color=#FFCC00>Horses are enabled.</color>");
                                }
                                if (FengGameManagerMKII.Gamemode.Motd != string.Empty)
                                {
                                    this.addLINE("<color=#FFCC00>MOTD:" + FengGameManagerMKII.Gamemode.Motd + "</color>");
                                }
                            }
                            else
                            {
                                object[] objArray6;
                                bool flag2;
                                object[] objArray7;
                                if (this.inputLine.StartsWith("/kick"))
                                {
                                    num8 = Convert.ToInt32(this.inputLine.Substring(6));
                                    if (num8 == PhotonNetwork.player.ID)
                                    {
                                        this.addLINE("error:can't kick yourself.");
                                    }
                                    else if (!(FengGameManagerMKII.OnPrivateServer || PhotonNetwork.isMasterClient))
                                    {
                                        objArray6 = new object[] { "/kick #" + Convert.ToString(num8), LoginFengKAI.player.name };
                                        FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray6);
                                    }
                                    else
                                    {
                                        flag2 = false;
                                        foreach (PhotonPlayer player3 in PhotonNetwork.playerList)
                                        {
                                            if (num8 == player3.ID)
                                            {
                                                flag2 = true;
                                                if (FengGameManagerMKII.OnPrivateServer)
                                                {
                                                    FengGameManagerMKII.instance.kickPlayerRC(player3, false, "");
                                                }
                                                else if (PhotonNetwork.isMasterClient)
                                                {
                                                    FengGameManagerMKII.instance.kickPlayerRC(player3, false, "");
                                                    objArray7 = new object[] { "<color=#FFCC00>" + RCextensions.returnStringFromObject(player3.CustomProperties[PhotonPlayerProperty.name]) + " has been kicked from the server!</color>", string.Empty };
                                                    FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray7);
                                                }
                                            }
                                        }
                                        if (!flag2)
                                        {
                                            this.addLINE("error:no such player.");
                                        }
                                    }
                                }
                                else if (this.inputLine.StartsWith("/ban"))
                                {
                                    if (this.inputLine == "/banlist")
                                    {
                                        this.addLINE("<color=#FFCC00>List of banned players:</color>");
                                        foreach (int num10 in FengGameManagerMKII.banHash.Keys)
                                        {
                                            this.addLINE("<color=#FFCC00>" + Convert.ToString(num10) + ":" + ((string)FengGameManagerMKII.banHash[num10]) + "</color>");
                                        }
                                    }
                                    else
                                    {
                                        num8 = Convert.ToInt32(this.inputLine.Substring(5));
                                        if (num8 == PhotonNetwork.player.ID)
                                        {
                                            this.addLINE("error:can't kick yourself.");
                                        }
                                        else if (!(FengGameManagerMKII.OnPrivateServer || PhotonNetwork.isMasterClient))
                                        {
                                            objArray6 = new object[] { "/kick #" + Convert.ToString(num8), LoginFengKAI.player.name };
                                            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray6);
                                        }
                                        else
                                        {
                                            flag2 = false;
                                            foreach (PhotonPlayer player3 in PhotonNetwork.playerList)
                                            {
                                                if (num8 == player3.ID)
                                                {
                                                    flag2 = true;
                                                    if (FengGameManagerMKII.OnPrivateServer)
                                                    {
                                                        FengGameManagerMKII.instance.kickPlayerRC(player3, true, "");
                                                    }
                                                    else if (PhotonNetwork.isMasterClient)
                                                    {
                                                        FengGameManagerMKII.instance.kickPlayerRC(player3, true, "");
                                                        objArray7 = new object[] { "<color=#FFCC00>" + RCextensions.returnStringFromObject(player3.CustomProperties[PhotonPlayerProperty.name]) + " has been banned from the server!</color>", string.Empty };
                                                        FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray7);
                                                    }
                                                }
                                            }
                                            if (!flag2)
                                            {
                                                this.addLINE("error:no such player.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                this.inputLine = string.Empty;
                ChatInputField?.Select();
                ChatInputField.text = string.Empty;
                return;
            }
            this.inputLine = "\t";
            ChatInputField?.Select();
        }
    Label_013D:
        string text = string.Empty;
        if (messages.Count < 15)
        {
            for (num4 = 0; num4 < messages.Count; num4++)
            {
                text = text + messages[num4] + "\n";
            }
        }
        else
        {
            for (int i = messages.Count - 15; i < messages.Count; i++)
            {
                text = text + messages[i] + "\n";
            }
        }

        if (ChatText != null) ChatText.text = text;
        this.inputLine = ChatInputField?.text;
    }
}

