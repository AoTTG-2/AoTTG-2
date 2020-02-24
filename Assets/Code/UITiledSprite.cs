using UnityEngine;

[ExecuteInEditMode]
public class UITiledSprite : UISlicedSprite
{
    public override UISprite.Type type
    {
        get
        {
            return UISprite.Type.Tiled;
        }
    }
}

