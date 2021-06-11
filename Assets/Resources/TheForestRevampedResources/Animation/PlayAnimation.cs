using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
     [SerializeField] private Animator animationController;

    public GameObject Tree;
    public GameObject Particles;
    public float distance = 15f;
    GameObject[] titans;
    

    public void Update()
    {
        titans = GameObject.FindGameObjectsWithTag("titan"); //This is a horrible way to solve this issue, especially since this has to be inside the Update(),
        //Extremely perfromance heavy and needs a better way
        foreach (GameObject titan in titans){
            float enemdistance = Vector3.Distance(titan.transform.position, transform.position);
            if (enemdistance <= distance){
                Debug.Log("Titan has collided with tree");
                Particles.SetActive(true);
                animationController.SetBool("playFall", true); 
                Invoke("DestroyTree", 4);
            }
        }   
    }
    public void DestroyTree(){
        Destroy(Tree);
    }
}


