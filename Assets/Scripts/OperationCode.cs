using System;

public class OperationCode
{
    public const byte Authenticate = 230;
    public const byte ChangeGroups = 0xf8;
    public const byte CreateGame = 0xe3;
    public const byte FindFriends = 0xde;
    public const byte GetLobbyStats = 0xdd;
    public const byte GetProperties = 0xfb;
    public const byte GetRegions = 220;
    public const byte JoinGame = 0xe2;
    public const byte JoinLobby = 0xe5;
    public const byte JoinRandomGame = 0xe1;
    public const byte Leave = 0xfe;
    public const byte LeaveLobby = 0xe4;
    public const byte RaiseEvent = 0xfd;
    public const byte SetProperties = 0xfc;
    public const byte WebRpc = 0xdb;
}

