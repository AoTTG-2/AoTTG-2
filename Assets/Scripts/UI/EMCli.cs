using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.UI;
using UnityEngine.SceneManagement;

public class EMCli : MonoBehaviour
{
    private static Rect windowRect = new Rect(Screen.width - 320f, 0, 300f, 430f);
    private static GUILayoutOption[] options1 = new GUILayoutOption[] { GUILayout.Width(320f), GUILayout.MaxHeight(430f) };
    private static GUI.WindowFunction func1 = new GUI.WindowFunction(WindowLayout);
    private static string layout = string.Empty;
    private static Vector2 scrollPosition = Vector2.zero;
    private static string inputLine = string.Empty;
    public bool Visible = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Visible = !Visible;
        }
    }

    void OnGUI()
    {
        if (Visible)
        {
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.RightAlt)
            {
                if (GUI.GetNameOfFocusedControl() == "CLI")
                {
                    GUI.FocusControl(string.Empty);
                }
                else
                {
                    GUI.FocusControl("CLI");
                    //GUI.GetNameOfFocusedControl().SendCli();
                }
            }

            EMCliGUI();
        }
    }

    public static void AddLine(string line)
    {
        layout = string.Concat(layout, '\n', line);
        scrollPosition += new Vector2(0, 150);
    }

    public void EMCliGUI()
    {
        GUI.backgroundColor = Color.green;
        windowRect = GUILayout.Window(208, windowRect, func1, "EM Command Line", options1);
    }
    
    public static void WindowLayout(int windowID)
    {
        GUI.backgroundColor = Color.green;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(">", GUILayout.MaxWidth(25)))
        {
            try
            {
                EnterCommand(inputLine);
            }
            catch (Exception ex)
            {
                $" {ex.ToString()} ".SendError(true);
            }
            
            inputLine = string.Empty;

            return;
        }
        GUI.SetNextControlName("CLI");
        inputLine = GUILayout.TextField(inputLine, new GUILayoutOption[0]);
        if (GUILayout.Button("X".RepaintError(true), GUILayout.MaxWidth(25)))
        {
            inputLine = string.Empty;
        }
        GUILayout.EndHorizontal();

        if ((Event.current.isKey))
        {
            if (Event.current.type == EventType.KeyUp)
            {
                //Event.current.keyCode.ToString().SendCli();
                if ((Event.current.keyCode == KeyCode.Return) && GUI.GetNameOfFocusedControl().Equals("CLI"))
                {
                    if (!string.IsNullOrEmpty(inputLine))
                    {
                        try
                        {
                            EnterCommand(inputLine);
                        }
                        catch (Exception ex)
                        {
                            $" {ex.ToString() }".SendError(true);
                        }
                        inputLine = string.Empty;
                        GUI.FocusControl(string.Empty);
                        return;
                    }
                }
            }
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.Label(layout);
        GUILayout.EndScrollView();

        //GUILayout.Button(ScrollPosition.ToString());

        GUI.DragWindow();
    }

    public static void EnterCommand(string command)
    {
        if (command.StartsWith("/"))
        {
            if (command.StartsWith("/sp"))
            {
                AottgUi.TestSpawn();
            }
            else if (command.StartsWith("/cn"))
            {
                FengGameManagerMKII.instance.StartCoroutine(EMCli.ConnectAndJoinIE(false));
                FengGameManagerMKII.showHackMenu = false;
            }
            else
            {
                $"[{command}] - there is no such command!".SendError(true);
            }
        }
        else
        {
            $"[{command}] is not a command!".SendError(true);
        }
    }

    public static IEnumerator ConnectAndJoinIE(bool spawnAfterLoad)
    {
        if(spawnAfterLoad) SceneManager.sceneLoaded += onLevelWasLoaded;
        if (PhotonNetwork.JoinedRoomOrLobby()) PhotonNetwork.Disconnect();

        while (PhotonNetwork.JoinedRoomOrLobby())
        {
            yield return new WaitForEndOfFrame();
        }
        "Connecting...".SendProcessing(true);
        while (!PhotonNetwork.ConnectToMaster("app-eu.exitgamescloud.com", 5055, "f1f6195c-df4a-40f9-bae5-4744c32901ef", "Test"))
        {
            yield return new WaitForEndOfFrame();
        }
        "Connected to master...".SendProcessing(true);
        RoomOptions roomOptions = new RoomOptions
        {
            isVisible = true,
            isOpen = true,
            maxPlayers = 10
        };
        while (!PhotonNetwork.JoinOrCreateRoom("TestServer`The City`abnormal`999999`day``1", roomOptions, TypedLobby.Default))
        {
            yield return new WaitForEndOfFrame();
        }
        //ServerList.instance.gameObject.SetActive(false);
        "Room has been created!".SendProcessing(true);
    }

    private static void onLevelWasLoaded(Scene scene, LoadSceneMode mode)
    {
        AottgUi.TestSpawn();
        SceneManager.sceneLoaded -= onLevelWasLoaded;
    }
}
