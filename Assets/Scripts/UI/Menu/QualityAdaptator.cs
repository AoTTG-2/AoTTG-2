using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UCamera = UnityEngine.Camera;

namespace Assets.Scripts.UI.Menu
{
    public partial class MainMenu
    {
        public class QualityAdaptator
        {
            const byte SCALE_FRAME_STEP = 10;
            const byte MAX_CONSECUTIVE_LOW = 10;
            const float TARGET_FPS = 60;
            const float TRIGGER_LOW = .9f;

            int x, y;
            float avgDeltaTime,rescaleFactor = 1f;
            short waitToAvg = 0, consecutiveLow = 0;
            private UCamera blenderCam;
            private Volume postProcess;
            private RenderTexture texture;
            private bool anyFormatSupported = false;
            private RenderTextureFormat supportedFormat;

            private void checkSupportedRenderFormat()
            {
                RenderTextureFormat[] possible_formats = new RenderTextureFormat[]
                {
                RenderTextureFormat.Default, RenderTextureFormat.ARGB32, RenderTextureFormat.BGR101010_XR,
                RenderTextureFormat.DefaultHDR,  RenderTextureFormat.RGB111110Float, RenderTextureFormat.RGB565,
                RenderTextureFormat.ARGBHalf,  RenderTextureFormat.ARGB2101010, RenderTextureFormat.ARGB4444,
                RenderTextureFormat.ARGB1555, RenderTextureFormat.ARGBInt, RenderTextureFormat.ARGBFloat,
                RenderTextureFormat.ARGB64, RenderTextureFormat.BGRA10101010_XR, RenderTextureFormat.BGRA32,
                RenderTextureFormat.RGBAUShort
                };

                foreach (var format in possible_formats)
                {
                    if (SystemInfo.SupportsRenderTextureFormat(format))
                    {
                        this.supportedFormat = format;
                        anyFormatSupported = true;
                        break;
                    }
                }
            }

            public QualityAdaptator(RenderTexture rendered)
            {
                this.waitToAvg = -10;
                this.checkSupportedRenderFormat();
                this.texture = rendered;
            }

            private void recalculatePostRenderEffects()
            {
                if (this.postProcess.profile.TryGet<DepthOfField>(out var depthEffect))
                {
                    depthEffect.focalLength.value = 75 + Mathf.Log(this.texture.height / 1080f, 2) * 20;
                }
            }

            private int getClosestF4Res(float value)
            {
                int size = (int) value;
                while (size % 4 != 0)
                    size++;
                return size;
            }

            private void recalculateSceneRenderer()
            {
                if (anyFormatSupported)
                {
                    this.texture.Release();
                    this.texture.width = this.getClosestF4Res(this.x * this.rescaleFactor);
                    this.texture.height = this.getClosestF4Res(this.y * this.rescaleFactor);
                    this.texture.format = supportedFormat;
                    this.texture.Create();

                    this.recalculatePostRenderEffects();
                }
            }

            public void setCameraResolution()
            {
                this.consecutiveLow = 0;
                if (this.waitToAvg > 0)
                    this.waitToAvg = 0;
                this.x = Screen.width;
                this.y = Screen.height;
                this.recalculateSceneRenderer();
                var aspectRatio = (float) this.texture.width / this.texture.height;
                this.blenderCam.aspect = aspectRatio;

                //super ultra wide monitor ratio goes up to ~3.5 (32/9)
                //and the default considered is the ~1.7 (16/9) standard
                float capped_normalized_ratio = (Mathf.Max(Mathf.Min(aspectRatio, 3.5f), 1.5f) - 1.5f) / 2f;
                this.blenderCam.focalLength = Mathf.Lerp(90, 40, capped_normalized_ratio);

                var framerate = 1 / avgDeltaTime;

#if UNITY_EDITOR
                Debug.Log(framerate.ToString("N0")+" "+this.rescaleFactor+" RESETTING RENDER TO " + this.texture.width + "x" + this.texture.height);
#endif
            }

            public void findComponentReferences()
            {
                this.blenderCam = GameObject.Find("Camera").GetComponent<UCamera>();
                this.postProcess = GameObject.FindObjectOfType<Volume>();
            }

            private bool rescaleNeeded()
            {
                return waitToAvg > SCALE_FRAME_STEP && (1 / this.avgDeltaTime) < TARGET_FPS*TRIGGER_LOW;
            }

            public void checkResolution()
            {
                this.waitToAvg++;
                this.avgDeltaTime = this.avgDeltaTime * .6f + Time.deltaTime * .4f;
                if(x != Screen.width || y!= Screen.height)
                {
                    this.rescaleFactor = 1f;
                    this.setCameraResolution();
                }
                if (this.rescaleNeeded())
                {
                    //It rescale only if the framerate has 3 consecutive frames lower than the target framerate
                    this.consecutiveLow++;
                    if (this.consecutiveLow > MAX_CONSECUTIVE_LOW)
                    {
                        //if scaling to half the normal size (rendering 1/4 the number of pixels) won't help
                        //there's no point in further scaling down
                        if (this.rescaleFactor > .5f)
                        {
                            this.rescaleFactor *= .9f;
                            this.setCameraResolution();
                        }
                    }
                }
                else
                    this.consecutiveLow = 0;
            }

            public void useCamera(bool enable)
            {
                if (this.blenderCam)
                    this.blenderCam.enabled = enable;
            }
        }
    }
}
