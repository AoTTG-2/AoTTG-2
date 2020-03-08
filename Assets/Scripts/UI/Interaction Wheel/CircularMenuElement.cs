using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CircularMenuElement
{
    public string Name;
    public Image SceneImage;
    public Color NormalColor = new Color(1f, 1f, 1f, 0.5f);
    public Color HighLightedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    public Color PressedColor = Color.gray;
}