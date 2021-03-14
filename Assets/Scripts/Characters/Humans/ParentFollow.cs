using UnityEngine;

namespace Assets.Scripts.Characters.Humans
{
    public class ParentFollow : MonoBehaviour
    {
        public bool isActiveInScene;
        public Transform parent;

        private void Awake()
        {
            this.isActiveInScene = true;
        }

        public void RemoveParent()
        {
            this.parent = null;
        }

        public void SetParent(Transform transform)
        {
            this.parent = transform;
        }

        private void Update()
        {
            if (this.isActiveInScene && (this.parent != null))
            {
                transform.position = this.parent.position;
            }
        }
    }
}

