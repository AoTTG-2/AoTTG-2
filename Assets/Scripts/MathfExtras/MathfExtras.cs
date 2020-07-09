using UnityEngine;

public static class MathfExtras
{
    public static float AngleDifference(float a, float b)
    {
        var diff = (b - a + 180f) % 360f - 180f;
        return diff < -180f ? diff + 360f : diff;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360f);
        
        if (min == 0f && max != 0f && Mathf.Repeat(max, 360f) == 0f)
            return angle;

        min = Mathf.Repeat(min, 360f);
        max = Mathf.Repeat(max, 360f);

        var relMin = AngleDifference(angle, min);
        var relMax = AngleDifference(angle, max);

        if (relMin < 0f && relMax > 0f)
            return angle;

        return Mathf.Abs(relMin) < Mathf.Abs(relMax) ? min : max;
    }
}