using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBuildSystem : MonoBehaviour
{
    public List<buildObjects> objects = new List<buildObjects>();
    public buildObjects currentobject;
    private Vector3 currentpos;
    private Vector3 currentrot;
    public Transform currentpreview;
    public Transform cam;
    public RaycastHit hit;
    public LayerMask layer;

    public float offset = 1.0f;
    public float gridSize = 1.0f;

    public bool IsBuilding;

    // Start is called before the first frame update
    void Start()
    {
        
        changeCurrentBuilding(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (IsBuilding)
            startPreview();
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Build();

        if (Input.GetKeyDown("0") || Input.GetKeyDown("1") || Input.GetKeyDown("2"));
            switchCurrentBuilding();
    }

    public void switchCurrentBuilding()
    {
        for(int i = 0; i <3; i ++)
        {
            if (Input.GetKeyDown("" + i))
                changeCurrentBuilding(i);

        }


    }

    public void changeCurrentBuilding(int cur) 
    {
        currentobject = objects[cur];
        if (currentpreview != null)
            Destroy(currentpreview.gameObject);
        GameObject curprev = Instantiate(currentobject.preview, currentpos, Quaternion.Euler (currentrot)) as GameObject;
        currentpreview = curprev.transform;
    }
    public void startPreview()
    {
        if(Physics.Raycast(cam.position, cam.forward, out hit, 40, layer))
        
              if (hit.transform != this.transform)
            showPreview(hit);
                
        
    }
    public void showPreview (RaycastHit hit2)
    {
        currentpos = hit2.point;
        currentpos -= Vector3.one * offset;
        currentpos /= gridSize;
        currentpos = new Vector3(Mathf.Round(currentpos.x), Mathf.Round(currentpos.y), Mathf.Round(currentpos.z));
        currentpos *= gridSize;
        currentpos += Vector3.one * offset;
        currentpreview.position = currentpos;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            currentrot += new Vector3(0, 45, 0);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentrot -= new Vector3(0, 45, 0);
        currentpreview.localEulerAngles = currentrot;
    }

    public void Build()
    {
        PreviewObject PO = currentpreview.GetComponent<PreviewObject>();
        if(PO.IsBuildable)
        {
            Instantiate(currentobject.prefab, currentpos, Quaternion.Euler(currentrot));
        }

    }
}

[System.Serializable]
public class buildObjects
{
    public string name;
    public GameObject prefab;
    public GameObject preview;
    public int resources;

}


