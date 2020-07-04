using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldCannon
{
    [RequireComponent(typeof(PhotonView))]
    public sealed class UnmannedCannon : Photon.MonoBehaviour
    {
        public const string InteractableName = "MountInteractable";

        public bool destroyed;
        public bool disabled;
        public string settings;
        public Hero storedHero;

        private static readonly Dictionary<CannonType, string> PrefabNameByType = new Dictionary<CannonType, string>
    {
        { CannonType.Ground, "CannonGround" },
        { CannonType.Wall, "CannonWall" }
    };

        [SerializeField]
        private bool autoGenerateSettings = true;

        [SerializeField]
        private CannonType type = CannonType.Ground;

        [PunRPC]
        public void RequestMountRPC(int viewID, PhotonMessageInfo info)
        {
            Debug.Assert(PhotonNetwork.isMasterClient, $"{nameof(RequestMountRPC)} can only be called on MasterClient.");
            Debug.Assert(photonView.isMine, $"MasterClient should own the {nameof(UnmannedCannon)} when {nameof(RequestMountRPC)} is called.");
            Debug.Assert(!disabled, $"Can't call {nameof(RequestMountRPC)} when cannon is disabled.");

            var requestingHero = PhotonView.Find(viewID).gameObject.GetComponent<Hero>();
            Debug.Assert(requestingHero.photonView.owner == info.sender, "Hero owner and RPC sender must match up.");

            var requestExists = FengGameManagerMKII.instance.AllowedCannonRequests.ContainsKey(info.sender.ID);
            Debug.Assert(!requestExists, "Can't request cannon: One has already been requested.");

            if (!requestExists)
            {
                disabled = true;
                StartCoroutine(WaitAndEnable());
                FengGameManagerMKII.instance.AllowedCannonRequests.Add(info.sender.ID,
                    new CannonValues(photonView.viewID, settings));
                //requestingHero.photonView.RPC(nameof(requestingHero.SpawnCannonRPC),
                //    info.sender,
                //    settings);
            }
        }

        [PunRPC]
        public void SetSize(string settings, PhotonMessageInfo info)
        {
            if (info.sender.IsMasterClient)
            {
                var strArray = settings.Split(new char[] { ',' });
                if (strArray.Length > 15)
                {
                    var a = 1f;
                    var gameObject = base.gameObject;
                    if (strArray[2] != "default")
                    {
                        if (strArray[2].StartsWith("transparent"))
                        {
                            float foo;
                            if (float.TryParse(strArray[2].Substring(11), out foo))
                                a = foo;

                            foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material) FengGameManagerMKII.RCassets.LoadAsset("transparent");
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]),
                                        renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                }
                            }
                        }
                        else
                        {
                            foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material) FengGameManagerMKII.RCassets.LoadAsset(strArray[2]);
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]),
                                        renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                }
                            }
                        }
                    }

                    var x = gameObject.transform.localScale.x * Convert.ToSingle(strArray[3]) - 0.001f;
                    var y = gameObject.transform.localScale.y * Convert.ToSingle(strArray[4]);
                    var z = gameObject.transform.localScale.z * Convert.ToSingle(strArray[5]);
                    gameObject.transform.localScale = new Vector3(x, y, z);
                    if (strArray[6] != "0")
                    {
                        var color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]),
                            Convert.ToSingle(strArray[9]), a);
                        foreach (var filter in gameObject.GetComponentsInChildren<MeshFilter>())
                        {
                            var mesh = filter.mesh;
                            var colorArray = new Color[mesh.vertexCount];
                            for (var i = 0; i < mesh.vertexCount; i++)
                                colorArray[i] = color;

                            mesh.colors = colorArray;
                        }
                    }
                }
            }
        }

        public void TryMount(Hero player)
        {
            var playerViewID = player.photonView.viewID;
            photonView.RPC(nameof(RequestMountRPC), PhotonTargets.MasterClient, playerViewID);
        }

        private void OnValidate()
        {
            if (autoGenerateSettings)
            {
                var pos = transform.position;
                var rot = transform.rotation;
                var prefabName = PrefabNameByType[type];
                settings = $"photon,{prefabName},{pos.x},{pos.y},{pos.z},{rot.x},{rot.y},{rot.z},{rot.w}";
            }
        }

        private void Reset()
        {
            // Yes, this is null when the component is first added.
            if (photonView.ObservedComponents == null)
                photonView.ObservedComponents = new List<Component>();

            photonView.ObservedComponents.Remove(this);
            photonView.ObservedComponents.Add(this);
        }

        private void Start()
        {
            if ((int) FengGameManagerMKII.settings[0x40] >= 100)
                GetComponent<Collider>().enabled = false;
        }

        private IEnumerator WaitAndEnable()
        {
            yield return new WaitForSeconds(5f);
            if (!destroyed)
                disabled = false;
        }
    }
}