using UnityEngine;

public class Hair : MonoBehaviour
{
    [SerializeField] private int id;
    private Material hairMat;

    public int ID { get { return id; } }

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
