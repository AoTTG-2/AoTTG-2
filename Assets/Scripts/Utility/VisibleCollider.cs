using Assets.Scripts.Settings.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class VisibleCollider : MonoBehaviour
    {

        public Color Color;
        public Material Material;

        private Collider[] gameObjectColliders;
        private GameObject[] visibleColliders = new GameObject[0];
        private void Awake()
        {
            gameObjectColliders = gameObject.GetComponents<Collider>();


            Setting.Debug.ShowColliders.OnValueChanged += ShowColliders_OnValueChanged;
            ShowColliders_OnValueChanged(Setting.Debug.ShowColliders.Value);
        }

        private void OnDestroy()
        {
            Setting.Debug.ShowColliders.OnValueChanged -= ShowColliders_OnValueChanged;
        }

        private void ShowColliders_OnValueChanged(bool value)
        {
            if (!value)
            {
                foreach (var visibleCollider in visibleColliders)
                {
                    Destroy(visibleCollider);
                }

                visibleColliders = new GameObject[0];
                return;
            }

            var visibleGameObjects = new HashSet<GameObject>();
            foreach (var collider in gameObjectColliders)
            {
                var colTransform = collider.transform;
                if (collider is BoxCollider boxCollider)
                {
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    visibleGameObjects.Add(cube);
                    cube.transform.parent = transform;
                    cube.transform.rotation = new Quaternion();
                    cube.transform.localScale = boxCollider.size;
                    cube.transform.localPosition = boxCollider.center;
                    SetColor(cube);
                    //TODO: Capsules with a height lower than 1 should use a sphere

                }
                else if (collider is CapsuleCollider capsuleCollider)
                {
                    var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    visibleGameObjects.Add(capsule);
                    capsule.transform.parent = transform;
                    capsule.transform.rotation = new Quaternion();
                    if (capsuleCollider.direction == 0) // X axis
                    {
                        var currentRot = capsule.transform.rotation.eulerAngles;
                        capsule.transform.rotation = Quaternion.Euler(currentRot.x, currentRot.y, currentRot.z + 90);
                        capsule.transform.localScale = new Vector3(capsuleCollider.radius * 2,
                            capsuleCollider.height / 2, capsuleCollider.radius * 2);
                    }
                    else if (capsuleCollider.direction == 1) // Y axis
                    {
                        capsule.transform.localScale = new Vector3(capsuleCollider.radius * 2,
                            capsuleCollider.height / 2, capsuleCollider.radius * 2);
                    }
                    else if (capsuleCollider.direction == 2) // Z axis
                    {

                    }

                    capsule.transform.localPosition = capsuleCollider.center;
                    SetColor(capsule);
                }
            }

            visibleColliders = visibleGameObjects.ToArray();
        }
        
        private void SetColor(GameObject col)
        {
            var render = col.GetComponent<Renderer>();
            if (Material != null) render.material = Material;
            render.material.color = Color;
            Destroy(col.GetComponent<Collider>());
        }
    }
}
