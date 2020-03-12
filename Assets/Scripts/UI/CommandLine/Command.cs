using UnityEngine;
using System.Collections;

public class Command
{
    public string Name;
    public string Description;
    public string Format;
    public string[] LastArgsAsString;
    public string RawLine;

    public delegate void CommandFunction(Command command);
    private CommandFunction commandFunction = new CommandFunction(defaultFunction);

    public delegate void OnExecutedCommand(Command command);
    private OnExecutedCommand onExecutedCommand = new OnExecutedCommand(defaultCallback);

    private static void defaultFunction(Command command)
    {
        if (command != null) $"{(command).Name}: default function has been called! Perhaps you should specify your intentions!".SendWarning(true);
        else "Command.defaultFunction: Sender is null! Smth went wrong!".SendError(true);
    }
    
    private static void defaultCallback(Command command)
    {
        if (command != null) $"{(command).Name}: default callback has been called! Perhaps you should specify your intentions!".SendWarning(true);
        else "Command.defaultCallback: Sender is null! Smth went wrong!".SendError(true);
    }

    public Command(string name, string description, string format, Command.CommandFunction function)
    {
        Name = name;
        Description = description;
        Format = format;
        commandFunction = new CommandFunction(function);
        CommandHandler.AddCommand(this);
    }

    public Command()
    {
        
    }

    public void Execute(string line)
    {
        RawLine = line;
        LastArgsAsString = line.GetArgs();
        commandFunction(this);
    }
}
