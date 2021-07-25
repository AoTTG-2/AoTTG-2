using Assets.Scripts.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// Influences the behavior of a <see cref="Flare"/> after its been fired.
    /// </summary>
    public class FlareMovement : MonoBehaviour
    {
        public string color;
        private GameObject hero;
        private GameObject hint;
        private bool nohint;
        private Vector3 offY;
        private float timer;

        [SerializeField] private ParticleSystem particle = default;

        public void HideHint()
        {
            UnityEngine.Object.Destroy(this.hint);
            this.nohint = true;
        }

        private void Start()
        {
            this.hero = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
            if (!this.nohint && (this.hero != null))
            {
                this.hint = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/" + this.color + "FlareHint"));
                if (this.color == "Black")
                {
                    this.offY = (Vector3) (Vector3.up * 0.4f);
                }
                else
                {
                    this.offY = (Vector3) (Vector3.up * 0.5f);
                }
                this.hint.transform.parent = base.transform.root;
                this.hint.transform.position = this.hero.transform.position + this.offY;
                Vector3 vector = base.transform.position - this.hint.transform.position;
                float num = Mathf.Atan2(-vector.z, vector.x) * Mathf.Rad2Deg;
                this.hint.transform.rotation = Quaternion.Euler(-90f, num + 180f, 0f);
                this.hint.transform.localScale = Vector3.zero;
                object[] args = new object[] { "x", 1f, "y", 1f, "z", 1f, "easetype", iTween.EaseType.easeOutElastic, "time", 1f };
                iTween.ScaleTo(this.hint, iTween.Hash(args));
                object[] objArray2 = new object[] { "x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2.5f };
                iTween.ScaleTo(this.hint, iTween.Hash(objArray2));
            }
        }

        private void Update()
        {
            this.timer += Time.deltaTime;
            if (this.hint != null)
            {
                if (this.timer < 3f)
                {
                    this.hint.transform.position = this.hero.transform.position + this.offY;
                    Vector3 vector = base.transform.position - this.hint.transform.position;
                    float num = Mathf.Atan2(-vector.z, vector.x) * Mathf.Rad2Deg;
                    this.hint.transform.rotation = Quaternion.Euler(-90f, num + 180f, 0f);
                }
                else if (this.hint != null)
                {
                    UnityEngine.Object.Destroy(this.hint);
                }
            }
            if (this.timer < 4f)
            {
                base.GetComponent<Rigidbody>().AddForce((Vector3) (((base.transform.forward + (base.transform.up * 5f)) * Time.deltaTime) * 5f), ForceMode.VelocityChange);
            }
            else
            {
                base.GetComponent<Rigidbody>().AddForce((Vector3) ((-base.transform.up * Time.deltaTime) * 7f), ForceMode.Acceleration);
            }
        }

        [PunRPC]
        public void SetColorRpc(string color, PhotonMessageInfo info)
        {
            if (info.sender.ID != photonView.owner.ID) return;
            var flareColor = JsonConvert.DeserializeObject<Color>(color, new ColorJsonConverter());
            var main = particle.main;
            main.startColor = new ParticleSystem.MinMaxGradient(flareColor);
        }
    }
}

