using UnityEngine;

namespace Assets.Scripts.Characters.Base
{

    public class Body : MonoBehaviour
    {
        public Transform Head;
        public Transform Neck;
        public Transform Chest;
        public Transform Hip;

        [Space]
        public Transform ShoulderLeft;
        public Transform UpperArmLeft;
        public Transform ArmLeft;
        public Transform HandLeft;

        [Space]
        public Transform ShoulderRight;
        public Transform UpperArmRight;
        public Transform ArmRight;
        public Transform HandRight;

        [Space]
        public Transform LegLeft;
        public Transform ShinLeft;
        public Transform FootLeft;

        [Space]
        public Transform LegRight;
        public Transform ShinRight;
        public Transform FootRight;
    }

}