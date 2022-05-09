using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AoTTG1 used the iTween package http://www.pixelplacement.com/itween/index.php. Still the Unity4 version.
/// </summary>
public class iTween : MonoBehaviour
{
    public string _name;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmap10;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmap11;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmap12;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmap13;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmap14;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmap8;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmap9;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmapA;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmapB;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmapC;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmapD;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmapE;
    [CompilerGenerated]
    private static Dictionary<string, int> f__switchSmapF;
    private ApplyTween apply;
    private AudioSource audioSource;
    private static GameObject cameraFade;
    private Color[,] colors;
    public float delay;
    public float delayStarted;
    private EasingFunction ease;
    public EaseType easeType;
    private float[] floats;
    public string id;
    private bool isLocal;
    public bool isPaused;
    public bool isRunning;
    public bool kinematic;
    private float lastRealTime;
    public bool loop;
    public LoopType loopType;
    public string method;
    private NamedValueColor namedcolorvalue;
    private CRSpline path;
    private float percentage;
    private bool physics;
    private Vector3 postUpdate;
    private Vector3 preUpdate;
    private Rect[] rects;
    private bool reverse;
    private float runningTime;
    private Space space;
    private Transform thisTransform;
    public float time;
    private Hashtable tweenArguments;
    public static List<Hashtable> tweens = new List<Hashtable>();
    public string type;
    private bool useRealTime;
    private Vector2[] vector2s;
    private Vector3[] vector3s;
    public bool wasPaused;

    private iTween(Hashtable h)
    {
        this.tweenArguments = h;
    }

    private void ApplyAudioToTargets()
    {
        this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
        this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
        this.audioSource.volume = this.vector2s[2].x;
        this.audioSource.pitch = this.vector2s[2].y;
        if (this.percentage == 1f)
        {
            this.audioSource.volume = this.vector2s[1].x;
            this.audioSource.pitch = this.vector2s[1].y;
        }
    }

    private void ApplyColorTargets()
    {
        this.colors[0, 2].r = this.ease(this.colors[0, 0].r, this.colors[0, 1].r, this.percentage);
        this.colors[0, 2].g = this.ease(this.colors[0, 0].g, this.colors[0, 1].g, this.percentage);
        this.colors[0, 2].b = this.ease(this.colors[0, 0].b, this.colors[0, 1].b, this.percentage);
        this.colors[0, 2].a = this.ease(this.colors[0, 0].a, this.colors[0, 1].a, this.percentage);
        this.tweenArguments["onupdateparams"] = this.colors[0, 2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.colors[0, 1];
        }
    }

    private void ApplyColorToTargets()
    {
        for (int i = 0; i < this.colors.GetLength(0); i++)
        {
            this.colors[i, 2].r = this.ease(this.colors[i, 0].r, this.colors[i, 1].r, this.percentage);
            this.colors[i, 2].g = this.ease(this.colors[i, 0].g, this.colors[i, 1].g, this.percentage);
            this.colors[i, 2].b = this.ease(this.colors[i, 0].b, this.colors[i, 1].b, this.percentage);
            this.colors[i, 2].a = this.ease(this.colors[i, 0].a, this.colors[i, 1].a, this.percentage);
        }
        if (base.GetComponent<Image>() != null)
        {
            base.GetComponent<Image>().color = this.colors[0, 2];
        }
        else if (base.GetComponent<Text>() != null)
        {
            base.GetComponent<Text>().material.color = this.colors[0, 2];
        }
        else if (base.GetComponent<Renderer>() != null)
        {
            for (int j = 0; j < this.colors.GetLength(0); j++)
            {
                base.GetComponent<Renderer>().materials[j].SetColor(this.namedcolorvalue.ToString(), this.colors[j, 2]);
            }
        }
        else if (base.GetComponent<Light>() != null)
        {
            base.GetComponent<Light>().color = this.colors[0, 2];
        }
        if (this.percentage == 1f)
        {
            if (base.GetComponent<Image>() != null)
            {
                base.GetComponent<Image>().color = this.colors[0, 1];
            }
            else if (base.GetComponent<Text>() != null)
            {
                base.GetComponent<Text>().material.color = this.colors[0, 1];
            }
            else if (base.GetComponent<Renderer>() != null)
            {
                for (int k = 0; k < this.colors.GetLength(0); k++)
                {
                    base.GetComponent<Renderer>().materials[k].SetColor(this.namedcolorvalue.ToString(), this.colors[k, 1]);
                }
            }
            else if (base.GetComponent<Light>() != null)
            {
                base.GetComponent<Light>().color = this.colors[0, 1];
            }
        }
    }

    private void ApplyFloatTargets()
    {
        this.floats[2] = this.ease(this.floats[0], this.floats[1], this.percentage);
        this.tweenArguments["onupdateparams"] = this.floats[2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.floats[1];
        }
    }

    private void ApplyLookToTargets()
    {
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        if (this.isLocal)
        {
            this.thisTransform.localRotation = Quaternion.Euler(this.vector3s[2]);
        }
        else
        {
            this.thisTransform.rotation = Quaternion.Euler(this.vector3s[2]);
        }
    }

    private void ApplyMoveByTargets()
    {
        this.preUpdate = this.thisTransform.position;
        Vector3 eulerAngles = new Vector3();
        if (this.tweenArguments.Contains("looktarget"))
        {
            eulerAngles = this.thisTransform.eulerAngles;
            this.thisTransform.eulerAngles = this.vector3s[4];
        }
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        this.thisTransform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
        this.vector3s[3] = this.vector3s[2];
        if (this.tweenArguments.Contains("looktarget"))
        {
            this.thisTransform.eulerAngles = eulerAngles;
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyMoveToPathTargets()
    {
        this.preUpdate = this.thisTransform.position;
        float num = this.ease(0f, 1f, this.percentage);
        if (this.isLocal)
        {
            this.thisTransform.localPosition = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
        }
        else
        {
            this.thisTransform.position = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
        }
        if (this.tweenArguments.Contains("orienttopath") && ((bool)this.tweenArguments["orienttopath"]))
        {
            float lookAhead;
            if (this.tweenArguments.Contains("lookahead"))
            {
                lookAhead = (float)this.tweenArguments["lookahead"];
            }
            else
            {
                lookAhead = Defaults.lookAhead;
            }
            float num3 = this.ease(0f, 1f, Mathf.Min((float)1f, (float)(this.percentage + lookAhead)));
            this.tweenArguments["looktarget"] = this.path.Interp(Mathf.Clamp(num3, 0f, 1f));
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyMoveToTargets()
    {
        this.preUpdate = this.thisTransform.position;
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        if (this.isLocal)
        {
            this.thisTransform.localPosition = this.vector3s[2];
        }
        else
        {
            this.thisTransform.position = this.vector3s[2];
        }
        if (this.percentage == 1f)
        {
            if (this.isLocal)
            {
                this.thisTransform.localPosition = this.vector3s[1];
            }
            else
            {
                this.thisTransform.position = this.vector3s[1];
            }
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyPunchPositionTargets()
    {
        this.preUpdate = this.thisTransform.position;
        Vector3 eulerAngles = new Vector3();
        if (this.tweenArguments.Contains("looktarget"))
        {
            eulerAngles = this.thisTransform.eulerAngles;
            this.thisTransform.eulerAngles = this.vector3s[4];
        }
        if (this.vector3s[1].x > 0f)
        {
            this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
        }
        else if (this.vector3s[1].x < 0f)
        {
            this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
        }
        if (this.vector3s[1].y > 0f)
        {
            this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
        }
        else if (this.vector3s[1].y < 0f)
        {
            this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
        }
        if (this.vector3s[1].z > 0f)
        {
            this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
        }
        else if (this.vector3s[1].z < 0f)
        {
            this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
        }
        this.thisTransform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
        this.vector3s[3] = this.vector3s[2];
        if (this.tweenArguments.Contains("looktarget"))
        {
            this.thisTransform.eulerAngles = eulerAngles;
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyPunchRotationTargets()
    {
        this.preUpdate = this.thisTransform.eulerAngles;
        if (this.vector3s[1].x > 0f)
        {
            this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
        }
        else if (this.vector3s[1].x < 0f)
        {
            this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
        }
        if (this.vector3s[1].y > 0f)
        {
            this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
        }
        else if (this.vector3s[1].y < 0f)
        {
            this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
        }
        if (this.vector3s[1].z > 0f)
        {
            this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
        }
        else if (this.vector3s[1].z < 0f)
        {
            this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
        }
        this.thisTransform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
        this.vector3s[3] = this.vector3s[2];
        this.postUpdate = this.thisTransform.eulerAngles;
        if (this.physics)
        {
            this.thisTransform.eulerAngles = this.preUpdate;
            base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
        }
    }

    private void ApplyPunchScaleTargets()
    {
        if (this.vector3s[1].x > 0f)
        {
            this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
        }
        else if (this.vector3s[1].x < 0f)
        {
            this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
        }
        if (this.vector3s[1].y > 0f)
        {
            this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
        }
        else if (this.vector3s[1].y < 0f)
        {
            this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
        }
        if (this.vector3s[1].z > 0f)
        {
            this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
        }
        else if (this.vector3s[1].z < 0f)
        {
            this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
        }
        this.thisTransform.localScale = this.vector3s[0] + this.vector3s[2];
    }

    private void ApplyRectTargets()
    {
        this.rects[2].x = this.ease(this.rects[0].x, this.rects[1].x, this.percentage);
        this.rects[2].y = this.ease(this.rects[0].y, this.rects[1].y, this.percentage);
        this.rects[2].width = this.ease(this.rects[0].width, this.rects[1].width, this.percentage);
        this.rects[2].height = this.ease(this.rects[0].height, this.rects[1].height, this.percentage);
        this.tweenArguments["onupdateparams"] = this.rects[2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.rects[1];
        }
    }

    private void ApplyRotateAddTargets()
    {
        this.preUpdate = this.thisTransform.eulerAngles;
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        this.thisTransform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
        this.vector3s[3] = this.vector3s[2];
        this.postUpdate = this.thisTransform.eulerAngles;
        if (this.physics)
        {
            this.thisTransform.eulerAngles = this.preUpdate;
            base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
        }
    }

    private void ApplyRotateToTargets()
    {
        this.preUpdate = this.thisTransform.eulerAngles;
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        if (this.isLocal)
        {
            this.thisTransform.localRotation = Quaternion.Euler(this.vector3s[2]);
        }
        else
        {
            this.thisTransform.rotation = Quaternion.Euler(this.vector3s[2]);
        }
        if (this.percentage == 1f)
        {
            if (this.isLocal)
            {
                this.thisTransform.localRotation = Quaternion.Euler(this.vector3s[1]);
            }
            else
            {
                this.thisTransform.rotation = Quaternion.Euler(this.vector3s[1]);
            }
        }
        this.postUpdate = this.thisTransform.eulerAngles;
        if (this.physics)
        {
            this.thisTransform.eulerAngles = this.preUpdate;
            base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
        }
    }

    private void ApplyScaleToTargets()
    {
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        this.thisTransform.localScale = this.vector3s[2];
        if (this.percentage == 1f)
        {
            this.thisTransform.localScale = this.vector3s[1];
        }
    }

    private void ApplyShakePositionTargets()
    {
        if (this.isLocal)
        {
            this.preUpdate = this.thisTransform.localPosition;
        }
        else
        {
            this.preUpdate = this.thisTransform.position;
        }
        Vector3 eulerAngles = new Vector3();
        if (this.tweenArguments.Contains("looktarget"))
        {
            eulerAngles = this.thisTransform.eulerAngles;
            this.thisTransform.eulerAngles = this.vector3s[3];
        }
        if (this.percentage == 0f)
        {
            this.thisTransform.Translate(this.vector3s[1], this.space);
        }
        if (this.isLocal)
        {
            this.thisTransform.localPosition = this.vector3s[0];
        }
        else
        {
            this.thisTransform.position = this.vector3s[0];
        }
        float num = 1f - this.percentage;
        this.vector3s[2].x = UnityEngine.Random.Range((float)(-this.vector3s[1].x * num), (float)(this.vector3s[1].x * num));
        this.vector3s[2].y = UnityEngine.Random.Range((float)(-this.vector3s[1].y * num), (float)(this.vector3s[1].y * num));
        this.vector3s[2].z = UnityEngine.Random.Range((float)(-this.vector3s[1].z * num), (float)(this.vector3s[1].z * num));
        if (this.isLocal)
        {
            this.thisTransform.localPosition += this.vector3s[2];
        }
        else
        {
            this.thisTransform.position += this.vector3s[2];
        }
        if (this.tweenArguments.Contains("looktarget"))
        {
            this.thisTransform.eulerAngles = eulerAngles;
        }
        this.postUpdate = this.thisTransform.position;
        if (this.physics)
        {
            this.thisTransform.position = this.preUpdate;
            base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
        }
    }

    private void ApplyShakeRotationTargets()
    {
        this.preUpdate = this.thisTransform.eulerAngles;
        if (this.percentage == 0f)
        {
            this.thisTransform.Rotate(this.vector3s[1], this.space);
        }
        this.thisTransform.eulerAngles = this.vector3s[0];
        float num = 1f - this.percentage;
        this.vector3s[2].x = UnityEngine.Random.Range((float)(-this.vector3s[1].x * num), (float)(this.vector3s[1].x * num));
        this.vector3s[2].y = UnityEngine.Random.Range((float)(-this.vector3s[1].y * num), (float)(this.vector3s[1].y * num));
        this.vector3s[2].z = UnityEngine.Random.Range((float)(-this.vector3s[1].z * num), (float)(this.vector3s[1].z * num));
        this.thisTransform.Rotate(this.vector3s[2], this.space);
        this.postUpdate = this.thisTransform.eulerAngles;
        if (this.physics)
        {
            this.thisTransform.eulerAngles = this.preUpdate;
            base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
        }
    }

    private void ApplyShakeScaleTargets()
    {
        if (this.percentage == 0f)
        {
            this.thisTransform.localScale = this.vector3s[1];
        }
        this.thisTransform.localScale = this.vector3s[0];
        float num = 1f - this.percentage;
        this.vector3s[2].x = UnityEngine.Random.Range((float)(-this.vector3s[1].x * num), (float)(this.vector3s[1].x * num));
        this.vector3s[2].y = UnityEngine.Random.Range((float)(-this.vector3s[1].y * num), (float)(this.vector3s[1].y * num));
        this.vector3s[2].z = UnityEngine.Random.Range((float)(-this.vector3s[1].z * num), (float)(this.vector3s[1].z * num));
        this.thisTransform.localScale += this.vector3s[2];
    }

    private void ApplyStabTargets()
    {
    }

    private void ApplyVector2Targets()
    {
        this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
        this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
        this.tweenArguments["onupdateparams"] = this.vector2s[2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.vector2s[1];
        }
    }

    private void ApplyVector3Targets()
    {
        this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
        this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
        this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
        this.tweenArguments["onupdateparams"] = this.vector3s[2];
        if (this.percentage == 1f)
        {
            this.tweenArguments["onupdateparams"] = this.vector3s[1];
        }
    }

    public static void AudioFrom(GameObject target, Hashtable args)
    {
        AudioSource audio;
        Vector2 vector;
        Vector2 vector2;
        args = CleanArgs(args);
        if (args.Contains("audiosource"))
        {
            audio = (AudioSource)args["audiosource"];
        }
        else if (target.GetComponent<AudioSource>() != null)
        {
            audio = target.GetComponent<AudioSource>();
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: AudioFrom requires an AudioSource.");
            return;
        }
        vector.x = vector2.x = audio.volume;
        vector.y = vector2.y = audio.pitch;
        if (args.Contains("volume"))
        {
            vector2.x = (float)args["volume"];
        }
        if (args.Contains("pitch"))
        {
            vector2.y = (float)args["pitch"];
        }
        audio.volume = vector2.x;
        audio.pitch = vector2.y;
        args["volume"] = vector.x;
        args["pitch"] = vector.y;
        if (!args.Contains("easetype"))
        {
            args.Add("easetype", EaseType.linear);
        }
        args["type"] = "audio";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void AudioFrom(GameObject target, float volume, float pitch, float time)
    {
        object[] args = new object[] { "volume", volume, "pitch", pitch, "time", time };
        AudioFrom(target, Hash(args));
    }

    public static void AudioTo(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        if (!args.Contains("easetype"))
        {
            args.Add("easetype", EaseType.linear);
        }
        args["type"] = "audio";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void AudioTo(GameObject target, float volume, float pitch, float time)
    {
        object[] args = new object[] { "volume", volume, "pitch", pitch, "time", time };
        AudioTo(target, Hash(args));
    }

    public static void AudioUpdate(GameObject target, Hashtable args)
    {
        float updateTime;
        AudioSource audio;
        CleanArgs(args);
        Vector2[] vectorArray = new Vector2[4];
        if (args.Contains("time"))
        {
            updateTime = (float)args["time"];
            updateTime *= Defaults.updateTimePercentage;
        }
        else
        {
            updateTime = Defaults.updateTime;
        }
        if (args.Contains("audiosource"))
        {
            audio = (AudioSource)args["audiosource"];
        }
        else if (target.GetComponent<AudioSource>() != null)
        {
            audio = target.GetComponent<AudioSource>();
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.");
            return;
        }
        vectorArray[0] = vectorArray[1] = new Vector2(audio.volume, audio.pitch);
        if (args.Contains("volume"))
        {
            vectorArray[1].x = (float)args["volume"];
        }
        if (args.Contains("pitch"))
        {
            vectorArray[1].y = (float)args["pitch"];
        }
        vectorArray[3].x = Mathf.SmoothDampAngle(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
        vectorArray[3].y = Mathf.SmoothDampAngle(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
        audio.volume = vectorArray[3].x;
        audio.pitch = vectorArray[3].y;
    }

    public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
    {
        object[] args = new object[] { "volume", volume, "pitch", pitch, "time", time };
        AudioUpdate(target, Hash(args));
    }

    private void Awake()
    {
        this.thisTransform = base.transform;
        this.RetrieveArgs();
        this.lastRealTime = Time.realtimeSinceStartup;
    }

    private void CallBack(string callbackType)
    {
        if (this.tweenArguments.Contains(callbackType) && !this.tweenArguments.Contains("ischild"))
        {
            GameObject gameObject;
            if (this.tweenArguments.Contains(callbackType + "target"))
            {
                gameObject = (GameObject)this.tweenArguments[callbackType + "target"];
            }
            else
            {
                gameObject = base.gameObject;
            }
            if (this.tweenArguments[callbackType].GetType() == typeof(string))
            {
                gameObject.SendMessage((string)this.tweenArguments[callbackType], this.tweenArguments[callbackType + "params"], SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                UnityEngine.Debug.LogError("iTween Error: Callback method references must be passed as a String!");
                UnityEngine.Object.Destroy(this);
            }
        }
    }

    public static GameObject CameraFadeAdd()
    {
        if (cameraFade != null)
        {
            return null;
        }
        cameraFade = new GameObject("iTween Camera Fade");
        cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)Defaults.cameraFadeDepth);
        cameraFade.AddComponent<Image>();
        //cameraFade.GetComponent<Image>().texture = CameraTexture(Color.black);
        cameraFade.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
        return cameraFade;
    }

    public static GameObject CameraFadeAdd(Texture2D texture)
    {
        if (cameraFade != null)
        {
            return null;
        }
        cameraFade = new GameObject("iTween Camera Fade");
        cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)Defaults.cameraFadeDepth);
        cameraFade.AddComponent<Image>();
        //cameraFade.GetComponent<Image>().texture = texture;
        cameraFade.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
        return cameraFade;
    }

    public static GameObject CameraFadeAdd(Texture2D texture, int depth)
    {
        if (cameraFade != null)
        {
            return null;
        }
        cameraFade = new GameObject("iTween Camera Fade");
        cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)depth);
        cameraFade.AddComponent<Image>();
        //cameraFade.GetComponent<Image>().texture = texture;
        cameraFade.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
        return cameraFade;
    }

    public static void CameraFadeDepth(int depth)
    {
        if (cameraFade != null)
        {
            cameraFade.transform.position = new Vector3(cameraFade.transform.position.x, cameraFade.transform.position.y, (float)depth);
        }
    }

    public static void CameraFadeDestroy()
    {
        if (cameraFade != null)
        {
            UnityEngine.Object.Destroy(cameraFade);
        }
    }

    public static void CameraFadeFrom(Hashtable args)
    {
        if (cameraFade != null)
        {
            ColorFrom(cameraFade, args);
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
        }
    }

    public static void CameraFadeFrom(float amount, float time)
    {
        if (cameraFade != null)
        {
            object[] args = new object[] { "amount", amount, "time", time };
            CameraFadeFrom(Hash(args));
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
        }
    }

    public static void CameraFadeSwap(Texture2D texture)
    {
        if (cameraFade != null)
        {
            //cameraFade.GetComponent<Image>().texture = texture;
        }
    }

    public static void CameraFadeTo(Hashtable args)
    {
        if (cameraFade != null)
        {
            ColorTo(cameraFade, args);
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
        }
    }

    public static void CameraFadeTo(float amount, float time)
    {
        if (cameraFade != null)
        {
            object[] args = new object[] { "amount", amount, "time", time };
            CameraFadeTo(Hash(args));
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
        }
    }

    public static Texture2D CameraTexture(Color color)
    {
        Texture2D textured = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        Color[] colors = new Color[Screen.width * Screen.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        textured.SetPixels(colors);
        textured.Apply();
        return textured;
    }

    private static Hashtable CleanArgs(Hashtable args)
    {
        Hashtable hashtable = new Hashtable(args.Count);
        Hashtable hashtable2 = new Hashtable(args.Count);
        IDictionaryEnumerator enumerator = args.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    DictionaryEntry current = (DictionaryEntry)enumerator.Current;
                    hashtable.Add(current.Key, current.Value);
                }
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
        IDictionaryEnumerator enumerator2 = hashtable.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                if (enumerator2.Current != null)
                {
                    DictionaryEntry entry2 = (DictionaryEntry)enumerator2.Current;
                    if (entry2.Value.GetType() == typeof(int))
                    {
                        int num = (int)entry2.Value;
                        float num2 = num;
                        args[entry2.Key] = num2;
                    }
                    if (entry2.Value.GetType() == typeof(double))
                    {
                        double num3 = (double)entry2.Value;
                        float num4 = (float)num3;
                        args[entry2.Key] = num4;
                    }
                }
            }
        }
        finally
        {
            IDisposable disposable2 = enumerator2 as IDisposable;
            if (disposable2 != null)
            {
                disposable2.Dispose();
            }
        }
        IDictionaryEnumerator enumerator3 = args.GetEnumerator();
        try
        {
            while (enumerator3.MoveNext())
            {
                if (enumerator3.Current != null)
                {
                    DictionaryEntry entry3 = (DictionaryEntry)enumerator3.Current;
                    string key = entry3.Key.ToString().ToLower();
                    hashtable2.Add(key, entry3.Value);
                }
            }
        }
        finally
        {
            IDisposable disposable3 = enumerator3 as IDisposable;
            if (disposable3 != null)
            {
                disposable3.Dispose();
            }
        }
        args = hashtable2;
        return args;
    }

    private float clerp(float start, float end, float value)
    {
        float num = 0f;
        float num2 = 360f;
        float num3 = Mathf.Abs((float)((num2 - num) * 0.5f));
        float num5 = 0f;
        if ((end - start) < -num3)
        {
            num5 = ((num2 - start) + end) * value;
            return (start + num5);
        }
        if ((end - start) > num3)
        {
            num5 = -((num2 - end) + start) * value;
            return (start + num5);
        }
        return (start + ((end - start) * value));
    }

    public static void ColorFrom(GameObject target, Hashtable args)
    {
        Color color = new Color();
        Color color2 = new Color();
        args = CleanArgs(args);
        if (!args.Contains("includechildren") || ((bool)args["includechildren"]))
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    Hashtable hashtable = (Hashtable)args.Clone();
                    hashtable["ischild"] = true;
                    if (current != null)
                        ColorFrom(current.gameObject, hashtable);
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
        if (!args.Contains("easetype"))
        {
            args.Add("easetype", EaseType.linear);
        }
        if (target.GetComponent<Image>() != null)
        {
            color2 = color = target.GetComponent<Image>().color;
        }
        else if (target.GetComponent<Text>() != null)
        {
            color2 = color = target.GetComponent<Text>().material.color;
        }
        else if (target.GetComponent<Renderer>() != null)
        {
            color2 = color = target.GetComponent<Renderer>().material.color;
        }
        else if (target.GetComponent<Light>() != null)
        {
            color2 = color = target.GetComponent<Light>().color;
        }
        if (args.Contains("color"))
        {
            color = (Color)args["color"];
        }
        else
        {
            if (args.Contains("r"))
            {
                color.r = (float)args["r"];
            }
            if (args.Contains("g"))
            {
                color.g = (float)args["g"];
            }
            if (args.Contains("b"))
            {
                color.b = (float)args["b"];
            }
            if (args.Contains("a"))
            {
                color.a = (float)args["a"];
            }
        }
        if (args.Contains("amount"))
        {
            color.a = (float)args["amount"];
            args.Remove("amount");
        }
        else if (args.Contains("alpha"))
        {
            color.a = (float)args["alpha"];
            args.Remove("alpha");
        }
        if (target.GetComponent<Image>() != null)
        {
            target.GetComponent<Image>().color = color;
        }
        else if (target.GetComponent<Text>() != null)
        {
            target.GetComponent<Text>().material.color = color;
        }
        else if (target.GetComponent<Renderer>() != null)
        {
            target.GetComponent<Renderer>().material.color = color;
        }
        else if (target.GetComponent<Light>() != null)
        {
            target.GetComponent<Light>().color = color;
        }
        args["color"] = color2;
        args["type"] = "color";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void ColorFrom(GameObject target, Color color, float time)
    {
        object[] args = new object[] { "color", color, "time", time };
        ColorFrom(target, Hash(args));
    }

    public static void ColorTo(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        if (!args.Contains("includechildren") || ((bool)args["includechildren"]))
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    Hashtable hashtable = (Hashtable)args.Clone();
                    hashtable["ischild"] = true;
                    if (current != null)
                        ColorTo(current.gameObject, hashtable);
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
        if (!args.Contains("easetype"))
        {
            args.Add("easetype", EaseType.linear);
        }
        args["type"] = "color";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void ColorTo(GameObject target, Color color, float time)
    {
        object[] args = new object[] { "color", color, "time", time };
        ColorTo(target, Hash(args));
    }

    public static void ColorUpdate(GameObject target, Hashtable args)
    {
        float updateTime;
        CleanArgs(args);
        Color[] colorArray = new Color[4];
        if (!args.Contains("includechildren") || ((bool)args["includechildren"]))
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    if (current != null)
                        ColorUpdate(current.gameObject, args);
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
        if (args.Contains("time"))
        {
            updateTime = (float)args["time"];
            updateTime *= Defaults.updateTimePercentage;
        }
        else
        {
            updateTime = Defaults.updateTime;
        }
        if (target.GetComponent<Image>() != null)
        {
            colorArray[0] = colorArray[1] = target.GetComponent<Image>().color;
        }
        else if (target.GetComponent<Text>() != null)
        {
            colorArray[0] = colorArray[1] = target.GetComponent<Text>().material.color;
        }
        else if (target.GetComponent<Renderer>() != null)
        {
            colorArray[0] = colorArray[1] = target.GetComponent<Renderer>().material.color;
        }
        else if (target.GetComponent<Light>() != null)
        {
            colorArray[0] = colorArray[1] = target.GetComponent<Light>().color;
        }
        if (args.Contains("color"))
        {
            colorArray[1] = (Color)args["color"];
        }
        else
        {
            if (args.Contains("r"))
            {
                colorArray[1].r = (float)args["r"];
            }
            if (args.Contains("g"))
            {
                colorArray[1].g = (float)args["g"];
            }
            if (args.Contains("b"))
            {
                colorArray[1].b = (float)args["b"];
            }
            if (args.Contains("a"))
            {
                colorArray[1].a = (float)args["a"];
            }
        }
        colorArray[3].r = Mathf.SmoothDamp(colorArray[0].r, colorArray[1].r, ref colorArray[2].r, updateTime);
        colorArray[3].g = Mathf.SmoothDamp(colorArray[0].g, colorArray[1].g, ref colorArray[2].g, updateTime);
        colorArray[3].b = Mathf.SmoothDamp(colorArray[0].b, colorArray[1].b, ref colorArray[2].b, updateTime);
        colorArray[3].a = Mathf.SmoothDamp(colorArray[0].a, colorArray[1].a, ref colorArray[2].a, updateTime);
        if (target.GetComponent<Image>() != null)
        {
            target.GetComponent<Image>().color = colorArray[3];
        }
        else if (target.GetComponent<Text>() != null)
        {
            target.GetComponent<Text>().material.color = colorArray[3];
        }
        else if (target.GetComponent<Renderer>() != null)
        {
            target.GetComponent<Renderer>().material.color = colorArray[3];
        }
        else if (target.GetComponent<Light>() != null)
        {
            target.GetComponent<Light>().color = colorArray[3];
        }
    }

    public static void ColorUpdate(GameObject target, Color color, float time)
    {
        object[] args = new object[] { "color", color, "time", time };
        ColorUpdate(target, Hash(args));
    }

    private void ConflictCheck()
    {
        foreach (iTween tween in base.GetComponents<iTween>())
        {
            if (tween.type == "value")
            {
                return;
            }
            if (tween.isRunning && (tween.type == this.type))
            {
                if (tween.method != this.method)
                {
                    return;
                }
                if (tween.tweenArguments.Count != this.tweenArguments.Count)
                {
                    tween.Dispose();
                    return;
                }
                IDictionaryEnumerator enumerator = this.tweenArguments.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DictionaryEntry current = (DictionaryEntry)enumerator.Current;
                        if (!tween.tweenArguments.Contains(current.Key))
                        {
                            tween.Dispose();
                            break;
                        }
                        if (!tween.tweenArguments[current.Key].Equals(this.tweenArguments[current.Key]) && (((string)current.Key) != "id"))
                        {
                            tween.Dispose();
                            break;
                        }
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
                this.Dispose();
            }
        }
    }

    public static int Count()
    {
        return tweens.Count;
    }

    public static int Count(string type)
    {
        int num = 0;
        for (int i = 0; i < tweens.Count; i++)
        {
            Hashtable hashtable = tweens[i];
            if ((((string)hashtable["type"]) + ((string)hashtable["method"])).Substring(0, type.Length).ToLower() == type.ToLower())
            {
                num++;
            }
        }
        return num;
    }

    public static int Count(GameObject target)
    {
        return target.GetComponents<iTween>().Length;
    }

    public static int Count(GameObject target, string type)
    {
        int num = 0;
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
            {
                num++;
            }
        }
        return num;
    }

    private void DisableKinematic()
    {
    }

    private void Dispose()
    {
        for (int i = 0; i < tweens.Count; i++)
        {
            Hashtable hashtable = tweens[i];
            if (((string)hashtable["id"]) == this.id)
            {
                tweens.RemoveAt(i);
                break;
            }
        }
        UnityEngine.Object.Destroy(this);
    }

    public static void DrawLine(Transform[] line)
    {
        if (line.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                vectorArray[i] = line[i].position;
            }
            DrawLineHelper(vectorArray, Defaults.color, "gizmos");
        }
    }

    public static void DrawLine(Vector3[] line)
    {
        if (line.Length > 0)
        {
            DrawLineHelper(line, Defaults.color, "gizmos");
        }
    }

    public static void DrawLine(Transform[] line, Color color)
    {
        if (line.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                vectorArray[i] = line[i].position;
            }
            DrawLineHelper(vectorArray, color, "gizmos");
        }
    }

    public static void DrawLine(Vector3[] line, Color color)
    {
        if (line.Length > 0)
        {
            DrawLineHelper(line, color, "gizmos");
        }
    }

    public static void DrawLineGizmos(Transform[] line)
    {
        if (line.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                vectorArray[i] = line[i].position;
            }
            DrawLineHelper(vectorArray, Defaults.color, "gizmos");
        }
    }

    public static void DrawLineGizmos(Vector3[] line)
    {
        if (line.Length > 0)
        {
            DrawLineHelper(line, Defaults.color, "gizmos");
        }
    }

    public static void DrawLineGizmos(Transform[] line, Color color)
    {
        if (line.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                vectorArray[i] = line[i].position;
            }
            DrawLineHelper(vectorArray, color, "gizmos");
        }
    }

    public static void DrawLineGizmos(Vector3[] line, Color color)
    {
        if (line.Length > 0)
        {
            DrawLineHelper(line, color, "gizmos");
        }
    }

    public static void DrawLineHandles(Transform[] line)
    {
        if (line.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                vectorArray[i] = line[i].position;
            }
            DrawLineHelper(vectorArray, Defaults.color, "handles");
        }
    }

    public static void DrawLineHandles(Vector3[] line)
    {
        if (line.Length > 0)
        {
            DrawLineHelper(line, Defaults.color, "handles");
        }
    }

    public static void DrawLineHandles(Transform[] line, Color color)
    {
        if (line.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                vectorArray[i] = line[i].position;
            }
            DrawLineHelper(vectorArray, color, "handles");
        }
    }

    public static void DrawLineHandles(Vector3[] line, Color color)
    {
        if (line.Length > 0)
        {
            DrawLineHelper(line, color, "handles");
        }
    }

    private static void DrawLineHelper(Vector3[] line, Color color, string method)
    {
        Gizmos.color = color;
        for (int i = 0; i < (line.Length - 1); i++)
        {
            if (method == "gizmos")
            {
                Gizmos.DrawLine(line[i], line[i + 1]);
            }
            else if (method == "handles")
            {
                UnityEngine.Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
            }
        }
    }

    public static void DrawPath(Transform[] path)
    {
        if (path.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                vectorArray[i] = path[i].position;
            }
            DrawPathHelper(vectorArray, Defaults.color, "gizmos");
        }
    }

    public static void DrawPath(Vector3[] path)
    {
        if (path.Length > 0)
        {
            DrawPathHelper(path, Defaults.color, "gizmos");
        }
    }

    public static void DrawPath(Transform[] path, Color color)
    {
        if (path.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                vectorArray[i] = path[i].position;
            }
            DrawPathHelper(vectorArray, color, "gizmos");
        }
    }

    public static void DrawPath(Vector3[] path, Color color)
    {
        if (path.Length > 0)
        {
            DrawPathHelper(path, color, "gizmos");
        }
    }

    public static void DrawPathGizmos(Transform[] path)
    {
        if (path.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                vectorArray[i] = path[i].position;
            }
            DrawPathHelper(vectorArray, Defaults.color, "gizmos");
        }
    }

    public static void DrawPathGizmos(Vector3[] path)
    {
        if (path.Length > 0)
        {
            DrawPathHelper(path, Defaults.color, "gizmos");
        }
    }

    public static void DrawPathGizmos(Transform[] path, Color color)
    {
        if (path.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                vectorArray[i] = path[i].position;
            }
            DrawPathHelper(vectorArray, color, "gizmos");
        }
    }

    public static void DrawPathGizmos(Vector3[] path, Color color)
    {
        if (path.Length > 0)
        {
            DrawPathHelper(path, color, "gizmos");
        }
    }

    public static void DrawPathHandles(Transform[] path)
    {
        if (path.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                vectorArray[i] = path[i].position;
            }
            DrawPathHelper(vectorArray, Defaults.color, "handles");
        }
    }

    public static void DrawPathHandles(Vector3[] path)
    {
        if (path.Length > 0)
        {
            DrawPathHelper(path, Defaults.color, "handles");
        }
    }

    public static void DrawPathHandles(Transform[] path, Color color)
    {
        if (path.Length > 0)
        {
            Vector3[] vectorArray = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                vectorArray[i] = path[i].position;
            }
            DrawPathHelper(vectorArray, color, "handles");
        }
    }

    public static void DrawPathHandles(Vector3[] path, Color color)
    {
        if (path.Length > 0)
        {
            DrawPathHelper(path, color, "handles");
        }
    }

    private static void DrawPathHelper(Vector3[] path, Color color, string method)
    {
        Vector3[] pts = PathControlPointGenerator(path);
        Vector3 to = Interp(pts, 0f);
        Gizmos.color = color;
        int num = path.Length * 20;
        for (int i = 1; i <= num; i++)
        {
            float t = ((float)i) / ((float)num);
            Vector3 from = Interp(pts, t);
            if (method == "gizmos")
            {
                Gizmos.DrawLine(from, to);
            }
            else if (method == "handles")
            {
                UnityEngine.Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
            }
            to = from;
        }
    }

    private float easeInBack(float start, float end, float value)
    {
        end -= start;
        value /= 1f;
        float num = 1.70158f;
        return ((((end * value) * value) * (((num + 1f) * value) - num)) + start);
    }

    private float easeInBounce(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        return ((end - this.easeOutBounce(0f, end, num - value)) + start);
    }

    private float easeInCirc(float start, float end, float value)
    {
        end -= start;
        return ((-end * (Mathf.Sqrt(1f - (value * value)) - 1f)) + start);
    }

    private float easeInCubic(float start, float end, float value)
    {
        end -= start;
        return ((((end * value) * value) * value) + start);
    }

    private float easeInElastic(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        float num2 = num * 0.3f;
        float num3 = 0f;
        float num4 = 0f;
        if (value == 0f)
        {
            return start;
        }
        if ((value /= num) == 1f)
        {
            return (start + end);
        }
        if ((num4 == 0f) || (num4 < Mathf.Abs(end)))
        {
            num4 = end;
            num3 = num2 / 4f;
        }
        else
        {
            num3 = (num2 / 6.283185f) * Mathf.Asin(end / num4);
        }
        return (-((num4 * Mathf.Pow(2f, 10f * --value)) * Mathf.Sin((((value * num) - num3) * 6.283185f) / num2)) + start);
    }

    private float easeInExpo(float start, float end, float value)
    {
        end -= start;
        return ((end * Mathf.Pow(2f, 10f * (value - 1f))) + start);
    }

    private float easeInOutBack(float start, float end, float value)
    {
        float num = 1.70158f;
        end -= start;
        value /= 0.5f;
        if (value < 1f)
        {
            num *= 1.525f;
            return (((end * 0.5f) * ((value * value) * (((num + 1f) * value) - num))) + start);
        }
        value -= 2f;
        num *= 1.525f;
        return (((end * 0.5f) * (((value * value) * (((num + 1f) * value) + num)) + 2f)) + start);
    }

    private float easeInOutBounce(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        if (value < (num * 0.5f))
        {
            return ((this.easeInBounce(0f, end, value * 2f) * 0.5f) + start);
        }
        return (((this.easeOutBounce(0f, end, (value * 2f) - num) * 0.5f) + (end * 0.5f)) + start);
    }

    private float easeInOutCirc(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return (((-end * 0.5f) * (Mathf.Sqrt(1f - (value * value)) - 1f)) + start);
        }
        value -= 2f;
        return (((end * 0.5f) * (Mathf.Sqrt(1f - (value * value)) + 1f)) + start);
    }

    private float easeInOutCubic(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return (((((end * 0.5f) * value) * value) * value) + start);
        }
        value -= 2f;
        return (((end * 0.5f) * (((value * value) * value) + 2f)) + start);
    }

    private float easeInOutElastic(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        float num2 = num * 0.3f;
        float num3 = 0f;
        float num4 = 0f;
        if (value == 0f)
        {
            return start;
        }
        if ((value /= (num * 0.5f)) == 2f)
        {
            return (start + end);
        }
        if ((num4 == 0f) || (num4 < Mathf.Abs(end)))
        {
            num4 = end;
            num3 = num2 / 4f;
        }
        else
        {
            num3 = (num2 / 6.283185f) * Mathf.Asin(end / num4);
        }
        if (value < 1f)
        {
            return ((-0.5f * ((num4 * Mathf.Pow(2f, 10f * --value)) * Mathf.Sin((((value * num) - num3) * 6.283185f) / num2))) + start);
        }
        return (((((num4 * Mathf.Pow(2f, -10f * --value)) * Mathf.Sin((((value * num) - num3) * 6.283185f) / num2)) * 0.5f) + end) + start);
    }

    private float easeInOutExpo(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return (((end * 0.5f) * Mathf.Pow(2f, 10f * (value - 1f))) + start);
        }
        value--;
        return (((end * 0.5f) * (-Mathf.Pow(2f, -10f * value) + 2f)) + start);
    }

    private float easeInOutQuad(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return ((((end * 0.5f) * value) * value) + start);
        }
        value--;
        return (((-end * 0.5f) * ((value * (value - 2f)) - 1f)) + start);
    }

    private float easeInOutQuart(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return ((((((end * 0.5f) * value) * value) * value) * value) + start);
        }
        value -= 2f;
        return (((-end * 0.5f) * ((((value * value) * value) * value) - 2f)) + start);
    }

    private float easeInOutQuint(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return (((((((end * 0.5f) * value) * value) * value) * value) * value) + start);
        }
        value -= 2f;
        return (((end * 0.5f) * (((((value * value) * value) * value) * value) + 2f)) + start);
    }

    private float easeInOutSine(float start, float end, float value)
    {
        end -= start;
        return (((-end * 0.5f) * (Mathf.Cos(3.141593f * value) - 1f)) + start);
    }

    private float easeInQuad(float start, float end, float value)
    {
        end -= start;
        return (((end * value) * value) + start);
    }

    private float easeInQuart(float start, float end, float value)
    {
        end -= start;
        return (((((end * value) * value) * value) * value) + start);
    }

    private float easeInQuint(float start, float end, float value)
    {
        end -= start;
        return ((((((end * value) * value) * value) * value) * value) + start);
    }

    private float easeInSine(float start, float end, float value)
    {
        end -= start;
        return (((-end * Mathf.Cos(value * 1.570796f)) + end) + start);
    }

    private float easeOutBack(float start, float end, float value)
    {
        float num = 1.70158f;
        end -= start;
        value--;
        return ((end * (((value * value) * (((num + 1f) * value) + num)) + 1f)) + start);
    }

    private float easeOutBounce(float start, float end, float value)
    {
        value /= 1f;
        end -= start;
        if (value < 0.3636364f)
        {
            return ((end * ((7.5625f * value) * value)) + start);
        }
        if (value < 0.7272727f)
        {
            value -= 0.5454546f;
            return ((end * (((7.5625f * value) * value) + 0.75f)) + start);
        }
        if (value < 0.90909090909090906)
        {
            value -= 0.8181818f;
            return ((end * (((7.5625f * value) * value) + 0.9375f)) + start);
        }
        value -= 0.9545454f;
        return ((end * (((7.5625f * value) * value) + 0.984375f)) + start);
    }

    private float easeOutCirc(float start, float end, float value)
    {
        value--;
        end -= start;
        return ((end * Mathf.Sqrt(1f - (value * value))) + start);
    }

    private float easeOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return ((end * (((value * value) * value) + 1f)) + start);
    }

    private float easeOutElastic(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        float num2 = num * 0.3f;
        float num3 = 0f;
        float num4 = 0f;
        if (value == 0f)
        {
            return start;
        }
        if ((value /= num) == 1f)
        {
            return (start + end);
        }
        if ((num4 == 0f) || (num4 < Mathf.Abs(end)))
        {
            num4 = end;
            num3 = num2 * 0.25f;
        }
        else
        {
            num3 = (num2 / 6.283185f) * Mathf.Asin(end / num4);
        }
        return ((((num4 * Mathf.Pow(2f, -10f * value)) * Mathf.Sin((((value * num) - num3) * 6.283185f) / num2)) + end) + start);
    }

    private float easeOutExpo(float start, float end, float value)
    {
        end -= start;
        return ((end * (-Mathf.Pow(2f, -10f * value) + 1f)) + start);
    }

    private float easeOutQuad(float start, float end, float value)
    {
        end -= start;
        return (((-end * value) * (value - 2f)) + start);
    }

    private float easeOutQuart(float start, float end, float value)
    {
        value--;
        end -= start;
        return ((-end * ((((value * value) * value) * value) - 1f)) + start);
    }

    private float easeOutQuint(float start, float end, float value)
    {
        value--;
        end -= start;
        return ((end * (((((value * value) * value) * value) * value) + 1f)) + start);
    }

    private float easeOutSine(float start, float end, float value)
    {
        end -= start;
        return ((end * Mathf.Sin(value * 1.570796f)) + start);
    }

    private void EnableKinematic()
    {
    }

    public static void FadeFrom(GameObject target, Hashtable args)
    {
        ColorFrom(target, args);
    }

    public static void FadeFrom(GameObject target, float alpha, float time)
    {
        object[] args = new object[] { "alpha", alpha, "time", time };
        FadeFrom(target, Hash(args));
    }

    public static void FadeTo(GameObject target, Hashtable args)
    {
        ColorTo(target, args);
    }

    public static void FadeTo(GameObject target, float alpha, float time)
    {
        object[] args = new object[] { "alpha", alpha, "time", time };
        FadeTo(target, Hash(args));
    }

    public static void FadeUpdate(GameObject target, Hashtable args)
    {
        args["a"] = args["alpha"];
        ColorUpdate(target, args);
    }

    public static void FadeUpdate(GameObject target, float alpha, float time)
    {
        object[] args = new object[] { "alpha", alpha, "time", time };
        FadeUpdate(target, Hash(args));
    }

    private void FixedUpdate()
    {
        if (this.isRunning && this.physics)
        {
            if (!this.reverse)
            {
                if (this.percentage < 1f)
                {
                    this.TweenUpdate();
                }
                else
                {
                    this.TweenComplete();
                }
            }
            else if (this.percentage > 0f)
            {
                this.TweenUpdate();
            }
            else
            {
                this.TweenComplete();
            }
        }
    }

    public static float FloatUpdate(float currentValue, float targetValue, float speed)
    {
        float num = targetValue - currentValue;
        currentValue += (num * speed) * Time.deltaTime;
        return currentValue;
    }

    private void GenerateAudioToTargets()
    {
        this.vector2s = new Vector2[3];
        if (this.tweenArguments.Contains("audiosource"))
        {
            this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
        }
        else if (base.GetComponent<AudioSource>() != null)
        {
            this.audioSource = base.GetComponent<AudioSource>();
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: AudioTo requires an AudioSource.");
            this.Dispose();
        }
        this.vector2s[0] = this.vector2s[1] = new Vector2(this.audioSource.volume, this.audioSource.pitch);
        if (this.tweenArguments.Contains("volume"))
        {
            this.vector2s[1].x = (float)this.tweenArguments["volume"];
        }
        if (this.tweenArguments.Contains("pitch"))
        {
            this.vector2s[1].y = (float)this.tweenArguments["pitch"];
        }
    }

    private void GenerateColorTargets()
    {
        this.colors = new Color[1, 3];
        this.colors[0, 0] = (Color)this.tweenArguments["from"];
        this.colors[0, 1] = (Color)this.tweenArguments["to"];
    }

    private void GenerateColorToTargets()
    {
        if (base.GetComponent<Image>() != null)
        {
            this.colors = new Color[1, 3];
            this.colors[0, 0] = this.colors[0, 1] = base.GetComponent<Image>().color;
        }
        else if (base.GetComponent<Text>() != null)
        {
            this.colors = new Color[1, 3];
            this.colors[0, 0] = this.colors[0, 1] = base.GetComponent<Text>().material.color;
        }
        else if (base.GetComponent<Renderer>() != null)
        {
            this.colors = new Color[base.GetComponent<Renderer>().materials.Length, 3];
            for (int i = 0; i < base.GetComponent<Renderer>().materials.Length; i++)
            {
                this.colors[i, 0] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
                this.colors[i, 1] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
            }
        }
        else if (base.GetComponent<Light>() != null)
        {
            this.colors = new Color[1, 3];
            this.colors[0, 0] = this.colors[0, 1] = base.GetComponent<Light>().color;
        }
        else
        {
            this.colors = new Color[1, 3];
        }
        if (this.tweenArguments.Contains("color"))
        {
            for (int j = 0; j < this.colors.GetLength(0); j++)
            {
                this.colors[j, 1] = (Color)this.tweenArguments["color"];
            }
        }
        else
        {
            if (this.tweenArguments.Contains("r"))
            {
                for (int k = 0; k < this.colors.GetLength(0); k++)
                {
                    this.colors[k, 1].r = (float)this.tweenArguments["r"];
                }
            }
            if (this.tweenArguments.Contains("g"))
            {
                for (int m = 0; m < this.colors.GetLength(0); m++)
                {
                    this.colors[m, 1].g = (float)this.tweenArguments["g"];
                }
            }
            if (this.tweenArguments.Contains("b"))
            {
                for (int n = 0; n < this.colors.GetLength(0); n++)
                {
                    this.colors[n, 1].b = (float)this.tweenArguments["b"];
                }
            }
            if (this.tweenArguments.Contains("a"))
            {
                for (int num6 = 0; num6 < this.colors.GetLength(0); num6++)
                {
                    this.colors[num6, 1].a = (float)this.tweenArguments["a"];
                }
            }
        }
        if (this.tweenArguments.Contains("amount"))
        {
            for (int num7 = 0; num7 < this.colors.GetLength(0); num7++)
            {
                this.colors[num7, 1].a = (float)this.tweenArguments["amount"];
            }
        }
        else if (this.tweenArguments.Contains("alpha"))
        {
            for (int num8 = 0; num8 < this.colors.GetLength(0); num8++)
            {
                this.colors[num8, 1].a = (float)this.tweenArguments["alpha"];
            }
        }
    }

    private void GenerateFloatTargets()
    {
        this.floats = new float[3];
        this.floats[0] = (float)this.tweenArguments["from"];
        this.floats[1] = (float)this.tweenArguments["to"];
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs((float)(this.floats[0] - this.floats[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private static string GenerateID()
    {
        return Guid.NewGuid().ToString();
    }

    private void GenerateLookToTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.thisTransform.eulerAngles;
        if (this.tweenArguments.Contains("looktarget"))
        {
            if (this.tweenArguments["looktarget"].GetType() == typeof(Transform))
            {
                Vector3? nullable = (Vector3?)this.tweenArguments["up"];
                this.thisTransform.LookAt((Transform)this.tweenArguments["looktarget"], !nullable.HasValue ? Defaults.up : nullable.Value);
            }
            else if (this.tweenArguments["looktarget"].GetType() == typeof(Vector3))
            {
                Vector3? nullable2 = (Vector3?)this.tweenArguments["up"];
                this.thisTransform.LookAt((Vector3)this.tweenArguments["looktarget"], !nullable2.HasValue ? Defaults.up : nullable2.Value);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!");
            this.Dispose();
        }
        this.vector3s[1] = this.thisTransform.eulerAngles;
        this.thisTransform.eulerAngles = this.vector3s[0];
        if (this.tweenArguments.Contains("axis"))
        {
            string key = (string)this.tweenArguments["axis"];
            if (key != null)
            {
                int num;
                if (f__switchSmap13 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                    dictionary.Add("x", 0);
                    dictionary.Add("y", 1);
                    dictionary.Add("z", 2);
                    f__switchSmap13 = dictionary;
                }
                if (f__switchSmap13.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            this.vector3s[1].y = this.vector3s[0].y;
                            this.vector3s[1].z = this.vector3s[0].z;
                            break;

                        case 1:
                            this.vector3s[1].x = this.vector3s[0].x;
                            this.vector3s[1].z = this.vector3s[0].z;
                            break;

                        case 2:
                            this.vector3s[1].x = this.vector3s[0].x;
                            this.vector3s[1].y = this.vector3s[0].y;
                            break;
                    }
                }
            }
        }
        this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
        if (this.tweenArguments.Contains("speed"))
        {
            float num2 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num2 / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateMoveByTargets()
    {
        Vector3 vector;
        this.vector3s = new Vector3[6];
        this.vector3s[4] = this.thisTransform.eulerAngles;
        this.vector3s[3] = vector = this.thisTransform.position;
        this.vector3s[0] = this.vector3s[1] = vector;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = this.vector3s[0] + ((Vector3)this.tweenArguments["amount"]);
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = this.vector3s[0].x + ((float)this.tweenArguments["x"]);
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = this.vector3s[0].y + ((float)this.tweenArguments["y"]);
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = this.vector3s[0].z + ((float)this.tweenArguments["z"]);
            }
        }
        this.thisTransform.Translate(this.vector3s[1], this.space);
        this.vector3s[5] = this.thisTransform.position;
        this.thisTransform.position = this.vector3s[0];
        if (this.tweenArguments.Contains("orienttopath") && ((bool)this.tweenArguments["orienttopath"]))
        {
            this.tweenArguments["looktarget"] = this.vector3s[1];
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateMoveToPathTargets()
    {
        Vector3[] vectorArray2;
        bool flag;
        int num2;
        if (this.tweenArguments["path"].GetType() == typeof(Vector3[]))
        {
            Vector3[] sourceArray = (Vector3[])this.tweenArguments["path"];
            if (sourceArray.Length == 1)
            {
                UnityEngine.Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
                this.Dispose();
            }
            vectorArray2 = new Vector3[sourceArray.Length];
            Array.Copy(sourceArray, vectorArray2, sourceArray.Length);
        }
        else
        {
            Transform[] transformArray = (Transform[])this.tweenArguments["path"];
            if (transformArray.Length == 1)
            {
                UnityEngine.Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
                this.Dispose();
            }
            vectorArray2 = new Vector3[transformArray.Length];
            for (int i = 0; i < transformArray.Length; i++)
            {
                vectorArray2[i] = transformArray[i].position;
            }
        }
        if (this.thisTransform.position != vectorArray2[0])
        {
            if (!this.tweenArguments.Contains("movetopath") || ((bool)this.tweenArguments["movetopath"]))
            {
                flag = true;
                num2 = 3;
            }
            else
            {
                flag = false;
                num2 = 2;
            }
        }
        else
        {
            flag = false;
            num2 = 2;
        }
        this.vector3s = new Vector3[vectorArray2.Length + num2];
        if (flag)
        {
            this.vector3s[1] = this.thisTransform.position;
            num2 = 2;
        }
        else
        {
            num2 = 1;
        }
        Array.Copy(vectorArray2, 0, this.vector3s, num2, vectorArray2.Length);
        this.vector3s[0] = this.vector3s[1] + (this.vector3s[1] - this.vector3s[2]);
        this.vector3s[this.vector3s.Length - 1] = this.vector3s[this.vector3s.Length - 2] + (this.vector3s[this.vector3s.Length - 2] - this.vector3s[this.vector3s.Length - 3]);
        if (this.vector3s[1] == this.vector3s[this.vector3s.Length - 2])
        {
            Vector3[] destinationArray = new Vector3[this.vector3s.Length];
            Array.Copy(this.vector3s, destinationArray, this.vector3s.Length);
            destinationArray[0] = destinationArray[destinationArray.Length - 3];
            destinationArray[destinationArray.Length - 1] = destinationArray[2];
            this.vector3s = new Vector3[destinationArray.Length];
            Array.Copy(destinationArray, this.vector3s, destinationArray.Length);
        }
        this.path = new CRSpline(this.vector3s);
        if (this.tweenArguments.Contains("speed"))
        {
            float num3 = PathLength(this.vector3s);
            this.time = num3 / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateMoveToTargets()
    {
        this.vector3s = new Vector3[3];
        if (this.isLocal)
        {
            this.vector3s[0] = this.vector3s[1] = this.thisTransform.localPosition;
        }
        else
        {
            this.vector3s[0] = this.vector3s[1] = this.thisTransform.position;
        }
        if (this.tweenArguments.Contains("position"))
        {
            if (this.tweenArguments["position"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)this.tweenArguments["position"];
                this.vector3s[1] = transform.position;
            }
            else if (this.tweenArguments["position"].GetType() == typeof(Vector3))
            {
                this.vector3s[1] = (Vector3)this.tweenArguments["position"];
            }
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("orienttopath") && ((bool)this.tweenArguments["orienttopath"]))
        {
            this.tweenArguments["looktarget"] = this.vector3s[1];
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GeneratePunchPositionTargets()
    {
        this.vector3s = new Vector3[5];
        this.vector3s[4] = this.thisTransform.eulerAngles;
        this.vector3s[0] = this.thisTransform.position;
        this.vector3s[1] = this.vector3s[3] = Vector3.zero;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GeneratePunchRotationTargets()
    {
        this.vector3s = new Vector3[4];
        this.vector3s[0] = this.thisTransform.eulerAngles;
        this.vector3s[1] = this.vector3s[3] = Vector3.zero;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GeneratePunchScaleTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.thisTransform.localScale;
        this.vector3s[1] = Vector3.zero;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GenerateRectTargets()
    {
        this.rects = new Rect[3];
        this.rects[0] = (Rect)this.tweenArguments["from"];
        this.rects[1] = (Rect)this.tweenArguments["to"];
    }

    private void GenerateRotateAddTargets()
    {
        Vector3 vector;
        this.vector3s = new Vector3[5];
        this.vector3s[3] = vector = this.thisTransform.eulerAngles;
        this.vector3s[0] = this.vector3s[1] = vector;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x += (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y += (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z += (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateRotateByTargets()
    {
        Vector3 vector;
        this.vector3s = new Vector3[4];
        this.vector3s[3] = vector = this.thisTransform.eulerAngles;
        this.vector3s[0] = this.vector3s[1] = vector;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] += Vector3.Scale((Vector3)this.tweenArguments["amount"], new Vector3(360f, 360f, 360f));
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x += 360f * ((float)this.tweenArguments["x"]);
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y += 360f * ((float)this.tweenArguments["y"]);
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z += 360f * ((float)this.tweenArguments["z"]);
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateRotateToTargets()
    {
        this.vector3s = new Vector3[3];
        if (this.isLocal)
        {
            this.vector3s[0] = this.vector3s[1] = this.thisTransform.localEulerAngles;
        }
        else
        {
            this.vector3s[0] = this.vector3s[1] = this.thisTransform.eulerAngles;
        }
        if (this.tweenArguments.Contains("rotation"))
        {
            if (this.tweenArguments["rotation"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)this.tweenArguments["rotation"];
                this.vector3s[1] = transform.eulerAngles;
            }
            else if (this.tweenArguments["rotation"].GetType() == typeof(Vector3))
            {
                this.vector3s[1] = (Vector3)this.tweenArguments["rotation"];
            }
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
        this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateScaleAddTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.vector3s[1] = this.thisTransform.localScale;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x += (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y += (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z += (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateScaleByTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.vector3s[1] = this.thisTransform.localScale;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3)this.tweenArguments["amount"]);
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x *= (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y *= (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z *= (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateScaleToTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.vector3s[1] = this.thisTransform.localScale;
        if (this.tweenArguments.Contains("scale"))
        {
            if (this.tweenArguments["scale"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)this.tweenArguments["scale"];
                this.vector3s[1] = transform.localScale;
            }
            else if (this.tweenArguments["scale"].GetType() == typeof(Vector3))
            {
                this.vector3s[1] = (Vector3)this.tweenArguments["scale"];
            }
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateShakePositionTargets()
    {
        this.vector3s = new Vector3[4];
        this.vector3s[3] = this.thisTransform.eulerAngles;
        this.vector3s[0] = this.thisTransform.position;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GenerateShakeRotationTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.thisTransform.eulerAngles;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GenerateShakeScaleTargets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = this.thisTransform.localScale;
        if (this.tweenArguments.Contains("amount"))
        {
            this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
        }
        else
        {
            if (this.tweenArguments.Contains("x"))
            {
                this.vector3s[1].x = (float)this.tweenArguments["x"];
            }
            if (this.tweenArguments.Contains("y"))
            {
                this.vector3s[1].y = (float)this.tweenArguments["y"];
            }
            if (this.tweenArguments.Contains("z"))
            {
                this.vector3s[1].z = (float)this.tweenArguments["z"];
            }
        }
    }

    private void GenerateStabTargets()
    {
        if (this.tweenArguments.Contains("audiosource"))
        {
            this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
        }
        else if (base.GetComponent<AudioSource>() != null)
        {
            this.audioSource = base.GetComponent<AudioSource>();
        }
        else
        {
            base.gameObject.AddComponent<AudioSource>();
            this.audioSource = base.GetComponent<AudioSource>();
            this.audioSource.playOnAwake = false;
        }
        this.audioSource.clip = (AudioClip)this.tweenArguments["audioclip"];
        if (this.tweenArguments.Contains("pitch"))
        {
            this.audioSource.pitch = (float)this.tweenArguments["pitch"];
        }
        if (this.tweenArguments.Contains("volume"))
        {
            this.audioSource.volume = (float)this.tweenArguments["volume"];
        }
        this.time = this.audioSource.clip.length / this.audioSource.pitch;
    }

    private void GenerateTargets()
    {
        string type = this.type;
        if (type != null)
        {
            Dictionary<string, int> dictionary;
            int num;
            if (f__switchSmap12 == null)
            {
                dictionary = new Dictionary<string, int>(10);
                dictionary.Add("value", 0);
                dictionary.Add("color", 1);
                dictionary.Add("audio", 2);
                dictionary.Add("move", 3);
                dictionary.Add("scale", 4);
                dictionary.Add("rotate", 5);
                dictionary.Add("shake", 6);
                dictionary.Add("punch", 7);
                dictionary.Add("look", 8);
                dictionary.Add("stab", 9);
                f__switchSmap12 = dictionary;
            }
            if (f__switchSmap12.TryGetValue(type, out num))
            {
                string method;
                int num2;
                switch (num)
                {
                    case 0:
                        method = this.method;
                        if (method != null)
                        {
                            if (f__switchSmap9 == null)
                            {
                                dictionary = new Dictionary<string, int>(5);
                                dictionary.Add("float", 0);
                                dictionary.Add("vector2", 1);
                                dictionary.Add("vector3", 2);
                                dictionary.Add("color", 3);
                                dictionary.Add("rect", 4);
                                f__switchSmap9 = dictionary;
                            }
                            if (f__switchSmap9.TryGetValue(method, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GenerateFloatTargets();
                                        this.apply = new ApplyTween(this.ApplyFloatTargets);
                                        return;

                                    case 1:
                                        this.GenerateVector2Targets();
                                        this.apply = new ApplyTween(this.ApplyVector2Targets);
                                        return;

                                    case 2:
                                        this.GenerateVector3Targets();
                                        this.apply = new ApplyTween(this.ApplyVector3Targets);
                                        return;

                                    case 3:
                                        this.GenerateColorTargets();
                                        this.apply = new ApplyTween(this.ApplyColorTargets);
                                        return;

                                    case 4:
                                        this.GenerateRectTargets();
                                        this.apply = new ApplyTween(this.ApplyRectTargets);
                                        return;
                                }
                            }
                        }
                        break;

                    case 1:
                        method = this.method;
                        if (method != null)
                        {
                            if (f__switchSmapA == null)
                            {
                                dictionary = new Dictionary<string, int>(1);
                                dictionary.Add("to", 0);
                                f__switchSmapA = dictionary;
                            }
                            if (f__switchSmapA.TryGetValue(method, out num2) && (num2 == 0))
                            {
                                this.GenerateColorToTargets();
                                this.apply = new ApplyTween(this.ApplyColorToTargets);
                            }
                        }
                        break;

                    case 2:
                        method = this.method;
                        if (method != null)
                        {
                            if (f__switchSmapB == null)
                            {
                                dictionary = new Dictionary<string, int>(1);
                                dictionary.Add("to", 0);
                                f__switchSmapB = dictionary;
                            }
                            if (f__switchSmapB.TryGetValue(method, out num2) && (num2 == 0))
                            {
                                this.GenerateAudioToTargets();
                                this.apply = new ApplyTween(this.ApplyAudioToTargets);
                            }
                        }
                        break;

                    case 3:
                        method = this.method;
                        if (method != null)
                        {
                            if (f__switchSmapC == null)
                            {
                                dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("to", 0);
                                dictionary.Add("by", 1);
                                dictionary.Add("add", 1);
                                f__switchSmapC = dictionary;
                            }
                            if (f__switchSmapC.TryGetValue(method, out num2))
                            {
                                if (num2 == 0)
                                {
                                    if (this.tweenArguments.Contains("path"))
                                    {
                                        this.GenerateMoveToPathTargets();
                                        this.apply = new ApplyTween(this.ApplyMoveToPathTargets);
                                    }
                                    else
                                    {
                                        this.GenerateMoveToTargets();
                                        this.apply = new ApplyTween(this.ApplyMoveToTargets);
                                    }
                                }
                                else if (num2 == 1)
                                {
                                    this.GenerateMoveByTargets();
                                    this.apply = new ApplyTween(this.ApplyMoveByTargets);
                                    break;
                                }
                            }
                        }
                        break;

                    case 4:
                        method = this.method;
                        if (method != null)
                        {
                            if (f__switchSmapD == null)
                            {
                                dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("to", 0);
                                dictionary.Add("by", 1);
                                dictionary.Add("add", 2);
                                f__switchSmapD = dictionary;
                            }
                            if (f__switchSmapD.TryGetValue(method, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GenerateScaleToTargets();
                                        this.apply = new ApplyTween(this.ApplyScaleToTargets);
                                        return;

                                    case 1:
                                        this.GenerateScaleByTargets();
                                        this.apply = new ApplyTween(this.ApplyScaleToTargets);
                                        return;

                                    case 2:
                                        this.GenerateScaleAddTargets();
                                        this.apply = new ApplyTween(this.ApplyScaleToTargets);
                                        return;
                                }
                            }
                        }
                        break;

                    case 5:
                        method = this.method;
                        if (method != null)
                        {
                            if (f__switchSmapE == null)
                            {
                                dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("to", 0);
                                dictionary.Add("add", 1);
                                dictionary.Add("by", 2);
                                f__switchSmapE = dictionary;
                            }
                            if (f__switchSmapE.TryGetValue(method, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GenerateRotateToTargets();
                                        this.apply = new ApplyTween(this.ApplyRotateToTargets);
                                        return;

                                    case 1:
                                        this.GenerateRotateAddTargets();
                                        this.apply = new ApplyTween(this.ApplyRotateAddTargets);
                                        return;

                                    case 2:
                                        this.GenerateRotateByTargets();
                                        this.apply = new ApplyTween(this.ApplyRotateAddTargets);
                                        return;
                                }
                            }
                        }
                        break;

                    case 6:
                        method = this.method;
                        if (method != null)
                        {
                            if (f__switchSmapF == null)
                            {
                                dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("position", 0);
                                dictionary.Add("scale", 1);
                                dictionary.Add("rotation", 2);
                                f__switchSmapF = dictionary;
                            }
                            if (f__switchSmapF.TryGetValue(method, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GenerateShakePositionTargets();
                                        this.apply = new ApplyTween(this.ApplyShakePositionTargets);
                                        return;

                                    case 1:
                                        this.GenerateShakeScaleTargets();
                                        this.apply = new ApplyTween(this.ApplyShakeScaleTargets);
                                        return;

                                    case 2:
                                        this.GenerateShakeRotationTargets();
                                        this.apply = new ApplyTween(this.ApplyShakeRotationTargets);
                                        return;
                                }
                            }
                        }
                        break;

                    case 7:
                        method = this.method;
                        if (method != null)
                        {
                            if (f__switchSmap10 == null)
                            {
                                dictionary = new Dictionary<string, int>(3);
                                dictionary.Add("position", 0);
                                dictionary.Add("rotation", 1);
                                dictionary.Add("scale", 2);
                                f__switchSmap10 = dictionary;
                            }
                            if (f__switchSmap10.TryGetValue(method, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.GeneratePunchPositionTargets();
                                        this.apply = new ApplyTween(this.ApplyPunchPositionTargets);
                                        return;

                                    case 1:
                                        this.GeneratePunchRotationTargets();
                                        this.apply = new ApplyTween(this.ApplyPunchRotationTargets);
                                        return;

                                    case 2:
                                        this.GeneratePunchScaleTargets();
                                        this.apply = new ApplyTween(this.ApplyPunchScaleTargets);
                                        return;
                                }
                            }
                        }
                        break;

                    case 8:
                        method = this.method;
                        if (method != null)
                        {
                            if (f__switchSmap11 == null)
                            {
                                dictionary = new Dictionary<string, int>(1);
                                dictionary.Add("to", 0);
                                f__switchSmap11 = dictionary;
                            }
                            if (f__switchSmap11.TryGetValue(method, out num2) && (num2 == 0))
                            {
                                this.GenerateLookToTargets();
                                this.apply = new ApplyTween(this.ApplyLookToTargets);
                            }
                        }
                        break;

                    case 9:
                        this.GenerateStabTargets();
                        this.apply = new ApplyTween(this.ApplyStabTargets);
                        break;
                }
            }
        }
    }

    private void GenerateVector2Targets()
    {
        this.vector2s = new Vector2[3];
        this.vector2s[0] = (Vector2)this.tweenArguments["from"];
        this.vector2s[1] = (Vector2)this.tweenArguments["to"];
        if (this.tweenArguments.Contains("speed"))
        {
            Vector3 a = new Vector3(this.vector2s[0].x, this.vector2s[0].y, 0f);
            Vector3 b = new Vector3(this.vector2s[1].x, this.vector2s[1].y, 0f);
            float num = Math.Abs(Vector3.Distance(a, b));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GenerateVector3Targets()
    {
        this.vector3s = new Vector3[3];
        this.vector3s[0] = (Vector3)this.tweenArguments["from"];
        this.vector3s[1] = (Vector3)this.tweenArguments["to"];
        if (this.tweenArguments.Contains("speed"))
        {
            float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
            this.time = num / ((float)this.tweenArguments["speed"]);
        }
    }

    private void GetEasingFunction()
    {
        switch (this.easeType)
        {
            case EaseType.easeInQuad:
                this.ease = new EasingFunction(this.easeInQuad);
                break;

            case EaseType.easeOutQuad:
                this.ease = new EasingFunction(this.easeOutQuad);
                break;

            case EaseType.easeInOutQuad:
                this.ease = new EasingFunction(this.easeInOutQuad);
                break;

            case EaseType.easeInCubic:
                this.ease = new EasingFunction(this.easeInCubic);
                break;

            case EaseType.easeOutCubic:
                this.ease = new EasingFunction(this.easeOutCubic);
                break;

            case EaseType.easeInOutCubic:
                this.ease = new EasingFunction(this.easeInOutCubic);
                break;

            case EaseType.easeInQuart:
                this.ease = new EasingFunction(this.easeInQuart);
                break;

            case EaseType.easeOutQuart:
                this.ease = new EasingFunction(this.easeOutQuart);
                break;

            case EaseType.easeInOutQuart:
                this.ease = new EasingFunction(this.easeInOutQuart);
                break;

            case EaseType.easeInQuint:
                this.ease = new EasingFunction(this.easeInQuint);
                break;

            case EaseType.easeOutQuint:
                this.ease = new EasingFunction(this.easeOutQuint);
                break;

            case EaseType.easeInOutQuint:
                this.ease = new EasingFunction(this.easeInOutQuint);
                break;

            case EaseType.easeInSine:
                this.ease = new EasingFunction(this.easeInSine);
                break;

            case EaseType.easeOutSine:
                this.ease = new EasingFunction(this.easeOutSine);
                break;

            case EaseType.easeInOutSine:
                this.ease = new EasingFunction(this.easeInOutSine);
                break;

            case EaseType.easeInExpo:
                this.ease = new EasingFunction(this.easeInExpo);
                break;

            case EaseType.easeOutExpo:
                this.ease = new EasingFunction(this.easeOutExpo);
                break;

            case EaseType.easeInOutExpo:
                this.ease = new EasingFunction(this.easeInOutExpo);
                break;

            case EaseType.easeInCirc:
                this.ease = new EasingFunction(this.easeInCirc);
                break;

            case EaseType.easeOutCirc:
                this.ease = new EasingFunction(this.easeOutCirc);
                break;

            case EaseType.easeInOutCirc:
                this.ease = new EasingFunction(this.easeInOutCirc);
                break;

            case EaseType.linear:
                this.ease = new EasingFunction(this.linear);
                break;

            case EaseType.spring:
                this.ease = new EasingFunction(this.spring);
                break;

            case EaseType.easeInBounce:
                this.ease = new EasingFunction(this.easeInBounce);
                break;

            case EaseType.easeOutBounce:
                this.ease = new EasingFunction(this.easeOutBounce);
                break;

            case EaseType.easeInOutBounce:
                this.ease = new EasingFunction(this.easeInOutBounce);
                break;

            case EaseType.easeInBack:
                this.ease = new EasingFunction(this.easeInBack);
                break;

            case EaseType.easeOutBack:
                this.ease = new EasingFunction(this.easeOutBack);
                break;

            case EaseType.easeInOutBack:
                this.ease = new EasingFunction(this.easeInOutBack);
                break;

            case EaseType.easeInElastic:
                this.ease = new EasingFunction(this.easeInElastic);
                break;

            case EaseType.easeOutElastic:
                this.ease = new EasingFunction(this.easeOutElastic);
                break;

            case EaseType.easeInOutElastic:
                this.ease = new EasingFunction(this.easeInOutElastic);
                break;
        }
    }

    public static Hashtable Hash(params object[] args)
    {
        Hashtable hashtable = new Hashtable(args.Length / 2);
        if ((args.Length % 2) != 0)
        {
            UnityEngine.Debug.LogError("Tween Error: Hash requires an even number of arguments!");
            return null;
        }
        for (int i = 0; i < (args.Length - 1); i += 2)
        {
            hashtable.Add(args[i], args[i + 1]);
        }
        return hashtable;
    }

    public static void Init(GameObject target)
    {
        MoveBy(target, Vector3.zero, 0f);
    }

    private static Vector3 Interp(Vector3[] pts, float t)
    {
        int num = pts.Length - 3;
        int index = Mathf.Min(Mathf.FloorToInt(t * num), num - 1);
        float num3 = (t * num) - index;
        Vector3 vector = pts[index];
        Vector3 vector2 = pts[index + 1];
        Vector3 vector3 = pts[index + 2];
        Vector3 vector4 = pts[index + 3];
        return (Vector3)(0.5f * (((((((-vector + (3f * vector2)) - (3f * vector3)) + vector4) * ((num3 * num3) * num3)) + (((((2f * vector) - (5f * vector2)) + (4f * vector3)) - vector4) * (num3 * num3))) + ((-vector + vector3) * num3)) + (2f * vector2)));
    }

    private void LateUpdate()
    {
        if ((this.tweenArguments.Contains("looktarget") && this.isRunning) && (((this.type == "move") || (this.type == "shake")) || (this.type == "punch")))
        {
            LookUpdate(base.gameObject, this.tweenArguments);
        }
    }

    private static void Launch(GameObject target, Hashtable args)
    {
        if (!args.Contains("id"))
        {
            args["id"] = GenerateID();
        }
        if (!args.Contains("target"))
        {
            args["target"] = target;
        }
        tweens.Insert(0, args);
        target.AddComponent<iTween>();
    }

    private float linear(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, value);
    }

    public static void LookFrom(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        Vector3 eulerAngles = target.transform.eulerAngles;
        if (args["looktarget"].GetType() == typeof(Transform))
        {
            Vector3? nullable = (Vector3?)args["up"];
            target.transform.LookAt((Transform)args["looktarget"], !nullable.HasValue ? Defaults.up : nullable.Value);
        }
        else if (args["looktarget"].GetType() == typeof(Vector3))
        {
            Vector3? nullable2 = (Vector3?)args["up"];
            target.transform.LookAt((Vector3)args["looktarget"], !nullable2.HasValue ? Defaults.up : nullable2.Value);
        }
        if (args.Contains("axis"))
        {
            Vector3 vector2 = target.transform.eulerAngles;
            string key = (string)args["axis"];
            if (key != null)
            {
                int num;
                if (f__switchSmap8 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                    dictionary.Add("x", 0);
                    dictionary.Add("y", 1);
                    dictionary.Add("z", 2);
                    f__switchSmap8 = dictionary;
                }
                if (f__switchSmap8.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            vector2.y = eulerAngles.y;
                            vector2.z = eulerAngles.z;
                            break;

                        case 1:
                            vector2.x = eulerAngles.x;
                            vector2.z = eulerAngles.z;
                            break;

                        case 2:
                            vector2.x = eulerAngles.x;
                            vector2.y = eulerAngles.y;
                            break;
                    }
                }
            }
            target.transform.eulerAngles = vector2;
        }
        args["rotation"] = eulerAngles;
        args["type"] = "rotate";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void LookFrom(GameObject target, Vector3 looktarget, float time)
    {
        object[] args = new object[] { "looktarget", looktarget, "time", time };
        LookFrom(target, Hash(args));
    }

    public static void LookTo(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        if (args.Contains("looktarget") && (args["looktarget"].GetType() == typeof(Transform)))
        {
            Transform transform = (Transform)args["looktarget"];
            args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        args["type"] = "look";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void LookTo(GameObject target, Vector3 looktarget, float time)
    {
        object[] args = new object[] { "looktarget", looktarget, "time", time };
        LookTo(target, Hash(args));
    }

    public static void LookUpdate(GameObject target, Hashtable args)
    {
        float updateTime;
        CleanArgs(args);
        Vector3[] vectorArray = new Vector3[5];
        if (args.Contains("looktime"))
        {
            updateTime = (float)args["looktime"];
            updateTime *= Defaults.updateTimePercentage;
        }
        else if (args.Contains("time"))
        {
            updateTime = ((float)args["time"]) * 0.15f;
            updateTime *= Defaults.updateTimePercentage;
        }
        else
        {
            updateTime = Defaults.updateTime;
        }
        vectorArray[0] = target.transform.eulerAngles;
        if (args.Contains("looktarget"))
        {
            if (args["looktarget"].GetType() == typeof(Transform))
            {
                Vector3? nullable = (Vector3?)args["up"];
                target.transform.LookAt((Transform)args["looktarget"], !nullable.HasValue ? Defaults.up : nullable.Value);
            }
            else if (args["looktarget"].GetType() == typeof(Vector3))
            {
                Vector3? nullable2 = (Vector3?)args["up"];
                target.transform.LookAt((Vector3)args["looktarget"], !nullable2.HasValue ? Defaults.up : nullable2.Value);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!");
            return;
        }
        vectorArray[1] = target.transform.eulerAngles;
        target.transform.eulerAngles = vectorArray[0];
        vectorArray[3].x = Mathf.SmoothDampAngle(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
        vectorArray[3].y = Mathf.SmoothDampAngle(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
        vectorArray[3].z = Mathf.SmoothDampAngle(vectorArray[0].z, vectorArray[1].z, ref vectorArray[2].z, updateTime);
        target.transform.eulerAngles = vectorArray[3];
        if (args.Contains("axis"))
        {
            vectorArray[4] = target.transform.eulerAngles;
            string key = (string)args["axis"];
            if (key != null)
            {
                int num2;
                if (f__switchSmap14 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                    dictionary.Add("x", 0);
                    dictionary.Add("y", 1);
                    dictionary.Add("z", 2);
                    f__switchSmap14 = dictionary;
                }
                if (f__switchSmap14.TryGetValue(key, out num2))
                {
                    switch (num2)
                    {
                        case 0:
                            vectorArray[4].y = vectorArray[0].y;
                            vectorArray[4].z = vectorArray[0].z;
                            break;

                        case 1:
                            vectorArray[4].x = vectorArray[0].x;
                            vectorArray[4].z = vectorArray[0].z;
                            break;

                        case 2:
                            vectorArray[4].x = vectorArray[0].x;
                            vectorArray[4].y = vectorArray[0].y;
                            break;
                    }
                }
            }
            target.transform.eulerAngles = vectorArray[4];
        }
    }

    public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
    {
        object[] args = new object[] { "looktarget", looktarget, "time", time };
        LookUpdate(target, Hash(args));
    }

    public static void MoveAdd(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "move";
        args["method"] = "add";
        Launch(target, args);
    }

    public static void MoveAdd(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        MoveAdd(target, Hash(args));
    }

    public static void MoveBy(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "move";
        args["method"] = "by";
        Launch(target, args);
    }

    public static void MoveBy(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        MoveBy(target, Hash(args));
    }

    public static void MoveFrom(GameObject target, Hashtable args)
    {
        bool isLocal;
        args = CleanArgs(args);
        if (args.Contains("islocal"))
        {
            isLocal = (bool)args["islocal"];
        }
        else
        {
            isLocal = Defaults.isLocal;
        }
        if (args.Contains("path"))
        {
            Vector3[] vectorArray2;
            if (args["path"].GetType() == typeof(Vector3[]))
            {
                Vector3[] sourceArray = (Vector3[])args["path"];
                vectorArray2 = new Vector3[sourceArray.Length];
                Array.Copy(sourceArray, vectorArray2, sourceArray.Length);
            }
            else
            {
                Transform[] transformArray = (Transform[])args["path"];
                vectorArray2 = new Vector3[transformArray.Length];
                for (int i = 0; i < transformArray.Length; i++)
                {
                    vectorArray2[i] = transformArray[i].position;
                }
            }
            if (vectorArray2[vectorArray2.Length - 1] != target.transform.position)
            {
                Vector3[] destinationArray = new Vector3[vectorArray2.Length + 1];
                Array.Copy(vectorArray2, destinationArray, vectorArray2.Length);
                if (isLocal)
                {
                    destinationArray[destinationArray.Length - 1] = target.transform.localPosition;
                    target.transform.localPosition = destinationArray[0];
                }
                else
                {
                    destinationArray[destinationArray.Length - 1] = target.transform.position;
                    target.transform.position = destinationArray[0];
                }
                args["path"] = destinationArray;
            }
            else
            {
                if (isLocal)
                {
                    target.transform.localPosition = vectorArray2[0];
                }
                else
                {
                    target.transform.position = vectorArray2[0];
                }
                args["path"] = vectorArray2;
            }
        }
        else
        {
            Vector3 position;
            Vector3 vector2;
            if (isLocal)
            {
                vector2 = position = target.transform.localPosition;
            }
            else
            {
                vector2 = position = target.transform.position;
            }
            if (args.Contains("position"))
            {
                if (args["position"].GetType() == typeof(Transform))
                {
                    Transform transform = (Transform)args["position"];
                    position = transform.position;
                }
                else if (args["position"].GetType() == typeof(Vector3))
                {
                    position = (Vector3)args["position"];
                }
            }
            else
            {
                if (args.Contains("x"))
                {
                    position.x = (float)args["x"];
                }
                if (args.Contains("y"))
                {
                    position.y = (float)args["y"];
                }
                if (args.Contains("z"))
                {
                    position.z = (float)args["z"];
                }
            }
            if (isLocal)
            {
                target.transform.localPosition = position;
            }
            else
            {
                target.transform.position = position;
            }
            args["position"] = vector2;
        }
        args["type"] = "move";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void MoveFrom(GameObject target, Vector3 position, float time)
    {
        object[] args = new object[] { "position", position, "time", time };
        MoveFrom(target, Hash(args));
    }

    public static void MoveTo(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        if (args.Contains("position") && (args["position"].GetType() == typeof(Transform)))
        {
            Transform transform = (Transform)args["position"];
            args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        args["type"] = "move";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void MoveTo(GameObject target, Vector3 position, float time)
    {
        object[] args = new object[] { "position", position, "time", time };
        MoveTo(target, Hash(args));
    }

    public static void MoveUpdate(GameObject target, Hashtable args)
    {
        float updateTime;
        bool isLocal;
        CleanArgs(args);
        Vector3[] vectorArray = new Vector3[4];
        Vector3 position = target.transform.position;
        if (args.Contains("time"))
        {
            updateTime = (float)args["time"];
            updateTime *= Defaults.updateTimePercentage;
        }
        else
        {
            updateTime = Defaults.updateTime;
        }
        if (args.Contains("islocal"))
        {
            isLocal = (bool)args["islocal"];
        }
        else
        {
            isLocal = Defaults.isLocal;
        }
        if (isLocal)
        {
            vectorArray[0] = vectorArray[1] = target.transform.localPosition;
        }
        else
        {
            vectorArray[0] = vectorArray[1] = target.transform.position;
        }
        if (args.Contains("position"))
        {
            if (args["position"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["position"];
                vectorArray[1] = transform.position;
            }
            else if (args["position"].GetType() == typeof(Vector3))
            {
                vectorArray[1] = (Vector3)args["position"];
            }
        }
        else
        {
            if (args.Contains("x"))
            {
                vectorArray[1].x = (float)args["x"];
            }
            if (args.Contains("y"))
            {
                vectorArray[1].y = (float)args["y"];
            }
            if (args.Contains("z"))
            {
                vectorArray[1].z = (float)args["z"];
            }
        }
        vectorArray[3].x = Mathf.SmoothDamp(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
        vectorArray[3].y = Mathf.SmoothDamp(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
        vectorArray[3].z = Mathf.SmoothDamp(vectorArray[0].z, vectorArray[1].z, ref vectorArray[2].z, updateTime);
        if (args.Contains("orienttopath") && ((bool)args["orienttopath"]))
        {
            args["looktarget"] = vectorArray[3];
        }
        if (args.Contains("looktarget"))
        {
            LookUpdate(target, args);
        }
        if (isLocal)
        {
            target.transform.localPosition = vectorArray[3];
        }
        else
        {
            target.transform.position = vectorArray[3];
        }
        if (target.GetComponent<Rigidbody>() != null)
        {
            Vector3 vector3 = target.transform.position;
            target.transform.position = position;
            target.GetComponent<Rigidbody>().MovePosition(vector3);
        }
    }

    public static void MoveUpdate(GameObject target, Vector3 position, float time)
    {
        object[] args = new object[] { "position", position, "time", time };
        MoveUpdate(target, Hash(args));
    }

    private void OnDisable()
    {
        this.DisableKinematic();
    }

    private void OnEnable()
    {
        if (this.isRunning)
        {
            this.EnableKinematic();
        }
        if (this.isPaused)
        {
            this.isPaused = false;
            if (this.delay > 0f)
            {
                this.wasPaused = true;
                this.ResumeDelay();
            }
        }
    }

    private static Vector3[] PathControlPointGenerator(Vector3[] path)
    {
        Vector3[] sourceArray = path;
        int num = 2;
        Vector3[] destinationArray = new Vector3[sourceArray.Length + num];
        Array.Copy(sourceArray, 0, destinationArray, 1, sourceArray.Length);
        destinationArray[0] = destinationArray[1] + (destinationArray[1] - destinationArray[2]);
        destinationArray[destinationArray.Length - 1] = destinationArray[destinationArray.Length - 2] + (destinationArray[destinationArray.Length - 2] - destinationArray[destinationArray.Length - 3]);
        if (destinationArray[1] == destinationArray[destinationArray.Length - 2])
        {
            Vector3[] vectorArray3 = new Vector3[destinationArray.Length];
            Array.Copy(destinationArray, vectorArray3, destinationArray.Length);
            vectorArray3[0] = vectorArray3[vectorArray3.Length - 3];
            vectorArray3[vectorArray3.Length - 1] = vectorArray3[2];
            destinationArray = new Vector3[vectorArray3.Length];
            Array.Copy(vectorArray3, destinationArray, vectorArray3.Length);
        }
        return destinationArray;
    }

    public static float PathLength(Transform[] path)
    {
        Vector3[] vectorArray = new Vector3[path.Length];
        float num = 0f;
        for (int i = 0; i < path.Length; i++)
        {
            vectorArray[i] = path[i].position;
        }
        Vector3[] pts = PathControlPointGenerator(vectorArray);
        Vector3 a = Interp(pts, 0f);
        int num3 = path.Length * 20;
        for (int j = 1; j <= num3; j++)
        {
            float t = ((float)j) / ((float)num3);
            Vector3 b = Interp(pts, t);
            num += Vector3.Distance(a, b);
            a = b;
        }
        return num;
    }

    public static float PathLength(Vector3[] path)
    {
        float num = 0f;
        Vector3[] pts = PathControlPointGenerator(path);
        Vector3 a = Interp(pts, 0f);
        int num2 = path.Length * 20;
        for (int i = 1; i <= num2; i++)
        {
            float t = ((float)i) / ((float)num2);
            Vector3 b = Interp(pts, t);
            num += Vector3.Distance(a, b);
            a = b;
        }
        return num;
    }

    public static void Pause()
    {
        for (int i = 0; i < tweens.Count; i++)
        {
            Hashtable hashtable = tweens[i];
            GameObject target = (GameObject)hashtable["target"];
            Pause(target);
        }
    }

    public static void Pause(string type)
    {
        ArrayList list = new ArrayList();
        for (int i = 0; i < tweens.Count; i++)
        {
            Hashtable hashtable = tweens[i];
            GameObject obj2 = (GameObject)hashtable["target"];
            list.Insert(list.Count, obj2);
        }
        for (int j = 0; j < list.Count; j++)
        {
            Pause((GameObject)list[j], type);
        }
    }

    public static void Pause(GameObject target)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if (tween.delay > 0f)
            {
                tween.delay -= Time.time - tween.delayStarted;
                tween.StopCoroutine("TweenDelay");
            }
            tween.isPaused = true;
            tween.enabled = false;
        }
    }

    public static void Pause(GameObject target, bool includechildren)
    {
        Pause(target);
        if (includechildren)
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    Pause(current.gameObject, true);
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
    }

    public static void Pause(GameObject target, string type)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
            {
                if (tween.delay > 0f)
                {
                    tween.delay -= Time.time - tween.delayStarted;
                    tween.StopCoroutine("TweenDelay");
                }
                tween.isPaused = true;
                tween.enabled = false;
            }
        }
    }

    public static void Pause(GameObject target, string type, bool includechildren)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
            {
                if (tween.delay > 0f)
                {
                    tween.delay -= Time.time - tween.delayStarted;
                    tween.StopCoroutine("TweenDelay");
                }
                tween.isPaused = true;
                tween.enabled = false;
            }
        }
        if (includechildren)
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    if (current != null)
                        Pause(current.gameObject, type, true);
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
    }

    public static Vector3 PointOnPath(Transform[] path, float percent)
    {
        Vector3[] vectorArray = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            vectorArray[i] = path[i].position;
        }
        return Interp(PathControlPointGenerator(vectorArray), percent);
    }

    public static Vector3 PointOnPath(Vector3[] path, float percent)
    {
        return Interp(PathControlPointGenerator(path), percent);
    }

    private float punch(float amplitude, float value)
    {
        float num = 9f;
        if (value == 0f)
        {
            return 0f;
        }
        if (value == 1f)
        {
            return 0f;
        }
        float num2 = 0.3f;
        num = (num2 / 6.283185f) * Mathf.Asin(0f);
        return ((amplitude * Mathf.Pow(2f, -10f * value)) * Mathf.Sin((((value * 1f) - num) * 6.283185f) / num2));
    }

    public static void PunchPosition(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "punch";
        args["method"] = "position";
        args["easetype"] = EaseType.punch;
        Launch(target, args);
    }

    public static void PunchPosition(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        PunchPosition(target, Hash(args));
    }

    public static void PunchRotation(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "punch";
        args["method"] = "rotation";
        args["easetype"] = EaseType.punch;
        Launch(target, args);
    }

    public static void PunchRotation(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        PunchRotation(target, Hash(args));
    }

    public static void PunchScale(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "punch";
        args["method"] = "scale";
        args["easetype"] = EaseType.punch;
        Launch(target, args);
    }

    public static void PunchScale(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        PunchScale(target, Hash(args));
    }

    public static void PutOnPath(GameObject target, Transform[] path, float percent)
    {
        Vector3[] vectorArray = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            vectorArray[i] = path[i].position;
        }
        target.transform.position = Interp(PathControlPointGenerator(vectorArray), percent);
    }

    public static void PutOnPath(GameObject target, Vector3[] path, float percent)
    {
        target.transform.position = Interp(PathControlPointGenerator(path), percent);
    }

    public static void PutOnPath(Transform target, Transform[] path, float percent)
    {
        Vector3[] vectorArray = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            vectorArray[i] = path[i].position;
        }
        target.position = Interp(PathControlPointGenerator(vectorArray), percent);
    }

    public static void PutOnPath(Transform target, Vector3[] path, float percent)
    {
        target.position = Interp(PathControlPointGenerator(path), percent);
    }

    public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
    {
        return new Rect(FloatUpdate(currentValue.x, targetValue.x, speed), FloatUpdate(currentValue.y, targetValue.y, speed), FloatUpdate(currentValue.width, targetValue.width, speed), FloatUpdate(currentValue.height, targetValue.height, speed));
    }

    public static void Resume()
    {
        for (int i = 0; i < tweens.Count; i++)
        {
            Hashtable hashtable = tweens[i];
            GameObject target = (GameObject)hashtable["target"];
            Resume(target);
        }
    }

    public static void Resume(string type)
    {
        ArrayList list = new ArrayList();
        for (int i = 0; i < tweens.Count; i++)
        {
            Hashtable hashtable = tweens[i];
            GameObject obj2 = (GameObject)hashtable["target"];
            list.Insert(list.Count, obj2);
        }
        for (int j = 0; j < list.Count; j++)
        {
            Resume((GameObject)list[j], type);
        }
    }

    public static void Resume(GameObject target)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            tween.enabled = true;
        }
    }

    public static void Resume(GameObject target, bool includechildren)
    {
        Resume(target);
        if (includechildren)
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    if (current != null)
                        Resume(current.gameObject, true);
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
    }

    public static void Resume(GameObject target, string type)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
            {
                tween.enabled = true;
            }
        }
    }

    public static void Resume(GameObject target, string type, bool includechildren)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
            {
                tween.enabled = true;
            }
        }
        if (includechildren)
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    if (current != null)
                        Resume(current.gameObject, type, true);
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
    }

    private void ResumeDelay()
    {
        base.StartCoroutine("TweenDelay");
    }

    private void RetrieveArgs()
    {
        foreach (Hashtable hashtable in tweens)
        {
            if (((GameObject)hashtable["target"]) == base.gameObject)
            {
                this.tweenArguments = hashtable;
                break;
            }
        }
        this.id = (string)this.tweenArguments["id"];
        this.type = (string)this.tweenArguments["type"];
        this._name = (string)this.tweenArguments["thisName"];
        this.method = (string)this.tweenArguments["method"];
        if (this.tweenArguments.Contains("time"))
        {
            this.time = (float)this.tweenArguments["time"];
        }
        else
        {
            this.time = Defaults.time;
        }
        if (base.GetComponent<Rigidbody>() != null)
        {
            this.physics = true;
        }
        if (this.tweenArguments.Contains("delay"))
        {
            this.delay = (float)this.tweenArguments["delay"];
        }
        else
        {
            this.delay = Defaults.delay;
        }
        if (this.tweenArguments.Contains("namedcolorvalue"))
        {
            if (this.tweenArguments["namedcolorvalue"].GetType() == typeof(NamedValueColor))
            {
                this.namedcolorvalue = (NamedValueColor)((int)this.tweenArguments["namedcolorvalue"]);
            }
            else
            {
                try
                {
                    this.namedcolorvalue = (NamedValueColor)((int)Enum.Parse(typeof(NamedValueColor), (string)this.tweenArguments["namedcolorvalue"], true));
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("iTween: Unsupported namedcolorvalue supplied! Default will be used.");
                    this.namedcolorvalue = NamedValueColor._Color;
                }
            }
        }
        else
        {
            this.namedcolorvalue = Defaults.namedColorValue;
        }
        if (this.tweenArguments.Contains("looptype"))
        {
            if (this.tweenArguments["looptype"].GetType() == typeof(LoopType))
            {
                this.loopType = (LoopType)((int)this.tweenArguments["looptype"]);
            }
            else
            {
                try
                {
                    this.loopType = (LoopType)((int)Enum.Parse(typeof(LoopType), (string)this.tweenArguments["looptype"], true));
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("iTween: Unsupported loopType supplied! Default will be used.");
                    this.loopType = LoopType.none;
                }
            }
        }
        else
        {
            this.loopType = LoopType.none;
        }
        if (this.tweenArguments.Contains("easetype"))
        {
            if (this.tweenArguments["easetype"].GetType() == typeof(EaseType))
            {
                this.easeType = (EaseType)((int)this.tweenArguments["easetype"]);
            }
            else
            {
                try
                {
                    this.easeType = (EaseType)((int)Enum.Parse(typeof(EaseType), (string)this.tweenArguments["easetype"], true));
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("iTween: Unsupported easeType supplied! Default will be used.");
                    this.easeType = Defaults.easeType;
                }
            }
        }
        else
        {
            this.easeType = Defaults.easeType;
        }
        if (this.tweenArguments.Contains("space"))
        {
            if (this.tweenArguments["space"].GetType() == typeof(Space))
            {
                this.space = (Space)((int)this.tweenArguments["space"]);
            }
            else
            {
                try
                {
                    this.space = (Space)((int)Enum.Parse(typeof(Space), (string)this.tweenArguments["space"], true));
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("iTween: Unsupported space supplied! Default will be used.");
                    this.space = Defaults.space;
                }
            }
        }
        else
        {
            this.space = Defaults.space;
        }
        if (this.tweenArguments.Contains("islocal"))
        {
            this.isLocal = (bool)this.tweenArguments["islocal"];
        }
        else
        {
            this.isLocal = Defaults.isLocal;
        }
        if (this.tweenArguments.Contains("ignoretimescale"))
        {
            this.useRealTime = (bool)this.tweenArguments["ignoretimescale"];
        }
        else
        {
            this.useRealTime = Defaults.useRealTime;
        }
        this.GetEasingFunction();
    }

    public static void RotateAdd(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "rotate";
        args["method"] = "add";
        Launch(target, args);
    }

    public static void RotateAdd(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        RotateAdd(target, Hash(args));
    }

    public static void RotateBy(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "rotate";
        args["method"] = "by";
        Launch(target, args);
    }

    public static void RotateBy(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        RotateBy(target, Hash(args));
    }

    public static void RotateFrom(GameObject target, Hashtable args)
    {
        bool isLocal;
        Vector3 eulerAngles;
        Vector3 vector2;
        args = CleanArgs(args);
        if (args.Contains("islocal"))
        {
            isLocal = (bool)args["islocal"];
        }
        else
        {
            isLocal = Defaults.isLocal;
        }
        if (isLocal)
        {
            vector2 = eulerAngles = target.transform.localEulerAngles;
        }
        else
        {
            vector2 = eulerAngles = target.transform.eulerAngles;
        }
        if (args.Contains("rotation"))
        {
            if (args["rotation"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["rotation"];
                eulerAngles = transform.eulerAngles;
            }
            else if (args["rotation"].GetType() == typeof(Vector3))
            {
                eulerAngles = (Vector3)args["rotation"];
            }
        }
        else
        {
            if (args.Contains("x"))
            {
                eulerAngles.x = (float)args["x"];
            }
            if (args.Contains("y"))
            {
                eulerAngles.y = (float)args["y"];
            }
            if (args.Contains("z"))
            {
                eulerAngles.z = (float)args["z"];
            }
        }
        if (isLocal)
        {
            target.transform.localEulerAngles = eulerAngles;
        }
        else
        {
            target.transform.eulerAngles = eulerAngles;
        }
        args["rotation"] = vector2;
        args["type"] = "rotate";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void RotateFrom(GameObject target, Vector3 rotation, float time)
    {
        object[] args = new object[] { "rotation", rotation, "time", time };
        RotateFrom(target, Hash(args));
    }

    public static void RotateTo(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        if (args.Contains("rotation") && (args["rotation"].GetType() == typeof(Transform)))
        {
            Transform transform = (Transform)args["rotation"];
            args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        args["type"] = "rotate";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void RotateTo(GameObject target, Vector3 rotation, float time)
    {
        object[] args = new object[] { "rotation", rotation, "time", time };
        RotateTo(target, Hash(args));
    }

    public static void RotateUpdate(GameObject target, Hashtable args)
    {
        float updateTime;
        bool isLocal;
        CleanArgs(args);
        Vector3[] vectorArray = new Vector3[4];
        Vector3 eulerAngles = target.transform.eulerAngles;
        if (args.Contains("time"))
        {
            updateTime = (float)args["time"];
            updateTime *= Defaults.updateTimePercentage;
        }
        else
        {
            updateTime = Defaults.updateTime;
        }
        if (args.Contains("islocal"))
        {
            isLocal = (bool)args["islocal"];
        }
        else
        {
            isLocal = Defaults.isLocal;
        }
        if (isLocal)
        {
            vectorArray[0] = target.transform.localEulerAngles;
        }
        else
        {
            vectorArray[0] = target.transform.eulerAngles;
        }
        if (args.Contains("rotation"))
        {
            if (args["rotation"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["rotation"];
                vectorArray[1] = transform.eulerAngles;
            }
            else if (args["rotation"].GetType() == typeof(Vector3))
            {
                vectorArray[1] = (Vector3)args["rotation"];
            }
        }
        vectorArray[3].x = Mathf.SmoothDampAngle(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
        vectorArray[3].y = Mathf.SmoothDampAngle(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
        vectorArray[3].z = Mathf.SmoothDampAngle(vectorArray[0].z, vectorArray[1].z, ref vectorArray[2].z, updateTime);
        if (isLocal)
        {
            target.transform.localEulerAngles = vectorArray[3];
        }
        else
        {
            target.transform.eulerAngles = vectorArray[3];
        }
        if (target.GetComponent<Rigidbody>() != null)
        {
            Vector3 euler = target.transform.eulerAngles;
            target.transform.eulerAngles = eulerAngles;
            target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(euler));
        }
    }

    public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
    {
        object[] args = new object[] { "rotation", rotation, "time", time };
        RotateUpdate(target, Hash(args));
    }

    public static void ScaleAdd(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "scale";
        args["method"] = "add";
        Launch(target, args);
    }

    public static void ScaleAdd(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        ScaleAdd(target, Hash(args));
    }

    public static void ScaleBy(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "scale";
        args["method"] = "by";
        Launch(target, args);
    }

    public static void ScaleBy(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        ScaleBy(target, Hash(args));
    }

    public static void ScaleFrom(GameObject target, Hashtable args)
    {
        Vector3 localScale;
        args = CleanArgs(args);
        Vector3 vector2 = localScale = target.transform.localScale;
        if (args.Contains("scale"))
        {
            if (args["scale"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["scale"];
                localScale = transform.localScale;
            }
            else if (args["scale"].GetType() == typeof(Vector3))
            {
                localScale = (Vector3)args["scale"];
            }
        }
        else
        {
            if (args.Contains("x"))
            {
                localScale.x = (float)args["x"];
            }
            if (args.Contains("y"))
            {
                localScale.y = (float)args["y"];
            }
            if (args.Contains("z"))
            {
                localScale.z = (float)args["z"];
            }
        }
        target.transform.localScale = localScale;
        args["scale"] = vector2;
        args["type"] = "scale";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void ScaleFrom(GameObject target, Vector3 scale, float time)
    {
        object[] args = new object[] { "scale", scale, "time", time };
        ScaleFrom(target, Hash(args));
    }

    public static void ScaleTo(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        if (args.Contains("scale") && (args["scale"].GetType() == typeof(Transform)))
        {
            Transform transform = (Transform)args["scale"];
            args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        args["type"] = "scale";
        args["method"] = "to";
        Launch(target, args);
    }

    public static void ScaleTo(GameObject target, Vector3 scale, float time)
    {
        object[] args = new object[] { "scale", scale, "time", time };
        ScaleTo(target, Hash(args));
    }

    public static void ScaleUpdate(GameObject target, Hashtable args)
    {
        float updateTime;
        CleanArgs(args);
        Vector3[] vectorArray = new Vector3[4];
        if (args.Contains("time"))
        {
            updateTime = (float)args["time"];
            updateTime *= Defaults.updateTimePercentage;
        }
        else
        {
            updateTime = Defaults.updateTime;
        }
        vectorArray[0] = vectorArray[1] = target.transform.localScale;
        if (args.Contains("scale"))
        {
            if (args["scale"].GetType() == typeof(Transform))
            {
                Transform transform = (Transform)args["scale"];
                vectorArray[1] = transform.localScale;
            }
            else if (args["scale"].GetType() == typeof(Vector3))
            {
                vectorArray[1] = (Vector3)args["scale"];
            }
        }
        else
        {
            if (args.Contains("x"))
            {
                vectorArray[1].x = (float)args["x"];
            }
            if (args.Contains("y"))
            {
                vectorArray[1].y = (float)args["y"];
            }
            if (args.Contains("z"))
            {
                vectorArray[1].z = (float)args["z"];
            }
        }
        vectorArray[3].x = Mathf.SmoothDamp(vectorArray[0].x, vectorArray[1].x, ref vectorArray[2].x, updateTime);
        vectorArray[3].y = Mathf.SmoothDamp(vectorArray[0].y, vectorArray[1].y, ref vectorArray[2].y, updateTime);
        vectorArray[3].z = Mathf.SmoothDamp(vectorArray[0].z, vectorArray[1].z, ref vectorArray[2].z, updateTime);
        target.transform.localScale = vectorArray[3];
    }

    public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
    {
        object[] args = new object[] { "scale", scale, "time", time };
        ScaleUpdate(target, Hash(args));
    }

    public static void ShakePosition(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "shake";
        args["method"] = "position";
        Launch(target, args);
    }

    public static void ShakePosition(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        ShakePosition(target, Hash(args));
    }

    public static void ShakeRotation(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "shake";
        args["method"] = "rotation";
        Launch(target, args);
    }

    public static void ShakeRotation(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        ShakeRotation(target, Hash(args));
    }

    public static void ShakeScale(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "shake";
        args["method"] = "scale";
        Launch(target, args);
    }

    public static void ShakeScale(GameObject target, Vector3 amount, float time)
    {
        object[] args = new object[] { "amount", amount, "time", time };
        ShakeScale(target, Hash(args));
    }

    private float spring(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = ((Mathf.Sin((value * 3.141593f) * (0.2f + (((2.5f * value) * value) * value))) * Mathf.Pow(1f - value, 2.2f)) + value) * (1f + (1.2f * (1f - value)));
        return (start + ((end - start) * value));
    }

    public static void Stab(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        args["type"] = "stab";
        Launch(target, args);
    }

    public static void Stab(GameObject target, AudioClip audioclip, float delay)
    {
        object[] args = new object[] { "audioclip", audioclip, "delay", delay };
        Stab(target, Hash(args));
    }

    public static void Stop()
    {
        for (int i = 0; i < tweens.Count; i++)
        {
            Hashtable hashtable = tweens[i];
            GameObject target = (GameObject)hashtable["target"];
            Stop(target);
        }
        tweens.Clear();
    }

    public static void Stop(string type)
    {
        ArrayList list = new ArrayList();
        for (int i = 0; i < tweens.Count; i++)
        {
            Hashtable hashtable = tweens[i];
            GameObject obj2 = (GameObject)hashtable["target"];
            list.Insert(list.Count, obj2);
        }
        for (int j = 0; j < list.Count; j++)
        {
            Stop((GameObject)list[j], type);
        }
    }

    public static void Stop(GameObject target)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            tween.Dispose();
        }
    }

    public static void Stop(GameObject target, bool includechildren)
    {
        Stop(target);
        if (includechildren)
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    Stop(current.gameObject, true);
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
    }

    public static void Stop(GameObject target, string type)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
            {
                tween.Dispose();
            }
        }
    }

    public static void Stop(GameObject target, string type, bool includechildren)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if ((tween.type + tween.method).Substring(0, type.Length).ToLower() == type.ToLower())
            {
                tween.Dispose();
            }
        }
        if (includechildren)
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    Stop(current.gameObject, type, true);
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
    }

    public static void StopByName(string name)
    {
        ArrayList list = new ArrayList();
        for (int i = 0; i < tweens.Count; i++)
        {
            Hashtable hashtable = tweens[i];
            GameObject obj2 = (GameObject)hashtable["target"];
            list.Insert(list.Count, obj2);
        }
        for (int j = 0; j < list.Count; j++)
        {
            StopByName((GameObject)list[j], name);
        }
    }

    public static void StopByName(GameObject target, string name)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if (tween._name == name)
            {
                tween.Dispose();
            }
        }
    }

    public static void StopByName(GameObject target, string name, bool includechildren)
    {
        foreach (iTween tween in target.GetComponents<iTween>())
        {
            if (tween._name == name)
            {
                tween.Dispose();
            }
        }
        if (includechildren)
        {
            IEnumerator enumerator = target.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform)enumerator.Current;
                    StopByName(current.gameObject, name, true);
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
    }

    private void TweenComplete()
    {
        this.isRunning = false;
        if (this.percentage > 0.5f)
        {
            this.percentage = 1f;
        }
        else
        {
            this.percentage = 0f;
        }
        this.apply();
        if (this.type == "value")
        {
            this.CallBack("onupdate");
        }
        if (this.loopType == LoopType.none)
        {
            this.Dispose();
        }
        else
        {
            this.TweenLoop();
        }
        this.CallBack("oncomplete");
    }

    private void TweenLoop()
    {
        this.DisableKinematic();
        switch (this.loopType)
        {
            case LoopType.loop:
                this.percentage = 0f;
                this.runningTime = 0f;
                this.apply();
                base.StartCoroutine("TweenRestart");
                break;

            case LoopType.pingPong:
                this.reverse = !this.reverse;
                this.runningTime = 0f;
                base.StartCoroutine("TweenRestart");
                break;
        }
    }

    private void TweenStart()
    {
        this.CallBack("onstart");
        if (!this.loop)
        {
            this.ConflictCheck();
            this.GenerateTargets();
        }
        if (this.type == "stab")
        {
            this.audioSource.PlayOneShot(this.audioSource.clip);
        }
        if ((((this.type == "move") || (this.type == "scale")) || ((this.type == "rotate") || (this.type == "punch"))) || (((this.type == "shake") || (this.type == "curve")) || (this.type == "look")))
        {
            this.EnableKinematic();
        }
        this.isRunning = true;
    }

    private void TweenUpdate()
    {
        this.apply();
        this.CallBack("onupdate");
        this.UpdatePercentage();
    }

    private void Update()
    {
        if (this.isRunning && !this.physics)
        {
            if (!this.reverse)
            {
                if (this.percentage < 1f)
                {
                    this.TweenUpdate();
                }
                else
                {
                    this.TweenComplete();
                }
            }
            else if (this.percentage > 0f)
            {
                this.TweenUpdate();
            }
            else
            {
                this.TweenComplete();
            }
        }
    }

    private void UpdatePercentage()
    {
        if (this.useRealTime)
        {
            this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
        }
        else
        {
            this.runningTime += Time.deltaTime;
        }
        if (this.reverse)
        {
            this.percentage = 1f - (this.runningTime / this.time);
        }
        else
        {
            this.percentage = this.runningTime / this.time;
        }
        this.lastRealTime = Time.realtimeSinceStartup;
    }

    public static void ValueTo(GameObject target, Hashtable args)
    {
        args = CleanArgs(args);
        if ((!args.Contains("onupdate") || !args.Contains("from")) || !args.Contains("to"))
        {
            UnityEngine.Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
        }
        else
        {
            args["type"] = "value";
            if (args["from"].GetType() == typeof(Vector2))
            {
                args["method"] = "vector2";
            }
            else if (args["from"].GetType() == typeof(Vector3))
            {
                args["method"] = "vector3";
            }
            else if (args["from"].GetType() == typeof(Rect))
            {
                args["method"] = "rect";
            }
            else if (args["from"].GetType() == typeof(float))
            {
                args["method"] = "float";
            }
            else if (args["from"].GetType() == typeof(Color))
            {
                args["method"] = "color";
            }
            else
            {
                UnityEngine.Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
                return;
            }
            if (!args.Contains("easetype"))
            {
                args.Add("easetype", EaseType.linear);
            }
            Launch(target, args);
        }
    }

    public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
    {
        Vector2 vector = targetValue - currentValue;
        currentValue += (Vector2)((vector * speed) * Time.deltaTime);
        return currentValue;
    }

    public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
    {
        Vector3 vector = targetValue - currentValue;
        currentValue += (Vector3)((vector * speed) * Time.deltaTime);
        return currentValue;
    }

    private delegate void ApplyTween();

    private class CRSpline
    {
        public Vector3[] pts;

        public CRSpline(params Vector3[] pts)
        {
            this.pts = new Vector3[pts.Length];
            Array.Copy(pts, this.pts, pts.Length);
        }

        public Vector3 Interp(float t)
        {
            int num = this.pts.Length - 3;
            int index = Mathf.Min(Mathf.FloorToInt(t * num), num - 1);
            float num3 = (t * num) - index;
            Vector3 vector = this.pts[index];
            Vector3 vector2 = this.pts[index + 1];
            Vector3 vector3 = this.pts[index + 2];
            Vector3 vector4 = this.pts[index + 3];
            return (Vector3)(0.5f * (((((((-vector + (3f * vector2)) - (3f * vector3)) + vector4) * ((num3 * num3) * num3)) + (((((2f * vector) - (5f * vector2)) + (4f * vector3)) - vector4) * (num3 * num3))) + ((-vector + vector3) * num3)) + (2f * vector2)));
        }
    }

    public static class Defaults
    {
        public static int cameraFadeDepth = 0xf423f;
        public static Color color = Color.white;
        public static float delay = 0f;
        public static iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
        public static bool isLocal = false;
        public static float lookAhead = 0.05f;
        public static float lookSpeed = 3f;
        public static iTween.LoopType loopType = iTween.LoopType.none;
        public static iTween.NamedValueColor namedColorValue = iTween.NamedValueColor._Color;
        public static bool orientToPath = false;
        public static Space space = Space.Self;
        public static float time = 1f;
        public static Vector3 up = Vector3.up;
        public static float updateTime = (1f * updateTimePercentage);
        public static float updateTimePercentage = 0.05f;
        public static bool useRealTime = false;
    }

    public enum EaseType
    {
        easeInQuad,
        easeOutQuad,
        easeInOutQuad,
        easeInCubic,
        easeOutCubic,
        easeInOutCubic,
        easeInQuart,
        easeOutQuart,
        easeInOutQuart,
        easeInQuint,
        easeOutQuint,
        easeInOutQuint,
        easeInSine,
        easeOutSine,
        easeInOutSine,
        easeInExpo,
        easeOutExpo,
        easeInOutExpo,
        easeInCirc,
        easeOutCirc,
        easeInOutCirc,
        linear,
        spring,
        easeInBounce,
        easeOutBounce,
        easeInOutBounce,
        easeInBack,
        easeOutBack,
        easeInOutBack,
        easeInElastic,
        easeOutElastic,
        easeInOutElastic,
        punch
    }

    private delegate float EasingFunction(float start, float end, float Value);

    public enum LoopType
    {
        none,
        loop,
        pingPong
    }

    public enum NamedValueColor
    {
        _Color,
        _SpecColor,
        _Emission,
        _ReflectColor
    }
}