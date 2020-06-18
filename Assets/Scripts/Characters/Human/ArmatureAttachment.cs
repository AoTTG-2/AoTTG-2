using System;
using System.Collections.Generic;
using UnityEngine;

//Defines what bone this object will be parented to
public class ArmatureAttachment : MonoBehaviour
{
    //An enum of armature bones for the inspector drop-down
    private enum ArmatureBone
    {
        Controller_Body,
        hip,
        spine,
        chest,
        neck,
        head,
        shoulder_L,
        upper_arm_L,
        forearm_L,
        hand_L,
        shoulder_R,
        upper_arm_R,
        forearm_R,
        hand_R,
        thigh_L,
        shin_L,
        foot_L,
        thigh_R,
        shin_R,
        foot_R
    }

    [Serializable]
    private struct AttachPoint
    {
        [SerializeField] private GameObject targetObject;
        [SerializeField] private ArmatureBone parentBone;

        public void AttachToArmature(ArmatureData armature)
        {
            string boneName = parentBone.ToString();
            //Use the bone name string to get the corresponding game object
            GameObject parentBone2 = (GameObject) typeof(ArmatureData).GetProperty(boneName).GetValue(armature, null);
            //Parent the object to the bone
            targetObject.transform.parent = parentBone2.transform;
        }
    }

    [SerializeField] List<AttachPoint> attachPoints;

    public void AttachToArmature(ArmatureData armature)
    {
        foreach (AttachPoint attachPoints in attachPoints)
            attachPoints.AttachToArmature(armature);
    }
}
