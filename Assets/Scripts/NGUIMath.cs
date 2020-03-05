using System;
using UnityEngine;

public static class NGUIMath
{

    public static Vector3[] CalculateWidgetCorners(UIWidget w)
    {
        Vector2 relativeSize = w.relativeSize;
        Vector2 pivotOffset = w.pivotOffset;
        Vector4 relativePadding = w.relativePadding;
        float x = (pivotOffset.x * relativeSize.x) - relativePadding.x;
        float y = (pivotOffset.y * relativeSize.y) + relativePadding.y;
        float num3 = ((x + relativeSize.x) + relativePadding.x) + relativePadding.z;
        float num4 = ((y - relativeSize.y) - relativePadding.y) - relativePadding.w;
        Transform cachedTransform = w.cachedTransform;
        return new Vector3[] { cachedTransform.TransformPoint(x, y, 0f), cachedTransform.TransformPoint(x, num4, 0f), cachedTransform.TransformPoint(num3, num4, 0f), cachedTransform.TransformPoint(num3, y, 0f) };
    }

    public static Rect ConvertToPixels(Rect rect, int width, int height, bool round)
    {
        Rect rect2 = rect;
        if (round)
        {
            rect2.xMin = Mathf.RoundToInt(rect.xMin * width);
            rect2.xMax = Mathf.RoundToInt(rect.xMax * width);
            rect2.yMin = Mathf.RoundToInt((1f - rect.yMax) * height);
            rect2.yMax = Mathf.RoundToInt((1f - rect.yMin) * height);
            return rect2;
        }
        rect2.xMin = rect.xMin * width;
        rect2.xMax = rect.xMax * width;
        rect2.yMin = (1f - rect.yMax) * height;
        rect2.yMax = (1f - rect.yMin) * height;
        return rect2;
    }

    public static Rect ConvertToTexCoords(Rect rect, int width, int height)
    {
        Rect rect2 = rect;
        if ((width != 0f) && (height != 0f))
        {
            rect2.xMin = rect.xMin / ((float)width);
            rect2.xMax = rect.xMax / ((float)width);
            rect2.yMin = 1f - (rect.yMax / ((float)height));
            rect2.yMax = 1f - (rect.yMin / ((float)height));
        }
        return rect2;
    }
}

