using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalDestruction : MonoBehaviour
{

[SerializeField] private Animator animationController;

    public GameObject Tree;
    public GameObject dustParticles;
    public GameObject Leaves;
    public GameObject leafParticle;
    public GameObject branchDebris;
    
    bool treeHit = false;
    

   void OnCollisionEnter(Collision collision){
        Vector3 contactDirection = collision.contacts[0].point - transform.position;
        Vector3 crossLR = Vector3.Cross(transform.forward, contactDirection);
        float angle = Vector3.Angle(contactDirection, transform.forward);
        if (collision.gameObject.tag == "titan" && treeHit == false){

            if (angle<45){
                //print("front"); //Just for debug purposes
                treeHit = true;
                deployDust();
                animationController.SetTrigger("fallFront");
                Invoke("DestroyTree", 5);
                return;
            }
            else if(angle>135){
                //print("back");
                treeHit = true;
                deployDust();
                animationController.SetTrigger("fallBack"); 
                Invoke("DestroyTree", 5);
                return;
            }

            if (crossLR.y<0){
                //print ("left");
                treeHit = true;
                deployDust();
                animationController.SetTrigger("fallLeft"); 
                Invoke("DestroyTree", 5);
            }
            else if (crossLR.y>0){
                //print("right");
                treeHit = true;
                deployDust();
                animationController.SetTrigger("fallRight"); 
                Invoke("DestroyTree", 5);
            }
  
        }
        
    }
    public void DestroyTree(){
        Vector3 leafLocation = leafParticle.transform.position;
        GameObject.Instantiate(branchDebris, leafLocation, Quaternion.Euler(new Vector3(-90, Random.Range(0, 360), 0)));
        Destroy(Tree);
    }

    public void deployLeafParticles(){
        Vector3 leafLocation = leafParticle.transform.position;
        GameObject.Instantiate(Leaves, leafLocation, Quaternion.identity);
    }

    public void deployDust(){
        Vector3 treePosition = transform.position;
        GameObject.Instantiate(dustParticles, treePosition, Quaternion.identity);
        dustParticles.SetActive(true);
    }

    public void disableColliders(){
        foreach(Collider coll in GetComponents<Collider> ()) {
                    coll.enabled = false;
                } 
        
    }
       
}

