using UnityEngine;

[ExecuteInEditMode]
public class UISlicedSprite : UISprite
{
    public override UISprite.Type type
    {
        get
        {
            return UISprite.Type.Sliced;
        }
    }
}

