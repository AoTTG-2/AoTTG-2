using Assets.Scripts.Characters.Humans;
using UnityEngine;
using Assets.Scripts.Inventory.Items.Data;
using Assets.Scripts.Inventory;

namespace Assets.Scripts.Inventory.Items
{
    public class Lantern : Item
    {

        public Light lanternLight;
        private Camera mainCamera;
        private GameObject heroLight;
        private bool lightStatus;
        private PhotonView photonView;
        private LanternLightBehaviour lightBehaviour;

        public override void Use(Hero hero)
        {

            if (heroLight == null)
            {

                SetUpLight(hero);
                return;

            }

            photonView.RPC(nameof(lightBehaviour.ToggleLight), PhotonTargets.All);


        }

        void SetUpLight(Hero hero)
        {

            mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            heroLight = PhotonNetwork.Instantiate(nameof(lanternLight), hero.transform.position, Quaternion.identity, 0);

            Vector3 offset = new Vector3(0f, 1f, .5f);
            heroLight.transform.position += offset;
            heroLight.transform.parent = mainCamera.transform;
            photonView = heroLight.gameObject.GetComponent<PhotonView>();
            lightBehaviour = heroLight.GetComponent<LanternLightBehaviour>();

            photonView.RPC(nameof(lightBehaviour.ToggleLight), PhotonTargets.All);

        }

        public Lantern(LanternData data) : base(data)
        {



        }

    }

}
