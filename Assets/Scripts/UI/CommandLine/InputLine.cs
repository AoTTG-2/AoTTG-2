using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class InputLine
{
    public static List<string> Inputs = new List<string>();
    private static List<string> inputsCopy = new List<string>();
    private static string inputLineCopy = string.Empty;
    private static string inputLineBackup = string.Empty;
    public static string inputLine = string.Empty;
    public static bool IsSuggestionModeEnabled = false;
    public static int pointer = 0;

    public static void Backup()
    {
        inputLineBackup = inputLine;
    }

    public static void OnSwitchFix() // Let me know if you know better solution
    {
        inputLine = inputLine.Replace("`", string.Empty);
    }

    public static void Restore()
    {
        inputLine = inputLineBackup;
    }

    public static bool IsEmpty()
    {
        return string.IsNullOrEmpty(inputLine);
    }

    public static void Switch()
    {
        if (IsSuggestionModeEnabled)
        {
            SwitchToDefault();
        }
        else
        {
            SwitchToSuggestions();
        }
    }

    public static void SwitchToSuggestions()
    {
        inputsCopy = Inputs;
        inputLineCopy = inputLine;
        Inputs = new List<string>();
        foreach(Command command in CommandHandler.Instance.Commands)
        {
            if (command.Name.StartsWith(inputLine))
            {
                AddInput(command.Name);
            }
        }
        pointer = 0;

        if (Inputs.Count != 0) inputLine = Inputs[pointer];
        else
        {
            $"Commands not found!".SendError(true);
        }
        IsSuggestionModeEnabled = true;
    }

    public static void SwitchToDefault()
    {
        Inputs = inputsCopy;
        inputLine = inputLineCopy;
        pointer = 0;
        IsSuggestionModeEnabled = false;
    }
    
    public static void AddInputToCopy(string input)
    {
        if (inputsCopy.Contains(input))
        {
            inputsCopy.Remove(input);
        }
        inputsCopy.Add(input);
    }

    public static void AddInput(string input)
    {
        if (Inputs.Contains(input))
        {
            Inputs.Remove(input);
        }
        Inputs.Add(input);
    }

    public static void Up()
    {
        if (Inputs.Count == 0) return;

        ++pointer;

        if (pointer > Inputs.Count - 1)
        {
            pointer = 0;
        }
        if (pointer < 0)
        {
            pointer = 0;
        }
        inputLine = Inputs[pointer];
    }

    public static void Down()
    {
        if (Inputs.Count == 0) return;

        --pointer;

        if (pointer < 0)
        {
            pointer = Inputs.Count - 1;
        }
        if (pointer > Inputs.Count - 1)
        {
            pointer = Inputs.Count - 1;
        }
        inputLine = Inputs[pointer];
    }

    public static void ClearInputs()
    {
        Inputs = new List<string>();
    }
}
