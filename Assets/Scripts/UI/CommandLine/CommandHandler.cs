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
            if (!Instance.Commands.Contains(command)) Instance.Commands.Add(command);
        }
    }

    void Start()
    {
        UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        Commands = new List<Command>();
        Instance = this;
        Command sp = new Command("/sp", "Spawn", "/sp", Spawn);
        Command cn = new Command("/cn", "Test connect", "/cn", TestConnect);
        Command printMessage = new Command("/prm", "Print info about this command", "/prm [1] [2] [3]", PrintMessage);
        Command cl = new Command("/cl", "Clear Command Line Layout", "/cl", ClearCLILayout);
        Command clearCommandsHistory = new Command("/c", "Clear commands history", "/c", ClearCommandsHistory);
        Command fastLoadAndSpawn = new Command("/fls", "Create/Join a room and spawn with one click!", "/fls", FastLoadAndSpawn);
    }

    void FastLoadAndSpawn(Command command)
    {
        FengGameManagerMKII.instance.StartCoroutine(EMCli.ConnectAndJoinIE(true));
        FengGameManagerMKII.showHackMenu = false;
    }

    void ClearCommandsHistory(Command command)
    {
        InputLine.ClearInputs();
        "Commands history has been cleaned!".SendProcessing(true);
    }

    void ClearCLILayout(Command command)
    {
        EMCli.ClearLayout();
    }

    void Spawn(Command command)
    {
        AottgUi.TestSpawn();
    }

    void TestConnect(Command command)
    {
        FengGameManagerMKII.instance.StartCoroutine(EMCli.ConnectAndJoinIE(false));
        FengGameManagerMKII.showHackMenu = false;
    }

    void PrintMessage(Command command)
    {
        //You can access all arguments from Command as well as description and some other info
        $"Info: {command.Format} - \n description: {command.Description} \n Args count: {command.LastArgsAsString.Length}".SendProcessing(true);
    }

    public static void ExecuteLine(string line)
    {
        Command cd = getCommand(line);
        if (cd != null)
        {
            cd.Execute(line);
        }
    }

    private static Command getCommand(string line)
    {
        string nameOfCommand = line.SplitCommand()[0];
        foreach (Command command in Instance.Commands)
        {
            if (command.Name.Equals(nameOfCommand))
            {
                return command;
            }
        }
        $"Command {nameOfCommand} not found!".SendError(true);
        return null;
    }
}
