namespace ExitGames.Client.Photon
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading;

    internal class SocketUdp : IPhotonSocket
    {
        private Socket sock;
        private readonly object syncer;

        public SocketUdp(PeerBase npeer) : base(npeer)
        {
            this.syncer = new object();
            if (base.ReportDebugOfLevel(DebugLevel.ALL))
            {
                base.Listener.DebugReturn(DebugLevel.ALL, "CSharpSocket: UDP, Unity3d.");
            }
            base.Protocol = ConnectionProtocol.Udp;
            base.PollReceive = false;
        }

        public override bool Connect()
        {
            object syncer = this.syncer;
            lock (syncer)
            {
                if (!base.Connect())
                {
                    return false;
                }
                base.State = PhotonSocketState.Connecting;
                new Thread(new ThreadStart(this.DnsAndConnect)) { Name = "photon dns thread", IsBackground = true }.Start();
                return true;
            }
        }

        public override bool Disconnect()
        {
            if (base.ReportDebugOfLevel(DebugLevel.INFO))
            {
                base.EnqueueDebugReturn(DebugLevel.INFO, "CSharpSocket.Disconnect()");
            }
            base.State = PhotonSocketState.Disconnecting;
            object syncer = this.syncer;
            lock (syncer)
            {
                if (this.sock != null)
                {
                    try
                    {
                        this.sock.Close();
                        this.sock = null;
                    }
                    catch (Exception exception)
                    {
                        base.EnqueueDebugReturn(DebugLevel.INFO, "Exception in Disconnect(): " + exception);
                    }
                }
            }
            base.State = PhotonSocketState.Disconnected;
            return true;
        }

        internal void DnsAndConnect()
        {
            try
            {
                object syncer = this.syncer;
                lock (syncer)
                {
                    this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPAddress ipAddress = IPhotonSocket.GetIpAddress(base.ServerAddress);
                    this.sock.Connect(ipAddress, base.ServerPort);
                    base.State = PhotonSocketState.Connected;
                }
            }
            catch (SecurityException exception)
            {
                if (base.ReportDebugOfLevel(DebugLevel.ERROR))
                {
                    base.Listener.DebugReturn(DebugLevel.ERROR, "Connect() failed: " + exception.ToString());
                }
                base.HandleException(StatusCode.SecurityExceptionOnConnect);
                return;
            }
            catch (Exception exception2)
            {
                if (base.ReportDebugOfLevel(DebugLevel.ERROR))
                {
                    base.Listener.DebugReturn(DebugLevel.ERROR, "Connect() failed: " + exception2.ToString());
                }
                base.HandleException(StatusCode.ExceptionOnConnect);
                return;
            }
            new Thread(new ThreadStart(this.ReceiveLoop)) { Name = "photon receive thread", IsBackground = true }.Start();
        }

        public override PhotonSocketError Receive(out byte[] data)
        {
            data = null;
            return PhotonSocketError.NoData;
        }

        public void ReceiveLoop()
        {
            byte[] buffer = new byte[base.MTU];
            while (base.State == PhotonSocketState.Connected)
            {
                try
                {
                    int length = this.sock.Receive(buffer);
                    base.HandleReceivedDatagram(buffer, length, true);
                    continue;
                }
                catch (Exception exception)
                {
                    if ((base.State != PhotonSocketState.Disconnecting) && (base.State != PhotonSocketState.Disconnected))
                    {
                        if (base.ReportDebugOfLevel(DebugLevel.ERROR))
                        {
                            base.EnqueueDebugReturn(DebugLevel.ERROR, string.Concat(new object[] { "Receive issue. State: ", base.State, " Exception: ", exception }));
                        }
                        base.HandleException(StatusCode.ExceptionOnReceive);
                    }
                    continue;
                }
            }
            this.Disconnect();
        }

        public override PhotonSocketError Send(byte[] data, int length)
        {
            object syncer = this.syncer;
            lock (syncer)
            {
                if (!this.sock.Connected)
                {
                    return PhotonSocketError.Skipped;
                }
                try
                {
                    this.sock.Send(data, 0, length, SocketFlags.None);
                }
                catch
                {
                    return PhotonSocketError.Exception;
                }
            }
            return PhotonSocketError.Success;
        }
    }
}

