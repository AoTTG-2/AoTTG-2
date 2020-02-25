using System;
using UnityEngine;
using Xft;

public class XWeaponTrailDemo : MonoBehaviour
{
    public XWeaponTrail ProTrailDistort;
    public XWeaponTrail ProTraillong;
    public XWeaponTrail ProTrailShort;
    public XWeaponTrail SimpleTrail;
    public Animation SwordAnimation;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0f, 0f, 150f, 30f), "Activate Trail1"))
        {
            this.ProTrailDistort.Deactivate();
            this.ProTrailShort.Deactivate();
            this.ProTraillong.Deactivate();
            this.SwordAnimation.Play();
            this.SimpleTrail.Activate();
        }
        if (GUI.Button(new Rect(0f, 30f, 150f, 30f), "Stop Trail1"))
        {
            this.SimpleTrail.Deactivate();
        }
        if (GUI.Button(new Rect(0f, 60f, 150f, 30f), "Stop Trail1 Smoothly"))
        {
            this.SimpleTrail.StopSmoothly(0.3f);
        }
        if (GUI.Button(new Rect(0f, 120f, 150f, 30f), "Activate Trail2"))
        {
            this.SimpleTrail.Deactivate();
            this.SwordAnimation.Play();
            this.ProTrailDistort.Activate();
            this.ProTrailShort.Activate();
            this.ProTraillong.Activate();
        }
        if (GUI.Button(new Rect(0f, 150f, 150f, 30f), "Stop Trail2"))
        {
            this.ProTrailDistort.Deactivate();
            this.ProTrailShort.Deactivate();
            this.ProTraillong.Deactivate();
        }
        if (GUI.Button(new Rect(0f, 180f, 150f, 30f), "Stop Trail2 Smoothly"))
        {
            this.ProTrailDistort.StopSmoothly(0.3f);
            this.ProTrailShort.StopSmoothly(0.3f);
            this.ProTraillong.StopSmoothly(0.3f);
        }
    }

    public void Start()
    {
        this.ProTrailDistort.Init();
        this.ProTrailShort.Init();
        this.ProTraillong.Init();
        this.SimpleTrail.Init();
    }
}

