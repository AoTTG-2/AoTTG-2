using Assets.Scripts.UI.CommandLine;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
    public  List<Command> Commands;
    
    public static CommandHandler Instance;

    public static void AddCommand(Command command)
    {
        if (Instance != null)
        {
            if (!Instance.Commands.Contains(command))
            {
                Instance.Commands.Add(command);
            }
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Commands = new List<Command>();
        Command spawn = new Command("/spawn", "Spawn", string.Empty, ConsoleCommands.Spawn);
        Command connect = new Command("/connect", "Test connect", string.Empty, ConsoleCommands.TestConnect);
        Command printMessage = new Command("/print", "Print info about this command", "[1] [2] [3]", ConsoleCommands.PrintMessage);
        Command clearCLIMessages = new Command("/cl", "Clear Command Line Layout", string.Empty, ConsoleCommands.ClearCLIMessages);
        Command clearCommandsHistory = new Command("/c", "Clear commands history", string.Empty, ConsoleCommands.ClearCommandsHistory);
        Command fastLoadAndSpawn = new Command("/start", "Create/Join a room and spawn with one click!", string.Empty, ConsoleCommands.FastLoadAndSpawn);
        Command Info = new Command("/help", "Use this command to get the description of any available command in this system!", "[first characters]", ConsoleCommands.GetInfoAboutCommand);
        Command InfoAboutCommandLine = new Command("/info", "More information about this Command Line", string.Empty, ConsoleCommands.Info);
        Command SwitchDebugLevel = new Command("/debug", "Enable/Disable debug levels for this console. Only messages with allowed levels will be shown.", "[level (0-8)]", ConsoleCommands.SwitchDebugLevel);
    }

    public static void ExecuteLine(string line)
    {
        Command cd = line.GetCommand();
        if (cd != null)
        {
            cd.Execute(line);
        }
    }
}
