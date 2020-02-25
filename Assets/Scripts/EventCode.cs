using System;

public class EventCode
{
    public const byte AppStats = 0xe2;
    [Obsolete("TCP routing was removed after becoming obsolete.")]
    public const byte AzureNodeInfo = 210;
    public const byte GameList = 230;
    public const byte GameListUpdate = 0xe5;
    public const byte Join = 0xff;
    public const byte Leave = 0xfe;
    public const byte Match = 0xe3;
    public const byte PropertiesChanged = 0xfd;
    public const byte QueueState = 0xe4;
    [Obsolete("Use PropertiesChanged now.")]
    public const byte SetProperties = 0xfd;
    public const byte TypedLobbyStats = 0xe0;
}

