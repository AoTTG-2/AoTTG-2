using System;

public class PhotonMessageInfo
{
    public PhotonView photonView;
    public PhotonPlayer sender;
    private int timeInt;

    public PhotonMessageInfo()
    {
        this.sender = PhotonNetwork.player;
        this.timeInt = (int) (PhotonNetwork.time * 1000.0);
        this.photonView = null;
    }

    public PhotonMessageInfo(PhotonPlayer player, int timestamp, PhotonView view)
    {
        this.sender = player;
        this.timeInt = timestamp;
        this.photonView = view;
    }

    public override string ToString()
    {
        return string.Format("[PhotonMessageInfo: player='{1}' timestamp={0}]", this.timestamp, this.sender);
    }

    public double timestamp
    {
        get
        {
            return (((double) this.timeInt) / 1000.0);
        }
    }
}

