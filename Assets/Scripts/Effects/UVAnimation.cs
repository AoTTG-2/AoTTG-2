using System;
using System.IO;
using UnityEngine;

public class UVAnimation
{
    public int curFrame;
    public Vector2[] frames;
    public int loopCycles;
    public bool loopReverse;
    public string name;
    protected int numLoops;
    protected int stepDir = 1;
    public Vector2[] UVDimensions;

    public void BuildFromFile(string path, int index, float uvTime, Texture mainTex)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("wrong ean file path!");
        }
        else
        {
            FileStream input = new FileStream(path, FileMode.Open);
            BinaryReader br = new BinaryReader(input);
            EanFile file = new EanFile();
            file.Load(br, input);
            input.Close();
            EanAnimation animation = file.Anims[index];
            this.frames = new Vector2[animation.TotalCount];
            this.UVDimensions = new Vector2[animation.TotalCount];
            int tileCount = animation.TileCount;
            int num2 = ((animation.TotalCount + tileCount) - 1) / tileCount;
            int num3 = 0;
            int width = mainTex.width;
            int height = mainTex.height;
            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j < tileCount; j++)
                {
                    if (num3 >= animation.TotalCount)
                    {
                        break;
                    }
                    Vector2 zero = Vector2.zero;
                    zero.x = ((float) animation.Frames[num3].Width) / ((float) width);
                    zero.y = ((float) animation.Frames[num3].Height) / ((float) height);
                    this.frames[num3].x = ((float) animation.Frames[num3].X) / ((float) width);
                    this.frames[num3].y = 1f - (((float) animation.Frames[num3].Y) / ((float) height));
                    this.UVDimensions[num3] = zero;
                    this.UVDimensions[num3].y = -this.UVDimensions[num3].y;
                    num3++;
                }
            }
        }
    }

    public Vector2[] BuildUVAnim(Vector2 start, Vector2 cellSize, int cols, int rows, int totalCells)
    {
        int index = 0;
        this.frames = new Vector2[totalCells];
        this.UVDimensions = new Vector2[totalCells];
        this.frames[0] = start;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (index >= totalCells)
                {
                    break;
                }
                this.frames[index].x = start.x + (cellSize.x * j);
                this.frames[index].y = start.y - (cellSize.y * i);
                this.UVDimensions[index] = cellSize;
                this.UVDimensions[index].y = -this.UVDimensions[index].y;
                index++;
            }
        }
        return this.frames;
    }

    public bool GetNextFrame(ref Vector2 uv, ref Vector2 dm)
    {
        if (((this.curFrame + this.stepDir) < this.frames.Length) && ((this.curFrame + this.stepDir) >= 0))
        {
            this.curFrame += this.stepDir;
            uv = this.frames[this.curFrame];
            dm = this.UVDimensions[this.curFrame];
        }
        else if ((this.stepDir > 0) && this.loopReverse)
        {
            this.stepDir = -1;
            this.curFrame += this.stepDir;
            uv = this.frames[this.curFrame];
            dm = this.UVDimensions[this.curFrame];
        }
        else
        {
            if (((this.numLoops + 1) > this.loopCycles) && (this.loopCycles != -1))
            {
                return false;
            }
            this.numLoops++;
            if (this.loopReverse)
            {
                this.stepDir *= -1;
                this.curFrame += this.stepDir;
            }
            else
            {
                this.curFrame = 0;
            }
            uv = this.frames[this.curFrame];
            dm = this.UVDimensions[this.curFrame];
        }
        return true;
    }

    public void PlayInReverse()
    {
        this.stepDir = -1;
        this.curFrame = this.frames.Length - 1;
    }

    public void Reset()
    {
        this.curFrame = 0;
        this.stepDir = 1;
        this.numLoops = 0;
    }

    public void SetAnim(Vector2[] anim)
    {
        this.frames = anim;
    }
}

