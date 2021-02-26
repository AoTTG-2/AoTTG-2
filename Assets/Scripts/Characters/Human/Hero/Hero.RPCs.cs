using Assets.Scripts;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Hero
{
    #region RPCs
    [PunRPC]
    private void BackToHumanRPC()
    {
        titanForm = false;
        erenTitanGameObject = null;
        smoothSyncMovement.disabled = false;
    }

    [PunRPC]
    public void BadGuyReleaseMe()
    {
        hookBySomeOne = false;
        badGuy = null;
    }

    [PunRPC]
    public void BlowAway(Vector3 force)
    {
        if (photonView.isMine)
        {
            rigidBody.AddForce(force, ForceMode.Impulse);
            transform.LookAt(transform.position);
        }
    }

    [PunRPC]
    public void HookFail()
    {
        hookTarget = null;
        hookSomeOne = false;
    }

    [PunRPC]
    public void LoadSkinRPC(int horse, string url)
    {
        if (((int) FengGameManagerMKII.settings[0]) == 1)
        {
            StartCoroutine(LoadSkinE(horse, url));
        }
    }

    [PunRPC]
    private void Net3DMGSMOKE(bool ifON)
    {
        if (smoke_3dmg != null)
        {
            smoke_3dmgEmission.enabled = ifON;
        }
    }

    [PunRPC]
    private void NetContinueAnimation()
    {
        IEnumerator enumerator = animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null && current.speed == 1f)
                {
                    return;
                }
                current.speed = 1f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        PlayAnimation(CurrentPlayingClipName());
    }

    [PunRPC]
    private void NetCrossFade(string aniName, float time)
    {
        currentAnimation = aniName;
        if (animation != null)
        {
            animation.CrossFade(aniName, time);
        }
    }

    [PunRPC]
    public void NetDie(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true, PhotonMessageInfo info = new PhotonMessageInfo())
    {
        if ((photonView.isMine && (GameSettings.Gamemode.GamemodeType != GamemodeType.TitanRush)))
        {
            if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
            {
                photonView.RPC(nameof(BackToHumanRPC), PhotonTargets.Others, new object[0]);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if ((info.sender.CustomProperties[PhotonPlayerProperty.name] == null) || (info.sender.CustomProperties[PhotonPlayerProperty.isTitan] == null))
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (viewID < 0)
                {
                    if (titanName == "")
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + " (possibly valid).</color>");
                    }
                    else
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                    }
                }
                else if (PhotonView.Find(viewID) == null)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
            }
        }
        if (PhotonNetwork.isMasterClient)
        {
            int iD = photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
        if (photonView.isMine)
        {
            Vector3 vector = (Vector3.up * 5000f);
            if (myBomb != null)
            {
                myBomb.destroyMe();
            }
            if (myCannon != null)
            {
                PhotonNetwork.Destroy(myCannon);
            }
            if (titanForm && (erenTitanGameObject != null))
            {
                erenTitanGameObject.GetComponent<ErenTitan>().lifeTime = 0.1f;
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = vector;
            }
        }
        if (bulletLeft != null)
        {
            bulletLeft.removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.removeMe();
        }

        if (!(useGun || (!photonView.isMine)))
        {
            //TODO: Re-enable these again
            //leftbladetrail.Deactivate();
            //rightbladetrail.Deactivate();
            //leftbladetrail2.Deactivate();
            //rightbladetrail2.Deactivate();
        }
        FalseAttack();
        BreakApart(v, isBite);
        if (photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        hasDied = true;
        audioSystem
            .PlayOneShot(audioSystem.clipDie)
            .Disconnect(audioSystem.clipDie);

        smoothSyncMovement.disabled = true;
        if (photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            if (viewID != -1)
            {
                PhotonView view2 = PhotonView.Find(viewID);
                if (view2 != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(killByTitan, $"<color=#ffc000>[{info.sender.ID}]</color> " + RCextensions.returnStringFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                    view2.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), $"<color=#ffc000>[{info.sender.ID}]</color> " + titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
            }
        }
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(photonView);
        }
    }

    [PunRPC]
    public void NetDie2(int viewID = -1, string titanName = "", PhotonMessageInfo info = new PhotonMessageInfo())
    {
        GameObject obj2;
        if ((photonView.isMine) && (GameSettings.Gamemode.GamemodeType != GamemodeType.TitanRush))
        {
            if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
            {
                photonView.RPC(nameof(BackToHumanRPC), PhotonTargets.Others, new object[0]);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if ((info.sender.CustomProperties[PhotonPlayerProperty.name] == null) || (info.sender.CustomProperties[PhotonPlayerProperty.isTitan] == null))
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (viewID < 0)
                {
                    if (titanName == "")
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + " (possibly valid).</color>");
                    }
                    else if (GameSettings.PvP.Bomb.Value && (!GameSettings.PvP.Cannons.Value))
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                    }
                }
                else if (PhotonView.Find(viewID) == null)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
            }
        }
        if (photonView.isMine)
        {
            Vector3 vector = (Vector3.up * 5000f);
            if (myBomb != null)
            {
                myBomb.destroyMe();
            }
            if (myCannon != null)
            {
                PhotonNetwork.Destroy(myCannon);
            }
            PhotonNetwork.RemoveRPCs(photonView);
            if (titanForm && (erenTitanGameObject != null))
            {
                erenTitan.lifeTime = 0.1f;
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = vector;
            }
        }

        if (bulletLeft != null)
        {
            bulletLeft.removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.removeMe();
        }
        audioSystem
            .PlayOneShot(audioSystem.clipDie)
            .Disconnect(audioSystem.clipDie);

        if (photonView.isMine)
        {
            var cam = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>();

            cam.SetMainObject(null, true, false);
            cam.SetSpectorMode(true);
            cam.gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        FalseAttack();
        hasDied = true;
        smoothSyncMovement.disabled = true;
        if (photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, ((int) PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            if (viewID != -1)
            {
                PhotonView view2 = PhotonView.Find(viewID);
                if (view2 != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(true, $"<color=#ffc000>[{info.sender.ID}]</color> " + RCextensions.returnStringFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                    view2.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(true, $"<color=#ffc000>[{info.sender.ID}]</color> " + titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
            }
        }
        if (photonView.isMine)
        {
            obj2 = PhotonNetwork.Instantiate("hitMeat2", audioSystem.Position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject) Instantiate(Resources.Load("hitMeat2"));
        }
        obj2.transform.position = audioSystem.Position;
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(photonView);
        }
        if (PhotonNetwork.isMasterClient)
        {
            int iD = photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
    }

    [PunRPC]
    public void NetGrabbed(int id, bool leftHand)
    {
        titanWhoGrabMeID = id;
        Grabbed(PhotonView.Find(id).gameObject, leftHand);
    }

    [PunRPC]
    private void NetLaughAttack()
    {
        throw new NotImplementedException("Titan laugh attack is not implemented yet");
        //foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        //{
        //    if (((Vector3.Distance(obj2.transform.position, transform.position) < 50f) && (Vector3.Angle(obj2.transform.forward, transform.position - obj2.transform.position) < 90f)) && (obj2.GetComponent<TITAN>() != null))
        //    {
        //        obj2.GetComponent<TITAN>().beLaughAttacked();
        //    }
        //}
    }

    [PunRPC]
    private void NetPauseAnimation()
    {
        IEnumerator enumerator = animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                current.speed = 0f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }

    [PunRPC]
    public void NetPlayAnimation(string aniName)
    {
        currentAnimation = aniName;
        if (animation != null)
        {
            animation.Play(aniName);
        }
    }

    [PunRPC]
    private void NetPlayAnimationAt(string aniName, float normalizedTime)
    {
        currentAnimation = aniName;
        if (animation != null)
        {
            animation.Play(aniName);
            animation[aniName].normalizedTime = normalizedTime;
        }
    }

    [PunRPC]
    private void NetSetIsGrabbedFalse()
    {
        State = HERO_STATE.Idle;
    }

    [PunRPC]
    private void NetTauntAttack(float tauntTime, float distance = 100f)
    {
        throw new NotImplementedException("Titan taunt behavior is not yet implemented");
        //foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        //{
        //    if ((Vector3.Distance(obj2.transform.position, transform.position) < distance) && (obj2.GetComponent<TITAN>() != null))
        //    {
        //        obj2.GetComponent<TITAN>().beTauntedBy(gameObject, tauntTime);
        //    }
        //}
    }

    [PunRPC]
    public void NetUngrabbed()
    {
        Ungrabbed();
        NetPlayAnimation(standAnimation);
        FalseAttack();
    }

    [PunRPC]
    public void ReturnFromCannon(PhotonMessageInfo info)
    {
        if (info.sender == photonView.owner)
        {
            isCannon = false;
            smoothSyncMovement.disabled = false;
        }
    }

    [PunRPC]
    private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
    {
        hookBySomeOne = true;
        badGuy = PhotonView.Find(hooker).gameObject;
        if (Vector3.Distance(hookPosition, transform.position) < 15f)
        {
            launchForce = PhotonView.Find(hooker).gameObject.transform.position - transform.position;
            rigidBody.AddForce((-rigidBody.velocity * 0.9f), ForceMode.VelocityChange);
            float num = Mathf.Pow(launchForce.magnitude, 0.1f);
            if (grounded)
            {
                rigidBody.AddForce((Vector3.up * Mathf.Min((launchForce.magnitude * 0.2f), 10f)), ForceMode.Impulse);
            }
            rigidBody.AddForce(((launchForce * num) * 0.1f), ForceMode.Impulse);
            if (State != HERO_STATE.Grab)
            {
                dashTime = 1f;
                CrossFade("dash", 0.05f);
                animation["dash"].time = 0.1f;
                State = HERO_STATE.AirDodge;
                FalseAttack();
                facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * Mathf.Rad2Deg;
                var quaternion = Quaternion.Euler(0f, facingDirection, 0f);
                transform.rotation = quaternion;
                rigidBody.rotation = quaternion;
                targetRotation = quaternion;
            }
        }
        else
        {
            hookBySomeOne = false;
            badGuy = null;
            PhotonView.Find(hooker).RPC(nameof(HookFail), PhotonView.Find(hooker).owner, new object[0]);
        }
    }

    [PunRPC]
    public void SetMyCannon(int viewID, PhotonMessageInfo info)
    {
        if (info.sender == photonView.owner)
        {
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                myCannon = view.gameObject;
                if (myCannon != null)
                {
                    myCannonBase = myCannon.transform;
                    myCannonPlayer = myCannonBase.Find("PlayerPoint");
                    isCannon = true;
                }
            }
        }
    }

    [PunRPC]
    public void SetMyPhotonCamera(float offset, PhotonMessageInfo info)
    {
        if (photonView.owner == info.sender)
        {
            CameraMultiplier = offset;
            smoothSyncMovement.PhotonCamera = true;
            isPhotonCamera = true;
        }
    }

    [PunRPC]
    private void SetMyTeam(int val)
    {
        myTeam = val;
        checkLeftTrigger.myTeam = val;
        checkRightTrigger.myTeam = val;
        if (PhotonNetwork.isMasterClient)
        {
            object[] objArray;
            //TODO: Sync these upon gamemode syncSettings
            if (GameSettings.PvP.Mode == PvpMode.AhssVsBlades)
            {
                int num = 0;
                if (photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam] != null)
                {
                    num = RCextensions.returnIntFromObject(photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam]);
                }
                if (val != num)
                {
                    objArray = new object[] { num };
                    photonView.RPC(nameof(SetMyTeam), PhotonTargets.AllBuffered, objArray);
                }
            }
            else if (GameSettings.PvP.Mode == PvpMode.FreeForAll && (val != photonView.owner.ID))
            {
                objArray = new object[] { photonView.owner.ID };
                photonView.RPC(nameof(SetMyTeam), PhotonTargets.AllBuffered, objArray);
            }
        }
    }


    [PunRPC]
    public void SpawnCannonRPC(string settings, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient && photonView.isMine && !myCannon)
        {
            if (myHorse && isMounted)
                GetOffHorse();

            Idle();

            if (bulletLeft)
                bulletLeft.removeMe();

            if (bulletRight)
                bulletRight.removeMe();

            if ((smoke_3dmgEmission.enabled) && photonView.isMine)
            {
                object[] parameters = new object[] { false };
                photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, parameters);
            }
            smoke_3dmgEmission.enabled = false;
            rigidBody.velocity = Vector3.zero;
            string[] strArray = settings.Split(new char[] { ',' });
            if (strArray.Length > 15)
            {
                myCannon = PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])), 0);
            }
            else
            {
                myCannon = PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0);
            }
            myCannonBase = myCannon.transform;
            myCannonPlayer = myCannon.transform.Find("PlayerPoint");
            isCannon = true;
            myCannon.GetComponent<Cannon>().myHero = this;
            myCannonRegion = null;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(myCannon.transform.Find("Barrel").Find("FiringPoint").gameObject, true, false);
            Camera.main.fieldOfView = 55f;
            photonView.RPC(nameof(SetMyCannon), PhotonTargets.OthersBuffered, new object[] { myCannon.GetPhotonView().viewID });
            skillCDLastCannon = skillCDLast;
            skillCDLast = 3.5f;
            skillCDDuration = 3.5f;
        }
    }

    [PunRPC]
    private void WhoIsMyErenTitan(int id)
    {
        erenTitanGameObject = PhotonView.Find(id).gameObject;
        titanForm = true;
    }

    #endregion

}
