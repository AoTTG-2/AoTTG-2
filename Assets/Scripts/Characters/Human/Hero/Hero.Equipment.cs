using Assets.Scripts.Settings;
using System;
using UnityEngine;

public partial class Hero
{

    enum Side
    {
        Left,
        Right
    }

    public void Launch(Vector3 des, bool left = true, bool leviMode = false)
    {
        if (isMounted)
        {
            Unmounted();
        }
        if (State != HERO_STATE.Attack)
        {
            Idle();
        }
        var vector = des - transform.position;
        if (left)
        {
            launchPointLeft = des;
        }
        else
        {
            launchPointRight = des;
        }
        vector.Normalize();
        vector = (vector * 20f);
        if (((bulletLeft != null) && (bulletRight != null)) && (bulletLeft.isHooked() && bulletRight.isHooked()))
        {
            vector = (vector * 0.8f);
        }
        if (!animation.IsPlaying(ANIM_ATTACK_5) && !animation.IsPlaying(ANIM_SPECIAL_PETRA))
        {
            leviMode = false;
        }
        else
        {
            leviMode = true;
        }
        if (!leviMode)
        {
            FalseAttack();
            Idle();
            if (useGun)
            {
                CrossFade(AHSS_HOOK_FWD_BOTH, 0.1f);
            }
            else if (left && !isRightHandHooked)
            {
                CrossFade(ANIM_AIR_HOOK_L_JUST, 0.1f);
            }
            else if (!left && !isLeftHandHooked)
            {
                CrossFade(ANIM_AIR_HOOK_R_JUST, 0.1f);
            }
            else
            {
                CrossFade(ANIM_DASH, 0.1f);
                animation[ANIM_DASH].time = 0f;
            }
        }

        isLaunchLeft = left;
        isLaunchRight = !left;

        launchForce = vector;
        if (!leviMode)
        {
            if (vector.y < 30f)
            {
                launchForce += (Vector3.up * (30f - vector.y));
            }
            if (des.y >= transform.position.y)
            {
                launchForce += ((Vector3.up * (des.y - transform.position.y)) * 10f);
            }
            rigidBody.AddForce(launchForce);
        }
        facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * Mathf.Rad2Deg;
        Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
        transform.rotation = quaternion;
        rigidBody.rotation = quaternion;
        targetRotation = quaternion;
        if (left)
        {
            launchElapsedTimeL = 0f;
        }
        else
        {
            launchElapsedTimeR = 0f;
        }
        if (leviMode)
        {
            launchElapsedTimeR = -100f;
        }
        if (animation.IsPlaying(ANIM_SPECIAL_PETRA))
        {
            launchElapsedTimeR = -100f;
            launchElapsedTimeL = -100f;
            if (bulletRight != null)
            {
                bulletRight.disable();
                ReleaseIfIHookSb();
            }
            if (bulletLeft != null)
            {
                bulletLeft.disable();
                ReleaseIfIHookSb();
            }
        }
        sparksEmission.enabled = false;
    }

    private void LaunchRope(Side side, float distance, Vector3 point, bool single)
    {
        if (currentGas == 0f)
        {
            return;
        }
        UseGas(0f);
        var bullet = PhotonNetwork.Instantiate(HOOK_STRING, transform.position, transform.rotation, 0).GetComponent<Bullet>();
        GameObject hookRef = null;
        int mult = 0;
        switch (side)
        {
            case Side.Left:
                bulletLeft = bullet;
                hookRef = !useGun ? hookRefL1 : hookRefL2;
                mult = -1;
                break;
            case Side.Right:
                bulletRight = bullet;
                hookRef = !useGun ? hookRefR1 : hookRefR2;
                mult = 1;
                break;
        }


        bullet.transform.position = hookRef.transform.position;

        float num = !single ? ((distance <= 50f) ? (distance * 0.05f) : (distance * 0.3f)) : 0f;
        var vector = point + (transform.right * num * mult) - bullet.transform.position;
        bullet.launch((vector.normalized * 3f), rigidBody.velocity, hookRef, side == Side.Left, gameObject, false);
        switch (side)
        {
            case Side.Left:
                launchPointLeft = Vector3.zero;
                break;
            case Side.Right:
                launchPointRight = Vector3.zero;
                break;
        }
    }

    private void LaunchLeftRope(float distance, Vector3 point, bool single)
    {
        LaunchRope(Side.Left, distance, point, single);
    }

    private void LaunchRightRope(float distance, Vector3 point, bool single)
    {
        LaunchRope(Side.Right, distance, point, single);
    }


    private void ChangeBlade()
    {
        if ((!useGun || grounded) || GameSettings.PvP.AhssAirReload.Value)
        {
            State = HERO_STATE.ChangeBlade;
            throwedBlades = false;
            Equipment.Weapon.PlayReloadAnimation();
        }
    }

    public void FillGas()
    {
        currentGas = totalGas;
    }


    public void GetSupply()
    {
        if (((animation.IsPlaying(standAnimation) || animation.IsPlaying(ANIM_RUN_1)) || animation.IsPlaying(ANIM_RUN_SASHA)) &&
            (((currentBladeSta != totalBladeSta) || (currentBladeNum != totalBladeNum)) || (((currentGas != totalGas) || (leftBulletLeft != bulletMAX)) || (rightBulletLeft != bulletMAX))))
        {
            State = HERO_STATE.FillGas;
            CrossFade(SUPPLY_STRING, 0.1f);
        }
    }


    [Obsolete("Using a weapon should be moved within Weapon class...")]
    private void UseGas(float amount = 0)
    {
        if (amount == 0f)
        {
            amount = useGasSpeed;
        }
        if (currentGas > 0f)
        {
            currentGas -= amount;
            if (currentGas < 0f)
            {
                currentGas = 0f;
            }
        }
    }

}
