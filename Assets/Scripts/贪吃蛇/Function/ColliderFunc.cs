 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFunc : MonoBehaviour
{
    public GameObject localPlane;
    public Vector2 xy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Plane"&&localPlane != other.transform.parent.gameObject)
        {
            PlaneController.Instance.ChangeLocalPlanes((int)xy.x, (int)xy.y, other.transform.gameObject);
            localPlane = other.transform.gameObject;
            
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Plane")
        {
            localPlane = null;
            StartCoroutine(CheckPlane());
        }
    }

    IEnumerator CheckPlane()
    {
        yield return new WaitForSeconds(8f);
        if(localPlane == null)
        {
            Debug.Log("∑≈÷√" + (int)xy.x + " " + (int)xy.y);
            localPlane = PlaneController.Instance.CreatPlane((int)xy.x,(int)xy.y);
            
        }
    }
}
