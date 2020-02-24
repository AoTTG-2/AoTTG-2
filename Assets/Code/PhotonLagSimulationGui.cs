using ExitGames.Client.Photon;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhotonLagSimulationGui : MonoBehaviour
{
    public bool Visible = true;
    public int WindowId = 0x65;
    public Rect WindowRect = new Rect(0f, 100f, 120f, 100f);

    private void NetSimHasNoPeerWindow(int windowId)
    {
        GUILayout.Label("No peer to communicate with. ", new GUILayoutOption[0]);
    }

    private void NetSimWindow(int windowId)
    {
        bool flag;
        bool flag2;
        GUILayout.Label(string.Format("Rtt:{0,4} +/-{1,3}", this.Peer.RoundTripTime, this.Peer.RoundTripTimeVariance), new GUILayoutOption[0]);
        if ((flag2 = GUILayout.Toggle(flag = this.Peer.IsSimulationEnabled, "Simulate", new GUILayoutOption[0])) != flag)
        {
            this.Peer.IsSimulationEnabled = flag2;
        }
        float incomingLag = this.Peer.NetworkSimulationSettings.IncomingLag;
        GUILayout.Label("Lag " + incomingLag, new GUILayoutOption[0]);
        incomingLag = GUILayout.HorizontalSlider(incomingLag, 0f, 500f, new GUILayoutOption[0]);
        this.Peer.NetworkSimulationSettings.IncomingLag = (int) incomingLag;
        this.Peer.NetworkSimulationSettings.OutgoingLag = (int) incomingLag;
        float incomingJitter = this.Peer.NetworkSimulationSettings.IncomingJitter;
        GUILayout.Label("Jit " + incomingJitter, new GUILayoutOption[0]);
        incomingJitter = GUILayout.HorizontalSlider(incomingJitter, 0f, 100f, new GUILayoutOption[0]);
        this.Peer.NetworkSimulationSettings.IncomingJitter = (int) incomingJitter;
        this.Peer.NetworkSimulationSettings.OutgoingJitter = (int) incomingJitter;
        float incomingLossPercentage = this.Peer.NetworkSimulationSettings.IncomingLossPercentage;
        GUILayout.Label("Loss " + incomingLossPercentage, new GUILayoutOption[0]);
        incomingLossPercentage = GUILayout.HorizontalSlider(incomingLossPercentage, 0f, 10f, new GUILayoutOption[0]);
        this.Peer.NetworkSimulationSettings.IncomingLossPercentage = (int) incomingLossPercentage;
        this.Peer.NetworkSimulationSettings.OutgoingLossPercentage = (int) incomingLossPercentage;
        if (GUI.changed)
        {
            this.WindowRect.height = 100f;
        }
        GUI.DragWindow();
    }

    public void OnGUI()
    {
        if (this.Visible)
        {
            if (this.Peer == null)
            {
                this.WindowRect = GUILayout.Window(this.WindowId, this.WindowRect, new GUI.WindowFunction(this.NetSimHasNoPeerWindow), "Netw. Sim.", new GUILayoutOption[0]);
            }
            else
            {
                this.WindowRect = GUILayout.Window(this.WindowId, this.WindowRect, new GUI.WindowFunction(this.NetSimWindow), "Netw. Sim.", new GUILayoutOption[0]);
            }
        }
    }

    public void Start()
    {
        this.Peer = PhotonNetwork.networkingPeer;
    }

    public PhotonPeer Peer { get; set; }
}

