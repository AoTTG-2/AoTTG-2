using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillPlayers : MonoBehaviour
{

    public GameObject PlayerRow;
	// Use this for initialization
	void Start () 
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(PlayerRow, transform);
        }
    }
    
}
