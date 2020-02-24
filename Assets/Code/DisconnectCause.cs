using System;

public enum DisconnectCause
{
    AuthenticationTicketExpired = 0x7ff1,
    DisconnectByClientTimeout = 0x410,
    [Obsolete("Replaced by clearer: DisconnectByServerTimeout")]
    DisconnectByServer = 0x411,
    DisconnectByServerLogic = 0x413,
    DisconnectByServerTimeout = 0x411,
    DisconnectByServerUserLimit = 0x412,
    Exception = 0x402,
    ExceptionOnConnect = 0x3ff,
    InternalReceiveException = 0x40f,
    InvalidAuthentication = 0x7fff,
    InvalidRegion = 0x7ff4,
    MaxCcuReached = 0x7ff5,
    SecurityExceptionOnConnect = 0x3fe,
    [Obsolete("Replaced by clearer: DisconnectByClientTimeout")]
    TimeoutDisconnect = 0x410
}

