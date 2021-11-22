using UnityEngine;

namespace Assets.Scripts.Room
{
    /// <summary>
    /// Use this to place the Dummy Titan in the scene. If you do not use this script the dummy titan won't work in multiplayer
    /// </summary>
    public class DummyTitanSpawner : Spawner
    {
        public GameObject dummyprefab;

        // Destroys the placeholder dummy titan on start and reinstantiates it for use in multiplayer. Use the placeholder to make it easier to place in scene.
        void Start()
        {
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.Instantiate("DummyTitanPrefab", transform.position, transform.rotation, 0);
            }
        }
    }
}

