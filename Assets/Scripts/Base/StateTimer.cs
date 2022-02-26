using UnityEngine;

namespace Assets.Scripts.Base
{
    public abstract class StateTimer : MonoBehaviour
    {
        #region ProtectedProperties
        protected float totalTimeInState;
        protected int maxTimer;
        protected float timer;
        protected float timeToAdd;
        #endregion

        #region Public Properties
        public bool IsActiveState { get { return timer > 0; } }
        #endregion

        #region Constructors
        protected StateTimer()
        {
            maxTimer = 15;
            timeToAdd = 5;
        }
        #endregion

        #region Monobehaviours
        protected virtual void FixedUpdate()
        {
            CalcTotalTime();
            SubtractTime();
            SetState();
        }

        protected virtual void Awake()
        {
            enabled = true;
        }
        #endregion

        #region Private Methods
        private void CalcTotalTime()
        {
            if (IsActiveState)
            {
                totalTimeInState += Time.deltaTime;
            }
            else if (totalTimeInState > 0)
            {
                totalTimeInState = 0;
            }
        }

        private void SubtractTime()
        {
            var deltaTime = Time.deltaTime;
            var result = timer - deltaTime;
            timer = result < 0 ? 0 : result;
        }
        #endregion

        #region Protected Methods
        protected abstract void SetState();
        #endregion

        #region Public Methods
        public void AddTime(float time)
        {
            var total = timer + time;
            timer = (total < maxTimer) ? total : maxTimer;
        }

        public void AddTime()
        {
            AddTime(timeToAdd);
        }
        #endregion
    }
}
