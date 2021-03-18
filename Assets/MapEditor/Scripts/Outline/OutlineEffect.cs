/*
//  Copyright (c) 2015 José Guerreiro. All rights reserved.
//
//  MIT license, see http://www.opensource.org/licenses/mit-license.php
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sub license, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
*/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace OutlineEffect
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class OutlineEffect : MonoBehaviour
    {
        public static OutlineEffect Instance { get; private set; }

        private readonly LinkedSet<Outline> outlines = new LinkedSet<Outline>();

        [Range(1.0f, 6.0f)]
        public float lineThickness = 1.25f;
        [Range(0, 10)]
        public float lineIntensity = .5f;
        [Range(0, 1)]
        public float fillAmount = 0.2f;

        public Color lineColor0 = Color.red;
        public Color lineColor1 = Color.green;
        public Color lineColor2 = Color.blue;
        public Color lineColor3 = Color.cyan;

        public bool additiveRendering = false;

        public bool backfaceCulling = true;

        [Header("These settings can affect performance!")]
        public bool cornerOutlines = false;
        public bool addLinesBetweenColors = false;

        [Header("Advanced settings")]
        public bool scaleWithScreenSize = true;
        [Range(0.1f, .9f)]
        public float alphaCutoff = .5f;
        public bool flipY = false;
        public Camera sourceCamera;
        public bool autoEnableOutlines = true;

        [HideInInspector]
        public Camera outlineCamera;
        Material outline1Material;
        Material outline1MaterialAlpha;
        Material outline2Material;
        Material outline2MaterialAlpha;
        Material outline3Material;
        Material outline3MaterialAlpha;
        Material outline4Material;
        Material outline4MaterialAlpha;
        Material outlineEraseMaterial;

        [SerializeField] Shader outlineShader;
        [SerializeField] Shader outlineBufferShader;
        [HideInInspector] public Material outlineShaderMaterial;
        [HideInInspector] public RenderTexture renderTexture;
        [HideInInspector] public RenderTexture extraRenderTexture;

        CommandBuffer commandBuffer;

        Material GetMaterialFromID(Outline outline)
        {
            if (outline.color == 0)
            {
                if (outline.alphaIsTransparency)
                    return outline1MaterialAlpha;
                else
                    return outline1Material;
            }
            else if (outline.color == 1)
            {
                if (outline.alphaIsTransparency)
                    return outline2MaterialAlpha;
                else
                    return outline2Material;
            }
            else if (outline.color == 2)
            {
                if (outline.alphaIsTransparency)
                    return outline3MaterialAlpha;
                else
                    return outline3Material;
            }
            else if (outline.color == 3)
            {
                if (outline.alphaIsTransparency)
                    return outline4MaterialAlpha;
                else
                    return outline4Material;
            }
            else
                return outline1Material;
        }
        List<Material> materialBuffer = new List<Material>();
        Material CreateMaterial(Color emissionColor, bool alpha)
        {
            Material m = new Material(outlineBufferShader);
            m.SetColor("_Color", emissionColor);

            float alphaf = (alpha) ? 1.0f : 0.0f;
            m.SetFloat("_Alpha", alphaf);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
            return m;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                throw new System.Exception("you can only have one outline camera in the scene");
            }

            Instance = this;
        }

        void Start()
        {
            CreateMaterialsIfNeeded();
            UpdateMaterialsPublicProperties();

            if (sourceCamera == null)
            {
                sourceCamera = GetComponent<Camera>();

                if (sourceCamera == null)
                    sourceCamera = Camera.main;
            }

            if (outlineCamera == null)
            {
                foreach (Camera c in GetComponentsInChildren<Camera>())
                {
                    if (c.name == "Outline Camera")
                    {
                        outlineCamera = c;
                        c.enabled = false;

                        break;
                    }
                }

                if (outlineCamera == null)
                {
                    GameObject cameraGameObject = new GameObject("Outline Camera");
                    cameraGameObject.transform.parent = sourceCamera.transform;
                    outlineCamera = cameraGameObject.AddComponent<Camera>();
                    outlineCamera.enabled = false;
                }
            }

            renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            UpdateOutlineCameraFromSource();

            commandBuffer = new CommandBuffer();
            outlineCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);
        }

        bool RenderTheNextFrame;
        public void OnPreRender()
        {
            if (commandBuffer == null)
                return;

            // the first frame during which there are no outlines, we still need to render 
            // to clear out any outlines that were being rendered on the previous frame
            if (outlines.Count == 0)
            {
                if (!RenderTheNextFrame)
                    return;

                RenderTheNextFrame = false;
            }
            else
            {
                RenderTheNextFrame = true;
            }

            CreateMaterialsIfNeeded();

            if (renderTexture == null || renderTexture.width != sourceCamera.pixelWidth || renderTexture.height != sourceCamera.pixelHeight)
            {
                renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                outlineCamera.targetTexture = renderTexture;
            }
            UpdateMaterialsPublicProperties();
            UpdateOutlineCameraFromSource();
            outlineCamera.targetTexture = renderTexture;
            commandBuffer.SetRenderTarget(renderTexture);

            commandBuffer.Clear();

            foreach (Outline outline in outlines)
            {
                LayerMask l = sourceCamera.cullingMask;

                if (outline != null && l == (l | (1 << outline.gameObject.layer)))
                {
                    for (int v = 0; v < outline.SharedMaterials.Length; v++)
                    {
                        Material m = null;

                        if (outline.SharedMaterials[v].mainTexture != null && outline.SharedMaterials[v])
                        {
                            foreach (Material g in materialBuffer)
                            {
                                if (g.mainTexture == outline.SharedMaterials[v].mainTexture)
                                {
                                    if (outline.eraseRenderer && g.color == outlineEraseMaterial.color)
                                        m = g;
                                    else if (g.color == GetMaterialFromID(outline).color)
                                        m = g;
                                }
                            }

                            if (m == null)
                            {
                                if (outline.eraseRenderer)
                                    m = new Material(outlineEraseMaterial);
                                else
                                    m = new Material(GetMaterialFromID(outline));
                                m.mainTexture = outline.SharedMaterials[v].mainTexture;
                                materialBuffer.Add(m);
                            }
                        }
                        else
                        {
                            if (outline.eraseRenderer)
                                m = outlineEraseMaterial;
                            else
                                m = GetMaterialFromID(outline);
                        }

                        if (backfaceCulling)
                            m.SetInt("_Culling", (int)UnityEngine.Rendering.CullMode.Back);
                        else
                            m.SetInt("_Culling", (int)UnityEngine.Rendering.CullMode.Off);

                        commandBuffer.DrawRenderer(outline.Renderer, m, 0, 0);
                        MeshFilter mL = outline.MeshFilter;
                        if (mL)
                        {
                            if (mL.sharedMesh != null)
                            {
                                for (int i = 1; i < mL.sharedMesh.subMeshCount; i++)
                                    commandBuffer.DrawRenderer(outline.Renderer, m, i, 0);
                            }
                        }
                        SkinnedMeshRenderer sMR = outline.SkinnedMeshRenderer;
                        if (sMR)
                        {
                            if (sMR.sharedMesh != null)
                            {
                                for (int i = 1; i < sMR.sharedMesh.subMeshCount; i++)
                                    commandBuffer.DrawRenderer(outline.Renderer, m, i, 0);
                            }
                        }
                    }
                }
            }

            outlineCamera.Render();
        }

        private void OnEnable()
        {
            if (autoEnableOutlines)
            {
                Outline[] o = FindObjectsOfType<Outline>();

                foreach (Outline oL in o)
                {
                    oL.enabled = false;
                    oL.enabled = true;
                }
            }
        }

        void OnDestroy()
        {
            if (renderTexture != null)
                renderTexture.Release();
            if (extraRenderTexture != null)
                extraRenderTexture.Release();
            DestroyMaterials();
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (outlineShaderMaterial != null)
            {
                outlineShaderMaterial.SetTexture("_OutlineSource", renderTexture);

                if (addLinesBetweenColors)
                {
                    Graphics.Blit(source, extraRenderTexture, outlineShaderMaterial, 0);
                    outlineShaderMaterial.SetTexture("_OutlineSource", extraRenderTexture);
                }
                Graphics.Blit(source, destination, outlineShaderMaterial, 1);
            }
        }

        private void CreateMaterialsIfNeeded()
        {
            if (outlineShaderMaterial == null)
            {
                outlineShaderMaterial = new Material(outlineShader);
                outlineShaderMaterial.hideFlags = HideFlags.HideAndDontSave;
                UpdateMaterialsPublicProperties();
            }
            if (outlineEraseMaterial == null)
                outlineEraseMaterial = CreateMaterial(new Color(0, 0, 0, 0), false);
            if (outline1Material == null)
                outline1Material = CreateMaterial(new Color(1, 0, 0, 0), false);
            if (outline1MaterialAlpha == null)
                outline1MaterialAlpha = CreateMaterial(new Color(1, 0, 0, 0), true);
            if (outline2Material == null)
                outline2Material = CreateMaterial(new Color(0, 1, 0, 0), false);
            if (outline2MaterialAlpha == null)
                outline2MaterialAlpha = CreateMaterial(new Color(0, 1, 0, 0), true);
            if (outline3Material == null)
                outline3Material = CreateMaterial(new Color(0, 0, 1, 0), false);
            if (outline3MaterialAlpha == null)
                outline3MaterialAlpha = CreateMaterial(new Color(0, 0, 1, 0), true);
            if (outline4Material == null)
                outline4Material = CreateMaterial(new Color(0, 0, 0, 1), false);
            if (outline4MaterialAlpha == null)
                outline4MaterialAlpha = CreateMaterial(new Color(0, 0, 0, 1), true);
        }

        private void DestroyMaterials()
        {
            foreach (Material m in materialBuffer)
                DestroyImmediate(m);
            materialBuffer.Clear();
            DestroyImmediate(outlineShaderMaterial);
            DestroyImmediate(outlineEraseMaterial);
            DestroyImmediate(outline1Material);
            DestroyImmediate(outline2Material);
            DestroyImmediate(outline3Material);
            outlineShader = null;
            outlineBufferShader = null;
            outlineShaderMaterial = null;
            outlineEraseMaterial = null;
            outline1Material = null;
            outline2Material = null;
            outline3Material = null;
            outline4Material = null;
        }

        public void UpdateMaterialsPublicProperties()
        {
            if (outlineShaderMaterial)
            {
                float scalingFactor = 1;
                if (scaleWithScreenSize)
                {
                    // If Screen.height gets bigger, outlines gets thicker
                    scalingFactor = Screen.height / 360.0f;
                }

                // If scaling is too small (height less than 360 pixels), make sure you still render the outlines, but render them with 1 thickness
                if (scaleWithScreenSize && scalingFactor < 1)
                {
                    if (UnityEngine.XR.XRSettings.isDeviceActive && sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
                    {
                        outlineShaderMaterial.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureWidth) * 1000.0f);
                        outlineShaderMaterial.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureHeight) * 1000.0f);
                    }
                    else
                    {
                        outlineShaderMaterial.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
                        outlineShaderMaterial.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
                    }
                }
                else
                {
                    if (UnityEngine.XR.XRSettings.isDeviceActive && sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
                    {
                        outlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * (lineThickness / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureWidth) * 1000.0f);
                        outlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * (lineThickness / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureHeight) * 1000.0f);
                    }
                    else
                    {
                        outlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * (lineThickness / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
                        outlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * (lineThickness / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
                    }
                }
                outlineShaderMaterial.SetFloat("_LineIntensity", lineIntensity);
                outlineShaderMaterial.SetFloat("_FillAmount", fillAmount);
                outlineShaderMaterial.SetColor("_LineColor1", lineColor0 * lineColor0);
                outlineShaderMaterial.SetColor("_LineColor2", lineColor1 * lineColor1);
                outlineShaderMaterial.SetColor("_LineColor3", lineColor2 * lineColor2);
                outlineShaderMaterial.SetColor("_LineColor4", lineColor3 * lineColor3);
                if (flipY)
                    outlineShaderMaterial.SetInt("_FlipY", 1);
                else
                    outlineShaderMaterial.SetInt("_FlipY", 0);
                if (!additiveRendering)
                    outlineShaderMaterial.SetInt("_Dark", 1);
                else
                    outlineShaderMaterial.SetInt("_Dark", 0);
                if (cornerOutlines)
                    outlineShaderMaterial.SetInt("_CornerOutlines", 1);
                else
                    outlineShaderMaterial.SetInt("_CornerOutlines", 0);

                Shader.SetGlobalFloat("_OutlineAlphaCutoff", alphaCutoff);
            }
        }

        void UpdateOutlineCameraFromSource()
        {
            outlineCamera.CopyFrom(sourceCamera);
            outlineCamera.renderingPath = RenderingPath.Forward;
            outlineCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            outlineCamera.clearFlags = CameraClearFlags.SolidColor;
            outlineCamera.rect = new Rect(0, 0, 1, 1);
            outlineCamera.cullingMask = 0;
            outlineCamera.targetTexture = renderTexture;
            outlineCamera.enabled = false;
#if UNITY_5_6_OR_NEWER
            outlineCamera.allowHDR = false;
#else
        outlineCamera.hdr = false;
#endif
        }

        public void AddOutline(Outline outline)
            => outlines.Add(outline);

        public void RemoveOutline(Outline outline)
            => outlines.Remove(outline);
    }
}