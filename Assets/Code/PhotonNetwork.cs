using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class PhotonNetwork
{
    private static bool _mAutomaticallySyncScene = false;
    private static bool autoJoinLobbyField = true;
    public static bool InstantiateInRoomOnly = true;
    private static bool isOfflineMode = false;
    internal static int lastUsedViewSubId = 0;
    internal static int lastUsedViewSubIdStatic = 0;
    public static PhotonLogLevel logLevel = PhotonLogLevel.ErrorsOnly;
    private static bool m_autoCleanUpPlayerObjects = true;
    private static bool m_isMessageQueueRunning = true;
    internal static List<int> manuallyAllocatedViewIds = new List<int>();
    public static readonly int MAX_VIEW_IDS = 0x3e8;
    internal static NetworkingPeer networkingPeer;
    private static Room offlineModeRoom = null;
    public static EventCallback OnEventCall;
    internal static readonly PhotonHandler photonMono;
    public static ServerSettings PhotonServerSettings = ((ServerSettings) Resources.Load("PhotonServerSettings", typeof(ServerSettings)));
    public static float precisionForFloatSynchronization = 0.01f;
    public static float precisionForQuaternionSynchronization = 1f;
    public static float precisionForVectorSynchronization = 9.9E-05f;
    public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();
    private static int sendInterval = 50;
    private static int sendIntervalOnSerialize = 100;
    public static HashSet<GameObject> SendMonoMessageTargets;
    public const string serverSettingsAssetFile = "PhotonServerSettings";
    public const string serverSettingsAssetPath = "Assets/Photon Unity Networking/Resources/PhotonServerSettings.asset";
    public static bool UseNameServer = true;
    public static bool UsePrefabCache = true;
    public const string versionPUN = "1.28";

    static PhotonNetwork()
    {
        Application.runInBackground = true;
        GameObject obj2 = new GameObject();
        photonMono = obj2.AddComponent<PhotonHandler>();
        obj2.name = "PhotonMono";
        obj2.hideFlags = HideFlags.HideInHierarchy;
        networkingPeer = new NetworkingPeer(photonMono, string.Empty, ConnectionProtocol.Udp);
        CustomTypes.Register();
    }

    private static int[] AllocateSceneViewIDs(int countOfNewViews)
    {
        int[] numArray = new int[countOfNewViews];
        for (int i = 0; i < countOfNewViews; i++)
        {
            numArray[i] = AllocateViewID(0);
        }
        return numArray;
    }

    public static int AllocateViewID()
    {
        int item = AllocateViewID(player.ID);
        manuallyAllocatedViewIds.Add(item);
        return item;
    }

    private static int AllocateViewID(int ownerId)
    {
        if (ownerId != 0)
        {
            int lastUsedViewSubId = PhotonNetwork.lastUsedViewSubId;
            int num6 = ownerId * MAX_VIEW_IDS;
            for (int j = 1; j < MAX_VIEW_IDS; j++)
            {
                lastUsedViewSubId = (lastUsedViewSubId + 1) % MAX_VIEW_IDS;
                if (lastUsedViewSubId != 0)
                {
                    int key = lastUsedViewSubId + num6;
                    if (!networkingPeer.photonViewList.ContainsKey(key) && !manuallyAllocatedViewIds.Contains(key))
                    {
                        PhotonNetwork.lastUsedViewSubId = lastUsedViewSubId;
                        return key;
                    }
                }
            }
            throw new Exception(string.Format("AllocateViewID() failed. User {0} is out of subIds, as all viewIDs are used.", ownerId));
        }
        int lastUsedViewSubIdStatic = PhotonNetwork.lastUsedViewSubIdStatic;
        int num2 = ownerId * MAX_VIEW_IDS;
        for (int i = 1; i < MAX_VIEW_IDS; i++)
        {
            lastUsedViewSubIdStatic = (lastUsedViewSubIdStatic + 1) % MAX_VIEW_IDS;
            if (lastUsedViewSubIdStatic != 0)
            {
                int num4 = lastUsedViewSubIdStatic + num2;
                if (!networkingPeer.photonViewList.ContainsKey(num4))
                {
                    PhotonNetwork.lastUsedViewSubIdStatic = lastUsedViewSubIdStatic;
                    return num4;
                }
            }
        }
        throw new Exception(string.Format("AllocateViewID() failed. Room (user {0}) is out of subIds, as all room viewIDs are used.", ownerId));
    }

    public static bool CloseConnection(PhotonPlayer kickPlayer)
    {
        if (!VerifyCanUseNetwork())
        {
            return false;
        }
        if (!player.isMasterClient)
        {
            Debug.LogError("CloseConnection: Only the masterclient can kick another player.");
            return false;
        }
        if (kickPlayer == null)
        {
            Debug.LogError("CloseConnection: No such player connected!");
            return false;
        }
        RaiseEventOptions options = new RaiseEventOptions();
        options.TargetActors = new int[] { kickPlayer.ID };
        RaiseEventOptions raiseEventOptions = options;
        return networkingPeer.OpRaiseEvent(0xcb, null, true, raiseEventOptions);
    }

    public static bool ConnectToBestCloudServer(string gameVersion)
    {
        if (PhotonServerSettings == null)
        {
            Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
            return false;
        }
        if (PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
        {
            return ConnectUsingSettings(gameVersion);
        }
        networkingPeer.IsInitialConnect = true;
        networkingPeer.SetApp(PhotonServerSettings.AppID, gameVersion);
        CloudRegionCode bestRegionCodeInPreferences = PhotonHandler.BestRegionCodeInPreferences;
        if (bestRegionCodeInPreferences != CloudRegionCode.none)
        {
            Debug.Log("Best region found in PlayerPrefs. Connecting to: " + bestRegionCodeInPreferences);
            return networkingPeer.ConnectToRegionMaster(bestRegionCodeInPreferences);
        }
        return networkingPeer.ConnectToNameServer();
    }

    public static bool ConnectToMaster(string masterServerAddress, int port, string appID, string gameVersion)
    {
        if (networkingPeer.PeerState != PeerStateValue.Disconnected)
        {
            Debug.LogWarning("ConnectToMaster() failed. Can only connect while in state 'Disconnected'. Current state: " + networkingPeer.PeerState);
            return false;
        }
        if (offlineMode)
        {
            offlineMode = false;
            Debug.LogWarning("ConnectToMaster() disabled the offline mode. No longer offline.");
        }
        if (!isMessageQueueRunning)
        {
            isMessageQueueRunning = true;
            Debug.LogWarning("ConnectToMaster() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
        }
        networkingPeer.SetApp(appID, gameVersion);
        networkingPeer.IsUsingNameServer = false;
        networkingPeer.IsInitialConnect = true;
        networkingPeer.MasterServerAddress = masterServerAddress + ":" + port;
        return networkingPeer.Connect(networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
    }

    public static bool ConnectUsingSettings(string gameVersion)
    {
        if (PhotonServerSettings == null)
        {
            Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
            return false;
        }
        SwitchToProtocol(PhotonServerSettings.Protocol);
        networkingPeer.SetApp(PhotonServerSettings.AppID, gameVersion);
        if (PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
        {
            offlineMode = true;
            return true;
        }
        if (offlineMode)
        {
            Debug.LogWarning("ConnectUsingSettings() disabled the offline mode. No longer offline.");
        }
        offlineMode = false;
        isMessageQueueRunning = true;
        networkingPeer.IsInitialConnect = true;
        if (PhotonServerSettings.HostType == ServerSettings.HostingOption.SelfHosted)
        {
            networkingPeer.IsUsingNameServer = false;
            networkingPeer.MasterServerAddress = PhotonServerSettings.ServerAddress + ":" + PhotonServerSettings.ServerPort;
            return networkingPeer.Connect(networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
        }
        if (PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
        {
            return ConnectToBestCloudServer(gameVersion);
        }
        return networkingPeer.ConnectToRegionMaster(PhotonServerSettings.PreferredRegion);
    }

    public static bool CreateRoom(string roomName)
    {
        return CreateRoom(roomName, null, null);
    }

    public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
    {
        if (offlineMode)
        {
            if (offlineModeRoom != null)
            {
                Debug.LogError("CreateRoom failed. In offline mode you still have to leave a room to enter another.");
                return false;
            }
            offlineModeRoom = new Room(roomName, roomOptions);
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
            return true;
        }
        if ((networkingPeer.server == ServerConnection.MasterServer) && connectedAndReady)
        {
            return networkingPeer.OpCreateGame(roomName, roomOptions, typedLobby);
        }
        Debug.LogError("CreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
        return false;
    }

    public static bool CreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers)
    {
        RoomOptions roomOptions = new RoomOptions {
            isVisible = isVisible,
            isOpen = isOpen,
            maxPlayers = maxPlayers
        };
        return CreateRoom(roomName, roomOptions, null);
    }

    [Obsolete("Use overload with RoomOptions and TypedLobby parameters.")]
    public static bool CreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers, Hashtable customRoomProperties, string[] propsToListInLobby)
    {
        RoomOptions roomOptions = new RoomOptions {
            isVisible = isVisible,
            isOpen = isOpen,
            maxPlayers = maxPlayers,
            customRoomProperties = customRoomProperties,
            customRoomPropertiesForLobby = propsToListInLobby
        };
        return CreateRoom(roomName, roomOptions, null);
    }

    public static void Destroy(PhotonView targetView)
    {
        if (targetView != null)
        {
            networkingPeer.RemoveInstantiatedGO(targetView.gameObject, !inRoom);
        }
        else
        {
            Debug.LogError("Destroy(targetPhotonView) failed, cause targetPhotonView is null.");
        }
    }

    public static void Destroy(GameObject targetGo)
    {
        networkingPeer.RemoveInstantiatedGO(targetGo, !inRoom);
    }

    public static void DestroyAll()
    {
        if (isMasterClient)
        {
            networkingPeer.DestroyAll(false);
        }
        else
        {
            Debug.LogError("Couldn't call DestroyAll() as only the master client is allowed to call this.");
        }
    }

    public static void DestroyPlayerObjects(PhotonPlayer targetPlayer)
    {
        if (player == null)
        {
            Debug.LogError("DestroyPlayerObjects() failed, cause parameter 'targetPlayer' was null.");
        }
        DestroyPlayerObjects(targetPlayer.ID);
    }

    public static void DestroyPlayerObjects(int targetPlayerId)
    {
        if (VerifyCanUseNetwork())
        {
            if (!player.isMasterClient && (targetPlayerId != player.ID))
            {
                Debug.LogError("DestroyPlayerObjects() failed, cause players can only destroy their own GameObjects. A Master Client can destroy anyone's. This is master: " + isMasterClient);
            }
            else
            {
                networkingPeer.DestroyPlayerObjects(targetPlayerId, false);
            }
        }
    }

    public static void Disconnect()
    {
        if (offlineMode)
        {
            offlineMode = false;
            offlineModeRoom = null;
            networkingPeer.states = PeerStates.Disconnecting;
            networkingPeer.OnStatusChanged(StatusCode.Disconnect);
        }
        else if (networkingPeer != null)
        {
            networkingPeer.Disconnect();
        }
    }

    public static void FetchServerTimestamp()
    {
        if (networkingPeer != null)
        {
            networkingPeer.FetchServerTimestamp();
        }
    }

    public static bool FindFriends(string[] friendsToFind)
    {
        return (((networkingPeer != null) && !isOfflineMode) && networkingPeer.OpFindFriends(friendsToFind));
    }

    public static int GetPing()
    {
        return networkingPeer.RoundTripTime;
    }

    public static RoomInfo[] GetRoomList()
    {
        if (!offlineMode && (networkingPeer != null))
        {
            return networkingPeer.mGameListCopy;
        }
        return new RoomInfo[0];
    }

    [Obsolete("Used for compatibility with Unity networking only. Encryption is automatically initialized while connecting.")]
    public static void InitializeSecurity()
    {
    }

    public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group)
    {
        return Instantiate(prefabName, position, rotation, group, null);
    }

    public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
    {
        if (connected && (!InstantiateInRoomOnly || inRoom))
        {
            GameObject obj2;
            if (!UsePrefabCache || !PrefabCache.TryGetValue(prefabName, out obj2))
            {
                if (prefabName.StartsWith("RCAsset/"))
                {
                    obj2 = FengGameManagerMKII.InstantiateCustomAsset(prefabName);
                }
                else
                {
                    obj2 = (GameObject) Resources.Load(prefabName, typeof(GameObject));
                }
                if (UsePrefabCache)
                {
                    PrefabCache.Add(prefabName, obj2);
                }
            }
            if (obj2 == null)
            {
                Debug.LogError("Failed to Instantiate prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)");
                return null;
            }
            if (obj2.GetComponent<PhotonView>() == null)
            {
                Debug.LogError("Failed to Instantiate prefab:" + prefabName + ". Prefab must have a PhotonView component.");
                return null;
            }
            int[] viewIDs = new int[obj2.GetPhotonViewsInChildren().Length];
            for (int i = 0; i < viewIDs.Length; i++)
            {
                viewIDs[i] = AllocateViewID(player.ID);
            }
            Hashtable evData = networkingPeer.SendInstantiate(prefabName, position, rotation, group, viewIDs, data, false);
            return networkingPeer.DoInstantiate2(evData, networkingPeer.mLocalActor, obj2);
        }
        Debug.LogError(string.Concat(new object[] { "Failed to Instantiate prefab: ", prefabName, ". Client should be in a room. Current connectionStateDetailed: ", connectionStatesDetailed }));
        return null;
    }

    public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
    {
        if (connected && (!InstantiateInRoomOnly || inRoom))
        {
            GameObject obj2;
            if (!isMasterClient)
            {
                Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". Client is not the MasterClient in this room.");
                return null;
            }
            if (!UsePrefabCache || !PrefabCache.TryGetValue(prefabName, out obj2))
            {
                obj2 = (GameObject) Resources.Load(prefabName, typeof(GameObject));
                if (UsePrefabCache)
                {
                    PrefabCache.Add(prefabName, obj2);
                }
            }
            if (obj2 == null)
            {
                Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)");
                return null;
            }
            if (obj2.GetComponent<PhotonView>() == null)
            {
                Debug.LogError("Failed to InstantiateSceneObject prefab:" + prefabName + ". Prefab must have a PhotonView component.");
                return null;
            }
            int[] viewIDs = AllocateSceneViewIDs(obj2.GetPhotonViewsInChildren().Length);
            if (viewIDs == null)
            {
                Debug.LogError(string.Concat(new object[] { "Failed to InstantiateSceneObject prefab: ", prefabName, ". No ViewIDs are free to use. Max is: ", MAX_VIEW_IDS }));
                return null;
            }
            Hashtable evData = networkingPeer.SendInstantiate(prefabName, position, rotation, group, viewIDs, data, true);
            return networkingPeer.DoInstantiate2(evData, networkingPeer.mLocalActor, obj2);
        }
        Debug.LogError(string.Concat(new object[] { "Failed to InstantiateSceneObject prefab: ", prefabName, ". Client should be in a room. Current connectionStateDetailed: ", connectionStatesDetailed }));
        return null;
    }

    public static void InternalCleanPhotonMonoFromSceneIfStuck()
    {
        PhotonHandler[] handlerArray = UnityEngine.Object.FindObjectsOfType(typeof(PhotonHandler)) as PhotonHandler[];
        if ((handlerArray != null) && (handlerArray.Length > 0))
        {
            Debug.Log("Cleaning up hidden PhotonHandler instances in scene. Please save it. This is not an issue.");
            foreach (PhotonHandler handler in handlerArray)
            {
                handler.gameObject.hideFlags = HideFlags.None;
                if ((handler.gameObject != null) && (handler.gameObject.name == "PhotonMono"))
                {
                    UnityEngine.Object.DestroyImmediate(handler.gameObject);
                }
                UnityEngine.Object.DestroyImmediate(handler);
            }
        }
    }

    public static bool JoinLobby()
    {
        return JoinLobby(null);
    }

    public static bool JoinLobby(TypedLobby typedLobby)
    {
        bool flag;
        if (!connected || (Server != ServerConnection.MasterServer))
        {
            return false;
        }
        if (typedLobby == null)
        {
            typedLobby = TypedLobby.Default;
        }
        if (flag = networkingPeer.OpJoinLobby(typedLobby))
        {
            networkingPeer.lobby = typedLobby;
        }
        return flag;
    }

    public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
    {
        if (offlineMode)
        {
            if (offlineModeRoom != null)
            {
                Debug.LogError("JoinOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
                return false;
            }
            offlineModeRoom = new Room(roomName, roomOptions);
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
            return true;
        }
        if ((networkingPeer.server == ServerConnection.MasterServer) && connectedAndReady)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                Debug.LogError("JoinOrCreateRoom failed. A roomname is required. If you don't know one, how will you join?");
                return false;
            }
            return networkingPeer.OpJoinRoom(roomName, roomOptions, typedLobby, true);
        }
        Debug.LogError("JoinOrCreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
        return false;
    }

    public static bool JoinRandomRoom()
    {
        return JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, null, null);
    }

    public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers)
    {
        return JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom, null, null);
    }

    public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter)
    {
        if (offlineMode)
        {
            if (offlineModeRoom != null)
            {
                Debug.LogError("JoinRandomRoom failed. In offline mode you still have to leave a room to enter another.");
                return false;
            }
            offlineModeRoom = new Room("offline room", null);
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
            return true;
        }
        if ((networkingPeer.server == ServerConnection.MasterServer) && connectedAndReady)
        {
            Hashtable target = new Hashtable();
            target.MergeStringKeys(expectedCustomRoomProperties);
            if (expectedMaxPlayers > 0)
            {
                target[(byte) 0xff] = expectedMaxPlayers;
            }
            return networkingPeer.OpJoinRandomRoom(target, 0, null, matchingType, typedLobby, sqlLobbyFilter);
        }
        Debug.LogError("JoinRandomRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
        return false;
    }

    public static bool JoinRoom(string roomName)
    {
        if (offlineMode)
        {
            if (offlineModeRoom != null)
            {
                Debug.LogError("JoinRoom failed. In offline mode you still have to leave a room to enter another.");
                return false;
            }
            offlineModeRoom = new Room(roomName, null);
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
            return true;
        }
        if ((networkingPeer.server == ServerConnection.MasterServer) && connectedAndReady)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                Debug.LogError("JoinRoom failed. A roomname is required. If you don't know one, how will you join?");
                return false;
            }
            return networkingPeer.OpJoinRoom(roomName, null, null, false);
        }
        Debug.LogError("JoinRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
        return false;
    }

    [Obsolete("Use overload with roomOptions and TypedLobby parameter.")]
    public static bool JoinRoom(string roomName, bool createIfNotExists)
    {
        if (((connectionStatesDetailed != PeerStates.Joining) && (connectionStatesDetailed != PeerStates.Joined)) && (connectionStatesDetailed != PeerStates.ConnectedToGameserver))
        {
            if (room == null)
            {
                if (roomName != string.Empty)
                {
                    if (offlineMode)
                    {
                        offlineModeRoom = new Room(roomName, null);
                        NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
                        return true;
                    }
                    return networkingPeer.OpJoinRoom(roomName, null, null, createIfNotExists);
                }
                Debug.LogError("JoinRoom aborted: You must specifiy a room name!");
            }
            else
            {
                Debug.LogError("JoinRoom aborted: You are already in a room!");
            }
        }
        else
        {
            Debug.LogError("JoinRoom aborted: You can only join a room while not currently connected/connecting to a room.");
        }
        return false;
    }

    public static bool LeaveLobby()
    {
        return ((connected && (Server == ServerConnection.MasterServer)) && networkingPeer.OpLeaveLobby());
    }

    public static bool LeaveRoom()
    {
        if (offlineMode)
        {
            offlineModeRoom = null;
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom, new object[0]);
            return true;
        }
        if (room == null)
        {
            Debug.LogWarning("PhotonNetwork.room is null. You don't have to call LeaveRoom() when you're not in one. State: " + connectionStatesDetailed);
        }
        return networkingPeer.OpLeave();
    }

    public static void LoadLevel(int levelNumber)
    {
        networkingPeer.SetLevelInPropsIfSynced(levelNumber);
        isMessageQueueRunning = false;
        networkingPeer.loadingLevelAndPausedNetwork = true;
        Application.LoadLevel(levelNumber);
    }

    public static void LoadLevel(string levelName)
    {
        networkingPeer.SetLevelInPropsIfSynced(levelName);
        isMessageQueueRunning = false;
        networkingPeer.loadingLevelAndPausedNetwork = true;
        Application.LoadLevel(levelName);
    }

    public static void NetworkStatisticsReset()
    {
        networkingPeer.TrafficStatsReset();
    }

    public static string NetworkStatisticsToString()
    {
        if ((networkingPeer != null) && !offlineMode)
        {
            return networkingPeer.VitalStatsToString(false);
        }
        return "Offline or in OfflineMode. No VitalStats available.";
    }

    public static void OverrideBestCloudServer(CloudRegionCode region)
    {
        PhotonHandler.BestRegionCodeInPreferences = region;
    }

    public static bool RaiseEvent(byte eventCode, object eventContent, bool sendReliable, RaiseEventOptions options)
    {
        if (inRoom && (eventCode < 0xff))
        {
            return networkingPeer.OpRaiseEvent(eventCode, eventContent, sendReliable, options);
        }
        Debug.LogWarning("RaiseEvent() failed. Your event is not being sent! Check if your are in a Room and the eventCode must be less than 200 (0..199).");
        return false;
    }

    public static void RefreshCloudServerRating()
    {
        throw new NotImplementedException("not available at the moment");
    }

    public static void RemoveRPCs(PhotonPlayer targetPlayer)
    {
        if (VerifyCanUseNetwork())
        {
            if (!targetPlayer.isLocal && !isMasterClient)
            {
                Debug.LogError("Error; Only the MasterClient can call RemoveRPCs for other players.");
            }
            else
            {
                networkingPeer.OpCleanRpcBuffer(targetPlayer.ID);
            }
        }
    }

    public static void RemoveRPCs(PhotonView targetPhotonView)
    {
        if (VerifyCanUseNetwork())
        {
            networkingPeer.CleanRpcBufferIfMine(targetPhotonView);
        }
    }

    public static void RemoveRPCsInGroup(int targetGroup)
    {
        if (VerifyCanUseNetwork())
        {
            networkingPeer.RemoveRPCsInGroup(targetGroup);
        }
    }

    internal static void RPC(PhotonView view, string methodName, PhotonPlayer targetPlayer, params object[] parameters)
    {
        if (VerifyCanUseNetwork())
        {
            if (room == null)
            {
                Debug.LogWarning("Cannot send RPCs in Lobby, only processed locally");
            }
            else
            {
                if (player == null)
                {
                    Debug.LogError("Error; Sending RPC to player null! Aborted \"" + methodName + "\"");
                }
                if (networkingPeer != null)
                {
                    networkingPeer.RPC(view, methodName, targetPlayer, parameters);
                }
                else
                {
                    Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
                }
            }
        }
    }

    internal static void RPC(PhotonView view, string methodName, PhotonTargets target, params object[] parameters)
    {
        if (VerifyCanUseNetwork())
        {
            if (room == null)
            {
                Debug.LogWarning("Cannot send RPCs in Lobby! RPC dropped.");
            }
            else if (networkingPeer != null)
            {
                networkingPeer.RPC(view, methodName, target, parameters);
            }
            else
            {
                Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
            }
        }
    }

    public static void SendOutgoingCommands()
    {
        if (VerifyCanUseNetwork())
        {
            while (networkingPeer.SendOutgoingCommands())
            {
            }
        }
    }

    public static void SetLevelPrefix(short prefix)
    {
        if (VerifyCanUseNetwork())
        {
            networkingPeer.SetLevelPrefix(prefix);
        }
    }

    public static bool SetMasterClient(PhotonPlayer masterClientPlayer)
    {
        return networkingPeer.SetMasterClient(masterClientPlayer.ID, true);
    }

    public static void SetPlayerCustomProperties(Hashtable customProperties)
    {
        if (customProperties == null)
        {
            customProperties = new Hashtable();
            foreach (object obj2 in player.customProperties.Keys)
            {
                customProperties[(string) obj2] = null;
            }
        }
        if ((room != null) && room.isLocalClientInside)
        {
            player.SetCustomProperties(customProperties);
        }
        else
        {
            player.InternalCacheProperties(customProperties);
        }
    }

    public static void SetReceivingEnabled(int group, bool enabled)
    {
        if (VerifyCanUseNetwork())
        {
            networkingPeer.SetReceivingEnabled(group, enabled);
        }
    }

    public static void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
    {
        if (VerifyCanUseNetwork())
        {
            networkingPeer.SetReceivingEnabled(enableGroups, disableGroups);
        }
    }

    public static void SetSendingEnabled(int group, bool enabled)
    {
        if (VerifyCanUseNetwork())
        {
            networkingPeer.SetSendingEnabled(group, enabled);
        }
    }

    public static void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
    {
        if (VerifyCanUseNetwork())
        {
            networkingPeer.SetSendingEnabled(enableGroups, disableGroups);
        }
    }

    public static void SwitchToProtocol(ConnectionProtocol cp)
    {
        if (networkingPeer.UsedProtocol != cp)
        {
            try
            {
                networkingPeer.Disconnect();
                networkingPeer.StopThread();
            }
            catch
            {
            }
            networkingPeer = new NetworkingPeer(photonMono, string.Empty, cp);
            Debug.Log("Protocol switched to: " + cp);
        }
    }

    public static void UnAllocateViewID(int viewID)
    {
        manuallyAllocatedViewIds.Remove(viewID);
        if (networkingPeer.photonViewList.ContainsKey(viewID))
        {
            Debug.LogWarning(string.Format("Unallocated manually used viewID: {0} but found it used still in a PhotonView: {1}", viewID, networkingPeer.photonViewList[viewID]));
        }
    }

    private static bool VerifyCanUseNetwork()
    {
        if (connected)
        {
            return true;
        }
        Debug.LogError("Cannot send messages when not connected. Either connect to Photon OR use offline mode!");
        return false;
    }

    public static bool WebRpc(string name, object parameters)
    {
        return networkingPeer.WebRpc(name, parameters);
    }

    public static AuthenticationValues AuthValues
    {
        get
        {
            return ((networkingPeer == null) ? null : networkingPeer.CustomAuthenticationValues);
        }
        set
        {
            if (networkingPeer != null)
            {
                networkingPeer.CustomAuthenticationValues = value;
            }
        }
    }

    public static bool autoCleanUpPlayerObjects
    {
        get
        {
            return m_autoCleanUpPlayerObjects;
        }
        set
        {
            if (room != null)
            {
                Debug.LogError("Setting autoCleanUpPlayerObjects while in a room is not supported.");
            }
            else
            {
                m_autoCleanUpPlayerObjects = value;
            }
        }
    }

    public static bool autoJoinLobby
    {
        get
        {
            return autoJoinLobbyField;
        }
        set
        {
            autoJoinLobbyField = value;
        }
    }

    public static bool automaticallySyncScene
    {
        get
        {
            return _mAutomaticallySyncScene;
        }
        set
        {
            _mAutomaticallySyncScene = value;
            if (_mAutomaticallySyncScene && (room != null))
            {
                networkingPeer.LoadLevelIfSynced();
            }
        }
    }

    public static bool connected
    {
        get
        {
            if (offlineMode)
            {
                return true;
            }
            if (networkingPeer == null)
            {
                return false;
            }
            return (((!networkingPeer.IsInitialConnect && (networkingPeer.states != PeerStates.PeerCreated)) && ((networkingPeer.states != PeerStates.Disconnected) && (networkingPeer.states != PeerStates.Disconnecting))) && (networkingPeer.states != PeerStates.ConnectingToNameServer));
        }
    }

    public static bool connectedAndReady
    {
        get
        {
            if (!connected)
            {
                return false;
            }
            if (!offlineMode)
            {
                switch (connectionStatesDetailed)
                {
                    case PeerStates.ConnectingToGameserver:
                    case PeerStates.Joining:
                    case PeerStates.Leaving:
                    case PeerStates.ConnectingToMasterserver:
                    case PeerStates.Disconnecting:
                    case PeerStates.Disconnected:
                    case PeerStates.ConnectingToNameServer:
                    case PeerStates.Authenticating:
                    case PeerStates.PeerCreated:
                        return false;
                }
            }
            return true;
        }
    }

    public static bool connecting
    {
        get
        {
            return (networkingPeer.IsInitialConnect && !offlineMode);
        }
    }

    public static ConnectionState connectionState
    {
        get
        {
            if (offlineMode)
            {
                return ConnectionState.Connected;
            }
            if (networkingPeer == null)
            {
                return ConnectionState.Disconnected;
            }
            PeerStateValue peerState = networkingPeer.PeerState;
            switch (peerState)
            {
                case PeerStateValue.Disconnected:
                    return ConnectionState.Disconnected;

                case PeerStateValue.Connecting:
                    return ConnectionState.Connecting;

                case PeerStateValue.Connected:
                    return ConnectionState.Connected;

                case PeerStateValue.Disconnecting:
                    return ConnectionState.Disconnecting;
            }
            if (peerState != PeerStateValue.InitializingApplication)
            {
                return ConnectionState.Disconnected;
            }
            return ConnectionState.InitializingApplication;
        }
    }

    public static PeerStates connectionStatesDetailed
    {
        get
        {
            if (offlineMode)
            {
                return ((offlineModeRoom == null) ? PeerStates.ConnectedToMaster : PeerStates.Joined);
            }
            if (networkingPeer == null)
            {
                return PeerStates.Disconnected;
            }
            return networkingPeer.states;
        }
    }

    public static int countOfPlayers
    {
        get
        {
            return (networkingPeer.mPlayersInRoomsCount + networkingPeer.mPlayersOnMasterCount);
        }
    }

    public static int countOfPlayersInRooms
    {
        get
        {
            return networkingPeer.mPlayersInRoomsCount;
        }
    }

    public static int countOfPlayersOnMaster
    {
        get
        {
            return networkingPeer.mPlayersOnMasterCount;
        }
    }

    public static int countOfRooms
    {
        get
        {
            return networkingPeer.mGameCount;
        }
    }

    public static bool CrcCheckEnabled
    {
        get
        {
            return networkingPeer.CrcEnabled;
        }
        set
        {
            if (!connected && !connecting)
            {
                networkingPeer.CrcEnabled = value;
            }
            else
            {
                Debug.Log("Can't change CrcCheckEnabled while being connected. CrcCheckEnabled stays " + networkingPeer.CrcEnabled);
            }
        }
    }

    public static int FriendsListAge
    {
        get
        {
            return ((networkingPeer == null) ? 0 : networkingPeer.FriendsListAge);
        }
    }

    public static string gameVersion
    {
        get
        {
            return networkingPeer.mAppVersion;
        }
        set
        {
            networkingPeer.mAppVersion = value;
        }
    }

    public static bool inRoom
    {
        get
        {
            return (connectionStatesDetailed == PeerStates.Joined);
        }
    }

    public static bool insideLobby
    {
        get
        {
            return networkingPeer.insideLobby;
        }
    }

    public static bool isMasterClient
    {
        get
        {
            return (offlineMode || (networkingPeer.mMasterClient == networkingPeer.mLocalActor));
        }
    }

    public static bool isMessageQueueRunning
    {
        get
        {
            return m_isMessageQueueRunning;
        }
        set
        {
            if (value)
            {
                PhotonHandler.StartFallbackSendAckThread();
            }
            networkingPeer.IsSendingOnlyAcks = !value;
            m_isMessageQueueRunning = value;
        }
    }

    public static bool isNonMasterClientInRoom
    {
        get
        {
            return (!isMasterClient && (room != null));
        }
    }

    public static TypedLobby lobby
    {
        get
        {
            return networkingPeer.lobby;
        }
        set
        {
            networkingPeer.lobby = value;
        }
    }

    public static PhotonPlayer masterClient
    {
        get
        {
            if (networkingPeer == null)
            {
                return null;
            }
            return networkingPeer.mMasterClient;
        }
    }

    [Obsolete("Used for compatibility with Unity networking only.")]
    public static int maxConnections
    {
        get
        {
            if (room == null)
            {
                return 0;
            }
            return room.maxPlayers;
        }
        set
        {
            room.maxPlayers = value;
        }
    }

    public static int MaxResendsBeforeDisconnect
    {
        get
        {
            return networkingPeer.SentCountAllowance;
        }
        set
        {
            if (value < 3)
            {
                value = 3;
            }
            if (value > 10)
            {
                value = 10;
            }
            networkingPeer.SentCountAllowance = value;
        }
    }

    public static bool NetworkStatisticsEnabled
    {
        get
        {
            return networkingPeer.TrafficStatsEnabled;
        }
        set
        {
            networkingPeer.TrafficStatsEnabled = value;
        }
    }

    public static bool offlineMode
    {
        get
        {
            return isOfflineMode;
        }
        set
        {
            if (value != isOfflineMode)
            {
                if (value && connected)
                {
                    Debug.LogError("Can't start OFFLINE mode while connected!");
                }
                else
                {
                    if (networkingPeer.PeerState != PeerStateValue.Disconnected)
                    {
                        networkingPeer.Disconnect();
                    }
                    isOfflineMode = value;
                    if (isOfflineMode)
                    {
                        NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster, new object[0]);
                        networkingPeer.ChangeLocalID(1);
                        networkingPeer.mMasterClient = player;
                    }
                    else
                    {
                        offlineModeRoom = null;
                        networkingPeer.ChangeLocalID(-1);
                        networkingPeer.mMasterClient = null;
                    }
                }
            }
        }
    }

    public static PhotonPlayer[] otherPlayers
    {
        get
        {
            if (networkingPeer == null)
            {
                return new PhotonPlayer[0];
            }
            return networkingPeer.mOtherPlayerListCopy;
        }
    }

    public static int PacketLossByCrcCheck
    {
        get
        {
            return networkingPeer.PacketLossByCrc;
        }
    }

    public static PhotonPlayer player
    {
        get
        {
            if (networkingPeer == null)
            {
                return null;
            }
            return networkingPeer.mLocalActor;
        }
    }

    public static PhotonPlayer[] playerList
    {
        get
        {
            if (networkingPeer == null)
            {
                return new PhotonPlayer[0];
            }
            return networkingPeer.mPlayerListCopy;
        }
    }

    public static string playerName
    {
        get
        {
            return networkingPeer.PlayerName;
        }
        set
        {
            networkingPeer.PlayerName = value;
        }
    }

    public static int ResentReliableCommands
    {
        get
        {
            return networkingPeer.ResentReliableCommands;
        }
    }

    public static Room room
    {
        get
        {
            if (isOfflineMode)
            {
                return offlineModeRoom;
            }
            return networkingPeer.mCurrentGame;
        }
    }

    public static int sendRate
    {
        get
        {
            return (0x3e8 / sendInterval);
        }
        set
        {
            sendInterval = 0x3e8 / value;
            if (photonMono != null)
            {
                photonMono.updateInterval = sendInterval;
            }
            if (value < sendRateOnSerialize)
            {
                sendRateOnSerialize = value;
            }
        }
    }

    public static int sendRateOnSerialize
    {
        get
        {
            return (0x3e8 / sendIntervalOnSerialize);
        }
        set
        {
            if (value > sendRate)
            {
                Debug.LogError("Error, can not set the OnSerialize SendRate more often then the overall SendRate");
                value = sendRate;
            }
            sendIntervalOnSerialize = 0x3e8 / value;
            if (photonMono != null)
            {
                photonMono.updateIntervalOnSerialize = sendIntervalOnSerialize;
            }
        }
    }

    public static ServerConnection Server
    {
        get
        {
            return networkingPeer.server;
        }
    }

    public static string ServerAddress
    {
        get
        {
            return ((networkingPeer == null) ? "<not connected>" : networkingPeer.ServerAddress);
        }
    }

    public static double time
    {
        get
        {
            if (offlineMode)
            {
                return (double) Time.time;
            }
            return (((double) networkingPeer.ServerTimeInMilliSeconds) / 1000.0);
        }
    }

    public static int unreliableCommandsLimit
    {
        get
        {
            return networkingPeer.LimitOfUnreliableCommands;
        }
        set
        {
            networkingPeer.LimitOfUnreliableCommands = value;
        }
    }

    public delegate void EventCallback(byte eventCode, object content, int senderId);

    public static List<FriendInfo> Friends { get; set; }
}