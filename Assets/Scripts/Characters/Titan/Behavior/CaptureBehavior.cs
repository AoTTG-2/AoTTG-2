using Assets.Scripts.Gamemode;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    /// <summary>
    /// Influences the Titans <see cref="TitanState.Chase"/> state for the <see cref="CaptureGamemode"/>
    /// </summary>
    public class CaptureBehavior : TitanBehavior
    {
        public CaptureBehavior(PVPcheckPoint checkpoint)
        {
            CheckPoint = checkpoint;
        }

        private PVPcheckPoint CheckPoint { get; set; }
        private Vector3 TargetLocation { get; set; }
        private bool IsGoingToCheckpoint { get; set; }
        private float nextUpdate { get; set; }
        private float RotationModifier { get; set; }

        protected override bool OnWandering()
        {
            if (CheckPoint != null)
            {
                if (!Titan.Animation.IsPlaying(Titan.AnimationWalk))
                {
                    Titan.CrossFade(Titan.AnimationWalk, 0.5f);
                }

                if (Time.time < nextUpdate) return true;
                nextUpdate = Mathf.FloorToInt(Time.time) + 0.1f;

                if (CheckPoint.state == CheckPointState.Titan && Vector3.Distance(Titan.transform.position, TargetLocation) < 15f * CheckPoint.size)
                {
                    GameObject chkPtNext;
                    if (UnityEngine.Random.Range(0, 100) > 48)
                    {
                        chkPtNext = CheckPoint.chkPtNext;
                        if ((chkPtNext != null) && ((chkPtNext.GetComponent<PVPcheckPoint>().state != CheckPointState.Titan) || (UnityEngine.Random.Range(0, 100) < 20)))
                        {
                            TargetLocation = chkPtNext.transform.position;
                            CheckPoint = chkPtNext.GetComponent<PVPcheckPoint>();
                        }
                    }
                    else
                    {
                        chkPtNext = CheckPoint.chkPtPrevious;
                        if ((chkPtNext != null) && ((chkPtNext.GetComponent<PVPcheckPoint>().state != CheckPointState.Titan) || (UnityEngine.Random.Range(0, 100) < 5)))
                        {
                            TargetLocation = chkPtNext.transform.position;
                            CheckPoint = chkPtNext.GetComponent<PVPcheckPoint>();
                        }
                    }
                }
                else
                {
                    TargetLocation = CheckPoint.transform.position;
                }
                IsGoingToCheckpoint = true;
                return true;
            }
            IsGoingToCheckpoint = false;
            return false;
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
            var current = -Mathf.Atan2(vector17.z, vector17.x) * Mathf.Rad2Deg + RotationModifier;
            float num4 = -Mathf.DeltaAngle(current, Titan.transform.rotation.eulerAngles.y - 90f);
            Titan.transform.rotation = Quaternion.Lerp(Titan.transform.rotation, Quaternion.Euler(0f, Titan.transform.rotation.eulerAngles.y + num4, 0f), ((Titan.Speed * 0.5f) * Time.deltaTime) / Titan.Size);
            return true;
        }

        protected override bool OnWanderingUpdateEverySecond(int seconds)
        {
            if (seconds % 2 != 0) return true;
            Pathfinding();
            //if (Titan.IsStuck())
            //{
            //    RotationModifier = Random.Range(-100f, 100f);
            //}
            //else
            //{
            //    RotationModifier = 0f;
            //}
            return true;
        }

        private void Pathfinding()
        {
            Vector3 forwardDirection = Titan.Body.Hip.transform.TransformDirection(new Vector3(-0.3f, 0, 1f));
            RaycastHit objectHit;
            var mask = ~LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(Titan.Body.Hip.transform.position, forwardDirection, out objectHit, 50, mask))
            {
                Vector3 leftDirection = Titan.Body.Hip.transform.TransformDirection(new Vector3(-0.3f, -1f, 1f));
                Vector3 rightDirection = Titan.Body.Hip.transform.TransformDirection(new Vector3(-0.3f, 1f, 1f));
                RaycastHit leftHit;
                RaycastHit rightHit;
                Physics.Raycast(Titan.Body.Hip.transform.position, leftDirection, out leftHit, 250, mask);
                Physics.Raycast(Titan.Body.Hip.transform.position, rightDirection, out rightHit, 250, mask);

                if (Vector3.Distance(leftHit.point, TargetLocation) > Vector3.Distance(rightHit.point, TargetLocation))
                {
                    RotationModifier += 35f;
                }
                else
                {
                    RotationModifier += -35f;
                }
            }
            else
            {
                RotationModifier = 0f;

            }
        }
    }
}
