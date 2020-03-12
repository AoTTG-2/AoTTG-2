using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class InputLine
{
    public static List<string> Inputs = new List<string>();
    public static string inputLine = string.Empty;

    public static int pointer = 0;
    
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
