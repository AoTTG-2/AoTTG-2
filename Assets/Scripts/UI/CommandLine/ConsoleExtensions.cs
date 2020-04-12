using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using Assets.Scripts.UI;

public static class ConsoleExtensions
{
    private static readonly string Green = "[00ff00]";
    private static readonly string Error = "[ff0000]";
    private static readonly string Processing = "[ffa500]";
    private static readonly string Warning = "[ffff00]";
    private static readonly string Default = "[ffffff]";
    private static readonly string Yellow = "[FFCC00]";
    private static readonly string Red = "[FE2E2E]";

    public static string SendCli(this string input)
    {
        ConsoleMessage message = new ConsoleMessage(input, DebugLevel.Debug);
        message.SendToConsole();
        EMCli.RefreshLayout();
        return input;
    }

    public static string SendError(this string input, bool bold = false)
    {
        ConsoleMessage message = new ConsoleMessage(input.RepaintError(bold), DebugLevel.Error);
        message.SendToConsole();
        EMCli.RefreshLayout();
        return input;
    }

    public static string SendWarning(this string input, bool bold = false)
    {
        ConsoleMessage message = new ConsoleMessage(input.RepaintWarning(bold), DebugLevel.Warning);
        message.SendToConsole();
        EMCli.RefreshLayout();
        return input;
    }

    public static string SendProcessing(this string input, bool bold = false)
    {
        ConsoleMessage message = new ConsoleMessage(input.RepaintProcessing(bold), DebugLevel.Info);
        message.SendToConsole();
        EMCli.RefreshLayout();
        return input;
    }

    public static string RepaintError(this string input, bool bold = false)
    {
        string colored = string.Concat(Error, input).HexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string RepaintWarning(this string input, bool bold = false)
    {
        string colored = string.Concat(Warning, input).HexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string RepaintCustom(this string input, string colorCode, bool bold = false)
    {
        string colored = string.Concat(colorCode, input).HexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string RepaintGreen(this string input, bool bold = false)
    {
        string colored = string.Concat(Green, input).HexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string RepaintYellow(this string input, bool bold = false)
    {
        string colored = string.Concat(Yellow, input).HexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string RepaintProcessing(this string input, bool bold = false)
    {
        string colored = string.Concat(Processing, input).HexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string HexColor2(this string text)
    {
        int clrs = 0;
        text = Regex.Replace(text, @"<(\/|)(x|y)>", string.Empty);
        text = text.Replace("[-]", "</color>");
        text = Regex.Replace(text, "(\\[)[A-Fa-f0-9]{6,8}(\\])", delegate (Match match)
        {
            string ret = match.Value;
            if (match.Success)
            {
                ret = ret.Replace("[", "<color=#").Replace("]", ">");
                clrs++;
            }
            return ret;
        });
        clrs = clrs - Regex.Matches(text, "</color>").Count;
        for (int i = 0; i < clrs; i++)
        {
            text = string.Concat(text, "</color>");
        }
        return text;
    }

    public static string[] SplitCommand(this string commandLine)
    {
        string[] args = commandLine.Split(new char[] { ' ' });
        return args;
    }

    public static string[] GetArgs(this string commandLine)
    {
        string[] buf = commandLine.Split(new char[] { ' ' });
        string[] args = new string[buf.Length - 1];
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = buf[i + 1];
        }
        return args;
    }

    public static Rect ShiftToLeft(this Rect rect, float left)
    {
        return Shift(rect, left, 0);
    }

    public static Rect ShiftToRight(this Rect rect, float right)
    {
        return Shift(rect, -right, 0);
    }

    public static Rect ShiftDown(this Rect rect, float down)
    {
        return Shift(rect, 0, -down);
    }

    public static Rect Shift(Rect rect, float x, float y)
    {
        float newX = rect.x - x;
        float newY = rect.y - y;
        float width = rect.width;
        float height = rect.height;
        Rect newRect = new Rect(newX, newY, width, height);
        return newRect;
    }

    public static Command GetCommand(this string line)
    {
        string nameOfCommand = line.SplitCommand()[0];
        foreach (Command command in CommandHandler.Instance.Commands)
        {
            if (command.Name.Equals(nameOfCommand))
            {
                return command;
            }
        }
        $"Command {nameOfCommand} not found!".SendError(true);
        return null;
    }

    public static float ToFloat(this string str)
    {
        return Convert.ToSingle(str);
    }

    public static int ToInt32(this string str)
    {
        return Convert.ToInt32(str);
    }

    public static byte ToByte(this string str)
    {
        return Convert.ToByte(str);
    }

    public static long ToInt64(this string str)
    {
        return Convert.ToInt64(str);
    }

    public static double ToDouble(this string str)
    {
        return Convert.ToDouble(str);
    }

    public static int ToInt32(this bool input)
    {
        if (input) return 1;
        else return 0;
    }

    public static bool ToBool(this int input)
    {
        return input.Equals(1);
    }
}
