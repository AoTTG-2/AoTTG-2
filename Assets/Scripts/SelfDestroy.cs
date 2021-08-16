using UnityEngine;

/// <summary>
/// AoTTG1 script which will automatically destroy the gameObject after <see cref="CountDown"/>
/// </summary>
public class SelfDestroy : Photon.MonoBehaviour
{
    public float CountDown = 5f;

    private void Start()
    {
    }

    private void Update()
    {
        this.CountDown -= Time.deltaTime;
        if (this.CountDown <= 0f)
        {
            if (base.photonView != null)
            {
                if (base.photonView.viewID == 0)
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
                else if (base.photonView.isMine)
                {
                    PhotonNetwork.Destroy(base.gameObject);
                }
            }
            else
            {
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
    }
}

