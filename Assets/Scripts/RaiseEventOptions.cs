using ExitGames.Client.Photon.Lite;
using System;

public class RaiseEventOptions
{
    public int CacheSliceIndex;
    public EventCaching CachingOption;
    public static readonly RaiseEventOptions Default = new RaiseEventOptions();
    public bool ForwardToWebhook;
    public byte InterestGroup;
    public ReceiverGroup Receivers;
    public byte SequenceChannel;
    public int[] TargetActors;
}

