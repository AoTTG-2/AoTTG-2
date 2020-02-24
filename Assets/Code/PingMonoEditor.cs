using ExitGames.Client.Photon;
using System;
using System.Net.Sockets;
using UnityEngine;

public class PingMonoEditor : PhotonPing
{
    private Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    public override void Dispose()
    {
        try
        {
            this.sock.Close();
        }
        catch
        {
        }
        this.sock = null;
    }

    public override bool Done()
    {
        if (!base.GotResult && (this.sock != null))
        {
            if (this.sock.Available <= 0)
            {
                return false;
            }
            int num = this.sock.Receive(base.PingBytes, SocketFlags.None);
            if ((base.PingBytes[base.PingBytes.Length - 1] != base.PingId) || (num != base.PingLength))
            {
                Debug.Log("ReplyMatch is false! ");
            }
            base.Successful = (num == base.PingBytes.Length) && (base.PingBytes[base.PingBytes.Length - 1] == base.PingId);
            base.GotResult = true;
        }
        return true;
    }

    public override bool StartPing(string ip)
    {
        base.Init();
        try
        {
            this.sock.ReceiveTimeout = 0x1388;
            this.sock.Connect(ip, 0x13bf);
            base.PingBytes[base.PingBytes.Length - 1] = base.PingId;
            this.sock.Send(base.PingBytes);
            base.PingBytes[base.PingBytes.Length - 1] = (byte) (base.PingId - 1);
        }
        catch (Exception exception)
        {
            this.sock = null;
            Console.WriteLine(exception);
        }
        return false;
    }
}

