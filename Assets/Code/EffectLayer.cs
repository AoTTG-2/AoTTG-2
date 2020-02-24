using System;
using System.Collections;
using UnityEngine;

public class EffectLayer : MonoBehaviour
{
    public EffectNode[] ActiveENodes;
    public bool AlongVelocity;
    public int AngleAroundAxis;
    public bool AttractionAffectorEnable;
    public AnimationCurve AttractionCurve;
    public Vector3 AttractionPosition;
    public float AttractMag = 0.1f;
    public EffectNode[] AvailableENodes;
    public int AvailableNodeCount;
    public Vector3 BoxSize;
    public float ChanceToEmit = 100f;
    public Vector3 CircleDir;
    public Transform ClientTransform;
    public Color Color1 = Color.white;
    public Color Color2;
    public Color Color3;
    public Color Color4;
    public bool ColorAffectorEnable;
    public int ColorAffectType;
    public float ColorGradualTimeLength = 1f;
    public COLOR_GRADUAL_TYPE ColorGradualType;
    public int Cols = 1;
    public float DeltaRot;
    public float DeltaScaleX;
    public float DeltaScaleY;
    public float DiffDistance = 0.1f;
    public int EanIndex;
    public string EanPath = "none";
    public float EmitDelay;
    public float EmitDuration = 10f;
    public int EmitLoop = 1;
    public Vector3 EmitPoint;
    public int EmitRate = 20;
    protected Emitter emitter;
    public int EmitType;
    public bool IsEmitByDistance;
    public bool IsNodeLifeLoop = true;
    public bool IsRandomDir;
    public bool JetAffectorEnable;
    public float JetMax;
    public float JetMin;
    public Vector3 LastClientPos;
    public Vector3 LinearForce;
    public bool LinearForceAffectorEnable;
    public float LinearMagnitude = 1f;
    public float LineLengthLeft = -1f;
    public float LineLengthRight = 1f;
    public int LoopCircles = -1;
    protected Camera MainCamera;
    public UnityEngine.Material Material;
    public int MaxENodes = 1;
    public float MaxFps = 60f;
    public int MaxRibbonElements = 6;
    public float NodeLifeMax = 1f;
    public float NodeLifeMin = 1f;
    public Vector2 OriLowerLeftUV = Vector2.zero;
    public int OriPoint;
    public int OriRotationMax;
    public int OriRotationMin;
    public float OriScaleXMax = 1f;
    public float OriScaleXMin = 1f;
    public float OriScaleYMax = 1f;
    public float OriScaleYMin = 1f;
    public float OriSpeed;
    public Vector2 OriUVDimensions = Vector2.one;
    public Vector3 OriVelocityAxis;
    public float Radius;
    public bool RandomOriRot;
    public bool RandomOriScale;
    public int RenderType;
    public float RibbonLen = 1f;
    public float RibbonWidth = 0.5f;
    public bool RotAffectorEnable;
    public AnimationCurve RotateCurve;
    public RSTYPE RotateType;
    public int Rows = 1;
    public bool ScaleAffectorEnable;
    public RSTYPE ScaleType;
    public AnimationCurve ScaleXCurve;
    public AnimationCurve ScaleYCurve;
    public float SpriteHeight = 1f;
    public int SpriteType;
    public int SpriteUVStretch;
    public float SpriteWidth = 1f;
    public float StartTime;
    public int StretchType;
    public bool SyncClient;
    public float TailDistance;
    public bool UseAttractCurve;
    public bool UseVortexCurve;
    public bool UVAffectorEnable;
    public float UVTime = 30f;
    public int UVType;
    public VertexPool Vertexpool;
    public bool VortexAffectorEnable;
    public AnimationCurve VortexCurve;
    public Vector3 VortexDirection;
    public float VortexMag = 0.1f;

    public void AddActiveNode(EffectNode node)
    {
        if (this.AvailableNodeCount == 0)
        {
            Debug.LogError("out index!");
        }
        if (this.AvailableENodes[node.Index] != null)
        {
            this.ActiveENodes[node.Index] = node;
            this.AvailableENodes[node.Index] = null;
            this.AvailableNodeCount--;
        }
    }

    protected void AddNodes(int num)
    {
        int num2 = 0;
        for (int i = 0; i < this.MaxENodes; i++)
        {
            if (num2 == num)
            {
                break;
            }
            EffectNode node = this.AvailableENodes[i];
            if (node != null)
            {
                this.AddActiveNode(node);
                num2++;
                this.emitter.SetEmitPosition(node);
                float life = 0f;
                if (this.IsNodeLifeLoop)
                {
                    life = -1f;
                }
                else
                {
                    life = UnityEngine.Random.Range(this.NodeLifeMin, this.NodeLifeMax);
                }
                Vector3 emitRotation = this.emitter.GetEmitRotation(node);
                node.Init(emitRotation.normalized, this.OriSpeed, life, UnityEngine.Random.Range(this.OriRotationMin, this.OriRotationMax), UnityEngine.Random.Range(this.OriScaleXMin, this.OriScaleXMax), UnityEngine.Random.Range(this.OriScaleYMin, this.OriScaleYMax), this.Color1, this.OriLowerLeftUV, this.OriUVDimensions);
            }
        }
    }

    public void FixedUpdateCustom()
    {
        int nodes = this.emitter.GetNodes();
        this.AddNodes(nodes);
        for (int i = 0; i < this.MaxENodes; i++)
        {
            EffectNode node = this.ActiveENodes[i];
            if (node != null)
            {
                node.Update();
            }
        }
    }

    public RibbonTrail GetRibbonTrail()
    {
        if ((!((this.ActiveENodes == null) | (this.ActiveENodes.Length != 1)) && (this.MaxENodes == 1)) && (this.RenderType == 1))
        {
            return this.ActiveENodes[0].Ribbon;
        }
        return null;
    }

    public VertexPool GetVertexPool()
    {
        return this.Vertexpool;
    }

    protected void Init()
    {
        this.AvailableENodes = new EffectNode[this.MaxENodes];
        this.ActiveENodes = new EffectNode[this.MaxENodes];
        for (int i = 0; i < this.MaxENodes; i++)
        {
            EffectNode node = new EffectNode(i, this.ClientTransform, this.SyncClient, this);
            ArrayList afts = this.InitAffectors(node);
            node.SetAffectorList(afts);
            if (this.RenderType == 0)
            {
                node.SetType(this.SpriteWidth, this.SpriteHeight, (STYPE) this.SpriteType, (ORIPOINT) this.OriPoint, this.SpriteUVStretch, this.MaxFps);
            }
            else
            {
                node.SetType(this.RibbonWidth, this.MaxRibbonElements, this.RibbonLen, this.ClientTransform.position, this.StretchType, this.MaxFps);
            }
            this.AvailableENodes[i] = node;
        }
        this.AvailableNodeCount = this.MaxENodes;
        this.emitter = new Emitter(this);
    }

    protected ArrayList InitAffectors(EffectNode node)
    {
        ArrayList list = new ArrayList();
        if (this.UVAffectorEnable)
        {
            UVAnimation frame = new UVAnimation();
            Texture mainTex = this.Vertexpool.GetMaterial().GetTexture("_MainTex");
            if (this.UVType == 2)
            {
                frame.BuildFromFile(this.EanPath, this.EanIndex, this.UVTime, mainTex);
                this.OriLowerLeftUV = frame.frames[0];
                this.OriUVDimensions = frame.UVDimensions[0];
            }
            else if (this.UVType == 1)
            {
                float num = mainTex.width / this.Cols;
                float num2 = mainTex.height / this.Rows;
                Vector2 cellSize = new Vector2(num / ((float) mainTex.width), num2 / ((float) mainTex.height));
                Vector2 start = new Vector2(0f, 1f);
                frame.BuildUVAnim(start, cellSize, this.Cols, this.Rows, this.Cols * this.Rows);
                this.OriLowerLeftUV = start;
                this.OriUVDimensions = cellSize;
                this.OriUVDimensions.y = -this.OriUVDimensions.y;
            }
            if (frame.frames.Length == 1)
            {
                this.OriLowerLeftUV = frame.frames[0];
                this.OriUVDimensions = frame.UVDimensions[0];
            }
            else
            {
                frame.loopCycles = this.LoopCircles;
                Affector affector = new UVAffector(frame, this.UVTime, node);
                list.Add(affector);
            }
        }
        if (this.RotAffectorEnable && (this.RotateType != RSTYPE.NONE))
        {
            Affector affector2;
            if (this.RotateType == RSTYPE.CURVE)
            {
                affector2 = new RotateAffector(this.RotateCurve, node);
            }
            else
            {
                affector2 = new RotateAffector(this.DeltaRot, node);
            }
            list.Add(affector2);
        }
        if (this.ScaleAffectorEnable && (this.ScaleType != RSTYPE.NONE))
        {
            Affector affector3;
            if (this.ScaleType == RSTYPE.CURVE)
            {
                affector3 = new ScaleAffector(this.ScaleXCurve, this.ScaleYCurve, node);
            }
            else
            {
                affector3 = new ScaleAffector(this.DeltaScaleX, this.DeltaScaleY, node);
            }
            list.Add(affector3);
        }
        if (this.ColorAffectorEnable && (this.ColorAffectType != 0))
        {
            ColorAffector affector4;
            if (this.ColorAffectType == 2)
            {
                Color[] colorArr = new Color[] { this.Color1, this.Color2, this.Color3, this.Color4 };
                affector4 = new ColorAffector(colorArr, this.ColorGradualTimeLength, this.ColorGradualType, node);
            }
            else
            {
                Color[] colorArray2 = new Color[] { this.Color1, this.Color2 };
                affector4 = new ColorAffector(colorArray2, this.ColorGradualTimeLength, this.ColorGradualType, node);
            }
            list.Add(affector4);
        }
        if (this.LinearForceAffectorEnable)
        {
            Affector affector5 = new LinearForceAffector((Vector3) (this.LinearForce.normalized * this.LinearMagnitude), node);
            list.Add(affector5);
        }
        if (this.JetAffectorEnable)
        {
            Affector affector6 = new JetAffector(this.JetMin, this.JetMax, node);
            list.Add(affector6);
        }
        if (this.VortexAffectorEnable)
        {
            Affector affector7;
            if (this.UseVortexCurve)
            {
                affector7 = new VortexAffector(this.VortexCurve, this.VortexDirection, node);
            }
            else
            {
                affector7 = new VortexAffector(this.VortexMag, this.VortexDirection, node);
            }
            list.Add(affector7);
        }
        if (this.AttractionAffectorEnable)
        {
            Affector affector8;
            if (this.UseVortexCurve)
            {
                affector8 = new AttractionForceAffector(this.AttractionCurve, this.AttractionPosition, node);
            }
            else
            {
                affector8 = new AttractionForceAffector(this.AttractMag, this.AttractionPosition, node);
            }
            list.Add(affector8);
        }
        return list;
    }

    private void OnDrawGizmosSelected()
    {
    }

    public void RemoveActiveNode(EffectNode node)
    {
        if (this.AvailableNodeCount == this.MaxENodes)
        {
            Debug.LogError("out index!");
        }
        if (this.ActiveENodes[node.Index] != null)
        {
            this.ActiveENodes[node.Index] = null;
            this.AvailableENodes[node.Index] = node;
            this.AvailableNodeCount++;
        }
    }

    public void Reset()
    {
        for (int i = 0; i < this.MaxENodes; i++)
        {
            if (this.ActiveENodes == null)
            {
                return;
            }
            EffectNode node = this.ActiveENodes[i];
            if (node != null)
            {
                node.Reset();
                this.RemoveActiveNode(node);
            }
        }
        this.emitter.Reset();
    }

    public void StartCustom()
    {
        if (this.MainCamera == null)
        {
            this.MainCamera = Camera.main;
        }
        this.Init();
        this.LastClientPos = this.ClientTransform.position;
    }
}

