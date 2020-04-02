using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.UI;
using UnityEngine.SceneManagement;

public class EMCli : Photon.MonoBehaviour
{
    private static float layoutWidth = 300f;
    private static float floatShift = layoutWidth + 25;
    private static Rect rectMainGUI = new Rect(0, 0, layoutWidth, 430f);
    private static Rect rectSuggestionLayout = rectMainGUI;
    private static Rect rectSuggestionsLayoutShift = new Rect(320f, 0, layoutWidth, 430f);
    private static GUILayoutOption[] guiLayoutOptionMainGUI = new GUILayoutOption[] { GUILayout.Width(320f), GUILayout.MaxHeight(430f) };
    private static GUI.WindowFunction windowFunctionMainGUI = new GUI.WindowFunction(WindowLayoutMainGUI);
    private static string layout = string.Empty;
    private static Vector2 scrollPosition = Vector2.zero;
    public bool Visible = true;
    private static bool readyToJoinOrCreateRoom = false;
    private static Vector2 scrollPositionSuggestions = Vector2.zero;
    private static string suggestions = string.Empty;

    private static Color backgroundColor = Color.black; //use whatever color you like
    private static string textColorCode = "[ffffff]";
    private static string crossColorCode = "[ff0000]";
    private static string enterColorCode = "[ffffff]";

    private static string nameOfControle = "CLI";
    private static string head = "Command Line".RepaintCustom(textColorCode, true);
    private static string arrow = ">".RepaintCustom(enterColorCode, true);
    private static string delete = "x".RepaintCustom(crossColorCode, true);
    private static KeyCode keyCodeShowHide = KeyCode.BackQuote; //Show/Hide console
    private static KeyCode keyCodeFocus = KeyCode.RightAlt; //Press this and start typing right away
    private static KeyCode keyCodeEnter = KeyCode.Return; //Enter a command

    // Use this for initialization
    void Start()
    {
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        EnterCommand("/info");
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F3)) //Does not work here
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
            if (Event.current.keyCode == keyCodeFocus)
            {
                if (GUI.GetNameOfFocusedControl() == nameOfControle)
                {
                    GUI.FocusControl(string.Empty);
                }
                else
                {
                    GUI.FocusControl(nameOfControle);
                }
            }

            if (GUI.GetNameOfFocusedControl() == nameOfControle) //logic for command suggestions
            {
                if (!InputLine.IsEmpty())
                {
                    if (Event.current.keyCode == KeyCode.RightControl)
                    {
                        InputLine.Switch(); 
                    }
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
            else if (Event.current.keyCode == keyCodeShowHide)
            {
                Visible = !Visible;
            }

        }
        else if (Event.current.type == EventType.KeyDown)
        {
            
        }
        
        if(GUI.GetNameOfFocusedControl() != nameOfControle && InputLine.IsSuggestionModeEnabled)
        {
            InputLine.Switch();
        }

         if (Visible) EMCliGUI();
    }

    public static void AddLine(string line)
    {
        layout = string.Concat(layout, '\n', arrow, line);
        scrollPosition += new Vector2(0, 150);
    }

    public void EMCliGUI()
    {
        GUI.backgroundColor = backgroundColor;
        rectMainGUI = GUILayout.Window(208, rectMainGUI, windowFunctionMainGUI, head, guiLayoutOptionMainGUI);
        if (!string.IsNullOrEmpty(InputLine.inputLine))
        {
            GUI.Label(rectMainGUI.ShiftToRight(floatShift), getRelevantSuggestions().RepaintYellow(true));
        }
    }

    private static string getRelevantSuggestions()
    {
        string relevantSuggestions = string.Empty;
        foreach(Command command in CommandHandler.Instance.Commands)
        {
            if (command.Format.StartsWith(InputLine.inputLine))
            {
                relevantSuggestions = string.Concat(relevantSuggestions, command.Format, "\n");
            }
        }
        return relevantSuggestions;
    }
    
    public static void WindowLayoutMainGUI(int windowID)
    {
        GUI.backgroundColor = backgroundColor;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(arrow, GUILayout.MaxWidth(25)))
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
        if (GUILayout.Button(delete, GUILayout.MaxWidth(25)))
        {
            InputLine.inputLine = string.Empty;
        }
        GUILayout.EndHorizontal();

        if ((Event.current.isKey))
        {
            if (Event.current.type == EventType.KeyUp)
            {
                if ((Event.current.keyCode == keyCodeEnter) && GUI.GetNameOfFocusedControl().Equals(nameOfControle))
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

        GUI.DragWindow();
    }

    public static void EnterCommand(string command)
    {
        if (!InputLine.IsSuggestionModeEnabled) InputLine.AddInput(command);
        else InputLine.AddInputToCopy(command);
        CommandHandler.ExecuteLine(command);
    }

    public static IEnumerator ConnectAndJoinIE(bool spawnAfterLoad)
    {
        if (spawnAfterLoad) SceneManager.sceneLoaded += onLevelWasLoaded;
        if (PhotonNetwork.JoinedRoomOrLobby()) PhotonNetwork.Disconnect();

        while (PhotonNetwork.JoinedRoomOrLobby())
        {
            yield return new WaitForEndOfFrame();
        }
        "Connecting...".SendProcessing(true);
        PhotonNetwork.PhotonServerSettings.JoinLobby = false;
        PhotonNetwork.ConnectToRegion(CloudRegionCode.eu, "2020");
        while (!readyToJoinOrCreateRoom)
        {
            yield return new WaitForEndOfFrame();
        }
        //"Connected to master...".SendProcessing(true);
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 10
        };
        PhotonNetwork.PhotonServerSettings.JoinLobby = true;
        PhotonNetwork.JoinOrCreateRoom("TestServer`The City`abnormal`999999`day``1", roomOptions, TypedLobby.Default);
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
