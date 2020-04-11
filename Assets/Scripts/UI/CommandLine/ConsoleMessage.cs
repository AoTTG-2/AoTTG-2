using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;

public class ConsoleMessage
{
    public string Message;
    public DebugLevel ConsoleMessageDebugLevel;

    public ConsoleMessage(string message, DebugLevel level)
    {
        Message = message;
        ConsoleMessageDebugLevel = level;
    }

    public void SendToConsole()
    {
        EMCli.AddMessage(this);
    }
}
