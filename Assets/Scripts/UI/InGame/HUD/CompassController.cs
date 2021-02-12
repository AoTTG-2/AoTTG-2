using UnityEngine;
using UnityEngine.UI;

public class CompassController : MonoBehaviour
{

    public bool compassMode;
    public RawImage compassImage;
    public Transform cam;

    void Update()
    {
        if(compassMode)
        {
            compassImage.uvRect = new Rect (cam.localEulerAngles.y / 360f, 0f, 1f, 1f);
        }
    }
}
