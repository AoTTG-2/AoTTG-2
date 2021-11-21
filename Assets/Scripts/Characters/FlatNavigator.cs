using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Constants;
using System.Collections.Generic;
using UnityEngine;

public class FlatNavigator : MonoBehaviour
{

    public Color Color;
    public Material Material;

    private List<GameObject> visibleNavigations = new List<GameObject>();
    private bool display = true;

    private CapsuleCollider navBox;

    private const int MaxNav = 20;
    private const float BaseMaxNavLength = 120;
    private float maxNavLength;
    private List<Vector2> navPoints = new List<Vector2>();

    public void Start()
    {
        navBox = gameObject.GetComponent<CapsuleCollider>();
        navBox.height *= gameObject.transform.lossyScale.y;
        navBox.radius *= gameObject.transform.lossyScale.y;

        navPoints.Add(ToVec2(GetNavigatorBox().transform.position));
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
        // Find Navigation Route
        Vector2 targetPos = ToVec2(targetPosition);
        bool canNavigate = false;
        for (int i = 0; i < navPoints.Count; i++)
        {
            if (CanNavigate(navPoints[i], targetPos))
            {
                for (int j = i + 1; j < navPoints.Count; j++)
                {
                    RemoveFromNavPoints(i + 1);
                }
                AddToNavPoints(targetPos);
                canNavigate = true;
                break;
            }
        }
        if (!canNavigate && (navPoints[navPoints.Count - 1] - targetPos).magnitude > GetNavigatorBox().radius)
        {
            AddToNavPoints(targetPos);
        }
        if (navPoints.Count > 0)
        {
            navPoints[0] = ToVec2(GetNavigatorBox().transform.position);
            if (navPoints.Count > 1 && (navPoints[0] - navPoints[1]).magnitude < GetNavigatorBox().radius)
            {
                RemoveFromNavPoints(1);
            }
        }

        // Environmental Adaptation
        if (!IsOverlapping(navPoints[navPoints.Count - 1]))
        {
            List<Vector3> addition = new List<Vector3>(); // z value is used for setting the index of the navPoint
            for (int i = 1; i < navPoints.Count; i++)
            {
                foreach (RaycastHit interuption in NavigationInteruptions(navPoints[i - 1], navPoints[i]))
                {
                    if (interuption.collider == null) continue;
                    Vector2 interuptionPos = ToVec2(interuption.point);
                    if ((interuptionPos - navPoints[i]).magnitude > GetNavigatorBox().radius && (interuptionPos - navPoints[i - 1]).magnitude > GetNavigatorBox().radius && (interuptionPos - navPoints[i]).magnitude + (interuptionPos - navPoints[i - 1]).magnitude < GetNavigatorBox().radius + (navPoints[i - 1] - navPoints[i]).magnitude)
                    {
                        addition.Add(new Vector3(interuptionPos.x, interuptionPos.y, i));
                    }
                }
            }
            for (int i = addition.Count - 1; i >= 0; i--)
            {
                AddToNavPoints((int) addition[i].z, new Vector2(addition[i].x, addition[i].y));
            }
        }

        // Fix Navigation
        if (!IsOverlapping(navPoints[navPoints.Count - 1]))
        {
            for (int i = 1; i < navPoints.Count - 1; i++)
            {
                if (IsOverlapping(navPoints[i]))
                {
                    Vector2 newEnd;
                    Vector2[] adds = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1) };
                    foreach (Vector2 add in adds)
                    {
                        if (!IsOverlapping(newEnd = navPoints[i] + add * GetNavigatorBox().radius))
                        {
                            navPoints[i] = newEnd;
                            break;
                        }
                    }
                }
            }
        }

        // Shorten Navigation Route
        for (int i = 2; i < navPoints.Count; i++)
        {
            if ((navPoints[i - 2] - navPoints[i - 1]).magnitude > GetNavigatorBox().radius)
            {
                Vector2 shortStart = navPoints[i - 1] + (navPoints[i - 2] - navPoints[i - 1]).normalized * GetNavigatorBox().radius;
                if (CanNavigate(shortStart, navPoints[i]))
                {
                    navPoints[i - 1] = shortStart;
                }
            }
        }

        // Reset Navigation When Reach Max Length
        float length = 0;
        for (int i = 1; i < navPoints.Count; i++)
        {
            if ((length += (navPoints[i] - navPoints[i - 1]).magnitude) > maxNavLength)
            {
                navPoints.Clear();
                navPoints.Add(ToVec2(GetNavigatorBox().transform.position));
                break;
            }
        }
    }

    public Vector3 GetNavDir()
    {
        // When The Titan Cannot Reach The Player
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
        CapsuleCollider capsule = GetNavigatorBox();
        Vector3 capsuleStart = start + new Vector3(0, capsule.height / 2 - capsule.radius, 0);
        Vector3 capsuleEnd = start - new Vector3(0, capsule.height / 2 - capsule.radius, 0);
        return !Physics.CapsuleCast(capsuleStart, capsuleEnd, capsule.radius, targetPos - start, out _, (targetPos - start).magnitude, mask);
    }

    private RaycastHit[] NavigationInteruptions(Vector2 s, Vector2 e)
    {
        Vector3 start = ToVec3(s);
        Vector3 targetPos = ToVec3(e);
        LayerMask mask = Layers.Ground.ToLayer();
        CapsuleCollider capsule = GetNavigatorBox();
        Vector3 capsuleStart = start + new Vector3(0, capsule.height / 2 - capsule.radius, 0);
        Vector3 capsuleEnd = start - new Vector3(0, capsule.height / 2 - capsule.radius, 0);
        return Physics.CapsuleCastAll(capsuleStart, capsuleEnd, capsule.radius, targetPos - start, (targetPos - start).magnitude, mask);
    }

    private bool IsOverlapping(Vector2 p)
    {
        Vector3 pos = ToVec3(p);
        LayerMask mask = Layers.Ground.ToLayer();
        CapsuleCollider capsule = GetNavigatorBox();
        Vector3 capsuleStart = pos + new Vector3(0, capsule.height / 2 - capsule.radius, 0);
        Vector3 capsuleEnd = pos - new Vector3(0, capsule.height / 2 - capsule.radius, 0);
        return Physics.CheckCapsule(capsuleStart, capsuleEnd, capsule.radius, mask);
    }

    private CapsuleCollider GetNavigatorBox()
    {
        return navBox;
    }

    private float GetY()
    {
        return GetNavigatorBox().transform.position.y;
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
