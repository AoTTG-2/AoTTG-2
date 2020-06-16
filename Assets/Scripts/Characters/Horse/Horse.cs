using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Horse : PhotonView
{
    private float awayTimer;
    private HorseController controller;
    private new Animation animation;
    private new Rigidbody rigidbody;
    public GameObject dust;
    public GameObject myHeroGobj;
    private Vector3 setPoint;
    private float speed = 45f;
    private string State = "idle";
    private float timeElapsed;

    private void crossFade(string aniName, float time)
    {
        animation.CrossFade(aniName, time);
        if (PhotonNetwork.connected && photonView.isMine)
        {
            photonView.RPC<string, float>(netCrossFade, PhotonTargets.Others, aniName, time);
        }
    }

    private void followed()
    {
        if (myHeroGobj != null)
        {
            State = "follow";
            setPoint = (myHeroGobj.transform.position + (Vector3.right * Random.Range(-6, 6))) + (Vector3.forward * Random.Range(-6, 6));
            setPoint.y = getHeight(setPoint + ((Vector3) (Vector3.up * 5f)));
            awayTimer = 0f;
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
        return Physics.Raycast(gameObject.transform.position + ((Vector3) (Vector3.up * 0.1f)), -Vector3.up, (float) 0.3f, mask3.value);
    }

    private void LateUpdate()
    {
        if ((myHeroGobj == null) && photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        if (State == "mounted")
        {
            if (myHeroGobj == null)
            {
                unmounted();
                return;
            }
            myHeroGobj.transform.position = transform.position + ((Vector3) (Vector3.up * 1.68f));
            myHeroGobj.transform.rotation = transform.rotation;
            var myHero = myHeroGobj.GetComponent<Hero>();
            var myHeroRigidbody = myHeroGobj.GetComponent<Rigidbody>();
            var myHeroAnimation = myHeroGobj.GetComponent<Animation>();
            myHeroRigidbody.velocity = rigidbody.velocity;
            if (controller.targetDirection != -874f)
            {
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, controller.targetDirection, 0f), (100f * Time.deltaTime) / (rigidbody.velocity.magnitude + 20f));
                if (controller.isWALKDown)
                {
                    rigidbody.AddForce((Vector3) ((transform.forward * speed) * 0.6f), ForceMode.Acceleration);
                    if (rigidbody.velocity.magnitude >= (speed * 0.6f))
                    {
                        rigidbody.AddForce((Vector3) ((-speed * 0.6f) * rigidbody.velocity.normalized), ForceMode.Acceleration);
                    }
                }
                else
                {
                    rigidbody.AddForce((Vector3) (transform.forward * speed), ForceMode.Acceleration);
                    if (rigidbody.velocity.magnitude >= speed)
                    {
                        rigidbody.AddForce((Vector3) (-speed * rigidbody.velocity.normalized), ForceMode.Acceleration);
                    }
                }
                if (rigidbody.velocity.magnitude > 8f)
                {
                    if (!animation.IsPlaying("horse_Run"))
                    {
                        crossFade("horse_Run", 0.1f);
                    }
                    if (!myHeroAnimation.IsPlaying("horse_Run"))
                    {
                        myHero.crossFade("horse_run", 0.1f);
                    }
                    if (!dust.GetComponent<ParticleSystem>().enableEmission)
                    {
                        dust.GetComponent<ParticleSystem>().enableEmission = true;
                        object[] parameters = new object[] { true };
                        photonView.RPC<bool>(setDust, PhotonTargets.Others, parameters);
                    }
                }
                else
                {
                    if (!animation.IsPlaying("horse_WALK"))
                    {
                        crossFade("horse_WALK", 0.1f);
                    }
                    if (!myHeroAnimation.IsPlaying("horse_idle"))
                    {
                        myHero.crossFade("horse_idle", 0.1f);
                    }
                    if (dust.GetComponent<ParticleSystem>().enableEmission)
                    {
                        dust.GetComponent<ParticleSystem>().enableEmission = false;
                        object[] objArray2 = new object[] { false };
                        photonView.RPC<bool>(setDust, PhotonTargets.Others, objArray2);
                    }
                }
            }
            else
            {
                toIdleAnimation();
                if (rigidbody.velocity.magnitude > 15f)
                {
                    if (!myHeroAnimation.IsPlaying("horse_Run"))
                    {
                        myHero.crossFade("horse_run", 0.1f);
                    }
                }
                else if (!myHeroAnimation.IsPlaying("horse_idle"))
                {
                    myHero.crossFade("horse_idle", 0.1f);
                }
            }
            if ((controller.isAttackDown || controller.isAttackIIDown) && IsGrounded())
            {
                rigidbody.AddForce((Vector3) (Vector3.up * 25f), ForceMode.VelocityChange);
            }
        }
        else if (State == "follow")
        {
            if (myHeroGobj == null)
            {
                unmounted();
                return;
            }
            if (rigidbody.velocity.magnitude > 8f)
            {
                if (!animation.IsPlaying("horse_Run"))
                {
                    crossFade("horse_Run", 0.1f);
                }
                if (!dust.GetComponent<ParticleSystem>().enableEmission)
                {
                    dust.GetComponent<ParticleSystem>().enableEmission = true;
                    photonView.RPC<bool>(setDust, PhotonTargets.Others, true);
                }
            }
            else
            {
                if (!animation.IsPlaying("horse_WALK"))
                {
                    crossFade("horse_WALK", 0.1f);
                }
                if (dust.GetComponent<ParticleSystem>().enableEmission)
                {
                    dust.GetComponent<ParticleSystem>().enableEmission = false;
                    photonView.RPC<bool>(setDust, PhotonTargets.Others, false);
                }
            }

            var horizontalVector = setPoint - transform.position;
            var horizontalAngle = -Mathf.Atan2(horizontalVector.z, horizontalVector.x) * 57.29578f;
            float num = -Mathf.DeltaAngle(horizontalAngle, gameObject.transform.rotation.eulerAngles.y - 90f);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, gameObject.transform.rotation.eulerAngles.y + num, 0f), (200f * Time.deltaTime) / (rigidbody.velocity.magnitude + 20f));
            if (Vector3.Distance(setPoint, transform.position) < 20f)
            {
                rigidbody.AddForce((Vector3) ((transform.forward * speed) * 0.7f), ForceMode.Acceleration);
                if (rigidbody.velocity.magnitude >= speed)
                {
                    rigidbody.AddForce((Vector3) ((-speed * 0.7f) * rigidbody.velocity.normalized), ForceMode.Acceleration);
                }
            }
            else
            {
                rigidbody.AddForce((Vector3) (transform.forward * speed), ForceMode.Acceleration);
                if (rigidbody.velocity.magnitude >= speed)
                {
                    rigidbody.AddForce((Vector3) (-speed * rigidbody.velocity.normalized), ForceMode.Acceleration);
                }
            }
            timeElapsed += Time.deltaTime;
            if (timeElapsed > 0.6f)
            {
                timeElapsed = 0f;
                if (Vector3.Distance(myHeroGobj.transform.position, setPoint) > 20f)
                {
                    followed();
                }
            }
            if (Vector3.Distance(myHeroGobj.transform.position, transform.position) < 5f)
            {
                unmounted();
            }
            if (Vector3.Distance(setPoint, transform.position) < 5f)
            {
                unmounted();
            }
            awayTimer += Time.deltaTime;
            if (awayTimer > 6f)
            {
                awayTimer = 0f;
                LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("Ground");
                if (Physics.Linecast(transform.position + Vector3.up, myHeroGobj.transform.position + Vector3.up, mask2.value))
                {
                    transform.position = new Vector3(myHeroGobj.transform.position.x, getHeight(myHeroGobj.transform.position + ((Vector3) (Vector3.up * 5f))), myHeroGobj.transform.position.z);
                }
            }
        }
        else if (State == "idle")
        {
            toIdleAnimation();
            if ((myHeroGobj != null) && (Vector3.Distance(myHeroGobj.transform.position, transform.position) > 20f))
            {
                followed();
            }
        }
        rigidbody.AddForce(new Vector3(0f, -50f * rigidbody.mass, 0f));
    }

    public void mounted()
    {
        State = "mounted";
        controller.enabled = true;
    }

    [PunRPC]
    private void netCrossFade(string aniName, float time)
    {
        animation.CrossFade(aniName, time);
    }

    [PunRPC]
    private void netPlayAnimation(string aniName)
    {
        animation.Play(aniName);
    }

    [PunRPC]
    private void netPlayAnimationAt(string aniName, float normalizedTime)
    {
        animation.Play(aniName);
        animation[aniName].normalizedTime = normalizedTime;
    }

    public void playAnimation(string aniName)
    {
        animation.Play(aniName);
        if (PhotonNetwork.connected && photonView.isMine)
            photonView.RPC<string>(netPlayAnimation, PhotonTargets.Others, aniName);
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        animation.Play(aniName);
        animation[aniName].normalizedTime = normalizedTime;
        if (PhotonNetwork.connected && photonView.isMine)
            photonView.RPC<string, float>(
                netPlayAnimationAt,
                PhotonTargets.Others,
                aniName,
                normalizedTime);
    }

    [PunRPC, Obsolete]
    private void setDust(bool enable)
    {
        var particleSystem = dust.GetComponent<ParticleSystem>();
        if (particleSystem.enableEmission)
            particleSystem.enableEmission = enable;
    }

    private void Start()
    {
        controller = GetComponent<HorseController>();
        animation = GetComponent<Animation>();
    }

    private void toIdleAnimation()
    {
        if (rigidbody.velocity.magnitude > 0.1f)
        {
            if (rigidbody.velocity.magnitude > 15f)
            {
                if (!animation.IsPlaying("horse_Run"))
                {
                    crossFade("horse_Run", 0.1f);
                }
                if (!dust.GetComponent<ParticleSystem>().enableEmission)
                {
                    dust.GetComponent<ParticleSystem>().enableEmission = true;
                    object[] parameters = new object[] { true };
                    photonView.RPC<bool>(setDust, PhotonTargets.Others, parameters);
                }
            }
            else
            {
                if (!animation.IsPlaying("horse_WALK"))
                {
                    crossFade("horse_WALK", 0.1f);
                }
                if (dust.GetComponent<ParticleSystem>().enableEmission)
                {
                    dust.GetComponent<ParticleSystem>().enableEmission = false;
                    object[] objArray2 = new object[] { false };
                    photonView.RPC<bool>(setDust, PhotonTargets.Others, objArray2);
                }
            }
        }
        else
        {
            if (animation.IsPlaying("horse_idle1") && (animation["horse_idle1"].normalizedTime >= 1f))
            {
                crossFade("horse_idle0", 0.1f);
            }
            if (animation.IsPlaying("horse_idle2") && (animation["horse_idle2"].normalizedTime >= 1f))
            {
                crossFade("horse_idle0", 0.1f);
            }
            if (animation.IsPlaying("horse_idle3") && (animation["horse_idle3"].normalizedTime >= 1f))
            {
                crossFade("horse_idle0", 0.1f);
            }
            if ((!animation.IsPlaying("horse_idle0") && !animation.IsPlaying("horse_idle1")) && (!animation.IsPlaying("horse_idle2") && !animation.IsPlaying("horse_idle3")))
            {
                crossFade("horse_idle0", 0.1f);
            }
            if (animation.IsPlaying("horse_idle0"))
            {
                int num = Random.Range(0, 0x2710);
                if (num < 10)
                {
                    crossFade("horse_idle1", 0.1f);
                }
                else if (num < 20)
                {
                    crossFade("horse_idle2", 0.1f);
                }
                else if (num < 30)
                {
                    crossFade("horse_idle3", 0.1f);
                }
            }
            if (dust.GetComponent<ParticleSystem>().enableEmission)
            {
                dust.GetComponent<ParticleSystem>().enableEmission = false;
                photonView.RPC<bool>(setDust, PhotonTargets.Others, false);
            }
            rigidbody.AddForce(-rigidbody.velocity, ForceMode.VelocityChange);
        }
    }   

    public void unmounted()
    {
        State = "idle";
        controller.enabled = false;
    }
}

