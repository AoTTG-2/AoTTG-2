﻿using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public static class Extensions2
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
        EMCli.AddLine(input);
        return input;
    }

    public static string SendError(this string input, bool bold = false)
    {
        EMCli.AddLine(input.RepaintError(bold));
        return input;
    }

    public static string SendWarning(this string input, bool bold = false)
    {
        EMCli.AddLine(input.RepaintWarning(bold));
        return input;
    }

    public static string SendProcessing(this string input, bool bold = false)
    {
        EMCli.AddLine(input.RepaintProcessing(bold));
        return input;
    }

    public static string RepaintError(this string input, bool bold = false)
    {
        string colored = string.Concat(Error, input).hexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string RepaintWarning(this string input, bool bold = false)
    {
        string colored = string.Concat(Warning, input).hexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string RepaintGreen(this string input, bool bold = false)
    {
        string colored = string.Concat(Green, input).hexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string RepaintYellow(this string input, bool bold = false)
    {
        string colored = string.Concat(Yellow, input).hexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string RepaintProcessing(this string input, bool bold = false)
    {
        string colored = string.Concat(Processing, input).hexColor2();
        if (bold) return string.Concat("<b>", colored, "</b>");
        return colored;
    }
    public static string hexColor2(this string text)
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
}
