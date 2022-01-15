using Assets.Scripts.Constants;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{

    public class FlatNavigator : MonoBehaviour
    {

        public Color Color;
        public Material Material;
        public bool display = false; // Tick this in the inspector to display path

        private readonly List<GameObject> visibleNavigations = new List<GameObject>();

        private CapsuleCollider navBox; // Hitbox used for checking possible path.

        private const int MaxNav = 20;
        private const float BaseMaxNavLength = 120;
        private float maxNavLength;
        private readonly List<Vector2> navPoints = new List<Vector2>(); // Used 2D Vectors for optimization
        private float length = 0;

        public void Start()
        {
            navBox = gameObject.GetComponent<CapsuleCollider>();
            navBox.height *= gameObject.transform.lossyScale.y;
            navBox.radius *= gameObject.transform.lossyScale.y;

            navPoints.Add(ToVec2(navBox.transform.position));
            maxNavLength = BaseMaxNavLength + 50 * transform.root.gameObject.GetComponent<TitanBase>().Size;
        }

        public void Update()
        {
            foreach (GameObject navigation in visibleNavigations)
            {
                Destroy(navigation);
            }
            visibleNavigations.Clear();

            if (display)
            {
                for (int i = 0; i < navPoints.Count - 1; i++)
                {
                    RenderNavigation(navPoints[i], navPoints[i + 1]);
                }
            }
        }

        public void OnDestroy()
        {
            foreach (GameObject navigation in visibleNavigations)
            {
                Destroy(navigation);
            }
        }

        private void RenderNavigation(Vector2 s, Vector2 e)
        {
            Vector3 start = ToVec3(s);
            Vector3 end = ToVec3(e);
            GameObject navigation = new GameObject();
            visibleNavigations.Add(navigation);
            navigation.transform.position = start;
            LineRenderer renderer = navigation.AddComponent<LineRenderer>();
            renderer.material = Material;
            renderer.material.color = Color;
            renderer.startColor = Color;
            renderer.endColor = Color;
            renderer.startWidth = 0.2f;
            renderer.endWidth = 0.2f;
            renderer.SetPosition(0, start);
            renderer.SetPosition(1, end);
            if ((end - start).magnitude > 0)
            {
                navigation.transform.localRotation = Quaternion.LookRotation(end - start, end - start);
            }
        }

        public void Navigate(Vector3 targetPosition)
        {
            Vector2 targetPos = ToVec2(targetPosition);
            FindNavigationRoute(targetPos);
            AdaptToEnvironmentalChanges();
            FixNavigation();
            ShortenNavigation();
            NavigationResetCheck();
        }

        private void FindNavigationRoute(Vector2 targetPos)
        {
            // Cut short any redundant point (Remove children points if parent point can reach targetPos)
            for (int i = 0; i < navPoints.Count; i++)
            {
                if (CanNavigate(navPoints[i], targetPos))
                {
                    for (int j = i + 1; j < navPoints.Count; j++)
                    {
                        RemoveFromNavPoints(i + 1);
                    }
                    break;
                }
            }
            // Connect last point to the targetPos
            if ((navPoints[navPoints.Count - 1] - targetPos).magnitude > navBox.radius)
            {
                AddToNavPoints(targetPos);
            }

            // Merge points if they are in close proximity
            for (int i = 1; i < navPoints.Count - 1; i++)
            {
                if ((navPoints[i] - navPoints[i + 1]).magnitude < navBox.radius)
                {
                    navPoints[i] = (navPoints[i] + navPoints[i + 1]) / 2;
                    RemoveFromNavPoints(i + 1);
                }
            }

            // Remove point if it gets too close to the entity (To stop the entity from targeting that point
            // and avoid them standing at that point forever)
            if (navPoints.Count > 0)
            {
                navPoints[0] = ToVec2(navBox.transform.position);
                if (navPoints.Count > 1 && (navPoints[0] - navPoints[1]).magnitude < navBox.radius)
                {
                    RemoveFromNavPoints(1);
                }
            }
        }

        private void AdaptToEnvironmentalChanges()
        {
            if (!IsOverlapping(navPoints[navPoints.Count - 1]))
            {
                List<Vector3> addition = new List<Vector3>(); // z value is used for setting the index of the navPoint
                for (int i = 1; i < navPoints.Count; i++)
                {
                    if (CanNavigate(navPoints[i - 1], navPoints[i]) || IsOverlapping(navPoints[i - 1]) || IsOverlapping(navPoints[i]))
                    {
                        // Add another point at the position where it was interrupted
                        Vector2 interruptionPos = (navPoints[i] + navPoints[i - 1]) / 2;
                        if ((interruptionPos - navPoints[i]).magnitude > navBox.radius && (interruptionPos - navPoints[i - 1]).magnitude > navBox.radius && (interruptionPos - navPoints[i]).magnitude + (interruptionPos - navPoints[i - 1]).magnitude < navBox.radius + (navPoints[i - 1] - navPoints[i]).magnitude)
                        {
                            addition.Add(new Vector3(interruptionPos.x, interruptionPos.y, i));
                        }
                    }
                }
                // Actually adding them in
                for (int i = addition.Count - 1; i >= 0; i--)
                {
                    AddToNavPoints((int) addition[i].z, new Vector2(addition[i].x, addition[i].y));
                }
            }
        }

        private void FixNavigation()
        {
            if (!IsOverlapping(navPoints[navPoints.Count - 1]))
            {
                for (int i = 1; i < navPoints.Count - 1; i++)
                {
                    // Move any point that is overlapping the environment
                    if (IsOverlapping(navPoints[i]))
                    {
                        Vector2 newEnd;
                        // Possible directions to move the point
                        Vector2[] adds = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1) };
                        foreach (Vector2 add in adds)
                        {
                            newEnd = navPoints[i] + add * navBox.radius;
                            if (!IsOverlapping(newEnd))
                            {
                                navPoints[i] = newEnd;
                                break;
                            }
                        }
                    }
                }
            }
        }

        // Shorten the navigation length when possible to optimize movement
        private void ShortenNavigation()
        {
            for (int i = 2; i < navPoints.Count; i++)
            {
                if ((navPoints[i - 2] - navPoints[i - 1]).magnitude > navBox.radius)
                {
                    Vector2 shortStart = navPoints[i - 1] + (navPoints[i - 2] - navPoints[i - 1]).normalized * navBox.radius;
                    if (CanNavigate(shortStart, navPoints[i]))
                    {
                        navPoints[i - 1] = shortStart;
                    }
                }
            }
        }

        // Reset the navigation when the max length is reached
        private void NavigationResetCheck()
        {
            length = 0;
            for (int i = 1; i < navPoints.Count; i++)
            {
                if ((length += (navPoints[i] - navPoints[i - 1]).magnitude) > maxNavLength)
                {
                    navPoints.Clear();
                    navPoints.Add(ToVec2(navBox.transform.position)); // The first point must be the entiy position
                    break;
                }
            }
        }

        // Return the current direction of movement
        public Vector3 GetNavDir()
        {
            // Move toward the target if the navigation length is 5 times longer than moving straight toward the target
            if (length > 5 * (navPoints[0] - navPoints[navPoints.Count - 1]).magnitude)
            {
                return ToVec3(navPoints[navPoints.Count - 1] - navPoints[0]);
            }

            // When The Titan Cannot Reach The Player
            // This behavior stop the titan from moving away from the player when the player is hiding
            // in somewhere the titan cannot reach
            if (IsOverlapping(navPoints[navPoints.Count - 1]))
            {
                float minDistance = (navPoints[0] - navPoints[navPoints.Count - 1]).magnitude;
                for (int i = 1; i < navPoints.Count - 1; i++)
                {
                    if ((navPoints[i] - navPoints[navPoints.Count - 1]).magnitude < minDistance)
                    {
                        return ToVec3(navPoints[i] - navPoints[0]);
                    }
                }
                return ToVec3(navPoints[navPoints.Count - 1] - navPoints[0]);
            }

            // Finally return the current direction when none of the case above is true
            // The titan won't follow the displayed navigation line if one of the case above is true
            if (navPoints.Count > 1)
            {
                return ToVec3(navPoints[1] - navPoints[0]);
            }
            return new Vector3(0, 0, 0);
        }

        private void AddToNavPoints(Vector2 v)
        {
            if (navPoints.Count <= MaxNav)
            {
                navPoints.Add(v);
            }
        }

        private void AddToNavPoints(int index, Vector2 v)
        {
            if (navPoints.Count <= MaxNav)
            {
                navPoints.Insert(index, v);
            }
        }

        private void RemoveFromNavPoints(int index)
        {
            if (navPoints.Count > index)
            {
                navPoints.RemoveAt(index);
            }
        }

        private bool CanNavigate(Vector2 s, Vector2 e)
        {
            Vector3 start = ToVec3(s);
            Vector3 targetPos = ToVec3(e);
            LayerMask mask = Layers.Ground.ToLayer();
            CapsuleCollider capsule = navBox;
            Vector3 capsuleStart = start + new Vector3(0, capsule.height / 2 - capsule.radius, 0);
            Vector3 capsuleEnd = start - new Vector3(0, capsule.height / 2 - capsule.radius, 0);
            return !Physics.CapsuleCast(capsuleStart, capsuleEnd, capsule.radius, targetPos - start, out _, (targetPos - start).magnitude, mask);
        }

        private bool IsOverlapping(Vector2 p)
        {
            Vector3 pos = ToVec3(p);
            LayerMask mask = Layers.Ground.ToLayer();
            CapsuleCollider capsule = navBox;
            Vector3 capsuleStart = pos + new Vector3(0, capsule.height / 2 - capsule.radius, 0);
            Vector3 capsuleEnd = pos - new Vector3(0, capsule.height / 2 - capsule.radius, 0);
            return Physics.CheckCapsule(capsuleStart, capsuleEnd, capsule.radius, mask);
        }

        private float GetY()
        {
            return navBox.transform.position.y;
        }

        private Vector3 ToVec3(Vector2 vec)
        {
            return new Vector3(vec.x, GetY(), vec.y);
        }

        private Vector2 ToVec2(Vector3 vec)
        {
            return new Vector2(vec.x, vec.z);
        }

    }
}