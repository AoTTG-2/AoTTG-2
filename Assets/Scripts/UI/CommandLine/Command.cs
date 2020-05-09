using UnityEngine;
using System.Collections;

public class Command
{
    public string Name;
    public string Description;
    public string FormatOfArgs;

    public string[] LastArgsAsString;
    public string RawLine;
    private int lastIndex => LastArgsAsString.Length - 1;

    public int LastArgAsInt32 => LastArgsAsString[lastIndex].ToInt32();
    public long LastArgAsLong => LastArgsAsString[lastIndex].ToInt64();
    public double LastArgAsDouble => LastArgsAsString[lastIndex].ToDouble();
    public float LastArgAsFloat => LastArgsAsString[lastIndex].ToFloat();
    public byte LastArgAsByte => LastArgsAsString[lastIndex].ToByte();
    public bool LastArgAsBool => LastArgsAsString[lastIndex].ToByte().Equals(1);
    public string LastArgAsString => LastArgsAsString[lastIndex];

    public delegate void CommandFunction(Command command);
    private CommandFunction commandFunction = new CommandFunction(defaultFunction);

    public delegate void OnExecutedCommand(Command command);
    private OnExecutedCommand onExecutedCommand = new OnExecutedCommand(defaultCallback);

    public string Format
    {
        get
        {
            return string.Concat(Name, " ", FormatOfArgs);
        }
    }

    private static void defaultFunction(Command command)
    {
        if (command != null) $"{(command).Name}: default function has been called! Perhaps you should specify your intentions!".SendWarning(true);
        else "Command.defaultFunction: Sender is null! Smth went wrong!".SendError(true);
    }
    
    private static void defaultCallback(Command command) //Is yet to be implemented
    {
        if (command != null) $"{(command).Name}: default callback has been called! Perhaps you should specify your intentions!".SendWarning(true);
        else "Command.defaultCallback: Sender is null! Smth went wrong!".SendError(true);
    }

    public Command(string name, string description, string formatOfArgs, Command.CommandFunction function)
    {
        Name = name;
        Description = description;
        FormatOfArgs = formatOfArgs;
        commandFunction = new CommandFunction(function);
        CommandHandler.AddCommand(this);
    }

    public void Execute(string line)
    {
        RawLine = line;
        LastArgsAsString = line.GetArgs();
        commandFunction(this);
    }
}
