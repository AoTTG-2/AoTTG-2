using Photon;
using System;
using UnityEngine;

public class Horse : Photon.MonoBehaviour
{
    private float awayTimer;
    private TITAN_CONTROLLER controller;
    public GameObject dust;
    public GameObject myHero;
    private Vector3 setPoint;
    private float speed = 45f;
    private string State = "idle";
    private float timeElapsed;

    private void crossFade(string aniName, float time)
    {
        base.GetComponent<Animation>().CrossFade(aniName, time);
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName, time };
            base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
        }
    }

    private void followed()
    {
        if (this.myHero != null)
        {
            this.State = "follow";
            this.setPoint = (this.myHero.transform.position + (Vector3.right * UnityEngine.Random.Range(-6, 6))) + (Vector3.forward * UnityEngine.Random.Range(-6, 6));
            this.setPoint.y = this.getHeight(this.setPoint + ((Vector3) (Vector3.up * 5f)));
            this.awayTimer = 0f;
        }
    }

    private float getHeight(Vector3 pt)
    {
        RaycastHit hit;
        LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("Ground");
        if (Physics.Raycast(pt, -Vector3.up, out hit, 1000f, mask2.value))
        {
            return hit.point.y;
        }
        return 0f;
    }

    public bool IsGrounded()
    {
        LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(base.gameObject.transform.position + ((Vector3) (Vector3.up * 0.1f)), -Vector3.up, (float) 0.3f, mask3.value);
    }

    private void LateUpdate()
    {
        if ((this.myHero == null) && base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.gameObject);
        }
        if (this.State == "mounted")
        {
            if (this.myHero == null)
            {
                this.unmounted();
                return;
            }
            this.myHero.transform.position = base.transform.position + ((Vector3) (Vector3.up * 1.68f));
            this.myHero.transform.rotation = base.transform.rotation;
            this.myHero.GetComponent<Rigidbody>().velocity = base.GetComponent<Rigidbody>().velocity;
            if (this.controller.targetDirection != -874f)
            {
                base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, this.controller.targetDirection, 0f), (100f * Time.deltaTime) / (base.GetComponent<Rigidbody>().velocity.magnitude + 20f));
                if (this.controller.isWALKDown)
                {
                    base.GetComponent<Rigidbody>().AddForce((Vector3) ((base.transform.forward * this.speed) * 0.6f), ForceMode.Acceleration);
                    if (base.GetComponent<Rigidbody>().velocity.magnitude >= (this.speed * 0.6f))
                    {
                        base.GetComponent<Rigidbody>().AddForce((Vector3) ((-this.speed * 0.6f) * base.GetComponent<Rigidbody>().velocity.normalized), ForceMode.Acceleration);
                    }
                }
                else
                {
                    base.GetComponent<Rigidbody>().AddForce((Vector3) (base.transform.forward * this.speed), ForceMode.Acceleration);
                    if (base.GetComponent<Rigidbody>().velocity.magnitude >= this.speed)
                    {
                        base.GetComponent<Rigidbody>().AddForce((Vector3) (-this.speed * base.GetComponent<Rigidbody>().velocity.normalized), ForceMode.Acceleration);
                    }
                }
                if (base.GetComponent<Rigidbody>().velocity.magnitude > 8f)
                {
                    if (!base.GetComponent<Animation>().IsPlaying("horse_Run"))
                    {
                        this.crossFade("horse_Run", 0.1f);
                    }
                    if (!this.myHero.GetComponent<Animation>().IsPlaying("horse_Run"))
                    {
                        this.myHero.GetComponent<HERO>().crossFade("horse_run", 0.1f);
                    }
                    if (!this.dust.GetComponent<ParticleSystem>().enableEmission)
                    {
                        this.dust.GetComponent<ParticleSystem>().enableEmission = true;
                        object[] parameters = new object[] { true };
                        base.photonView.RPC("setDust", PhotonTargets.Others, parameters);
                    }
                }
                else
                {
                    if (!base.GetComponent<Animation>().IsPlaying("horse_WALK"))
                    {
                        this.crossFade("horse_WALK", 0.1f);
                    }
                    if (!this.myHero.GetComponent<Animation>().IsPlaying("horse_idle"))
                    {
                        this.myHero.GetComponent<HERO>().crossFade("horse_idle", 0.1f);
                    }
                    if (this.dust.GetComponent<ParticleSystem>().enableEmission)
                    {
                        this.dust.GetComponent<ParticleSystem>().enableEmission = false;
                        object[] objArray2 = new object[] { false };
                        base.photonView.RPC("setDust", PhotonTargets.Others, objArray2);
                    }
                }
            }
            else
            {
                this.toIdleAnimation();
                if (base.GetComponent<Rigidbody>().velocity.magnitude > 15f)
                {
                    if (!this.myHero.GetComponent<Animation>().IsPlaying("horse_Run"))
                    {
                        this.myHero.GetComponent<HERO>().crossFade("horse_run", 0.1f);
                    }
                }
                else if (!this.myHero.GetComponent<Animation>().IsPlaying("horse_idle"))
                {
                    this.myHero.GetComponent<HERO>().crossFade("horse_idle", 0.1f);
                }
            }
            if ((this.controller.isAttackDown || this.controller.isAttackIIDown) && this.IsGrounded())
            {
                base.GetComponent<Rigidbody>().AddForce((Vector3) (Vector3.up * 25f), ForceMode.VelocityChange);
            }
        }
        else if (this.State == "follow")
        {
            if (this.myHero == null)
            {
                this.unmounted();
                return;
            }
            if (base.GetComponent<Rigidbody>().velocity.magnitude > 8f)
            {
                if (!base.GetComponent<Animation>().IsPlaying("horse_Run"))
                {
                    this.crossFade("horse_Run", 0.1f);
                }
                if (!this.dust.GetComponent<ParticleSystem>().enableEmission)
                {
                    this.dust.GetComponent<ParticleSystem>().enableEmission = true;
                    object[] objArray3 = new object[] { true };
                    base.photonView.RPC("setDust", PhotonTargets.Others, objArray3);
                }
            }
            else
            {
                if (!base.GetComponent<Animation>().IsPlaying("horse_WALK"))
                {
                    this.crossFade("horse_WALK", 0.1f);
                }
                if (this.dust.GetComponent<ParticleSystem>().enableEmission)
                {
                    this.dust.GetComponent<ParticleSystem>().enableEmission = false;
                    object[] objArray4 = new object[] { false };
                    base.photonView.RPC("setDust", PhotonTargets.Others, objArray4);
                }
            }
            float num = -Mathf.DeltaAngle(FengMath.getHorizontalAngle(base.transform.position, this.setPoint), base.gameObject.transform.rotation.eulerAngles.y - 90f);
            base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, base.gameObject.transform.rotation.eulerAngles.y + num, 0f), (200f * Time.deltaTime) / (base.GetComponent<Rigidbody>().velocity.magnitude + 20f));
            if (Vector3.Distance(this.setPoint, base.transform.position) < 20f)
            {
                base.GetComponent<Rigidbody>().AddForce((Vector3) ((base.transform.forward * this.speed) * 0.7f), ForceMode.Acceleration);
                if (base.GetComponent<Rigidbody>().velocity.magnitude >= this.speed)
                {
                    base.GetComponent<Rigidbody>().AddForce((Vector3) ((-this.speed * 0.7f) * base.GetComponent<Rigidbody>().velocity.normalized), ForceMode.Acceleration);
                }
            }
            else
            {
                base.GetComponent<Rigidbody>().AddForce((Vector3) (base.transform.forward * this.speed), ForceMode.Acceleration);
                if (base.GetComponent<Rigidbody>().velocity.magnitude >= this.speed)
                {
                    base.GetComponent<Rigidbody>().AddForce((Vector3) (-this.speed * base.GetComponent<Rigidbody>().velocity.normalized), ForceMode.Acceleration);
                }
            }
            this.timeElapsed += Time.deltaTime;
            if (this.timeElapsed > 0.6f)
            {
                this.timeElapsed = 0f;
                if (Vector3.Distance(this.myHero.transform.position, this.setPoint) > 20f)
                {
                    this.followed();
                }
            }
            if (Vector3.Distance(this.myHero.transform.position, base.transform.position) < 5f)
            {
                this.unmounted();
            }
            if (Vector3.Distance(this.setPoint, base.transform.position) < 5f)
            {
                this.unmounted();
            }
            this.awayTimer += Time.deltaTime;
            if (this.awayTimer > 6f)
            {
                this.awayTimer = 0f;
                LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("Ground");
                if (Physics.Linecast(base.transform.position + Vector3.up, this.myHero.transform.position + Vector3.up, mask2.value))
                {
                    base.transform.position = new Vector3(this.myHero.transform.position.x, this.getHeight(this.myHero.transform.position + ((Vector3) (Vector3.up * 5f))), this.myHero.transform.position.z);
                }
            }
        }
        else if (this.State == "idle")
        {
            this.toIdleAnimation();
            if ((this.myHero != null) && (Vector3.Distance(this.myHero.transform.position, base.transform.position) > 20f))
            {
                this.followed();
            }
        }
        base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -50f * base.GetComponent<Rigidbody>().mass, 0f));
    }

    public void mounted()
    {
        this.State = "mounted";
        base.gameObject.GetComponent<TITAN_CONTROLLER>().enabled = true;
    }

    [PunRPC]
    private void netCrossFade(string aniName, float time)
    {
        base.GetComponent<Animation>().CrossFade(aniName, time);
    }

    [PunRPC]
    private void netPlayAnimation(string aniName)
    {
        base.GetComponent<Animation>().Play(aniName);
    }

    [PunRPC]
    private void netPlayAnimationAt(string aniName, float normalizedTime)
    {
        base.GetComponent<Animation>().Play(aniName);
        base.GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
    }

    public void playAnimation(string aniName)
    {
        base.GetComponent<Animation>().Play(aniName);
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName };
            base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
        }
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        base.GetComponent<Animation>().Play(aniName);
        base.GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName, normalizedTime };
            base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
        }
    }

    [PunRPC]
    private void setDust(bool enable)
    {
        if (this.dust.GetComponent<ParticleSystem>().enableEmission)
        {
            this.dust.GetComponent<ParticleSystem>().enableEmission = enable;
        }
    }

    private void Start()
    {
        this.controller = base.gameObject.GetComponent<TITAN_CONTROLLER>();
    }

    private void toIdleAnimation()
    {
        if (base.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            if (base.GetComponent<Rigidbody>().velocity.magnitude > 15f)
            {
                if (!base.GetComponent<Animation>().IsPlaying("horse_Run"))
                {
                    this.crossFade("horse_Run", 0.1f);
                }
                if (!this.dust.GetComponent<ParticleSystem>().enableEmission)
                {
                    this.dust.GetComponent<ParticleSystem>().enableEmission = true;
                    object[] parameters = new object[] { true };
                    base.photonView.RPC("setDust", PhotonTargets.Others, parameters);
                }
            }
            else
            {
                if (!base.GetComponent<Animation>().IsPlaying("horse_WALK"))
                {
                    this.crossFade("horse_WALK", 0.1f);
                }
                if (this.dust.GetComponent<ParticleSystem>().enableEmission)
                {
                    this.dust.GetComponent<ParticleSystem>().enableEmission = false;
                    object[] objArray2 = new object[] { false };
                    base.photonView.RPC("setDust", PhotonTargets.Others, objArray2);
                }
            }
        }
        else
        {
            if (base.GetComponent<Animation>().IsPlaying("horse_idle1") && (base.GetComponent<Animation>()["horse_idle1"].normalizedTime >= 1f))
            {
                this.crossFade("horse_idle0", 0.1f);
            }
            if (base.GetComponent<Animation>().IsPlaying("horse_idle2") && (base.GetComponent<Animation>()["horse_idle2"].normalizedTime >= 1f))
            {
                this.crossFade("horse_idle0", 0.1f);
            }
            if (base.GetComponent<Animation>().IsPlaying("horse_idle3") && (base.GetComponent<Animation>()["horse_idle3"].normalizedTime >= 1f))
            {
                this.crossFade("horse_idle0", 0.1f);
            }
            if ((!base.GetComponent<Animation>().IsPlaying("horse_idle0") && !base.GetComponent<Animation>().IsPlaying("horse_idle1")) && (!base.GetComponent<Animation>().IsPlaying("horse_idle2") && !base.GetComponent<Animation>().IsPlaying("horse_idle3")))
            {
                this.crossFade("horse_idle0", 0.1f);
            }
            if (base.GetComponent<Animation>().IsPlaying("horse_idle0"))
            {
                int num = UnityEngine.Random.Range(0, 0x2710);
                if (num < 10)
                {
                    this.crossFade("horse_idle1", 0.1f);
                }
                else if (num < 20)
                {
                    this.crossFade("horse_idle2", 0.1f);
                }
                else if (num < 30)
                {
                    this.crossFade("horse_idle3", 0.1f);
                }
            }
            if (this.dust.GetComponent<ParticleSystem>().enableEmission)
            {
                this.dust.GetComponent<ParticleSystem>().enableEmission = false;
                object[] objArray3 = new object[] { false };
                base.photonView.RPC("setDust", PhotonTargets.Others, objArray3);
            }
            base.GetComponent<Rigidbody>().AddForce(-base.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
        }
    }

    public void unmounted()
    {
        this.State = "idle";
        base.gameObject.GetComponent<TITAN_CONTROLLER>().enabled = false;
    }
}

