using UnityEngine;

public class Horse : PhotonView
{
    private new Animation animation;

    private float awayTimer;

    private HorseController controller;

    [SerializeField]
    private ParticleSystem dustParticles;

    [SerializeField]
    private float gravityFactor = -20f;

    private LayerMask groundMask;
    private Hero hero;
    private Animation heroAnimation;
    private Rigidbody heroRigidbody;
    private Transform heroTransform;
    private LayerMask isGroundedMask;
    private new Rigidbody rigidbody;
    private Vector3 setPoint;

    private float speed = 45f;

    private string state = "idle";

    private float timeElapsed;

    public Hero MyHero
    {
        get
        {
            return hero;
        }

        set
        {
            hero = value;
            heroTransform = hero.transform;
            heroAnimation = hero.GetComponent<Animation>();
            heroRigidbody = hero.GetComponent<Rigidbody>();
        }
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(gameObject.transform.position + Vector3.up * 0.1f, -Vector3.up, (float) 0.3f, isGroundedMask.value);
    }

    public void Mounted()
    {
        state = "mounted";
        controller.enabled = true;
    }

    public void Unmounted()
    {
        state = "idle";
        controller.enabled = false;
    }

    private void CrossFade(string aniName, float time)
    {
        animation.CrossFade(aniName, time);
        if (PhotonNetwork.connected && photonView.isMine)
            photonView.RPC("netCrossFade", PhotonTargets.Others, aniName, time);
    }

    private void Followed()
    {
        if (MyHero != null)
        {
            state = "follow";
            setPoint = (heroTransform.position + (Vector3.right * Random.Range(-6, 6))) + (Vector3.forward * Random.Range(-6, 6));
            setPoint.y = GetHeight(setPoint + Vector3.up * 5f);
            awayTimer = 0f;
        }
    }

    private float GetHeight(Vector3 pt)
    {
        RaycastHit hit;
        if (Physics.Raycast(pt, -Vector3.up, out hit, 1000f, groundMask.value))
            return hit.point.y;

        return 0f;
    }

    private void LateUpdate()
    {
        if (MyHero == null && photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
            return;
        }

        if (state == "mounted")
        {
            if (MyHero == null)
            {
                Unmounted();
                return;
            }

            heroTransform.position = transform.position + Vector3.up * 1.68f;
            heroTransform.rotation = transform.rotation;
            heroRigidbody.velocity = rigidbody.velocity;

            if (controller.TargetDirection != -874f)
            {
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, controller.TargetDirection, 0f), (100f * Time.deltaTime) / (rigidbody.velocity.magnitude + 20f));
                if (controller.ShouldWalk)
                {
                    rigidbody.AddForce((transform.forward * speed) * 0.6f, ForceMode.Acceleration);
                    if (rigidbody.velocity.magnitude >= (speed * 0.6f))
                        rigidbody.AddForce((-speed * 0.6f) * rigidbody.velocity.normalized, ForceMode.Acceleration);
                }
                else
                {
                    rigidbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
                    if (rigidbody.velocity.magnitude >= speed)
                        rigidbody.AddForce(-speed * rigidbody.velocity.normalized, ForceMode.Acceleration);
                }

                if (rigidbody.velocity.magnitude > 8f)
                {
                    if (!animation.IsPlaying("horse_Run"))
                        CrossFade("horse_Run", 0.1f);

                    if (!heroAnimation.IsPlaying("horse_Run"))
                        MyHero.crossFade("horse_run", 0.1f);

                    EnableDust();
                }
                else
                {
                    if (!animation.IsPlaying("horse_WALK"))
                        CrossFade("horse_WALK", 0.1f);

                    if (!heroAnimation.IsPlaying("horse_idle"))
                        MyHero.crossFade("horse_idle", 0.1f);

                    DisableDust();
                }
            }
            else
            {
                ToIdleAnimation();
                if (rigidbody.velocity.magnitude > 15f)
                {
                    if (!heroAnimation.IsPlaying("horse_Run"))
                        MyHero.crossFade("horse_run", 0.1f);
                }
                else if (!heroAnimation.IsPlaying("horse_idle"))
                    MyHero.crossFade("horse_idle", 0.1f);
            }
            if (controller.ShouldJump && IsGrounded())
                rigidbody.AddForce(Vector3.up * 25f, ForceMode.VelocityChange);
        }
        else if (state == "follow")
        {
            if (MyHero == null)
            {
                Unmounted();
                return;
            }

            if (rigidbody.velocity.magnitude > 8f)
            {
                if (!animation.IsPlaying("horse_Run"))
                    CrossFade("horse_Run", 0.1f);

                EnableDust();
            }
            else
            {
                if (!animation.IsPlaying("horse_WALK"))
                    CrossFade("horse_WALK", 0.1f);

                DisableDust();
            }

            var horizontalVector = setPoint - transform.position;
            var horizontalAngle = -Mathf.Atan2(horizontalVector.z, horizontalVector.x) * 57.29578f;
            var num = -Mathf.DeltaAngle(horizontalAngle, gameObject.transform.rotation.eulerAngles.y - 90f);
            gameObject.transform.rotation = Quaternion.Lerp(
                gameObject.transform.rotation,
                Quaternion.Euler(0f, gameObject.transform.rotation.eulerAngles.y + num, 0f),
                (200f * Time.deltaTime) / (GetComponent<Rigidbody>().velocity.magnitude + 20f));
            
            if (Vector3.Distance(setPoint, transform.position) < 20f)
            {
                rigidbody.AddForce((transform.forward * speed) * 0.7f, ForceMode.Acceleration);
                if (rigidbody.velocity.magnitude >= speed)
                    rigidbody.AddForce((-speed * 0.7f) * rigidbody.velocity.normalized, ForceMode.Acceleration);
            }
            else
            {
                rigidbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
                if (rigidbody.velocity.magnitude >= speed)
                    rigidbody.AddForce(-speed * rigidbody.velocity.normalized, ForceMode.Acceleration);
            }

            timeElapsed += Time.deltaTime;
            if (timeElapsed > 0.6f)
            {
                timeElapsed = 0f;
                if (Vector3.Distance(heroTransform.position, setPoint) > 20f)
                    Followed();
            }

            if (Vector3.Distance(heroTransform.position, transform.position) < 5f)
                Unmounted();

            if (Vector3.Distance(setPoint, transform.position) < 5f)
                Unmounted();

            awayTimer += Time.deltaTime;
            if (awayTimer > 6f)
            {
                awayTimer = 0f;
                if (Physics.Linecast(transform.position + Vector3.up, heroTransform.position + Vector3.up, groundMask.value))
                    transform.position = new Vector3(
                        heroTransform.position.x,
                        GetHeight(heroTransform.position + Vector3.up * 5f),
                        heroTransform.position.z);
            }
        }
        else if (state == "idle")
        {
            ToIdleAnimation();
            var heroDistance = Vector3.Distance(heroTransform.position, transform.position);
            if (MyHero && heroDistance > 20f)
                Followed();
        }

        rigidbody.AddForce(new Vector3(0f, gravityFactor * rigidbody.mass, 0f));
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

    private void Reset()
    {
        dustParticles = GetComponentInChildren<ParticleSystem>();
    }

    [PunRPC]
    private void setDust(bool enable)
    {
        if (dustParticles.enableEmission)
            dustParticles.enableEmission = enable;
    }

    private void Start()
    {
        animation = GetComponent<Animation>();
        rigidbody = GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<HorseController>();

        LayerMask enemyBoxMask = 1 << LayerMask.NameToLayer("EnemyBox");
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        isGroundedMask = enemyBoxMask | groundMask;
    }

    private void ToIdleAnimation()
    {
        if (rigidbody.velocity.magnitude > 0.1f)
        {
            if (rigidbody.velocity.magnitude > 15f)
            {
                if (!animation.IsPlaying("horse_Run"))
                    CrossFade("horse_Run", 0.1f);

                EnableDust();
            }
            else
            {
                if (!animation.IsPlaying("horse_WALK"))
                    CrossFade("horse_WALK", 0.1f);

                DisableDust();
            }
        }
        else
        {
            if (animation.IsPlaying("horse_idle1") && (animation["horse_idle1"].normalizedTime >= 1f))
                CrossFade("horse_idle0", 0.1f);

            if (animation.IsPlaying("horse_idle2") && (animation["horse_idle2"].normalizedTime >= 1f))
                CrossFade("horse_idle0", 0.1f);

            if (animation.IsPlaying("horse_idle3") && (animation["horse_idle3"].normalizedTime >= 1f))
                CrossFade("horse_idle0", 0.1f);

            if ((!animation.IsPlaying("horse_idle0") && !animation.IsPlaying("horse_idle1")) && (!animation.IsPlaying("horse_idle2") && !animation.IsPlaying("horse_idle3")))
                CrossFade("horse_idle0", 0.1f);

            if (animation.IsPlaying("horse_idle0"))
            {
                switch (Random.Range(0, 1000))
                {
                    case 0:
                        CrossFade("horse_idle1", 0.1f);
                        break;

                    case 1:
                        CrossFade("horse_idle2", 0.1f);
                        break;

                    case 2:
                        CrossFade("horse_idle3", 0.1f);
                        break;

                    default:
                        break;
                }
            }

            DisableDust();

            rigidbody.AddForce(-rigidbody.velocity, ForceMode.VelocityChange);
        }
    }

    private void EnableDust()
    {
        if (!dustParticles.enableEmission)
        {
            dustParticles.enableEmission = true;
            photonView.RPC("setDust", PhotonTargets.Others, true);
        }
    }

    private void DisableDust()
    {
        if (dustParticles.enableEmission)
        {
            dustParticles.enableEmission = false;
            photonView.RPC("setDust", PhotonTargets.Others, false);
        }
    }
}