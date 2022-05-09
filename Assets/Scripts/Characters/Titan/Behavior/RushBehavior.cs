using Assets.Scripts.Events;
using Assets.Scripts.Gamemode;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    /// <summary>
    /// Overrides the titans state behavior for the <see cref="TitanRushGamemode"/>
    /// </summary>
    public class RushBehavior : TitanBehavior
    {
        public RushBehavior(ArrayList route)
        {
            checkPoints = route;
        }

        public event OnCheckpointArrived OnCheckpointArrived;
        private float activeRad = 0f;
        private ArrayList checkPoints;
        private Vector3 TargetLocation { get; set; }
        private bool IsGoingToCheckpoint { get; set; }

        protected override bool OnWandering()
        {
            if (!Titan.Animation.IsPlaying(Titan.AnimationWalk))
                Titan.CrossFade(Titan.AnimationWalk, 0.5f);

            if (checkPoints.Count > 1)
            {
                if (Vector3.Distance((Vector3) checkPoints[0], Titan.transform.position) > activeRad)
                {
                    TargetLocation = (Vector3) checkPoints[0];
                    IsGoingToCheckpoint = true;
                }

                if (Vector3.Distance(Titan.transform.position, TargetLocation) < 10f)
                {
                    if (checkPoints.Count == 4)
                    {
                        FengGameManagerMKII.instance.sendChatContentInfo("<color=#A8FF24>*WARNING!* An abnormal titan is approaching the north gate!</color>");
                    }
                    checkPoints.RemoveAt(0);
                    if (checkPoints.Count == 1 && PhotonNetwork.isMasterClient)
                    {
                        OnCheckpointArrived?.Invoke(TargetLocation, Titan);
                    }
                    else
                    {
                        TargetLocation = (Vector3) checkPoints[0];
                    }
                }
            }
            return true;
        }

        protected override bool OnChase()
        {
            Titan.SetState(TitanState.Wandering);
            return true;
        }

        protected override bool OnWanderingFixedUpdate()
        {
            if (!IsGoingToCheckpoint) return false;
            Vector3 vector12 = Titan.transform.forward * Titan.Speed;
            Vector3 vector14 = vector12 - Titan.Rigidbody.velocity;
            vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
            vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
            vector14.y = 0f;
            Titan.Rigidbody.AddForce(vector14, ForceMode.VelocityChange);

            Vector3 vector17 = TargetLocation - Titan.transform.position;
            var current = -Mathf.Atan2(vector17.z, vector17.x) * Mathf.Rad2Deg;
            float num4 = -Mathf.DeltaAngle(current, Titan.transform.rotation.eulerAngles.y - 90f);
            Titan.transform.rotation = Quaternion.Lerp(Titan.transform.rotation, Quaternion.Euler(0f, Titan.transform.rotation.eulerAngles.y + num4, 0f), ((Titan.Speed * 0.5f) * Time.deltaTime) / Titan.Size);
            return true;
        }
    }
}
