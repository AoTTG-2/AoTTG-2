using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{

    public bool foundation;
    public List<Collider> col = new List<Collider>();
    public Material green;
    public Material red;
    public bool IsBuildable;

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 26 && foundation)
            col.Add(other);
    }

    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == 26 && foundation)
            col.Remove(other);

    }
    void Update()
    {

        changecolour();
    }


    public void changecolour()
    {
        if (col.Count == 0)
            IsBuildable = true;
        else
            IsBuildable = false;

        if (IsBuildable)
        {
            foreach (Transform child in this.transform)
            {
                child.GetComponent<Renderer>().material = green;


            }


        }
        else
        {
            foreach (Transform child in this.transform)
            {
                child.GetComponent<Renderer>().material = red;


            }


        }

    }




 
}
