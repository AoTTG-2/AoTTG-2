using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDestroy : MonoBehaviour
{
    [SerializeField] private Animator animationController;

    public GameObject House;
    public int HouseHp=3;
    bool gotHit = false;

    public void OnTriggerEnter(Collider other){
         if (other.gameObject.name == "hand_L_001" && gotHit==false){
             HouseHp = HouseHp-1;
             Debug.Log(HouseHp);
             gotHit=true;
             if(HouseHp==0){
                 animationController.SetTrigger("collapse");
             }

         }
    }
    public void OnTriggerExit(Collider other){
         if (other.gameObject.name == "hand_L_001" && gotHit==true){
             gotHit=false;
         }
}
}