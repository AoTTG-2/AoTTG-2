using System;
using UnityEngine;

public class Emitter
{
    private float EmitDelayTime;
    private float EmitLoop;
    private float EmitterElapsedTime;
    private bool IsFirstEmit = true;
    private Vector3 LastClientPos = Vector3.zero;
    public EffectLayer Layer;

    public Emitter(EffectLayer owner)
    {
        this.Layer = owner;
        this.EmitLoop = this.Layer.EmitLoop;
        this.LastClientPos = this.Layer.ClientTransform.position;
    }

    protected int EmitByDistance()
    {
        Vector3 vector = this.Layer.ClientTransform.position - this.LastClientPos;
        if (vector.magnitude >= this.Layer.DiffDistance)
        {
            this.LastClientPos = this.Layer.ClientTransform.position;
            return 1;
        }
        return 0;
    }

    protected int EmitByRate()
    {
        int num = UnityEngine.Random.Range(0, 100);
        if ((num >= 0) && (num > this.Layer.ChanceToEmit))
        {
            return 0;
        }
        this.EmitDelayTime += Time.deltaTime;
        if ((this.EmitDelayTime < this.Layer.EmitDelay) && !this.IsFirstEmit)
        {
            return 0;
        }
        this.EmitterElapsedTime += Time.deltaTime;
        if (this.EmitterElapsedTime >= this.Layer.EmitDuration)
        {
            if (this.EmitLoop > 0f)
            {
                this.EmitLoop--;
            }
            this.EmitterElapsedTime = 0f;
            this.EmitDelayTime = 0f;
            this.IsFirstEmit = false;
        }
        if (this.EmitLoop == 0f)
        {
            return 0;
        }
        if (this.Layer.AvailableNodeCount == 0)
        {
            return 0;
        }
        int num2 = ((int) (this.EmitterElapsedTime * this.Layer.EmitRate)) - (this.Layer.ActiveENodes.Length - this.Layer.AvailableNodeCount);
        int availableNodeCount = 0;
        if (num2 > this.Layer.AvailableNodeCount)
        {
            availableNodeCount = this.Layer.AvailableNodeCount;
        }
        else
        {
            availableNodeCount = num2;
        }
        if (availableNodeCount <= 0)
        {
            return 0;
        }
        return availableNodeCount;
    }

    public Vector3 GetEmitRotation(EffectNode node)
    {
        Vector3 zero = Vector3.zero;
        if (this.Layer.EmitType == 2)
        {
            if (!this.Layer.SyncClient)
            {
                return (node.Position - (this.Layer.ClientTransform.position + this.Layer.EmitPoint));
            }
            return (node.Position - this.Layer.EmitPoint);
        }
        if (this.Layer.EmitType == 3)
        {
            Vector3 vector2;
            if (!this.Layer.SyncClient)
            {
                vector2 = node.Position - (this.Layer.ClientTransform.position + this.Layer.EmitPoint);
            }
            else
            {
                vector2 = node.Position - this.Layer.EmitPoint;
            }
            Vector3 toDirection = Vector3.RotateTowards(vector2, this.Layer.CircleDir, (90 - this.Layer.AngleAroundAxis) * Mathf.Deg2Rad, 1f);
            return (Vector3) (Quaternion.FromToRotation(vector2, toDirection) * vector2);
        }
        if (this.Layer.IsRandomDir)
        {
            Quaternion quaternion2 = Quaternion.Euler(0f, 0f, (float) this.Layer.AngleAroundAxis);
            Quaternion quaternion3 = Quaternion.Euler(0f, (float) UnityEngine.Random.Range(0, 360), 0f);
            return (Vector3) (((Quaternion.FromToRotation(Vector3.up, this.Layer.OriVelocityAxis) * quaternion3) * quaternion2) * Vector3.up);
        }
        return this.Layer.OriVelocityAxis;
    }

    public int GetNodes()
    {
        if (this.Layer.IsEmitByDistance)
        {
            return this.EmitByDistance();
        }
        return this.EmitByRate();
    }

    public void Reset()
    {
        this.EmitterElapsedTime = 0f;
        this.EmitDelayTime = 0f;
        this.IsFirstEmit = true;
        this.EmitLoop = this.Layer.EmitLoop;
    }

    public void SetEmitPosition(EffectNode node)
    {
        Vector3 zero = Vector3.zero;
        if (this.Layer.EmitType == 1)
        {
            Vector3 emitPoint = this.Layer.EmitPoint;
            float num = UnityEngine.Random.Range((float) (emitPoint.x - (this.Layer.BoxSize.x / 2f)), (float) (emitPoint.x + (this.Layer.BoxSize.x / 2f)));
            float num2 = UnityEngine.Random.Range((float) (emitPoint.y - (this.Layer.BoxSize.y / 2f)), (float) (emitPoint.y + (this.Layer.BoxSize.y / 2f)));
            float num3 = UnityEngine.Random.Range((float) (emitPoint.z - (this.Layer.BoxSize.z / 2f)), (float) (emitPoint.z + (this.Layer.BoxSize.z / 2f)));
            zero.x = num;
            zero.y = num2;
            zero.z = num3;
            if (!this.Layer.SyncClient)
            {
                zero = this.Layer.ClientTransform.position + zero;
            }
        }
        else if (this.Layer.EmitType == 0)
        {
            zero = this.Layer.EmitPoint;
            if (!this.Layer.SyncClient)
            {
                zero = this.Layer.ClientTransform.position + this.Layer.EmitPoint;
            }
        }
        else if (this.Layer.EmitType == 2)
        {
            zero = this.Layer.EmitPoint;
            if (!this.Layer.SyncClient)
            {
                zero = this.Layer.ClientTransform.position + this.Layer.EmitPoint;
            }
            Vector3 vector3 = (Vector3) (Vector3.up * this.Layer.Radius);
            zero = ((Vector3) (Quaternion.Euler((float) UnityEngine.Random.Range(0, 360), (float) UnityEngine.Random.Range(0, 360), (float) UnityEngine.Random.Range(0, 360)) * vector3)) + zero;
        }
        else if (this.Layer.EmitType == 4)
        {
            Vector3 vector4 = this.Layer.EmitPoint + ((Vector3) ((this.Layer.ClientTransform.localRotation * Vector3.forward) * this.Layer.LineLengthLeft));
            Vector3 vector5 = this.Layer.EmitPoint + ((Vector3) ((this.Layer.ClientTransform.localRotation * Vector3.forward) * this.Layer.LineLengthRight));
            Vector3 vector6 = vector5 - vector4;
            float num4 = ((float) (node.Index + 1)) / ((float) this.Layer.MaxENodes);
            float num5 = vector6.magnitude * num4;
            zero = vector4 + ((Vector3) (vector6.normalized * num5));
            if (!this.Layer.SyncClient)
            {
                zero = this.Layer.ClientTransform.TransformPoint(zero);
            }
        }
        else if (this.Layer.EmitType == 3)
        {
            float num6 = ((float) (node.Index + 1)) / ((float) this.Layer.MaxENodes);
            float y = 360f * num6;
            Vector3 vector7 = (Vector3) (Quaternion.Euler(0f, y, 0f) * (Vector3.right * this.Layer.Radius));
            zero = (Vector3) (Quaternion.FromToRotation(Vector3.up, this.Layer.CircleDir) * vector7);
            if (!this.Layer.SyncClient)
            {
                zero = (this.Layer.ClientTransform.position + zero) + this.Layer.EmitPoint;
            }
            else
            {
                zero += this.Layer.EmitPoint;
            }
        }
        node.SetLocalPosition(zero);
    }
}

