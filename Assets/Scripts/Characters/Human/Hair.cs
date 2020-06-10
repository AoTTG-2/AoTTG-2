using UnityEngine;

public class Hair : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private SEX sex;
    private Material hairMat;

    public int ID { get { return id; } }
    public SEX Sex { get { return sex; } }

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
