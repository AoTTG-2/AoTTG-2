using System;

public class ErrorCode
{
    [Obsolete("No longer used, cause random matchmaking is no longer a process.")]
    public const int AlreadyMatched = 0x7ffb;
    public const int AuthenticationTicketExpired = 0x7ff1;
    public const int CustomAuthenticationFailed = 0x7ff3;
    public const int GameClosed = 0x7ffc;
    public const int GameDoesNotExist = 0x7ff6;
    public const int GameFull = 0x7ffd;
    public const int GameIdAlreadyExists = 0x7ffe;
    public const int InternalServerError = -1;
    public const int InvalidAuthentication = 0x7fff;
    public const int InvalidOperationCode = -2;
    public const int InvalidRegion = 0x7ff4;
    public const int MaxCcuReached = 0x7ff5;
    public const int NoRandomMatchFound = 0x7ff8;
    public const int Ok = 0;
    public const int OperationNotAllowedInCurrentState = -3;
    public const int ServerFull = 0x7ffa;
    public const int UserBlocked = 0x7ff9;
    public const int WebHookCallFailed = 0x7ff0;
}

