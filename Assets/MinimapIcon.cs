using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    [SerializeField] private Transform parent;

    private void Start()
    {
        parent = GetComponentInParent<Transform>();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.parent.position.x, 245f, transform.parent.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(90f, transform.parent.rotation.eulerAngles.y, 0f));
    }
}
