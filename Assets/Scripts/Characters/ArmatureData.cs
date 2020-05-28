using System.Reflection;
using UnityEngine;

//Sets and stores the references to the bone transforms in a humanoid armature
[ExecuteInEditMode]
public class ArmatureData : MonoBehaviour
{
    //The parent Game Object of the armature hierarchy 
    [HideInInspector] public GameObject armatureObject;

    #region Armature References
    [SerializeField] public Transform Controller_Body;
    [SerializeField] public Transform hip;
    [SerializeField] public Transform spine;
    [SerializeField] public Transform chest;
    [SerializeField] public Transform neck;
    [SerializeField] public Transform head;

    [SerializeField] public Transform shoulder_L;
    [SerializeField] public Transform upper_arm_L;
    [SerializeField] public Transform forearm_L;
    [SerializeField] public Transform hand_L;

    [SerializeField] public Transform shoulder_R;
    [SerializeField] public Transform upper_arm_R;
    [SerializeField] public Transform forearm_R;
    [SerializeField] public Transform hand_R;

    [SerializeField] public Transform thigh_L;
    [SerializeField] public Transform shin_L;
    [SerializeField] public Transform foot_L;

    [SerializeField] public Transform thigh_R;
    [SerializeField] public Transform shin_R;
    [SerializeField] public Transform foot_R;
    #endregion

    #region Methods
    //Store the armature transforms when the prefab is updated
    public void SetBoneReferences()
    {
        if (!armatureObject)
            Debug.Log("No armature set, can't update bone references");
        else
        {
            SetBoneReferencesHelper(armatureObject.transform);
            Debug.Log("Bone references updated");
        }
    }

    //Recurse through the hierarchy and store the bone transforms in the associated field
    private void SetBoneReferencesHelper(Transform parent)
    {
        foreach (Transform child in parent)
        {
            FieldInfo referenceInfo = GetType().GetField(child.name);

            if (referenceInfo != null)
            {
                referenceInfo.SetValue(this, child.transform);
                SetBoneReferencesHelper(child);
            }
        }
    }
    #endregion
}