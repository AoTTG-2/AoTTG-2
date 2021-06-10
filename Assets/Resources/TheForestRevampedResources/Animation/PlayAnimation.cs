using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
     [SerializeField] private Animator animationController;

    public GameObject Tree;
    public GameObject Particles;
    public float distance = 15f;
    

    public void Update()
    {
        if (Vector3.Distance(GameObject.FindWithTag("titan").transform.position, transform.position) <= distance)
        {
            Debug.Log("Titan has collided with tree");
            Particles.SetActive(true);
            animationController.SetBool("playFall", true); 
            Invoke("DestroyTree", 4);
        }
    }
    public void DestroyTree(){
        Destroy(Tree);
    }
}


