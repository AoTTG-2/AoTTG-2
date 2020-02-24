using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal class PhotonHandler : Photon.MonoBehaviour, IPhotonPeerListener
{
    public static bool AppQuits;
    internal static CloudRegionCode BestRegionCodeCurrently = CloudRegionCode.none;
    private int nextSendTickCount;
    private int nextSendTickCountOnSerialize;
    public static System.Type PingImplementation;
    private const string PlayerPrefsKey = "PUNCloudBestRegion";
    private static bool sendThreadShouldRun;
    public static PhotonHandler SP;
    public int updateInterval;
    public int updateIntervalOnSerialize;

    protected void Awake()
    {
        if (((SP != null) && (SP != this)) && (SP.gameObject != null))
        {
            UnityEngine.Object.DestroyImmediate(SP.gameObject);
        }
        SP = this;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        this.updateInterval = 0x3e8 / PhotonNetwork.sendRate;
        this.updateIntervalOnSerialize = 0x3e8 / PhotonNetwork.sendRateOnSerialize;
        StartFallbackSendAckThread();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {
            UnityEngine.Debug.LogError(message);
        }
        else if (level == DebugLevel.WARNING)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        else if ((level == DebugLevel.INFO) && (PhotonNetwork.logLevel >= PhotonLogLevel.Informational))
        {
            UnityEngine.Debug.Log(message);
        }
        else if ((level == DebugLevel.ALL) && (PhotonNetwork.logLevel == PhotonLogLevel.Full))
        {
            UnityEngine.Debug.Log(message);
        }
    }

    public static bool FallbackSendAckThread()
    {
        if (sendThreadShouldRun && (PhotonNetwork.networkingPeer != null))
        {
            PhotonNetwork.networkingPeer.SendAcksOnly();
        }
        return sendThreadShouldRun;
    }

    protected void OnApplicationQuit()
    {
        AppQuits = true;
        StopFallbackSendAckThread();
        PhotonNetwork.Disconnect();
    }

    protected void OnCreatedRoom()
    {
        PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(Application.loadedLevelName);
    }

    public void OnEvent(EventData photonEvent)
    {
    }

    protected void OnJoinedRoom()
    {
        PhotonNetwork.networkingPeer.LoadLevelIfSynced();
    }

    protected void OnLevelWasLoaded(int level)
    {
        PhotonNetwork.networkingPeer.NewSceneLoaded();
        PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(Application.loadedLevelName);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
    }

    protected internal static void PingAvailableRegionsAndConnectToBest()
    {
        SP.StartCoroutine(SP.PingAvailableRegionsCoroutine(true));
    }

    [DebuggerHidden]
    internal IEnumerator PingAvailableRegionsCoroutine(bool connectToBest)
    {
        return new PingAvailableRegionsCoroutinec__IteratorA { connectToBest = connectToBest };
    }

    public static void StartFallbackSendAckThread()
    {
        if (!sendThreadShouldRun)
        {
            sendThreadShouldRun = true;
            SupportClass.CallInBackground(new Func<bool>(PhotonHandler.FallbackSendAckThread));
        }
    }

    public static void StopFallbackSendAckThread()
    {
        sendThreadShouldRun = false;
    }

    protected void Update()
    {
        if (PhotonNetwork.networkingPeer == null)
        {
            UnityEngine.Debug.LogError("NetworkPeer broke!");
        }
        else if ((((PhotonNetwork.connectionStatesDetailed != PeerStates.PeerCreated) && (PhotonNetwork.connectionStatesDetailed != PeerStates.Disconnected)) && !PhotonNetwork.offlineMode) && PhotonNetwork.isMessageQueueRunning)
        {
            for (bool flag = true; PhotonNetwork.isMessageQueueRunning && flag; flag = PhotonNetwork.networkingPeer.DispatchIncomingCommands())
            {
            }
            int num = (int)(Time.realtimeSinceStartup * 1000f);
            if (PhotonNetwork.isMessageQueueRunning && (num > this.nextSendTickCountOnSerialize))
            {
                PhotonNetwork.networkingPeer.RunViewUpdate();
                this.nextSendTickCountOnSerialize = num + this.updateIntervalOnSerialize;
                this.nextSendTickCount = 0;
            }
            num = (int)(Time.realtimeSinceStartup * 1000f);
            if (num > this.nextSendTickCount)
            {
                for (bool flag2 = true; PhotonNetwork.isMessageQueueRunning && flag2; flag2 = PhotonNetwork.networkingPeer.SendOutgoingCommands())
                {
                }
                this.nextSendTickCount = num + this.updateInterval;
            }
        }
    }

    internal static CloudRegionCode BestRegionCodeInPreferences
    {
        get
        {
            string str = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
            if (!string.IsNullOrEmpty(str))
            {
                return Region.Parse(str);
            }
            return CloudRegionCode.none;
        }
        set
        {
            if (value == CloudRegionCode.none)
            {
                PlayerPrefs.DeleteKey("PUNCloudBestRegion");
            }
            else
            {
                PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
            }
        }
    }

    [CompilerGenerated]
    private sealed class PingAvailableRegionsCoroutinec__IteratorA : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal bool connectToBest;
        internal List<Region>.Enumerator Ss_89__1;
        internal Region best__3;
        internal PhotonPingManager pingManager__0;
        internal Region region__2;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint)this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    PhotonHandler.BestRegionCodeCurrently = CloudRegionCode.none;
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_01A4;

                default:
                    goto Label_0266;
            }
            if (PhotonNetwork.networkingPeer.AvailableRegions == null)
            {
                if ((PhotonNetwork.connectionStatesDetailed != PeerStates.ConnectingToNameServer) && (PhotonNetwork.connectionStatesDetailed != PeerStates.ConnectedToNameServer))
                {
                    UnityEngine.Debug.LogError("Call ConnectToNameServer to ping available regions.");
                    goto Label_0266;
                }
                UnityEngine.Debug.Log(string.Concat(new object[] { "Waiting for AvailableRegions. states: ", PhotonNetwork.connectionStatesDetailed, " Server: ", PhotonNetwork.Server, " PhotonNetwork.networkingPeer.AvailableRegions ", PhotonNetwork.networkingPeer.AvailableRegions != null }));
                this.Scurrent = new WaitForSeconds(0.25f);
                this.SPC = 1;
                goto Label_0268;
            }
            if ((PhotonNetwork.networkingPeer.AvailableRegions == null) || (PhotonNetwork.networkingPeer.AvailableRegions.Count == 0))
            {
                UnityEngine.Debug.LogError("No regions available. Are you sure your appid is valid and setup?");
                goto Label_0266;
            }
            this.pingManager__0 = new PhotonPingManager();
            this.Ss_89__1 = PhotonNetwork.networkingPeer.AvailableRegions.GetEnumerator();
            try
            {
                while (this.Ss_89__1.MoveNext())
                {
                    this.region__2 = this.Ss_89__1.Current;
                    PhotonHandler.SP.StartCoroutine(this.pingManager__0.PingSocket(this.region__2));
                }
            }
            finally
            {
                this.Ss_89__1.Dispose();
            }
        Label_01A4:
            while (!this.pingManager__0.Done)
            {
                this.Scurrent = new WaitForSeconds(0.1f);
                this.SPC = 2;
                goto Label_0268;
            }
            this.best__3 = this.pingManager__0.BestRegion;
            PhotonHandler.BestRegionCodeCurrently = this.best__3.Code;
            PhotonHandler.BestRegionCodeInPreferences = this.best__3.Code;
            UnityEngine.Debug.Log(string.Concat(new object[] { "Found best region: ", this.best__3.Code, " ping: ", this.best__3.Ping, ". Calling ConnectToRegionMaster() is: ", this.connectToBest }));
            if (this.connectToBest)
            {
                PhotonNetwork.networkingPeer.ConnectToRegionMaster(this.best__3.Code);
            }
            this.SPC = -1;
        Label_0266:
            return false;
        Label_0268:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }
}