using Assets.Scripts.Characters.Titan;
using UnityEngine;

public sealed class TitanTrigger : MonoBehaviour
{
    private MindlessTitan Titan { get; set; }

    private void Start()
    {
        Titan = gameObject.GetComponentInParent<MindlessTitan>();
    }

    public void SetCollision(bool value)
    {
        Titan.IsColliding = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Titan.IsColliding && other.transform.root.gameObject.GetPhotonView().isMine)
            Titan.IsColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (Titan.IsColliding && other.transform.root.gameObject.GetPhotonView().isMine)
            Titan.IsColliding = false;
    }
}

