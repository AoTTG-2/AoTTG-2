using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
     [SerializeField] private Animator animationController;

    public GameObject Tree;
    public GameObject Particles;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Enemy has collided with tree");
            Particles.SetActive(true);
            animationController.SetBool("playFall", true);
            Invoke("DestroyTree", 4);

        }
    }
    public void DestroyTree(){
        Destroy(Tree);
    }
}


