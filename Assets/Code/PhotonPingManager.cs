using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PhotonPingManager
{
    public static int Attempts = 5;
    public static bool IgnoreInitialAttempt = true;
    public static int MaxMilliseconsPerPing = 800;
    private int PingsRunning;
    public bool UseNative;

    [DebuggerHidden]
    public IEnumerator PingSocket(Region region)
    {
        return new PingSocketc__IteratorB { region = region, f__this = this };
    }

    public static string ResolveHost(string hostName)
    {
        try
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
            if (hostAddresses.Length == 1)
            {
                return hostAddresses[0].ToString();
            }
            for (int i = 0; i < hostAddresses.Length; i++)
            {
                IPAddress address = hostAddresses[i];
                if (address != null)
                {
                    string str2 = address.ToString();
                    if (str2.IndexOf('.') >= 0)
                    {
                        return str2;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Debug.Log("Exception caught! " + exception.Source + " Message: " + exception.Message);
        }
        return string.Empty;
    }

    public Region BestRegion
    {
        get
        {
            Region region = null;
            int ping = 0x7fffffff;
            foreach (Region region2 in PhotonNetwork.networkingPeer.AvailableRegions)
            {
                Debug.Log("BestRegion checks region: " + region2);
                if ((region2.Ping != 0) && (region2.Ping < ping))
                {
                    ping = region2.Ping;
                    region = region2;
                }
            }
            return region;
        }
    }

    public bool Done
    {
        get
        {
            return (PingsRunning == 0);
        }
    }

    [CompilerGenerated]
    private sealed class PingSocketc__IteratorB : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal PhotonPingManager f__this;
        internal string cleanIpOfRegion__3;
        internal Exception e__8;
        internal int i__5;
        internal int indexOfColon__4;
        internal bool overtime__6;
        internal PhotonPing ping__0;
        internal int replyCount__2;
        internal int rtt__9;
        internal float rttSum__1;
        internal Stopwatch sw__7;
        internal Region region;

        [DebuggerHidden]
        public void Dispose()
        {
            SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint)SPC;
            SPC = -1;
            switch (num)
            {
                case 0:
                    region.Ping = Attempts * MaxMilliseconsPerPing;
                    f__this.PingsRunning++;
                    if (PhotonHandler.PingImplementation != typeof(PingNativeDynamic))
                    {
                        ping__0 = (PhotonPing)Activator.CreateInstance(PhotonHandler.PingImplementation);
                        break;
                    }
                    Debug.Log("Using constructor for new PingNativeDynamic()");
                    ping__0 = new PingNativeDynamic();
                    break;

                case 1:
                //goto Label_01B9;

                case 2:
                //goto Label_0265;

                case 3:
                    SPC = -1;
                    goto Label_02B0;

                default:
                    goto Label_02B0;
            }
            rttSum__1 = 0f;
            replyCount__2 = 0;
            cleanIpOfRegion__3 = region.HostAndPort;
            indexOfColon__4 = cleanIpOfRegion__3.LastIndexOf(':');
            if (indexOfColon__4 > 1)
            {
                cleanIpOfRegion__3 = cleanIpOfRegion__3.Substring(0, indexOfColon__4);
            }
            cleanIpOfRegion__3 = ResolveHost(cleanIpOfRegion__3);
            i__5 = 0;
            while (i__5 < Attempts)
            {
                overtime__6 = false;
                sw__7 = new Stopwatch();
                sw__7.Start();
                try
                {
                    ping__0.StartPing(cleanIpOfRegion__3);
                }
                catch (Exception exception)
                {
                    e__8 = exception;
                    Debug.Log("catched: " + e__8);
                    f__this.PingsRunning--;
                    break;
                }
                while (!ping__0.Done())
                {
                    if (sw__7.ElapsedMilliseconds >= MaxMilliseconsPerPing)
                    {
                        overtime__6 = true;
                        break;
                    }
                    Scurrent = 0;
                    SPC = 1;
                    goto Label_02B2;
                }
                rtt__9 = (int)sw__7.ElapsedMilliseconds;
                if ((!IgnoreInitialAttempt || (i__5 != 0)) && (ping__0.Successful && !overtime__6))
                {
                    rttSum__1 += rtt__9;
                    replyCount__2++;
                    region.Ping = (int)(rttSum__1 / replyCount__2);
                }
                Scurrent = new WaitForSeconds(0.1f);
                SPC = 2;
                goto Label_02B2;
                //i__5++;
            }
            f__this.PingsRunning--;
            Scurrent = null;
            SPC = 3;
            goto Label_02B2;
        Label_02B0:
            return false;
        Label_02B2:
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
                return Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return Scurrent;
            }
        }
    }
}

