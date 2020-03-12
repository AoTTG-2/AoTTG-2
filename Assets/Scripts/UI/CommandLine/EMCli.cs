using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.UI;
using UnityEngine.SceneManagement;

public class EMCli : Photon.MonoBehaviour
{
    private static Rect windowRect = new Rect(Screen.width - 320f, 0, 300f, 430f);
    private static GUILayoutOption[] guiLayoutOption = new GUILayoutOption[] { GUILayout.Width(320f), GUILayout.MaxHeight(430f) };
    private static GUI.WindowFunction windowFunction = new GUI.WindowFunction(WindowLayout);
    private static string layout = string.Empty;
    private static Vector2 scrollPosition = Vector2.zero;
    //private static string inputLine = string.Empty;
    private static string nameOfControle = "CLI";
    private static string head = "Command Line".RepaintGreen(true);
    public bool Visible = true;
    private static bool readyToJoinOrCreateRoom = false;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F3))
        //{
        //    Visible = !Visible;
        //}
        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    InputLine.Up();
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    InputLine.Down();
        //}
    }

    public static void ClearLayout()
    {
        layout = string.Empty;
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.KeyUp)
        {
            if (Event.current.keyCode == KeyCode.RightAlt)
            {
                if (GUI.GetNameOfFocusedControl() == nameOfControle)
                {
                    GUI.FocusControl(string.Empty);
                }
                else
                {
                    GUI.FocusControl(nameOfControle);
                    //GUI.GetNameOfFocusedControl().SendCli();
                }
            }
            if (Event.current.keyCode == KeyCode.UpArrow)
            {
                InputLine.Up();
            }
            else if (Event.current.keyCode == KeyCode.DownArrow)
            {
                InputLine.Down();
            }
            else if (Event.current.keyCode == KeyCode.F3)
            {
                Visible = !Visible;
            }
        }
        //else if (Event.current.type == EventType.KeyDown)
        //{

        //}

         if (Visible) EMCliGUI();
    }

    public static void AddLine(string line)
    {
        layout = string.Concat(layout, '\n', line);
        scrollPosition += new Vector2(0, 150);
    }

    public void EMCliGUI()
    {
        GUI.backgroundColor = Color.green;
        windowRect = GUILayout.Window(208, windowRect, windowFunction, head, guiLayoutOption);
    }
    
    public static void WindowLayout(int windowID)
    {
        GUI.backgroundColor = Color.green;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(">", GUILayout.MaxWidth(25)))
        {
            try
            {
                EnterCommand(InputLine.inputLine);
            }
            catch (Exception ex)
            {
                $" {ex.ToString()} ".SendError(true);
                Debug.LogException(ex);
            }

            InputLine.inputLine = string.Empty;

            return;
        }
        GUI.SetNextControlName(nameOfControle);
        InputLine.inputLine = GUILayout.TextField(InputLine.inputLine, new GUILayoutOption[0]);
        if (GUILayout.Button("X".RepaintError(true), GUILayout.MaxWidth(25)))
        {
            InputLine.inputLine = string.Empty;
        }
        GUILayout.EndHorizontal();

        if ((Event.current.isKey))
        {
            if (Event.current.type == EventType.KeyUp)
            {
                //Event.current.keyCode.ToString().SendCli();
                if ((Event.current.keyCode == KeyCode.Return) && GUI.GetNameOfFocusedControl().Equals(nameOfControle))
                {
                    if (!string.IsNullOrEmpty(InputLine.inputLine))
                    {
                        try
                        {
                            EnterCommand(InputLine.inputLine);
                        }
                        catch (Exception ex)
                        {
                            $" {ex.ToString() }".SendError(true);
                            Debug.LogException(ex);
                        }
                        InputLine.inputLine = string.Empty;
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
        //if (command.StartsWith("/"))
        //{
        //    if (command.StartsWith("/sp"))
        //    {
        //        AottgUi.TestSpawn();
        //    }
        //    else if (command.StartsWith("/cn"))
        //    {
        //        FengGameManagerMKII.instance.StartCoroutine(EMCli.ConnectAndJoinIE(false));
        //        FengGameManagerMKII.showHackMenu = false;
        //    }
        //    else if (command.StartsWith("/dc"))
        //    {
        //        PhotonNetwork.Disconnect();
        //    }

        //    else
        //    {
        //        $"[{command}] - there is no such command!".SendError(true);
        //    }
        //}
        //else
        //{
        //    $"[{command}] is not a command!".SendError(true);
        //}
        InputLine.AddInput(command);
        CommandHandler.ExecuteLine(command);
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
        PhotonNetwork.ConnectToMaster("app-eu.exitgamescloud.com", 5055, "f1f6195c-df4a-40f9-bae5-4744c32901ef", "Test");
        while (!readyToJoinOrCreateRoom)
        {
            yield return new WaitForEndOfFrame();
        }
        //"Connected to master...".SendProcessing(true);
        RoomOptions roomOptions = new RoomOptions
        {
            isVisible = true,
            isOpen = true,
            maxPlayers = 10
        };

        PhotonNetwork.JoinOrCreateRoom("TestServer`The City`abnormal`999999`day``1", roomOptions, TypedLobby.Default);
        //ServerList.instance.gameObject.SetActive(false);
        "Creating a room...".SendProcessing(true);
    }
    
    private void OnConnectedToMaster()
    {
        "Connected to master".SendWarning(true);
        readyToJoinOrCreateRoom = true;
    }

    private void OnDisconnectedFromPhoton()
    {
        "Disconnected from master".SendWarning(true);
        readyToJoinOrCreateRoom = false;
    }

    private static void onLevelWasLoaded(Scene scene, LoadSceneMode mode)
    {
        AottgUi.TestSpawn();
        SceneManager.sceneLoaded -= onLevelWasLoaded;
    }
}
