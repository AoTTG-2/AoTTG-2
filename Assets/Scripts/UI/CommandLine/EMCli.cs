using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.UI.InGame;
using System.Collections.Generic;
using System.Linq;

public class EMCli : Photon.MonoBehaviour
{
    public static float LayoutWidth { get; private set; } = 320f;
    public static float LayoutHeight { get; private set; } = 430f;
    private static float floatShiftRight = 0; // Defines a shift of suggestion layout
    private static float floatShiftDown = LayoutHeight + 5f;
    private static Rect rectMainGUI = new Rect(0, 0, LayoutWidth, LayoutHeight);
    private static Rect rectSuggestionLayout = rectMainGUI;
    private static GUILayoutOption[] guiLayoutOptionMainGUI = new GUILayoutOption[] { GUILayout.Width(LayoutWidth), GUILayout.MaxHeight(LayoutHeight) };
    public static GUI.WindowFunction WindowFunctionMainGUI = new GUI.WindowFunction(WindowLayoutMainGUI);
    private static string layout = string.Empty;
    public static List<ConsoleMessage> Messages { get; private set; }
    private static Vector2 scrollPosition = Vector2.zero;
    public bool Visible = false;
    public static bool VisibleOnStart = false;
    private static bool readyToJoinOrCreateRoom = false;
    private static Vector2 scrollPositionSuggestions = Vector2.zero;
    private static string suggestions = string.Empty;
    private static bool focusOnEnable = false;

    private static Color backgroundColor = Color.black; // Use whatever color you like
    private static string textColorCode = "[ffffff]";
    private static string crossColorCode = "[ff0000]";
    private static string enterColorCode = "[ffffff]";
    private static string optionsColorCode = "[ffffff]";

    private static string nameOfControle = "CLI";
    private static string head = "Command Line".RepaintCustom(textColorCode, true);
    private static string arrow = ">".RepaintCustom(enterColorCode, true);
    private static string delete = "x".RepaintCustom(crossColorCode, true);
    private static string options = "*".RepaintCustom(optionsColorCode, true);

    // Rebinds
    private static KeyCode keyCodeShowHide = KeyCode.BackQuote; // Show/Hide console
    private static KeyCode keyCodeShowHide2 = KeyCode.Escape; // Another key to show/hide console
    private static KeyCode keyCodeFocus = KeyCode.RightAlt; // Press this and start typing right away
    private static KeyCode keyCodeEnter = KeyCode.Return; // Enter a command
    private static KeyCode keyCodeSwitch = KeyCode.RightControl; // Press this button and use up/down arrow to select a command from suggestions.
    private static KeyCode keyCodeUp = KeyCode.UpArrow; // Use arrows while typing. You will get your previous input.
    private static KeyCode keyCodeDown = KeyCode.DownArrow;

    public static bool FocusOnEnable = true; // Setting: if false console wont focus on input line when you open it

    private static bool autofocus => focusOnEnable && FocusOnEnable; //First one for technical realization and second one for settings
    private static bool focusedOnInputLine => GUI.GetNameOfFocusedControl().Equals(nameOfControle);
    
    void Start()
    {
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        Messages = new List<ConsoleMessage>();
        EnterCommand("/info");
        SettingsGUI.LoadPrefs();
        SettingsGUI.ApplyChangesIfModified();
        Visible = VisibleOnStart;
    }
    
    void Update()
    {

    }

    public static void SetupGUI(float width, float height)
    {
        LayoutWidth = width;
        LayoutHeight = height;
        rectMainGUI = new Rect(0, 0, LayoutWidth, LayoutHeight);
        guiLayoutOptionMainGUI = new GUILayoutOption[] { GUILayout.Width(LayoutWidth), GUILayout.MaxHeight(LayoutHeight) };
    }

    public static void ResetGUISize()
    {
        SetupGUI(320, 430);
    }

    public static void SwitchBack()
    {
        WindowFunctionMainGUI = new GUI.WindowFunction(WindowLayoutMainGUI);
    }

    public static void SwitchDebugLevel(DebugLevel level)
    {
        foreach(DebugLevelOption dloption in DebugLevelOption.DebugLevelOptions)
        {
            if (level.Equals(dloption.DebugLevel))
            {
                dloption.Enabled = !dloption.Enabled;
                break;
            }
        }
    }

    public static void ExcludeLevel(DebugLevel level)
    {
        foreach (DebugLevelOption dloption in DebugLevelOption.DebugLevelOptions)
        {
            if (level.Equals(dloption.DebugLevel))
            {
                dloption.Enabled = false;
                break;
            }
        }
    }

    public static void IncludeLevel(DebugLevel level)
    {
        foreach (DebugLevelOption dloption in DebugLevelOption.DebugLevelOptions)
        {
            if (level.Equals(dloption.DebugLevel))
            {
                dloption.Enabled = true;
                break;
            }
        }
    }

    public static void RefreshLayout()
    {
        layout = string.Empty;
        foreach(ConsoleMessage message in Messages)
        {
            foreach(DebugLevelOption option in DebugLevelOption.DebugLevelOptions)
            {
                if (option.DebugLevel.Equals(message.ConsoleMessageDebugLevel))
                {
                    if (option.Enabled)
                    {
                        AddLine(message.Message);
                    }
                }
            }
        }
    }

    public static void AddMessage(ConsoleMessage consoleMessage)
    {
        if (Messages.Contains(consoleMessage))
        {
            Debug.LogError($"Message already exist: [{consoleMessage.Message}]");
        }
        else 
        {
            Messages.Add(consoleMessage);
        }
    }

    public static void ClearMessages()
    {
        Messages = new List<ConsoleMessage>();
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.KeyUp)
        {
            KeyUpGUI();
        }

        if (!focusedOnInputLine && InputLine.IsSuggestionModeEnabled)
        {
            InputLine.Switch();
        }

        if (Visible) EMCliGUI();
    }

    private void KeyUpGUI()
    {
        if (Event.current.keyCode == keyCodeFocus)
        {
            if (focusedOnInputLine)
            {
                GUI.FocusControl(string.Empty);
            }
            else
            {
                GUI.FocusControl(nameOfControle);
            }
        }

        if (focusedOnInputLine) //logic for command suggestions
        {
            if (!InputLine.IsEmpty())
            {
                if (Event.current.keyCode == keyCodeSwitch)
                {
                    InputLine.Switch();
                }
            }
        }

        if (Event.current.keyCode == keyCodeUp)
        {
            InputLine.Up();
        }
        else if (Event.current.keyCode == keyCodeDown)
        {
            InputLine.Down();
        }
        else if (Event.current.keyCode == keyCodeShowHide)
        {
            if (!(focusOnEnable = Visible = !Visible)) InputLine.OnSwitchFix();
        }
        else if (Event.current.keyCode == keyCodeShowHide2)
        {
            if (Visible)
            {
                Visible = !Visible;
            }
        }
    }

    public static void AddLine(string line)
    {
        layout = string.Concat(layout, '\n', arrow, line);
        scrollPosition += new Vector2(0, 150);
    }

    public void EMCliGUI()
    {
        GUI.backgroundColor = backgroundColor;
        rectMainGUI = GUILayout.Window(208, rectMainGUI, WindowFunctionMainGUI, head, guiLayoutOptionMainGUI);
        if (!string.IsNullOrEmpty(InputLine.inputLine))
        {
            GUI.Label(rectMainGUI.ShiftToRight(floatShiftRight).ShiftDown(floatShiftDown), getRelevantSuggestions().RepaintYellow(true));
        }
    }

    private static string getRelevantSuggestions()
    {
        string relevantSuggestions = string.Empty;
        foreach (Command command in CommandHandler.Instance.Commands.Take(10))
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

        GUILayout.BeginHorizontal();
        InputGUI();
        GUILayout.EndHorizontal();

        GUI.DragWindow();
        if (autofocus)
        {
            GUI.FocusControl(nameOfControle);
            focusOnEnable = false;
        }
    }

    public static void InputGUI()
    {
        if (GUILayout.Button(options, GUILayout.MaxWidth(25)))
        {
            ConsoleOptionsGUI.Switch();
        }
        else if (GUILayout.Button(arrow, GUILayout.MaxWidth(25)))
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
    }

    public static void EnterCommand(string command)
    {
        if (!InputLine.IsSuggestionModeEnabled) InputLine.AddInput(command);
        else InputLine.AddInputToCopy(command);
        CommandHandler.ExecuteLine(command);
    }

    public static IEnumerator ConnectAndJoinIE(bool spawnAfterLoad)
    {
        if (spawnAfterLoad) SceneManager.sceneLoaded += onLevelWasLoadedSpawn;
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        PhotonNetwork.PhotonServerSettings.JoinLobby = false;
        var roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 10,
            CustomRoomProperties = new ExitGames.Client.Photon. Hashtable
                {
                    { "name", "TestDev" },
                    { "level", "The City - Classic" },
                    { "gamemode", "Titans" }
                },
            CustomRoomPropertiesForLobby = new[] { "name", "level", "gamemode" }
        };

        PhotonNetwork.PhotonServerSettings.JoinLobby = true;
        PhotonNetwork.CreateRoom(Guid.NewGuid().ToString(), roomOptions, TypedLobby.Default);
        
        "Creating a room...".SendProcessing(true);
    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameObject.Find("Canvas").GetComponent<UiHandler>().ShowInGameUi();
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    private static void onLevelWasLoadedSpawn(Scene scene, LoadSceneMode mode)
    {
        CommandHandler.Instance.StartCoroutine(spawnIE());
        SceneManager.sceneLoaded -= onLevelWasLoadedSpawn;
    }

    private static IEnumerator spawnIE()
    {
        GameObject canvas = GameObject.Find("Canvas");
        yield return canvas;
        SpawnMenu spawnMenu;
        while (!canvas.GetComponentInChildren<SpawnMenu>())
        {
            yield return null;
        }
        spawnMenu = canvas.GetComponentInChildren<SpawnMenu>();
        spawnMenu.Spawn();
    }
}