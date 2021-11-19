using Assets.Scripts.Constants;
using System.Collections.Generic;
using UnityEngine;

public class FlatNavigator : MonoBehaviour
{

    public Color Color;
    public Material Material;

    private List<GameObject> visibleNavigations = new List<GameObject>();
    private bool display = true;

    private const int MaxNav = 15;
    private List<Vector2> navStarts = new List<Vector2>();
    private List<Vector2> navEnds = new List<Vector2>();

    public void Start()
    {
        navStarts.Add(ToVec2(GetNavigatorBox().transform.position));
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
            for (int i = 0; i < navStarts.Count; i++)
            {
                if (navEnds.Count > i)
                {
                    RenderNavigation(navStarts[i], navEnds[i]);
                }
                else
                {
                    RenderNavigation(navStarts[i], navStarts[i]);
                }
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
        for (int i = 0; i < navStarts.Count; i++)
        {
            if (CanNavigate(navStarts[i], targetPos))
            {
                for (int j = i + 1; j < navStarts.Count; j++)
                {
                    RemoveFromNavStarts(i + 1);
                    RemoveFromNavEnds(i + 1);
                }
                RemoveFromNavEnds(i);
                AddToNavEnds(targetPos);
                canNavigate = true;
                break;
            }
        }

        if (!canNavigate)
        {
            if (navEnds.Count > 0 && (navEnds[navEnds.Count - 1] - navStarts[navEnds.Count - 1]).magnitude > GetNavigatorBox().radius)
            {
                AddToNavStarts(navEnds[navEnds.Count - 1]);
            }
        }

        if (navStarts.Count > 0)
        {
            navStarts[0] = ToVec2(GetNavigatorBox().transform.position);
            if (navStarts.Count > 1 && (navStarts[0] - navEnds[0]).magnitude < GetNavigatorBox().radius)
            {
                RemoveFromNavStarts(1);
                RemoveFromNavEnds(0);
            }
        }
        if (navEnds.Count < navStarts.Count)
        {
            AddToNavEnds(targetPos);
        }

        // Fix Navigation
        for (int i = 0; i < navEnds.Count - 1; i++)
        {
            if (IsOverlapping(navEnds[i]))
            {
                Vector2 newEnd;
                Vector2[] adds = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1) };
                foreach (Vector2 add in adds)
                {
                    if (!IsOverlapping(newEnd = navEnds[i] + add * GetNavigatorBox().radius))
                    {
                        navEnds[i] = newEnd;
                        navStarts[i + 1] = newEnd;
                        break;
                    }
                }
            }
        }

        // Shorten Navigation Route
        for (int i = 1; i < navEnds.Count; i++)
        {
            if ((navStarts[i - 1] - navEnds[i - 1]).magnitude > GetNavigatorBox().radius)
            {
                Vector2 shortStart = navStarts[i] + (navStarts[i - 1] - navEnds[i - 1]).normalized * GetNavigatorBox().radius;
                if (CanNavigate(shortStart, navEnds[i]))
                {
                    navStarts[i] = shortStart;
                    navEnds[i - 1] = shortStart;
                }
            }
        }
    }

    public Vector3 GetNavDir()
    {
        if (navEnds.Count > 0)
        {
            return ToVec3(navEnds[0] - navStarts[0]);
        }
        return new Vector3(0, 0, 0);
    }

    private void AddToNavStarts(Vector2 v)
    {
        if (navStarts.Count <= MaxNav)
        {
            navStarts.Add(v);
        }
    }

    private void AddToNavEnds(Vector2 v)
    {
        if (navEnds.Count <= MaxNav)
        {
            navEnds.Add(v);
        }
    }

    private void RemoveFromNavStarts(int index)
    {
        if (navStarts.Count > index)
        {
            navStarts.RemoveAt(index);
        }
    }

    private void RemoveFromNavEnds(int index)
    {
        if (navEnds.Count > index)
        {
            navEnds.RemoveAt(index);
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
        return gameObject.GetComponent<CapsuleCollider>();
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
