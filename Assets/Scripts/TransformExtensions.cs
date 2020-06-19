using UnityEngine;

public static class TransformExtensions
{
    public static bool TryFindChild(this Transform that, string name, out Transform transform)
    {
        transform = that.Find(name);
        return transform;
    }
}
