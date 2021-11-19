using Assets.Scripts.Room;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDummyTitan : Spawner
{
    public GameObject dummyprefab;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("DummyTitanPrefab", transform.position, transform.rotation, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
