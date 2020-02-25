using UnityEngine;

[ExecuteInEditMode]
public class UIFilledSprite : UISprite
{
    public override UISprite.Type type
    {
        get
        {
            return UISprite.Type.Filled;
        }
    }
}

