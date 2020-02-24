using System;

internal class PunEvent
{
    public const byte AssignMaster = 0xd0;
    public const byte CloseConnection = 0xcb;
    public const byte Destroy = 0xcc;
    public const byte DestroyPlayer = 0xcf;
    public const byte Instantiation = 0xca;
    public const byte OwnershipRequest = 0xd1;
    public const byte OwnershipTransfer = 210;
    public const byte RemoveCachedRPCs = 0xcd;
    public const byte RPC = 200;
    public const byte SendSerialize = 0xc9;
    public const byte SendSerializeReliable = 0xce;
    public const byte VacantViewIds = 0xd3;
}

