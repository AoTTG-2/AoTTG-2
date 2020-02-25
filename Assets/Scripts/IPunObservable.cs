using System;

public interface IPunObservable
{
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
}

