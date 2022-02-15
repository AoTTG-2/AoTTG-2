using Assets.Scripts.Characters.Titan;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans
{
    /// <summary>
    /// A "Bullet" is the hook that is fired with the ODMG
    /// </summary>
    public class Bullet : Photon.MonoBehaviour
    {
        private bool isdestroying;
        private float killTime;
        private float killTime2;
        private bool left = true;
        public bool leviMode;
        public float leviShootTime;
        private LineRenderer lineRenderer;
        private GameObject master;
        private GameObject myRef;
        public MindlessTitan myTitan;
        private ArrayList nodes = new ArrayList();
        private int phase;
        private GameObject rope;
        private int spiralcount;
        private ArrayList spiralNodes;
        private Vector3 velocity = Vector3.zero;
        private Vector3 velocity2 = Vector3.zero;

        public void checkTitan()
        {
            GameObject obj2 = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
            if (((obj2 != null) && (this.master != null)) && (this.master == obj2))
            {
                RaycastHit hit;
                LayerMask mask = ((int) 1) << LayerMask.NameToLayer("PlayerAttackBox");
                if (Physics.Raycast(base.transform.position, this.velocity, out hit, 10f, mask.value))
                {
                    Collider collider = hit.collider;
                    if (collider.name.Contains("PlayerCollisionDetection"))
                    {
                        var component = collider.transform.root.gameObject.GetComponent<MindlessTitan>();
                        if (component != null)
                        {
                            if (this.myTitan == null)
                            {
                                this.myTitan = component;
                                myTitan.IsHooked = true;
                            }
                            else if (this.myTitan != component)
                            {
                                myTitan.IsHooked = false;
                                this.myTitan = component;
                                myTitan.IsHooked = true;
                            }
                        }
                    }
                }
            }
        }

        public void disable()
        {
            this.phase = 2;
            this.killTime = 0f;
            object[] parameters = { 2 };
            base.photonView.RPC(nameof(setPhase), PhotonTargets.Others, parameters);
        }

        private void FixedUpdate()
        {
            if (!(((this.phase == 2) || (this.phase == 1)) ? !this.leviMode : true))
            {
                this.spiralcount++;
                if (this.spiralcount >= 60)
                {
                    this.isdestroying = true;
                    this.removeMe();
                    return;
                }
            }
            if (!(base.photonView.isMine))
            {
                if (this.phase == 0)
                {
                    Transform transform = base.gameObject.transform;
                    transform.position += ((this.velocity * Time.deltaTime) * 50f) + (this.velocity2 * Time.deltaTime);
                    this.nodes.Add(new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z));
                }
            }
            else if (this.phase == 0)
            {
                RaycastHit hit;
                this.checkTitan();
                Transform transform3 = base.gameObject.transform;
                transform3.position += ((this.velocity * Time.deltaTime) * 50f) + (this.velocity2 * Time.deltaTime);
                LayerMask mask = 1 << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask2 = 1 << LayerMask.NameToLayer("Ground");
                LayerMask mask3 = 1 << LayerMask.NameToLayer("NetworkObject");
                LayerMask mask4 = (mask | mask2) | mask3;
                bool flag2 = false;
                bool flag3 = false;
                if (this.nodes.Count > 1)
                {
                    flag3 = Physics.Linecast((Vector3) this.nodes[this.nodes.Count - 2], base.gameObject.transform.position, out hit, mask4.value);
                }
                else
                {
                    flag3 = Physics.Linecast((Vector3) this.nodes[this.nodes.Count - 1], base.gameObject.transform.position, out hit, mask4.value);
                }
                if (flag3)
                {
                    bool flag4 = true;
                    if (hit.collider.transform.gameObject.layer == LayerMask.NameToLayer("EnemyBox"))
                    {
                        object[] parameters = { hit.collider.transform.root.gameObject.GetPhotonView().viewID };
                        base.photonView.RPC(nameof(tieMeToOBJ), PhotonTargets.Others, parameters);
                        this.master.GetComponent<Hero>().lastHook = hit.collider.transform.root;
                        base.transform.parent = hit.collider.transform;
                    }
                    else if (hit.collider.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        this.master.GetComponent<Hero>().lastHook = null;
                    }
                    else if (((hit.collider.transform.gameObject.layer == LayerMask.NameToLayer("NetworkObject")) && (hit.collider.transform.gameObject.tag == "Player")) && !this.leviMode)
                    {
                        object[] objArray3 = { hit.collider.transform.root.gameObject.GetPhotonView().viewID };
                        base.photonView.RPC(nameof(tieMeToOBJ), PhotonTargets.Others, objArray3);
                        this.master.GetComponent<Hero>().HookToHuman(hit.collider.transform.root.gameObject, base.transform.position);
                        base.transform.parent = hit.collider.transform;
                        this.master.GetComponent<Hero>().lastHook = null;
                    }
                    else
                    {
                        flag4 = false;
                    }
                    if (this.phase == 2)
                    {
                        flag4 = false;
                    }
                    if (flag4)
                    {
                        this.master.GetComponent<Hero>().Launch(hit.point, this.left, this.leviMode);
                        base.transform.position = hit.point;
                        if (this.phase != 2)
                        {
                            this.phase = 1;
                            object[] objArray4 = { 1 };
                            base.photonView.RPC(nameof(setPhase), PhotonTargets.Others, objArray4);
                            object[] objArray5 = { base.transform.position };
                            base.photonView.RPC(nameof(tieMeTo), PhotonTargets.Others, objArray5);
                            if (this.leviMode)
                            {
                                this.getSpiral(this.master.transform.position, this.master.transform.rotation.eulerAngles);
                            }
                            flag2 = true;
                        }
                    }
                }
                this.nodes.Add(new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z));
                if (!flag2)
                {
                    this.killTime2 += Time.deltaTime;
                    if (this.killTime2 > 0.8f)
                    {
                        this.phase = 4;
                        object[] objArray6 = { 4 };
                        base.photonView.RPC(nameof(setPhase), PhotonTargets.Others, objArray6);
                    }
                }
            }
        }

        private void getSpiral(Vector3 masterposition, Vector3 masterrotation)
        {
            float num = 1.2f;
            float num2 = 30f;
            float num3 = 2f;
            float num4 = 0.5f;
            num = 30f;
            num3 = 0.05f + (this.spiralcount * 0.03f);
            if (this.spiralcount < 5)
            {
                num = Vector2.Distance(new Vector2(masterposition.x, masterposition.z), new Vector2(base.gameObject.transform.position.x, base.gameObject.transform.position.z));
            }
            else
            {
                num = 1.2f + ((60 - this.spiralcount) * 0.1f);
            }
            num4 -= this.spiralcount * 0.06f;
            float num6 = num / num2;
            float num7 = num3 / num2;
            float num8 = (num7 * 2f) * 3.141593f;
            num4 *= 6.283185f;
            this.spiralNodes = new ArrayList();
            for (int i = 1; i <= num2; i++)
            {
                float num10 = (i * num6) * (1f + (0.05f * i));
                float f = (((i * num8) + num4) + 1.256637f) + (masterrotation.y * 0.0173f);
                float x = Mathf.Cos(f) * num10;
                float z = -Mathf.Sin(f) * num10;
                this.spiralNodes.Add(new Vector3(x, 0f, z));
            }
        }

        public bool isHooked()
        {
            return (this.phase == 1);
        }

        [PunRPC]
        private void killObject()
        {
            UnityEngine.Object.Destroy(this.rope);
            UnityEngine.Object.Destroy(base.gameObject);
        }

        public void launch(Vector3 v, Vector3 v2, string launcher_ref, bool isLeft, GameObject hero, bool leviMode = false)
        {
            if (this.phase != 2)
            {
                this.master = hero;
                this.velocity = v;
                float f = Mathf.Acos(Vector3.Dot(v.normalized, v2.normalized)) * Mathf.Rad2Deg;
                if (Mathf.Abs(f) > 90f)
                {
                    this.velocity2 = Vector3.zero;
                }
                else
                {
                    this.velocity2 = Vector3.Project(v2, v);
                }
                if (launcher_ref == "hookRefL1")
                {
                    this.myRef = hero.GetComponent<Hero>().hookRefL1;
                }
                if (launcher_ref == "hookRefL2")
                {
                    this.myRef = hero.GetComponent<Hero>().hookRefL2;
                }
                if (launcher_ref == "hookRefR1")
                {
                    this.myRef = hero.GetComponent<Hero>().hookRefR1;
                }
                if (launcher_ref == "hookRefR2")
                {
                    this.myRef = hero.GetComponent<Hero>().hookRefR2;
                }
                this.nodes = new ArrayList();
                this.nodes.Add(this.myRef.transform.position);
                this.phase = 0;
                this.leviMode = leviMode;
                this.left = isLeft;
                if (base.photonView.isMine)
                {
                    object[] parameters = new object[] { hero.GetComponent<Hero>().photonView.viewID, launcher_ref };
                    base.photonView.RPC(nameof(myMasterIs), PhotonTargets.Others, parameters);
                    object[] objArray2 = new object[] { v, this.velocity2, this.left };
                    base.photonView.RPC(nameof(setVelocityAndLeft), PhotonTargets.Others, objArray2);
                }
                base.transform.position = this.myRef.transform.position;
                base.transform.rotation = Quaternion.LookRotation(v.normalized);
            }
        }

        [PunRPC]
        private void myMasterIs(int id, string launcherRef)
        {
            this.master = PhotonView.Find(id).gameObject;
            if (launcherRef == "hookRefL1")
            {
                this.myRef = this.master.GetComponent<Hero>().hookRefL1;
            }
            if (launcherRef == "hookRefL2")
            {
                this.myRef = this.master.GetComponent<Hero>().hookRefL2;
            }
            if (launcherRef == "hookRefR1")
            {
                this.myRef = this.master.GetComponent<Hero>().hookRefR1;
            }
            if (launcherRef == "hookRefR2")
            {
                this.myRef = this.master.GetComponent<Hero>().hookRefR2;
            }
        }

        [PunRPC]
        private void netLaunch(Vector3 newPosition)
        {
            this.nodes = new ArrayList();
            this.nodes.Add(newPosition);
        }

        [PunRPC]
        private void netUpdateLeviSpiral(Vector3 newPosition, Vector3 masterPosition, Vector3 masterrotation)
        {
            this.phase = 2;
            this.leviMode = true;
            this.getSpiral(masterPosition, masterrotation);
            Vector3 vector = masterPosition - ((Vector3) this.spiralNodes[0]);
            this.lineRenderer.SetVertexCount(this.spiralNodes.Count - ((int) (this.spiralcount * 0.5f)));
            for (int i = 0; i <= ((this.spiralNodes.Count - 1) - (this.spiralcount * 0.5f)); i++)
            {
                if (this.spiralcount < 5)
                {
                    Vector3 position = ((Vector3) this.spiralNodes[i]) + vector;
                    float num2 = (this.spiralNodes.Count - 1) - (this.spiralcount * 0.5f);
                    position = new Vector3(position.x, (position.y * ((num2 - i) / num2)) + (newPosition.y * (((float) i) / num2)), position.z);
                    this.lineRenderer.SetPosition(i, position);
                }
                else
                {
                    this.lineRenderer.SetPosition(i, ((Vector3) this.spiralNodes[i]) + vector);
                }
            }
        }

        [PunRPC]
        private void netUpdatePhase1(Vector3 newPosition, Vector3 masterPosition)
        {
            this.lineRenderer.SetVertexCount(2);
            this.lineRenderer.SetPosition(0, newPosition);
            this.lineRenderer.SetPosition(1, masterPosition);
            base.transform.position = newPosition;
        }

        private void OnDestroy()
        {
            if (this.myTitan != null)
            {
                myTitan.IsHooked = false;
            }
            UnityEngine.Object.Destroy(this.rope);
        }

        public void removeMe()
        {
            this.isdestroying = true;
            if (base.photonView.isMine)
            {                
                PhotonNetwork.Destroy(base.photonView);
                PhotonNetwork.RemoveRPCs(base.photonView);
            }
        }

        private void setLinePhase0()
        {
            if (this.master == null)
            {
                UnityEngine.Object.Destroy(this.rope);
                UnityEngine.Object.Destroy(base.gameObject);
            }
            else if (this.nodes.Count > 0)
            {
                Vector3 vector = this.myRef.transform.position - ((Vector3) this.nodes[0]);
                this.lineRenderer.SetVertexCount(this.nodes.Count);
                for (int i = 0; i <= (this.nodes.Count - 1); i++)
                {
                    this.lineRenderer.SetPosition(i, ((Vector3) this.nodes[i]) + vector * Mathf.Pow(0.75f, (float) i));
                }
                if (this.nodes.Count > 1)
                {
                    this.lineRenderer.SetPosition(1, this.myRef.transform.position);
                }
            }
        }

        [PunRPC]
        private void setPhase(int value)
        {
            this.phase = value;
        }

        [PunRPC]
        private void setVelocityAndLeft(Vector3 value, Vector3 v2, bool l)
        {
            this.velocity = value;
            this.velocity2 = v2;
            this.left = l;
            base.transform.rotation = Quaternion.LookRotation(value.normalized);
        }

        private void Start()
        {
            this.rope = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("rope"));
            this.lineRenderer = this.rope.GetComponent<LineRenderer>();
        }

        [PunRPC]
        private void tieMeTo(Vector3 p)
        {
            base.transform.position = p;
        }

        [PunRPC]
        private void tieMeToOBJ(int id)
        {
            base.transform.parent = PhotonView.Find(id).gameObject.transform;
        }

        private void Update()
        {
            if (this.master == null)
            {
                this.removeMe();
            }
            else if (!this.isdestroying)
            {
                if (this.leviMode)
                {
                    this.leviShootTime += Time.deltaTime;
                    if (this.leviShootTime > 0.4f)
                    {
                        this.phase = 2;
                        base.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
                if (this.phase == 0)
                {
                    this.setLinePhase0();
                }
                else if (this.phase == 1)
                {
                    Vector3 vector = base.transform.position - this.myRef.transform.position;
                    Vector3 vector1 = base.transform.position + this.myRef.transform.position;
                    Vector3 velocity = this.master.GetComponent<Rigidbody>().velocity;
                    float magnitude = velocity.magnitude;
                    float f = vector.magnitude;
                    int num3 = (int) ((f + magnitude) / 5f);
                    num3 = Mathf.Clamp(num3, 2, 6);
                    this.lineRenderer.SetVertexCount(num3);
                    this.lineRenderer.SetPosition(0, this.myRef.transform.position);
                    int index = 1;
                    float num5 = Mathf.Pow(f, 0.3f);
                    while (index < num3)
                    {
                        int num6 = num3 / 2;
                        float num7 = Mathf.Abs((int) (index - num6));
                        float num8 = (num6 - num7) / ((float) num6);
                        num8 = Mathf.Pow(num8, 0.5f);
                        float max = ((num5 + magnitude) * 0.0015f) * num8;
                        this.lineRenderer.SetPosition(index, (((new Vector3(UnityEngine.Random.Range(-max, max), UnityEngine.Random.Range(-max, max), UnityEngine.Random.Range(-max, max)) + this.myRef.transform.position) + (vector * (((float) index) / ((float) num3)))) - (((Vector3.up * num5) * 0.05f) * num8)) - (((velocity * 0.001f) * num8) * num5));
                        index++;
                    }
                    this.lineRenderer.SetPosition(num3 - 1, base.transform.position);
                }
                else if (this.phase == 2)
                {
                    if (!this.leviMode)
                    {
                        this.lineRenderer.SetVertexCount(2);
                        this.lineRenderer.SetPosition(0, base.transform.position);
                        this.lineRenderer.SetPosition(1, this.myRef.transform.position);
                        this.killTime += Time.deltaTime * 0.2f;
                        this.lineRenderer.SetWidth(0.1f - this.killTime, 0.1f - this.killTime);
                        if (this.killTime > 0.1f)
                        {
                            this.removeMe();
                        }
                    }
                    else
                    {
                        this.getSpiral(this.master.transform.position, this.master.transform.rotation.eulerAngles);
                        Vector3 vector4 = this.myRef.transform.position - ((Vector3) this.spiralNodes[0]);
                        this.lineRenderer.SetVertexCount(this.spiralNodes.Count - ((int) (this.spiralcount * 0.5f)));
                        for (int i = 0; i <= ((this.spiralNodes.Count - 1) - (this.spiralcount * 0.5f)); i++)
                        {
                            if (this.spiralcount < 5)
                            {
                                Vector3 position = ((Vector3) this.spiralNodes[i]) + vector4;
                                float num11 = (this.spiralNodes.Count - 1) - (this.spiralcount * 0.5f);
                                position = new Vector3(position.x, (position.y * ((num11 - i) / num11)) + (base.gameObject.transform.position.y * (i / num11)), position.z);
                                this.lineRenderer.SetPosition(i, position);
                            }
                            else
                            {
                                this.lineRenderer.SetPosition(i, ((Vector3) this.spiralNodes[i]) + vector4);
                            }
                        }
                    }
                }
                else if (this.phase == 4)
                {
                    Transform transform = base.gameObject.transform;
                    transform.position += this.velocity + this.velocity2 * Time.deltaTime;
                    this.nodes.Add(new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z));
                    Vector3 vector10 = this.myRef.transform.position - ((Vector3) this.nodes[0]);
                    for (int j = 0; j <= (this.nodes.Count - 1); j++)
                    {
                        this.lineRenderer.SetVertexCount(this.nodes.Count);
                        this.lineRenderer.SetPosition(j, ((Vector3) this.nodes[j]) + ((Vector3) (vector10 * Mathf.Pow(0.5f, (float) j))));
                    }
                    this.killTime2 += Time.deltaTime;
                    if (this.killTime2 > 0.8f)
                    {
                        this.killTime += Time.deltaTime * 0.2f;
                        this.lineRenderer.SetWidth(0.1f - this.killTime, 0.1f - this.killTime);
                        if (this.killTime > 0.1f)
                        {
                            this.removeMe();
                        }
                    }
                }
            }
        }
    }
}

