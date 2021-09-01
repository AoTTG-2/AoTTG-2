using System;
using UnityEngine;

/// <summary>
/// Used to determine if a titan is on the ground or not
/// </summary>
public class CheckHitGround : MonoBehaviour
{
    public bool isGrounded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            this.isGrounded = true;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyAABB"))
        {
            this.isGrounded = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            this.isGrounded = true;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyAABB"))
        {
            this.isGrounded = true;
        }
    }
}

