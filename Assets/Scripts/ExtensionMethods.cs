using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void PlayOneShotAndVisualize(this AudioSource a, AudioClip clip, GameObject go)
    {
        a.PlayOneShot(clip);
        go.GetComponent<Assets.Scripts.UI.InGame.SfxMarker>().TriggerSound();
    }
}
