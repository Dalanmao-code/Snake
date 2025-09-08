using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("场景类别")]
    [SerializeField] public List<Plane> planes;
    public static int PlanesLenth;
    public static PlaneController Instance;
    [Header("动态储存场景")]
    [SerializeField] public GameObject[,] localPlanes;
    public GameObject CenterPlane;
    [Header("生成物体相关")]
    [SerializeField] public float RandomSize;
    // Start is called before the first frame update

    private void OnEnable()
    {
        RandomCreatObjects(CenterPlane, 0);
        Instance = this;
        PlanesLenth = 240;
    }
    void Start()
    {
        localPlanes = new GameObject[3,3];
        IniPlanes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IniPlanes()
    {
        CreatPlane(0, 0);
        CreatPlane(0, 1);
        CreatPlane(0, 2);
        CreatPlane(1, 0);
        CreatPlane(1, 2);
        CreatPlane(2, 0);
        CreatPlane(2, 1);
        CreatPlane(2, 2);

    }

    public GameObject CreatPlane(int x,int y)
    {
        GameObject plane = ProbabilityCreat();
        if (x == 0 && y == 0)
        {
            plane.transform.position = new Vector3(CenterPlane.transform.position.x + PlanesLenth, CenterPlane.transform.position.y, CenterPlane.transform.position.z - PlanesLenth);
        }
        if (x == 0 && y == 1)
        {
            plane.transform.position = new Vector3(CenterPlane.transform.position.x, CenterPlane.transform.position.y, CenterPlane.transform.position.z - PlanesLenth);
        }
        if (x == 0 && y == 2)
        {
            plane.transform.position = new Vector3(CenterPlane.transform.position.x - PlanesLenth, CenterPlane.transform.position.y, CenterPlane.transform.position.z - PlanesLenth);
        }
        if (x == 1 && y == 0)
        {
            plane.transform.position = new Vector3(CenterPlane.transform.position.x + PlanesLenth, CenterPlane.transform.position.y, CenterPlane.transform.position.z);
        }
        if (x == 1 && y == 1)
        {
            plane.transform.position = new Vector3(CenterPlane.transform.position.x, CenterPlane.transform.position.y, CenterPlane.transform.position.z);
        }
        if (x == 1 && y == 2)
        {
            plane.transform.position = new Vector3(CenterPlane.transform.position.x - PlanesLenth, CenterPlane.transform.position.y, CenterPlane.transform.position.z );
        }
        if (x == 2 && y == 0)
        {
            plane.transform.position = new Vector3(CenterPlane.transform.position.x + PlanesLenth, CenterPlane.transform.position.y, CenterPlane.transform.position.z+ PlanesLenth);
        }
        if (x == 2 && y == 1)
        {
            plane.transform.position = new Vector3(CenterPlane.transform.position.x, CenterPlane.transform.position.y, CenterPlane.transform.position.z+ PlanesLenth);
        }
        if (x == 2 && y == 2)
        {
            plane.transform.position = new Vector3(CenterPlane.transform.position.x - PlanesLenth, CenterPlane.transform.position.y, CenterPlane.transform.position.z + PlanesLenth);
        }
        localPlanes[x,y] = plane;
        return plane;

    }

    //概率创造板块
    public GameObject ProbabilityCreat()
    {
        int Allcount = 0;
        foreach (var plane in planes)
        {
            Allcount += plane.Probability;
        }
        //Debug.Log(Allcount);
        int x = UnityEngine.Random.Range(1, Allcount+1);

        for(int i = 0; i < planes.Count; i++)
        {
            if (planes[i].Probability >= x)
            {
                Debug.Log(i);
                GameObject plane = Instantiate(planes[i].PlanePrefabs, this.transform);
                RandomCreatObjects(plane, i);
                return plane;
            }
            else
            {
                x -= planes[i].Probability;
            }
        }
        return null;

    }

    //概率创造物体
    public void RandomCreatObjects(GameObject plane,int planecount)
    {
        
        for (int i =0;i< planes[planecount].sceneObjs.Count; i++)
        {
            List<GameObject> objects_ = new List<GameObject>();
            for(int j = 0;j< planes[planecount].sceneObjs[i].Count+UnityEngine.Random.Range(-planes[planecount].sceneObjs[i].floatCount, planes[planecount].sceneObjs[i].floatCount+1); j++)
            {
                //防止与其他物体碰撞
                while (true)
                {
                    float x = UnityEngine.Random.Range(-RandomSize, RandomSize);
                    float y = UnityEngine.Random.Range(-RandomSize, RandomSize);
                    GameObject Ini = Instantiate(planes[planecount].sceneObjs[i].Object, plane.transform);
                    Ini.transform.localPosition = new Vector3(-49.618f + x, 1.094f, -16.568f + y);
                    foreach (GameObject obj in objects_)
                    {
                        if(Vector3.Distance(obj.transform.localPosition, Ini.transform.localPosition)< planes[planecount].sceneObjs[i].Intervaldistance)
                        {
                            Destroy(Ini);
                            break;
                        }
                    }
                    if(Ini!= null)
                    {
                        objects_.Add(Ini);
                        break;
                    }
                }
            }
        }
    }

    public void ChangeLocalPlanes(int x,int y,GameObject plane)
    {
        Debug.Log("发现");
        if (x == 1 && y == 1)
        {
            CenterPlane = plane;
        }
        
        localPlanes[x,y] = plane;
       
    }

}
[Serializable]
public class Plane
{
    public GameObject PlanePrefabs;
    public int Probability;
    public List<SceneObj> sceneObjs;
    public Plane(GameObject planePrefabs, int probability, List<SceneObj> sceneObjs)
    {
        PlanePrefabs = planePrefabs;
        Probability = probability;
        this.sceneObjs = sceneObjs;
    }
}
[Serializable]
public class SceneObj
{
    public GameObject Object;
    public int Count;
    public int floatCount;
    public float Intervaldistance;
    public SceneObj(GameObject _object,int _count,int _floatCount,float _intervaldistance)
    {
        this.Object = _object;
        this.Count = _count;
        this.floatCount = _floatCount;
        this.Intervaldistance = _intervaldistance;
    }
}
