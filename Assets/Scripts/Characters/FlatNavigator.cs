using Assets.Scripts.Constants;
using System.Collections.Generic;
using UnityEngine;

public class FlatNavigator : MonoBehaviour
{

    public Color Color;
    public Material Material;

    private List<GameObject> visibleNavigations = new List<GameObject>();

    private List<Vector3> navStarts = new List<Vector3>();
    private List<Vector3> navEnds = new List<Vector3>();

    public void Start()
    {
        navStarts.Add(GetNavigatorPos());
    }

    public void Update()
    {
        foreach (GameObject navigation in visibleNavigations)
        {
            Destroy(navigation);
        }
        visibleNavigations.Clear();

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

    public void OnDestroy()
    {
        foreach (GameObject navigation in visibleNavigations)
        {
            Destroy(navigation);
        }
    }

    private void RenderNavigation(Vector3 start, Vector3 end)
    {
        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        visibleNavigations.Add(capsule);
        capsule.transform.localPosition = (start + end) / 2;
        capsule.transform.localScale = new Vector3(GetNavigatorRadius() * 2, (end - start).magnitude / 2, GetNavigatorRadius() * 2);
        if ((end - start).magnitude > 0)
        {
            capsule.transform.localRotation = Quaternion.LookRotation(end - start, Vector3.back);
            capsule.transform.Rotate(Vector3.up, 90, Space.World);
        }
        SetColor(capsule);
    }

    private void SetColor(GameObject col)
    {
        var render = col.GetComponent<Renderer>();
        if (Material != null) render.material = Material;
        render.material.color = Color;
        Destroy(col.GetComponent<Collider>());
    }

    public void Navigate(Vector3 targetPosition)
    {
        Vector3 targetPos = new Vector3(targetPosition.x, GetNavigatorPos().y, targetPosition.z);
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
                navEnds.Add(targetPos);
                canNavigate = true;
                break;
            }
        }

        if (!canNavigate)
        {
            if (navEnds.Count > 0)
            {
                navStarts.Add(navEnds[navEnds.Count - 1]);
            }
        }

        if (navStarts.Count > 0)
        {
            navStarts[0] = GetNavigatorPos();
            if (navStarts.Count > 1 && (navStarts[0] - navStarts[1]).magnitude < GetNavigatorRadius() / 2)
            {
                RemoveFromNavStarts(0);
                RemoveFromNavEnds(0);
            }
        }
        if (navEnds.Count < navStarts.Count)
        {
            navEnds.Add(targetPos);
        }
    }

    public Vector3 GetNavDir()
    {
        if (navEnds.Count > 0)
        {
            return navEnds[0] - navStarts[0];
        }
        return new Vector3(0, 0, 0);
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

    private bool CanNavigate(Vector3 start, Vector3 targetPos)
    {
        LayerMask mask = Layers.Ground.ToLayer();
        return !Physics.SphereCast(start, GetNavigatorRadius(), targetPos - start, out _, (targetPos - start).magnitude, mask);
    }

    private Vector3 GetNavigatorPos()
    {
        return gameObject.GetComponent<SphereCollider>().transform.position;
    }

    private float GetNavigatorRadius()
    {
        return gameObject.GetComponent<SphereCollider>().radius;
    }

}
