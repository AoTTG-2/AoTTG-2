using Photon;
using System;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Miscellaneous/Photon View &v")]
public class PhotonView : Photon.MonoBehaviour
{
    protected internal bool destroyedByPhotonNetworkOrQuit;
    protected internal bool didAwake;
    private bool failedToFindOnSerialize;
    public int group;
    private object[] instantiationDataField;
    public int instantiationId;
    protected internal object[] lastOnSerializeDataReceived;
    protected internal object[] lastOnSerializeDataSent;
    protected internal bool mixedModeIsReliable;
    public Component observed;
    private MethodInfo OnSerializeMethodInfo;
    public OnSerializeRigidBody onSerializeRigidBodyOption = OnSerializeRigidBody.All;
    public OnSerializeTransform onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;
    public int ownerId;
    public int prefixBackup = -1;
    public int subId;
    public ViewSynchronization synchronization;

    protected internal void Awake()
    {
        PhotonNetwork.networkingPeer.RegisterPhotonView(this);
        this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
        this.didAwake = true;
    }

    protected internal void ExecuteOnSerialize(PhotonStream pStream, PhotonMessageInfo info)
    {
        if (!this.failedToFindOnSerialize)
        {
            if ((this.OnSerializeMethodInfo == null) && !NetworkingPeer.GetMethod(this.observed as UnityEngine.MonoBehaviour, PhotonNetworkingMessage.OnPhotonSerializeView.ToString(), out this.OnSerializeMethodInfo))
            {
                Debug.LogError("The observed monobehaviour (" + this.observed.name + ") of this PhotonView does not implement OnPhotonSerializeView()!");
                this.failedToFindOnSerialize = true;
            }
            else
            {
                object[] parameters = new object[] { pStream, info };
                this.OnSerializeMethodInfo.Invoke(this.observed, parameters);
            }
        }
    }

    public static PhotonView Find(int viewID)
    {
        return PhotonNetwork.networkingPeer.GetPhotonView(viewID);
    }

    public static PhotonView Get(Component component)
    {
        return component.GetComponent<PhotonView>();
    }

    public static PhotonView Get(GameObject gameObj)
    {
        return gameObj.GetComponent<PhotonView>();
    }

    protected internal void OnApplicationQuit()
    {
        this.destroyedByPhotonNetworkOrQuit = true;
    }

    protected internal void OnDestroy()
    {
        if (!this.destroyedByPhotonNetworkOrQuit)
        {
            PhotonNetwork.networkingPeer.LocalCleanPhotonView(this);
        }
        if (!this.destroyedByPhotonNetworkOrQuit && !Application.isLoadingLevel)
        {
            if (this.instantiationId > 0)
            {
                Debug.LogError(string.Concat(new object[] { "OnDestroy() seems to be called without PhotonNetwork.Destroy()?! GameObject: ", base.gameObject, " Application.isLoadingLevel: ", Application.isLoadingLevel }));
            }
            else if (this.viewID <= 0)
            {
                Debug.LogWarning(string.Format("OnDestroy manually allocated PhotonView {0}. The viewID is 0. Was it ever (manually) set?", this));
            }
            else if (this.isMine && !PhotonNetwork.manuallyAllocatedViewIds.Contains(this.viewID))
            {
                Debug.LogWarning(string.Format("OnDestroy manually allocated PhotonView {0}. The viewID is local (isMine) but not in manuallyAllocatedViewIds list. Use UnAllocateViewID() after you destroyed the PV.", this));
            }
        }
        if (PhotonNetwork.networkingPeer.instantiatedObjects.ContainsKey(this.instantiationId))
        {
            bool flag;
            GameObject obj2 = PhotonNetwork.networkingPeer.instantiatedObjects[this.instantiationId];
            if (flag = obj2 == base.gameObject)
            {
                object[] args = new object[] { this, this.instantiationId, !Application.isLoadingLevel ? string.Empty : "Loading new scene caused this.", flag, this.destroyedByPhotonNetworkOrQuit };
                Debug.LogWarning(string.Format("OnDestroy for PhotonView {0} but GO is still in instantiatedObjects. instantiationId: {1}. Use PhotonNetwork.Destroy(). {2} Identical with this: {3} PN.Destroyed called for this PV: {4}", args));
            }
        }
    }

    public void RPC(string methodName, PhotonPlayer targetPlayer, params object[] parameters)
    {
        PhotonNetwork.RPC(this, methodName, targetPlayer, parameters);
    }

    public void RPC(string methodName, PhotonTargets target, params object[] parameters)
    {
        if (PhotonNetwork.networkingPeer.hasSwitchedMC && (target == PhotonTargets.MasterClient))
        {
            PhotonNetwork.RPC(this, methodName, PhotonNetwork.masterClient, parameters);
        }
        else
        {
            PhotonNetwork.RPC(this, methodName, target, parameters);
        }
    }

    public override string ToString()
    {
        object[] args = new object[] { this.viewID, (base.gameObject == null) ? "GO==null" : base.gameObject.name, !this.isSceneView ? string.Empty : "(scene)", this.prefix };
        return string.Format("View ({3}){0} on {1} {2}", args);
    }

    public object[] instantiationData
    {
        get
        {
            if (!this.didAwake)
            {
                this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
            }
            return this.instantiationDataField;
        }
        set
        {
            this.instantiationDataField = value;
        }
    }

    public bool isMine
    {
        get
        {
            return ((this.ownerId == PhotonNetwork.player.ID) || (this.isSceneView && PhotonNetwork.isMasterClient));
        }
    }

    public bool isSceneView
    {
        get
        {
            return (this.ownerId == 0);
        }
    }

    public PhotonPlayer owner
    {
        get
        {
            return PhotonPlayer.Find(this.ownerId);
        }
    }

    public int OwnerActorNr
    {
        get
        {
            return this.ownerId;
        }
    }

    public int prefix
    {
        get
        {
            if ((this.prefixBackup == -1) && (PhotonNetwork.networkingPeer != null))
            {
                this.prefixBackup = PhotonNetwork.networkingPeer.currentLevelPrefix;
            }
            return this.prefixBackup;
        }
        set
        {
            this.prefixBackup = value;
        }
    }

    public int viewID
    {
        get
        {
            return ((this.ownerId * PhotonNetwork.MAX_VIEW_IDS) + this.subId);
        }
        set
        {
            bool flag = this.didAwake && (this.subId == 0);
            this.ownerId = value / PhotonNetwork.MAX_VIEW_IDS;
            this.subId = value % PhotonNetwork.MAX_VIEW_IDS;
            if (flag)
            {
                PhotonNetwork.networkingPeer.RegisterPhotonView(this);
            }
        }
    }
}

