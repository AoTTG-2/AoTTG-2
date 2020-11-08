using UnityEngine;

namespace Assets.Scripts.Characters.Humans
{
    public class HumanBody : MonoBehaviour
    {
        public Transform Hip;

        [Header("Chest")] 
        public Transform Spine;
        public Transform Chest;

        [Header("Head")]
        public Transform Head;
        public Transform Neck;

        [Header("Left Arm")]
        public Transform ShoulderLeft;
        public Transform UpperArmLeft;
        public Transform ForearmLeft;
        public Transform HandLeft;

        [Header("Right Arm")]
        public Transform ShoulderRight;
        public Transform UpperArmRight;
        public Transform ForearmRight;
        public Transform HandRight;

        [Header("Left Leg")]
        public Transform ThighLeft;
        public Transform ShinLeft;
        public Transform FootLeft;

        [Header("Right Leg")]
        public Transform ThighRight;
        public Transform ShinRight;
        public Transform FootRight;
    }
}
