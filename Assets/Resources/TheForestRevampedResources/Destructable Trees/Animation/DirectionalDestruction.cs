using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalDestruction : MonoBehaviour
{

[SerializeField] private Animator animationController;

    public GameObject Tree;
    public GameObject Particles;
    public GameObject Leaves;
    public GameObject leafParticle;
    bool treeHit = false;

   void OnCollisionEnter(Collision collision){
        Vector3 position = transform.position;
        Vector3 dir = collision.contacts[0].point - transform.position;
        Vector3 crossLR = Vector3.Cross(transform.forward, dir);
        float angle = Vector3.Angle(dir, transform.forward);
        if (collision.gameObject.name == "Hero(Clone)" && treeHit == false){

            

            if (angle<45){
                print("front");
                treeHit = true;
                GameObject.Instantiate(Particles, position, Quaternion.identity);
                Particles.SetActive(true);
                animationController.SetTrigger("fallFront");  
                Invoke("DestroyTree", 5);
                return;
            }
            else if(angle>135){
                print("back");
                treeHit = true;
                GameObject.Instantiate(Particles, position, Quaternion.identity);
                Particles.SetActive(true);
                animationController.SetTrigger("fallBack"); 
                Invoke("DestroyTree", 5);
                return;
            }

            if (crossLR.y<0){
                print ("left");
                treeHit = true;
                GameObject.Instantiate(Particles, position, Quaternion.identity);
                Particles.SetActive(true);
                animationController.SetTrigger("fallLeft"); 
                Invoke("DestroyTree", 5);
            }
            else if (crossLR.y>0){
                print("right");
                treeHit = true;
                GameObject.Instantiate(Particles, position, Quaternion.identity);
                Particles.SetActive(true);
                animationController.SetTrigger("fallRight"); 
                Invoke("DestroyTree", 5);
            }

            
        }
        
    }
    public void DestroyTree(){
        Destroy(Tree);
    }

    public void LeavesGo(){
        Vector3 leafLocation = leafParticle.transform.position;
        GameObject.Instantiate(Leaves, leafLocation, Quaternion.identity);
    }
       
}

