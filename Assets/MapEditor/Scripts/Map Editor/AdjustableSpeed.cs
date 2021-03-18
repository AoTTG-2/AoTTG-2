using UnityEngine;

namespace MapEditor
{
    //A class for storing and retrieving multiple speed values
    public class AdjustableSpeed
    {
        //The three speeds the camera can move at
        private readonly float slowSpeed;
        private readonly float normalSpeed;

        private readonly float fastSpeed;

        //Determines if the slow and fast speeds are enabled
        private readonly bool slowSpeedEnabled = true;
        private readonly bool fastSpeedEnabled = true;

        //Calculate the three speeds based on a default speed and a multiplier
        public AdjustableSpeed(float defaultSpeed, float speedMultiplier)
        {
            normalSpeed = defaultSpeed;
            slowSpeed = defaultSpeed / speedMultiplier;
            fastSpeed = defaultSpeed * speedMultiplier;
        }

        //Overloaded constructor that accepts booleans to toggle the slow and fast speeds
        public AdjustableSpeed(float defaultSpeed, float speedMultiplier, bool slowSpeedEnabled, bool fastSpeedEnabled)
            : this(defaultSpeed, speedMultiplier)
        {
            this.slowSpeedEnabled = slowSpeedEnabled;
            this.fastSpeedEnabled = fastSpeedEnabled;
        }

        //Set the speed based on if control or shift is held
        public float GetSpeed()
        {
            if (slowSpeedEnabled && Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                return slowSpeed;
            if (fastSpeedEnabled && Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
                return fastSpeed;

            return normalSpeed;
        }
    }
}