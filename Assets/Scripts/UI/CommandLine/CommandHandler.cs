using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI;

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
        UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        Commands = new List<Command>();
        Instance = this;
        Command sp = new Command("/spawn", "Spawn", string.Empty, ConsoleCommands.Spawn);
        Command cn = new Command("/connect", "Test connect", string.Empty, ConsoleCommands.TestConnect);
        Command printMessage = new Command("/print", "Print info about this command", "[1] [2] [3]", ConsoleCommands.PrintMessage);
        Command cl = new Command("/cl", "Clear Command Line Layout", string.Empty, ConsoleCommands.ClearCLILayout);
        Command clearCommandsHistory = new Command("/c", "Clear commands history", string.Empty, ConsoleCommands.ClearCommandsHistory);
        Command fastLoadAndSpawn = new Command("/start", "Create/Join a room and spawn with one click!", string.Empty, ConsoleCommands.FastLoadAndSpawn);
        Command SomeCommand = new Command("/dosmth", "There could be lots of stuff ", "[1] [2] [3] [4] [5] [6-smth else] [7-wow] [8 - no] [9 - even more stuff incoming!]", ConsoleCommands.FastLoadAndSpawn);
        Command Info = new Command("/help", "Use this command to get the description of any available command in this system!", "[first characters]", ConsoleCommands.GetInfoAboutCommand);
        Command InfoAboutCommandLine = new Command("/info", "More information about this Command Line", string.Empty, ConsoleCommands.Info);
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
