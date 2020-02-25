using UnityEngine;

[AddComponentMenu("NGUI/Examples/Item Attachment Point")]
public class InvAttachmentPoint : MonoBehaviour
{
    private GameObject mChild;
    private GameObject mPrefab;
    public InvBaseItem.Slot slot;

    public GameObject Attach(GameObject prefab)
    {
        if (this.mPrefab != prefab)
        {
            this.mPrefab = prefab;
            if (this.mChild != null)
            {
                Object.Destroy(this.mChild);
            }
            if (this.mPrefab != null)
            {
                Transform transform = base.transform;
                this.mChild = Object.Instantiate(this.mPrefab, transform.position, transform.rotation) as GameObject;
                Transform transform2 = this.mChild.transform;
                transform2.parent = transform;
                transform2.localPosition = Vector3.zero;
                transform2.localRotation = Quaternion.identity;
                transform2.localScale = Vector3.one;
            }
        }
        return this.mChild;
    }
}

