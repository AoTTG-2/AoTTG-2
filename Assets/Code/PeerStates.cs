using System;

public enum PeerStates
{
    Uninitialized,
    PeerCreated,
    Queued,
    Authenticated,
    JoinedLobby,
    DisconnectingFromMasterserver,
    ConnectingToGameserver,
    ConnectedToGameserver,
    Joining,
    Joined,
    Leaving,
    DisconnectingFromGameserver,
    ConnectingToMasterserver,
    QueuedComingFromGameserver,
    Disconnecting,
    Disconnected,
    ConnectedToMaster,
    ConnectingToNameServer,
    ConnectedToNameServer,
    DisconnectingFromNameServer,
    Authenticating
}