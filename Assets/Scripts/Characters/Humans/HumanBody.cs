using UnityEngine;

namespace Assets.Scripts.Characters.Humans
{
    /// <summary>
    /// Contains references for all human bodyparts that are used. Always use this class instead of GameObject.Find()!
    /// </summary>
    public class HumanBody : MonoBehaviour
    {

        [SerializeField] public GameObject Armature;
        public Transform ControllerBody;
        public Transform hip;

        [Header("Chest")] 
        public Transform spine;
        public Transform chest;

        [Header("Head")]
        public Transform neck;
        public Transform head;

        [Header("Left Arm")]
        public Transform shoulder_L;
        public Transform upper_arm_L;
        public Transform forearm_L;
        public Transform hand_L;

        [Header("Right Arm")]
        public Transform shoulder_R;
        public Transform upper_arm_R;
        public Transform forearm_R;
        public Transform hand_R;

        [Header("Left Leg")]
        public Transform thigh_L;
        public Transform shin_L;
        public Transform foot_L;
        public Transform toe_L;

        [Header("Right Leg")]
        public Transform thigh_R;
        public Transform shin_R;
        public Transform foot_R;
        public Transform toe_R;

        public Transform[] Bones { get; set; }

        private void Awake()
        {
            Bones = new Transform[]
            {
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                ControllerBody,
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
                toe_L,
                thigh_R,
                shin_R,
                foot_R,
                toe_R
            };
        }
    }
}
