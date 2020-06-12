using UnityEngine;

public class Hair : MonoBehaviour
{
    private Material hairMat;

    private void Awake()
    {
        hairMat = gameObject.GetComponent<MeshRenderer>().material;
    }

    public Color HairColor
    {
        get { return hairMat.color; }
        set { hairMat.color = value; }
    }
}
