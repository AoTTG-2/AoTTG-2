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
    bool treeHit = false;
    bool particleplay = false;
    
    public void Start(){
        InvokeRepeating("Hit", 0.0f, 0.5f);
        
    }
    
    public void Hit()
    {
        titans = GameObject.FindGameObjectsWithTag("titan"); //This is a horrible way to solve this issue, it's a performance hog
        Vector3 position = transform.position;
        foreach (GameObject titan in titans){
            float enemdistance = Vector3.Distance(titan.transform.position, transform.position);
            if (enemdistance <= distance && treeHit==false){
                GameObject.Instantiate(Particles, position, Quaternion.identity);
                //Debug.Log("Titan has collided with tree");
                Particles.SetActive(true);
                animationController.SetBool("playFall", true); 
                Invoke("DestroyTree", 4);
                treeHit=true;
            }
        }   
    }
    public void DestroyTree(){
        Destroy(Tree);
    }
    
}


