using Assets.Scripts.UI.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OldCannon
{
    [RequireComponent(typeof(Interactable), typeof(PhotonView))]
    public sealed class Cannon : Photon.MonoBehaviour
    {
        [HideInInspector]
        public Hero Hero;

        [SerializeField]
        private Transform ballPoint;

        [SerializeField]
        private Transform barrel;

        private Quaternion correctBarrelRot = Quaternion.identity;

        private Vector3 correctPlayerPos = Vector3.zero;

        private Quaternion correctPlayerRot = Quaternion.identity;

        private float currentRot = 0f;

        [SerializeField]
        private Transform firingPoint;

        [SerializeField]
        private bool isCannonGround;

        [SerializeField]
        private LineRenderer myCannonLine;

        [SerializeField]
        private Transform playerPoint;

        private string settings;

        [SerializeField]
        private float smoothingDelay = 5f;

        public static Cannon Create(Hero hero, string settings)
        {
            var strArray = settings.Split(new char[] { ',' });
            var prefabName = "RC Resources/RC Prefabs/" + strArray[1];

            Cannon cannon;
            if (strArray.Length > 15)
            {
                var position = new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14]));
                var rotation = new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[16]), Convert.ToSingle(strArray[17]), Convert.ToSingle(strArray[18]));
                cannon = PhotonNetwork.Instantiate(prefabName, position, rotation, 0).GetComponent<Cannon>();
            }
            else
            {
                var position = new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4]));
                var rotation = new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]));
                cannon = PhotonNetwork.Instantiate(prefabName, position, rotation, 0).GetComponent<Cannon>();
            }

            cannon.Hero = hero;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(cannon.firingPoint.gameObject, true, false);
            Camera.main.fieldOfView = 55f;

            hero.HeroDied += cannon.OnHeroDied;

            return cannon;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(barrel.rotation);
            }
            else
            {
                correctPlayerPos = (Vector3) stream.ReceiveNext();
                correctPlayerRot = (Quaternion) stream.ReceiveNext();
                correctBarrelRot = (Quaternion) stream.ReceiveNext();
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
                    if (strArray[2] != "default")
                    {
                        if (strArray[2].StartsWith("transparent"))
                        {
                            float result;
                            if (float.TryParse(strArray[2].Substring(11), out result))
                                a = result;

                            foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material) FengGameManagerMKII.RCassets.LoadAsset("transparent");
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    renderer.material.mainTextureScale = new Vector2(
                                        renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]),
                                        renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                }
                            }
                        }
                        else
                        {
                            foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
                            {
                                if (!renderer.name.Contains("Line Renderer"))
                                {
                                    renderer.material = (Material) FengGameManagerMKII.RCassets.LoadAsset(strArray[2]);
                                    if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                    {
                                        renderer.material.mainTextureScale = new Vector2(
                                            renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]),
                                            renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                    }
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
                        var color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), a);
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

        public void TryUnmount(Hero _)
        {
            if (!photonView.isMine)
                return;

            if (Hero)
            {
                Hero.isCannon = false;
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(Hero.gameObject, true, false);
                Hero.baseRigidBody.velocity = Vector3.zero;
                Hero.photonView.RPC(nameof(Hero.ReturnFromCannon), PhotonTargets.Others);
                Hero.skillCdLast = Hero.skillCdLastCannon;
                Hero.skillCdDuration = Hero.skillCdLast;
            }

            PhotonNetwork.Destroy(gameObject);
        }

        private void ApplyPlayerInput()
        {
            var vector = new Vector3(0f, -30f, 0f);
            var position = ballPoint.position;
            var vector3 = ballPoint.forward * 300f;
            var num = 40f / vector3.magnitude;
            myCannonLine.startWidth = 0.5f;
            myCannonLine.endWidth = 40f;
            myCannonLine.positionCount = 100;
            for (var i = 0; i < 100; i++)
            {
                myCannonLine.SetPosition(i, position);
                position += (vector3 * num) + (((0.5f * vector) * num) * num);
                vector3 += vector * num;
            }


            var rotationSpeed = InputManager.Key(InputCannon.Slow) ? 5f : 30f;

            if (isCannonGround)
            {
                if (InputManager.Key(InputCannon.Up))
                {
                    if (currentRot <= 32f)
                    {
                        currentRot += Time.deltaTime * rotationSpeed;
                        barrel.Rotate(new Vector3(0f, 0f, Time.deltaTime * rotationSpeed));
                    }
                }
                else if (InputManager.Key(InputCannon.Down) && (currentRot >= -18f))
                {
                    currentRot += Time.deltaTime * -rotationSpeed;
                    barrel.Rotate(new Vector3(0f, 0f, Time.deltaTime * -rotationSpeed));
                }

                if (InputManager.Key(InputCannon.Left))
                    transform.Rotate(new Vector3(0f, Time.deltaTime * -rotationSpeed, 0f));
                else if (InputManager.Key(InputCannon.Right))
                    transform.Rotate(new Vector3(0f, Time.deltaTime * rotationSpeed, 0f));
            }
            else
            {
                if (InputManager.Key(InputCannon.Up))
                {
                    if (currentRot >= -50f)
                    {
                        currentRot += Time.deltaTime * -rotationSpeed;
                        barrel.Rotate(new Vector3(Time.deltaTime * -rotationSpeed, 0f, 0f));
                    }
                }
                else if (InputManager.Key(InputCannon.Down) && (currentRot <= 40f))
                {
                    currentRot += Time.deltaTime * rotationSpeed;
                    barrel.Rotate(new Vector3(Time.deltaTime * rotationSpeed, 0f, 0f));
                }

                if (InputManager.Key(InputCannon.Left))
                    transform.Rotate(new Vector3(0f, Time.deltaTime * -rotationSpeed, 0f));
                else if (InputManager.Key(InputCannon.Right))
                    transform.Rotate(new Vector3(0f, Time.deltaTime * rotationSpeed, 0f));
            }
            if (InputManager.Key(InputCannon.Shoot))
            {
                Fire();
            }
        }

        private void ApplyPredictedTransform()
        {
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * smoothingDelay);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * smoothingDelay);
            barrel.rotation = Quaternion.Lerp(barrel.rotation, correctBarrelRot, Time.deltaTime * smoothingDelay);
        }

        private void Awake()
        {
            if (photonView)
            {
                correctPlayerPos = transform.position;
                correctPlayerRot = transform.rotation;
                correctBarrelRot = barrel.rotation;

                if (PhotonNetwork.isMasterClient)
                {
                    var owner = photonView.owner;
                    CannonValues values;
                    if (FengGameManagerMKII.instance.AllowedCannonRequests.TryGetValue(owner.ID, out values))
                    {
                        photonView.RPC(nameof(SetSize), PhotonTargets.All, settings = values.settings);
                        FengGameManagerMKII.instance.AllowedCannonRequests.Remove(owner.ID);

                        var viewID = values.viewID;
                        var component = PhotonView.Find(viewID).GetComponent<UnmannedCannon>();
                        component.disabled = true;
                        component.destroyed = true;
                        PhotonNetwork.Destroy(component.gameObject);
                    }
                    else if (!(owner.IsLocal || FengGameManagerMKII.instance.restartingMC))
                        FengGameManagerMKII.instance.kickPlayerRC(owner, false, "spawning cannon without request.");
                }
            }
        }

        private void Fire()
        {
            if (Hero.skillCdDuration <= 0f)
            {
                var boom = PhotonNetwork.Instantiate("FX/boom2", firingPoint.position, firingPoint.rotation, 0);
                var boomCheckColliders = boom.GetComponentsInChildren<EnemyCheckCollider>();

                foreach (var collider in boomCheckColliders)
                    collider.dmg = 0;

                CannonBall.Create(ballPoint.position,
                    firingPoint.rotation,
                    firingPoint.forward * 300f,
                    this,
                    Hero.photonView.viewID);

                Hero.skillCdDuration = 3.5f;
            }
        }

        private void OnDestroy()
        {
            Hero.HeroDied -= OnHeroDied;

            if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.isRestarting)
            {
                var strArray = settings.Split(new char[] { ',' });
                if (strArray[0] == "photon")
                {
                    if (strArray.Length > 15)
                    {
                        var cannon = PhotonNetwork.Instantiate(
                            "RC Resources/RC Prefabs/" + "Unmanned" + strArray[1],
                            new Vector3(
                                Convert.ToSingle(strArray[12]),
                                Convert.ToSingle(strArray[13]),
                                Convert.ToSingle(strArray[14])),
                            new Quaternion(
                                Convert.ToSingle(strArray[15]),
                                Convert.ToSingle(strArray[0x10]),
                                Convert.ToSingle(strArray[0x11]),
                                Convert.ToSingle(strArray[0x12])), 0).GetComponent<UnmannedCannon>();
                        cannon.settings = settings;
                        cannon.photonView.RPC(nameof(cannon.SetSize), PhotonTargets.AllBuffered, settings);
                    }
                    else
                    {
                        PhotonNetwork.Instantiate("RC Resources/RC Prefabs/" + "Unmanned" + strArray[1],
                            new Vector3(
                                Convert.ToSingle(strArray[2]),
                                Convert.ToSingle(strArray[3]),
                                Convert.ToSingle(strArray[4])),
                            new Quaternion(
                                Convert.ToSingle(strArray[5]),
                                Convert.ToSingle(strArray[6]),
                                Convert.ToSingle(strArray[7]),
                                Convert.ToSingle(strArray[8])), 0).GetComponent<UnmannedCannon>().settings = settings;
                    }
                }
            }
        }

        private void OnHeroDied(Hero hero)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        private void Reset()
        {
            // Yes, this is null when the component is first added.
            if (photonView.ObservedComponents == null)
                photonView.ObservedComponents = new List<Component>();

            photonView.ObservedComponents.Remove(this);
            photonView.ObservedComponents.Add(this);

            playerPoint = transform.Find("PlayerPoint");
            barrel = transform.Find("Barrel");
            firingPoint = barrel.Find("FiringPoint");
            ballPoint = barrel.Find("BallPoint");
            myCannonLine = ballPoint.GetComponent<LineRenderer>();

            if (gameObject.name.Contains("CannonGround"))
                isCannonGround = true;

            GetComponent<Interactable>().SetDefaults("Unmount Cannon", (UnityEngine.Sprite) null, TryUnmount);
        }

        private void Update()
        {
            if (photonView.isMine)
                ApplyPlayerInput();
            else
                ApplyPredictedTransform();

            if (Hero.isCannon)
            {
                Hero.transform.position = playerPoint.position;
                Hero.transform.rotation = playerPoint.rotation;
            }
        }
    }
}